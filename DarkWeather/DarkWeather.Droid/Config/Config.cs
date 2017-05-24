using PCLAppConfig;
using System.Reflection;

namespace DarkWeather.Droid
{
    public class Config
    {
        private static Assembly assembly;

        static Config()
        {
            assembly = typeof(App).GetTypeInfo().Assembly;
            ConfigurationManager.Initialise(assembly.GetManifestResourceStream("DarkWeather.app.config"));
        }

        public static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}