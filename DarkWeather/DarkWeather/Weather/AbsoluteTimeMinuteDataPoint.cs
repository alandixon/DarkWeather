using DarkSkyApi.Models;
using System;
using System.Collections.Generic;

namespace DarkWeather.Weather
{
    public class AbsoluteTimeMinuteDataPoint
    {
        public DateTime Time { get; set; }
        public float PrecipitationIntensity { get; set; }
        public float PrecipitationProbability { get; set; }

        // According to DarkSky, this can be one of "rain", "snow", or "sleet" or undefined if precipIntensity is zero
        // See https://darksky.net/dev/docs/response#data-point
        public string PrecipitationType { get; set; }

        public AbsoluteTimeMinuteDataPoint() { }

        /// <summary> Convert offset time minutepoint list to absolute </summary>
        /// <param name="minuteDataPoints"></param>
        /// <returns></returns>
        public static List<AbsoluteTimeMinuteDataPoint> ConvertFromOffsetTime(IList<MinuteDataPoint> minuteDataPoints)
        {
            List<AbsoluteTimeMinuteDataPoint> absoluteTimeMinuteDataPoints = new List<AbsoluteTimeMinuteDataPoint>();
            foreach (var minuteDataPoint in minuteDataPoints)
            {
                absoluteTimeMinuteDataPoints.Add(ConvertFromOffsetTime(minuteDataPoint));
            }
            return absoluteTimeMinuteDataPoints;
        }

        /// <summary> Convert offset time minutepoint to absolute </summary>
        /// <param name="minuteDataPoint"></param>
        /// <returns></returns>
        public static AbsoluteTimeMinuteDataPoint ConvertFromOffsetTime(MinuteDataPoint minuteDataPoint)
        {
            var absoluteTimeMinuteDataPoint = new AbsoluteTimeMinuteDataPoint()
            {
                Time = minuteDataPoint.Time.DateTime,
                PrecipitationIntensity = minuteDataPoint.PrecipitationIntensity,
                PrecipitationProbability = minuteDataPoint.PrecipitationProbability,
                PrecipitationType = minuteDataPoint.PrecipitationType
            };
            return absoluteTimeMinuteDataPoint;
        }

    }
}
