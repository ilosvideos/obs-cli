using System;
using System.ComponentModel;

namespace obs_cli.Objects
{
    public class WebcamResolution : INotifyPropertyChanged
    {
        public string displayValue { get; }
        public string value { get; }

        public WebcamResolution(string displayValue, string value)
        {
            this.displayValue = displayValue;
            this.value = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
