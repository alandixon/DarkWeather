using DarkSkyApi.Models;
using System.Collections.Generic;

namespace DarkWeather.Weather
{
    public class AbsoluteTimeHourDataPoint : AbsoluteTimeDataPoint
    {
        public static readonly int FortyEight = 48;

        public float TemperatureF { get; set; }
        public float TemperatureC { get; set; }
        public float ApparentTemperatureF { get; set; }
        public float ApparentTemperatureC { get; set; }
        public float CloudCover { get; set; }

        public Sun Sun { get; set; }

        //According to DarkSky, this can be one of "rain", "snow", or "sleet" or undefined if precipIntensity is zero
        //See https://darksky.net/dev/docs/response#data-point
        public string PrecipitationType { get; set; }

        public AbsoluteTimeHourDataPoint()
        {
            Sun = new Sun();
        }

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
                LocalTime = hourDataPoint.Time.DateTime.ToLocalTime(),
                PrecipitationIntensity = hourDataPoint.PrecipitationIntensity,
                PrecipitationProbability = hourDataPoint.PrecipitationProbability,
                PrecipitationType = hourDataPoint.PrecipitationType,
                TemperatureF = hourDataPoint.Temperature,
                TemperatureC = (hourDataPoint.Temperature - 32) * 5 / 9,
                ApparentTemperatureF = hourDataPoint.ApparentTemperature,
                ApparentTemperatureC = (hourDataPoint.ApparentTemperature - 32) * 5 / 9,
                CloudCover = hourDataPoint.CloudCover
            };
            return absoluteTimeHourDataPoint;
        }

    }
}
