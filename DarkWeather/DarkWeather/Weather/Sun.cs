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
    public class Sun
    {
        public SunState SunState { get; set; }

        /// <summary> 
        /// 
        /// </summary>
        public float SunRatio { get; set; }
    }
}
