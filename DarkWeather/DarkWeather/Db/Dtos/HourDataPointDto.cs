using DarkSkyApi.Models;
using SQLite;

namespace DarkWeather.Db
{
    public class HourDataPointDto : HourDataPoint, IDto
    {
        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }
    }
}
