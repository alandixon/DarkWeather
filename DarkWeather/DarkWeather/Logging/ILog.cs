namespace DarkWeather.Logging
{
    public interface ILog
    {
        /// <summary> Debug with tag and message</summary>
        /// <param name="tag">Class</param>
        /// <param name="message">Log message</param>
        void Debug(string tag, string message);

        /// <summary> Debug with tag, log message and optional mark</summary>
        /// <param name="tag">Class</param>
        /// <param name="message">Log message</param>
        /// <param name="mark">Prepend message with a mark to make it easier to find in the debug log</param>
        void Debug(string tag, string message, bool mark);


        /// <summary> Info with tag and message</summary>
        /// <param name="tag">Class</param>
        /// <param name="message">Log message</param>
        void Info(string tag, string message);

        /// <summary> Info with tag, log message and optional mark</summary>
        /// <param name="tag">Class</param>
        /// <param name="message">Log message</param>
        /// <param name="mark">Prepend message with a mark to make it easier to find in the debug log</param>
        void Info(string tag, string message, bool mark);


        /// <summary> Error with tag and message</summary>
        /// <param name="tag">Class</param>
        /// <param name="message">Log message</param>
        void Error(string tag, string message);

        /// <summary> Error with tag, log message and optional mark</summary>
        /// <param name="tag">Class</param>
        /// <param name="message">Log message</param>
        /// <param name="mark">Prepend message with a mark to make it easier to find in the debug log</param>
        void Error(string tag, string message, bool mark);


    }
}
