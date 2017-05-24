using DarkWeather.Logging;
using PCLStorage;
using SQLite;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Darkweather
{

    public class Database
    {
        private static ILog Log = DependencyService.Get<ILog>();
        private static string logTag = typeof(Database).FullName;

        private string path;

        public Database(string name)
        {
            path = Path.Combine(FileSystem.Current.LocalStorage.Path, name + ".db");
            Connection = new SQLiteAsyncConnection(path);
        }

        public SQLiteAsyncConnection Connection
        {
            get;
            private set;
        }

        /// <summary> Check for table existence 
        /// Currently this only returns true f there is at least one row in the table </summary>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static async Task<bool> TableExistsAsync(SQLiteAsyncConnection connection, string tableName)
        {
            string cmd = string.Format("SELECT 1 FROM {0} LIMIT 1", tableName);
            try
            {
                string result = await connection.ExecuteScalarAsync<string>(cmd);
            }
            catch (SQLiteException sqex)
            {
                return false;
            }
            return true;
        }

        /// <summary> Get kvp </summary>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<string> GetStringValue(SQLiteAsyncConnection connection, string tableName, string key)
        {
            string cmd = string.Format("SELECT value FROM {0} WHERE key='{1}'", tableName, key);
            string result;
            try
            {
                result = await connection.ExecuteScalarAsync<string>(cmd);
            }
            catch (Exception ex)
            {
                Log.Error(logTag, string.Format("GetStringValue() had a problem getting the value for key={0} from table {1}: {2}", key, tableName, ex.Message));
                throw;
            }
            return result;
        }

        /// <summary> Delete kvp if it's present </summary>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<bool> DeleteStringValue(SQLiteAsyncConnection connection, string tableName, string key)
        {
            string cmd = string.Format("SELECT ID FROM {0} WHERE key='{1}'", tableName, key);
            bool existing;
            try
            {
                int id = await connection.ExecuteScalarAsync<int>(cmd);
                existing = true;
                cmd = string.Format("DELETE FROM {0} WHERE key='{1}'", tableName, key);
                await connection.ExecuteScalarAsync<int>(cmd);
            }
            catch (Exception ex)
            {
                Log.Error(logTag, string.Format("DeleteStringValue() had a problem deleting key={0} from table {1}: {2}", key, tableName, ex.Message));
                throw;
            }
            return existing;
        }

        /// <summary> Delete rows older than the provided DateTime </summary>
        /// <param name="connection"></param>
        /// <param name="tableName"></param>
        /// <param name="dateTime"></param>
        /// <returns>true on success</returns>
        public static async Task<bool> DeleteValuesBeforeDateTime(SQLiteAsyncConnection connection, string tableName, DateTime dateTime)
        {
            string cmd = string.Format("DELETE FROM {0} WHERE Time<'{1}'", tableName, dateTime.Ticks);
            bool success;
            try
            {
                await connection.ExecuteScalarAsync<int>(cmd);
                Log.Debug(logTag, string.Format("DeleteValuesBeforeDateTime() ran against table {0}", tableName));
                success = true;
            }
            catch (Exception ex)
            {
                Log.Error(logTag, string.Format("DeleteValuesBeforeDateTime() had a problem on table {0}: {1}", tableName, ex.Message));
                throw;
            }
            return success;
        }

    }

}