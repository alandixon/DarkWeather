# DarkWeather

An experimental app using the API from https://darksky.net

## Requirements

* Minimum Androd version - 4.0.3 (API level 15)

* An API key from DarkSky: https://darksky.net/dev/

* Google Play Services must be running on the device (needed for location).
In an Emulator, this typically means setting CPU/ABI to include the Google APIs

## Further development

* Display week data. Currently we only do hour and 48 hour.

* Currently data is only being put into the db, not read out. Probably most needed at startup before further data is requested via the API.

* Present more data data at the day and week level. DarkSky returns a lot, but we're not using most of it.
 
* Improve the db interface. Probably with a simple ORM like [SQLite-Net Extensions](https://bitbucket.org/twincoders/sqlite-net-extensions)

* Purge data at intervals while the app is running. Currently only happens at startup.

* Logging needs help. Currently, all logging is driven into the Android debug log. I initially looked at NLog but it turned into a bit of a mare.

* iOS and Windows implementation