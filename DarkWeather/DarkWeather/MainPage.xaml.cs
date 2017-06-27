using DarkWeather.Logging;
using System;
using Xamarin.Forms;

namespace DarkWeather
{
    public partial class MainPage : TabbedPage
    {
        private ILog Log = DependencyService.Get<ILog>();
        private string logTag = typeof(MainPage).FullName;

        private Model model;

        public MainPage()
        {
            InitializeComponent();


            navigationDrawer1.DrawerWidth = 200;
            // hamburgerButton.Image = (FileImageSource)ImageSource.FromFile("Hamburger50.png");

            //List<string> list = new List<string>();
            //list.Add("Home");
            //list.Add("Profile");
            //list.Add("Inbox");
            //list.Add("Outbox");
            //list.Add("Sent");
            //list.Add("Draft");
            //listView.ItemsSource = list;




            Settings.EnableLabelUri(poweredByLabel);
            Settings.EnableLabelUri(faqLabel);
            model = new Model();
            BindingContext = model;

            Log.Debug(logTag, "MainPage started", true);

        }

        void hamburgerButton_Clicked(object sender, EventArgs e)
        {
            navigationDrawer1.ToggleDrawer();
        }

        //private void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        //{
        //    // Your codes here
        //    navigation.ToggleDrawer();
        //}

        public void RefreshClicked(object sender, EventArgs args)
        {
            model.RefreshFromDarkSky();
        }

        public void SaveApiKeyClicked(object sender, EventArgs args)
        {
            model.SaveApiKey(apiKey.Text);
        }

        public void ApiKeyChanged(object sender, EventArgs args)
        {
            model.ApiKeyHasChanged = true;
        }

        private void RefreshDelayPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (model != null)
            {
                model.RefreshDelayString = (sender as Picker).SelectedItem.ToString();
            }
        }

        private void HamburgerButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}
