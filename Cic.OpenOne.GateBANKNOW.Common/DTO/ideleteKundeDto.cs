using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// InputParameter für deleteKunde Methode
    /// </summary>
    public class ideleteKundeDto
    {
        /// <summary>
        /// ID des Kunden
        /// </summary>
        public long sysID
        {
            get;
            set;
        }
    }
}
