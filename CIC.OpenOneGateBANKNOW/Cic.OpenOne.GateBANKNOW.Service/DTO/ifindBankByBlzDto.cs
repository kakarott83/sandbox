using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DAO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// InputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateKundeService"/> Methode
    /// </summary>
    public class ifindBankByBlzDto
    {
        /// <summary>
        /// BLZ (BC or PC)
        /// </summary>
        public String bcpcNummer
        {
            get;
            set;
        }

        /// <summary>
        /// KontoNr
        /// </summary>
        public String kontoNr
        {
            get;
            set;
        }
    }
}
