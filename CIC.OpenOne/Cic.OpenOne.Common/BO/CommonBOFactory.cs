using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DAO.Versicherung;
using System;

namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// CommonDaoFactory-Klasse
    /// </summary>
    public class CommonBOFactory
    {
        private static volatile CommonBOFactory _self;
        private static readonly object InstanceLocker = new Object();

        /// <summary>
        /// Instanz der Common BO Factory erzeugen
        /// </summary>
        /// <returns></returns>
        public static CommonBOFactory getInstance()
        {
            if (_self == null)
            {
                lock (InstanceLocker)
                {
                    if (_self == null)
                        _self = new CommonBOFactory();
                }
            }
            return _self;
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
    }
}