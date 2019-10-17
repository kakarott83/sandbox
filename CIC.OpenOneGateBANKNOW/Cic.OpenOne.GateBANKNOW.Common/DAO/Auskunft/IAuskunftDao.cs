using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using System;
using System.Collections.Generic;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    /// <summary>
    /// Information Data Access Object Interface
    /// </summary>
    public interface IAuskunftDao
    {
        /// <summary>
        /// getUnprocessedBatchAuskunft
        /// </summary>
        /// <returns></returns>
        AuskunftDto getUnprocessedBatchAuskunft();

        /// <summary>
        /// getUnprocessedBatchAuskunft
        /// </summary>
        /// <returns></returns>
        List<AuskunftDto> getUnprocessedBatchAuskunfte();

        /// <summary>
        /// Hole das DebugFlag aus AuskunftTyp
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        bool getLogdumpFlag(long sysAuskunft);

        /// <summary>
        /// Save Information Data
        /// </summary>
        /// <param name="auskunfttyp">type</param>
        /// <returns>result</returns>
        long SaveAuskunft(string auskunfttyp);

        /// <summary>
        /// Update Information
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <param name="returnCode">return code</param>
        void UpdateAuskunft(long sysAuskunft, long returnCode);

        /// <summary>
        /// UpdateAuskunftDtoAuskunft
        /// </summary>
        /// <param name="auskunftDto">auskunftDto</param>
        /// <param name="returnCode">returnCode</param>
        void UpdateAuskunftDtoAuskunft(AuskunftDto auskunftDto, long returnCode);

        /// <summary>
        /// Find by ID
        /// </summary>
        /// <param name="sysId">ID</param>
        /// <returns>Information Data</returns>
        AuskunftDto FindBySysId(long sysId);

        /// <summary>
        /// Get Information description
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <returns>Description</returns>
        string GetAuskunfttypBezeichng(long sysAuskunft);

        /// <summary>
        /// UpdateAuskunftDtoAuskunft
        /// </summary>
        /// <param name="auskunftDto">auskunftDto</param>
        /// <param name="returnCode">returnCode</param>
        /// <param name="text">text</param>
        void UpdateAuskunftDtoAuskunft(AuskunftDto auskunftDto, long returnCode, string text);

        /// <summary>
        /// getAuskunftHostcomputer
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        /// <returns></returns>
        string getAuskunftHostcomputer(long sysAuskunft);

        /// <summary>
        /// setAuskunftHostcomputer
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        void setAuskunftHostcomputer(long sysAuskunft);

        /// <summary>
        /// setAuskunftWfuser
        /// </summary>
        /// <param name="sysAuskunft">sysAuskunft</param>
        void setAuskunftWfuser(long sysAuskunft, long syswfuser);

        /// <summary>
        /// setAuskunftAreaUndId
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="area"></param>
        /// <param name="sysid"></param>
        void setAuskunfAreaUndId(long sysAuskunft, string area, long sysid);

        /// <summary>
        /// getEntitySoapLog
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        Cic.OpenOne.Common.DTO.SoapXMLDto getEntitySoapLog(long sysAuskunft);

        /// <summary>
        /// getEntitySoapLogEurotax
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="bezeichnung"></param>
        /// <returns></returns>
        Cic.OpenOne.Common.DTO.SoapXMLDto getEntitySoapLogEurotax(long sysid, string bezeichnung);

        /// <summary>
        /// getEntitySoapLogEurotaxVin
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="bezeichnung"></param>
        /// <returns></returns>
        Cic.OpenOne.Common.DTO.SoapXMLDto getEntitySoapLogEurotaxVin(long sysid, string bezeichnung);

        /// <summary>
        /// FindByAreaSysId
        /// </summary>
        /// <param name="sysId"></param>
        /// <param name="area"></param>
        /// <param name="auskunfttyp"></param>
        /// <returns></returns>
        AuskunftDto FindByAreaSysId(long sysId, string area, string auskunfttyp);

        /// <summary>
        /// Sets the statusnum Field to 1 (invalid) for all suitable auskunft entries
        /// </summary>
        /// <param name="sysId"></param>
        /// <param name="area"></param>
        /// <param name="auskunfttyp"></param>
        void invalidateAuskunft(long sysId, string area, string auskunfttyp);

        /// <summary>
        /// Sets the auskunft STATUSNUM Field
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="status"></param>
        void setAuskunfStatusNum(long sysAuskunft, short status);


        /// <summary>
        /// Delivers the auskunft configuration
        /// </summary>
        /// <param name="bezeichnung"></param>
        /// <returns></returns>
        AuskunftCFGDto getConfiguration(String bezeichnung);
    }
}