using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für copyAdresse Methode
    /// </summary>
    public class icopyAdresseDto
    {
        /// <summary>
        /// Persistenzobjekt zu kopierende Adresse
        /// </summary>
        public DTO.AdresseDto adresse
        {
            get;
            set;
        }
    }
}
