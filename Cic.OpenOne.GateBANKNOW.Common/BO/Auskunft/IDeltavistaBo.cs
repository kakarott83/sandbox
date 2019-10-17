using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    /// <summary>
    /// Deltavista Business Object Schnittstelle
    /// </summary>
    public interface IDeltavistaBo
    {
        /// <summary>
        /// getIdentifiedAddress
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto getIdentifiedAddress(DeltavistaInDto inDto);

        /// <summary>
        /// getIdentifiedAddressArb
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto getIdentifiedAddressArb(DeltavistaInDto inDto);

        /// <summary>
        /// getCompanyDetailsByAddressId
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto getCompanyDetailsByAddressId(DeltavistaInDto inDto);

        /// <summary>
        /// getDebtDetailsByAddressId
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto getDebtDetailsByAddressId(DeltavistaInDto inDto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto orderCresuraReport(DeltavistaInDto inDto);

        /// <summary>
        /// getReport
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto getReport(DeltavistaInDto inDto);

        /// <summary>
        /// getIdentifiedAddress
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto getIdentifiedAddress(long sysAuskunft);

        /// <summary>
        /// getIdentifiedAddressArb
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto getIdentifiedAddressArb(long sysAuskunft);

        /// <summary>
        /// getCompanyDetailsByAddressId
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto getCompanyDetailsByAddressId(long sysAuskunft);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto getDebtDetailsByAddressId(long sysAuskunft);

        /// <summary>
        /// orderCresuraReport
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto orderCresuraReport(long sysAuskunft);

        /// <summary>
        /// getReport
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto getReport(long sysAuskunft);
    }
}