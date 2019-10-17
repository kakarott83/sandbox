using Cic.One.Web.DAO;

namespace Cic.One.Web.BO.Search
{
    /// <summary>
    /// Implementierung der Dookumentensuche
    /// </summary>
    public class DocumentSearchBo : AbstractDocumentSearchBo
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="dao">IDocumentSearchDao</param>
        public DocumentSearchBo(IDocumentSearchDao dao)
            : base(dao)
        {
        }
    }
}