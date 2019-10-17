using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;

namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// defines interface for business objects providing dictionary lists
    /// </summary>
    public interface IDictionaryListsBo
    {
        /// <summary>
        /// get list values of given type
        /// </summary>
        /// <returns>DropListDto[]</returns>
        DropListDto[] listById(DDLKPPOSType code);

        /// <summary>
        /// get list values of type by string codes
        /// </summary>
        /// <param name="code"></param>
        /// <param name="domainid"></param>
        /// <returns></returns>
        DropListDto[] listByCode(String code, String domainid);

        /// <summary>
        /// get list of Aufbau
        /// </summary>
        /// <returns>DropListDto</returns>
        DropListDto[] listAufbau();

        /// <summary>
        /// get list of Treibstoffart
        /// </summary>
        /// <returns>DropListDto</returns>
        DropListDto[] listTreibstoffart();

        /// <summary>
        /// get list of Getriebeart
        /// </summary>
        /// <returns>DropListDto</returns>
        DropListDto[] listGetriebeart();

		/// <summary>
		/// get list of SlaPause reasons
		/// </summary>
		/// <returns>DropListDto</returns>
		DropListDto[] listSlaPause ();

        /// <summary>
        /// Auslandsausweisarten auflisten
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        DropListDto[] listAusweisarten();

         /// <summary>
        /// get list values of given type
        /// </summary>
        /// <returns>DropListDto[]</returns>
        DropListDto[] listByCode(DDLKPPOSType code);

        /// <summary>
        /// get list values of given type
        /// </summary>
        /// <param name="code"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        DropListDto[] listByCode(DDLKPPOSType code, DDLKPPOSDomain domain);

        /// <summary>
        /// list of anreden
        /// </summary>
        /// <returns>olistAnredenDto</returns>
        DropListDto[] listAnreden();

        /// <summary>
        /// list of countrys
        /// </summary>
        /// <returns>olistLaenderDto</returns>
        DropListDto[] listLaender();

        /// <summary>
        /// list of kantone
        /// </summary>
        /// <returns>olistKantoneDto</returns>
        DropListDto[] listKantone(string isoCode);

        /// <summary>
        /// list of berufliche situationen
        /// </summary>
        /// <returns>olistBeruflicheSituationen</returns>
        DropListDto[] listBeruflicheSituationen();

        /// <summary>
        /// list of sprachen
        /// </summary>
        /// <returns>olistSprachenDto</returns>
        DropListDto[] listSprachen();

        /// <summary>
        /// list of wohnsituationen
        /// </summary>
        /// <returns>olistWohnSituationen</returns>
        DropListDto[] listWohnSituationen();

        /// <summary>
        /// list of zivilstaende
        /// </summary>
        /// <returns>olistzivilstaende</returns>
        DropListDto[] listZivilstaende();

        /// <summary>
        /// list of Nationalitaeten
        /// </summary>
        /// <returns>olistNationalitaetenDto</returns>
        DropListDto[] listNationalitaeten();

        /// <summary>
        /// delivers a list of brands
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        DropListDto[] listMarken(int obart);

        /// <summary>
        /// Zusatzeinkommes auflisten
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        DropListDto[] listZusatzeinkommen();

        /// <summary>
        /// Auslagenarten auflisten
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        DropListDto[] listAuslagenarten();

         /// <summary>
        /// Weitere Auslagenarten Auflisten
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        DropListDto[] listWeitereAuslagenarten();
        
        /// <summary>
        /// Unterstützungsarten auflisten
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        DropListDto[] listUnterstuetzungsarten();

        /// <summary>
        /// Rechtsformen auflisten
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        DropListDto[] listRechtsformen();

        /// <summary>
        /// Branchen auflisten
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        DropListDto[] listBranchen();

        /// <summary>
        /// Berufsauslagenarten
        /// </summary>
        /// <returns></returns>
        DropListDto[] listBerufsauslagenart();


         /// <summary>
        /// get list of MitantragstellerStati
        /// </summary>
        /// <returns>oliststatusmitantDto</returns>
        DropListDto[] listMitantragstellerStati();

        /// <summary>
        /// get list of Insurance
        /// </summary>
        /// <returns>InsuranceDto</returns>
        InsuranceDto[] listInsurance();

        /// <summary>
        /// get list of Verwendungszweck
        /// </summary>
        /// <returns>DropListDto</returns>
        DropListDto[] listVerwendungszweck();

        /// <summary>
        /// Liefert eine Liste der Fremdbanken
        /// </summary>
        /// <returns>FremdbankDto</returns>
        FremdbankDto[] listFremdBanken();

        /// <summary>
        /// updates/creates Zusatzdaten
        /// </summary>
        /// <param name="ddlkpspos"></param>
        /// <returns></returns>
        DdlkpsposDto[] createOrUpdateDdlkpspos(DdlkpsposDto[] ddlkpspos);
        /// <summary>
        /// Returns all Rub Infos
        /// </summary>
        /// <param name="iRub"></param>
        /// <returns></returns>
        List<DdlkprubDto> getRubInfo(igetRubDto iRub);
        /// <summary>
        /// Returns all Ddlkprub Details
        /// </summary>
        /// <param name="sysddlkprub"></param>
        /// <returns></returns>
        DdlkprubDto getDdlkprubDetails(long sysddlkprub);

        /// <summary>
        /// Returns all Ddlkpcol Details
        /// </summary>
        /// <param name="sysddlkpcol"></param>
        /// <returns></returns>
        DdlkpcolDto getDdlkpcolDetails(long sysddlkpcol);

        /// <summary>
        /// Returns all DdlkpposDto Details
        /// </summary>
        /// <param name="sysddlkppos"></param>
        /// <returns></returns>
        DdlkpposDto getDdlkpposDetails(long sysddlkppos);

        /// <summary>
        /// Returns all Ddlkpspos Details
        /// </summary>
        /// <param name="sysddlkpspos"></param>
        /// <returns></returns>
        DdlkpsposDto getDdlkpsposDetails(long sysddlkpspos);

        /// <summary>
        /// Returns the translated DDLKPPOS List for the code and domain
        /// </summary>
        /// <param name="code"></param>
        /// <param name="domainid"></param>
        /// <returns></returns>
        DropListDto[] getDdlkpsposDetails(String code, String domainid);

        /// <summary>
        /// Returns the translated DDLKPPOS List for the code and domain, id is a string
        /// </summary>
        /// <param name="code"></param>
        /// <param name="domainid"></param>
        /// <returns></returns>
        DropListDto[] getDdlkpsposDetailsById(String code, String domainid);
    }
}
