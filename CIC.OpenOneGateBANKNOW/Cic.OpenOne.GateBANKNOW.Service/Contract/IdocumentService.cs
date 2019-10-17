using Cic.OpenOne.GateBANKNOW.Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.Contract
{
    /// <summary>
    /// Service providing Acess to DMSDOC Datamodel
    /// </summary>
    [ServiceContract(Name = "IdocumentService", Namespace = "http://cic-software.de/GateBANKNOW")]
    public interface IdocumentService
    {
        /// <summary>
        /// fetches a list of available document types, filtering by docextension and docgroup
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        olistAvailableDocumentTypes listAvailableDocumentTypes(ilistAvailableDocumentTypes input);

        /// <summary>
        /// creates or updates a document (the file) for dmsdoc storage
        /// outgoing
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateDocumentDto createOrUpdateDocument(icreateOrUpdateDocumentDto input);

        /// <summary>
        /// removes the defined document by setting its valid dates
        /// </summary>
        /// <param name="sysdmsdoc"></param>
        [OperationContract]
        odeleteDocumentDto deleteDocument(long sysdmsdoc);

        /// <summary>
        /// returns a certain Document
        /// </summary>
        /// <param name="sysdmsdoc"></param>
        /// <returns></returns>
        [OperationContract]
        ogetDocumentDto getDocument(long sysdmsdoc);

        /// <summary>
        /// Searches Documents by the given filters, sorting and pagination
        /// </summary>
        /// <param name="input">isearchDocumentDto</param>
        /// <returns>osearchDocumentDto</returns>
        [OperationContract]
        osearchDmsDocDto searchDmsDoc(isearchDmsDocDto input);


        #region DMSINTERFACE
        /// <summary>
        /// Creates or Updates a dossier (the attributes) in the DMS via the DMS-HTTP-Interface
        /// outgoing
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateDMSAkteDto createOrUpdateDMSAkte(icreateOrUpdateDMSAkteDto input);

        /// <summary>
        /// creates or updates a document (the file) in the DMS via the DMS Documentimport Interface
        /// outgoing
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        ocreateOrUpdateDMSDokumentDto createOrUpdateDMSDokument(icreateOrUpdateDMSDokumentDto input);



        ///////////////////DEPRECATED FROM HERE/////////////////////////////////////////////////////////////
        /// <summary>
        /// triggers a system-event (calling a webservice processing the event-data)
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        [OperationContract]
        oExecEventDto execEvent(iExecEventDto e);
        /// <summary>
        /// interface from DMS to OL for new incoming Documents
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        oDMSUploadDto execDMSUploadTrigger(iDMSUploadDto input);

        [OperationContract]
        ogetDMSUrlDto getDMSUrl(igetDMSUrlDto input);

        #endregion
    }
}
