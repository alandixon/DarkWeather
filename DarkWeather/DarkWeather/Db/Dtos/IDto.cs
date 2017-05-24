using SQLite;

namespace DarkWeather.Db
{
    /// <summary> Interface for Data Transfer Objects  </summary>
    public interface IDto
    {
        [PrimaryKey, AutoIncrement, Column("Id")]
        int Id { get; set; }
    }
}
