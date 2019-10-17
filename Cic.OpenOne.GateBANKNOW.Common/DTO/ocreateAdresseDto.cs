using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// OutputParameter für createAdresse Methode
    /// </summary>
    public class ocreateAdresseDto
    {
        /// <summary>
        /// Allgemeines Messageobjekt
        /// </summary>
        public DTO.Message message
        {
            get;
            set;
        }

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
