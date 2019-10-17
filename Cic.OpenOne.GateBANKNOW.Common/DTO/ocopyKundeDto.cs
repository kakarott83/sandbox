using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// OutputParameter für copyKunde Methode
    /// </summary>
    public class ocopyKundeDto
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
        /// Persistenzobjekt Kunde
        /// </summary>
        public DTO.KundeDto kunde
        {
            get;
            set;
        }
    }
}
