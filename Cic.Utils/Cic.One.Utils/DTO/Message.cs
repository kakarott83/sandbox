namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// Message-Klasse
    /// </summary>
    public class Message
    {
        /// <summary>
        /// code
        /// </summary>
        public string code;

        /// <summary>
        /// message
        /// </summary>
        public string message;

        /// <summary>
        /// detail
        /// </summary>
        public string detail;

        /// <summary>
        /// stacktrace
        /// </summary>
        public string stacktrace;

        /// <summary>
        /// duration
        /// </summary>
        public long duration;

        /// <summary>
        /// type
        /// </summary>
        public MessageType type;
    }


    /// <summary>
    /// MessageType-Enum
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Debug
        /// </summary>
        Debug = 1,

        /// <summary>
        /// Info
        /// </summary>
        Info = 2,

        /// <summary>
        /// Warn
        /// </summary>
        Warn = 3,

        /// <summary>
        /// Error
        /// </summary>
        Error = 4,

        /// <summary>
        /// Fatal
        /// </summary>
        Fatal = 5
    }
}