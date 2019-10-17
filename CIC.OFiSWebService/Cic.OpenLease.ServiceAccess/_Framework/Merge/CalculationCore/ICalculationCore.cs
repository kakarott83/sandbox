// OWNER MK, 19-03-2009
namespace Cic.OpenLease.ServiceAccess.Merge.CalculationCore
{
    /// <summary>
    /// Kalkulation Service. Dient dazu Kalkulationen zu durchfuhren. 
    /// </summary>
    [System.CLSCompliant(true)]
    [System.ServiceModel.ServiceContract(Name = "CalculationCore", Namespace = "http://cic-software.de/Cic.OpenLease.Service.Merge.CalculationCore")]
    public interface ICalculationCore
    {
        #region New

        [System.ServiceModel.OperationContract]
        CalculationDto Calculate(CalculationDto calculationDto);

 

        /// <summary>
        /// Not in use.
        /// </summary>
        /// <param name="residualValueRequestDto"><see cref="ResidualValueRequestDto"/></param>
        /// <returns>Restwert</returns>
        [System.ServiceModel.OperationContract]
        decimal DeliverResidualValue(ResidualValueRequestDto residualValueRequestDto);
        #endregion

        #region Obsolete
        
        #endregion
    }
}
