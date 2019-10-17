// OWNER MK, 21-07-2009
using Cic.OpenLease.ServiceAccess.DdOw;
using System;
using System.Collections.Generic;
namespace Cic.OpenLease.ServiceAccess.Merge.OlClient
{
    [System.CLSCompliant(true)]
    [System.ServiceModel.ServiceContract(Name = "OlClientService", Namespace = "http://cic-software.de/Cic.OpenLease.Service.Services.Merge.OpenLease")]
    public interface IOlClientService
    {
        //[System.ServiceModel.OperationContract]
        //HotOutputDto Execute(HotInputDto hotInputDto);

        [System.ServiceModel.OperationContract]
        DocumentShortDto[] DeliverDocumentsList(AreaConstants area);

        [System.ServiceModel.OperationContract]
        DocumentShortDto[] DeliverDocumentsListNew(long sysAreaId);

        [System.ServiceModel.OperationContract]
        long PrintDocumentNew(DocumentShortDto[] document, long sysAreaId);

        [System.ServiceModel.OperationContract]
        long PrintDocument(DocumentShortDto document, long sysAreaId);

        [System.ServiceModel.OperationContract]
        DocumentDto DeliverPrintedDocument(long sysEaiHot);

        [System.ServiceModel.OperationContract]
        DocumentDto[] DeliverPrintedDocuments(AreaConstants area, long sysAreaId);

        [System.ServiceModel.OperationContract]        
        int SearchCount(EAIHotDto searchData);

        [System.ServiceModel.OperationContract]
        DocumentListDto[] DeliverUploadedDocuments(long sysangebot);
        
        [System.ServiceModel.OperationContract]
        List<EAIHotDto> Search(EAIHotDto searchData, Cic.OpenLease.ServiceAccess.SearchParameters searchParameters, EAIHotSortData[] sortColumns);

       // [System.ServiceModel.OperationContract]
        //ActivityInfo getActivityInfos();

        /// <summary>
        /// Returns the amount of due activities for the current user
        /// </summary>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        int getDueActivities();


        /// <summary>
        /// Uploads a document to dmsdoc/dmsdocarea
        /// </summary>
        /// <param name="doc"></param>
        [System.ServiceModel.OperationContract]
        String uploadDocument(DocumentUploadDto doc);
    }
}
