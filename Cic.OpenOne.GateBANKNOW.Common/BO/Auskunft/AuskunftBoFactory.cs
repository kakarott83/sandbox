using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.Common.DAO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.EQUIFAX;
    using CRIF;
    using Schufa;

    /// <summary>
    /// AuskunftBoFactory
    /// </summary>
    [System.CLSCompliant(false)]
    public class AuskunftBoFactory : AuskunftManagement.Common.BO.IAuskunftBoFactory
    {
        /// <summary>
        /// Static method to create DecisionEngineBo
        /// </summary>
        /// <returns></returns>
        public static IDecisionEngineGuardeanBo CreateDefaultDecisionEngineGuardeanBo()
        {
            IDecisionEngineGuardeanWSDao Dewsdao = new DecisionEngineGuardeanWSDao();
            DecisionEngineGuardeanStatusUpdateWSDao Desuwsdao = new DecisionEngineGuardeanStatusUpdateWSDao();
            IDecisionEngineGuardeanDBDao Dedbdao = new DecisionEngineGuardeanDBDao();
            IAuskunftDao Auskunftdao = new AuskunftDao();
            ILandDao LandDao = new LandDao();
            return new DecisionEngineGuardeanBo(Dewsdao, Desuwsdao, Dedbdao, Auskunftdao, LandDao);
        }
        
        /// <summary>
        /// Static method to create AggregationBo
        /// </summary>
        /// <returns></returns>
        public static IAggregationBo CreateDefaultAggregationBo()
        {
            IAggregationDao AggregationDao = new AggregationDao();
            IAuskunftDao Auskunftdao = new AuskunftDao();
            return new AggregationBo(AggregationDao, Auskunftdao);
        }

        /// <summary>
        /// Static method to create KREMOBo
        /// </summary>
        /// <returns></returns>
        public static IKREMOBo CreateDefaultKREMOBo()
        {
            IKREMOWSDao Kremowsdao = new KREMOWSDao();
            IKREMODBDao Kremodbdao = new KREMODBDao();
            IAuskunftDao Auskunftdao = new AuskunftDao();
            return new KREMOBo(Kremowsdao, Kremodbdao, Auskunftdao);
        }

        /// <summary>
        /// Static method to create DeltavistaBo
        /// </summary>
        /// <returns></returns>
        public static IDeltavistaBo CreateDefaultDeltavistaBo()
        {
            IDeltavistaWSDao Dvwsdao = new DeltavistaWSDao();
            IDeltavistaDBDao Dvdbdao = new DeltavistaDBDao();
            IAuskunftDao Auskunftdao = new AuskunftDao();
            return new DeltavistaBo(Dvwsdao, Dvdbdao, Auskunftdao);
        }

        /// <summary>
        /// Static method to create ZekBo
        /// </summary>
        /// <returns></returns>
        public static IZekBo CreateDefaultZekBo()
        {
            IZekWSDao Zekwsdao = new ZekWSDao();
            IZekDBDao Zekdbdao = new ZekDBDao();
            IAuskunftDao Auskunftdao = new AuskunftDao();
            return new ZekBo(Zekwsdao, Zekdbdao, Auskunftdao);
        }

        /// <summary>
        /// Static method to create ZekBatchBo
        /// </summary>
        /// <returns></returns>
        public static IZekBatchBo CreateDefaultZekBatchBo()
        {
            IZekBatchWSDao ZekBatchwsdao = new ZekBatchWSDao();
            IZekBatchDBDao ZekBatchdbdao = new ZekBatchDBDao();
            IAuskunftDao Auskunftdao = new AuskunftDao();
            return new ZekBatchBo(ZekBatchwsdao, ZekBatchdbdao, Auskunftdao);
        }

        /// <summary>
        /// Static method to create DecisionEngineBo
        /// </summary>
        /// <returns></returns>
        public static IDecisionEngineBo CreateDefaultDecisionEngineBo()
        {
            IDecisionEngineWSDao Dewsdao = new DecisionEngineWSDao();
            IDecisionEngineDBDao Dedbdao = new DecisionEngineDBDao();
            IAuskunftDao Auskunftdao = new AuskunftDao();
            return new DecisionEngineBo(Dewsdao, Dedbdao, Auskunftdao);
        }

        /// <summary>
        /// Static method to create EurotaxBo
        /// </summary>
        /// <returns></returns>
        public static IEurotaxBo CreateDefaultEurotaxBo()
        {
            IEurotaxWSDao Eurotaxwsdao = new EurotaxWSDao();
            IEurotaxDBDao Eurotaxdbdao = new EurotaxDBDao();
            IAuskunftDao Auskunftdao = new AuskunftDao();
            IVGDao VGDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getVGDao();
            IObTypDao obtypDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao();
            return new EurotaxBo(Eurotaxwsdao, Eurotaxdbdao, Auskunftdao, VGDao, obtypDao);
        }

        /// <summary>
        /// Static method to create S1Bo
        /// </summary>
        /// <returns></returns>
        public static IRISKEWBS1Bo CreateDefaultRISKEWBS1Bo()
        {
            return new RISKEWBS1Bo();
        }

        public static ISchufaWSDao CreateSchufaWsDao()
        {
            return new SchufaWSDao();
        }

        public static ISchufaDBDao CreateSchufaDBDao()
        {
            return new SchufaDBDao();
        }

        public static IAuskunftDao CreateAuskunftDao()
        {
            return new AuskunftDao();
        }

        public static IAuskunftBo CreateCrifIdentifyAddressBO()
        {
            return new CrifIdentifyAddressBo(new CrifWSDao(), new CrifDBDao(), new AuskunftDao());
        }

        //###
        public static IAuskunftBo CreateCrifGetArchivedReportBO()
        {
            return new CrifGetArchivedReportBo(new CrifWSDao(), new CrifDBDao(), new AuskunftDao());
        }

        public static IAuskunftBo CreateCrifGetListOfReadyOfflineReportsBO()
        {
            return new CrifGetListOfReadyOfflineReportsBo(new CrifWSDao(), new CrifDBDao(), new AuskunftDao());
        }

        public static IAuskunftBo CreateCrifPollOfflineReportBO()
        {
            return new CrifPollOfflineReportBo(new CrifWSDao(), new CrifDBDao(), new AuskunftDao());
        }

        public static IAuskunftBo CreateCrifOrderOfflineReportBO()
        {
            return new CrifOrderOfflineReportBo(new CrifWSDao(), new CrifDBDao(), new AuskunftDao());
        }

        public static IAuskunftBo CreateCrifGetDebtDetailsBO()
        {
            return new CrifGetDebtDetailsBo(new CrifWSDao(), new CrifDBDao(), new AuskunftDao());
        }

        public static IAuskunftBo CreateCrifGetReportBO(string auskunfttyp)
        {
            return new CrifGetReportBo(new CrifWSDao(), new CrifDBDao(), new AuskunftDao(), auskunfttyp);
        }

        public static IAuskunftBo CreateCrifKontrollinhaberBO(string auskunfttyp)
        {
            return new CrifKontrollinhaberBo(new CrifWSDao(), new CrifDBDao(), new AuskunftDao(), auskunfttyp);
        }

        public static IAuskunftBo CreateEquifaxBO()
        {
            return new EQUIFAXBo(new EQUIFAXDao(), new AuskunftDao());
        }
        public Cic.OpenOne.AuskunftManagement.Common.BO.SF.ICommonAuskunftBo getServiceFacadeObject(string bezeichnung)
        {
            return getServiceFacade(bezeichnung);
        }

        /// <summary>
        /// Instantiiert die im Auskunfttypen angegebene ServiceFacade
        /// </summary>
        /// <param name="bezeichnung"></param>
        /// <returns>IAuskunftBo</returns>
        public static IAuskunftBo getServiceFacade(string bezeichnung)
        {
            IAuskunftBo auskunftBo = null;
            if (bezeichnung.Equals(AuskunfttypDao.KREMOCallByValues))
            {
                auskunftBo = new KREMOcallByValues();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.EurotaxGetForecast))
            {
                auskunftBo = new EurotaxGetForecast();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.EurotaxGetRemo))
            {
                auskunftBo = new EurotaxGetRemo();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.EurotaxGetValuation))
            {
                auskunftBo = new EurotaxGetValuation();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.DecisionEngineExecute))
            {
                auskunftBo = new DecisionEngineExecute();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.DeltavistaGetAddressId))
            {
                auskunftBo = new DVAddressIdentification();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.DeltavistaGetAddressIdArb))
            {
                auskunftBo = new DVAddressIdentificationArb();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.DeltavistaGetCompanyDetails))
            {
                auskunftBo = new DVgetCompanyDetails();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.DeltavistaGetDebtDetails))
            {
                auskunftBo = new DVgetDebtDetails();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.DeltavistaGetReport))
            {
                auskunftBo = new DVgetReport();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.DeltavistaOrderCresuraReport))
            {
                auskunftBo = new DVorderCresuraReport();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.ZEKInformativabfrage))
            {
                auskunftBo = new ZekInformativabfrage();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.ZEKKreditgesuchAblehnen))
            {
                auskunftBo = new ZekKreditgesuchAblehnen();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.ZEKKreditgesuchNeu))
            {
                auskunftBo = new ZekKreditgesuchNeu();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.ZEKMeldungKartenengagement))
            {
                auskunftBo = new ZekMeldungKartenengagement();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.ZEKMeldungUeberziehungskredit))
            {
                auskunftBo = new ZekMeldungUeberziehungskredit();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.ZEKRegisterBardarlehen))
            {
                auskunftBo = new ZekRegisterBardarlehen();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.ZEKRegisterFestkredit))
            {
                auskunftBo = new ZekRegisterFestkredit();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.ZEKRegisterKontokorrentkredit))
            {
                auskunftBo = new ZekRegisterKontokorrentkredit();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.ZEKRegisterLeasingMietvertrag))
            {
                auskunftBo = new ZekRegisterLeasingMietvertrag();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.ZEKRegisterTeilzahlungskredit))
            {
                auskunftBo = new ZekRegisterTeilzahlungskredit();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.ZEKRegisterTeilzahlungsvertrag))
            {
                auskunftBo = new ZekRegisterTeilzahlungsvertrag();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.ZEKUpdateAddress))
            {
                auskunftBo = new ZekUpdateAddress();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.ZEKUpdateBardarlehen))
            {
                auskunftBo = new ZekUpdateBardarlehen();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.ZEKUpdateFestkredit))
            {
                auskunftBo = new ZekUpdateFestkredit();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.ZEKUpdateKontokorrentkredit))
            {
                auskunftBo = new ZekUpdateKontokorrentkredit();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.ZEKUpdateLeasingMietvertrag))
            {
                auskunftBo = new ZekUpdateLeasingMietvertrag();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.ZEKUpdateTeilzahlungskredit))
            {
                auskunftBo = new ZekUpdateTeilzahlungskredit();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.ZEKUpdateTeilzahlungsvertrag))
            {
                auskunftBo = new ZekUpdateTeilzahlungsvertrag();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.ZEKCloseBardarlehen))
            {
                auskunftBo = new ZEKCloseBardarlehen();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.ZEKCloseLeasingMietvertrag))
            {
                auskunftBo = new ZEKCloseLeasingMietvertrag();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.ZEKCloseFestkredit))
            {
                auskunftBo = new ZEKCloseFestkredit();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.ZEKCloseTeilzahlungskredit))
            {
                auskunftBo = new ZEKCloseTeilzahlungskredit();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.ZEKCloseTeilzahlungsvertrag))
            {
                auskunftBo = new ZekCloseTeilzahlungsvertrag();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.ZEKCloseKontokorrentkredit))
            {
                auskunftBo = new ZEKCloseKontokorrentkredit();
            }
            //Ecode178
            else if (bezeichnung.Equals(AuskunfttypDao.ZEKeCode178Abfragen))
            {
                auskunftBo = new ZekeCode178Abfrage();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.ZEKeCode178Abmelden))
            {
                auskunftBo = new ZekeCode178Abmelden();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.ZEKeCode178Anmelden))
            {
                auskunftBo = new ZekeCode178Anmelden();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.ZEKeCode178Mutieren))
            {
                auskunftBo = new ZekeCode178Mutieren();
            }

            //ZekGetArms
            else if (bezeichnung.Equals(AuskunfttypDao.ZEKgetARMs))
            {
                auskunftBo = new ZekGetARMs();
            }

            // Zek Batch
            else if (bezeichnung.Equals(AuskunfttypDao.ZEKBatchCloseContracts))
            {
                auskunftBo = new ZEKBatchCloseContracts();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.ZEKBatchUpdateContracts))
            {
                auskunftBo = new ZEKBatchUpdateContracts();
            }

            // Aggregation
            else if (bezeichnung.Equals(AuskunfttypDao.AggregationCallByValues))
            {
                auskunftBo = new AggregationCallByValues();
            }

            // S1 - RISKEWB_S1
            else if (bezeichnung.Equals(AuskunfttypDao.RISKEWB_S1))
            {
                auskunftBo = new RISKEWBS1sendData();
            }
            //Vorinkassoscore
            else if (bezeichnung.Equals(AuskunfttypDao.VI_SCORE_S1))
            {
                auskunftBo = new RISKEWBS1sendData();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.CreditDecision))
            {
                auskunftBo = new DecisionEngineGuardeanExecute();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.CreditDecisionStatusUpdate))
            {
                auskunftBo = new DecisionEngineGuardeanStatusUpdateExecute();
            }

            else if (bezeichnung.Equals(AuskunfttypDao.SchufaKorrekturAdresse))
            {
                auskunftBo = new SchufaKorrekturAdresse(CreateSchufaWsDao(), CreateSchufaDBDao(), CreateAuskunftDao());
            }
            else if (bezeichnung.Equals(AuskunfttypDao.SchufaMeldungVertragsdaten))
            {
                auskunftBo = new SchufaMeldungVertragsdaten(CreateSchufaWsDao(), CreateSchufaDBDao(), CreateAuskunftDao());
            }
            else if (bezeichnung.Equals(AuskunfttypDao.SchufaKorrekturVerbraucherdaten))
            {
                auskunftBo = new SchufaKorrekturVerbraucherdaten(CreateSchufaWsDao(), CreateSchufaDBDao(), CreateAuskunftDao());
            }
            else if (bezeichnung.Equals(AuskunfttypDao.SchufaLoeschungTodesfall))
            {
                auskunftBo = new SchufaLoeschungTodesfall(CreateSchufaWsDao(), CreateSchufaDBDao(), CreateAuskunftDao());
            }
            else if (bezeichnung.Equals(AuskunfttypDao.SchufaNamensAenderung))
            {
                auskunftBo = new SchufaNamensAenderung(CreateSchufaWsDao(), CreateSchufaDBDao(), CreateAuskunftDao());
            }
            else if (bezeichnung.Equals(AuskunfttypDao.SchufaNeumeldungAdresse))
            {
                auskunftBo = new SchufaNeumeldungAdresse(CreateSchufaWsDao(), CreateSchufaDBDao(), CreateAuskunftDao());
            }
            else if (bezeichnung.Equals(AuskunfttypDao.SchufaNeumeldungTodesfall))
            {
                auskunftBo = new SchufaNeumeldungTodesfall(CreateSchufaWsDao(), CreateSchufaDBDao(), CreateAuskunftDao());
            }
            else if (bezeichnung.Equals(AuskunfttypDao.SchufaAbrufNachmeldung))
            {
                auskunftBo = new SchufaAbrufNachmeldung(CreateSchufaWsDao(), CreateSchufaDBDao(), CreateAuskunftDao());
            }
            else if (bezeichnung.Equals(AuskunfttypDao.SchufaAnfrageBonitaetsAuskunft))
            {
                auskunftBo = new SchufaAnfrageBonitaetsauskunft(CreateSchufaWsDao(), CreateSchufaDBDao(), CreateAuskunftDao());
            }
            else if (bezeichnung.Equals(AuskunfttypDao.SchufaAbrufManuelleWeiterverarbeitung))
            {
                auskunftBo = new SchufaAbrufManuelleWeiterverarbeitung(CreateSchufaWsDao(), CreateSchufaDBDao(), CreateAuskunftDao());
            }

            else if (bezeichnung.Equals(AuskunfttypDao.CrifIdentifyAddress))
            {
                auskunftBo = CreateCrifIdentifyAddressBO();
            }

            //###
            else if (bezeichnung.Equals(AuskunfttypDao.CrifGetArchivedReport))
            {
                auskunftBo = CreateCrifGetArchivedReportBO();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.CrifGetListOfReadyOfflineReports))
            {
                auskunftBo = CreateCrifGetListOfReadyOfflineReportsBO();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.CrifPollOfflineReport))
            {
                auskunftBo = CreateCrifPollOfflineReportBO();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.CrifGetOfflineReport))
            {
                auskunftBo = CreateCrifOrderOfflineReportBO();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.CrifGetDebtDetail))
            {
                auskunftBo = CreateCrifGetDebtDetailsBO();
            }
            else if (bezeichnung.Equals(AuskunfttypDao.CrifGetReport))
            {
                auskunftBo = CreateCrifGetReportBO(AuskunfttypDao.CrifGetReport);
            }
            else if (bezeichnung.Equals(AuskunfttypDao.CrifGetReportArb))
            {
                auskunftBo = CreateCrifGetReportBO(AuskunfttypDao.CrifGetReportArb);
            }
            else if (bezeichnung.Equals(AuskunfttypDao.CrifKontrollinhaber))
            {
                auskunftBo = CreateCrifKontrollinhaberBO(AuskunfttypDao.CrifKontrollinhaber);
            }
            else if (bezeichnung.Equals(AuskunfttypDao.EQUIFAXRisk))
            {
                auskunftBo = CreateEquifaxBO();
            }

            return auskunftBo;
        }
    }
}