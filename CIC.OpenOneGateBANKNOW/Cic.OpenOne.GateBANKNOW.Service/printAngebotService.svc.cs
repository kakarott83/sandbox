using System;
using System.ServiceModel;
using AutoMapper;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Service.BO;
using Cic.OpenOne.GateBANKNOW.Service.Contract;
using Cic.OpenOne.GateBANKNOW.Service.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "printAngebotService" in code, svc and config file together.
    /// <summary>
    /// Der Service printAngebot liefert eine Liste aller verfügbaren Dokumente(Vorlagen) und erstellt einen Druckauftrag
    /// </summary>
    /// 
    [ServiceBehavior(Namespace = "http://cic-software.de/GateBANKNOW")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    class printAngebotService : IprintAngebotService
    {
        /// <summary>
        /// Liefert eine Liste der verfügbaren Dokumente im Kontext (20100914_Logik_Druckmatrix_v2)
        /// </summary>
        /// <param name="input">ilistAvailableDokumenteDto</param>
        /// <returns>olistAvailableDokumenteDto</returns>
        /// 
        public olistAvailableDokumenteDto listAvailableDokumente(ilistAvailableDokumenteDto input)
        {
            olistAvailableDokumenteDto rval = new olistAvailableDokumenteDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input listAvailableDokumenteDto was sent.");
                }
                if (input.kontext == null)
                {
                    throw new ArgumentException("No Kontext was sent.");
                }
                if (input.kontext.sysID == 0 || input.kontext.sysID == 0)
                {
                    throw new ArgumentException("No Angebot sysID was sent.");
                }
                cctx.validateService();

                try
                {
                    IPrintAngAntBo bo = BOFactory.getInstance().createPrintAngAntBo();
                    String subArea = input.kontext.subarea.ToString();
                    if (input.kontext.subarea == DocumentSubArea.All)
                    {
                        subArea = String.Empty;
                    }
                    Common.DTO.DokumenteDto[] commonDto = bo.listAvailableDokumente(Common.DTO.Enums.EaiHotOltable.Angebot, input.kontext.sysID,
                                                                                    cctx.getMembershipInfo().sysPERSON, cctx.getUserLanguange(), subArea, cctx.getMembershipInfo().sysPEROLE);
                    rval.dokumente = Mapper.Map<Common.DTO.DokumenteDto[], Service.DTO.DokumenteDto[]>(commonDto);
                    rval.success();
                }
                catch (ApplicationException ex)
                {
                    rval.message = new OpenOne.Common.DTO.Message();
                    rval.message.code = "Timeout";
                    rval.message.detail = "Timeout during EAIRequest";
                    rval.message.message = ex.ToString();
                }
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
        /// Erstellt Druckauftrag für ausgewählte Dokumente
        /// </summary>
        /// <param name="input">iprintCheckedDokumenteDto</param>
        /// <returns>oprintCheckedDokumenteDto</returns>
        public oprintCheckedDokumenteDto printCheckedDokumente(iprintCheckedDokumenteDto input)
        {
            oprintCheckedDokumenteDto rval = new oprintCheckedDokumenteDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input printCheckedDokumenteDto was sent.");
                }
                if (input.sysid == 0)
                {
                    throw new ArgumentException("No Angebot Sysid was sent.");
                }
                if (input.dokumente == null)
                {
                    throw new ArgumentException("No Dokumentelist was sent.");
                }
                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANGEBOT, cctx.getMembershipInfo().sysPUSER, input.sysid, DateTime.Now);
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                try
                {
                    IPrintAngAntBo bo = BOFactory.getInstance().createPrintAngAntBo();
                    rval.file = bo.printCheckedDokumente(Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.DokumenteDto[], Cic.OpenOne.GateBANKNOW.Common.DTO.DokumenteDto[]>(input.dokumente), input.sysid, input.variantenid,Cic.OpenOne.GateBANKNOW.Common.DTO.Enums.EaiHotOltable.Angebot, cctx.getMembershipInfo().sysPERSON, input.eCodeEintragung);
                    if (rval.file == null)
                    {
                        rval.message = new OpenOne.Common.DTO.Message();
                        rval.message.code = "No PDF generated";
                        rval.message.detail = "Print PDF didn´t work correctly";
                    }
                    else
                    {
                        rval.success();
                    }
                }
                catch (ApplicationException ex)
                {
                    rval.message = new OpenOne.Common.DTO.Message();
                    rval.message.code = "Timeout";
                    rval.message.detail = "Timeout during EAIRequest";
                    rval.message.message = ex.ToString();
                }
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
                cctx.fillBaseDto(rval, e);
                return rval;
            }
            catch (Exception e)//unhandled exception - should not happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Checkt ob der letzte Druckauftrag fertig ist
        /// </summary>
        /// <param name="input">iprintCheckedDokumenteDto</param>
        /// <returns>oprintCheckedDokumenteDto</returns>
        public oprintCheckedDokumenteDto checkPrintCheckedDokumente(iprintCheckedDokumenteDto input)
        {
            oprintCheckedDokumenteDto rval = new oprintCheckedDokumenteDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input printCheckedDokumenteDto was sent.");
                }
                if (input.sysid == 0 || input.sysid == 0)
                {
                    throw new ArgumentException("No Angebot sysid was sent.");
                }
                if (input.dokumente == null)
                {
                    throw new ArgumentException("No listDokumente was sent.");
                }
                cctx.validateService();

                try
                {
                    IPrintAngAntBo bo = BOFactory.getInstance().createPrintAngAntBo();
                    rval.file = bo.checkPrintCheckedDokumente(Mapper.Map<Cic.OpenOne.GateBANKNOW.Service.DTO.DokumenteDto[], Cic.OpenOne.GateBANKNOW.Common.DTO.DokumenteDto[]>(input.dokumente), input.sysid,input.variantenid, Cic.OpenOne.GateBANKNOW.Common.DTO.Enums.EaiHotOltable.Angebot, cctx.getMembershipInfo().sysPERSON);
                    if (rval.file == null)
                    {
                        rval.message = new OpenOne.Common.DTO.Message();
                        rval.message.code = "No PDF generated";
                        rval.message.detail = "Print PDF didn´t work correctly";
                    }
                    else
                    {
                        rval.success();
                    }
                }
                catch (ApplicationException ex)
                {
                    rval.message = new OpenOne.Common.DTO.Message();
                    rval.message.code = "Timeout";
                    rval.message.detail = "Timeout during EAIRequest";
                    rval.message.message = ex.ToString();
                }
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
    }
}