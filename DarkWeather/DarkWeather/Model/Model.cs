using DarkSkyApi;
using DarkSkyApi.Models;
using DarkWeather.Location;
using DarkWeather.Logging;
using DarkWeather.Secure;
using DarkWeather.Version;
using DarkWeather.Weather;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DarkWeather
{
    public class Model : INotifyPropertyChanged
    {
        // Logging
        private ILog Log = DependencyService.Get<ILog>();
        private string logTag = typeof(Model).FullName;

        // Versioning
        private IVersion version = DependencyService.Get<IVersion>();

        // Set after initialisation has completed
        // It prevents NotifyPropertyChanged() getting called too early
        public bool AllowChangeNotification { get; set; }

        private Db.DataTransfer db = null;

        private int currentRefreshDelayMinutes = 5;

        public Model()
        {
            // Wire up location updates
            Global.updateLocation += new Global.LocationDelegate(UpdateLocation);

            // Fetch and apply the ApiKey if there is one
            string apiKey = SecureProperties.GetApiKey();
            if (!string.IsNullOrEmpty(apiKey))
            {
                ApiKey = apiKey;
            }
            AllowChangeNotificationAfterAWhile();

            // Initialise the db
            db = new Db.DataTransfer();
            // Purge rows older than one day
            PurgeOldData();

            SetEmptyDataPoints();

            // Look for a valid Location, but don't wait for the result
            GetLocationAsync();

            DataLabelText = "Waiting for data";

            // Start the timer for calling the DarkSky API
            Device.StartTimer(new TimeSpan(0, currentRefreshDelayMinutes, 0), ApiCallTimerTick);

            // Wait 3 seconds for location and api key to stabilise
            Task.Delay(3000).ContinueWith((t) =>
            {
                // Get a weather data snapshot
                RefreshFromDarkSky();
            });

        }

        #region Methods

        /// <summary> Called when the api call timer ticks. Check for new time value </summary>
        /// <returns>true for the timer to keep recurring</returns>
        private bool ApiCallTimerTick()
        {
            RefreshFromDarkSky();

            if (currentRefreshDelayMinutes != RefreshDelayMinutes)
            {
                Log.Debug(logTag, string.Format("API refresh rate changed from {0} to {1} minutes", currentRefreshDelayMinutes, RefreshDelayMinutes), true);
                currentRefreshDelayMinutes = RefreshDelayMinutes;
                // Create a new timer
                Device.StartTimer(new TimeSpan(0, currentRefreshDelayMinutes, 0), ApiCallTimerTick);
                // and don't restart this one
                return false;
            }
            // No changes, just restart this timer
            return true;
        }

        private void AllowChangeNotificationAfterAWhile()
        {
            Task.Delay(5000)
                .ContinueWith((t) =>
                {
                    AllowChangeNotification = true;
                    NotifyPropertyChanged("DataLabelText");
                });
        }

        /// <summary> Get location either from the db or online </summary>
        /// <returns></returns>
        private async Task GetLocationAsync()
        {
            await Task.Run(() =>
            {
                string strLoc = GetStringFromDbAsync("Location").Result;
                Location.GeoLocation location;
                string parseFailMessage;
                if (DarkWeather.Location.GeoLocation.TryParse(strLoc, out location, out parseFailMessage))
                {
                    Location = location;
                }
                else
                {
                }
            });
        }

        /// <summary> Save the api key in a secure location </summary>
        /// <param name="apiKey"></param>
        public void SaveApiKey(string apiKey)
        {
            SecureProperties.SaveApiKey(apiKey);
            ApiKeyHasChanged = false;
        }

        /// <summary> Get new data from DarkSky </summary>
        public async void RefreshFromDarkSky()
        {
            try
            {
                Log.Info(logTag, "RefreshFromDarkSky()", true);

                if (location == null)
                {
                    throw new NullReferenceException("Location is null");
                }
                DarkSkyService service = new DarkSkyService(ApiKey);
                Forecast forecast = await service.GetWeatherDataAsync(Location.Latitude, Location.Longitude);

                // Process days
                if (forecast != null && forecast.Daily != null && forecast.Daily.Days != null)
                {
                    WeatherDays = AbsoluteTimeDayDataPoint.ConvertFromOffsetTime(forecast.Daily.Days);
                    //Todo: No storage yet - needs implementing
                    //await Task.Run(() => SaveToDbAsync(forecast.Hourly.Hours));
                }
                // Check presence of minutes - occasionally they seem to be missing! 
                if (forecast != null && forecast.Minutely != null && forecast.Minutely.Minutes != null)
                {
                    RainfallMinutes = AbsoluteTimeMinuteDataPoint.ConvertFromOffsetTime(forecast.Minutely.Minutes);
                    await Task.Run(() => SaveToDbAsync(forecast.Minutely.Minutes));
                }
                // and hours
                if (forecast != null && forecast.Hourly != null && forecast.Hourly.Hours != null)
                {
                    WeatherHours = AbsoluteTimeHourDataPoint.ConvertFromOffsetTime(forecast.Hourly.Hours);
                    await Task.Run(() => SaveToDbAsync(forecast.Hourly.Hours));
                }

                UpdateSunEventTimes(RainfallMinutes, WeatherHours, WeatherDays);
                UpdateSummaries(forecast);

                DataLabelText = string.Format("Data from {0:00}:{1:00}", DateTime.Now.Hour, DateTime.Now.Minute);

                ApiCallsMade = service.ApiCallsMade ?? 0;
                float apparentTemp = forecast.Currently.ApparentTemperature;
                float temp = forecast.Currently.Temperature;
                float pressure = forecast.Currently.Pressure;
                float cloudCover = forecast.Currently.CloudCover;
                float humidity = forecast.Currently.Humidity;
                string summary = forecast.Currently.Summary;
            }
            catch (Exception ex)
            {
                Log.Error(logTag, "RefreshFromDarkSky() failed - " +  ex.Message);
            }
        }

        private void UpdateSummaries(Forecast forecast)
        {
            if (forecast != null)
            {
                if (forecast.Daily != null)
                {
                    SummaryDay = forecast.Daily.Summary;
                }

                if (forecast.Hourly != null)
                {
                    SummaryHour = forecast.Hourly.Summary;
                }
                else
                {
                    SummaryHour = SummaryDay;
                }

                if (forecast.Minutely != null)
                {
                    SummaryMinute = forecast.Minutely.Summary;
                }
                else
                {
                    SummaryMinute = SummaryHour;
                }

                if (forecast.Currently != null)
                {
                    SummaryCurrent = forecast.Currently.Summary;
                }
                else
                {
                    SummaryMinute = SummaryMinute;
                }
            }
        }

        public void UpdateLocation(float latitude, float longitude)
        {
            Location = new GeoLocation(latitude, longitude, DateTime.Now);
            SaveToDbAsync("Location", Location.ToString(), true);
        }

        public void UpdateSunEventTimes(List<AbsoluteTimeMinuteDataPoint> weatherMinutes, List<AbsoluteTimeHourDataPoint> weatherHours, List<AbsoluteTimeDayDataPoint> weatherDays)
        {
            foreach (AbsoluteTimeDayDataPoint dayPoint in weatherDays)
            {
                foreach (AbsoluteTimeHourDataPoint hourPoint in weatherHours)
                {
                    if (dayPoint.LocalTime.DayOfYear == hourPoint.LocalTime.DayOfYear)
                    {
                        if (hourPoint.LocalTime < dayPoint.SunriseTime)
                        {
                            hourPoint.Sun.SunState = SunState.PreSunrise;
                            hourPoint.Sun.SunPercent = 0;
                        }
                        else if (hourPoint.LocalTime < dayPoint.SunsetTime)
                        {
                            hourPoint.Sun.SunState = SunState.SunUp;
                            hourPoint.Sun.SunPercent = 100;
                        }
                        else
                        {
                            hourPoint.Sun.SunState = SunState.PostSunset;
                            hourPoint.Sun.SunPercent = 0;
                        }
                    }
                }
                foreach (AbsoluteTimeMinuteDataPoint minutePoint in weatherMinutes)
                {
                    if (dayPoint.LocalTime.DayOfYear == minutePoint.LocalTime.DayOfYear)
                    {
                        if (minutePoint.LocalTime < dayPoint.SunriseTime)
                        {
                            minutePoint.Sun.SunState = SunState.PreSunrise;
                            minutePoint.Sun.SunPercent = 0;
                        }
                        else if (minutePoint.LocalTime < dayPoint.SunsetTime)
                        {
                            minutePoint.Sun.SunState = SunState.SunUp;
                            minutePoint.Sun.SunPercent = 100;
                        }
                        else
                        {
                            minutePoint.Sun.SunState = SunState.PostSunset;
                            minutePoint.Sun.SunPercent = 0;
                        }
                    }
                }
            }

        }

        /// <summary> Fill in empty data so the chart time axis looks good </summary>
        private void SetEmptyDataPoints()
        {
            RainfallMinutes = new List<AbsoluteTimeMinuteDataPoint>()
                {
                    new AbsoluteTimeMinuteDataPoint { Time = DateTime.Now,  LocalTime = DateTime.Now.ToLocalTime(), PrecipitationProbability = (float)0.0, PrecipitationIntensity = (float)0.0},
                    new AbsoluteTimeMinuteDataPoint { Time = DateTime.Now.AddMinutes(60), LocalTime = DateTime.Now.ToLocalTime().AddMinutes(60), PrecipitationProbability = (float)0.0,  PrecipitationIntensity = (float)0.0}
                };

            WeatherHours = new List<AbsoluteTimeHourDataPoint>()
                {
                    new AbsoluteTimeHourDataPoint{ Time = DateTime.Now,  LocalTime = DateTime.Now.ToLocalTime().AddMinutes(0)},
                    new AbsoluteTimeHourDataPoint{ Time = DateTime.Now.AddHours(AbsoluteTimeHourDataPoint.FortyEight),  LocalTime = DateTime.Now.ToLocalTime().AddHours(AbsoluteTimeHourDataPoint.FortyEight)}
                };
        }

        private void PurgeOldData()
        {
            DateTime purgeDate = DateTime.Now.AddDays(-1);
            db.DeleteValuesBeforeDateTime(purgeDate, typeof(MinuteDataPoint));
            db.DeleteValuesBeforeDateTime(purgeDate, typeof(HourDataPoint));
        }

        #endregion Methods


        #region dB methods

        /// <summary> Save hourDataPoints to the db </summary>
        /// <param name="hourDataPoints"></param>
        private async void SaveToDbAsync(IEnumerable<HourDataPoint> hourDataPoints)
        {
            int count = 0;

            try
            {
                foreach (HourDataPoint hourDataPoint in hourDataPoints)
                {
                    int pk = await db.SaveHourDataPointAsync(hourDataPoint);
                    count++;
                }
                // Todo Log count
            }
            catch (Exception ex)
            {
                Log.Error(logTag, "SaveToDbAsync(hourDataPoints) failed - " + ex.Message);
                throw;
            }
        }

        /// <summary> Save minuteDataPoints to the db </summary>
        /// <param name="minuteDataPoints"></param>
        private async void SaveToDbAsync(IEnumerable<MinuteDataPoint> minuteDataPoints)
        {
            int count = 0;

            try
            {
                foreach (MinuteDataPoint minuteDataPoint in minuteDataPoints)
                {
                    int pk = await db.SaveMinuteDataPointAsync(minuteDataPoint);
                    count++;
                }
                // Todo Log count
            }
            catch (Exception ex)
            {
                Log.Error(logTag, "SaveToDbAsync(minuteDataPoints) failed - " + ex.Message);
                throw;
            }
        }

        /// <summary> Save a kvp to the db, updating an existing one if specified </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="update"></param>
        private async void SaveToDbAsync(string key, string value, bool update)
        {
            try
            {
                if (update)
                {
                    await db.DeleteValueFromKvpAsync(key);
                }
                int pk = await db.SaveKeyValuePairAsync(new KeyValuePair<string, string>(key, value));
            }
            catch (Exception ex)
            {
                Log.Error(logTag, "SaveToDbAsync(KeyValue pair) failed - " + ex.Message);
                throw;
            }
        }

        /// <summary> Get a kvp value from the db </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private async Task<string> GetStringFromDbAsync(string key)
        {
            string result;
            try
            {
                result = await db.FetchValueFromKvpAsync(key);
            }
            catch (Exception ex)
            {
                Log.Error(logTag, string.Format("GetStringFromDbAsync(key) failed for key={0} - {1}", key, ex.Message));
                throw;
            }
            return result;
        }

        #endregion dB methods


        #region Properties

        private List<AbsoluteTimeMinuteDataPoint> rainfallMinutes;
        public List<AbsoluteTimeMinuteDataPoint> RainfallMinutes
        {
            get
            {
                return rainfallMinutes;
            }
            set
            {
                rainfallMinutes = value;
                NotifyPropertyChanged("RainfallMinutes");
            }
        }

        private List<AbsoluteTimeHourDataPoint> weatherHours;
        public List<AbsoluteTimeHourDataPoint> WeatherHours
        {
            get
            {
                return weatherHours;
            }
            set
            {
                weatherHours = value;
                NotifyPropertyChanged("WeatherHours");
            }
        }

        private List<AbsoluteTimeDayDataPoint> weatherDays;
        public List<AbsoluteTimeDayDataPoint> WeatherDays
        {
            get
            {
                return weatherDays;
            }
            set
            {
                weatherDays = value;
                NotifyPropertyChanged("WeatherDays");
            }
        }

        private string apiKey=null;
        public string ApiKey
        {
            get
            {
                return apiKey;
                //return Secure.GetApiKey();
            }
            set
            {
                apiKey = value;
                NotifyPropertyChanged("ApiKey");
                // Update presence notifier to enable the Refresh key 
                ApiKeyPresent = !string.IsNullOrWhiteSpace(apiKey);
            }
        }

        private bool apiKeyPresent;
        public bool ApiKeyPresent
        {
            get
            {
                return apiKeyPresent;
            }
            set
            {
                apiKeyPresent = value;
                NotifyPropertyChanged("ApiKeyPresent");
            }
        }

        int apiCallsMade;
        public int ApiCallsMade
        {
            get
            {
                return apiCallsMade;
            }
            set
            {
                apiCallsMade = value;
                NotifyPropertyChanged("ApiCallsMade");
            }

        }

        private bool apiKeyHasChanged;
        public bool ApiKeyHasChanged
        {
            get
            {
                return apiKeyHasChanged;
            }
            set
            {
                if (AllowChangeNotification)
                {
                    apiKeyHasChanged = value;
                    NotifyPropertyChanged("ApiKeyHasChanged");
                }
            }
        }

        private IList<MinuteDataPoint> minuteDataPoints;
        public IList<MinuteDataPoint> MinuteDataPoints
        {
            get
            {
                if (minuteDataPoints == null)
                {
                    minuteDataPoints = new List<MinuteDataPoint>();
                }
                return minuteDataPoints;
            }
            set
            {
                minuteDataPoints = value;
                NotifyPropertyChanged("MinuteDataPoints");
            }
        }


        private string dataLabelText;
        public string DataLabelText
        {
            get
            {
                return dataLabelText;
            }
            set
            {
                if (AllowChangeNotification)
                {
                    dataLabelText = value;
                    NotifyPropertyChanged("DataLabelText");
                }
            }
        }

        private Location.GeoLocation location;
        public Location.GeoLocation Location
        {
            get
            {
                return location;
            }
            private set
            {
                location = value;
                NotifyPropertyChanged("Location");
            }
        }

        private string summaryCurrent;
        public string SummaryCurrent
        {
            get
            {
                return summaryCurrent;
            }
            private set
            {
                summaryCurrent = value;
                NotifyPropertyChanged("SummaryCurrent");
            }
        }

        private string summaryMinute;
        public string SummaryMinute
        {
            get
            {
                return summaryMinute;
            }
            private set
            {
                summaryMinute = value;
                NotifyPropertyChanged("SummaryMinute");
            }
        }

        private string summaryHour;
        public string SummaryHour
        {
            get
            {
                return summaryHour;
            }
            private set
            {
                summaryHour = value;
                NotifyPropertyChanged("SummaryHour");
            }
        }

        private string summaryDay;
        public string SummaryDay
        {
            get
            {
                return summaryDay;
            }
            private set
            {
                summaryDay = value;
                NotifyPropertyChanged("SummaryDay");
            }
        }

        private string refreshDelayString = "5 minutes";
        public string RefreshDelayString
        {
            get
            {
                return refreshDelayString;
            }
            set
            {
                refreshDelayString = value;
                // NotifyPropertyChanged("RefreshDelayString");
            }
        }

        /// <summary> Int value extracted from RefreshDelayString </summary>
        public int RefreshDelayMinutes {
            get
            {
                int newRefreshDelayMinutes;
                if (int.TryParse((RefreshDelayString.Split(new char[] { ' ' })[0]), out newRefreshDelayMinutes))
                {
                    return newRefreshDelayMinutes;
                }
                return currentRefreshDelayMinutes;
            }
        }

        /// <summary> Google version name (not version number/code), generated by the droid project from the manifest </summary>
        public string AppVersionName
        {
            get
            {
                return version.VersionName;
            }
        }


        #endregion Properties

        //the view will register to this event when the DataContext is set
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (AllowChangeNotification && PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }

}
