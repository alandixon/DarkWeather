using DarkWeather.Logging;
using System;
using Xamarin.Forms;

namespace DarkWeather
{
    public partial class FortyEightHourPage : ContentPage
    {
        private ILog Log = DependencyService.Get<ILog>();
        private string logTag = typeof(FortyEightHourPage).FullName;

        public FortyEightHourPage()
        {
            InitializeComponent();
            App.FortyEightHourPageVM = new FortyEightHourPageVM(App.Model);
            BindingContext = App.FortyEightHourPageVM;

            Log.Debug(logTag, "FortyEightHourPage started", true);

        }

        private void AppSettings_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(App.AppSettingsPage);
        }

        private void OneHour_Clicked(object sender, EventArgs e)
        {
            App.RootPage.Master = App.HomeMenuPage;
            App.RootPage.Detail = App.HomeNavigationPage;
        }
    }
}
