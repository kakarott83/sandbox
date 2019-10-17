using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// 
    /// </summary>
    public class ZekeCode178Dto
    {
        /// <summary>
        /// Getter/Setter Ecode178id
        /// </summary>
        public string Ecode178id { get; set; }

        /// <summary>
        /// Getter/Setter Fzstammnummer
        /// </summary>
        public string Fzstammnummer { get; set; }

        /// <summary>
        /// Getter/Setter Ecodestatus
        /// </summary>
        public string Ecodestatus { get; set; }

        /// <summary>
        /// Getter/Setter Haendlenummer
        /// </summary>
        public string Haendlenummer { get; set; }

        /// <summary>
        /// Getter/Setter Chassisnummer
        /// </summary>
        public string Chassisnummer { get; set; }

        /// <summary>
        /// Getter/Setter Datumgueltigbis
        /// </summary>
        public string Datumgueltigbis { get; set; } //Format: YYYY-MM-DD

        /// <summary>
        /// Getter/Setter Datumgueltigab
        /// </summary>
        public string Datumgueltigab { get; set; }  //Format: YYYY-MM-DD

        /// <summary>
        /// Getter/Setter nextEcodeStateField
        /// </summary>
        public string NextEcodeState{ get; set; }  

        /// <summary>
        /// Getter/Setter stvaNummerField
        /// </summary>
        public string StvaNummer { get; set; }  
    }
}
