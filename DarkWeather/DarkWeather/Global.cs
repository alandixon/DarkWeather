namespace DarkWeather
{

    public class Global
    {
        public const string ThisAppName = "DarkWeather";
        public const string DarkSky = "DarkSky";
        public const string ApiKey = "ApiKey";

        // Location delegate
        public delegate void LocationDelegate(float latitude, float longitude);

        // Location updates instantiation
        public static LocationDelegate updateLocation;

    }

}
