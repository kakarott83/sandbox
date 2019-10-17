using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.One.Web.DAO;
using Cic.OpenOne.Common.Util.Security;
using Cic.One.Web.BO.Mail;
using Cic.One.Web.DAO.Mail;
using Cic.One.Web.BO.Search;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DAO.Prisma;


namespace Cic.One.Web.BO
{
    /// <summary>
    /// Factory of all Business Objects
    /// </summary>
    public class BOFactory : IBOFactory
    {
        private static bool CASworking = false;
        private static long CASchecked = 0;

        public BOFactory() { }

        /// <summary>
        /// returns the xpro search bo
        /// </summary>
        /// <returns></returns>
        public virtual IXproSearchBo getXproSearchBo()
        {
            return new XproSearchBo(DAOFactoryFactory.getInstance().getXproLoaderDao(), XproInfoFactory.getInstance());
        }

        /// <summary>
        /// returns the Guardean BO
        /// </summary>
        /// <returns></returns>
        public IGuardeanBo getGuardeanBo()
        {
            return new GuardeanBo();
        }

        /// <summary>
        /// returns the State Service BO
        /// </summary>
        /// <returns></returns>
        public IStateServiceBo getStateServiceBo()
        {
            return new StateServiceBo();
        }

        /// <summary>
        /// returns the Report BO
        /// </summary>
        /// <returns></returns>
        public virtual IReportBo getReportBo()
        {
            return new ReportBo();
        }

        /// <summary>
        /// returns the prisma product resolution bo
        /// </summary>
        /// <returns></returns>
        public IPrismaProductBo getProductBo(String language)
        {
            ConditionLinkType[] CONDITIONS = { };

            IPrismaDao pDao = new CachedPrismaDao();//TODO into commondao factory
            IObTypDao obDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao();
            ITranslateDao transDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao();
            PrismaProductBo bo = new PrismaProductBo(pDao, obDao, transDao, CONDITIONS, language);
            return bo;
        }

        /// <summary>
        /// returns the prisma product parameter resolution bo
        /// </summary>
        /// <returns></returns>
        public IPrismaParameterBo getProductParameterBo()
        {
            ConditionLinkType[] CONDITIONS = //{ ConditionLinkType.COMMON,  ConditionLinkType.PRODUCT };
                 { ConditionLinkType.COMMON, ConditionLinkType.BRAND, ConditionLinkType.OBTYP, ConditionLinkType.PRHGROUP, ConditionLinkType.PRKGROUP, ConditionLinkType.PRODUCT, ConditionLinkType.BCHNL, ConditionLinkType.OBART, ConditionLinkType.KDTYP, ConditionLinkType.USETYPE, ConditionLinkType.VART, ConditionLinkType.VTTYP };

            IPrismaDao pDao = PrismaDaoFactory.getInstance().getPrismaDao();//TODO into commondao factory
            IObTypDao obDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao();
            IPrismaParameterBo bo = new PrismaParameterBo(pDao, obDao, CONDITIONS);

            return bo;
        }

        /// <summary>
        /// returns the Clarion Application Service BO
        /// </summary>
        /// <returns></returns>
        public IWorkflowBo getWorkflowBo()
        {
            return new WorkflowBo(DAOFactoryFactory.getInstance().getWorkflowDao());
        }

        /// <summary>
        /// returns the Application Settings Bo
        /// </summary>
        /// <returns></returns>
        public IAppSettingsBo getAppSettingsBo()
        {
            return new AppSettingsBo(DAOFactoryFactory.getInstance().getAppSettingsDao());
        }

        /// <summary>
        /// returns the Authentication BO
        /// </summary>
        /// <returns></returns>
        public IAuthenticationBo getAuthenticationBo()
        {
            return new AuthenticationBo(new AppSettingsDao(), DAOFactoryFactory.getInstance().getAuthenticationDao());
        }

