# DarkWeather

An experimental app using the API from https://darksky.net

The graphics are inverted - the lower the blue goes, the more likely / more intense the rain

One hour: lots of rain           |  48 hour: starts rainy and then clears up 
:-------------------------:|:-------------------------:
![One hour](https://github.com/alandixon/DarkWeather/blob/master/Images/OneHourRainfallExample.JPG)  |  ![48 hour](https://github.com/alandixon/DarkWeather/blob/master/Images/FortyEightHourRainfallExample.JPG)



## Requirements

* Minimum Android version - 4.0.3 (API level 15)

* An API key from DarkSky: https://darksky.net/dev/

* Google Play Services must be running on the device (needed for location).
In an Emulator, this typically means setting CPU/ABI to include the Google APIs


## Extra libraries

The following extra libraries are used

| Lib | Function |
:-------------------------:|:-------------------------:
| [SyncFusion Xamarin Forms](https://help.syncfusion.com/xamarin) - [Community license](https://www.syncfusion.com/products/communitylicense)| UI components |
| [DarkSky API](https://github.com/jcheng31/DarkSkyApi) | DarkSky Access |
| [PCLAppConfig](https://github.com/mrbrl/PCLAppConfig) | Config |
| [PCLStorage](https://github.com/dsplaisted/PCLStorage) | Folder and file access |
| [PCLCrypto](https://github.com/AArnott/PCLCrypto) | Crypto/ Security |
| [SQLite-net pcl](https://github.com/praeclarum/sqlite-net) | Database |
| [SQLitePCL.raw](https://github.com/ericsink/SQLitePCL.raw) | Low level Database |



## Further development

* Display week data. Currently we only do hour and 48 hour.

* Currently data is only being put into the db, not read out. Probably most needed at startup before further data is requested via the API.

* Present more data data at the day and week level. DarkSky returns a lot, but we're not using most of it.
 
* Improve the db interface. Probably with a simple ORM like [SQLite-Net Extensions](https://bitbucket.org/twincoders/sqlite-net-extensions)

* Purge data at intervals while the app is running. Currently only happens at startup.

* Use sunrise / sunset times in response data to stop "sun" showing at night.

* Logging needs help. Currently, all logging is driven into the Android debug log. I initially looked at NLog but it turned into a bit of a mare.

* iOS and Windows implementation