using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.searchKundeService.getKundeDetail"/> Methode
    /// </summary>
    public class igetKundeDetailDto
    {
        /// <summary>
        /// Kunden Id
        /// </summary>
        public long syskunde
        {
            get;
            set;
        }

        /// <summary>
        /// Mitantragsteller für Kunde laden
        /// </summary>
        public int mitantragsteller
        {
            get;
            set;
        }

    }
}
