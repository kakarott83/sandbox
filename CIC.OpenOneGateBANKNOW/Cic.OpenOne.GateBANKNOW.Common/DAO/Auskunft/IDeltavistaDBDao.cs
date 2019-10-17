using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    /// <summary>
    /// Deltavista DB Data Access Object
    /// </summary>
    public interface IDeltavistaDBDao
    {
        /// <summary>
        /// Gets Username and Password
        /// </summary>
        /// <returns>IdentityDescriptor</returns>
        DAO.Auskunft.DeltavistaRef.IdentityDescriptor GetIdentityDescriptor();

        /// <summary>
        /// Gets Username and Password
        /// </summary>
        /// <returns>IdentityDescriptor</returns>
        DAO.Auskunft.DeltavistaRef.IdentityDescriptor GetIdentityDescriptorArb();

        /// <summary>
        /// Save or get ID Descriptor
        /// </summary>
        /// <returns>Descriptor</returns>
        DAO.Auskunft.DeltavistaRef2.IdentityDescriptor GetIdDescriptor();

        /// <summary>
        /// Save or Get Identified Address Input
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto">In Data Acces Object</param>
        void SaveGetIdentifiedAddressInput(long sysAuskunft, DeltavistaInDto inDto);

        /// <summary>
        /// Save or get Company Details
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto">In Data Acces Object</param>
        void SaveGetCompanyDetailsInput(long sysAuskunft, DeltavistaInDto inDto);

        /// <summary>
        /// Save or Get Details Input
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto">In Data Acces Object</param>
        void SaveGetDebtDetailsInput(long sysAuskunft, DeltavistaInDto inDto);

        /// <summary>
        /// Save Order Cresura Report Input
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto">In Data Acces Object</param>
        void SaveOrderCresuraReportInput(long sysAuskunft, DeltavistaInDto inDto);

        /// <summary>
        /// Save or Get Report Input
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto">In Data Acces Object</param>
        void SaveGetReportInput(long sysAuskunft, DeltavistaInDto inDto);

        /// <summary>
        /// Save or get Identified Address Output
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto">Out Data Acces Object</param>
        void SaveGetIdentifiedAddressOutput(long sysAuskunft, DeltavistaOutDto outDto);

        /// <summary>
        /// Save or Get Company Details Output
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto">Out Data Acces Object</param>
        void SaveGetCompanyDetailsOutput(long sysAuskunft, DeltavistaOutDto outDto);

        /// <summary>
        /// Save or get Details Output
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto">Out Data Acces Object</param>
        void SaveGetDebtDetailsOutput(long sysAuskunft, DeltavistaOutDto outDto);

        /// <summary>
        /// Save or get Cresura report Output
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto">Out Data Acces Object</param>
        void SaveOrderCresuraReportOutput(long sysAuskunft, DeltavistaOutDto outDto);

        /// <summary>
        /// Save or get report Output
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto">Out Data Acces Object</param>
        void SaveGetReportOutput(long sysAuskunft, DeltavistaOutDto inDto);

        /// <summary>
        /// Find Input by SysID
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>in Data Acces Object</returns>
        DeltavistaInDto FindBySysId(long sysAuskunft);
    }
}