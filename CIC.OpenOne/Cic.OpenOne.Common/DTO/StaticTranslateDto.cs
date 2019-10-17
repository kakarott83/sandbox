using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO
{
    /// <summary>
    /// Static Translation Dto
    /// </summary>
    public class StaticTranslateDto
    {
        /// <summary>
        /// Front ID
        /// </summary>
        public String frontid { get; set; }

        /// <summary>
        /// Typ
        /// </summary>
        public int typ {get; set;}

        /// <summary>
        /// Master-Phrase
        /// </summary>
        public string master {get; set;}

        /// <summary>
        /// Üebersetzung
        /// </summary>
        public string translation {get; set;}

        /// <summary>
        /// Übersetzungs-Blob 
        /// </summary>
        public byte[] replaceblob {get; set;}

        /// <summary>
        /// ISO Code
        /// </summary>
        public string isocode {get; set;}
    }
}
