namespace DarkWeather
{
    public class HomePageVM : PageVM
    {
        public HomePageVM(Model model) : base(model)
        {
            SunCloudEnabled = true;
            RainEnabled = true;
            TempEnabled = true;
        }
    }
}
