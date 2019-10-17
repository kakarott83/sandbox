using System;
using System.Collections.Generic;
using System.ServiceModel;
using AutoMapper;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Service.Contract;
using Cic.OpenOne.GateBANKNOW.Service.DTO;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DAO.Prisma;
using System.Linq;

namespace Cic.OpenOne.GateBANKNOW.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "getPartnerZusatzdatenService" in code, svc and config file together.
    /// <summary>
    /// Der Service getPartnerzusatzdaten liefert Zusatzdaten eines Partners.
    /// </summary>
    /// 
    [ServiceBehavior(Namespace = "http://cic-software.de/GateBANKNOW")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    class getPartnerZusatzdatenService : IgetPartnerZusatzdatenService
    {
        /// <summary>
        /// Liefert Daten des Händlerprofils
        /// </summary>
        /// <returns>ogetProfilDto</returns>
        /// <remarks>Keine Testdaten für das Feld mobil vorhanden (Stand: Konzept b2b_steuerung_1_2.pdf)</remarks>
        /// 
        public ogetProfilDto getProfil()
        {
            ogetProfilDto rval = new ogetProfilDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                
                Mapper.Map<GateBANKNOW.Common.DTO.ProfilDto, ogetProfilDto>(
                  BOFactory.getInstance().createSimpleGetterBo().getProfil(cctx.getMembershipInfo().sysPEROLE),
                  rval);

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
            catch (Exception e)//unhandled exception - shouldnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Liefert Daten über den Key Account Manager des Händlers
        /// </summary>
        /// <returns>ogetKamDto</returns>
        public ogetKamDto getKam()
        {
            ogetKamDto rval = new ogetKamDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                

                AutoMapper.Mapper.Map<GateBANKNOW.Common.DTO.KamDto, ogetKamDto>(
                  BOFactory.getInstance().createSimpleGetterBo().getKam(cctx.getMembershipInfo().sysPEROLE),
                  rval);

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
            catch (Exception e)//unhandled exception - shouldnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Liefert Daten über den Abwicklungsort des Händlers
        /// </summary>
        /// <returns>ogetAbwicklungsortDto</returns>
        public ogetAbwicklungsortDto getAbwicklungsort()
        {
            ogetAbwicklungsortDto rval = new ogetAbwicklungsortDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                
                AutoMapper.Mapper.Map<GateBANKNOW.Common.DTO.AbwicklungsortDto, ogetAbwicklungsortDto>(
                 BOFactory.getInstance().createSimpleGetterBo().getAbwicklungsort(cctx.getMembershipInfo().sysPEROLE),
                  rval);

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
            catch (Exception e)//unhandled exception - shouldnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <returns></returns>
        /// <summary>
        /// Liefert eine Liste der verfügbaren News im Kontext
        /// </summary>
        /// <param name="input">ilistAvailableNewsDto</param>
        /// <returns>olistAvailableNewsDto</returns>
        public olistAvailableNewsDto listAvailableNews(ilistAvailableNewsDto input)
        {
            olistAvailableNewsDto rval = new olistAvailableNewsDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (input == null)
                {
                    throw new ArgumentException("No input listAvailableNewsDto is send");
                }
                cctx.validateService();

                IPrismaDao pDao = PrismaDaoFactory.getInstance().getPrismaDao();
                IObTypDao obDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao();
                PrismaNewsBo bo = new PrismaNewsBo(pDao, obDao, PrismaNewsBo.CONDITIONS_BANKNOW, cctx.getUserLanguange());

                //create context
                Cic.OpenOne.Common.DTO.Prisma.prKontextDto kontext = new Cic.OpenOne.Common.DTO.Prisma.prKontextDto();
                kontext.sysbrand = input.sysbrand;
                kontext.sysprchannel = input.sysprchannel;
                using (DdOlExtended ctx = new DdOlExtended())
                {
                    kontext.sysperole = PeRoleUtil.FindRootPEROLEByRoleType(ctx , cctx.getMembershipInfo().sysPEROLE, (int)RoleTypeTyp.HAENDLER);
                }
                List<Cic.OpenOne.Common.DTO.AvailableNewsDto> news = bo.listAvailableNews(kontext, cctx.getUserLanguange(), input.binaryData);
                news = (from s in news
                             select s).OrderByDescending(x => x.validFrom).ThenByDescending(x => x.sysID).ToList();
                rval.news = Mapper.Map<Cic.OpenOne.Common.DTO.AvailableNewsDto[], Cic.OpenOne.GateBANKNOW.Service.DTO.AvailableNewsDto[]>(news.ToArray());

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
                // Entsorgt die nicht verwendung der Exception
                //String message = e.Message;
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - should not happen!
            {
                // Entsorgt die nicht verwendung der Exception
                String message = e.Message;
                //cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Markiert die Alerts der übergebenen Antraege als gelesen
        /// </summary>
        /// <param name="input">ideleteAvailableAlertsDto</param>
        /// <returns>Liste der verfügbaren Alerts im Kontext</returns>
        public olistAvailableAlertsDto deleteAvailableAlerts(ideleteAvailableAlertsDto input)
        {
            olistAvailableAlertsDto rval = new olistAvailableAlertsDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                IRoleContextListsBo roleBo = BOFactory.getInstance().createRoleContextListBo();

                //remove given alerts
                if (input.sysIds != null && input.sysIds.Length > 0)
                    foreach (long id in input.sysIds)
                        roleBo.setAlertsAsReaded(id, cctx.getMembershipInfo().sysPEROLE);

                if (input.returnList)
                {
                    if (input.isoCode == null)
                        throw new ArgumentException("No isoCode given");
                    //return remaining alerts
                    Cic.OpenOne.GateBANKNOW.Common.DTO.AvailableAlertsDto[] outvals = roleBo.listAvailableAlerts(input.isoCode, cctx.getMembershipInfo().sysPEROLE);
                    rval.alerts = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.AvailableAlertsDto[], Cic.OpenOne.GateBANKNOW.Service.DTO.AvailableAlertsDto[]>(outvals);
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
        /// Liefert eine Liste der verfügbaren Alerts im Kontext
        /// </summary>
        /// <param name="isoCode">isoCode</param>
        /// <returns>olistAvailableAlertsDto</returns>
        public olistAvailableAlertsDto listAvailableAlerts(string isoCode)
        {
            olistAvailableAlertsDto rval = new olistAvailableAlertsDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                if (isoCode == null)
                {
                    throw new ArgumentException("No isoCode is send");
                }
                cctx.validateService();
                IRoleContextListsBo roleBo = BOFactory.getInstance().createRoleContextListBo();


                Cic.OpenOne.GateBANKNOW.Common.DTO.AvailableAlertsDto[] outvals = roleBo.listAvailableAlerts(isoCode, cctx.getMembershipInfo().sysPEROLE);
                rval.alerts = Mapper.Map<Cic.OpenOne.GateBANKNOW.Common.DTO.AvailableAlertsDto[], Cic.OpenOne.GateBANKNOW.Service.DTO.AvailableAlertsDto[]>(outvals);


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
        /// Liefert eine Liste der verfügbaren Kanäle im Kontext
        /// </summary>
        /// <returns>olistAvailableChannelsDto</returns>
        public olistAvailableChannelsDto listAvailableChannels()
        {
            olistAvailableChannelsDto rval = new olistAvailableChannelsDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();

                rval.channels = new RoleContextListsBo(new RoleContextListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao()).listAvailableChannels(cctx.getMembershipInfo().sysPEROLE, cctx.getUserLanguange());
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
            catch (Exception e)//unhandled exception - should not happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Liefert eine Liste der verfügbaren Brands (Bildwelten) im Kontext
        /// </summary>
        /// <returns>olistAvailableBrandsDto</returns>
        public olistAvailableBrandsDto listAvailableBrands()
        {
            olistAvailableBrandsDto rval = new olistAvailableBrandsDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();

                rval.brands = new RoleContextListsBo(new RoleContextListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao()).listAvailableBrands(cctx.getMembershipInfo().sysPEROLE);

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
            catch (Exception e)//unhandled exception - should not happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Liefert alle User zum Händler
        /// </summary>
        /// <returns>olistAvailableUserDto</returns>
        public olistAvailableUserDto listAvailableUser()
        {
            olistAvailableUserDto rval = new olistAvailableUserDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                MembershipUserValidationInfo user = cctx.getMembershipInfo();
                AbstractPeuserBo bo = new PeuserBo(new PeuserDao());
                rval.user = bo.listAvailableUser(user.sysPEROLE);
                if (rval.user == null)
                {
                    rval.message = new OpenOne.Common.DTO.Message();
                    rval.message.code = "Error";
                    rval.message.detail = "Es gibt keinen Händler für diesen User";
                }
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
            catch (Exception e)//unhandled exception - should not happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Liefert alle User zum Händler
        /// </summary>
        /// <returns>olistAvailableUserDto</returns>
        public olistAvailableUserDto listAvailableUserWithStatus(Cic.OpenOne.GateBANKNOW.Service.DTO.PeroleActiveStatus activeStatus)
        {
            olistAvailableUserDto rval = new olistAvailableUserDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                MembershipUserValidationInfo user = cctx.getMembershipInfo();
                AbstractPeuserBo bo = new PeuserBo(new PeuserDao());
                rval.user = bo.listAvailableUser(user.sysPEROLE,(Cic.OpenOne.GateBANKNOW.Common.DTO.PeroleActiveStatus)activeStatus);
                if (rval.user == null)
                {
                    rval.message = new OpenOne.Common.DTO.Message();
                    rval.message.code = "Error";
                    rval.message.detail = "Es gibt keinen Händler für diesen User";
                }
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
            catch (Exception e)//unhandled exception - should not happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
        }

        /// <summary>
        /// Liefert alle Quoten incl. deren zeitlicher Gültigkeiten
        /// </summary>
        /// <returns></returns>
        public olistQuoteInfoDto listAvailableQuotes()
        {
            olistQuoteInfoDto rval = new olistQuoteInfoDto();
            CredentialContext cctx = new CredentialContext();
            try
            {
                cctx.validateService();
                rval.quotes = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getQuoteDao().getQuotes();
            }
            catch (System.Data.Entity.Core.EntityException e)
            {
                cctx.fillBaseDto(rval, e, "F_00004_DatabaseUnreachableException");
            }
            catch (ServiceBaseException e)
            {
                cctx.fillBaseDto(rval, e);
            }
            catch (Exception e)
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
            }

            return rval;
        }

    }
}