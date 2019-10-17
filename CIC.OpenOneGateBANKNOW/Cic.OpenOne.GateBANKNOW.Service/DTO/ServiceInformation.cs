using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Common Webservice Information
    /// </summary>
    public class ServiceInformation : oBaseDto
    {
        /// <summary>
        /// Assembly Version of Service
        /// </summary>
        public string ServiceVersion
        {
            get;
            set;
        }

       
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