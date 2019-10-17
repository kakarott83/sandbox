using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// Dto für aggregierte Daten von VertriebsPartner
    /// </summary>
    public class AggregationVPOutDto
    {
        // <summary>
        // 
        // </summary>
        // public int SysAggOutVP { get; set; }               //       NUMBER(12,0), 

        /// <summary>
        /// Volumenengagement
        /// </summary>
        public decimal? Volumen { get; set; }                 //   NUMBER(15,2)

        /// <summary>
        /// Eventualvolumenengagement
        /// </summary>
        public decimal? EventualVolumen { get; set; }           //           NUMBER(15,2)

        /// <summary>
        /// Restwertengagement
        /// </summary>
        public decimal? Restwert { get; set; }                  //    NUMBER(15,2)

        /// <summary>
        /// Eventualrestwertengagement
        /// </summary>
        public decimal? EventualRestwert { get; set; }          //            NUMBER(15,2)

        /// <summary>
        /// Anzahl Anträge
        /// </summary>
        public int? AnzAT { get; set; }                     // NUMBER(5,0)

        /// <summary>
        /// Anzahl pendente Anträge
        /// </summary>
        public int? AnzATP { get; set; }                    //  NUMBER(5,0)

        /// <summary>
        /// Anzahl Verträge
        /// </summary>
        public int? AnzVT { get; set; }                     // NUMBER(5,0)

        /// <summary>
        /// Anzahl laufende Verträge
        /// </summary>
        public int? AnzVTL { get; set; }                    //  NUMBER(5,0)

        /// <summary>
        /// Wenn die dem Händler zugehörige prhgroup 'STRAT' enthält, dann 1 sonst 0
        /// </summary>
        public int? STRATEGICACCOUNT { get; set; }                    //  NUMBER(5,0)
    }
}
