using DarkWeather.Logging;
using System;
using Xamarin.Forms;

namespace DarkWeather
{
    public partial class AppSettingsPage : ContentPage
    {
        private ILog Log = DependencyService.Get<ILog>();
        private string logTag = typeof(AppSettingsPage).FullName;

        public AppSettingsPage()
        {
            InitializeComponent();

            Settings.EnableLabelUri(poweredByLabel);
            Settings.EnableLabelUri(faqLabel);

            BindingContext = Global.Model;

            Log.Debug(logTag, "AppSettingsPage started", true);

        }

        public void RefreshClicked(object sender, EventArgs args)
        {
            Global.Model.RefreshFromDarkSky();
        }

        public void SaveApiKeyClicked(object sender, EventArgs args)
        {
            Global.Model.SaveApiKey(apiKey.Text);
        }

        public void ApiKeyChanged(object sender, EventArgs args)
        {
            Global.Model.ApiKeyHasChanged = true;
        }

        private void RefreshDelayPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Global.Model != null)
            {
                Global.Model.RefreshDelayString = (sender as Picker).SelectedItem.ToString();
            }
        }

    }
}
