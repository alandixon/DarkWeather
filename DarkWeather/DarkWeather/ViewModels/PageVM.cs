using System.ComponentModel;

namespace DarkWeather
{
    public abstract class PageVM : INotifyPropertyChanged
    {
        public Model Model { get; set; }

        public PageVM(Model model)
        {
            Model = model;
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
