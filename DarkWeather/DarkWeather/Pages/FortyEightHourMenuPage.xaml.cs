using System;
using Xamarin.Forms;

namespace DarkWeather
{
    public partial class FortyEightHourMenuPage : ContentPage
    {
        public event EventHandler<bool> IsPresentedChange;

        public FortyEightHourMenuPage()
        {
            Title = "Menu";
            InitializeComponent();
            BindingContext = App.FortyEightHourPageVM;
        }

        private void Done_Clicked(object sender, EventArgs e)
        {
            var handler = IsPresentedChange;
            if (IsPresentedChange != null)
            {
                // Tell the observer that IsPresented is false i.e. close this page
                handler(this, false);
            }
        }
    }
}

