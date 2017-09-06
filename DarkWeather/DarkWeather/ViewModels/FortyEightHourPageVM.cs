namespace DarkWeather
{
    public class FortyEightHourPageVM : PageVM
    {
        public FortyEightHourPageVM(Model model) : base(model)
        {
            HourSummaryEnabled = true;
            SunCloudEnabled = true;
            RainEnabled = true;
            TempEnabled = true;
        }

        private bool hourSummaryEnabled;
        public bool HourSummaryEnabled
        {
            get { return hourSummaryEnabled; }
            set
            {
                hourSummaryEnabled = value;
                NotifyPropertyChanged("HourSummaryEnabled");
            }
        }

        private bool sunCloudEnabled;
        public bool SunCloudEnabled
        {
            get { return sunCloudEnabled; }
            set
            {
                sunCloudEnabled = value;
                NotifyPropertyChanged("SunCloudEnabled");
            }
        }

        private bool tempEnabled;
        public bool TempEnabled
        {
            get { return tempEnabled; }
            set
            {
                tempEnabled = value;
                NotifyPropertyChanged("TempEnabled");
            }
        }

    }
}
