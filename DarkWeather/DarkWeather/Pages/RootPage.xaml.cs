using Xamarin.Forms;

namespace DarkWeather
{
    public partial class RootPage : MasterDetailPage
    {
        public RootPage()
        {
            InitializeComponent();
            MasterBehavior = MasterBehavior.Popover;
        }
        
    }
}

