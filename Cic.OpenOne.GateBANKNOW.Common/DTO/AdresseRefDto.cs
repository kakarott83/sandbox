using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// DTO Adress Referenz
    /// </summary>
    public class AdresseRefDto
    {
        /// <summary>
        /// PKEY 
        /// </summary>
        public long sysadrref { get; set; }
        /// <summary>
        /// Verweis zur Adresse 
        /// </summary>
        public long sysadresse { get; set; }
        /// <summary>
        /// Verweis zum Adresstyp 
        /// </summary>
        public long sysadrtp { get; set; }
        /// <summary>
        /// Verweis zumInteressent 
        /// </summary>
        public long sysit { get; set; }
        /// <summary>
        /// Verweis zur Person 
        /// </summary>
        public long sysperson { get; set; }
        /// <summary>
        /// Verweis zum Angebot 
        /// </summary>
        public long sysangebot { get; set; }
        /// <summary>
        /// Verweis zum Antrag 
        /// </summary>
        public long sysantrag { get; set; }

    }
}
