using DarkWeather.Logging;
using System;
using Xamarin.Forms;

namespace DarkWeather
{
    public partial class OneHourPage : ContentPage
    {
        private ILog Log = DependencyService.Get<ILog>();
        private string logTag = typeof(OneHourPage).FullName;

        private Model model;

        public OneHourPage()
        {
            InitializeComponent();

            Global.Model = new Model();
            BindingContext = Global.Model;

            Global.OneHourPage = this;
            Global.AppSettingsPage = new AppSettingsPage();
            Global.FortyEightHourPage = new FortyEightHourPage();

            Log.Debug(logTag, "MainPage started", true);

        }

        private void AppSettings_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(Global.AppSettingsPage);
        }

        private void Burger_Clicked(object sender, EventArgs e)
        {

        }

        private void FortyEightHour_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(Global.FortyEightHourPage);
        }

        //public void RefreshClicked(object sender, EventArgs args)
        //{
        //    model.RefreshFromDarkSky();
        //}

        //public void SaveApiKeyClicked(object sender, EventArgs args)
        //{
        //    model.SaveApiKey(apiKey.Text);
        //}

        //public void ApiKeyChanged(object sender, EventArgs args)
        //{
        //    model.ApiKeyHasChanged = true;
        //}

        //private void RefreshDelayPicker_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (model != null)
        //    {
        //        model.RefreshDelayString = (sender as Picker).SelectedItem.ToString();
        //    }
        //}

    }
}
