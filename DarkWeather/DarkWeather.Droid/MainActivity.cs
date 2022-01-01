using Android.App;
using Android.Content.PM;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.OS;
using Android.Widget;
using DarkWeather.Droid.Logging;

namespace DarkWeather.Droid
{
    // http://stackoverflow.com/questions/24611977/android-locationclient-class-is-deprecated-but-used-in-documentation/25173057#25173057

    [Activity(Label = "DarkWeather", Icon = "@drawable/CloudSun128xpt", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : 
        global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity,
        GoogleApiClient.IConnectionCallbacks,
        GoogleApiClient.IOnConnectionFailedListener,
        ILocationListener
    {
        Log Log = new Log();
        private string logTag = typeof(Model).FullName;

        GoogleApiClient apiClient;
        LocationRequest locationRequest;

        #region App Lifecycle methods

        protected override void OnCreate(Bundle bundle)
        {
            Log.Debug(logTag, "OnCreate()", true);

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            // Initialise the classes that hook into the PCL
            Secure secure = new Secure();
            //EnvVars envvars = new EnvVars();

            // Test reading config
            string val = Config.GetValue("Test.Config.Key");

            if (!InitialiseGooglePlayServices())
            {
                Log.Error(logTag, "Google Play Services is not installed", true);
                Toast.MakeText(this, "Google Play Services is not installed", ToastLength.Long).Show();
                Finish();
            }

            // Fire up the api client for location callbacks
            apiClient = new GoogleApiClient.Builder(this).AddApi(LocationServices.API).AddConnectionCallbacks(this).AddOnConnectionFailedListener(this).Build();

            LoadApplication(new App());
        }

        protected override void OnStart()
        {
            base.OnStart();
            Log.Info(logTag, "OnStart()", true);
            apiClient.Connect();
        }

        protected override void OnStop()
        {
            Log.Info(logTag, "OnStop()", true);
            apiClient.Disconnect();
            base.OnStop();
        }

        protected override void OnResume()
        {
            base.OnResume();
            Log.Debug(logTag, "OnResume()", true);
        }

        protected override async void OnPause()
        {
            base.OnPause();
            Log.Debug(logTag, "OnPause()", true);
        }

        #endregion App Lifecycle methods


        #region Methods

        /// <summary> Try to initialise Play services</summary>
        /// <returns> true if successful </returns>
        private bool InitialiseGooglePlayServices()
        {
            return IsGooglePlayServicesInstalled();
        }

        /// <summary> Determine if Play services are installed </summary>
        /// <returns> true if they are </returns>
        private bool IsGooglePlayServicesInstalled()
        {
            int queryResult = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (queryResult == ConnectionResult.Success)
            {
                Log.Info(logTag, "Google Play Services found", true);
                return true;
            }

            if (GoogleApiAvailability.Instance.IsUserResolvableError(queryResult))
            {
                string errorString = GoogleApiAvailability.Instance.GetErrorString(queryResult);
                Log.Error(logTag, string.Format("There is a problem with Google Play Services on this device: {0} - {1}", queryResult, errorString), true);
            }
            return false;
        }

        /// <summary> Notify anyone who has requested location updates </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        private void UpdatePclModel(float latitude, float longitude)
        {
            Global.updateLocation?.Invoke(latitude, longitude);
        }

        #endregion Methods


        #region IConnectionCallbacks implementation

        /// <summary> LocationClient connection </summary>
        /// <param name="bundle"></param>
        public void OnConnected(Bundle bundle)
        {
            Log.Info(logTag, "OnConnected()", true);

            locationRequest = LocationRequest.Create();
            locationRequest.SetPriority(LocationRequest.PriorityHighAccuracy);
            locationRequest.SetInterval(1000); // Update location every second
            LocationServices.FusedLocationApi.RequestLocationUpdates(apiClient, locationRequest, this);
        }

        /// <summary> LocationClient connection suspended  </summary>
        /// <param name="i"> Cause </param>
        public void OnConnectionSuspended(int i)
        {
            Log.Info(logTag, "OnConnectionSuspended()", true);
        }

        #endregion IConnectionCallbacks implementation


        #region IOnConnectionFailedListener

        /// <summary> LocationClient connection failed </summary>
        /// <param name="bundle"></param>
        public void OnConnectionFailed(ConnectionResult bundle)
        {
            Log.Info(logTag, "OnConnectionFailed()", true);
        }

        #endregion IOnConnectionFailedListener


        #region ILocationListener

        /// <summary>  Returns changes in the user's location if they've been requested </summary>
        /// <param name="location"></param>
        public void OnLocationChanged(Android.Locations.Location location)
        {
            Log.Info(logTag, string.Format("OnLocationChanged() lat={0}  long={1}", location.Latitude, location.Longitude), true);
            UpdatePclModel((float)location.Latitude, (float)location.Longitude);
        }

        #endregion ILocationListener

    }
}

