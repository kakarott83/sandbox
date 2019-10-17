using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// OutputParameter für copyKonto Methode
    /// </summary>
    public class ocopyKontoDto
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
        /// Persistenzobjekt Konto
        /// </summary>
        public DTO.KontoDto konto
        {
            get;
            set;
        }
    }
}
