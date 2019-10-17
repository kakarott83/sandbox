using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class ServiceInfoDto : oBaseDto
    {
        /// <summary>
        /// Assembly Version of Service
        /// </summary>
        public string ServiceVersion
        {
            get;
            set;
        }

        public string netVersion {get;set;}
        public string iisVersion { get; set; }
        public string osVersion { get; set; }
        public string logLevel { get; set; }

        /// <summary>
        /// OL DB Version
        /// </summary>
        public string OpenLeaseDatabaseVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Instance information
        /// </summary>
        public DatabaseInstanceInfo DatabaseInstance
        {
            get;
            set;
        }

        /// <summary>
        /// Connection information
        /// </summary>
        public DatabaseConnectionInfo DatabaseConnection
        {
            get;
            set;
        }
    }
}