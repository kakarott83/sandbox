using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// Schnittstelle des Kalkulation Datenzugriffsobjekts
    /// </summary>
    public interface IKalkulationDao
    {
        /// <summary>
        /// Create a new Kalkulation Dto
        /// </summary>
        /// <param name="SysId">Primary Key des Angebots</param>
        /// <returns>KalkulationDto</returns>
        AngAntVarDto createKalkulation(long SysId);

        /// <summary>
        /// Return existing Kalkulation Dto 
        /// </summary>
        /// <param name="sysVar">Primary Key der Variante</param>
        /// <returns>KalkulationDto</returns>
        AngAntVarDto getKalkulation(long sysVar);

        /// <summary>
        /// Kalkulation aktualisieren
        /// </summary>
        /// <param name="kalkulation">Eingangs Kalkulation</param>
        /// <returns>Gespeicherte Kalkulation</returns>      
        AngAntVarDto updateKalkulation(AngAntVarDto kalkulation);

        /// <summary>
        /// Kalkulation aktualisieren
        /// </summary>
        /// <param name="sysVar">Primary Key der Variante</param>
        void deleteKalkulation(long sysVar);

       
       
    }
}
