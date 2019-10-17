using System;
using System.Linq;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DAO;
using System.Collections.Generic;

namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// Business Object for dictionary lists
    /// </summary>
    public class DictionaryListsBo : AbstractDictionaryListsBo
    {
        private String isoCode;
        private ITranslateBo translator;

        /// <summary>
        /// contructs the business object and sets the dao to use for data access
        /// </summary>
        /// <param name="dictionaryListsDao">the data access object to use</param>
        /// <param name="translateDao">Translate DAO</param>
        /// <param name="isoCode">ISO Code für Übersetzung</param>
        public DictionaryListsBo(IDictionaryListsDao dictionaryListsDao, ITranslateDao translateDao, String isoCode)
            : base(dictionaryListsDao, translateDao)
        {
            this.isoCode = isoCode;
            translator = new TranslateBo(translateDao);
        }

        /// <summary>
        /// get list values of given type
        /// </summary>
        /// <returns>DropListDto[]</returns>
        public override DropListDto[] listById(DDLKPPOSType code)
        {

            return dao.findByDDLKPPOSCode(code, isoCode, null, true);
        }


        /// <summary>
        /// get list values of given type
        /// </summary>
        /// <returns>DropListDto[]</returns>
        public override DropListDto[] listByCode(DDLKPPOSType code)
        {

            return dao.findByDDLKPPOSCode(code, isoCode);
        }

       /// <summary>
        /// get list values of given type
       /// </summary>
       /// <param name="code"></param>
       /// <param name="domain"></param>
       /// <returns></returns>
        public override DropListDto[] listByCode(DDLKPPOSType code, DDLKPPOSDomain domain)
        {

            return dao.findByDDLKPPOSCode(code, isoCode,domain.ToString());
        }

        /// <summary>
        /// get list values of type by string codes
        /// </summary>
        /// <param name="code"></param>
        /// <param name="domainid"></param>
        /// <returns></returns>
        public override DropListDto[] listByCode(String code, String domainid)
        {
            return dao.findByDDLKPPOSCode(code, isoCode, domainid,false);
        }

        /// <summary>
        /// get list of Anreden
        /// </summary>
        /// <returns>olistAnredenDto</returns>
        public override DropListDto[] listAnreden()
        {
            return dao.findByDDLKPPOSCode(DDLKPPOSType.ANREDEN, isoCode, DDLKPPOSDomain.KURZ.ToString());
        }

        /// <summary>
        /// get list of Aufbau
        /// </summary>
        /// <returns>DropListDto</returns>
        public override DropListDto[] listAufbau()
        {
            return dao.deliverAufbau(isoCode);
        }

        /// <summary>
        /// get list of Getriebeart
        /// </summary>
        /// <returns>DropListDto</returns>
        public override DropListDto[] listGetriebeart()
        {
            return dao.deliverGetriebeart(isoCode);
        }

		/// <summary>
		/// get list of SlaPause reasons
		/// </summary>
		/// <returns>DropListDto</returns>
		public override DropListDto[] listSlaPause ()
		{
			return dao.deliverSlaPause (isoCode);
		}
		/// <summary>
        /// get list of Treibstoffart
        /// </summary>
        /// <returns>DropListDto</returns>
        public override DropListDto[] listTreibstoffart()
        {
            return dao.deliverTreibstoffart(isoCode);
        }

        /// <summary>
        /// get list of Laender
        /// </summary>
        /// <returns>olistLeanderDto</returns>
        public override DropListDto[] listLaender()
        {

            return translator.TranslateList(dao.deliverLAND(), TranslateArea.LAND, isoCode);
        }

        /// <summary>
        /// get list of Kantone
        /// </summary>
        /// <returns>olistKantoneDto</returns>
        public override DropListDto[] listKantone(string isoCode)
        {
           
            return translator.TranslateList(dao.deliverSTAAT(), TranslateArea.STAAT, isoCode);
        }

        /// <summary>
        /// get list of berufliche Situationen
        /// </summary>
        /// <returns>olistBeruflicheSituationenDto</returns>
        public override DropListDto[] listBeruflicheSituationen()
        {
            return dao.findByDDLKPPOSCode(DDLKPPOSType.BERUFLICHESIT, isoCode);
        }

        /// <summary>
        /// get list of sprachen
        /// </summary>
        /// <returns>olistSprachenDto</returns>
        public override DropListDto[] listSprachen()
        {
           
            return translator.TranslateList(dao.deliverCTLANG(), TranslateArea.CTLANG, isoCode);
           
        }

        /// <summary>
        /// get list of Wohnsituationen
        /// </summary>
        /// <returns>olistWohnSituationenDto</returns>
        public override DropListDto[] listWohnSituationen()
        {
            return dao.findByDDLKPPOSCode(DDLKPPOSType.WOHNSITUATIONEN, isoCode);
        }

        /// <summary>
        /// get list of Zivilstaende
        /// </summary>
        /// <returns>olistzivilstaendeDto</returns>
        public override DropListDto[] listZivilstaende()
        {
            return dao.findByDDLKPPOSCode(DDLKPPOSType.ZIVILSTAENDE, isoCode);
        }

        /// <summary>
        /// get list of Nationalitaeten
        /// </summary>
        /// <returns>olistNationalitaetenDto</returns>
        public override DropListDto[] listNationalitaeten()
        {
            DropListDto[] nationalitaeten = translator.TranslateList(dao.deliverNATIONALITIES(), TranslateArea.LAND, isoCode);
            // SCHWEIZ,LIECHTENSTEIN,DEUTSCHLAND,FRANKREICH,ÖSTERREICH,ITALIEN

            DropListDto[] favoriten = new DropListDto[6];
            Array.Copy(nationalitaeten, 0, favoriten,0, 6);
            DropListDto[] andere = new DropListDto[nationalitaeten.Length-6];
            Array.Copy(nationalitaeten, 6, andere, 0, nationalitaeten.Length - 6);
           
            Array.Copy(favoriten, 0, nationalitaeten, 0, 6);
            Array.Copy(andere.OrderBy(x => x.bezeichnung).ToArray(), 0, nationalitaeten, 6, andere.Length);

            return nationalitaeten;
        }

        /// <summary>
        /// delivers a list of brands
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        public override DropListDto[] listMarken(int obart)
        {
            return dao.deliverBRAND(isoCode, obart);
        }

        /// <summary>
        /// Zusatzeinkommen auflisten
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        public override DropListDto[] listZusatzeinkommen()
        {
            return dao.findByDDLKPPOSCode(DDLKPPOSType.ZUSEINKART, isoCode);
        }

        /// <summary>
        /// Auslandsausweisarten auflisten
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        public override DropListDto[] listAusweisarten()
        {
            return dao.findByDDLKPPOSCode(DDLKPPOSType.AUSLAUSWEIS, isoCode);
        }

        /// <summary>
        /// Berufsauslagenart auflisten
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        public override DropListDto[] listBerufsauslagenart()
        {
            return dao.findByDDLKPPOSCode(DDLKPPOSType.BERUFAUSLART, isoCode);
        }

        /// <summary>
        /// Auslagenarten Auflisten
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        public override DropListDto[] listAuslagenarten()
        {
            return dao.findByDDLKPPOSCode(DDLKPPOSType.REGELMAUSLART, isoCode);
        }

        /// <summary>
        /// Weitere Auslagenarten Auflisten
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        public override DropListDto[] listWeitereAuslagenarten()
        {
            return dao.findByDDLKPPOSCode(DDLKPPOSType.WEITEREAUSLAGEN, isoCode);
        }

        /// <summary>
        /// Unterstützungsarten auflisten
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        public override DropListDto[] listUnterstuetzungsarten()
        {
            return dao.findByDDLKPPOSCode(DDLKPPOSType.UNTERSTZART, isoCode);
        }

        /// <summary>
        /// Rechtsformen auflisten
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        public override DropListDto[] listRechtsformen()
        {
            return dao.findByDDLKPPOSCode(DDLKPPOSType.DVLEGALFORM, isoCode);
        }

        /// <summary>
        /// Branchen auflisten
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        public override DropListDto[] listBranchen()
        {
            return dao.deliverBranche();
        }

        /// <summary>
        /// get list of MitantragstellerStati
        /// </summary>
        /// <returns>oliststatusmitantDto</returns>
        public override DropListDto[] listMitantragstellerStati()
        {
            return dao.findByDDLKPPOSCode(DDLKPPOSType.STATUSMITANT, isoCode);
        }


        /// <summary>
        /// get list of Verwendungszweck
        /// </summary>
        /// <returns>DropListDto</returns>
        public override DropListDto[] listVerwendungszweck()
        {
            return dao.findByDDLKPPOSCode(DDLKPPOSType.VERWENDUNG, isoCode);
        }

        /// <summary>
        /// get list of Insurance
        /// </summary>
        /// <returns>InsuranceDto</returns>
        public override InsuranceDto[] listInsurance()
        {
            return dao.deliverInsurance();
        }

        /// <summary>
        /// Liefert eine Liste der Fremdbanken
        /// </summary>
        /// <returns>FremdbankDto</returns>
        public override FremdbankDto[] listFremdBanken()
        {
            return dao.listFremdBanken();
        }

        /// <summary>
        /// updates/creates Zusatzdaten
        /// </summary>
        /// <param name="ddlkpspos"></param>
        /// <returns></returns>
        override public DdlkpsposDto[] createOrUpdateDdlkpspos(DdlkpsposDto[] ddlkpspos)
        {
            return dao.createOrUpdateDdlkpspos(ddlkpspos);
        }
        /// <summary>
        /// Returns all Rub Infos for Zusatzdaten Detail
        /// </summary>
        /// <param name="iRub"></param>
        /// <returns></returns>
        override public List<DdlkprubDto> getRubInfo(igetRubDto iRub)
        {
            List<DdlkpsposDto> eintraege = dao.getDdlkpspos(iRub.area, iRub.areaid);

            // Alle Rubriken
            List<DdlkprubDto> rubriken = dao.getDdlkprubs(iRub.crmarea);

            foreach (DdlkprubDto rubrik in rubriken)
            {
                rubrik.cols = dao.getDdlkpcols(rubrik.getEntityId());
                foreach (DdlkpcolDto col in rubrik.cols)
                {
                    if (eintraege != null && eintraege.Count > 0)
                    {
                        var query = eintraege.Where(a => a.sysddlkpcol == col.entityId);
                        if (query.Count() > 0)
                        {
                            DdlkpsposDto value = query.Select(a => a).First();
                            value = new DdlkpsposDto(value);
                            col.value = value;
                            if (col.type == 2)
                            {
                                //col.value.sysddlkppos = query.Where(a => a.value.Equals(value.value)).Select(a=>a.sysddlkppos).First();
                                //set the first value for selectboxes
                            }
                        }
                    }
                    if (col.type == 2)
                    {
                        col.selectItems = dao.getDdlkppos(col.entityId);
                        if (col.value != null && col.selectItems.Count > 0)
                        {
                            DdlkpposDto foundItem = col.selectItems.Where(a => a.value.Equals(col.value.value)).FirstOrDefault();
                            if (foundItem != null)
                                col.value.sysddlkppos = foundItem.sysddlkppos;
                        }
                    }
                }
            }
            return rubriken;
        }
        /// <summary>
        /// Returns all Ddlkprub Details
        /// </summary>
        /// <param name="sysddlkprub"></param>
        /// <returns></returns>
        override public DdlkprubDto getDdlkprubDetails(long sysddlkprub)
        {
            return dao.getDdlkprubDetails(sysddlkprub);
        }

        /// <summary>
        /// Returns all Ddlkpcol Details
        /// </summary>
        /// <param name="sysddlkpcol"></param>
        /// <returns></returns>
        override public DdlkpcolDto getDdlkpcolDetails(long sysddlkpcol)
        {
            return dao.getDdlkpcolDetails(sysddlkpcol);
        }

        /// <summary>
        /// Returns all Ddlkppos Details
        /// </summary>
        /// <param name="sysddlkppos"></param>
        /// <returns></returns>
        override public DdlkpposDto getDdlkpposDetails(long sysddlkppos)
        {
            return dao.getDdlkpposDetails(sysddlkppos);
        }

        /// <summary>
        /// Returns all Ddlkpspos Details
        /// </summary>
        /// <param name="sysddlkpspos"></param>
        /// <returns></returns>
        override public DdlkpsposDto getDdlkpsposDetails(long sysddlkpspos)
        {
            return dao.getDdlkpsposDetails(sysddlkpspos);
        }

        /// <summary>
        /// Returns the translated DDLKPPOS List for the code and domain
        /// </summary>
        /// <param name="code"></param>
        /// <param name="domainid"></param>
        /// <returns></returns>
        override public DropListDto[] getDdlkpsposDetails(String code,String domainid)
        {
            return dao.findByDDLKPPOSCode(code,isoCode,domainid,false);
        }
        /// <summary>
        /// Returns the translated DDLKPPOS List for the code and domain
        /// </summary>
        /// <param name="code"></param>
        /// <param name="domainid"></param>
        /// <returns></returns>
        override public DropListDto[] getDdlkpsposDetailsById(String code, String domainid)
        {
            return dao.findByDDLKPPOSCode(code, isoCode, domainid, true);
        }
    }
}