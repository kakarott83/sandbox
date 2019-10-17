using System;

namespace Cic.OpenOne.Common.Util.Config
{
    /// <summary>
    /// Class for holding database connection parameters
    /// </summary>
    public class DBSettings
    {
        /// <summary>
        /// isDirect
        /// </summary>
        public bool isDirect { get; set; }

        // values for direct connection (DirectMode = True)
        /// <summary>
        /// ServerName
        /// </summary>
        public String ServerName { get; set; }
        /// <summary>
        /// ServerPort
        /// </summary>
        public String ServerPort { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String ServerSID { get; set; }

        // Parameter to select the Oracle Client
        /// <summary>
        /// oraClientHome
        /// </summary>
        public String oraClientHome { get; set; }

        // values for tns connection (relevant only when DirectMode = False)
        // When Direct is false, in Data Source option you must specify either TNS name or TNS description.
        // Port and SID options are not allowed when Direct is false. 
        /// <summary>
        /// DataSource
        /// </summary>
        public String DataSource { get; set; }

        //values for db login
        /// <summary>
        /// UserId
        /// </summary>
        public String UserId { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        public String Password { get; set; }
        /// <summary>
        /// plainPassword
        /// </summary>
        public bool plainPassword { get; set; }

        //connection parameters
        /// <summary>
        /// timeout
        /// </summary>
        public String timeout { get; set; }
        /// <summary>
        /// minPool
        /// </summary>
        public String minPool { get; set; }
        /// <summary>
        /// maxPool
        /// </summary>
        public String maxPool { get; set; }
        /// <summary>
        /// valConn
        /// </summary>
        public String valConn { get; set; }
        /// <summary>
        /// stmtCache
        /// </summary>
        public String stmtCache { get; set; }

        /// <summary>
        /// if true, use CIC Password Service
        /// </summary>
        public bool dynamicPassword { get; set; }
        /// <summary>
        /// CIC Password Service App Id
        /// </summary>
        public String appId { get; set; }
        /// <summary>
        /// CIC Password Service Address
        /// </summary>
        public String passwordServiceAddress { get; set; }
    }
}