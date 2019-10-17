using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für saveKonto Methode
    /// </summary>
    public class isaveKontoDto
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
