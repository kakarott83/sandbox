using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DTO.Versicherung;

namespace Cic.OpenOne.Common.BO.Versicherung
{
    /// <summary>
    /// Schnittstelle des Versicherungs BO
    /// </summary>
    public interface IInsuranceBo
    {
       /// <summary>
       /// calculates an insurance
       /// </summary>
       /// <param name="vs"></param>
       /// <param name="inputValue"></param>
        /// <param name="perDate">Eingestelltes Datum</param>
        /// <returns></returns>
        oInsuranceDto calculateInsurance(AngAntVsDto vs, iInsuranceDto inputValue, DateTime perDate);
    }
}
