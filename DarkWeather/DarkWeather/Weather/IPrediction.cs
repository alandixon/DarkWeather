using System.Collections.Generic;

namespace DarkWeather.Weather
{
    interface IPrediction
    {
        IList<Datum> data { get; set; }
        string icon { get; set; }
        string summary { get; set; }
    }
}
