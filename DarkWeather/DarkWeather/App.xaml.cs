using Xamarin.Forms;

namespace DarkWeather
{
    public partial class App : Application
    {
        public static RootPage RootPage { get; set; }

        public static HomeMenuPage HomeMenuPage { get; set; }
        public static FortyEightHourMenuPage FortyEightHourMenuPage { get; set; }

        public static NavigationPage HomeNavigationPage { get; private set; }
        public static NavigationPage FortyEightHourNavigationPage { get; private set; }

        private static HomePage HomePage { get; set; }
        private static FortyEightHourPage FortyEightHourPage { get; set; }
        public static AppSettingsPage AppSettingsPage { get; set; }

        public static HomePageVM HomePageVM { get; set; }
        public static FortyEightHourPageVM FortyEightHourPageVM { get; set; }

        public static Model Model { get; set; }

        public App()
        {
            InitializeComponent();

            HomePage = new HomePage();
            FortyEightHourPage = new FortyEightHourPage();
            AppSettingsPage = new AppSettingsPage();

            RootPage = new RootPage();
            HomeMenuPage = new HomeMenuPage();
            FortyEightHourMenuPage = new FortyEightHourMenuPage();

            RootPage.Master = HomeMenuPage;
            // Wire up IsPresented so that the menu pages can close themselves
            HomeMenuPage.IsPresentedChange += MenuPage_IsPresentedChange;
            FortyEightHourMenuPage.IsPresentedChange += MenuPage_IsPresentedChange;

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

        private void MenuPage_IsPresentedChange(object sender, bool isPresented)
        {
            RootPage.IsPresented = isPresented;
        }

    }
}
