using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für copyKonto Methode
    /// </summary>
    public class ocopyKontoDto : oBaseDto
    {
        /// <summary>
        /// Persistenzobjekt Konto
        /// </summary>
        public DTO.KontoDto konto
        {
            get;
            set;
        }
    }
}
