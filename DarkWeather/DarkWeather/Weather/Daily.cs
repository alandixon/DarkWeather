using System.Collections.Generic;

namespace DarkWeather.Weather
{
    public class Daily : IPrediction
    {
        public IList<Datum> data { get; set; }
        public string icon { get; set; }
        public string summary { get; set; }
    }
}
