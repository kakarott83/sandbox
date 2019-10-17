using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// Schnellkalkulation Daten Zugriffsobjekt
    /// </summary>
    public class SchnellkalkulationDao : ISchnellkalkulationDao
    {
        /// <summary>
        /// Erzeuge eine neue Schnellkalkulation
        /// </summary>
        /// <returns>Kalkulation Daten Transfer Objekt</returns>
        public KalkulationDto CreateSchnellkalkulation()
        {
            KalkulationDto retval = new KalkulationDto();
            retval.angAntKalkDto = new AngAntKalkDto();
            retval.angAntProvDto = new List<AngAntProvDto>();
            retval.angAntSubvDto = new List<AngAntSubvDto>();
            retval.angAntVsDto = new List<AngAntVsDto>();
            return retval;
        }
    }
}
