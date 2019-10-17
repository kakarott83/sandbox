using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.getBuchwertService.performKaufofferte"/> Methode
    /// </summary>
    public class iperformKaufofferteDto
    {
        /// <summary>
        /// SysID des Antrags
        /// </summary>
        public long sysid { get; set; }

        /// <summary>
        /// isHaendler=true -> HD, else KD
        /// </summary>
        public bool isHaendler { get; set; }

        /// <summary>
        /// PerDatum-Code für den Buchwert (DDLKPPOS BW_BERECHNUNG_PER)
        /// </summary>
        public String perDatumCode { get; set; }
    }

}
