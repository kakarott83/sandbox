using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenOne.Common.Model.DdOl;

namespace Cic.OpenLease.Service.Provision
{
    /// <summary>
    /// Interface for an Provision Calculator
    /// </summary>
    public interface IProvisionCalculator
    {

        ProvisionDto calculate(DdOlExtended context, PROVDao prov, ProvisionDto param);
    }
}