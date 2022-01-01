using DarkWeather.Logging;
using System;
using Xamarin.Forms;

namespace DarkWeather
{
    public partial class HomePage : ContentPage
    {
        private ILog Log = DependencyService.Get<ILog>();
        private string logTag = typeof(HomePage).FullName;

        private Model model;

        public HomePage()
        {
            InitializeComponent();

            Global.Model = new Model();
            BindingContext = Global.Model;

            Global.HomePage = this;
            Global.AppSettingsPage = new AppSettingsPage();
            Global.FortyEightHourPage = new FortyEightHourPage();

            Log.Debug(logTag, "HomePage started", true);

        }

        private void AppSettings_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(Global.AppSettingsPage);
        }

        private void FortyEightHour_Clicked(object sender, EventArgs e)
        {
            App.RootPage.Detail = App.FortyEightHourNavigationPage;
        }

    }
}
