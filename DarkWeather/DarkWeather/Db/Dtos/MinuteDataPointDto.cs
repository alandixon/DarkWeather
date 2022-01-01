using DarkSkyApi.Models;
using SQLite;

namespace DarkWeather.Db
{
    public class MinuteDataPointDto : MinuteDataPoint, IDto
    {
        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }
    }
}
