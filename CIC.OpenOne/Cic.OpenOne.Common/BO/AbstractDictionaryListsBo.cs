using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DAO;
using System.Collections.Generic;
using System;

namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// Abstract Business object for dictionary lists
    /// </summary>
    public abstract class AbstractDictionaryListsBo : IDictionaryListsBo
    {
        /// <summary>
        /// data access object to use
        /// </summary>
        protected IDictionaryListsDao dao;
        /// <summary>
        /// Translate Dao
        /// </summary>
        protected ITranslateDao translateDao;

        /// <summary>
        /// constructs a distionaryLists business object
        /// </summary>
        /// <param name="dictionaryListsDao">the data access object to use</param>
        /// <param name="translateDao">Translate DAO</param>
        public AbstractDictionaryListsBo(IDictionaryListsDao dictionaryListsDao, ITranslateDao translateDao)
        {
            this.translateDao = translateDao;
            this.dao = dictionaryListsDao;
        }

        /// <summary>
        /// get list values of type by string codes
        /// </summary>
        /// <param name="code"></param>
        /// <param name="domainid"></param>
        /// <returns></returns>
        public abstract DropListDto[] listByCode(String code, String domainid);

        /// <summary>
        /// get list values of given type
        /// </summary>
        /// <returns>DropListDto[]</returns>
        public abstract DropListDto[] listById(DDLKPPOSType code);

        /// <summary>
        /// get list of Aufbau
        /// </summary>
        /// <returns>DropListDto</returns>
        public abstract DropListDto[] listAufbau();

        /// <summary>
        /// get list of Treibstoffart
        /// </summary>
        /// <returns>DropListDto</returns>
        public abstract DropListDto[] listTreibstoffart();

        /// <summary>
        /// get list of Getriebeart
        /// </summary>
        /// <returns>DropListDto</returns>
        public abstract DropListDto[] listGetriebeart();

		/// <summary>
		/// get list of SlaPause reasons
		/// </summary>
		/// <returns>DropListDto</returns>
		public abstract DropListDto[] listSlaPause ();

        /// <summary>
        /// Auslandsausweisarten auflisten
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        public abstract DropListDto[] listAusweisarten();

        /// <summary>
        /// get list values of given type
        /// </summary>
        /// <returns>DropListDto[]</returns>
        public abstract DropListDto[] listByCode(DDLKPPOSType code);

        /// <summary>
        /// get list values of given type
        /// </summary>
        /// <param name="code"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        public abstract DropListDto[] listByCode(DDLKPPOSType code, DDLKPPOSDomain domain);

        /// <summary>
        /// get a list of anreden
        /// </summary>
        /// <returns>olistAnredenDto</returns>
        public abstract DropListDto[] listAnreden();

        /// <summary>
        /// get a list of countrys
        /// </summary>
        /// <returns>olistLaenderDto</returns>
        public abstract DropListDto[] listLaender();

        /// <summary>
        /// get a list of kantone
        /// </summary>
        /// <returns>olistKantoneDto</returns>
        public abstract DropListDto[] listKantone(string isoCode);

        /// <summary>
        /// get a list of berufliche Situationen
        /// </summary>
        /// <returns>olistBeruflicheSituationenDto</returns>
        public abstract DropListDto[] listBeruflicheSituationen();

        /// <summary>
        /// get a list of languages
        /// </summary>
        /// <returns>olistSprachenDto</returns>
        public abstract DropListDto[] listSprachen();

        /// <summary>
        /// get a list of wohnsituationen
        /// </summary>
        /// <returns>olistWohnSituationenDto</returns>
        public abstract DropListDto[] listWohnSituationen();

        /// <summary>
        /// get a list of zivilstaende
        /// </summary>
        /// <returns>olistZivilstaendeDto</returns>
        public abstract DropListDto[] listZivilstaende();

        /// <summary>
        /// get a list of nationalitaeten
        /// </summary>
        /// <returns>olistNationalitaetenDto</returns>
        public abstract DropListDto[] listNationalitaeten();

        /// <summary>
        /// delivers a list of brands
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        public abstract DropListDto[] listMarken(int obart);

        /// <summary>
        /// Zusatzeinkommen auflisten
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        public abstract DropListDto[] listZusatzeinkommen();

        /// <summary>
        /// Auslagenarten auflisten
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        public abstract DropListDto[] listAuslagenarten();

         /// <summary>
        /// Weitere Auslagenarten Auflisten
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        public abstract DropListDto[] listWeitereAuslagenarten();
        
        /// <summary>
        /// Unterstützungsarten auflisten
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        public abstract DropListDto[] listUnterstuetzungsarten();

        /// <summary>
        /// Rechtsformen auflisten
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        public abstract DropListDto[] listRechtsformen();

        /// <summary>
        /// Branchen auflisten
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        public abstract DropListDto[] listBranchen();

        /// <summary>
        /// Berufsauslagenarten
        /// </summary>
        /// <returns></returns>
        public abstract DropListDto[] listBerufsauslagenart();

         /// <summary>
        /// get list of MitantragstellerStati
        /// </summary>
        /// <returns>oliststatusmitantDto</returns>
        public abstract DropListDto[] listMitantragstellerStati();

        /// <summary>
        /// get list of Insurance
        /// </summary>
        /// <returns>InsuranceDto</returns>
        public abstract InsuranceDto[] listInsurance();

        /// <summary>
        /// get list of Verwendungszweck
        /// </summary>
        /// <returns>DropListDto</returns>
        public abstract DropListDto[] listVerwendungszweck();
 
        /// <summary>
        /// Liefert eine Liste der Fremdbanken
        /// </summary>
        /// <returns>FremdbankDto</returns>
        public abstract FremdbankDto[] listFremdBanken();

        /// <summary>
        /// updates/creates Zusatzdaten
        /// </summary>
        /// <param name="ddlkpspos"></param>
        /// <returns></returns>
        public abstract DdlkpsposDto[] createOrUpdateDdlkpspos(DdlkpsposDto[] ddlkpspos);

        /// <summary>
        /// Returns all Rub Infos
        /// </summary>
        /// <param name="iRub"></param>
        /// <returns></returns>
        public abstract List<DdlkprubDto> getRubInfo(igetRubDto iRub);

        /// <summary>
        /// Returns all Ddlkprub Details
        /// </summary>
        /// <param name="sysddlkprub"></param>
        /// <returns></returns>
        public abstract DdlkprubDto getDdlkprubDetails(long sysddlkprub);

        /// <summary>
        /// Returns all Ptrelatep Details
        /// </summary>
        /// <param name="sysddlkpcol"></param>
        /// <returns></returns>
        public abstract DdlkpcolDto getDdlkpcolDetails(long sysddlkpcol);

        /// <summary>
        /// Returns all Ddlkppos Details
        /// </summary>
        /// <param name="sysddlkppos"></param>
        /// <returns></returns>
        public abstract DdlkpposDto getDdlkpposDetails(long sysddlkppos);

        /// <summary>
        /// Returns all Ddlkpspos Details
        /// </summary>
        /// <param name="sysddlkpspos"></param>
        /// <returns></returns>
        public abstract DdlkpsposDto getDdlkpsposDetails(long sysddlkpspos);


        /// <summary>
        /// Returns the translated DDLKPPOS List for the code and domain
        /// </summary>
        /// <param name="code"></param>
        /// <param name="domainid"></param>
        /// <returns></returns>
        public abstract DropListDto[] getDdlkpsposDetails(String code, String domainid);

        /// <summary>
        /// Returns the translated DDLKPPOS List for the code and domain, id is a string
        /// </summary>
        /// <param name="code"></param>
        /// <param name="domainid"></param>
        /// <returns></returns>
        public abstract DropListDto[] getDdlkpsposDetailsById(String code, String domainid);

    }
}