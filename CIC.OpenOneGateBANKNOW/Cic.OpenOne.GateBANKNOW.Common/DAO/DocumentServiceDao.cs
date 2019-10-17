using AutoMapper;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using CIC.Database.OW.EF6.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// Dao for DMS Document Service
    /// </summary>
    public class DocumentServiceDao : IDocumentServiceDao
    {
        private static String QUERY_DMSDOCUPDATE = "update dmsdoc set UNGUELTIGVON = :wfuser, UNGUELTIGAM = trunc(sysdate), UNGUELTIGUM = MDBS_TOCLATIME(sysdate), UNGUELTIGFLAG = 1 where sysdmsdoc=:sysdmsdoc";
        private static String QUERY_DMSDOC = "select * from dmsdoc where sysdmsdoc = :sysdmsdoc";
        private static String QUERY_DMSDOCWFTX = "select dmsdoc.* from dmsdoc,wftx where wftx.syswftx=dmsdoc.syswftx and wftx.document=:document";
        private static String QUERY_DMSDOCTYPE = @"SELECT vc_ctlutart_wftx.sysid syswftx,
  vc_ctlutart_wftx.actualterm1 name,wftx.docextension extension,BDEFGRP.sysbdefgrp, BDEFGRP.NAME groupname
FROM VC_CTLUTART_WFTX,
  WFTX,
  BDEFGRP, ctlang
WHERE wftx.syswftx             = vc_ctlutart_wftx.sysid
AND bdefgrp.sysbdefgrp         = wftx.syswftxgrp
AND vc_ctlutart_wftx.sysctlang = ctlang.sysctlang 
and ctlang.isocode = :isoCode
and wftx.syswftx=:syswftx 
ORDER BY vc_ctlutart_wftx.actualterm1";
        private static String QUERY_DOCTYPE=@"SELECT vc_ctlutart_wftx.sysid syswftx,
  vc_ctlutart_wftx.actualterm1 name,wftx.docextension extension,BDEFGRP.sysbdefgrp, BDEFGRP.NAME groupname
FROM VC_CTLUTART_WFTX,
  WFTX,
  BDEFGRP, ctlang
WHERE wftx.syswftx             = vc_ctlutart_wftx.sysid
AND bdefgrp.sysbdefgrp         = wftx.syswftxgrp
AND vc_ctlutart_wftx.sysctlang = ctlang.sysctlang 
and ctlang.isocode = :isoCode
";

        /// <summary>
        /// List all available documenttypes for the given language code
        /// </summary>
        /// <param name="isoCode"></param>
        /// <param name="extension"></param>
        /// <param name="sysbdefgrp"></param>
        /// <param name="groupname"></param>
        /// <returns></returns>
        public List<DocumentTypeDto> listAvailableDocumentTypes(String isoCode, String extension, long sysbdefgrp, String groupname)
        {

            using (DdOwExtended owCtx = new DdOwExtended())
            {
                String query = QUERY_DOCTYPE;
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isoCode", Value = isoCode });
                if (extension != null && extension.Length > 0)
                {
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "extension", Value = "%"+extension+"%" });
                    query += " and wftx.docextension like :extension ";
                }
                if (groupname != null && groupname.Length > 0)
                {
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "groupname", Value = "%" + groupname + "%" });
                    query += " and bdefgrp.name like :groupname ";
                }
                if (sysbdefgrp > 0)
                {
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysbdefgrp", Value = sysbdefgrp });
                    query += " and BDEFGRP.sysbdefgrp= :sysbdefgrp ";
                }
                query += " ORDER BY vc_ctlutart_wftx.actualterm1";
                return owCtx.ExecuteStoreQuery<DocumentTypeDto>(query, parameters.ToArray()).ToList();
            }

        }

        /// <summary>
        /// loads a certain document
        /// </summary>
        /// <param name="sysdmsdoc"></param>
        /// <returns></returns>
        public DmsDocDto getDocument(long sysdmsdoc)
        {
            using (DdOwExtended owCtx = new DdOwExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysdmsdoc", Value = sysdmsdoc });
                return owCtx.ExecuteStoreQuery<DmsDocDto>(QUERY_DMSDOC, parameters.ToArray()).FirstOrDefault();
            }
            
        }

        /// <summary>
        /// loads a certain document by wftx document id
        /// </summary>
        /// <param name="sysdmsdoc"></param>
        /// <returns></returns>
        public DmsDocDto getDocument(String document)
        {
            using (DdOwExtended owCtx = new DdOwExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "document", Value = document });
                return owCtx.ExecuteStoreQuery<DmsDocDto>(QUERY_DMSDOCWFTX, parameters.ToArray()).FirstOrDefault();
            }

        }

        /// <summary>
        /// gets a certain document type
        /// </summary>
        /// <param name="isoCode"></param>
        /// <param name="syswftx"></param>
        /// <returns></returns>
        public DocumentTypeDto getDocumentType(String isoCode, long syswftx)
        {
           using (DdOwExtended owCtx = new DdOwExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswftx", Value = syswftx });
               parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isoCode", Value = isoCode });
                return owCtx.ExecuteStoreQuery<DocumentTypeDto>(QUERY_DMSDOCTYPE, parameters.ToArray()).FirstOrDefault();
            }
        }

        /// <summary>
        /// removes the defined document by setting its valid dates
        /// </summary>
        /// <param name="sysdmsdoc"></param>
        /// <param name="syswfuser"></param>
        public void setDocumentDeleted(long sysdmsdoc, long syswfuser)
        {
            using (DdOwExtended owCtx = new DdOwExtended())
            {
                String wfuserCode = owCtx.ExecuteStoreQuery<String>("select code from wfuser where syswfuser=" + syswfuser, null).FirstOrDefault();
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                

                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysdmsdoc", Value = sysdmsdoc });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "wfuser", Value = wfuserCode });
                owCtx.ExecuteStoreCommand(QUERY_DMSDOCUPDATE, parameters.ToArray());
            }
        }

        /// <summary>
        /// Creates or Updates the document
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public DmsDocDto createOrUpdateDocument(DmsDocDto doc)
        {

            using (DdOwExtended owCtx = new DdOwExtended())
            {
                DMSDOC dbdoc = null;
                if(doc.sysdmsdoc>0)
                    dbdoc = (from t in owCtx.DMSDOC
                            where t.SYSDMSDOC==doc.sysdmsdoc
                            select t).FirstOrDefault();
                if (dbdoc == null)
                {
                    dbdoc = new DMSDOC();
                    owCtx.DMSDOC.Add(dbdoc);
                }
                dbdoc = Mapper.Map<DmsDocDto, DMSDOC>(doc,dbdoc);
                if (doc.syswftx>0)
                    dbdoc.SYSWFTX= doc.syswftx;
                owCtx.SaveChanges();

                DMSDOCAREA docarea = new DMSDOCAREA();
 
                
                docarea.DMSDOC = dbdoc;
                docarea.RANG = 0;
                docarea.AREA = "";
                docarea.SYSID = 0;

                owCtx.DMSDOCAREA.Add(docarea);
                owCtx.SaveChanges();

                return getDocument(dbdoc.SYSDMSDOC);
            }
            
        }
    }
}
