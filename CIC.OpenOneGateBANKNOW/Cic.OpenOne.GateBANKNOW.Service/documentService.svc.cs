using AutoMapper;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Service.BO;
using Cic.OpenOne.GateBANKNOW.Service.Contract;
using Cic.OpenOne.GateBANKNOW.Service.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service
{
   
    /// <summary>
    /// Service providing Acess to DMSDOC Datamodel
    /// </summary>
    [ServiceBehavior(Namespace = "http://cic-software.de/GateBANKNOW")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public class documentService : IdocumentService
    {
        /// <summary>
        /// fetches a list of available document types, filtering by docextension and docgroup
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public olistAvailableDocumentTypes listAvailableDocumentTypes(ilistAvailableDocumentTypes input)
        {
            olistAvailableDocumentTypes rval = new olistAvailableDocumentTypes();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();

                List<Cic.OpenOne.GateBANKNOW.Common.DTO.DocumentTypeDto> types = BOFactory.getInstance().createDocumentServiceBo().listAvailableDocumentTypes(cctx.getMembershipInfo().ISOLanguageCode, input.extension, input.sysbdefgrp, input.groupname);
                rval.types = Mapper.Map<List<Cic.OpenOne.GateBANKNOW.Common.DTO.DocumentTypeDto>, List<DocumentTypeDto>>(types);
                

                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// removes the defined document by setting its valid dates
        /// </summary>
        /// <param name="sysdmsdoc"></param>
        public odeleteDocumentDto deleteDocument(long sysdmsdoc)
        {
            odeleteDocumentDto rval = new odeleteDocumentDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                IDocumentServiceBo doc = BOFactory.getInstance().createDocumentServiceBo();
                doc.setDocumentDeleted(sysdmsdoc,cctx.getMembershipInfo().sysWFUSER);

                rval.success();

            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                
            }
            return rval;
        }

        /// <summary>
        /// returns a certain Document
        /// </summary>
        /// <param name="sysdmsdoc"></param>
        /// <returns></returns>
        public ogetDocumentDto getDocument(long sysdmsdoc)
        {
            ogetDocumentDto rval = new ogetDocumentDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                IDocumentServiceBo doc = BOFactory.getInstance().createDocumentServiceBo();
                Common.DTO.DmsDocDto dmsdoc = doc.getDocument(sysdmsdoc);
                Common.DTO.DocumentTypeDto dmsdoctype = null;
                if(dmsdoc!=null)
                 dmsdoctype = doc.getDocumentType(cctx.getMembershipInfo().ISOLanguageCode, dmsdoc.syswftx);

                rval.document = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.DmsDocDto, Cic.OpenOne.GateBANKNOW.Service.DTO.DmsDocDto>(dmsdoc);
                rval.documentType = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.DocumentTypeDto, Cic.OpenOne.GateBANKNOW.Service.DTO.DocumentTypeDto>(dmsdoctype);
                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Searches Documents by the given filters, sorting and pagination
        /// </summary>
        /// <param name="input">isearchDocumentDto</param>
        /// <returns>osearchDocumentDto</returns>
        public osearchDmsDocDto searchDmsDoc(isearchDmsDocDto input)
        {
            osearchDmsDocDto rval = new osearchDmsDocDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input searchVertragDto was sent.");
                }
                if (input.searchInput == null)
                {
                    throw new ArgumentException("No searchparameter was sent.");
                }
                cctx.validateService();
                MembershipUserValidationInfo user = cctx.getMembershipInfo();



                IDocumentServiceBo doc = BOFactory.getInstance().createDocumentServiceBo();
                Cic.OpenOne.GateBANKNOW.Common.DTO.oSearchDto<Cic.OpenOne.GateBANKNOW.Common.DTO.DmsDocDto> sr = doc.searchDocuments(input.rollenAttributRechte, input.searchInput, user.sysPEROLE, user.ISOLanguageCode);
                Cic.OpenOne.GateBANKNOW.Common.DTO.SearchResultMapper<Cic.OpenOne.GateBANKNOW.Common.DTO.DmsDocDto, DTO.DmsDocDto> searchMapper = new Cic.OpenOne.GateBANKNOW.Common.DTO.SearchResultMapper<Cic.OpenOne.GateBANKNOW.Common.DTO.DmsDocDto, DTO.DmsDocDto>();
                rval.result = searchMapper.mapSearchResult(sr);


                rval.success();
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ArgumentException e)
            {
                cctx.fillBaseDto(rval, e, "F_00003_ArgumentException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - should not happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }


       

        /// <summary>
        /// Creates or Updates a dossier (the attributes) in the DMS via the DMS-HTTP-Interface
        /// outgoing
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ocreateOrUpdateDMSAkteDto createOrUpdateDMSAkte(icreateOrUpdateDMSAkteDto input)
        {
            

            ServiceHandler<icreateOrUpdateDMSAkteDto, ocreateOrUpdateDMSAkteDto> ew = new ServiceHandler<icreateOrUpdateDMSAkteDto, ocreateOrUpdateDMSAkteDto>(input);
            return ew.process(delegate(icreateOrUpdateDMSAkteDto inputdata, ocreateOrUpdateDMSAkteDto rval)
            {
                if (inputdata == null)
                    throw new ArgumentException("No valid input");
                BOFactory.getInstance().createDMSBo().createOrUpdateDMSAkte(Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.icreateOrUpdateDMSAkteDto, Cic.OpenOne.GateBANKNOW.Common.DTO.icreateOrUpdateDMSAkteDto>(inputdata));
                rval.success();
            });  
        }

        /// <summary>
        /// creates or updates a document (the file) in the DMS via the DMS Documentimport Interface
        /// outgoing
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ocreateOrUpdateDMSDokumentDto createOrUpdateDMSDokument(icreateOrUpdateDMSDokumentDto input)
        {
            //also update Dossier-Attributes?

            ServiceHandler<icreateOrUpdateDMSDokumentDto, ocreateOrUpdateDMSDokumentDto> ew = new ServiceHandler<icreateOrUpdateDMSDokumentDto, ocreateOrUpdateDMSDokumentDto>(input);
            return ew.process(delegate(icreateOrUpdateDMSDokumentDto inputdata, ocreateOrUpdateDMSDokumentDto rval)
            {
                if (inputdata == null)
                    throw new ArgumentException("No valid input");
                BOFactory.getInstance().createDMSBo().createOrUpdateDMSDokument(Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.icreateOrUpdateDMSDokumentDto, Cic.OpenOne.GateBANKNOW.Common.DTO.icreateOrUpdateDMSDokmentDto>(inputdata));
                rval.success();
            }); 

           
        }


        /// <summary>
        /// creates or updates a document (the file) for dmsdoc storage
        /// outgoing
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ocreateOrUpdateDocumentDto createOrUpdateDocument(icreateOrUpdateDocumentDto input)
        {
            ServiceHandler<icreateOrUpdateDocumentDto, ocreateOrUpdateDocumentDto> ew = new ServiceHandler<icreateOrUpdateDocumentDto, ocreateOrUpdateDocumentDto>(input);
            return ew.process(delegate(icreateOrUpdateDocumentDto inputdata, ocreateOrUpdateDocumentDto rval)
            {
                if (inputdata == null)
                    throw new ArgumentException("No valid input");
                if (input.document.inhalt == null || input.document.inhalt.Length==0)
                    throw new ArgumentException("Missing File data");
                IDocumentServiceBo docsvc = BOFactory.getInstance().createDocumentServiceBo();
                Cic.OpenOne.GateBANKNOW.Common.DTO.DmsDocDto rvdoc = docsvc.createOrUpdateDocument(Mapper.Map< Cic.OpenOne.GateBANKNOW.Service.DTO.DmsDocDto,Cic.OpenOne.GateBANKNOW.Common.DTO.DmsDocDto>(inputdata.document));
                rval.document = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.DmsDocDto, Cic.OpenOne.GateBANKNOW.Service.DTO.DmsDocDto>(rvdoc);
                rval.success();
            });


        }

      
       
        ///////////////////DEPRECATED FROM HERE/////////////////////////////////////////////////////////////
        /// <summary>
        /// interface from DMS to OL for new incoming Documents
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public oDMSUploadDto execDMSUploadTrigger(iDMSUploadDto input)
        {
            ServiceHandler<iDMSUploadDto, oDMSUploadDto> ew = new ServiceHandler<iDMSUploadDto, oDMSUploadDto>(input);
            return ew.process(delegate(iDMSUploadDto inputdata, oDMSUploadDto rval)
            {
                if (inputdata == null)
                    throw new ArgumentException("No valid input");

                //BOFactory.getInstance().createDMSBo().execDMSUploadTrigger(Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.iDMSUploadDto, Cic.OpenOne.GateBANKNOW.Common.DTO.iDMSUploadDto>(inputdata));
                rval.success();
            });            
        }

        /// <summary>
        /// triggers a system-event (calling a webservice processing the event-data)
        /// </summary>
        /// <param name="evt"></param>
        /// <returns></returns>
        public oExecEventDto execEvent(iExecEventDto evt)
        {
            ServiceHandler<iExecEventDto, oExecEventDto> ew = new ServiceHandler<iExecEventDto, oExecEventDto>(evt);
            return ew.process(delegate(iExecEventDto input, oExecEventDto rval)
            {
                if (input == null)
                    throw new ArgumentException("No valid input");

                
                rval.success();
            });    
        }
        /// <summary>
        /// delivers a url for the given area/id into the DMS-Frontend
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ogetDMSUrlDto getDMSUrl(igetDMSUrlDto input)
        {


            ServiceHandler<igetDMSUrlDto, ogetDMSUrlDto> ew = new ServiceHandler<igetDMSUrlDto, ogetDMSUrlDto>(input);
            return ew.process(delegate(igetDMSUrlDto inputdata, ogetDMSUrlDto rval)
            {
                if (inputdata == null)
                    throw new ArgumentException("No valid input");

                //rval.url = BOFactory.getInstance().createDMSBo().getDMSUrl(Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.igetDMSUrlDto, Cic.OpenOne.GateBANKNOW.Common.DTO.igetDMSUrlDto>(inputdata));
                rval.success();
            });
        }

        
       
    }
}
