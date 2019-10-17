using System;
using System.ServiceModel;
using AutoMapper;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Service.BO;
using Cic.OpenOne.GateBANKNOW.Service.Contract;
using Cic.OpenOne.GateBANKNOW.Service.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "changeAntragService" in code, svc and config file together.
    /// <summary>
    /// Der Service changeAntrag ermöglicht das Kopieren und Löschen von Anträgen. Außerdem lassen sich Ablieferdatum, Stammnummer, Chassisnummer, Kontrollschild und Farbe ändern.
    /// </summary>
    [ServiceBehavior(Namespace = "http://cic-software.de/GateBANKNOW")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    class changeAntragService : IchangeAntragService
    {
        /// <summary>
        /// Kopiert Persistenzobjekte des Antrags (B2B)
        /// </summary>
        /// <param name="input">icopyAntragDto</param>
        /// <returns>ocopyAntragDto</returns>
        public ocopyAntragDto copyAntrag(icopyAntragDto input)
        {
            ocopyAntragDto rval = new ocopyAntragDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input copyAntragDto was sent.");
                }
                if (input.antrag == null)
                {
                    throw new ArgumentException("No Antrag was sent.");
                }
                if (input.antrag.sysid == 0 || input.antrag.sysid == 0)
                {
                    throw new ArgumentException("No Antrag sysid was sent.");
                }
                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, cctx.getMembershipInfo().sysPUSER, input.antrag.sysid, DateTime.Now);
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                IAngAntBo bo = BOFactory.getInstance().createAngAntBo();
                Common.DTO.AntragDto antrag = bo.copyAntrag(input.antrag.sysid, cctx.getMembershipInfo().sysPEROLE, true);

                rval.antrag = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.AntragDto, Cic.OpenOne.GateBANKNOW.Service.DTO.AntragDto>(antrag);

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
            catch (Exception e)//unhandled exception - shouldnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Kopiert Persistenzobjekte des Antrags by sysId (MAClient)
        /// </summary>
        /// <param name="sysId">Primary key</param>
        /// <returns>ocopyAntragDto</returns>
        public ocopyAntragDto copyAntragById(long sysId)
        {
            ocopyAntragDto rval = new ocopyAntragDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (sysId == 0)
                {
                    throw new ArgumentException("No Antrag Sysid was sent.");
                }

                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, cctx.getMembershipInfo().sysPUSER, sysId, DateTime.Now);
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                IAngAntBo bo = BOFactory.getInstance().createAngAntBoMA();

                Common.DTO.AntragDto antrag = bo.copyAntrag(sysId, cctx.getMembershipInfo().sysPEROLE, false);

                rval.antrag = Mapper.Map<Common.DTO.AntragDto, Service.DTO.AntragDto>(antrag);

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
                cctx.fillBaseDto(rval, e);
                return rval;
            }
            catch (Exception e)//unhandled exception - shouldnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Löscht Antrag und abhängige Tabellen
        /// </summary>
        /// <param name="input">ideleteAntragDto</param>
        /// <returns>odeleteAntragDto</returns>
        public odeleteAntragDto deleteAntrag(ideleteAntragDto input)
        {
            odeleteAntragDto rval = new odeleteAntragDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input deleteAntragDto was sent.");
                }
                if (input.sysID == 0 || input.sysID == 0)
                {
                    throw new ArgumentException("No input sysid was sent.");
                }
                cctx.validateService();
                AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, cctx.getMembershipInfo().sysPUSER, input.sysID, DateTime.Now);
                AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                IAngAntBo bo = BOFactory.getInstance().createAngAntBo();
                bo.deleteAntrag(input.sysID);

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
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Voraussichtlichen Liefertermin ändern
        /// </summary>
        /// <param name="input">isetAblieferdatumDto</param>
        /// <returns>osetAblieferdatumDto</returns>
        public osetAblieferdatumDto setAblieferdatum(isetAblieferdatumDto input)
        {
            osetAblieferdatumDto rval = new osetAblieferdatumDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input != null)
                {
                    if (input.ablieferdatum != null)
                    {
                        cctx.validateService();
                        AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, cctx.getMembershipInfo().sysPUSER, input.sysID, DateTime.Now);
                        AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                        Common.BO.ISimpleSetterBo bo = (ISimpleSetterBo)new SimpleSetterBo(new SimpleSetterDao());
                        bo.setAblieferdatum(input.sysID, input.ablieferdatum);

                        rval.message = new OpenOne.Common.DTO.Message();
                        rval.message.code = "True";
                        rval.message.detail = "Ablieferdatum saved!";
                        rval.success();
                    }
                    else
                    {
                        rval.message = new OpenOne.Common.DTO.Message();
                        rval.message.code = "Missing Data";
                        rval.message.detail = "Missing Ablieferdatum";
                    }
                }
                else
                {
                    rval.message = new OpenOne.Common.DTO.Message();
                    rval.message.code = "Missing Data";
                    rval.message.detail = "No Inputdata is set.";
                }
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e);
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Stammnummer ändern (*) ((*) Abweichende Änderungsmöglichkeit vom Zustandskonzept, daher via Setter Methoden)
        /// </summary>
        /// <param name="input">isetStammnummerDto</param>
        /// <returns>osetStammnummerDto</returns>
        public osetStammnummerDto setStammnummer(isetStammnummerDto input)
        {
            osetStammnummerDto rval = new osetStammnummerDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input != null)
                {
                    if (input.stammnummer != null)
                    {
                        cctx.validateService();
                        AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, cctx.getMembershipInfo().sysPUSER, input.sysID, DateTime.Now);
                        AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                        Common.BO.ISimpleSetterBo bo = (ISimpleSetterBo)new SimpleSetterBo(new SimpleSetterDao());
                        bo.setStammnummer(input.sysID, input.stammnummer);

                        rval.message = new OpenOne.Common.DTO.Message();
                        rval.message.code = "True";
                        rval.message.detail = "Stammnummer saved!";
                        rval.success();
                    }
                    else
                    {
                        rval.message = new OpenOne.Common.DTO.Message();
                        rval.message.code = "Missing Data";
                        rval.message.detail = "Missing Stammnummer";
                    }
                }
                else
                {
                    rval.message = new OpenOne.Common.DTO.Message();
                    rval.message.code = "Missing Data";
                    rval.message.detail = "No Inputdata is set.";
                }
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e);
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Chassisnummer ändern (*) ((*) Abweichende Änderungsmöglichkeit vom Zustandskonzept, daher via Setter Methoden)
        /// </summary>
        /// <param name="input">isetChassisnummerDto</param>
        /// <returns>osetChassisnummerDto</returns>
        public osetChassisnummerDto setChassisnummer(isetChassisnummerDto input)
        {
            osetChassisnummerDto rval = new osetChassisnummerDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input != null)
                {
                    if (input.chassisnummer != null)
                    {
                        cctx.validateService();
                        AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, cctx.getMembershipInfo().sysPUSER, input.sysID, DateTime.Now);
                        AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                        Common.BO.ISimpleSetterBo bo = (ISimpleSetterBo)new SimpleSetterBo(new SimpleSetterDao());
                        bo.setChassisnummer(input.sysID, input.chassisnummer);

                        rval.message = new OpenOne.Common.DTO.Message();
                        rval.message.code = "True";
                        rval.message.detail = "Chassisnummer saved!";
                        rval.success();
                    }
                    else
                    {
                        rval.message = new OpenOne.Common.DTO.Message();
                        rval.message.code = "Missing Data";
                        rval.message.detail = "Missing Chassisnummer";
                    }
                }
                else
                {
                    rval.message = new OpenOne.Common.DTO.Message();
                    rval.message.code = "Missing Data";
                    rval.message.detail = "No Inputdata is set";
                }
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e);
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Kontrollschild ändern (*) ((*) Abweichende Änderungsmöglichkeit vom Zustandskonzept, daher via Setter Methoden)
        /// </summary>
        /// <param name="input">isetKontrollschildDto</param>
        /// <returns>osetKontrollschildDto</returns>
        public osetKontrollschildDto setKontrollschild(isetKontrollschildDto input)
        {
            osetKontrollschildDto rval = new osetKontrollschildDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input != null)
                {
                    if (input.kontrollschild != null)
                    {
                        cctx.validateService();
                        AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, cctx.getMembershipInfo().sysPUSER, input.sysID, DateTime.Now);
                        AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                        Common.BO.ISimpleSetterBo bo = (ISimpleSetterBo)new SimpleSetterBo(new SimpleSetterDao());
                        bo.setKontrollschild(input.sysID, input.kontrollschild);

                        rval.message = new OpenOne.Common.DTO.Message();
                        rval.message.code = "True";
                        rval.message.detail = "Kontrollschild saved!";
                        rval.success();
                    }
                    else
                    {
                        rval.message = new OpenOne.Common.DTO.Message();
                        rval.message.code = "Missing Data";
                        rval.message.detail = "Missing Kontrollschild";
                    }
                }
                else
                {
                    rval.message = new OpenOne.Common.DTO.Message();
                    rval.message.code = "Missing Data";
                    rval.message.detail = "No Inputdata is set";
                }
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e);
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Farbe ändern (*) ((*) Abweichende Änderungsmöglichkeit vom Zustandskonzept, daher via Setter Methoden)
        /// </summary>
        /// <param name="input">isetFarbeDto</param>
        /// <returns>osetFarbeDto</returns>
        public osetFarbeDto setFarbe(isetFarbeDto input)
        {
            osetFarbeDto rval = new osetFarbeDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input != null)
                {
                    if (input.farbe != null)
                    {
                        cctx.validateService();
                        AuthenticationBo.validateUserPermission(ValidationType.ANTRAG, cctx.getMembershipInfo().sysPUSER, input.sysID, DateTime.Now);
                        AuthenticationBo.validateActivePerole(cctx.getMembershipInfo().sysPEROLE);

                        Common.BO.ISimpleSetterBo bo = (ISimpleSetterBo)new SimpleSetterBo(new SimpleSetterDao());
                        bo.setFarbe(input.sysID, input.farbe);

                        rval.message = new OpenOne.Common.DTO.Message();
                        rval.message.code = "True";
                        rval.message.detail = "Farbe saved!";
                        rval.success();
                    }
                    else
                    {
                        rval.message = new OpenOne.Common.DTO.Message();
                        rval.message.code = "Missing Data";
                        rval.message.detail = "Missing Farbe";
                    }
                }
                else
                {
                    rval.message = new OpenOne.Common.DTO.Message();
                    rval.message.code = "Missing Data";
                    rval.message.detail = "No Inputdata is set.";
                }
                return rval;
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e);
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }
    }
}