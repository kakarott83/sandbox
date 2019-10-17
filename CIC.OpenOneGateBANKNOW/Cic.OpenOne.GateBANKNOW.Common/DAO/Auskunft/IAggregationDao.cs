using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    /// <summary>
    /// Aggregation Data Access Object
    /// </summary>
    public interface IAggregationDao
    {
        /// <summary>
        /// Aggregiert OpenLease Daten über die SysID des Antrags
        /// </summary>
        /// <param name="AuskunftId">Information ID</param>
        /// <param name="SysAntragId"></param>
        /// <returns>OpenLeaseAggregationsdatenDto</returns>
        AggregationOLOutDto GetOLDatenBySysAntrag(long AuskunftId, long SysAntragId);

        /// <summary>
        /// Aggregiert DeltaVista Daten über die SysID des Antrags
        /// </summary>
        /// <param name="AuskunftId">Information ID</param>
        /// <param name="SysAntragId"></param>
        /// <returns>AggregationDVOutDto</returns>
        AggregationDVOutDto GetDVDatenBySysAntrag(long AuskunftId, long SysAntragId);

        /// <summary>
        /// Aggregiert ZEK Daten über die SysID des Antrags
        /// </summary>
        /// <param name="AuskunftId">Information ID</param>
        /// <param name="SysAntragId"></param>
        /// <returns>AggregationZekOutDto</returns>
        AggregationZekOutDto GetZEKDatenBySysAntrag(long AuskunftId, long SysAntragId);

        /// <summary>
        /// Aggregiert VP Daten über die SysID des Antrags
        /// </summary>
        /// <param name="AuskunftId">Information ID</param>
        /// <param name="SysAntragId"></param>
        /// <returns>AggregationVPOutDto</returns>
        AggregationVPOutDto GetVPDatenBySysAntrag(long AuskunftId, long SysAntragId);

        /// <summary>
        /// Save Aggregation Input
        /// </summary>
        /// <param name="sysAuskuft">Information ID</param>
        /// <param name="inDto">AggregationInDto</param>
        void SaveAggregationInDto(long sysAuskuft, AggregationInDto inDto);

        /// <summary>
        /// Save Aggregation Output
        /// </summary>
        /// <param name="outDto">Output Data</param>
        /// <param name="sysAuskunft">Information ID</param>
        void SaveAggregationOutDto(AggregationOutDto outDto, long sysAuskunft);

        /// <summary>
        /// Find by Information ID
        /// </summary>
        /// <param name="sysAuskunft">Infomration ID</param>
        /// <returns>Input Data</returns>
        AggregationInDto FindBySysId(long sysAuskunft);

        /// <summary>
        /// GetAntragstellerInfo
        /// </summary>
        /// <param name="auskunftId"></param>
        /// <param name="sysAntragId"></param>
        /// <returns></returns>
        void GetAntragstellerInfo(long auskunftId, long sysAntragId);
    }
}