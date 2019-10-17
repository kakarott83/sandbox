using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// BO for DMS Document Service
    /// </summary>
    public class DocumentServiceBo : AbstractDocumentServiceBo
    {
        public DocumentServiceBo(IDocumentServiceDao dao)
            : base(dao)
        {

        }

        /// <summary>
        /// Creates or Updates the document
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        override public DmsDocDto createOrUpdateDocument(DmsDocDto doc)
        {
            return dao.createOrUpdateDocument(doc);
            
        }

        /// <summary>
        /// List all available documenttypes for the given language code
        /// </summary>
        /// <param name="isoCode"></param>
        /// <param name="extension"></param>
        /// <param name="sysbdefgrp"></param>
        /// <param name="groupname"></param>
        /// <returns></returns>
        override public List<DocumentTypeDto> listAvailableDocumentTypes(String isoCode, String extension, long sysbdefgrp, String groupname)
        {
            return dao.listAvailableDocumentTypes(isoCode,extension,sysbdefgrp,groupname);
        }

        /// <summary>
        /// loads a certain document
        /// </summary>
        /// <param name="sysdmsdoc"></param>
        /// <returns></returns>
        public override DmsDocDto getDocument(long sysdmsdoc)
        {
            return dao.getDocument(sysdmsdoc);
        }

        /// <summary>
        /// loads a certain document by wftx id
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public override DmsDocDto getDocument(String document)
        {
            return dao.getDocument(document);
        }

        /// <summary>
        /// gets a certain document type
        /// </summary>
        /// <param name="isoCode"></param>
        /// <param name="syswftx"></param>
        /// <returns></returns>
        public override DocumentTypeDto getDocumentType(String isoCode, long syswftx)
        {
            return dao.getDocumentType(isoCode, syswftx);
        }

        /// <summary>
        /// removes the defined document by setting its valid dates
        /// </summary>
        /// <param name="sysdmsdoc"></param>
        /// <param name="syswfuser"></param>
        public override void setDocumentDeleted(long sysdmsdoc, long syswfuser)
        {
            dao.setDocumentDeleted(sysdmsdoc, syswfuser);
        }

        /// <summary>
        /// searches for dmsdocuments
        /// </summary>
        /// <param name="rollenAttributRechte"></param>
        /// <param name="searchInput"></param>
        /// <param name="sysperole"></param>
        /// <param name="isocode"></param>
        /// <returns></returns>
        public override oSearchDto<DmsDocDto> searchDocuments(bool rollenAttributRechte, iSearchDto searchInput, long sysperole, String isoCode)
        {
           
            long sysVpPerole = 0;
            long sysVpPerson = 0;
            using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended olCtx = new Cic.OpenOne.Common.Model.DdOl.DdOlExtended())
            {
                try
                {
                    CIC.Database.OL.EF6.Model.PEROLE pe = Cic.OpenOne.Common.Model.DdOl.PeRoleUtil.FindRootPEROLEObjByRoleType(olCtx , sysperole, (long)Cic.OpenOne.Common.Model.DdOl.RoleTypeTyp.HAENDLER);
                    sysVpPerole = pe.SYSPEROLE;
                    sysVpPerson = pe.SYSPERSON.HasValue ? pe.SYSPERSON.Value : 0;
                }catch(Exception)
                {
                    
                }
            }

            //PARAMETER von AUSSEN: and ctlang.isocode = :isoCode  and DMSDOC.SysWFTX = :syswftx and dmsdoc.gedrucktam>to_date('2014-01-01','yyyy-mm-dd') and dmsdoc.gedrucktam<=to_date('2014-02-01','yyyy-mm-dd')
            QueryInfoData infoData = new QueryInfoDataExt();

            if (!rollenAttributRechte)
            {
                infoData = new QueryInfoDataExt(
                "vc_ctlutart_wftx.ACTUALTERM1 typ,  dmsdoc.sysdmsdoc,  dmsdoc.name, dmsdoc.dateiname,  dmsdoc.syswftx, dmsdoc.bemerkung,  dmsdoc.gedrucktam,    SUBSTR(dmsdoc.dateiname,LENGTH(dmsdoc.dateiname)-instr(reverse(dmsdoc.dateiname),'.')+2) format",
                 "dmsdoc,  dmsdocarea,  vc_ctlutart_wftx,ctlang",
                 "DMSDOC.SYSDMSDOC",
                 "dmsdoc,  dmsdocarea,  vc_ctlutart_wftx,ctlang",
                 "dmsdoc.sysdmsdoc         = dmsdocarea.sysdmsdoc AND dmsdoc.syswftx             = vc_ctlutart_wftx.sysid AND vc_ctlutart_wftx.sysctlang = ctlang.sysctlang AND DMSDOCAREA.AREA = 'PERSON' AND DMSDOCAREA.SysID =" + sysVpPerson + " AND (dmsdoc.UNGUELTIGFLAG      = 0 OR dmsdoc.UNGUELTIGVON         > TRUNC(sysdate)) and ctlang.isocode='" + isoCode + "'"
                 );
            }
            else
            {
                //PARAMETER von AUSSEN: and isocode = :isoCode  and SysWFTX = :syswftx and gedrucktam>to_date('2014-01-01','yyyy-mm-dd') and gedrucktam<=to_date('2014-02-01','yyyy-mm-dd')
                infoData = new QueryInfoDataExt(
               "typ,  VC_DMSDOC_1.sysdmsdoc,  name,  dateiname,  bemerkung, syswftx, gedrucktam,    SUBSTR(dateiname,LENGTH(dateiname)-instr(reverse(dateiname),'.')+2) format",
                "VC_DMSDOC_1,  dmsdocarea",
                "VC_DMSDOC_1.SYSDMSDOC",
                "VC_DMSDOC_1,  dmsdocarea",
                "VC_DMSDOC_1.sysdmsdoc=dmsdocarea.sysdmsdoc and sysperole=" + sysperole + " and hperole= " + sysVpPerole + "  and  DMSDOCAREA.AREA = 'PERSON' AND DMSDOCAREA.SysID = " + sysVpPerson + " and isocode='" + isoCode + "'" 
              
                );
            }
            SearchBo<Cic.OpenOne.GateBANKNOW.Common.DTO.DmsDocDto> bo = new SearchBo<Cic.OpenOne.GateBANKNOW.Common.DTO.DmsDocDto>(infoData);
            return bo.search(searchInput);

        }
    }
}
