using System;
using Xamarin.Forms;

namespace DarkWeather
{
    public partial class MenuPage : ContentPage
    {
        public event EventHandler<bool> IsPresentedChange;

        public MenuPage()
        {
            Title = "Menu";
            InitializeComponent();
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

