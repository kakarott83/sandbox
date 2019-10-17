using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Output für listAvailableInsurance
    /// </summary>
    public class olistAvailableInsuranceDto : oBaseDto
    {
        /// <summary>
        /// Array der Versicherungen
        /// </summary>
        public InsuranceDto[] versicherung
        {
            get;
            set;
        }
    }
}