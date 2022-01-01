using System;

namespace DarkWeather.Weather
{
    public abstract class AbsoluteTimeDataPoint
    {
        public DateTime Time { get; set; }
        public DateTime LocalTime { get; set; }
        public float PrecipitationIntensity { get; set; }
        public float PrecipitationProbability { get; set; }

        // According to DarkSky, this can be one of "rain", "snow", or "sleet" or undefined if precipIntensity is zero
        // See https://darksky.net/dev/docs/response#data-point
        public string PrecipitationType { get; set; }

        public AbsoluteTimeDataPoint() { }
        
    }
}
