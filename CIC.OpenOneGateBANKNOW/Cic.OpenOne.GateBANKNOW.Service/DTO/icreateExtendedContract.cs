using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    public class icreateExtendedContract
    {
        /// <summary>
        /// Sysid Vorvertrag
        /// </summary>
        public long sysVorvertrag { get; set; }

        /// <summary>
        /// Clienttyp (10=B2B, 20=MA, 30=B2C, 50=ONE)
        /// </summary>
        public int? wsclient { get; set; }
    }
}