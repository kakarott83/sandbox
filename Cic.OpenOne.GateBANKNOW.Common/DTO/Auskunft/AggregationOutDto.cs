
namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// Dto für aggregierte Daten aus OpenLease
    /// </summary>
    public class AggregationOutDto
    {
        /// <summary>
        /// Getter/Setter Return code
        /// </summary>
        public long ReturnCode { get; set; }

        /// <summary>
        /// Aggregation OpenLease OutDto
        /// </summary>
        public AggregationOLOutDto aggOLOutDto { get; set; }

        /// <summary>
        /// Aggregation VertragsPartner OutDto
        /// </summary>
        public AggregationVPOutDto aggVPOutDto { get; set; }

        /// <summary>
        /// Aggregation DeltaVista OutDto
        /// </summary>
        public AggregationDVOutDto aggDVOutDto { get; set; }

        /// <summary>
        /// Aggregation ZEK OutDto
        /// </summary>
        public AggregationZekOutDto aggZEKOutDto { get; set; }
    }
}
