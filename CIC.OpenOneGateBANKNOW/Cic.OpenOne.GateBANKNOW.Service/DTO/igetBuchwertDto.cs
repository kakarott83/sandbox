using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.getBuchwertService.getBuchwert"/> Methode
    /// </summary>
    public class igetBuchwertDto
    {
        /// <summary>
        /// SysID des Antrags
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
    }

}
