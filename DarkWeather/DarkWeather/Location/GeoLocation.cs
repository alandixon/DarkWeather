using System;

namespace DarkWeather.Location
{
    public class GeoLocation
    {
        // Maximum allowed age is 15 minutes
        public readonly TimeSpan MaxAge = new TimeSpan(0, 15, 0);

        #region ctors

        /// <summary> Create invalid location </summary>
        public GeoLocation() :
            this(float.MinValue, float.MinValue)
        {
            IsValid = false;
        }

        public GeoLocation(float latitude, float longitude) :
            this(latitude, longitude, DateTime.Now)
        {
        }

        public GeoLocation(float latitude, float longitude, DateTime time) :
            this(latitude, longitude, time, string.Empty)
        {
        }

        public GeoLocation(float latitude, float longitude, DateTime time, string description)
        {
            Latitude = latitude;
            Longitude = longitude;
            Time = time;
            Description = description;
            IsValid = true;
        }

        #endregion ctors

        #region Properties

        public float Latitude { get; private set; }
        public float Longitude { get; private set; }
        public DateTime Time { get; private set; }
        public bool IsValid { get; private set; }

        private string description = string.Empty;
        public string Description
        {
            get
            {
                return description;
            }
            private set
            {
                // Location is stored as a CSV in the db and can't contain a comma
                description = value.Replace(',', ' ');
            }
        }

        
        public TimeSpan Age
        {
            get
            {
                if (!IsValid)
                {
                    return TimeSpan.MaxValue;
                }
                return DateTime.Now.Subtract(Time);
            }
        }

        #endregion Properties

        #region Methods

        public void SetInvalid()
        {
            IsValid = false;
        }

        public override string ToString()
        {
            if (!IsValid)
            {
                return string.Empty;
            }
            return string.Format("{0},{1},{2},{3}", Latitude, Longitude, Time.Ticks, Description);
        } 

        /// <summary> Try to parse a location string.
        /// Input should look like this:
        /// "LatitudeFloat, LongitudeFloat, TimeTicks"</summary>
        /// <param name="str"></param>
        /// <param name="location"></param>
        /// <param name="parseFailMessage"></param>
        /// <returns></returns>
        public static bool TryParse(string str, out GeoLocation location, out string parseFailMessage)
        {
            location = new GeoLocation(float.MinValue, float.MinValue, DateTime.MinValue);
            parseFailMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(str))
            {
                parseFailMessage = "No string found";
                return false;
            }

            string[] values = str.Split(new char[] { ',' });

            if (values.Length != 4)
            {
                parseFailMessage = string.Format("Couldn't find four comma-separated items in {0}", str);
                return false;
            }

            float latitude;
            if (!float.TryParse(values[0], out latitude))
            {
                parseFailMessage = string.Format("Couldn't parse the latitude: {0}", values[0]);
                return false;
            }

            float longitude;
            if (!float.TryParse(values[1], out longitude))
            {
                parseFailMessage = string.Format("Couldn't parse the longitude: {0}", values[1]);
                return false;
            }

            long ticks;
            if (!long.TryParse(values[2], out ticks))
            {
                parseFailMessage = string.Format("Couldn't parse the TimeTicks to a System.long: {0}", values[2]);
                return false;
            }

            DateTime dateTimeFromTicks;
            try
            {
                dateTimeFromTicks = new DateTime(ticks);
            }
            catch (ArgumentOutOfRangeException argoor)
            {
                parseFailMessage = string.Format("TimeTicks didn't parse to a sensible time value: {0}\n{1}", ticks, argoor.Message);
                return false;
            }            

            // The values[3] is the Description and doesn't need any parsing: just replace a null with an empty string
            location = new GeoLocation(latitude, longitude, dateTimeFromTicks, values[3] ?? string.Empty);
            return true;
        }
        
        #endregion Methods
    }
}
