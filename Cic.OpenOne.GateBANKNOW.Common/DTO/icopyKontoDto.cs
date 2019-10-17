using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// InputParameter für copyKonto Methode
    /// </summary>
    public class icopyKontoDto
    {
        /// <summary>
        /// Persistenzobjekt zu kopierendes Konto
        /// </summary>
        public DTO.KontoDto konto
        {
            get;
            set;
        }
    }
}
