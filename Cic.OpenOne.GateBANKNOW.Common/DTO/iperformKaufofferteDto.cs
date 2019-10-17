using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{

    /// <summary>
    /// InputParameter getBuchwertService.performKaufofferte Methode
    /// </summary>
    public class iperformKaufofferteDto
    {
        /// <summary>
        /// SysID des Antrags/OFFERTE
        /// </summary>
        public long sysid { get; set; }

        /// <summary>
        /// isHaendler=true -> HD, else KD
        /// </summary>
        public bool isHaendler { get; set; }

       
        /// <summary>
        ///  HD
        /// </summary>
        public long sysPerole { get; set; }

        /// <summary>
        /// ID des Mitarbeiter zum Händler, der die Offerte ausgelöst hat
        /// </summary>
        public long mittarbeiter { get; set; }

        /// <summary>
        /// herkunft 
        /// </summary>
        public string herkunft { get; set; }

        /// <summary>
        /// sysWFUser
        /// </summary>
        public long sysWFUser { get; set; }

        /// <summary>
        /// PerDatum-Code für den Buchwert (DDLKPPOS BW_BERECHNUNG_PER)
        /// </summary>
        public String perDatumCode { get; set; }
    }

}
