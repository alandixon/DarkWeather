// https://xamarinhelp.com/xamarin-forms-dependency-injection/
[assembly: Xamarin.Forms.Dependency(typeof(DarkWeather.Droid.Logging.Log))]
namespace DarkWeather.Droid.Logging
{

    public class Log : DarkWeather.Logging.ILog
    {
        private string markString = "## ";

        public void Debug(string tag, string message)
        {
            Android.Util.Log.Debug(tag, message);
        }

        public void Debug(string tag, string message, bool mark)
        {
            string newMessage = mark ? markString + message : markString;
            Debug(tag, newMessage);
        }


        public void Info(string tag, string message)
        {
            Android.Util.Log.Debug(tag, message);
        }

        public void Info(string tag, string message, bool mark)
        {
            string newMessage = mark ? markString + message : markString;
            Debug(tag, newMessage);
        }


        public void Error(string tag, string message)
        {
            Android.Util.Log.Error(tag, message);
        }

        public void Error(string tag, string message, bool mark)
        {
            string newMessage = mark ? markString + message : markString;
            Error(tag, newMessage);
        }
    }
}