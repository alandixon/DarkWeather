namespace DarkWeather
{
    public class HomePageVM : PageVM
    {
        public HomePageVM(Model model) : base(model)
        {
            CurrentSummaryEnabled = true;
            RainEnabled = true;
        }

        private bool currentSummaryEnabled;
        public bool CurrentSummaryEnabled
        {
            get { return currentSummaryEnabled; }
            set
            {
                currentSummaryEnabled = value;
                NotifyPropertyChanged("CurrentSummaryEnabled");
            }
        }


    }
}
