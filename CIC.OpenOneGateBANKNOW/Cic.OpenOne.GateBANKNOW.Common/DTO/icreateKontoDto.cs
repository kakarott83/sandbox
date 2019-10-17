using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// InputParameter für createKonto> Methode
    /// </summary>
    public class icreateKontoDto
    {
        /// <summary>
        /// ID des Kontos (leer für neues Konto)
        /// </summary>
        public long sysID
        {
            get;
            set;
        }
    }
}
