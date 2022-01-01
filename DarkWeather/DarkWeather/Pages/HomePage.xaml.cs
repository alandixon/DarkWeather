using DarkWeather.Logging;
using System;
using Xamarin.Forms;

namespace DarkWeather
{
    public partial class HomePage : ContentPage
    {
        private ILog Log = DependencyService.Get<ILog>();
        private string logTag = typeof(HomePage).FullName;

        public HomePage()
        {
            InitializeComponent();
            App.HomePageVM = new HomePageVM(App.Model);
            BindingContext = App.HomePageVM;

            Log.Debug(logTag, "HomePage started", true);
        }

        private void AppSettings_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(App.AppSettingsPage);
        }

        private void FortyEightHour_Clicked(object sender, EventArgs e)
        {
            App.RootPage.Master = App.FortyEightHourMenuPage;
            App.RootPage.Detail = App.FortyEightHourNavigationPage;        }

    }
}
