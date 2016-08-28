using System;
using System.Diagnostics;
using System.Text;

namespace TcgSdk.Common
{
    /// <summary>
    /// An error log class for TcgSdk
    /// </summary>
    internal class TcgSdkErrorLog
    {
        /// <summary>
        /// Source is this app, TcgSdk
        /// </summary>
        private string source = "TcgSDk";

        /// <summary>
        /// The Exception to parse exception count and messages from
        /// </summary>
        private Exception Exception { get; set; }

        /// <summary>
        /// The Message to write to the log
        /// </summary>
        private string Message { get; set; }

        /// <summary>
        /// The System.Diagnostics.EventLogEntryType of the log
        /// </summary>
        private EventLogEntryType EventLogEntryType { get; set; }

        /// <summary>
        /// The integer ID of the event
        /// </summary>
        private int EventId { get; set; }
        /// <summary>
        /// Create a new log object, but do not immediately write it to the event log.
        /// </summary>
        /// <param name="e">The exception being logged</param>
        /// <param name="eventLogEntryType">The event log entry type</param>
        /// <param name="eventId">The integer id of the event. Defaults to 0 (zero).</param>
        public TcgSdkErrorLog(Exception e, EventLogEntryType eventLogEntryType, int eventId = 0)
        {
            Exception = e;
            Message = createMessage();
            EventLogEntryType = eventLogEntryType;
            EventId = eventId;
        }

        /// <summary>
        /// Create a string message to write into the event log from the exception messages
        /// </summary>
        /// <returns>string message to write into the event log from the exception messages</returns>
        private string createMessage()
        {
            if (null == Exception)
                return null;

            Exception workingException = Exception;

            var sb = new StringBuilder();

            sb.AppendLine(string.Format("There are {0} total exceptions in this chain.", countExceptions(Exception)));
            sb.AppendLine("-------------------------------------");

            while(null != workingException)
            {
                sb.AppendLine(workingException.Message);
                sb.AppendLine("-------------------------------------");

                workingException = workingException.InnerException;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Count the number of exceptions in the InnerException chain
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private static int countExceptions(Exception e)
        {
            Exception workingException = e;

            int count = 1;

            while(null != workingException.InnerException)
            {
                count++;
                workingException = workingException.InnerException;
            }

            return count;

            
        }

        /// <summary>
        /// Write the event to the event log
        /// </summary>
        public void WriteLog()
        {
            try
            {
                EventLog.WriteEntry(source, Message, EventLogEntryType, EventId);
            }
            catch
            {
                // Stop exception here to avoid endless loop
            }
        }
        /// <summary>
        /// Create a new log object, and immediately write it to the event log.
        /// </summary>
        /// <param name="e">The exception being logged</param>
        /// <param name="eventLogEntryType">The event log entry type</param>
        /// <param name="eventId">The integer id of the event. Defaults to 0 (zero).</param>
        public static void WriteLog(Exception e, EventLogEntryType eventLogEntryType, int eventId = 0)
        {
            try
            { 
                var log = new TcgSdkErrorLog(e, eventLogEntryType, eventId);

                log.WriteLog();
            }
            catch
            {
                // Stop exception here to avoid endless loop
            }
        }

    }
}
