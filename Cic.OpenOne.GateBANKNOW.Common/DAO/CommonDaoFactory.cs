

using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// CommonDaoFactory-Klasse
    /// </summary>
    public class CommonDaoFactory
    {
        private static CommonDaoFactory _self = null;
        private static string LOCK = "LOCK";

        /// <summary>
        /// Instanz der Prisma DAO Factory erzeugen
        /// </summary>
        /// <returns></returns>
        public static CommonDaoFactory getInstance()
        {
            lock (LOCK)
            {
                if (_self == null)
                    _self = new CommonDaoFactory();
            }
            return _self;
        }

        /// <summary>
        /// DocumentServiceDao Instanz erzeugen
        /// </summary>
        /// <returns></returns>
        public IDocumentServiceDao getDocumentServiceDao()
        {
            return new DocumentServiceDao();
        }

        /// <summary>
        /// IncentivierungDao Instanz erzeugen
        /// </summary>
        /// <returns></returns>
        public IIncentivierungDao getIncentivierungDao()
        {
            return new IncentivierungDao();
        }

        /// <summary>
        /// Adresse DAO Instanz erzeugen
        /// </summary>
        /// <returns></returns>
        public IAdresseDao getAdresseDao()
        {
            return new AdresseDao();
        }

        /// <summary>
        /// Offer/Application DAO Instanz erzeugen
        /// </summary>
        /// <returns></returns>
        public IAngAntDao getAngAntDao()
        {
            return new CachedAngAntDao(getEaihotDao());
        }

        /// <summary>
        /// Creates a eaihot Dao
        /// </summary>
        /// <returns></returns>
        public IEaihotDao getEaihotDao()
        {
            return new EaihotDao();
        }

        /// <summary>
        /// Offer/Application DAO Instanz erzeugen
        /// </summary>
        /// <returns></returns>
        public IAngAntDao getAngAntDaoMA()
        {
            CachedAngAntDao rval = new CachedAngAntDao(getEaihotDao());
            rval.setIsB2B(false);
            return rval;
        }

        /// <summary>
        /// SimpleGetter Instanz erzeugen
        /// </summary>
        /// <returns></returns>
        public ISimpleGetterDao getSimpleGetterDao()
        {
            return new CachedSimpleGetterDao();
        }

        public IPruefungDao getPruefungDao()
        {
            return new PruefungDao();
        }

        public IDMSDao getDMSDao()
        {
            return new DMSDao();
        }

        public IDMRDao getDMRDao()
        {
            return new DMRDao();
        }

        public IKundeDao getKundeDao()
        {
            return new KundeDao();
        }

        public IKREMODBDao getKremoDao()
        {
            IKREMODBDao Kremodbdao = new KREMODBDao();
            return Kremodbdao;
        }
        
    }
}