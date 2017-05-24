using SQLite;
using System.Collections.Generic;

namespace DarkWeather.Db
{
    public class KeyValuePairDto : IDto
    {
        public readonly int MaxValueLength = 64;

        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }

        public KeyValuePairDto()
        {
        }

        public string Key { get; set; }

        private string val;
        public string Value
        {
            get
            {
                return val;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && value.Length > MaxValueLength)
                {
                    val = value.Substring(0, MaxValueLength);
                }
                else
                {
                    val = value;
                }
            }
        }
    }
}
