using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für Risikoprüfungsimulation Methode
    /// </summary>
    public class irisikoSimDto
    {  
        /// <summary>
        /// antrag
        /// </summary>
        public DTO.AntragDto antrag
        {
            get;
            set;
        }

        /// <summary>
        /// _Expected Loss
        /// </summary>
        public ELOutDto eloutDto
        {
            get;
            set;
        }
    }
}