        /// <summary>
        /// returns the Clarion Application Service BO
        /// </summary>
        /// <returns></returns>
        public ICASBo getCASBo()
        {
            long now = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
            if (!CASworking && (now - CASchecked) > 1000 * 60)
            {
                ICASBo rval = new CASBo();
                CASworking = rval.validateConnection();
                CASchecked = now;
            }
            if (CASworking)
                return new CASBo();

            return null;
        }

        /// <summary>
        /// returns the CRM Entity BO repsonsible for all create, update, get operations of Entities
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual IEntityBo getEntityBO(MembershipUserValidationInfo info)
        {
            IEntityDao crmEntityDao = DAOFactoryFactory.getInstance().getEntityDao();
            if (info != null)
            {
                crmEntityDao.setSysPerole(info.sysPEROLE);
                crmEntityDao.setSysWfuser(info.sysWFUSER);
                crmEntityDao.setISOLanguage(info.ISOLanguageCode);
            }
            EntityBo bo = new EntityBo(crmEntityDao, getAppSettingsBO(), getCASBo());
            if (info != null)
            {
                bo.setSysWfUser(info.sysWFUSER);
                bo.SysPerole = info.sysPEROLE;
            }
            return bo;
        }

        /// <summary>
        /// returns the Application Settings BO
        /// </summary>
        /// <returns></returns>
        public IAppSettingsBo getAppSettingsBO()
        {
            return new AppSettingsBo(DAOFactoryFactory.getInstance().getAppSettingsDao());
        }

        /// <summary>
        /// returns the Print BO
        /// </summary>
        /// <returns></returns>
        public IPrintBo getPrintBo()
        {
            return new PrintBo();
        }

        /// <summary>
        /// returns the BO for document management search
        /// </summary>
        /// <returns></returns>
        public IDocumentSearchBo getDocumentSearchBO()
        {
            return new DocumentSearchBo(DAOFactoryFactory.getInstance().getDocumentSearchDao());
        }

        /// <summary>
        /// RightsMap holen
        /// </summary>
        /// <returns>IVertragsListenBo</returns>
        public IRightsMapBo createRightsMapBo()
        {
            return new RightsMapBo(DAOFactoryFactory.getInstance().getRightsMapDao());
        }

        /// <summary>
        /// return the BO for Mail Management
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public IEntityMailBo getEntityMailBO(MembershipUserValidationInfo info)
        {
            IEntityDao crmEntityDao = DAOFactoryFactory.getInstance().getEntityDao();
            crmEntityDao.setSysPerole(info.sysPEROLE);

            IMailDao mailDao = MailDaoFactory.getInstance().getMailDao(info.sysWFUSER);
            IMailDBDao mailDBDao = MailDaoFactory.getInstance().getMailDBDao(info.sysWFUSER);

            EntityMailBo bo = new EntityMailBo(crmEntityDao, mailDao, mailDBDao, getAppSettingsBO());
            bo.setSysWfUser(info.sysWFUSER);

            return bo;
        }


        /// <summary>
        /// return the BO for Mail Management for a certain user
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public IEntityMailBo getEntityMailBO(long syswfuser)
        {
            IEntityDao entityDao = DAOFactoryFactory.getInstance().getEntityDao();
            long sysperole = entityDao.getSysPerole(syswfuser);
            entityDao.setSysPerole(sysperole);

            IMailDao mailDao = MailDaoFactory.getInstance().getMailDao(syswfuser);
            IMailDBDao mailDBDao = MailDaoFactory.getInstance().getMailDBDao(syswfuser);

            EntityMailBo bo = new EntityMailBo(entityDao, mailDao, mailDBDao, getAppSettingsBO());
            bo.setSysWfUser(syswfuser);

            return bo;
        }

        /// <summary>
        /// returns a new instance for the calculation implementation logic
        /// </summary>
        /// <returns></returns>
        public ICalculationBo getCalculationBo()
        {
            return new CalculationBO();
        }
    }
}