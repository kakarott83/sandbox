using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für copyAdresse Methode
    /// </summary>
    public class ocopyAdresseDto : oBaseDto
    {
        /// <summary>
        /// Persistenzobjekt Adresse
        /// </summary>
        public AdresseDto adresse
        {
            get;
            set;
        }
    }
}
