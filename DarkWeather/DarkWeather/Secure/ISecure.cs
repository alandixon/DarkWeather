namespace DarkWeather.Secure
{
    public interface ISecure
    {
        /// <summary> Save property to the Xamarin.Auth.AccountStore </summary>
        /// <param name="appName"></param>
        /// <param name="userName"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        void SaveProperty(string appName, string userName, string propertyName, string propertyValue);

        /// <summary>Get property from the Xamarin.Auth.AccountStore </summary>
        /// <param name="propertyName"></param>
        /// <param name="appName"></param>
        /// <returns></returns>
        string GetProperty(string propertyName, string appName);
    }
}
