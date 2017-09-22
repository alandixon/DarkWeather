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

    [DebuggerDisplay("SunState={SunState.ToString()} {SunPercent}%")]
    public class Sun
    {
        public SunState SunState { get; set; }

        public int SunPercent { get; set; }
    }
}
