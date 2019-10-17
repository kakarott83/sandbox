
namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Holds current DB Connection Info
    /// </summary>
    public class DatabaseConnectionInfo
    {
        /// <summary>
        /// DB Instance name
        /// </summary>
        public string sid
        {
            get;
            set;
        }

        /// <summary>
        /// DB Host Name
        /// </summary>
        public string hostName
        {
            get;
            set;
        }

        /// <summary>
        /// Direct or Oracle Client connection
        /// </summary>
        public bool directConnection
        {
            get;
            set;
        }

        /// <summary>
        /// DB Port
        /// </summary>
        public int port
        {
            get;
            set;
        }


        /// <summary>
        /// DB Client Version
        /// </summary>
        public string clientVersion
        {
            get;
            set;
        }

        /// <summary>
        /// DB Name
        /// </summary>
        public string name
        {
            get;
            set;
        }

        /// <summary>
        /// Roundtrip time for the most basic query
        /// </summary>
        public long latency { get; set; }

        /// <summary>
        /// Oracle Home
        /// </summary>
        public string orahome { get; set; }
    }
}