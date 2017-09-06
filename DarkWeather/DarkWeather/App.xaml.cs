using Xamarin.Forms;

namespace DarkWeather
{
    public partial class App : Application
    {
        public static RootPage RootPage { get; set; }
        private static MenuPage MenuPage { get; set; }

        public static NavigationPage HomeNavigationPage { get; private set; }
        public static NavigationPage FortyEightHourNavigationPage { get; private set; }

        private static HomePage HomePage { get; set; }
        private static FortyEightHourPage FortyEightHourPage { get; set; }

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

            HomePage = new HomePage();
            FortyEightHourPage = new FortyEightHourPage();

            RootPage = new RootPage();
            MenuPage = new MenuPage();
            RootPage.Master = MenuPage;
            // Wire up IsPresented so that the menu page can close itself
            MenuPage.IsPresentedChange += MenuPage_IsPresentedChange;

            HomeNavigationPage = new NavigationPage(HomePage);
            FortyEightHourNavigationPage = new NavigationPage(FortyEightHourPage);

            // Start at home page
            RootPage.Detail = HomeNavigationPage;

            // and point the app at our root page
            MainPage = RootPage;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private void MenuPage_IsPresentedChange(object sender, bool isPresented)
        {
            RootPage.IsPresented = isPresented;
        }

    }
}
