namespace DarkWeather
{
    public class FortyEightHourPageVM : PageVM
    {
        public FortyEightHourPageVM(Model model) : base(model)
        {
            SunCloudEnabled = true;
            RainEnabled = true;
            TempEnabled = true;
        }
    }
}
