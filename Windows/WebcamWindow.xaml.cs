using OBS;
using obs_cli.Behaviors;
using obs_cli.Controls;
using obs_cli.Data;
using obs_cli.Objects;
using obs_cli.Services;
using obs_cli.Utility;
using System;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using SystemTimer = System.Timers.Timer;
using WebcamDevice = obs_cli.Objects.Webcam;
using WindowsFormsPanel = System.Windows.Forms.Panel;

namespace obs_cli.Windows
{
    /// <summary>
    /// Interaction logic for WebcamWindow.xaml
    /// </summary>
    public partial class WebcamWindow : Window
    {
        public const double WEBCAM_ONLY_WIDTH = 720;
        public const double WEBCAM_ONLY_HEIGHT = 405;

        public WebcamDevice selectedWebcam;
        private WebcamResolution selectedWebcamResolution;
        private ItemPreviewPanel itemPreviewPanel;

        private SystemTimer changeResolutionTimer;
        private int elapsedTimerTime;
        private int webcamWidthBeforeChange;
        private int webcamHeightBeforeChange;

        private WindowsFormsPanel wfPanel;
        private WindowsFormsHostOverlay windowsFormsHostOverlay;

        private bool aligningWebcamWithControlBar;

        public double BorderSize => DpiUtil.GetNearestMultipleSize(this, resizeBorder.Padding.Left);

        public WebcamWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            EnumerateAndSetWebcams();

            // todo: bring this method over, but deal with it when I deal with attaching audio to webcam
            //Recorder.ScreenRecorder.Webcam_UpdateAudioDevice();
        }

        public WebcamWindow(double? width, double? height)
            : this()
        {
            if (width.HasValue)
                Width = width.Value;

            if (height.HasValue)
                Height = height.Value;
        }

        public void Show(double? width, double? height)
        {
            if (width.HasValue)
            {
                Width = width.Value;
                if (height.HasValue)
                {
                    Height = height.Value;
                }
                else
                {
                    Height = width.Value / (16.0 / 9.0);
                }
            }

            base.Show();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();

            wfPanel?.Controls.Remove(itemPreviewPanel);
            itemPreviewPanel?.Dispose();
            itemPreviewPanel = null;
            windowsFormsHostOverlay?.Close();
            windowsFormsHostOverlay = null;

            Store.Data.Webcam.DestroyObsWebcam();
        }

        private void InitializeWebcamPreview()
        {
            if (itemPreviewPanel != null) return;

            itemPreviewPanel = new ItemPreviewPanel(Store.Data.Webcam.Item, Store.Data.Webcam.Source);
            itemPreviewPanel.Dock = DockStyle.Fill;

            wfPanel = new WindowsFormsPanel();
            windowsFormsHostOverlay = new WindowsFormsHostOverlay(bTarget, wfPanel);
            windowsFormsHostOverlay.Title = "Webcam";

            wfPanel.Controls.Add(itemPreviewPanel);
            DisplayPanelMoveBehavior.Attach(itemPreviewPanel, this);
        }

        public void SetWebcam(WebcamDevice webcam)
        {
            Store.Data.Webcam.ActiveWebcamValue = webcam.value;
            selectedWebcam = webcam;
            SetWebcamSourceSettings();
        }

        private ObsData CreateWebcamSettings()
        {
            ObsData webcamSettings = new ObsData();

            webcamSettings.SetString(VideoCapture.VIDEO_DEVICE_ID, selectedWebcam.value);

            int buffering = 2;
            webcamSettings.SetInt(VideoCapture.BUFFERING, buffering); // 0 = Auto, 1 = Enable, 2 = Disable

            if (selectedWebcamResolution != null)
            {
                webcamSettings.SetString(VideoCapture.RESOLUTION, selectedWebcamResolution.value);
                webcamSettings.SetInt(VideoCapture.RESOLUTION_TYPE, 1);
            }
            else
            {
                var preferredResolution = WebcamService.GetOptimalWebcamResolution(selectedWebcam.dsDeviceValue);

                if (!string.IsNullOrEmpty(preferredResolution))
                {
                    webcamSettings.SetString(VideoCapture.RESOLUTION, preferredResolution);
                    webcamSettings.SetInt(VideoCapture.RESOLUTION_TYPE, 1);
                }
                else
                {
                    webcamSettings.Erase(VideoCapture.RESOLUTION);
                    webcamSettings.SetInt(VideoCapture.RESOLUTION_TYPE, 0);
                }
            }

            webcamSettings.SetBool(VideoCapture.ACTIVATE, true);

            return webcamSettings;
        }

