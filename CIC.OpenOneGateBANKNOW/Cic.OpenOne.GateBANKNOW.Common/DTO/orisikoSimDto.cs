using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// Outputparameter für Risikoprüfung Simulation
    /// </summary>
    public class orisikoSimDto
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
        /// Frontid results
        /// </summary>
        public string frontid { get; set; }

        /// <summary>
        /// Rules
        /// </summary>
        public string [] simulationDERules
        {
            get;
            set;
        }
    }
}
