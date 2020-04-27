using OBS;
using obs_cli.Data;
using obs_cli.Utility;
using System.Windows;
using WebcamDevice = obs_cli.Objects.Webcam;

namespace obs_cli.Windows
{
    /// <summary>
    /// Interaction logic for WebcamWindow.xaml
    /// </summary>
    public partial class WebcamWindow : Window
    {
        public WebcamWindow()
        {
            InitializeComponent();
            enum_webcams();
        }

        private void enum_webcams()
        {
            Store.Data.Webcam.Webcams.Clear();

            init_webcam_source(null);

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

        private void init_webcam_source(ObsData webcamSettings)
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
                // todo: bring over the BorderSize
                //Store.Data.Webcam.Item.SetBounds(new Vector2((float)(Width - (BorderSize * 2)), (float)(Height - (BorderSize * 2))), ObsBoundsType.ScaleOuter, ObsAlignment.Center);
                Store.Data.Webcam.Item.SetBounds(new Vector2((float)(Width - (4 * 2)), (float)(Height - (4 * 2))), ObsBoundsType.ScaleOuter, ObsAlignment.Center);
                Store.Data.Obs.MainScene.Items.Add(Store.Data.Webcam.Item);
            }
        }
    }
}
