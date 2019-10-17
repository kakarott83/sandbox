using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// OutputParameter für copyAntrag Methode
    /// </summary>
    public class ocopyAntragDto
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
        /// Persistenzobjekt Antrag
        /// </summary>
        public DTO.AntragDto antrag
        {
            get;
            set;
        }
    }
}
