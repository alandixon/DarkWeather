using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkWeather
{
    public class FortyEightHourPageVM : INotifyPropertyChanged
    {
        public Model Model { get; set; }

        public FortyEightHourPageVM(Model model)
        {
            Model = model;

            SunCloudEnabled = true;
            RainEnabled = true;
            TempEnabled = true;
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

        private bool rainEnabled;
        public bool RainEnabled
        {
            get { return rainEnabled; }
            set
            {
                rainEnabled = value;
                NotifyPropertyChanged("RainEnabled");
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

        //the view will register to this event when the DataContext is set
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (Model.AllowChangeNotification && PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

    }
}
