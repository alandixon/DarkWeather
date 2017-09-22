using DarkSkyApi.Models;
using System.Collections.Generic;

namespace DarkWeather.Weather
{
    public class AbsoluteTimeMinuteDataPoint : AbsoluteTimeDataPoint
    {
        public Sun Sun { get; set; }

        public AbsoluteTimeMinuteDataPoint()
        {
            Sun = new Sun();
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
