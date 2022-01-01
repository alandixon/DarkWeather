using DarkSkyApi.Models;
using Darkweather;
using DarkWeather.Logging;
using DarkWeather.Weather;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DarkWeather.Db
{

    public class DataTransfer
    {
        private ILog Log = DependencyService.Get<ILog>();
        private string logTag = typeof(DataTransfer).FullName;

        private Database database;
        private SQLiteAsyncConnection connection;
        

        public DataTransfer()
        {
            database = new Database(Global.ThisAppName);
            connection = database.Connection;
            InitialiseDbAsync();
        }

        /// <summary> Create tables if they don't exist </summary>
        /// <returns></returns>
        private async Task InitialiseDbAsync()
        {
            try
            {
                bool tableExists = await Database.TableExistsAsync(connection, "DatumDto");
                if (!tableExists)
                {
                    await connection.CreateTableAsync<DatumDto>();
                }

                tableExists = await Database.TableExistsAsync(connection, "HourDataPointDto");
                if (!tableExists)
                {
                    await connection.CreateTableAsync<HourDataPointDto>();
                }

                tableExists = await Database.TableExistsAsync(connection, "MinuteDataPointDto");
                if (!tableExists)
                {
                    await connection.CreateTableAsync<MinuteDataPointDto>();
                }

                tableExists = await Database.TableExistsAsync(connection, "KeyValuePairDto");
                if (!tableExists)
                {
                    await connection.CreateTableAsync<KeyValuePairDto>();
                }
            }
            catch (Exception ex)
            {
                Log.Error(logTag, "InitialiseDbAsync() failed: " + ex.Message);
                throw;
            }
        }

        /// <summary> Save a Datum in the db </summary>
        /// <param name="datum"></param>
        /// <returns>Primary key of the saved object</returns>
        public async Task<int> SaveDatumAsync(Datum datum)
        {
            DatumDto datumDto = Factory.CreateChildFromParent<DatumDto, Datum>(datum);
            return await connection.InsertAsync(datumDto);
        }

        /// <summary> Save an HourDataPoint in the db</summary>
        /// <param name="hourDataPoint"></param>
        /// <returns>Primary key of the saved object</returns>
        public async Task<int> SaveHourDataPointAsync(HourDataPoint hourDataPoint)
        {
            HourDataPointDto hourDataPointDto = Factory.CreateChildFromParent<HourDataPointDto, HourDataPoint>(hourDataPoint);
            return await connection.InsertAsync(hourDataPointDto);
        }

        /// <summary> Save a MinuteDataPoint in the db</summary>
        /// <param name="minuteDataPoint"></param>
        /// <returns>Primary key of the saved object</returns>
        public async Task<int> SaveMinuteDataPointAsync(MinuteDataPoint minuteDataPoint)
        {
            MinuteDataPointDto minuteDataPointDto = Factory.CreateChildFromParent<MinuteDataPointDto, MinuteDataPoint>(minuteDataPoint);
            return await connection.InsertAsync(minuteDataPointDto);
        }

        /// <summary> Save a KeyValuePair in the db</summary>
        /// <param name="kvp"></param>
        /// <returns>Primary key of the saved object</returns>
        public async Task<int> SaveKeyValuePairAsync(KeyValuePair<string, string> kvp)
        {
            KeyValuePairDto keyValuePairDto = new KeyValuePairDto()
            {
                Key = kvp.Key,
                Value = kvp.Value
            };
            return await connection.InsertAsync(keyValuePairDto);
        }

        /// <summary> Return a value for a given key </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> FetchValueFromKvpAsync(string key)
        {
            return await Database.GetStringValue(connection, "keyValuePairDto", key);
        }

        /// <summary> Delete a kvp from the db for the given key </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> DeleteValueFromKvpAsync(string key)
        {
            return await Database.DeleteStringValue(connection, "keyValuePairDto", key);
        }

        /// <summary> Delete rows of type (type) dated before provided DateTime </summary>
        /// <param name="dateTime"></param>
        /// <param name="type"></param>
        /// <returns>true on success</returns>
        public async Task<bool> DeleteValuesBeforeDateTime(DateTime dateTime, Type type)
        {
            if (type == typeof(MinuteDataPoint))
            {
                return await Database.DeleteValuesBeforeDateTime(connection, "MinuteDataPointDto", dateTime);
            }

            if (type == typeof(HourDataPoint))
            {
                return await Database.DeleteValuesBeforeDateTime(connection, "HourDataPointDto", dateTime);
            }

            throw new NotImplementedException(string.Format("DeleteValuesBeforeDateTime() doesn't work with type {0}", type.FullName));

        }

    }

}