        private void SetWebcamSourceSettings()
        {
            // Start a timer to see when the webcam resolution has changed
            webcamWidthBeforeChange = (int)Store.Data.Webcam.Source.Width;
            webcamHeightBeforeChange = (int)Store.Data.Webcam.Source.Height;

            ObsData webcamSettings = CreateWebcamSettings();

            Store.Data.Webcam.Source.Update(webcamSettings);
            webcamSettings.Dispose();

            InitializeWebcamPreview();

            changeResolutionTimer?.Stop();
            changeResolutionTimer = new SystemTimer();
            changeResolutionTimer.Interval = 100;
            changeResolutionTimer.Elapsed += new ElapsedEventHandler(CheckIfWebcamResolutionChanged);
            changeResolutionTimer.Enabled = true;
            changeResolutionTimer.Start();
        }

        private void CheckIfWebcamResolutionChanged(object sender, ElapsedEventArgs e)
        {
            if (Store.Data.Webcam.Source == null)
            {
                changeResolutionTimer?.Stop();
                changeResolutionTimer = null;
                return;
            }

            elapsedTimerTime += (int)changeResolutionTimer.Interval;

            if (webcamWidthBeforeChange != Store.Data.Webcam.Source.Width ||
                webcamHeightBeforeChange != Store.Data.Webcam.Source.Height)
            {
                changeResolutionTimer?.Stop();
                changeResolutionTimer = null;

                Store.Data.Webcam.CalculateItemPosition();
            }
            else if (elapsedTimerTime % 1000 == 0)
            {
                Store.Data.Webcam.CalculateItemPosition();
            }

            if (elapsedTimerTime >= 5000)
            {
                // If it doesn't work in 5 seconds, stop trying
                changeResolutionTimer?.Stop();
                changeResolutionTimer = null;
            }
        }

        private void WebcamWindow_OnLocationChanged(object sender, EventArgs e)
        {
            if (aligningWebcamWithControlBar) return;
            MouseLeftButtonDownBehavior();
        }

        private static void MouseLeftButtonDownBehavior()
        {
            if (Store.Data.Webcam.Item != null)
            {
                // When moving the webcam, put it outside the bounds of the recording and 
                // show the preview instead this avoids a window lag when dragging. 
                // Can't set visibility to false cause then the audio will cut out as well.
                Store.Data.Webcam.SetWebcamItemOffscreen();
            }
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MouseButtonUpBehavior();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MouseButtonUpBehavior();
        }

        private void WebcamWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            HwndSource source = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            source.AddHook(new HwndSourceHook(WndProc));
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == (int)Constants.WindowMessage.WM_EXITSIZEMOVE)
            {
                MouseButtonUpBehavior();
            }

            return IntPtr.Zero;
        }

        private void MouseButtonUpBehavior()
        {
            if (Store.Data.Webcam.Item != null)
            {
                Store.Data.Webcam.CalculateItemPosition();
            }

            ResizeMode = ResizeMode.CanResize; // Re-enable resizing on this window. Resizing disabled when mouse is dragged o nthe winform child (in DisplayPanelMoveBehavior)
        }

        public void EnumerateAndSetWebcams()
        {
            Store.Data.Webcam.Webcams.Clear();

            InitializeWebcamObsSource(null);

            ObsProperties webcamProperties = Store.Data.Webcam.Source.GetProperties();
            ObsProperty[] webcamPropertyList = webcamProperties.GetPropertyList();

            for (int i = 0; i < webcamPropertyList.Length; i++)
            {
                if (webcamPropertyList[i].Name.Equals("video_device_id"))
                {
                    string[] propertyNames = webcamPropertyList[i].GetListItemNames();
                    object[] propertyValues = webcamPropertyList[i].GetListItemValues();

                    for (int j = 0; j < propertyNames.Length; j++)
                    {
                        Store.Data.Webcam.Webcams.Add(new WebcamDevice(propertyNames[j], (string)propertyValues[j]));
                    }
                }
            }
        }

        private void InitializeWebcamObsSource(ObsData webcamSettings)
        {
            if (Store.Data.Webcam.Source == null)
            {
                if (webcamSettings == null)
                {
                    Store.Data.Webcam.Source = Store.Data.Obs.Presentation.CreateSource("dshow_input", "Webcam");
                }
                else
                {
                    Store.Data.Webcam.Source = Store.Data.Obs.Presentation.CreateSource("dshow_input", "Webcam", webcamSettings);
                }

                Store.Data.Webcam.Source.AudioOffset = Constants.Audio.DELAY_INPUT_ATTACHED_TO_WEBCAM;
                Store.Data.Obs.Presentation.AddSource(Store.Data.Webcam.Source);
            }

            if (Store.Data.Webcam.Item == null)
            {
                Store.Data.Webcam.Item = Store.Data.Obs.Presentation.CreateItem(Store.Data.Webcam.Source);
                Store.Data.Webcam.Item.Name = "Webcam SceneItem";
                Store.Data.Webcam.Item.SetBounds(new Vector2((float)(Width - (BorderSize * 2)), (float)(Height - (BorderSize * 2))), ObsBoundsType.ScaleOuter, ObsAlignment.Center);
                Store.Data.Obs.MainScene.Items.Add(Store.Data.Webcam.Item);
            }
        }
    }
}
