using DarkWeather.Weather;
using SQLite;

namespace DarkWeather.Db
{
    public class DatumDto : Datum, IDto
    {
        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }
    }
}
