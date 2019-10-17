using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.Common.BO;

using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    using Cic.OpenOne.Common.BO.Versicherung;
    using OpenOne.Common.DAO.Sms;

    /// <summary>
    /// Factory Class für creating CommonBOs with default DAOs
    /// </summary>
    public class BOFactory
    {

        private static BOFactory _self = null;
        private static string LOCK = "LOCK";

        /// <summary>
        /// Instanz der Prisma DAO Factory erzeugen
        /// </summary>
        /// <returns></returns>
        public static BOFactory getInstance()
        {
            lock (LOCK)
            {
                if (_self == null)
                {
                    _self = new BOFactory();
                }
            }
            return _self;
        }

        /// <summary>
        /// Konstruktor of BOFactory
        /// </summary>
        private BOFactory()
        {

        }

        /// <summary>
        /// FactoryCreate für DMS
        /// </summary>
        /// <returns>BO</returns>
        public IDMSBo createDMSBo()
        {
            return new DMSBo();
        }

        public IDMRBo createDMRBo()
        {
            return new DMRBo(Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getDMRDao());
        }

        /// <summary>
        /// FactoryCreate für DocumentService
        /// </summary>
        /// <returns>BO</returns>
        public IDocumentServiceBo createDocumentServiceBo()
        {
            return new DocumentServiceBo(Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getDocumentServiceDao());
        }

        /// <summary>
        /// FactoryCreate für AdresseBo
        /// </summary>
        /// <returns>BO</returns>
        public IAdresseBo createAdresseBo()
        {
            return new AdresseBo(Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getAdresseDao());
        }

        /// <summary>
        /// FactoryCreate für AngAntBo
        /// </summary>
        /// <returns>BO</returns>
        public IAngAntBo createAngAntBo()
        {

            return new AngAntBo(Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getAngAntDao(), new KundeDao(), new PrismaParameterBo(PrismaDaoFactory.getInstance().getPrismaDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao(), PrismaParameterBo.CONDITIONS_BANKNOW), new TranslateBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao()), OpenOne.Common.DAO.CommonDaoFactory.getInstance().getQuoteDao(), new VGDao(), Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getEaihotDao(), createTransactionRisikoBO());
        }

        /// <summary>
        /// FactoryCreate für AngAntBo MA Client
        /// </summary>
        /// <returns>BO</returns>
        public IAngAntBo createAngAntBoMA()
        {

            return new AngAntBo(Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getAngAntDaoMA(), new KundeDao(), new PrismaParameterBo(PrismaDaoFactory.getInstance().getPrismaDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao(), PrismaParameterBo.CONDITIONS_BANKNOW), new TranslateBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao()), OpenOne.Common.DAO.CommonDaoFactory.getInstance().getQuoteDao(), new VGDao(), Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getEaihotDao(), createTransactionRisikoBO());
        }

        /// <summary>
        /// FactoryCreate für DictionaryListBo
        /// </summary>
        /// <returns>BO</returns>
        public IDictionaryListsBo createDictionaryListsBo( String langCode)
        {
            return new DictionaryListsBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getDictionaryListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao(), langCode);
        }

        /// <summary>
        /// FactoryCreate für ItBo
        /// </summary>
        /// <returns>BO</returns>
        public IItBo createItBo()
        {
            return new ItBo(new ItDao());
        }

        /// <summary>
        /// FactoryCreate für ItBo
        /// </summary>
        /// <returns>BO</returns>
        public IKalkulationBo createKalkulationBo(String langCode)
        {
            return new KalkulationBo(new KalkulationDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getZinsDao(), PrismaDaoFactory.getInstance().getPrismaDao(), Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getAngAntDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getVGDao(), new EurotaxWSDao(), new EurotaxDBDao(), new AuskunftDao(), new KundeDao(), PrismaDaoFactory.getInstance().getProvisionDao(), PrismaDaoFactory.getInstance().getSubventionDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getInsuranceDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getMwStDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getQuoteDao(), langCode, PrismaDaoFactory.getInstance().getPrismaServiceDao());
        }

        /// <summary>
        /// FactoryCreate für KontoBo
        /// </summary>
        /// <returns>BO</returns>
        public IKontoBo createKontoBo()
        {
            return new KontoBo(new KontoDao());
        }

        /// <summary>
        /// FactoryCreate für KundeBo
        /// </summary>
        /// <returns>BO</returns>
        public IKundeBo createKundeBo()
        {
            return new KundeBo(Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getKundeDao());
        }

        /// <summary>
        /// Creates a Disclaimer management BO
        /// </summary>
        /// <returns></returns>
        public IDisclaimerBo createDisclaimerBo()
        {
            return new DisclaimerBo();
        }

        /// <summary>
        /// FactoryCreate für NotificationGatewayBo
        /// </summary>
        /// <returns>BO</returns>
        public INotificationGatewayBo createNotificationGateway()
        {
            return new NotificationGatewayBo(new NotificationGatewayDbDao(), new NotificationGatewaySmtpDao(), new SwisscomSmsDao());
        }

        /// <summary>
        /// FactoryCreate für RoleContextListsBo
        /// </summary>
        /// <returns>BO</returns>
        public IRoleContextListsBo createRoleContextListBo()
        {
            return new RoleContextListsBo(new RoleContextListsDao(),Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao() );
        }

        /// <summary>
        /// FactoryCreate für SchnellKalkulationBo
        /// </summary>
        /// <returns>BO</returns>
        public ISchnellKalkulationBo createSchnellkalkulationBo()
        {
            return new SchnellKalkulationBo(new SchnellkalkulationDao());
        }

  /*      /// <summary>
        /// FactoryCreate für SearchBo
        /// </summary>
        /// <returns>BO</returns>
        public ISearch<T> createSearch<T>()
        {
            return new SearchBo<T>();
        }

        /// <summary>
        /// FactoryCreate für SearchBo
        /// </summary>
        /// <returns>BO</returns>
        public ISearch<T> createSearch<T>(string Table)
        {
            return new SearchBo<T>(Table);
        }
        */
        /// <summary>
        /// FactoryCreate für SimpleGetterBo
        /// </summary>
        /// <returns>BO</returns>
        public ISimpleGetterBo createSimpleGetterBo()
        {
            return new SimpleGetterBo(Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getSimpleGetterDao());
        }

        /// <summary>
        /// FactoryCreate für SimpleSetterBo
        /// </summary>
        /// <returns>BO</returns>
        public ISimpleSetterBo createSimpleSetterBo()
        {
            return new SimpleSetterBo(new SimpleSetterDao());
        }

        /// <summary>
        /// FactoryCreate für VertragBo
        /// </summary>
        /// <returns>BO</returns>
        public IVertragBo createVertragBo()
        {
            return new VertragBo(new VertragDao(), Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getEaihotDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao());
        }

        /// <summary>
        /// FactoryCreate für ZinsBo
        /// </summary>
        /// <returns>BO</returns>
        public IZinsBo createZinsBo(ConditionLinkType[] Links, String langCode)
        {
            return new ZinsBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getZinsDao(), PrismaDaoFactory.getInstance().getPrismaDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao(), Links, langCode, Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getVGDao());
        }


        /// <summary>
        /// FactoryCreate für MwstBo
        /// </summary>
        /// <returns>BO</returns>
        public IMwStBo createMwstBo()
        {
            return new MwStBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getMwStDao()); 
        }

        /// <summary>
        /// FactoryCreate für ZusatzdatenBo
        /// </summary>
        /// <returns>BO</returns>
        public IZusatzdatenBo createZusatzdatenBo()
        {
            return new ZusatzdatenBo(new ZusatzdatenDao());
        }

        /// <summary>
        /// Buchwert BO erzeugen
        /// </summary>
        /// <returns>Buchwert BO</returns>
        public IBuchwertBo createBuchwertBo()
        {
            return new BuchwertBo(Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getEaihotDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao());

        }

        /// <summary>
        /// Prisma Parameter BO erzeugen
        /// </summary>
        /// <returns>Prisma Parameter BO</returns>
        public IPrismaParameterBo createPrismaParameterBo()
        {
            IPrismaDao pDao = PrismaDaoFactory.getInstance().getPrismaDao(); ;
            IObTypDao obDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao();
            PrismaParameterBo pbo = new PrismaParameterBo(pDao, obDao, PrismaParameterBo.CONDITIONS_BANKNOW);
            return pbo;
        }

        /// <summary>
        /// PrintAngAntBo erzeugen
        /// </summary>
        /// <returns>PrintAngAntBo</returns>
        public IPrintAngAntBo createPrintAngAntBo()
        {
            return new PrintAngAntBo(Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getEaihotDao(), Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getAngAntDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getDictionaryListsDao());
        }

        /// <summary>
        /// KorrekturBo erzeugen
        /// </summary>
        /// <returns>IKorrekturBo</returns>
        public IKorrekturBo createKorrekturBo()
        {
            return new KorrekturBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getKorrekturDao());
        }

        /// <summary>
        /// TranslateBo erzeugen
        /// </summary>
        /// <returns>ITranslateBo</returns>
        public ITranslateBo createTranslateBo()
        {
            return new TranslateBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao());
        }

        /// <summary>
        /// VertragsListenBo erzeugen
        /// </summary>
        /// <returns>IVertragsListenBo</returns>
        public IVertragsListenBo createVertragsListenBo()
        {
            return new VertragsListenBo(Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getEaihotDao());
        }

        /// <summary>
        /// RightsMap holen
        /// </summary>
        /// <returns>IVertragsListenBo</returns>
        public IRightsMapBo createRightsMapBo()
        {
            return new RightsMapBo(new RightsMapDao());
        }

        /// <summary>
        /// FileattBo holen
        /// </summary>
        /// <returns></returns>
        public IFileattBo createFileattBo()
        {
            return new FileattBo(new FileattDao());
        }

        /// <summary>
        /// FileattBo holen
        /// </summary>
        /// <returns></returns>
        public IFileBo createFileBo()
        {
            return new FileBo(new FileDao());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IPruefungBo createPruefungBo()
        {
            IPrismaDao pDao = PrismaDaoFactory.getInstance().getPrismaDao(); ;
            IObTypDao obDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao();
            PrismaParameterBo pbo = new PrismaParameterBo(pDao, obDao, PrismaParameterBo.CONDITIONS_BANKNOW);
            ITranslateDao transDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao();
            IPruefungDao pruefundDao = Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getPruefungDao();
           return new PruefungBo(pDao, obDao, transDao, pruefundDao);
        }

        /// <summary>
        /// FactoryCreate für B2BOL userManagement
        /// </summary>
        /// <returns>BO</returns>
        public IB2BOLBo createB2BOLBo()
        {
            return new B2BOLBo();
        }

        /// <summary>
        /// FactoryCreate für Eaihot Management
        /// </summary>
        /// <returns>BO</returns>
        public IEaihotBo createEaihotBo()
        {
            return new EaihotBo( Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getEaihotDao());
        }

        /// <summary>
        /// Factory for creation of manager class for incentivation program
        /// </summary>
        /// <returns>incentivation program object</returns>
        public IIncentivierungBo createIncentivierungBo()
        {
            return new IncentivierungBo(Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getIncentivierungDao());
        }


        public ITransactionRisikoBo createTransactionRisikoBO()
        {
            EurotaxBo eurotaxbo = new EurotaxBo(new EurotaxWSDao(), new EurotaxDBDao(), new AuskunftDao(), new VGDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao());
            ITranslateDao transDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao();
            return new TransactionRisikoBO(Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getAngAntDao(), new VGDao(), new EurotaxDBDao(), eurotaxbo, OpenOne.Common.DAO.CommonDaoFactory.getInstance().getMwStDao(), createTranslateBo(),new TransactionRisikoDao(), new EaihotDao());
        }

        /// <summary>
        /// Create the prisma product BO
        /// </summary>
        /// <param name="langCode"></param>
        /// <returns></returns>
        public IPrismaProductBo createPrismaProductBO(String langCode)
        {
            ITranslateDao transDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao();
            return new PrismaProductBo(PrismaDaoFactory.getInstance().getPrismaDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao(), transDao, PrismaProductBo.CONDITIONS_BANKNOW, langCode);
        }

        /// <summary>
        /// Create BO for calculating Credit expected loss customer risk
        /// </summary>
        /// <returns></returns>
        public IKundenRisikoBo createKundenRisikoBO(String langCode)
        {
            
            ITranslateDao transDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao();

            return new KundenRisikoBO(Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getAngAntDao(), new VGDao(), createPrismaProductBO(langCode), createPrismaParameterBo(), CommonBOFactory.getInstance().createMwstBo(), OpenOne.Common.DAO.CommonDaoFactory.getInstance().getMwStDao(), createTranslateBo(), new KundenRisikoDao(), new EaihotDao());
        }

        /// <summary>
        /// Creates a new insurance BO
        /// </summary>
        /// <returns></returns>
        public IInsuranceBo createInsuranceBO()
        {
            return new Cic.OpenOne.GateBANKNOW.Common.BO.Versicherung.InsuranceBo(OpenOne.Common.DAO.CommonDaoFactory.getInstance().getInsuranceDao(), OpenOne.Common.DAO.CommonDaoFactory.getInstance().getQuoteDao());
        }
	}
}
