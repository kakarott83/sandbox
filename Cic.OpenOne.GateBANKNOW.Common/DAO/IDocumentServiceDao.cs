using Cic.OpenOne.GateBANKNOW.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// Dao Interface for DMS Document Service
    /// </summary>
    public interface IDocumentServiceDao
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
        /// loads a certain document by wftx document id
        /// </summary>
        /// <param name="sysdmsdoc"></param>
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
        /// Creates or Updates the document
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        DmsDocDto createOrUpdateDocument(DmsDocDto doc);
    }
}
