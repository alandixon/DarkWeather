namespace DarkWeather
{
    public class FortyEightHourPageVM : PageVM
    {
        public FortyEightHourPageVM(Model model) : base(model)
        {
            HourSummaryEnabled = true;
            SunCloudEnabled = true;
            RainEnabled = true;
            TempCEnabled = true;
            WindEnabled = true;
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

        private bool windEnabled;
        public bool WindEnabled
        {
            get { return windEnabled; }
            set
            {
                windEnabled = value;
                NotifyPropertyChanged("WindEnabled");
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

        private bool tempCEnabled;
        public bool TempCEnabled
        {
            get { return tempCEnabled; }
            set
            {
                tempCEnabled = value;
                NotifyPropertyChanged("TempCEnabled");
                TempEnabled = tempCEnabled || tempFEnabled;
            }
        }

        private bool tempFEnabled;
        public bool TempFEnabled
        {
            get { return tempFEnabled; }
            set
            {
                tempFEnabled = value;
                NotifyPropertyChanged("TempFEnabled");
                TempEnabled = tempCEnabled || tempFEnabled;
            }
        }

    }
}
