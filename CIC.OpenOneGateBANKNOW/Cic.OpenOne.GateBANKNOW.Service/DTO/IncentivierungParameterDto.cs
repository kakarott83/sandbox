using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// incentivation level
    /// </summary>
    public enum IncentivierungStufe
    {
        Basic,
        Bronze,
        Silber,
        Gold,
        Platin,
    }

    /// <summary>
    /// Values of commissions depending on the current incentivation level
    /// </summary>
    public class IncentivierungParameterDto : Common.DTO.IncentivierungParameterDto
    {
    }

    /// <summary>
    /// Matrix for commission calculation depending on current incentivation level
    /// </summary>
    public class IncentivierungMatrixDto : Common.DTO.IncentivierungMatrixDto
    {
    }
}