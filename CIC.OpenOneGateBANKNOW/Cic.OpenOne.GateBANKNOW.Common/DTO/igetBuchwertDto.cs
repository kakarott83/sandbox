using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// InputParameter für getBuchwert Methode
    /// </summary>
    public class igetBuchwertDto
    {
         /// <summary>
        /// SysID des Vertrages
        /// </summary>
        public long sysid { get; set; }

        /// <summary>
        /// PerDatum für den Buchwert
        /// </summary>
        public DateTime perDatum { get; set; }

        /// <summary>
        /// Sprache
        /// </summary>
        public String sprache { get; set; }

        /// <summary>
        /// PerDatum-Code für den Buchwert (DDLKPPOS BW_BERECHNUNG_PER)
        /// </summary>
        public String perDatumCode { get; set; }

        /// <summary>
        /// aktuelle SYSVTRUEK der Buchwertberechnung
        /// </summary>
        public long currentSysVtruek { get; set; }

        /// <summary>
        /// SysEaihfile zu PDF Dokument Buchwertberechnung
        /// </summary>
        public long activeOfferSysEaihfile { get; set; }

        //current user role initiating the calculation
        public long sysPerole { get; set; }
    }

}
