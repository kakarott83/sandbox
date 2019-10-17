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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "changeAngebotService" in code, svc and config file together.
    /// <summary>
    /// Der Service changeAngebot kann vorhandene Angebote kopieren oder löschen.
    /// </summary>
    [ServiceBehavior(Namespace = "http://cic-software.de/GateBANKNOW")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    class changeAngebotService : IchangeAngebotService
    {
        /// <summary>
        /// Kopiert Persistenzobjekte des Angebots und aller Angebotsvarianten
        /// </summary>
        /// <param name="input">icopyAngebotDto</param>
        /// <returns>ocopyAngebotDto</returns>
        public ocopyAngebotDto copyAngebot(icopyAngebotDto input)
        {
            ocopyAngebotDto rval = new ocopyAngebotDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("Input copyAngebotDto was not sent.");
                }
                if (input.angebot == null)
                {
                    throw new ArgumentException("Input Angebot was not sent.");
                }
                if (input.angebot.sysid == 0 || input.angebot.sysid == 0)
                {
                    throw new ArgumentException("Input Angebot has no sysid.");
                }

                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANGEBOT, cctx.getMembershipInfo().sysPUSER, input.angebot.sysid, DateTime.Now);
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                IAngAntBo bo = BOFactory.getInstance().createAngAntBo();
                Common.DTO.AngebotDto angebot = bo.copyAngebot(input.angebot.sysid, cctx.getMembershipInfo().sysPEROLE, cctx.getUserLanguange());

                rval.angebot = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AngebotDto>(angebot);
                Cic.OpenOne.GateBANKNOW.Service.BO.StatusEPOSBo.setStatusEPOS(rval.angebot);
                if (angebot.errortext.Count > 0)
                {
                    rval.message.type = OpenOne.Common.DTO.MessageType.Info;
                    rval.message.message = String.Join(",", angebot.errortext.ToArray());
                }
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
        /// Kopiert Persistenzobjekte des Angebots und aller Angebotsvarianten
        /// </summary>
        /// <param name="sysid">Primary key</param>
        /// <returns>ocopyAngebotDto</returns>
        public ocopyAngebotDto copyAngebotById(long sysid)
        {
            ocopyAngebotDto rval = new ocopyAngebotDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (sysid == 0)
                {
                    throw new ArgumentException("No Angebot Sysid was sent.");
                }
                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANGEBOT, cctx.getMembershipInfo().sysPUSER, sysid, DateTime.Now);
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                IAngAntBo bo = BOFactory.getInstance().createAngAntBo();
                Common.DTO.AngebotDto angebot = bo.copyAngebot(sysid, cctx.getMembershipInfo().sysPEROLE, cctx.getUserLanguange());

                rval.angebot = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.AngebotDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AngebotDto>(angebot);
                Cic.OpenOne.GateBANKNOW.Service.BO.StatusEPOSBo.setStatusEPOS(rval.angebot);
                if (angebot.errortext.Count > 0)
                {
                    rval.message.type = OpenOne.Common.DTO.MessageType.Info;
                    rval.message.message = String.Join(",", angebot.errortext.ToArray());
                }
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
        /// Löscht Angebot und alle Angebotsvarianten sowie abhängige Tabellen
        /// </summary>
        /// <param name="input">ideleteAngebotDto</param>
        /// <returns>odelteAngebotDto</returns>
        public odeleteAngebotDto deleteAngebot(ideleteAngebotDto input)
        {
            odeleteAngebotDto rval = new odeleteAngebotDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input deleteAngebotDto was sent.");
                }
                if (input.sysID == 0 || input.sysID == 0)
                {
                    throw new ArgumentException("No input sysid was sent.");
                }
                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANGEBOT, cctx.getMembershipInfo().sysPUSER, input.sysID, DateTime.Now);
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                IAngAntBo bo = BOFactory.getInstance().createAngAntBo();
                bo.deleteAngebot(input.sysID);

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
                cctx.fillBaseDto(rval, e);
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
