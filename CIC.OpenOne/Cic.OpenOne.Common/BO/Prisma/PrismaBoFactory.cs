using System;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DAO.Prisma;

namespace Cic.OpenOne.Common.BO.Prisma
{
    /// <summary>
    /// Factory Class für creating PrismaBOs with default DAOs
    /// </summary>
    public class PrismaBoFactory
    {
        private static volatile PrismaBoFactory _self;
        private static readonly object InstanceLocker = new Object();

        /// <summary>
        /// PrismaBoFactory getInstance
        /// </summary>
        /// <returns></returns>
        public static PrismaBoFactory getInstance()
        {
            if (_self == null)
            {
                lock (InstanceLocker)
                {
                    if (_self == null)
                        _self = new PrismaBoFactory();
                }
            }
            return _self;
        }

        /// <summary>
        /// Konstruktor of PrismaBoFactory
        /// </summary>
        private PrismaBoFactory()
        {
        }

        /// <summary>
        /// FactoryCreate für PrismaParameterBo
        /// </summary>
        /// <returns>BO</returns>
        public IPrismaParameterBo createPrismaParameterBo(ConditionLinkType[] Links)
        {
            return new PrismaParameterBo(PrismaDaoFactory.getInstance().getPrismaDao(), CommonDaoFactory.getInstance().getObTypDao(), Links);
        }

        /// <summary>
        /// FactoryCreate für PrismaProductBo
        /// </summary>
        /// <returns>BO</returns>
        public IPrismaProductBo createPrismaProductBo(ConditionLinkType[] Links, String langCode)
        {
            return new PrismaProductBo(PrismaDaoFactory.getInstance().getPrismaDao(), CommonDaoFactory.getInstance().getObTypDao(), CommonDaoFactory.getInstance().getTranslateDao(), Links, langCode);
        }

        /// <summary>
        /// FactoryCreate für PrismaServiceBo
        /// </summary>
        /// <returns>BO</returns>
        public IPrismaServiceBo createPrismaServiceBo()
        {
            return new PrismaServiceBo(PrismaDaoFactory.getInstance().getPrismaServiceDao(), CommonDaoFactory.getInstance().getObTypDao(), CommonDaoFactory.getInstance().getTranslateDao());
        }

        /// <summary>
        /// FactoryCreate für ProvisionBo
        /// </summary>
        /// <returns>BO</returns>
        public IProvisionBo createProvisionBo()
        {
            return new ProvisionBo(PrismaDaoFactory.getInstance().getProvisionDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao(), PrismaBoFactory.getInstance().createPrismaParameterBo(PrismaParameterBo.CONDITIONS_BANKNOW), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getVGDao());
        }

        /// <summary>
        /// FactoryCreate für SortTreeBo
        /// </summary>
        /// <returns>BO</returns>
        public ISortTreeBo createSortTreeBo()
        {
            return new SortTreeBo(new SortTreeDao());
        }
    }
}