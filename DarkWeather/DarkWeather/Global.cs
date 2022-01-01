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

        public static OneHourPage OneHourPage { get; set; }
        public static AppSettingsPage AppSettingsPage { get; set; }
        public static FortyEightHourPage FortyEightHourPage { get; set; }

        public static Model Model { get; set; }

    }

}
