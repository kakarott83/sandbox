using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Cic.One.DTO;
using Cic.One.Web.BO;
using Cic.One.Web.BO.Search;

using Cic.One.Web.DAO;

using System.Reflection;
using Cic.One.Web.Service.DAO;
using AutoMapper;
using Cic.One.Web.Contract;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.BO.Search;
using Cic.One.DTO.BN;
using System.ServiceModel.Web;

namespace Cic.One.Web.Service
{
    [ServiceBehavior(Namespace = "http://cic-software.de/One")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public class searchService : IsearchService
    {
        private static Cic.OpenOne.Common.Util.Logging.ILog log = Cic.OpenOne.Common.Util.Logging.Log.GetLogger(typeof(searchService));

        /// <summary>
        /// delivers a list of LogDumps
        /// rh: 20161108
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchLogDumpDto searchLogDump(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<LogDumpDto, oSearchLogDumpDto>().search(iSearch);
        }

        /// <summary>
        /// delivers a list of Clarifications
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchClarificationDto searchClarification(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<ClarificationDto, oSearchClarificationDto>().search(iSearch);
        }

        /// <summary>
        /// delivers a list of Staffelpositionstyp
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchStaffelpositionstypDto searchStaffelpositionstyp(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<StaffelpositionstypDto, oSearchStaffelpositionstypDto>().search(iSearch);
          
        }

        /// <summary>
        /// delivers a list of Staffeltyp
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchStaffeltypDto searchStaffeltyp(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<StaffeltypDto, oSearchStaffeltypDto>().search(iSearch);
           
        }

        /// <summary>
        /// delivers a list of Rolle
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchRolleDto searchRolle(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<RolleDto, oSearchRolleDto>().search(iSearch);
           
        }

        /// <summary>
        /// delivers a list of Rollentyp
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchRollentypDto searchRollentyp(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<RollentypDto, oSearchRollentypDto>().search(iSearch);
           
        }

        /// <summary>
        /// delivers a list of Handelsgruppe
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchHandelsgruppeDto searchHandelsgruppe(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<HandelsgruppeDto, oSearchHandelsgruppeDto>().search(iSearch);
          
        }

        /// <summary>
        /// delivers a list of Vertriebskanal
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchVertriebskanalDto searchVertriebskanal(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<VertriebskanalDto, oSearchVertriebskanalDto>().search(iSearch);
           
        }

        /// <summary>
        /// delivers a list of Brand
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchBrandDto searchBrand(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<BrandDto, oSearchBrandDto>().search(iSearch);
          
        }

        /// <summary>
        /// delivers a list of Rechnung
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchRechnungDto searchRechnung(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<RechnungDto, oSearchRechnungDto>().search(iSearch);
          
        }

        /// <summary>
        /// delivers a list of Angobbrief
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchAngobbriefDto searchAngobbrief(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<AngobbriefDto, oSearchAngobbriefDto>().search(iSearch);
            
        }

        /// <summary>
        /// delivers a list of Zahlungsplan
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchZahlungsplanDto searchZahlungsplan(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<ZahlungsplanDto, oSearchZahlungsplanDto>().search(iSearch);
            
        }

		
        /// <summary>
		/// delivers a list of Kreditlinie
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
		public oSearchKreditlinieDto searchKreditlinie (iSearchDto iSearch)
        {
			return new SearchHandlerFactory<KreditlinieDto, oSearchKreditlinieDto> ().search (iSearch);
            
        }

        /// <summary>
        /// delivers a list of NKK Zahlungsplan
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchNkkabschlDto searchNkkabschl(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<NkkabschlDto, oSearchNkkabschlDto>().search(iSearch);

        }

        /// <summary>
        /// delivers a list of Fahrzeugbrief
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchFahrzeugbriefDto searchFahrzeugbrief(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<FahrzeugbriefDto, oSearchFahrzeugbriefDto>().search(iSearch);
        }

        /// <summary>
        /// delivers a list of Kalk
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchKalkDto searchKalk(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<KalkDto, oSearchKalkDto>().search(iSearch);
        }

        /// <summary>
        /// delivers a list of Person
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchPersonDto searchPerson(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<PersonDto, oSearchPersonDto>().search(iSearch);
            
        }

        /// <summary>
        /// Sucht nach Dokumenten
        /// </summary>
        /// <param name="dynamicSearch">Parameter</param>
        /// <returns>Infos zu den gefundenen Dokumenten</returns>
        public oDynamicDocumentSearchDto DynamicDocumentSearch(iDynamicDocumentSearchDto dynamicSearch)
        {
            ServiceHandler<iDynamicDocumentSearchDto, oDynamicDocumentSearchDto> ew = new ServiceHandler<iDynamicDocumentSearchDto, oDynamicDocumentSearchDto>(dynamicSearch);
            return ew.process(delegate(iDynamicDocumentSearchDto input, oDynamicDocumentSearchDto rval)
            {
                if (input == null || input.Dbas == null)
                    throw new ArgumentException("No valid input");

                IDocumentSearchBo bo = BOFactoryFactory.getInstance().getDocumentSearchBO();
                rval.Hitlist = bo.DynamicDocumentSearch(input);
            });
        }


        /// <summary>
        /// Lädt ein Dokument anhand dem Input
        /// </summary>
        /// <param name="docLoad"></param>
        /// <returns>Enthält das Dokument</returns>
        public oDocumentLoadDto DocumentLoad(iDocumentLoadDto docLoad)
        {
            ServiceHandler<iDocumentLoadDto, oDocumentLoadDto> ew = new ServiceHandler<iDocumentLoadDto, oDocumentLoadDto>(docLoad);
            return ew.process(delegate(iDocumentLoadDto input, oDocumentLoadDto rval)
            {
                if (input == null || input.Docid == null)
                    throw new ArgumentException("No valid input");

                IDocumentSearchBo bo = BOFactoryFactory.getInstance().getDocumentSearchBO();
                rval.Result = bo.DocumentLoad(input);
            });
        }

        /// <summary>
        /// delivers a list of Objekt
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchObjektDto searchObjekt(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<ObjektDto, oSearchObjektDto>().search(iSearch);
        }

     

        /// <summary>
        /// delivers a list of Recalc
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchRecalcDto searchRecalc(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<RecalcDto, oSearchRecalcDto>().search(iSearch);
        }

        /// <summary>
        /// delivers a list of Mycalc
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchMycalcDto searchMycalc(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<MycalcDto, oSearchMycalcDto>().search(iSearch);
        }

        /// <summary>
        /// delivers a list of MyCalc
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchMycalcfsDto searchMycalcfs(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<MycalcfsDto, oSearchMycalcfsDto>().search(iSearch);
        }


        /// <summary>
        /// delivers a list of Exptyp
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchExptypDto searchExptyp(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<ExptypDto, oSearchExptypDto>().search(iSearch);
        }

        /// <summary>
        /// delivers a list of Expval
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchExpvalDto searchExpval(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<ExpvalDto, oSearchExpvalDto>().search(iSearch);
        }

        /// <summary>
        /// delivers Report Data
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchReportDto searchReport(iSearchDto iSearch)
        {
            ServiceHandler<iSearchDto, oSearchReportDto> ew = new ServiceHandler<iSearchDto, oSearchReportDto>(iSearch);
            return ew.process(delegate(iSearchDto input, oSearchReportDto rval, CredentialContext ctx)
            {
                if (input == null)
                    throw new ArgumentException("No report input");

                IReportBo bo = BOFactoryFactory.getInstance().getReportBo();

                rval.result = bo.getReportData(input, ctx);

            });
            
        }

        /// <summary>
        /// searches in eaihot
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchEaihotDto searchEaihot(iSearchDto iSearch)
        {
            //for guid-login we allow it unsecured
            /*if (iSearch.filters != null && iSearch.filters.Length == 2 && iSearch.filters[0].fieldname.Equals("computername") && iSearch.filters[1].fieldname.Equals("code") 
                && iSearch.filters[1].value.Equals("LOGIN"))
            {
                ServiceHandler<iSearchDto, oSearchEaihotDto> ew = new ServiceHandler<iSearchDto, oSearchEaihotDto>(iSearch);
                return ew.process(delegate(iSearchDto input, oSearchEaihotDto rval, CredentialContext ctx)
                {

                    if (input == null)
                        throw new ArgumentException("No search input");
                    rval.result = new SearchBo<EaihotDto>(SearchQueryFactoryFactory.getInstance()).search(input);

                },false);
            }
            else*/
            {

                return new SearchHandlerFactory<Cic.One.DTO.EaihotDto, oSearchEaihotDto>().search(iSearch);
            }
            
        }

        /// <summary>
        /// delivers a list of Rahmen
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchRahmenDto searchRahmen(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<RahmenDto, oSearchRahmenDto>().search(iSearch);
           
        }

        /// <summary>
        /// delivers a list of Haendler
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchHaendlerDto searchHaendler(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<HaendlerDto, oSearchHaendlerDto>().search(iSearch);
         
        }


        /// <summary>
        /// delivers a list of generic view item results
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchGviewDto searchGview(iSearchDto iSearch)
        {
            ServiceHandler<iSearchDto, oSearchGviewDto> ew = new ServiceHandler<iSearchDto, oSearchGviewDto>(iSearch);
            return ew.process(delegate(iSearchDto input, oSearchGviewDto rval, CredentialContext ctx)
            {

                if (input == null)
                    throw new ArgumentException("No search input");

                rval.result = new SearchBo<GviewDto>(SearchQueryFactoryFactory.getInstance().getQueryInfo(iSearch.queryId), ctx.getMembershipInfo().sysPEROLE
                    , new Cic.One.Web.Service.DAO.GViewSearchDao(iSearch.queryId)).search(input);

            });
        }

        /// <summary>
        /// delivers a list of vorgänge
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchVorgangDto searchVorgang(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<VorgangDto, oSearchVorgangDto>().search(iSearch);
        }

        /// <summary>
        /// delivers a list for the vc_activities View
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchActivitiesDto searchActivities(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<ActivitiesDto, oSearchActivitiesDto>().search(iSearch);
        }

        /// <summary>
        /// delivers a list for the Oppotask-Table
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchOppotaskDto searchOppotask(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<OppotaskDto, oSearchOppotaskDto>().search(iSearch);
        }

        /// <summary>
        /// delivers a list for the In Equity Forecast-Table
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchIefDto searchIef(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<IefDto, oSearchIefDto>().search(iSearch);
        }

        /// <summary>
        /// delivers a list of finanzierungen
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchFinanzierungDto searchFinanzierungen(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<FinanzierungDto, oSearchFinanzierungDto>().search(iSearch);
        }

		/// <summary>
		/// delivers a list of Pread-Flags
		/// </summary>
		/// <param name="iSearch"></param>
		/// <returns></returns>
		public oSearchPreadDto searchPread (iSearchDto iSearch)
		{
			return new SearchHandlerFactory<PreadDto, oSearchPreadDto> ().search (iSearch);
		}

		/// <summary>
        /// delivers a list of customers
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchKundeDto searchKunden(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<KundeDto, oSearchKundeDto>().search(iSearch);
         
        }

        /// <summary>
        /// delivers a list of customers
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchItDto searchIt(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<ItDto, oSearchItDto>().search(iSearch);
         
        }

        /// <summary>
        /// delivers a list of memos
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchMemoDto searchMemos(iSearchDto iSearch)
        {
            ServiceHandler<iSearchDto, oSearchMemoDto> ew = new ServiceHandler<iSearchDto, oSearchMemoDto>(iSearch);
            return ew.process(delegate(iSearchDto input, oSearchMemoDto rval, CredentialContext ctx)
            {

                if (input == null)
                    throw new ArgumentException("No search input");
                rval.result = new SearchBo<MemoDto>(SearchQueryFactoryFactory.getInstance(), ctx.getMembershipInfo().sysPEROLE, input.queryId, ctx.getMembershipInfo().ISOLanguageCode).search(input);
                foreach(MemoDto m in                 rval.result.results)
                {
                    if (m.NOTIZMEMO != null)
                    {
                        //remove old clob content after new content
                        if (m.NOTIZMEMO.IndexOf('\0') > 0)
                        {
                            m.NOTIZMEMO = m.NOTIZMEMO.Substring(0, m.NOTIZMEMO.IndexOf('\0'));
                        }
                        //m.NOTIZMEMO = System.Web.HttpUtility.HtmlEncode(m.NOTIZMEMO);
                        m.NOTIZMEMO = m.NOTIZMEMO.Trim();
                    }
                }

            });
         
        }

        /// <summary>
        /// delivers a list of historical entries
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchHistoryDto searchHistory(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<HistoryDto, oSearchHistoryDto>().search(iSearch);

        }

        /// <summary>
        /// delivers a list of open invoices
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchRechnungFaelligDto searchRechnungFaellig(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<RechnungFaelligDto, oSearchRechnungFaelligDto>().search(iSearch);
        
        }

        /// <summary>
        /// delivers a list of tilgungen
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchTilgungDto searchTilgungen(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<TilgungDto, oSearchTilgungDto>().search(iSearch);
         
        }


        /// <summary>
        /// delivers a list of tilgungen
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchZinsabschlDto searchZinsabschl(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<ZinsabschlDto, oSearchZinsabschlDto>().search(iSearch);

        }

        /// <summary>
        /// delivers a list of contacts
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchContactDto searchContacts(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<ContactDto, oSearchContactDto>().search(iSearch);


           /* ServiceHandler<iSearchDto, oSearchContactDto> ew = new ServiceHandler<iSearchDto, oSearchContactDto>(iSearch);
            return ew.process(delegate(iSearchDto input, oSearchContactDto rval, CredentialContext ctx)
            {

                if (input == null)
                    throw new ArgumentException("No search input");

                rval.result = new SearchBo<ContactDto>(SearchQueryFactoryFactory.getInstance(), ctx.getMembershipInfo().sysPEROLE).search(input);

                //  rval.result.results =  (Mapper.Map<List<ContactDto>, List<ContactDto>>(rval.result.results.ToList())).ToArray();
                for (int i = 0; i <= rval.result.results.Length - 1; i++)
                {
                    rval.result.results[i] = Mapper.Map<ContactDto, ContactDto>(rval.result.results[i]);
                }

            });*/
        }


        /// <summary>
        /// delivers a list of Konto
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchKontoDto searchKonten(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<KontoDto, oSearchKontoDto>().search(iSearch);
         
        }


        /// <summary>
        /// delivers a list of Itkonto
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchItkontoDto searchItkonten(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<ItkontoDto, oSearchItkontoDto>().search(iSearch);
         
        }

        /// <summary>
        /// delivers a list of Camp
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchCampDto searchCamp(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<CampDto, oSearchCampDto>().search(iSearch);
          
        }

        /// <summary>
        /// delivers a list of Adresse
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchAdresseDto searchAdressen(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<AdresseDto, oSearchAdresseDto>().search(iSearch);
           
        }

        /// <summary>
        /// delivers a list of Itadresse
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchItadresseDto searchItadressen(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<ItadresseDto, oSearchItadresseDto>().search(iSearch);
           
        }


        /// <summary>
        /// delivers a list of Strasse
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchStrasseDto searchStrasse(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<StrasseDto, oSearchStrasseDto>().search(iSearch);

        }

        /// <summary>
        /// delivers a list of Plz
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchPlzDto searchPLZ(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<PlzDto, oSearchPlzDto>().search(iSearch);

        }


        /// <summary>
        /// delivers a list of Adresse
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchPtaskDto searchPtasks(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<PtaskDto, oSearchPtaskDto>().search(iSearch);
          
        }


        /// <summary>
        /// delivers a list of Apptmt
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchApptmtDto searchApptmts(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<ApptmtDto, oSearchApptmtDto>().search(iSearch);
            
        }

        /// <summary>
        /// delivers a list of Apptmt with Recurr
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchApptmtsWithRecurrDto searchApptmtsWithRecurr(iSearchApptmtsWithRecurrDto search)
        {

            ServiceHandler<iSearchApptmtsWithRecurrDto, oSearchApptmtsWithRecurrDto> ew = new ServiceHandler<iSearchApptmtsWithRecurrDto, oSearchApptmtsWithRecurrDto>(search);
            return ew.process(delegate(iSearchApptmtsWithRecurrDto input, oSearchApptmtsWithRecurrDto rval, CredentialContext ctx)
            {

                if (input == null)
                    throw new ArgumentException("No search input");

                rval.result = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo()).searchApptmts(input);

            });
        }


        /// <summary>
        /// delivers a list of Reminder
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchReminderDto searchReminders(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<ReminderDto, oSearchReminderDto>().search(iSearch);
            
        }

        /// <summary>
        /// delivers a list of Mailmsg
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchMailmsgDto searchMailmsgs(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<MailmsgDto, oSearchMailmsgDto>().search(iSearch);
            
        }

        /// <summary>
        /// delivers a list of Prun
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchPrunDto searchPruns(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<PrunDto, oSearchPrunDto>().search(iSearch);
            
        }

        /// <summary>
        /// delivers a list of Fileatt
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchFileattDto searchFileatts(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<FileattDto, oSearchFileattDto>().search(iSearch);
           
        }

        /// <summary>
        /// delivers a list of Besuchsberichte
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchBesuchsberichteDto searchBesuchsberichte(iSearchDto iSearch)
        {
            //return new SearchHandlerFactory<BesuchsberichtDto, oSearchBesuchsberichteDto>().search(iSearch);

            ServiceHandler<iSearchDto, oSearchBesuchsberichteDto> ew = new ServiceHandler<iSearchDto, oSearchBesuchsberichteDto>(iSearch);
            return ew.process(delegate(iSearchDto input, oSearchBesuchsberichteDto rval)
            {

                if (input == null)
                    throw new ArgumentException("No search input");


                QueryInfoDataType1 infoData = new QueryInfoDataType1("FILEATT", "FILEATT.SYSFILEATT");
                infoData.resultFields = "CONTENT,sysfileatt,area,sysid ";
                infoData.resultTables = "CIC.FILEATT FILEATT ";
                infoData.searchTables = "CIC.FILEATT FILEATT ";

                infoData.searchConditions = " 1=1 ";

                infoData.optimized = false;


                oSearchDto<Cic.One.DTO.BesuchsberichtDto> sResult = new SearchBo<Cic.One.DTO.BesuchsberichtDto>(infoData).search(input);


                List<BesuchsberichtDto> bblist = new List<BesuchsberichtDto>();
                foreach (Cic.One.DTO.BesuchsberichtDto bb in sResult.results)
                {
                    BesuchsberichtDto bericht = Cic.OpenOne.Common.Util.Serialization.XMLDeserializer.objectFromXml<BesuchsberichtDto>(bb.content, "UTF-8");
                    bericht.sysFileatt = bb.sysFileatt;
                    bericht.sysId = bb.sysId;
                    bericht.area = bb.area;
                    bblist.Add(bericht);
                }
                sResult.results = bblist.ToArray();

                rval.result = sResult;

            });
        }
           
        

        /// <summary>
        /// delivers a list of Prproducts
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchPrproductDto searchPrproducts(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<PrproductDto, oSearchPrproductDto>().search(iSearch);
           
        }

        /// <summary>
        /// delivers a list of Itemcat
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchItemcatDto searchItemcats(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<ItemcatDto, oSearchItemcatDto>().search(iSearch);
          
        }

        /// <summary>
        /// delivers a list of Ctlang
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchCtlangDto searchCtlangs(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<CtlangDto, oSearchCtlangDto>().search(iSearch);
            
        }

        /// <summary>
        /// delivers a list of Land
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchLandDto searchLand(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<LandDto, oSearchLandDto>().search(iSearch);
           
        }

        /// <summary>
        /// delivers a list of Branche
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchBrancheDto searchBranchen(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<BrancheDto, oSearchBrancheDto>().search(iSearch);
          
        }


        /// <summary>
        /// delivers a list of Antragsubersicht
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchAntragsubersichtDto searchAntragsubersicht(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<AntragsubersichtDto, oSearchAntragsubersichtDto>().search(iSearch);
          
        }

        /// <summary>
        /// delivers a list of EKFAntrage
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchEkfantrageDto searchEkfantrage(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<EkfantrageDto, oSearchEkfantrageDto>().search(iSearch);
           
        }

        /// <summary>
        /// delivers a list of Abgerechnetevertrage
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchAbgerechnetevertrageDto searchAbgerechnetevertrage(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<AbgerechnetevertrageDto, oSearchAbgerechnetevertrageDto>().search(iSearch);
         
        }

        /// <summary>
        /// delivers a list of Abrufscheine
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchAbrufscheineDto searchAbrufscheine(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<AbrufscheineDto, oSearchAbrufscheineDto>().search(iSearch);
           
        }

        /// <summary>
        /// delivers a list of Account
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchAccountDto searchAccounts(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<AccountDto, oSearchAccountDto>().search(iSearch);
          
        }

        /// <summary>
        /// delivers a list of WktAccount
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchWktAccountDto searchWktAccounts(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<WktaccountDto, oSearchWktAccountDto>().search(iSearch);
        }

        /// <summary>
        /// delivers a list of Account by the given query
        /// used by mass-letter gui
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, UriTemplate = "/searchAccountsWithQuery")]
        public oSearchAccountDto searchAccountsWithQuery(iSearchDto iSearch, String sql)
        {

            ServiceHandler<iSearchDto, oSearchAccountDto> ew = new ServiceHandler<iSearchDto, oSearchAccountDto>(iSearch);
            return ew.process(delegate(iSearchDto input, oSearchAccountDto rval, CredentialContext ctx)
            {

                if (input == null)
                    throw new ArgumentException("No search input");

                QueryInfoDataType1 infoData = new QueryInfoDataType1("PERSON", "PERSON.SYSPERSON");
                infoData.resultFields = "sysperson, matchcode, code,    syskdtyp,  anrede, anredecode, titel, titelCode, titel2,name, vorname, gebdatum, sysctlang, sysctlangkorr, syslandnat, einreisedatum, auslausweis, strasse, hsnr, plz, ort, sysland, sysstaat, wohnseit, strasse2, hsnr2, plz2, ort2, sysland2, sysstaat2, telefon, telefon2, handy, erreichbtel, erreichbvon, erreichbbis, email, url, sysbranche, rechtsform, rechtsformcode, gruendung, hregister, hregisterflag, revflag, zusatz, namekont, vornamekont,  fax, gebort, hregisterort, identeu, steuernr, identust, nomailingflag,privatflag, gesflag";
                infoData.resultTables = "CIC.PERSON PERSON ";
                infoData.searchTables = "CIC.PERSON PERSON ";
                infoData.permissionCondition = " and person.sysperson in (SELECT sysid FROM peuni, perolecache WHERE area = 'PERSON' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = {0} and {1}) ";
                infoData.searchConditions = " 1=1  ";
                //TODO make hint configurable
                infoData.queryStruct.partialQuery = "SELECT * FROM (SELECT rownum rnum, a.* FROM( select  {0} from person where sysperson in (" + sql + ") and {2}) a WHERE rownum <= {3}) WHERE rnum > {4}";
                infoData.queryStruct.countQuery = "select count(*) from person where sysperson in (" + sql + ") and {1}";
                infoData.optimized = false;

                rval.result = new SearchBo<AccountDto>(infoData).search(input);



            });
        }

        /// <summary>
        /// delivers a list of Wfuser
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchWfuserDto searchWfuser(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<WfuserDto, oSearchWfuserDto>().search(iSearch);
           
        }

        /// <summary>
        /// delivers a list of Oppo
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchOppoDto searchOpportunity(iSearchDto iSearch)
        {
            ServiceHandler<iSearchDto, oSearchOppoDto> ew = new ServiceHandler<iSearchDto, oSearchOppoDto>(iSearch);
            return ew.process(delegate(iSearchDto input, oSearchOppoDto rval, CredentialContext ctx)
            {

                if (input == null)
                    throw new ArgumentException("No search input");


                //First result: the ones directly on the person
                rval.result = new SearchBo<OpportunityDto>(SearchQueryFactoryFactory.getInstance(), ctx.getMembershipInfo().sysPEROLE).search(input);

                QueryInfoData qid = SearchQueryFactoryFactory.getInstance().getQueryInfo<OpportunityDto>();
                if (qid.resultTables.Contains("PTRELATE"))
                {
                    //OpportunityDto[] rval1 = bo.search(CreateISearchDto("OPPO.NAME, OPPO.DESCRIPTION", input.Filter, input.filters)).results;
                    List<OpportunityDto> rList1 = new List<OpportunityDto>(rval.result.results);

                    //second, the ones from the people added as beteiligter to the current oppo
                    //if a person-filter is given we can also look in crmnm
                    long sysperson = 0;
                    if (input.filters != null)
                    {

                        List<Filter> filters = new List<Filter>();
                        foreach (Filter f in input.filters)
                        {
                            if (f.fieldname.ToLower().IndexOf("sysperson") > -1 && f.filterType == FilterType.Equal)
                            {
                                try
                                {
                                    sysperson = long.Parse(f.value);
                                }
                                catch (Exception) { }
                            }
                            else filters.Add(f);
                        }
                        input.filters = filters.ToArray();
                    }

                    QueryInfoDataType1 infoData = new QueryInfoDataType1("OPPO", "OPPO.SYSOPPO");
                    infoData.resultFields = "OPPO.*, OPPOTP.NAME oppoTpBezeichnung, PERSON.NAME personName ";
                    infoData.resultTables = "CIC.OPPO OPPO, CIC.OPPOTP OPPOTP, CIC.PERSON PERSON, CIC.CRMNM CRMNM";
                    infoData.searchTables = "CIC.OPPO OPPO, CIC.OPPOTP OPPOTP, CIC.PERSON PERSON, CIC.CRMNM CRMNM";
                    infoData.permissionCondition = " and oppo.sysoppo in (SELECT sysid FROM peuni, perolecache WHERE area = 'OPPO' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = {0} and {1}) ";
                    if (sysperson > 0)
                        infoData.searchConditions = " OPPO.SYSOPPOTP = OPPOTP.SYSOPPOTP(+) and crmnm.sysidchild = PERSON.SYSPERSON and OPPO.SYSOPPO=CRMNM.SYSIDPARENT and CRMNM.PARENTAREA='OPPO' and CRMNM.CHILDAREA='PERSON' and crmnm.sysidchild is not null and crmnm.sysidchild=" + sysperson;
                    else infoData.searchConditions = " OPPO.SYSOPPOTP = OPPOTP.SYSOPPOTP(+) and crmnm.sysidchild = PERSON.SYSPERSON and OPPO.SYSOPPO=CRMNM.SYSIDPARENT and CRMNM.PARENTAREA='OPPO' and CRMNM.CHILDAREA='PERSON' and crmnm.sysidchild is not null ";

                    oSearchDto<OpportunityDto> s2result = new SearchBo<OpportunityDto>(infoData).search(input);
                    OpportunityDto[] rval2 = s2result.results;

                    List<OpportunityDto> rList2 = new List<OpportunityDto>(rval2);
                    int oldCount = rval.result.results.Length;

                    rval.result.results = rList1.Union(rval2, new EnityComparator<OpportunityDto>()).ToArray<OpportunityDto>();
                    rval.result.searchCountFiltered = rval.result.results.Length;
                    rval.result.searchCountMax += (rval.result.results.Length - oldCount);
                }


            });
        }

        /// <summary>
        /// delivers a list of Adrtp
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchAdrtpDto searchAdrtp(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<AdrtpDto, oSearchAdrtpDto>().search(iSearch);
          
        }

        /// <summary>
        /// delivers a list of Kontotp
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchKontotpDto searchKontotp(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<KontotpDto, oSearchKontotpDto>().search(iSearch);
        
        }

        /// <summary>
        /// delivers a list of Blz
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchBlzDto searchBlz(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<BlzDto, oSearchBlzDto>().search(iSearch);
          
        }

        /// <summary>
        /// delivers a list of Ptrelate
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchPtrelateDto searchPtrelates(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<PtrelateDto, oSearchPtrelateDto>().search(iSearch);
          
        }

        /// <summary>
        /// delivers a list of Crmnm
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchCrmnmDto searchCrmnms(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<CrmnmDto, oSearchCrmnmDto>().search(iSearch);
          
        }

        /// <summary>
        /// delivers a list of Partner
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchPartnerDto searchPartner(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<PartnerDto, oSearchPartnerDto>().search(iSearch);
          
        }

        /// <summary>
        /// delivers a list of Beteiligter
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchBeteiligterDto searchBeteiligter(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<BeteiligterDto, oSearchBeteiligterDto>().search(iSearch);
            
        }

        /// <summary>
        /// delivers a list of ddlkprub/Rubriken
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchDdlkprubDto searchDdlkprub(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<Cic.One.DTO.DdlkprubDto, oSearchDdlkprubDto>().search(iSearch);
           
        }

        /// <summary>
        /// delivers a list of ddlkprub/Rubriken, only the rubs for user-selected items
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest, UriTemplate = "/searchZusatzdaten")]
        public oSearchDdlkprubDto searchZusatzdaten(iSearchDto iSearch, Cic.One.DTO.igetRubDto rubInfo)
        {
            ServiceHandler<iSearchDto, oSearchDdlkprubDto> ew = new ServiceHandler<iSearchDto, oSearchDdlkprubDto>(iSearch);
            return ew.process(delegate(iSearchDto input, oSearchDdlkprubDto rval)
            {

                if (input == null)
                    throw new ArgumentException("No search input");


                //select ddlkprub.name , ddlkpcol.name, ddlkpspos.value from ddlkprub, ddlkpcol, ddlkpspos where ddlkpcol.sysddlkprub=ddlkprub.sysddlkprub and ddlkpspos.sysddlkpcol=ddlkpcol.sysddlkpcol;


                QueryInfoDataType1 infoData = new QueryInfoDataType1("DDLKPCOL", "ddlkpcol.sysddlkpcol");
                infoData.resultFields = " ddlkprub.name zusatzRub, ddlkpcol.name zusatzCol, ddlkpspos.value zusatzValue, ddlkprub.sysddlkprub sysddlkprub, ddlkpspos.sysddlkpspos sysddlkpspos";
                infoData.resultTables = "cic.ddlkprub, cic.ddlkpcol, cic.ddlkpspos";
                infoData.searchTables = "cic.ddlkprub, cic.ddlkpcol, cic.ddlkpspos";
                infoData.searchConditions = " ddlkpcol.sysddlkprub=ddlkprub.sysddlkprub and ddlkpspos.sysddlkpcol=ddlkpcol.sysddlkpcol ";// and ddlkprub.area='"+rubInfo.crmarea+"' and ddlkpspos.area='"+rubInfo.area+"' and ddlkpspos.sysid="+rubInfo.areaid;

                input.sortFields = new Sorting[2];
                Sorting sort = new Sorting();
                sort.fieldname = "ddlkprub.rank";
                sort.order = SortOrder.Asc;
                input.sortFields[0] = sort;
                sort = new Sorting();
                sort.fieldname = "ddlkpcol.rank";
                sort.order = SortOrder.Asc;
                input.sortFields[1] = sort;
                oSearchDto<Cic.One.DTO.DdlkprubDto> sResult = new SearchBo<Cic.One.DTO.DdlkprubDto>(infoData).search(input);

              
                String lastRub = "";

                foreach (Cic.One.DTO.DdlkprubDto rub in sResult.results)
                {
                    if (rub.zusatzRub != lastRub)
                    {
                        lastRub = rub.zusatzRub;
                    }
                    else
                    {
                        rub.zusatzRub = "";
                    }
                }


                rval.result = sResult;

            });
        }

        /// <summary>
        /// delivers a list of Ddlkpcol/Wertebereiche 
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchDdlkpcolDto searchDdlkpcol(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<Cic.One.DTO.DdlkpcolDto, oSearchDdlkpcolDto>().search(iSearch);
            
        }

        /// <summary>
        /// delivers a list of Ddlkppos/Werte
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchDdlkpposDto searchDdlkppos(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<Cic.One.DTO.DdlkpposDto, oSearchDdlkpposDto>().search(iSearch);
         
        }

        /// <summary>
        /// delivers a list of Ddlkpspos/WerteSelektiert
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchDdlkpsposDto searchDdlkpspos(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<Cic.One.DTO.DdlkpsposDto, oSearchDdlkpsposDto>().search(iSearch);
          
        }

        /// <summary>
        /// delivers a list of Camptp
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchCamptpDto searchCamptp(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<CamptpDto, oSearchCamptpDto>().search(iSearch);
           
        }

        /// <summary>
        /// delivers a list of Oppotp
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchOppotpDto searchOppotp(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<OppotpDto, oSearchOppotpDto>().search(iSearch);
           
        }

        /// <summary>
        /// delivers a list of Crmpr
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchCrmprDto searchCrmpr(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<CrmprDto, oSearchCrmprDto>().search(iSearch);
            
        }

        /// <summary>
        /// delivers a list of contacttp
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchContacttpDto searchContacttp(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<ContacttpDto, oSearchContacttpDto>().search(iSearch);
          
        }

        /// <summary>
        /// delivers a list of Itemcatm
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchItemcatmDto searchItemcatm(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<ItemcatmDto, oSearchItemcatmDto>().search(iSearch);
          
        }

        /// <summary>
        /// delivers a list of Recurr
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchRecurrDto searchRecurr(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<RecurrDto, oSearchRecurrDto>().search(iSearch);
           
        }

        /// <summary>
        /// delivers a list of Ptype
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchPtypeDto searchPtype(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<PtypeDto, oSearchPtypeDto>().search(iSearch);
            
        }


        /// <summary>
        /// delivers a list of Prunstep
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchPrunstepDto searchPrunstep(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<PrunstepDto, oSearchPrunstepDto>().search(iSearch);
          
        }

        /// <summary>
        /// delivers a list of Pstep
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchPstepDto searchPstep(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<PstepDto, oSearchPstepDto>().search(iSearch);
            
        }

        /// <summary>
        /// delivers a list of Prkgroup
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchPrkgroupDto searchPrkgroup(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<PrkgroupDto, oSearchPrkgroupDto>().search(iSearch);
          
        }

        /// <summary>
        /// delivers a list of Prkgroupm
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchPrkgroupmDto searchPrkgroupm(iSearchDto iSearch)
        {
            ServiceHandler<iSearchDto, oSearchPrkgroupmDto> ew = new ServiceHandler<iSearchDto, oSearchPrkgroupmDto>(iSearch);
            return ew.process(delegate(iSearchDto input, oSearchPrkgroupmDto rval)
            {

                if (input == null)
                    throw new ArgumentException("No search input");


                long sysperson = 0;
                if (input.filters != null)
                {
                    List<Filter> filters = new List<Filter>();
                    foreach (Filter f in input.filters)
                    {
                        if (f.fieldname.ToLower().IndexOf("sysperson") > -1)
                        {
                            sysperson = long.Parse(input.filters[0].value);
                        }
                        else filters.Add(f);
                    }
                    input.filters = filters.ToArray();
                }

                if (sysperson == 0)
                {
                    rval.result = new SearchBo<PrkgroupmDto>(SearchQueryFactoryFactory.getInstance()).search(input);
                }
                else
                {
                    QueryInfoDataType1 infoData = new QueryInfoDataType1("PRKGROUP", "PRKGROUP.SYSPRKGROUPM");
                    infoData.resultFields = " PRKGROUP.NAME, PRKGROUP.DESCRIPTION, PRKGROUP.SYSPRKGROUP,prkgroupm.SYSPRKGROUPM, prkgroupm.SYSPERSON, prkgroupm.ACTIVEFLAG, prkgroupm.VALIDFROM, prkgroupm.VALIDUNTIL, prkgroupm.flagmanuell";
                    infoData.resultTables = "CIC.PRKGROUP PRKGROUP,  (select * from cic.prkgroupm where  PRKGROUPM.SYSPRKGROUP = SYSPRKGROUP AND SYSPERSON = " + sysperson + ") prkgroupm";
                    infoData.searchTables = "CIC.PRKGROUP PRKGROUP,  (select * from cic.prkgroupm where  PRKGROUPM.SYSPRKGROUP = SYSPRKGROUP AND SYSPERSON = " + sysperson + ") prkgroupm";
                    infoData.searchConditions = " PRKGROUP.SYSPRKGROUP = PRKGROUPM.SYSPRKGROUP(+) AND PRKGROUP.ACTIVEFLAG = 1 ";
                    rval.result = new SearchBo<PrkgroupmDto>(infoData).search(input);
                }


            });
        }

        /// <summary>
        /// delivers a list of Prkgroupz
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchPrkgroupzDto searchPrkgroupz(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<PrkgroupzDto, oSearchPrkgroupzDto>().search(iSearch);
           
        }

        /// <summary>
        /// delivers a list of Prkgroups
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchPrkgroupsDto searchPrkgroups(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<PrkgroupsDto, oSearchPrkgroupsDto>().search(iSearch);
           
        }

        /// <summary>
        /// delivers a list of Pstep
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchSegDto searchSeg(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<SegDto, oSearchSegDto>().search(iSearch);
          
        }

        /// <summary>
        /// delivers a list of Segc
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchSegcDto searchSegc(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<SegcDto, oSearchSegcDto>().search(iSearch);
           
        }

        /// <summary>
        /// delivers a list of Stickynote
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchStickynoteDto searchStickynote(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<StickynoteDto, oSearchStickynoteDto>().search(iSearch);
         
        }

        /// <summary>
        /// delivers a list of Notizen
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchNotizDto searchNotiz(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<NotizDto, oSearchNotizDto>().search(iSearch);
            
        }

        /// <summary>
        /// delivers a list of Stickytype
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchStickytypeDto searchStickytype(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<StickytypeDto, oSearchStickytypeDto>().search(iSearch);
           
        }



        /// <summary>
        /// delivers a list of RegVars
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchRegVarDto searchRegVars(iSearchDto iSearch)
        {
            SearchCache.entityChanged("REGVAR");//always fresh db load
            return new SearchHandlerFactory<RegVarDto, oSearchRegVarDto>().search(iSearch);
            
        }

        /// <summary>
        /// delivers a list of Wfsignature
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchWfsignatureDto searchWfsignature(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<WfsignatureDto, oSearchWfsignatureDto>().search(iSearch);
          
        }

        /*
        public object executeScript(String input)
        {
            Session s = Session.Create(this);
            Roslyn.Scripting.CSharp.ScriptEngine engine = new Roslyn.Scripting.CSharp.ScriptEngine(new Assembly[]
                               {
                                   typeof(Console).Assembly,
                                   typeof(IEnumerable<>).Assembly,
                                   typeof(IQueryable).Assembly,
                                   typeof(EntityBo).Assembly,
                                   typeof(EntityDao).Assembly
                               },
                               new string[] 
                               { 
                                   "System", "System.Linq", 
                                   "System.Collections",
                                   "System.Collections.Generic",
                                   "Cic.One.Web.BO",
                                   "Cic.One.Web.DAO",
                                   "Cic.One.DTO"
                               }
                           );
           return engine.Execute(input,s);
        }*/


        /// <summary>
        /// delivers a list of AngVar
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchAngvarDto searchAngvar(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<AngvarDto, oSearchAngvarDto>().search(iSearch);
            
        }

        /// <summary>
        /// delivers a list of Angebot
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchAngebotDto searchAngebot(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<AngebotDto, oSearchAngebotDto>().search(iSearch);
            
        }

        /// <summary>
        /// delivers a list of BN Angebot
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchBNAngebotDto searchBNAngebot(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<BNAngebotDto, oSearchBNAngebotDto>().search(iSearch);

        }

        /// <summary>
        /// delivers a list of BN Angebot
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchBNAntragDto searchBNAntrag(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<BNAntragDto, oSearchBNAntragDto>().search(iSearch);

        }

        /// <summary>
        /// delivers a list of Antrag
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchAntragDto searchAntrag(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<AntragDto, oSearchAntragDto>().search(iSearch);

        }

        /// <summary>
        /// delivers a list of Vertrg
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchVertragDto searchVertrag(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<VertragDto, oSearchVertragDto>().search(iSearch);
           
        }

        /// <summary>
        /// delivers a list of AngOb
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchAngobDto searchAngob(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<AngobDto, oSearchAngobDto>().search(iSearch);
           
        }

        /// <summary>
        /// delivers a list of Angkalk
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchAngkalkDto searchAngkalk(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<AngkalkDto, oSearchAngkalkDto>().search(iSearch);
          
        }

        /// <summary>
        /// delivers a list of Angobsl
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchAngobslDto searchAngobsl(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<AngobslDto, oSearchAngobslDto>().search(iSearch);
            
        }

        /// <summary>
        /// delivers a list of Angobslpos
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchAngobslposDto searchAngobslpos(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<AngobslposDto, oSearchAngobslposDto>().search(iSearch);
           
        }

        /// <summary>
        /// delivers a list of AntOb
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchAntobDto searchAntob(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<AntobDto, oSearchAntobDto>().search(iSearch);
        
        }

        /// <summary>
        /// delivers a list of Antobsl
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchAntobslDto searchAntobsl(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<AntobslDto, oSearchAntobslDto>().search(iSearch);
           
        }

        /// <summary>
        /// delivers a list of Antobsl
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchAntobslposDto searchAntobslpos(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<AntobslposDto, oSearchAntobslposDto>().search(iSearch);
           
        }

        /// <summary>
        /// delivers a list of Ob
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchObDto searchOb(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<ObDto, oSearchObDto>().search(iSearch);
        
        }

        /// <summary>
        /// delivers a list of Vtobsl
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchVtobslDto searchVtobsl(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<VtobslDto, oSearchVtobslDto>().search(iSearch);
           
        }

        /// <summary>
        /// delivers a list of Vtobslpos
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchVtobslposDto searchVtobslpos(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<VtobslposDto, oSearchVtobslposDto>().search(iSearch);
          
        }

        /// <summary>
        /// delivers a list of Inbox-Items
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [WebInvoke(Method = "POST",RequestFormat = WebMessageFormat.Json,ResponseFormat = WebMessageFormat.Json,BodyStyle = WebMessageBodyStyle.Bare,UriTemplate = "/searchInbox")]
        public oSearchInboxDto searchInbox(iSearchDto iSearch)
        {
            oSearchInboxDto result = new SearchHandlerFactory<InboxDto, oSearchInboxDto>().search(iSearch);
            if (result == null || result.result == null || result.result.results == null)
                return result;

            try
            {
                Cic.OpenOne.Common.DAO.IDictionaryListsDao dao = DAOFactoryFactory.getInstance().getDictionaryListsDao();
                foreach (InboxDto inbox in result.result.results)
                {
					if (inbox.syshaendler == 0)
                        continue;

                    inbox.prio = dao.getPrioHaendler(inbox.syshaendler);
                    if (inbox.prio > 0)
                        inbox.indicatorContent = "prio" + inbox.prio.ToString();

                }
            }
            catch (Exception e)
            {
                log.Error("Fehler beim Ermitteln der Händlerprioritäten.", e);
            }

            return result;
        }

        /// <summary>
        /// delivers a list of Process Items from BPE
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchProcessDto searchProcess(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<ProcessDto, oSearchProcessDto>().search(iSearch);

        }

        /// <summary>
        /// delivers a list of Dmsdocs
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchDmsdocDto searchDmsdoc(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<DmsdocDto, oSearchDmsdocDto>().search(iSearch);

        }

        /// <summary>
        /// delivers a list of PUsers
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchPuserDto searchPuser(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<PuserDto, oSearchPuserDto>().search(iSearch);

        }

        /// <summary>
        /// delivers a list of Aufträge
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchAuftragDto searchAuftrag(iSearchDto iSearch)
        {
            return new SearchHandlerFactory<AuftragDto, oSearchAuftragDto>().search(iSearch);

        }



    }
}
