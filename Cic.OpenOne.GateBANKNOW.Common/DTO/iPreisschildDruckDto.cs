using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    public class iPreisschildDruckDto
    {

        /// <summary>
        /// SysID des Antrags/OFFERTE
        /// </summary>
        public long sysid { get; set; }

        /// <summary>
        /// ISOLanguageCode
        /// </summary>
        public string ISOLanguageCode { get; set; }

        /// <summary>
        ///  sysAngKalk 
        /// </summary>
        public long sysAngVar { get; set; }

        /// <summary>
        /// herkunft /B2B
        /// </summary>
        public string herkunft { get; set; }

        /// <summary>
        /// preisInklusiv
        /// </summary>
        public bool preisInklusive { get; set; }


    }
}
