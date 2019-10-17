using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// InputParameter für copyAntrag Methode
    /// </summary>
    public class icopyAntragDto
    {
        /// <summary>
        /// Persistenzobjekt zu kopierender Antrag
        /// </summary>
        public DTO.AntragDto antrag
        {
            get;
            set;
        }
    }
}
