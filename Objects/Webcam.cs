using System.ComponentModel;

namespace obs_cli.Objects
{
    public class Webcam : INotifyPropertyChanged
    {
        public string name { get; }
        public string value { get; }
        public string dsDeviceValue { get; }

        public Webcam(string name, string value)
        {
            this.name = name;
            this.value = value;
            this.dsDeviceValue = value;

            if (!string.IsNullOrEmpty(value))
            {
                // todo: not quite sure what #22 represents yet. something OBS adds in?
                var index = value.IndexOf(":\\");
                dsDeviceValue = index >= 0 ? value.Substring(value.IndexOf(":\\")).Replace("#22", "#") : value;;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
