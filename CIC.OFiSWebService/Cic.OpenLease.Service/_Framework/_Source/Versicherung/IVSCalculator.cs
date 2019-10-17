using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenOne.Common.Model.DdOl;
using CIC.Database.OL.EF6.Model;

namespace Cic.OpenLease.Service.Versicherung
{
    /// <summary>
    /// Interface for an InsuranceCalculator
    /// </summary>
    public interface IVSCalculator
    {
        /// <summary>
        /// calculates InsuranceResults
        /// </summary>
        /// <param name="context">DB Context</param>
        /// <param name="vstyp">VSTYP Entity</param>
        /// <param name="sysBrand">Brand </param>
        /// <param name="sysPerole">Perole</param>
        /// <param name="param">Insurance Parameters</param>
        /// <returns>Insurance Results</returns>
        InsuranceResultDto calculate(DdOlExtended context, long sysPerole, long sysBrand, VSTYP vstyp, InsuranceParameterDto param);
    }
}