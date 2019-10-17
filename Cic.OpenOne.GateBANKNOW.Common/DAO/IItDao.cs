using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// Interessent Datenzugriffsklasse
    /// </summary>
    public interface IItDao
    {
        /// <summary>
        /// Interessenten ändern
        /// </summary>
        /// <param name="ang">Neue Daten</param>
        /// <returns>Geänderte Daten</returns>
        KundeDto updateIt(KundeDto ang);
        
    }
}