using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Outputparameter für Risikoprüfung Simulation Interop
    /// </summary>
    public class orisikoSimIODto
    {
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
