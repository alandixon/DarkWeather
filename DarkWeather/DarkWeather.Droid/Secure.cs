using System.Linq;
using Xamarin.Auth;
using Xamarin.Forms;

// https://xamarinhelp.com/xamarin-forms-dependency-injection/
[assembly: Xamarin.Forms.Dependency(typeof(DarkWeather.Droid.Secure))]
namespace DarkWeather.Droid
{
    // Android can secure account credentials using the Xamarin.Auth.AccountStore
    //  This can hold multiple account pairs per AppName
    // Each Account has three parts: AppName, UserName, (e.g.) Password
    // https://developer.xamarin.com/recipes/cross-platform/xamarin-forms/general/store-credentials/
    // https://components.xamarin.com/gettingstarted/xamarin.auth
    public class Secure : DarkWeather.Secure.ISecure
    {
        public Secure()
        {
        }

        /// <summary> Does an account exist for this appName? </summary>
        /// <param name="appName"></param>
        /// <returns></returns>
        public bool AppAccountExists(string appName)
        {
            return AccountStore.Create(Forms.Context).FindAccountsForService(appName).Any() ? true : false;
        }

        /// <summary> Save property for given appName. Deletes the account (with ALL properties) if it already exists </summary>
        /// <param name="appName"></param>
        /// <param name="userName"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        public void SaveProperty(string appName, string userName, string propertyName, string propertyValue)
        {
            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(propertyValue))
            {
                // Remove any existing account with the same property
                string existing = GetProperty(propertyName, appName);
                if (!string.IsNullOrWhiteSpace(existing))
                {
                    DeleteAccount(appName);
                }
                // Create a new one
                Account account = new Account
                {
                    Username = userName
                };
                account.Properties.Add(propertyName, propertyValue);
                AccountStore.Create(Forms.Context).Save(account, Global.ThisAppName);
            }
        }

        /// <summary> Get property value for given appName </summary>
        /// <param name="propertyName"></param>
        /// <param name="appName"></param>
        /// <returns></returns>
        public string GetProperty(string propertyName, string appName)
        {
            var account = AccountStore.Create(Forms.Context).FindAccountsForService(appName).FirstOrDefault();
            return account?.Properties[propertyName];
        }

        /// <summary> Remove the account for a given appName </summary>
        /// <param name="appName"></param>
        public void DeleteAccount(string appName)
        {
            var account = AccountStore.Create(Forms.Context).FindAccountsForService(appName).FirstOrDefault();
            if (account != null)
            {
                AccountStore.Create(Forms.Context).Delete(account, appName);
            }
        }

    }
}
