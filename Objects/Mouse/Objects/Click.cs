using OBS;
using obs_cli.Data;
using obs_cli.Objects.Obs;
using System;
using System.Drawing;
using System.Timers;

namespace obs_cli.Objects.Mouse.Objects
{
    public class Click
    {
        public Guid Id;
        private int _timerDuration = 225;
        private Timer _highlightTimer;

        private Source source_click_highlight;
        private Item item_click_highlight;
        private string _highlightImagePath = $"{AppDomain.CurrentDomain.BaseDirectory}/Media/cursor_highlight_48x48.png";
        private int _highlightImageOffset;

        public bool IsLeftButtonDown = true;

        public Click(int x, int y, int screenBoundsHeight, int screenBoundsWidth, Action<Guid> removeCallback)
        {
            Id = Guid.NewGuid();
            _highlightImageOffset = Image.FromFile(_highlightImagePath).Width / 2;
            _highlightTimer = new Timer();
            _highlightTimer.Interval = _timerDuration;
            _highlightTimer.Elapsed += new ElapsedEventHandler(HideClickHighlightHandler);
            _highlightTimer.Elapsed += delegate { removeCallback(Id); };

            source_click_highlight = Store.Data.Obs.Presentation.CreateSource(SourceType.FFMPEG_SOURCE, "Click highlight", CreateClickHighlightSettings());
            Store.Data.Obs.Presentation.AddSource(source_click_highlight);

            item_click_highlight = Store.Data.Obs.Presentation.CreateItem(source_click_highlight);
            item_click_highlight.SetBounds(new Vector2(screenBoundsWidth, screenBoundsHeight), ObsBoundsType.None, ObsAlignment.Top);
            Store.Data.Obs.MainScene.Items.Add(item_click_highlight);

            SetHighlightPosition(x, y);
        }

        /// <summary>
        /// Sets the position of the click highlight.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetHighlightPosition(int x, int y)
        {
            item_click_highlight.Position = new Vector2(x - _highlightImageOffset, y - _highlightImageOffset);
            ResetTimer();
        }

        /// <summary>
        /// Stop the click's timer.
        /// </summary>
        private void StopTimer()
        {
            _highlightTimer.Stop();
        }

        /// <summary>
        /// Reset the click's timer.
        /// </summary>
        private void ResetTimer()
        {
            _highlightTimer.Stop();
            _highlightTimer.Start();
        }

        /// <summary>
        /// Hides the click highlight.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void HideClickHighlightHandler(object source, ElapsedEventArgs e)
        {
            if (IsLeftButtonDown)
                return;

            source_click_highlight.Remove();
            source_click_highlight.Dispose();
            source_click_highlight = null;

            item_click_highlight.Remove();
            item_click_highlight.Dispose();
            item_click_highlight = null;

            StopTimer();
        }

        /// <summary>
        /// Creates the click highlight settings.
        /// </summary>
        /// <returns></returns>
        private ObsData CreateClickHighlightSettings()
        {
            ObsData settings = new ObsData();
            settings.SetBool(MediaSource.IS_LOCAL_FILE, true);
            settings.SetBool(MediaSource.LOOPING, true);
            settings.SetBool(MediaSource.RESTART_ON_ACTIVATE, true);
            settings.SetBool(MediaSource.HW_DECODE, true);
            settings.SetString(MediaSource.LOCAL_FILE_PATH, _highlightImagePath);
            settings.SetBool(Common.ACTIVATE, true);
            return settings;
        }
    }
}
