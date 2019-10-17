using System.ServiceModel;
using Cic.One.DTO;

namespace Cic.One.Web.Contract
{
    /// <summary>
    /// provides methods for searching for Documents.
    /// </summary>
    [ServiceContract(Name = "IdocumentSearchService", Namespace = "http://cic-software.de/One")]
    public interface IdocumentSearchService
    {
        /// <summary>
        /// Sucht nach Dokumenten
        /// </summary>
        /// <param name="dynamicSearch">Parameter</param>
        /// <returns>Infos zu den gefundenen Dokumenten</returns>
        [OperationContract]
        oDynamicDocumentSearchDto DynamicDocumentSearch(iDynamicDocumentSearchDto input);

        /// <summary>
        /// Lädt ein Dokument anhand dem Input
        /// </summary>
        /// <param name="docLoad"></param>
        /// <returns>Enthält das Dokument</returns>
        [OperationContract]
        oDocumentLoadDto DocumentLoad(iDocumentLoadDto docLoad);

        /// <summary>
        /// Liefert die Version der ITA WebSearch zurück
        /// </summary>
        /// <param name="info"></param>
        /// <returns>Information</returns>
        [OperationContract]
        ogetVersionInfo getDocumentSearchVersionInfo(igetVersionInfo info);
    }
}