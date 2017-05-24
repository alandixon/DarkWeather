using Xamarin.Forms;

namespace DarkWeather.Secure
{
    public class SecureProperties
    {
        // https://xamarinhelp.com/xamarin-forms-dependency-injection/
        private static ISecure secure = DependencyService.Get<ISecure>();

        /// <summary> Call into Android to save a secure property</summary>
        /// <param name="apiKey"></param>
        public static void SaveApiKey(string apiKey)
        {
            secure.SaveProperty(Global.DarkSky, Global.ThisAppName, Global.ApiKey, apiKey);
        }

        /// <summary> Call into Android to get a secure property</summary>
        /// <returns> the secure value </returns>
        public static string GetApiKey()
        {
            return secure.GetProperty(Global.ApiKey, Global.ThisAppName);
        }

    }
}
