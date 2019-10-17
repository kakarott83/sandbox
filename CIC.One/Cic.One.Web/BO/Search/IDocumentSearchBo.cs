using Cic.One.DTO;

namespace Cic.One.Web.BO.Search
{
    /// <summary>
    /// Interface der DokumentenSuche (BO)
    /// </summary>
    public interface IDocumentSearchBo
    {
        /// <summary>
        /// Sucht nach Dokumenten
        /// </summary>
        /// <param name="input">Parameter</param>
        /// <returns>Liste von Infos der gefundenen Elementen</returns>
        HitlistDto DynamicDocumentSearch(iDynamicDocumentSearchDto input);

        /// <summary>
        /// Lädt ein Dokument anhand dem Input
        /// </summary>
        /// <param name="docLoad"></param>
        /// <returns>Enthält das Dokument</returns>
        byte[] DocumentLoad(iDocumentLoadDto input);

        /// <summary>
        /// Liefert die Version der ITA WebSearch zurück
        /// </summary>
        /// <param name="info"></param>
        /// <returns>Information</returns>
        ogetVersionInfo getVersionInfo(igetVersionInfo input);
    }
}