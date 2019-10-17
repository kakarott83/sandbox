using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// Enumerates all possible Antrag check Messages
    /// </summary>
    public enum checkAntragMessages
    {
        /// <summary>
        /// Maximales Kreditlimit
        /// </summary>
        MAX_KREDITLIMIT

    }
    /// <summary>
    /// Output für checkAntrag
    /// </summary>
    public class ocheckAntAngDto
    {
        /// <summary>
        /// Status Rot
        /// </summary>
        public static String STATUS_RED = "rot";
        /// <summary>
        /// Status Gruen
        /// </summary>
        public static String STATUS_GREEN = "gruen";
        /// <summary>
        /// Status Gelb
        /// </summary>
        public static String STATUS_YELLOW = "gelb";

        public static String STATUS_YELLOW_ONLY_FEL1 = "gelb_nur_FL1";

        /// <summary>
        /// Status (rot, gruen, gelb)
        /// </summary>
        public string status { get; set; }

        /// <summary>
        ///  Code der getroffenen Regel 
        /// </summary>
        public List<string> code { get; set; }

        /// <summary>
        /// Liste mit Fehlermeldungen
        /// </summary>
        public List<string> errortext { get; set; }



    }
}
