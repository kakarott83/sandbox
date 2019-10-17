using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    /// <summary>
    /// Aggregation Bo Interface
    /// </summary>
    public interface IAggregationBo
    {
        /// <summary>
        /// interface method to fill input values for Aggregation 
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns>AggregationOutDto filled with Aggregation return values</returns>
        AuskunftDto callByValues(AggregationInDto inDto);

        /// <summary>
        /// callByValues
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto callByValues(long sysAuskunft);
    }
}