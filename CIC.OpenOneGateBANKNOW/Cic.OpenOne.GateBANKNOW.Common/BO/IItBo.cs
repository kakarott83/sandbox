using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Iinterface for Conversions from Angebot to Antrag
    /// </summary>
    public interface IItBo
    {
        /// <summary>
        /// Interessent ändern
        /// </summary>
        /// <param name="ang">Kundendaten</param>
        /// <returns>Neue Interessentendaten</returns>
        KundeDto updateIt(KundeDto ang);
        
    }
}
