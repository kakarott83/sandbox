using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// InputParameter für createOrUpdateKundeService.createKunde Methode
    /// </summary>
    public class icreateKundeDto
    {
        /// <summary>
        /// ID des Kunden (leer für neuen Kunden)
        /// </summary>
        public long sysID
        {
            get;
            set;
        }
    }
}
