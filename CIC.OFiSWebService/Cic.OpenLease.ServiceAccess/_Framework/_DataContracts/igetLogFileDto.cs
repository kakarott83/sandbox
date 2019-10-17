using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenLease.ServiceAccess.Merge.DTO
{
    /// <summary>
    /// Input Dto vom getLogFile
    /// </summary>
    public class igetLogFileDto
    {
        /// <summary>
        /// PID des Logfiles
        /// </summary>
        public int pid { get; set; }

        /// <summary>
        /// Zeitraum der Auswertung von
        /// </summary>
        public DateTime vonDatum { get; set; }

        /// <summary>
        /// Zeitraum der Auswertung bis
        /// </summary>
        public DateTime bisDatum { get; set; }

        /// <summary>
        /// Auf bestimmten Service
        /// </summary>
        public string service { get; set; }

        /// <summary>
        /// auf bestimmte Methode
        /// </summary>
        public string method { get; set; }
    }
}