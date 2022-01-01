using DarkSkyApi.Models;
using System;
using System.Collections.Generic;

namespace DarkWeather.Weather
{
    public class AbsoluteTimeHourDataPoint
    {
        public DateTime Time { get; set; }
        public float PrecipitationIntensity { get; set; }
        public float PrecipitationProbability { get; set; }

        // According to DarkSky, this can be one of "rain", "snow", or "sleet" or undefined if precipIntensity is zero
        // See https://darksky.net/dev/docs/response#data-point
        public string PrecipitationType { get; set; }

        public AbsoluteTimeHourDataPoint() { }

        /// <summary> Convert offset time hourpoint list to absolute </summary>
        /// <param name="hourDataPoints"></param>
        /// <returns></returns>
        public static List<AbsoluteTimeHourDataPoint> ConvertFromOffsetTime(IList<HourDataPoint> hourDataPoints)
        {
            List<AbsoluteTimeHourDataPoint> absoluteTimeHourDataPoints = new List<AbsoluteTimeHourDataPoint>();
            foreach (var hourDataPoint in hourDataPoints)
            {
                absoluteTimeHourDataPoints.Add(ConvertFromOffsetTime(hourDataPoint));
            }
            return absoluteTimeHourDataPoints;
        }

        /// <summary> Convert offset time hourpoint to absolute </summary>
        /// <param name="hourDataPoint"></param>
        /// <returns></returns>
        public static AbsoluteTimeHourDataPoint ConvertFromOffsetTime(HourDataPoint hourDataPoint)
        {
            var absoluteTimeHourDataPoint = new AbsoluteTimeHourDataPoint()
            {
                Time = hourDataPoint.Time.DateTime,
                PrecipitationIntensity = hourDataPoint.PrecipitationIntensity,
                PrecipitationProbability = hourDataPoint.PrecipitationProbability,
                PrecipitationType = hourDataPoint.PrecipitationType
            };
            return absoluteTimeHourDataPoint;
        }

    }
}
