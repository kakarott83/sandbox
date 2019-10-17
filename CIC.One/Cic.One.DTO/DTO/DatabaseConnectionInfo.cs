using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
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
        /// ORA_HOME
        /// </summary>
        public string home
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
    }
}