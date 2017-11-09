using System.ComponentModel;
using System.Diagnostics;

namespace DarkWeather.Weather
{
    public enum SunState
    {
        Undefined=0,
        PreSunrise,
        SunUp,
        PostSunset
    }

    [DebuggerDisplay("SunState={SunState.ToString()} SunRatio={SunRatio}")]
    public class DayCycle : INotifyPropertyChanged
    {
        private SunState sunState;
        public SunState SunState
        { 
            get
            {
                return sunState;
            }
            set
            {
                sunState = value;
                NotifyPropertyChanged("SunState");
}
        }

        private float sunRatio;
        public float SunRatio
        {
            get
            {
                return sunRatio;
            }
            set
            {
                sunRatio = value;
                NotifyPropertyChanged("SunRatio");
                DarkRatio = 1 - sunRatio;
            }
        }

        private float darkRatio;
        public float DarkRatio
        {
            get
            {
                return darkRatio;
            }
            set
            {
                darkRatio = value;
                NotifyPropertyChanged("DarkRatio");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

    }
}
