using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter TransactionRisikoprüfung Methode
    /// </summary>
    public class orisikoSimulationDto : oBaseDto
    {

        /// <summary>
        /// fitted rules
        /// </summary>
        public string[] simulationDERules
        {
            get;
            set;
        }
        public bool error { get; set; }

    }
}