using Cic.One.Web.DAO;
using Cic.One.DTO;

namespace Cic.One.Web.BO.Search
{
    /// <summary>
    /// Abstrakte Dokumentensuche.
    /// Versucht so viel wie möglich zu machen
    /// </summary>
    public abstract class AbstractDocumentSearchBo : IDocumentSearchBo
    {
        private DAO.IDocumentSearchDao dao;
       

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="dao">IDocumentSearchDao</param>
        public AbstractDocumentSearchBo(IDocumentSearchDao dao)
        {
            this.dao = dao;
           
        }


        /// <summary>
        /// Sucht nach Dokumenten
        /// </summary>
        /// <param name="input">Parameter</param>
        /// <returns>Liste von Infos der gefundenen Elementen</returns>
        public HitlistDto DynamicDocumentSearch(iDynamicDocumentSearchDto input)
        {
            HitlistDto result = null;
            if (dao.Login(input.ProfileName))
            {
                result = dao.DynamicDocumentSearch(input);
                dao.Logout();
            }
            return result;
        }

        /// <summary>
        /// Lädt ein Dokument anhand dem Input
        /// </summary>
        /// <param name="docLoad"></param>
        /// <returns>Enthält das Dokument</returns>
        public byte[] DocumentLoad(iDocumentLoadDto input)
        {
            byte[] result = null;
            if (dao.Login(input.ProfileName))
            {
                result = dao.DocumentLoad(input);
                dao.Logout();
            }
            return result;
        }

        /// <summary>
        /// Liefert die Version der ITA WebSearch zurück
        /// </summary>
        /// <param name="info"></param>
        /// <returns>Information</returns>
        public ogetVersionInfo getVersionInfo(igetVersionInfo input)
        {
            ogetVersionInfo result = null;
            if (dao.Login())
            {
                result = dao.getVersionInfo(input);
                dao.Logout();
            }
            return result;
        }
    }
}