using DarkSkyApi.Models;
using System;
using System.Collections.Generic;

namespace DarkWeather.Weather
{
    public class AbsoluteTimeDayDataPoint
    {
        public DateTime Time { get; set; }
        public DateTime LocalTime { get; set; }
        public float PrecipitationIntensity { get; set; }
        public float PrecipitationProbability { get; set; }

        // According to DarkSky, this can be one of "rain", "snow", or "sleet" or undefined if precipIntensity is zero
        // See https://darksky.net/dev/docs/response#data-point
        public string PrecipitationType { get; set; }

        public Sun Sun { get; set; }

        public DateTime SunriseTime { get; set; }
        public DateTime SunsetTime { get; set; }

        public AbsoluteTimeDayDataPoint() { }

        /// <summary> Convert offset time dayPoint list to absolute </summary>
        /// <param name="DayDataPoints"></param>
        /// <returns></returns>
        public static List<AbsoluteTimeDayDataPoint> ConvertFromOffsetTime(IList<DayDataPoint> DayDataPoints)
        {
            List<AbsoluteTimeDayDataPoint> AbsoluteTimeDayDataPoints = new List<AbsoluteTimeDayDataPoint>();
            foreach (var DayDataPoint in DayDataPoints)
            {
                AbsoluteTimeDayDataPoints.Add(ConvertFromOffsetTime(DayDataPoint));
            }
            return AbsoluteTimeDayDataPoints;
        }

        /// <summary> Convert offset time dayPoint to absolute </summary>
        /// <param name="dayDataPoint"></param>
        /// <returns></returns>
        public static AbsoluteTimeDayDataPoint ConvertFromOffsetTime(DayDataPoint dayDataPoint)
        {
            var AbsoluteTimeDayDataPoint = new AbsoluteTimeDayDataPoint()
            {
                Time = dayDataPoint.Time.DateTime,
                LocalTime = dayDataPoint.Time.DateTime.ToLocalTime(),
                PrecipitationIntensity = dayDataPoint.PrecipitationIntensity,
                PrecipitationProbability = dayDataPoint.PrecipitationProbability,
                PrecipitationType = dayDataPoint.PrecipitationType,
                SunriseTime = dayDataPoint.SunriseTime.DateTime,
                SunsetTime = dayDataPoint.SunsetTime.DateTime
            };
            return AbsoluteTimeDayDataPoint;
        }

    }
}
