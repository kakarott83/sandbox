using Cic.OpenLease.Service.Services.DdOl;
using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenOne.Common.Model.DdOl;
using CIC.Database.OL.EF6.Model;

namespace Cic.OpenLease.Service.Provision
{
    /// <summary>
    /// Abstract Base Class of all provision calculators
    /// </summary>
    [System.CLSCompliant(true)]
    public abstract class AbstractProvisionCalculator : IProvisionCalculator
    {

        public abstract ProvisionDto calculate(DdOlExtended context, PROVDao prov, ProvisionDto param);
        /// <summary>
        /// Return true if the product has a vart of kind leasing
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sysprproduct"></param>
        /// <returns></returns>
        protected bool isLeasing(DdOlExtended context, long sysprproduct)
        {
            CalculationDao calcDao = new CalculationDao(context);
            VartDTO va = calcDao.getVART(sysprproduct);

            if (va == null) return false;
            return (va.CODE.IndexOf("LEASING") > -1);
        }
    }
}