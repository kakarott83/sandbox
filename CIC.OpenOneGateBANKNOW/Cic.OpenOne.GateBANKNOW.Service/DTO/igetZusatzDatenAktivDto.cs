using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für
    /// </summary>
    public class igetZusatzDatenAktivDto 
    {
        /// <summary>
        /// Interessent Id
        /// </summary>
        public long sysit
        {
            get;
            set;
        }

        /// <summary>
        /// Kundentyp
        /// </summary>
        public int kdtyp
        {
            get;
            set;
        }
    }
}