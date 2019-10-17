using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    /// <summary>
    /// Holds current DB Instance Info
    /// </summary>
    public class DatabaseInstanceInfo
    {
        /// <summary>
        /// Instance Name
        /// </summary>
        public string instanceName
        {
            get;
            set;
        }

        /// <summary>
        /// Host Name
        /// </summary>
        public string hostName
        {
            get;
            set;
        }

        /// <summary>
        /// Version of DB Vendor
        /// </summary>
        public string version
        {
            get;
            set;
        }
    }
}