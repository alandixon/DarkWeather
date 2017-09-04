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

            BindingContext = Global.Model;

            Log.Debug(logTag, "FortyEightHourPage started", true);

        }

    }
}
