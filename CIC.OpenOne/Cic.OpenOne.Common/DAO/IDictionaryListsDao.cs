using System;
using Cic.OpenOne.Common.DTO;
using System.Collections.Generic;

namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// defines the interface for DAOs providing dictionary lists
    /// </summary>
    public interface IDictionaryListsDao
    {
        /// <summary>
        /// finds a dictionary entry in DDLKPPOS by its CODE
        /// </summary>
        /// <param name="code">the CODE of the dictionary entries</param>
        /// <param name="isoCode">the isoCode of the language</param>
        /// <param name="domainId">the domainId</param>
        /// <param name="stringid">when the ddlkppos.id is a string and no number, set to true, the unique id will then be the database key</param>
        /// <returns>array of DropListDtos</returns>
        DropListDto[] findByDDLKPPOSCode(DDLKPPOSType code, String isoCode, String domainId, bool stringid);

        /// <summary>
        /// finds a dictionary entry in DDLKPPOS by its CODE
        /// </summary>
        /// <param name="code">the CODE of the dictionary entries as string</param>
        /// <param name="isoCode">the isoCode of the language</param>
        /// <param name="domainId">the domainId</param>
        /// <param name="stringid">when the ddlkppos.id is a string and no number, set to true, the unique id will then be the database key</param>
        /// <returns>array of DropListDtos</returns>
        DropListDto[] findByDDLKPPOSCode(String code, String isoCode, String domainId, bool stringid);

        /// <summary>
        /// finds a dictionary entry in DDLKPPOS by its CODE
        /// </summary>
        /// <param name="code">the CODE of the dictionary entries</param>
        /// <param name="isoCode">the isoCode of the language</param>
        /// <param name="domainId">the domainId</param>
        /// <returns>array of DropListDtos</returns>
        DropListDto[] findByDDLKPPOSCode(DDLKPPOSType code, String isoCode, String domainId);

        /// <summary>
        /// finds a dictionary entry in DDLKPPOS by its CODE, domainid assumed to be null
        /// </summary>
        /// <param name="code">the CODE of the dictionary entries</param>
        /// <param name="isoCode">the isoCode of the language</param>
        /// <returns>array of DropListDtos</returns>
        DropListDto[] findByDDLKPPOSCode(DDLKPPOSType code, String isoCode);

        /// <summary>
        /// Delivers all Aufbau Codes 
        /// </summary>
        /// <param name="isoCode"></param>
        /// <returns></returns>
        DropListDto[] deliverAufbau(String isoCode);

        /// <summary>
        /// Delivers all Treibstoffart Codes 
        /// </summary>
        /// <param name="isoCode"></param>
        /// <returns></returns>
        DropListDto[] deliverTreibstoffart(String isoCode);

        /// <summary>
        /// Delivers all Getriebeart Codes 
        /// </summary>
        /// <param name="isoCode"></param>
        /// <returns></returns>
        DropListDto[] deliverGetriebeart(String isoCode);
		
		/// <summary>
		/// Delivers all SlaPause Codes 
		/// </summary>
		/// <param name="isoCode"></param>
		/// <returns></returns>
		DropListDto[] deliverSlaPause (String isoCode);

        /// <summary>
        /// delivers a list of STAATen
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        DropListDto[] deliverSTAAT();

        /// <summary>
        /// delivers a list of countrys
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        DropListDto[] deliverLAND();

        /// <summary>
        /// Delivers a list of countrys (Nationalitäten)
        /// </summary>
        /// <returns>list of countrys</returns>
        DropListDto[] deliverNATIONALITIES();

        /// <summary>
        /// delivers a list of countrys
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        DropListDto[] deliverISO3LAND();

        /// <summary>
        /// delivers a list of languages
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        DropListDto[] deliverCTLANG();

        /// <summary>
        /// delivers a list of print languages 
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        DropListDto[] deliverCTLANG_PRINT();

        /// <summary>
        /// delivers a list of brands
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        DropListDto[] deliverBRAND(String isocode, int obart);

        /// <summary>
        /// Branchenliste zurückgeben
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        DropListDto[] deliverBranche();

        /// <summary>
        /// Versicherungsliste zurückgeben
        /// </summary>
        /// <returns>InsuranceDto</returns>
        InsuranceDto[] deliverInsurance();

        /// <summary>
        /// Liefert eine Liste der Fremdbanken
        /// </summary>
        /// <returns>FremdbankDto</returns>
        FremdbankDto[] listFremdBanken();

        /// <summary>
        /// updates/creates ddlkppos
        /// </summary>
        /// <param name="ddlkpspos"></param>
        /// <returns></returns>
        DdlkpsposDto[] createOrUpdateDdlkpspos(DdlkpsposDto[] ddlkpspos);

        /// <summary>
        /// returns select items for a rub entry
        /// </summary>
        /// <param name="sysddlkpcol"></param>
        /// <returns></returns>
        List<DdlkpposDto> getDdlkppos(long sysddlkpcol);

        /// <summary>
        /// returns the rub entries
        /// </summary>
        /// <param name="sysddlkprub"></param>
        /// <returns></returns>
        List<DdlkpcolDto> getDdlkpcols(long sysddlkprub);

        /// <summary>
        /// returns a list of rubs for the area
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        List<DdlkprubDto> getDdlkprubs(String area);

        /// <summary>
        /// returns a list of Ddlkpspos (rub-values for a certain entity)
        /// </summary>
        /// <param name="area"></param>
        /// <param name="areaid"></param>
        /// <returns></returns>
        List<DdlkpsposDto> getDdlkpspos(String area, long areaid);

        /// <summary>
        ///  Returns all Ddlkprub Details
        /// </summary>
        /// <param name="sysDdlkprub"></param>
        /// <returns></returns>
        DdlkprubDto getDdlkprubDetails(long sysDdlkprub);

        /// <summary>
        ///  Returns all Ddlkprub Details
        /// </summary>
        /// <param name="sysddlkpcol"></param>
        /// <returns></returns>
        DdlkpcolDto getDdlkpcolDetails(long sysddlkpcol);

        /// <summary>
        ///  Returns all Ddlkpcol Details
        /// </summary>
        /// <param name="sysDdlkppos"></param>
        /// <returns></returns>
        DdlkpposDto getDdlkpposDetails(long sysDdlkppos);

        /// <summary>
        ///  Returns all Ddlkpspos Details
        /// </summary>
        /// <param name="sysddlkpspos"></param>
        /// <returns></returns>
        DdlkpsposDto getDdlkpsposDetails(long sysddlkpspos);

        /// <summary>
        /// returns priority flag for salesmen
        /// </summary>
        /// <param name="syshaendler">salesman id</param>
        /// <returns>priority</returns>
        int getPrioHaendler(long syshaendler);
    }
}