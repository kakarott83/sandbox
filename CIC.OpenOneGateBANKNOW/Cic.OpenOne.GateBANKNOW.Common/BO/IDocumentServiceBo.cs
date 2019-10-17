using Cic.OpenOne.GateBANKNOW.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Interface for DMS Document Service
    /// </summary>
    public interface IDocumentServiceBo
    {
        
        /// <summary>
        /// List all available documenttypes for the given language code
        /// </summary>
        /// <param name="isoCode"></param>
        /// <param name="extension"></param>
        /// <param name="sysbdefgrp"></param>
        /// <param name="groupname"></param>
        /// <returns></returns>
        List<DocumentTypeDto> listAvailableDocumentTypes(String isoCode, String extension, long sysbdefgrp, String groupname);

        /// <summary>
        /// loads a certain document
        /// </summary>
        /// <param name="sysdmsdoc"></param>
        /// <returns></returns>
        DmsDocDto getDocument(long sysdmsdoc);


        /// <summary>
        /// loads a certain document by wftx id
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        DmsDocDto getDocument(String document);


        /// <summary>
        /// gets a certain document type
        /// </summary>
        /// <param name="isoCode"></param>
        /// <param name="syswftx"></param>
        /// <returns></returns>
        DocumentTypeDto getDocumentType(String isoCode, long syswftx);

        /// <summary>
        /// removes the defined document by setting its valid dates
        /// </summary>
        /// <param name="sysdmsdoc"></param>
        /// <param name="syswfuser"></param>
        void setDocumentDeleted(long sysdmsdoc, long syswfuser);

        /// <summary>
        /// searches for dmsdocuments
        /// </summary>
        /// <param name="rollenAttributRechte"></param>
        /// <param name="searchInput"></param>
        /// <param name="sysperole"></param>
        /// <param name="isoCode"></param>
        /// <returns></returns>
        oSearchDto<DmsDocDto> searchDocuments(bool rollenAttributRechte, iSearchDto searchInput, long sysperole, String isoCode);

        /// <summary>
        /// Creates or Updates the document
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        DmsDocDto createOrUpdateDocument(DmsDocDto doc);
    }
}
