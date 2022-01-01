using Xamarin.Forms;

namespace DarkWeather
{
    public partial class App : Application
    {
        public static NavigationPage NavigationPage { get; private set; }
        private static RootPage RootPage;

        public static bool MenuIsPresented
        {
            get
            {
                return RootPage.IsPresented;
            }
            set
            {
                RootPage.IsPresented = value;
            }
        }



        public App()
        {
            InitializeComponent();
            NavigationPage = new NavigationPage(new HomePage());
            RootPage = new RootPage();
            var menuPage = new MenuPage();
            RootPage.Master = menuPage;
            RootPage.Detail = NavigationPage;
            MainPage = RootPage;
            // Wire up IsPresented so that the menu page can close itself
            menuPage.IsPresentedChange += MenuPage_IsPresentedChange;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private void MenuPage_IsPresentedChange(object sender, bool isPresented)
        {
            RootPage.IsPresented = isPresented;
        }

    }
}
