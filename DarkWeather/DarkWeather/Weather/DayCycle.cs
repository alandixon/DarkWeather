using System;
using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Forms;

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
    public class DayCycle : BindableObject, INotifyPropertyChanged
    {

        #region Attached properties

        public static readonly BindableProperty SunStateProperty = BindableProperty.CreateAttached("SunState", typeof(SunState), typeof(DayCycle), SunState.Undefined);

        public SunState SunState
        {
            get
            {
                return GetSunState(this);
            }
            set
            {
                if (SunState == value) return;
                SetSunState(this, value);
                OnPropertyChanged("SunState");
            }
        }

        public static SunState GetSunState(BindableObject target)
        {
            return (SunState)target.GetValue(SunStateProperty);
        }

        public static void SetSunState(BindableObject target, SunState value)
        {
            target.SetValue(SunStateProperty, value);
        }


        public static readonly BindableProperty SunRatioProperty = BindableProperty.CreateAttached("SunRatio", typeof(float), typeof(DayCycle),  (float)0.0);

        public float SunRatio
        {
            get
            {
                return GetSunRatio(this);
            }
            set
            {
                DarkRatio = 1 - value;
                if (SunRatio == value) return;
                SunRatioChanged?.Invoke(value); // new
                SetSunRatio(this, value);
                OnPropertyChanged("SunRatio");
            }
        }

        public event Action<float> SunRatioChanged;  // new

        public static float GetSunRatio(BindableObject target)
        {
            return (float)target.GetValue(SunRatioProperty);
        }

        public static void SetSunRatio(BindableObject target, float value)
        {
            target.SetValue(SunRatioProperty, value);
        }


        public static readonly BindableProperty DarkRatioProperty = BindableProperty.CreateAttached("DarkRatio", typeof(float), typeof(DayCycle), (float)0.0);

        public float DarkRatio
        {
            get
            {
                return GetDarkRatio(this);
            }
            set
            {
                if (DarkRatio == value) return;
                DarkRatioChanged?.Invoke(value); // new
                SetDarkRatio(this, value);
                OnPropertyChanged("DarkRatio");
            }
        }

        public event Action<float> DarkRatioChanged;  // new

        public static float GetDarkRatio(BindableObject target)
        {
            return (float)target.GetValue(DarkRatioProperty);
        }

        public static void SetDarkRatio(BindableObject target, float value)
        {
            target.SetValue(DarkRatioProperty, value);
        }

        #endregion Attached properties

    }
}
