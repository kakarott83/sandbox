using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// InputParameter für Risikoprüfung Simulation
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

        public AngAntKalkDto angAntKalkDto
        {
            get;
            set;
        }

        public DecisionEngineInDto inputDE
        {
            get;
            set;
        }

      
    }
}
