using DarkWeather.Logging;
using System;
using Xamarin.Forms;

namespace DarkWeather
{
    public partial class MediumTermPage : ContentPage
    {
        private ILog Log = DependencyService.Get<ILog>();
        private string logTag = typeof(MainPage).FullName;

        private Model model;

        public MediumTermPage()
        {
            InitializeComponent();

            //Settings.EnableLabelUri(poweredByLabel);
            //Settings.EnableLabelUri(faqLabel);

            model = new Model();
            BindingContext = model;

            Log.Debug(logTag, "MediumTermPage started", true);

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
