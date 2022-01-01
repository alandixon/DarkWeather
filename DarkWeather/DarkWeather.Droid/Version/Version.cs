// https://xamarinhelp.com/xamarin-forms-dependency-injection/
using Android.App;
using DarkWeather.Version;

[assembly: Xamarin.Forms.Dependency(typeof(DarkWeather.Droid.Version.Version))]
namespace DarkWeather.Droid.Version
{

    public class Version : IVersion
    {
        string IVersion.VersionName {
            get
            {
                return Application.Context.ApplicationContext.PackageManager.GetPackageInfo(Application.Context.ApplicationContext.PackageName, 0).VersionName;
            }
        }
    }
}