using DarkSkyApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DarkWeather.Weather
{
    [DebuggerDisplay("UTC={Time.ToString(\"ddd dd HH:mm:ss\")} Local={LocalTime.ToString(\"ddd dd HH:mm:ss\")} Precip Int/Prob={PrecipitationIntensity}/{PrecipitationProbability}")]
    public class AbsoluteTimeHourDataPoint : IDayCycle
    {
        public static readonly int FortyEight = 48;

        public DateTime Time { get; set; }
        public DateTime LocalTime { get; set; }
        public float PrecipitationIntensity { get; set; }
        public float PrecipitationAccumulation { get; set; }
        public float PrecipitationProbability { get; set; }

        // According to DarkSky, this can be one of "rain", "snow", or "sleet" or undefined if precipIntensity is zero
        // See https://darksky.net/dev/docs/response#data-point
        public string PrecipitationType { get; set; }

        public float TemperatureF { get; set; }
        public float TemperatureC { get; set; }
        public float ApparentTemperatureF { get; set; }
        public float ApparentTemperatureC { get; set; }
        public float CloudCover { get; set; }
        public float WindSpeed { get; set; }
        public float SunRatio { get; set; }
        public float DarkRatio { get; set; }

        public DayCycle DayCycle { get; set; }

        public AbsoluteTimeHourDataPoint()
        {
            DayCycle = new DayCycle();
            DayCycle.SunRatioChanged += DayCycle_SunRatioChanged;
            DayCycle.DarkRatioChanged += DayCycle_DarkRatioChanged;
        }

        private void DayCycle_SunRatioChanged(float ratio)
        {
            SunRatio = ratio;
        }

        private void DayCycle_DarkRatioChanged(float ratio)
        {
            DarkRatio = ratio;
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
                CloudCover = hourDataPoint.CloudCover,
                WindSpeed = hourDataPoint.WindSpeed
            };
            return absoluteTimeHourDataPoint;
        }

    }
}
