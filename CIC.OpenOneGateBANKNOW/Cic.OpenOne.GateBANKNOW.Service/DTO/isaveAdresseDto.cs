using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für saveAdresse Methode
    /// </summary>
    class isaveAdresseDto
    {
        /// <summary>
        /// Persistenzobjekt Adresse
        /// </summary>
        public DTO.AdresseDto adresse
        {
            get;
            set;
        }
    }
}
