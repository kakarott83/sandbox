using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.Util;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// Defines globally valid registry paths for use in java clients as constant
    /// </summary>
    public class RegVarPaths
    {
        private static RegVarPaths instance;
        private static string LOCK = "LOCK";
        private RegVarPaths() { }
        public static RegVarPaths getInstance() 
        {
            lock (LOCK)
            {
                if (instance == null)
                    instance = new RegVarPaths();
            }
            return instance;
           
        }

        public String RECENTLIST { get { return "/USERS/DATA/RECENT/"; } set { } }
        public String USRCACHE { get { return "/USERS/USRCACHE/"; } set { } }
        public String FAVORITES { get { return "/USERS/USRCACHE/FAVORITES/"; } set { } }
        public String NOTES { get { return "/USERS/DATA/NOTES/"; } set { } }
        public String FOLLOWS { get { return "/USERS/DATA/FOLLOWS/"; } set { } }
        public String SIGHTFIELDS { get { return "/SETUP.NET/GESICHTSKREIS/"; } set { } }
        public String CHAT { get { return "/SETUP.NET/CHAT/"; } set { } }
        public String EXCHANGE { get { return "/USERS/EXCHANGE/"; } set { } }
        public String LUCENE { get { return "/GLOBAL/LUCENE/"; } set { } }
        public String REPORTS { get { return "/GLOBAL/REPORTS/"; } set { } }
       
    }
}