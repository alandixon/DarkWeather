using System;
using Xamarin.Forms;

namespace DarkWeather
{
    public class Settings
    {
        public static void EnableLabelUri(Label label)
        {
            label.TextColor = Color.Blue;
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {
                Device.OpenUri(new Uri(((Label)s).Text));
            };
            label.GestureRecognizers.Add(tapGestureRecognizer);
        }
    }
}
