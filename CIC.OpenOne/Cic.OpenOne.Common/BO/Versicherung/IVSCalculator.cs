using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO.Versicherung;

namespace Cic.OpenOne.Common.BO.Versicherung
{
    /// <summary>
    /// Interface for an InsuranceCalculator
    /// </summary>
    public interface IVSCalculator
    {
        /// <summary>
        /// Calculates the given Insurance
        /// </summary>
        /// <param name="param"></param>
        /// <param name="perDate">Eingestelltes Datum</param>
        /// <returns></returns>
        oInsuranceDto calculate(iInsuranceDto param, DateTime perDate);
    }
}
