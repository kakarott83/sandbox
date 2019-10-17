using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für TransactionRisikoprüfung Methode
    /// </summary>
    public class icheckTrRiskDto
    {
        /// <summary>
        /// antrag
        /// </summary>
        public DTO.AntragDto antrag
        {
            get;
            set;
        }


    }
}