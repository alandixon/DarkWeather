using DarkSkyApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DarkWeather.Weather
{
    [DebuggerDisplay("UTC={Time.ToString(\"ddd dd HH:mm:ss\")} Local={LocalTime.ToString(\"ddd dd HH:mm:ss\")} Precip Int/Prob={PrecipitationIntensity}/{PrecipitationProbability}")]
    public class AbsoluteTimeMinuteDataPoint : IDayCycle
    {
        public DateTime Time { get; set; }
        public DateTime LocalTime { get; set; }
        public float PrecipitationIntensity { get; set; }
        public float PrecipitationProbability { get; set; }

        // According to DarkSky, this can be one of "rain", "snow", or "sleet" or undefined if precipIntensity is zero
        // See https://darksky.net/dev/docs/response#data-point
        public string PrecipitationType { get; set; }

        public DayCycle DayCycle { get; set; }

        public AbsoluteTimeMinuteDataPoint()
        {
            DayCycle = new DayCycle();
        }

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
                LocalTime = minuteDataPoint.Time.DateTime.ToLocalTime(),
                PrecipitationIntensity = minuteDataPoint.PrecipitationIntensity,
                PrecipitationProbability = minuteDataPoint.PrecipitationProbability,
                PrecipitationType = minuteDataPoint.PrecipitationType
            };
            return absoluteTimeMinuteDataPoint;
        }

    }
}
