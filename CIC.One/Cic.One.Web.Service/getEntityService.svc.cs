using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Cic.One.DTO;
using Cic.One.Web.BO;
using Cic.One.Web.BO.Search;
using Cic.One.Web.DAO;
using System.Reflection;
using Cic.One.Web.Service.DAO;
using Cic.One.Web.Contract;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.BO.Prisma;
using AutoMapper;
using Cic.OpenLease.Service;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.Common.Util.Serialization;
using Cic.OpenOne.Common.Util.Logging;


namespace Cic.One.Web.Service
{
    [ServiceBehavior(Namespace = "http://cic-software.de/One")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public class getEntityService : IgetEntityService
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// delivers CREFO detail
        /// </summary>
        /// <param name="iDto"></param>
        /// <returns></returns>
        public ogetCREFODetailDto getCREFODetail(igetCREFODetailDto iDto)
        {
            ServiceHandler<igetCREFODetailDto, ogetCREFODetailDto> ew = new ServiceHandler<igetCREFODetailDto, ogetCREFODetailDto>(iDto);
            return ew.process(delegate(igetCREFODetailDto input, ogetCREFODetailDto rval, CredentialContext ctx)
            {
                if (input == null)
                    throw new ArgumentException("No input");


                CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto[] vars = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto[2];
                vars[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto();
                vars[0].LookupVariableName = "CrefoSearchSuccess";
                vars[0].VariableName = "CREFO";
                vars[0].Value = "VARS";
                vars[1] = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto();
                vars[1].LookupVariableName = "CompanyId";
                vars[1].VariableName = "CREFO";
                vars[1].Value = input.crefoid;


                CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionDto[] subscriptions = new CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionDto[3];
                subscriptions[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionDto();
                subscriptions[0].ObjectKey = "CrefoSuccess";
                subscriptions[0].ObjectName = "CREFO";
                subscriptions[0].ObjectType = "L";
                subscriptions[1] = new CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionDto();
                subscriptions[1].ObjectName = "CrefoResult";
                subscriptions[1].ObjectType = "Q";
                subscriptions[2] = new CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionDto();
                subscriptions[2].ObjectName = "CompanyId";
                subscriptions[2].ObjectType = "L";
                subscriptions[2].ObjectKey = "CompanyId";

                CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto[] queues = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto[1];
                queues[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto();
                queues[0].Name = "CrefoResult";
                queues[0].Records = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto[1];
                queues[0].Records[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto();
                queues[0].Records[0].Values = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto[2];
                queues[0].Records[0].Values[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto();
                queues[0].Records[0].Values[0].Value = "F01";
                queues[0].Records[0].Values[0].VariableName = "country";
                queues[0].Records[0].Values[1] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto();
                queues[0].Records[0].Values[1].Value = "F02";
                queues[0].Records[0].Values[1].VariableName = "DE";

                

                CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionsDataDto rvalq = BOS.getFormulaData(1, "SYSTEM", "CREFOBONITAET", vars, subscriptions, queues, ctx.getMembershipInfo().sysWFUSER);
                String success = (from lv in rvalq.Variables
                                  where lv.LookupVariableName.Equals("CrefoSuccess")
                                  select lv.Value).FirstOrDefault();
                if ("True".Equals(success))
                {
                    CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto[] records = (from lv in rvalq.Queues
                                                                                          where lv.Name.Equals("CrefoResult")
                                                                                          select lv.Records).FirstOrDefault();
                    try
                    {
                        rval.data = FileUtils.loadData(records[1].Values[0].Value);
                    }
                    catch (Exception ex)
                    {
                        _log.Warn("Loading Crefofile failed", ex);
                    }
                    rval.kundenrating = records[0].Values[0].Value;
                    rval.description1 = records[0].Values[1].Value;
                    rval.description2 = records[0].Values[2].Value;
                    //String filename = records[1].Values[1].Value;

                }
                else throw new Exception("CREFO not successful: " + success);

              

            });
        }

        /// <summary>
        /// delivers Guardean detail
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="gviewId"></param>
        /// <returns></returns>
        public ogetGuardeanDetailDto getGuardeanDetail(igetGuardeanDto iDto)
        {
            ServiceHandler<igetGuardeanDto, ogetGuardeanDetailDto> ew = new ServiceHandler<igetGuardeanDto, ogetGuardeanDetailDto>(iDto);
            return ew.process(delegate(igetGuardeanDto input, ogetGuardeanDetailDto rval, CredentialContext ctx)
            {
                if (input == null)
                    throw new ArgumentException("No input");

/*

                byte[] wfvconfigData = null;
                try
                {
                    wfvconfigData = FileUtils.loadData(FileUtils.getCurrentPath() + "\\..\\guardean.xml");
                }catch(Exception e)
                {
                    try
                    {
                        wfvconfigData = FileUtils.loadData(FileUtils.getCurrentPath() + "\\..\\..\\guardean.xml");
                    }catch(Exception exc)
                    {
                        wfvconfigData = FileUtils.loadData("C:\\project-data\\guardean.xml");
                    }
                }
                rval.gview= XMLDeserializer.objectFromXml<GviewDto>(wfvconfigData, "UTF-8");
                */
                IGuardeanBo bo = BOFactoryFactory.getInstance().getGuardeanBo();
                GuardeanBo gbo = (GuardeanBo)bo;
                
                ogetGuardeanDetailDto tmp = gbo.getAuskunft(iDto.context.entities);
                rval.data = tmp.data;
                rval.gview = tmp.gview;
                
               // rval.gview = bo.getGviewDetails(input, gviewId);

            });
        }

        /// <summary>
        /// delivers Gview detail
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="gviewId"></param>
        /// <param name="wcctx"></param>
        /// <returns></returns>
        public ogetGviewDetailDto getGviewDetail(long sysid, String gviewId, WorkflowContext wcctx)
        {
            if (wcctx == null)
            {
                wcctx = new WorkflowContext();
                wcctx.entities = new EntityContainer();
            }

            wcctx.areaid = ""+sysid;
            ServiceHandler<WorkflowContext, ogetGviewDetailDto> ew = new ServiceHandler<WorkflowContext, ogetGviewDetailDto>(wcctx);
            return ew.process(delegate(WorkflowContext input, ogetGviewDetailDto rval, CredentialContext ctx)
            {
                
                
                    IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());

                    rval.gview = bo.getGviewDetails(long.Parse(input.areaid),gviewId,input);
                

            });
        }

        /// <summary>
        /// delivers Staffelpositionstyp detail
        /// </summary>
        /// <param name="sysslpostyp"></param>
        /// <returns></returns>
        public ogetStaffelpositionstypDetailDto getStaffelpositionstypDetail(long sysslpostyp)
        {
            ServiceHandler<long, ogetStaffelpositionstypDetailDto> ew = new ServiceHandler<long, ogetStaffelpositionstypDetailDto>(sysslpostyp);
            return ew.process(delegate(long input, ogetStaffelpositionstypDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());

                rval.staffelpositionstyp = bo.getStaffelpositionstypDetails(input);
                
            });
        }

        /// <summary>
        /// delivers Staffeltyp detail
        /// </summary>
        /// <param name="syssltyp"></param>
        /// <returns></returns>
        public ogetStaffeltypDetailDto getStaffeltypDetail(long syssltyp)
        {
            ServiceHandler<long, ogetStaffeltypDetailDto> ew = new ServiceHandler<long, ogetStaffeltypDetailDto>(syssltyp);
            return ew.process(delegate(long input, ogetStaffeltypDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());

                rval.staffeltyp = bo.getStaffeltypDetails(input);
                
            });
        }

        /// <summary>
        /// delivers Rolle detail
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        public ogetRolleDetailDto getRolleDetail(long sysperole)
        {
            ServiceHandler<long, ogetRolleDetailDto> ew = new ServiceHandler<long, ogetRolleDetailDto>(sysperole);
            return ew.process(delegate(long input, ogetRolleDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());

                rval.rolle = bo.getRolleDetails(input);
                
            });
        }

        /// <summary>
        /// delivers Rollentyp detail
        /// </summary>
        /// <param name="sysroletype"></param>
        /// <returns></returns>
        public ogetRollentypDetailDto getRollentypDetail(long sysroletype)
        {
            ServiceHandler<long, ogetRollentypDetailDto> ew = new ServiceHandler<long, ogetRollentypDetailDto>(sysroletype);
            return ew.process(delegate(long input, ogetRollentypDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());

                rval.rollentyp = bo.getRollentypDetails(input);
                
            });
        }

        /// <summary>
        /// delivers Handelsgruppe detail
        /// </summary>
        /// <param name="sysprhgroup"></param>
        /// <returns></returns>
        public ogetHandelsgruppeDetailDto getHandelsgruppeDetail(long sysprhgroup)
        {
            ServiceHandler<long, ogetHandelsgruppeDetailDto> ew = new ServiceHandler<long, ogetHandelsgruppeDetailDto>(sysprhgroup);
            return ew.process(delegate(long input, ogetHandelsgruppeDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());

                rval.handelsgruppe = bo.getHandelsgruppeDetails(input);
                
            });
        }

        /// <summary>
        /// delivers Vertriebskanal detail
        /// </summary>
        /// <param name="sysbchannel"></param>
        /// <returns></returns>
        public ogetVertriebskanalDetailDto getVertriebskanalDetail(long sysbchannel)
        {
            ServiceHandler<long, ogetVertriebskanalDetailDto> ew = new ServiceHandler<long, ogetVertriebskanalDetailDto>(sysbchannel);
            return ew.process(delegate(long input, ogetVertriebskanalDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());

                rval.vertriebskanal = bo.getVertriebskanalDetails(input);
                
            });
        }

        /// <summary>
        /// delivers Brand detail
        /// </summary>
        /// <param name="sysbrand"></param>
        /// <returns></returns>
        public ogetBrandDetailDto getBrandDetail(long sysbrand)
        {
            ServiceHandler<long, ogetBrandDetailDto> ew = new ServiceHandler<long, ogetBrandDetailDto>(sysbrand);
            return ew.process(delegate(long input, ogetBrandDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());

                rval.brand = bo.getBrandDetails(input);
                
            });
        }

        /// <summary>
        /// delivers Rechnung detail
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public ogetRechnungDetailDto getRechnungDetail(long sysid)
        {
            ServiceHandler<long, ogetRechnungDetailDto> ew = new ServiceHandler<long, ogetRechnungDetailDto>(sysid);
            return ew.process(delegate(long input, ogetRechnungDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());

                rval.rechnung = bo.getRechnungDetails(input);
                
            });
        }

        /// <summary>
        /// delivers Angobbrief detail
        /// </summary>
        /// <param name="sysangobbrief"></param>
        /// <returns></returns>
        public ogetAngobbriefDetailDto getAngobbriefDetail(long sysangobbrief)
        {
            ServiceHandler<long, ogetAngobbriefDetailDto> ew = new ServiceHandler<long, ogetAngobbriefDetailDto>(sysangobbrief);
            return ew.process(delegate(long input, ogetAngobbriefDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());

                rval.angobbrief = bo.getAngobbriefDetails(input);
                
            });
        }

        /// <summary>
        /// delivers Zahlungsplan detail
        /// </summary>
        /// <param name="sysslpos"></param>
        /// <returns></returns>
        public ogetZahlungsplanDetailDto getZahlungsplanDetail(long sysslpos)
        {
            ServiceHandler<long, ogetZahlungsplanDetailDto> ew = new ServiceHandler<long, ogetZahlungsplanDetailDto>(sysslpos);
            return ew.process(delegate(long input, ogetZahlungsplanDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());

                rval.zahlungsplan = bo.getZahlungsplanDetails(input);
                
            });
        }

        /// <summary>
        /// delivers Fahrzeugbrief detail
        /// </summary>
        /// <param name="sysobbrief"></param>
        /// <returns></returns>
        public ogetFahrzeugbriefDetailDto getFahrzeugbriefDetail(long sysobbrief)
        {
            ServiceHandler<long, ogetFahrzeugbriefDetailDto> ew = new ServiceHandler<long, ogetFahrzeugbriefDetailDto>(sysobbrief);
            return ew.process(delegate(long input, ogetFahrzeugbriefDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());

                rval.fahrzeugbrief = bo.getFahrzeugbriefDetails(input);
                
            });
        }

        /// <summary>
        /// delivers Kalk detail
        /// </summary>
        /// <param name="syskalk"></param>
        /// <returns></returns>
        public ogetKalkDetailDto getKalkDetail(long syskalk)
        {
            ServiceHandler<long, ogetKalkDetailDto> ew = new ServiceHandler<long, ogetKalkDetailDto>(syskalk);
            return ew.process(delegate(long input, ogetKalkDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());

                rval.kalk = bo.getKalkDetails(input);
                
            });
        }

        /// <summary>
        /// delivers Person detail
        /// </summary>
        /// <param name="sysperson"></param>
        /// <returns></returns>
        public ogetPersonDetailDto getPersonDetail(long sysperson)
        {
            ServiceHandler<long, ogetPersonDetailDto> ew = new ServiceHandler<long, ogetPersonDetailDto>(sysperson);
            return ew.process(delegate(long input, ogetPersonDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());

                rval.person = bo.getPersonDetails(input);
                
            });
        }
        /// <summary>
        /// Returns the Eaihot
        /// </summary>
        /// <param name="syseaihot"></param>
        /// <returns></returns>
        public ogetEaihotDto getEaihotDetail(long syseaihot)
        {
            ServiceHandler<long, ogetEaihotDto> ew = new ServiceHandler<long, ogetEaihotDto>(syseaihot);
            return ew.process(delegate(long input, ogetEaihotDto rval, CredentialContext ctx)
            {

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.eaihot = bo.getEaihotDetails(input);
            });
        }


        /// <summary>
        /// Returns the image link to the entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="sysid"></param>
        /// <param name="vlmcode"></param>
        /// <returns></returns>
        public ogetEntityLinkDto getEntityLink(String entity, long sysid, String vlmcode)
        {
            ServiceHandler<long, ogetEntityLinkDto> ew = new ServiceHandler<long, ogetEntityLinkDto>(sysid);
            return ew.process(delegate(long input, ogetEntityLinkDto rval, CredentialContext ctx)
            {

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.url = bo.getEntityLink(entity, input, ctx.getMembershipInfo().sysWFUSER,vlmcode);
            });
        }

        /// <summary>
        /// Returns the icon Information
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="sysid"></param>
        /// <param name="vlmcode"></param>
        /// <returns></returns>
        public ogetEntityIconDto getEntityIcon(String entity, long sysid, String vlmcode)
        {
            ServiceHandler<long, ogetEntityIconDto> ew = new ServiceHandler<long, ogetEntityIconDto>(sysid);
            return ew.process(delegate(long input, ogetEntityIconDto rval, CredentialContext ctx)
            {
                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.icon = bo.getEntityIcon(entity, input, ctx.getMembershipInfo().sysWFUSER,vlmcode);
            });
        }

        /// <summary>
        /// Returns the icon Informations
        /// </summary>
        /// <param name="icons"></param>       
        /// <returns></returns>
        public ogetEntityIconsDto getEntityIcons(igetEntityIconsDto icons)
        {
            ServiceHandler<igetEntityIconsDto, ogetEntityIconsDto> ew = new ServiceHandler<igetEntityIconsDto, ogetEntityIconsDto>(icons);
            return ew.process(delegate(igetEntityIconsDto input, ogetEntityIconsDto rval, CredentialContext ctx)
            {

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.icons = new List<EntityIconDto>();
                
                rval.icons=bo.getEntityIcons(input.entity, icons.keys, ctx.getMembershipInfo().sysWFUSER, input.vlmcode);
                
            });
        }

        /// <summary>
        /// delivers Opportunity detail
        /// </summary>
        /// <param name="sysoppo"></param>
        /// <returns></returns>
        public ogetOpportunityDetailDto getOpportunityDetail(long sysoppo)
        {
            ServiceHandler<long, ogetOpportunityDetailDto> ew = new ServiceHandler<long, ogetOpportunityDetailDto>(sysoppo);
            return ew.process(delegate(long input, ogetOpportunityDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.opportunity = bo.getOpportunityDetails(input);
            });
        }

        /// <summary>
        /// delivers sysoppotask detail
        /// </summary>
        /// <param name="sysoppotask"></param>
        /// <returns></returns>
        public ogetOppotaskDetailDto getOppotaskDetail(long sysoppotask)
        {
            ServiceHandler<long, ogetOppotaskDetailDto> ew = new ServiceHandler<long, ogetOppotaskDetailDto>(sysoppotask);
            return ew.process(delegate(long input, ogetOppotaskDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.oppotask = bo.getOppotaskDetails(input);
            });
        }


        /// <summary>
        /// delivers Wfsignature detail
        /// </summary>
        /// <param name="syswfsignature"></param>
        /// <returns></returns>
        public ogetWfsignatureDetailDto getWfsignatureDetail(long syswfsignature)
        {

            ServiceHandler<long, ogetWfsignatureDetailDto> ew = new ServiceHandler<long, ogetWfsignatureDetailDto>(syswfsignature);
            return ew.process(delegate(long input, ogetWfsignatureDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.wfsignature = bo.getWfsignatureDetail(input);
            });
        }

        /// <summary>
        /// delivers Wfsignature
        /// </summary>
        /// <param name="stickytype"></param>
        /// <returns></returns>
        public ogetWfsignatureDetailDto getWfsignatureFromTypeDetail(igetWfsignatureDetailDto inp)
        {
            ServiceHandler<igetWfsignatureDetailDto, ogetWfsignatureDetailDto> ew = new ServiceHandler<igetWfsignatureDetailDto, ogetWfsignatureDetailDto>(inp);
            return ew.process(delegate(igetWfsignatureDetailDto input, ogetWfsignatureDetailDto rval, CredentialContext ctx)
            {

                if (input == null || input == null)
                    throw new ArgumentException("No valid input");

                rval.wfsignature = BOFactoryFactory.getInstance().getEntityMailBO(ctx.getMembershipInfo()).getWfsignatureDetail(input);

            });
        }

        /// <summary>
        /// delivers Kunde detail
        /// </summary>
        /// <param name="sysperson"></param>
        /// <returns></returns>
        public ogetKundeDetailDto getKundeDetail(long sysperson)
        {
            ServiceHandler<long, ogetKundeDetailDto> ew = new ServiceHandler<long, ogetKundeDetailDto>(sysperson);
            return ew.process(delegate(long input, ogetKundeDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.kunde = bo.getKundeDetails(input);
            });
        }

        /// <summary>
        /// delivers LogDump detail
        /// </summary>
        /// <param name="sysperson"></param>
        /// <returns></returns>
        public ogetLogDumpDetailDto getLogDumpDetail(long logDump)
        {
            ServiceHandler <long, ogetLogDumpDetailDto> ew = new ServiceHandler <long, ogetLogDumpDetailDto> (logDump);
            return ew.process (delegate(long input, ogetLogDumpDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException ("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO (ctx.getMembershipInfo());
                rval.logDump = bo.getLogDumpDetails (input);
            });
        }


        /// <summary>
        /// delivers It detail
        /// </summary>
        /// <param name="sysit"></param>
        /// <returns></returns>
        public ogetItDetailDto getItDetail(long sysit)
        {
            ServiceHandler<long, ogetItDetailDto> ew = new ServiceHandler<long, ogetItDetailDto>(sysit);
            return ew.process(delegate(long input, ogetItDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.It = bo.getItDetails(input);
            });
        }

        /// <summary>
        /// delivers objekt detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ogetObjektDetailDto getObjektDetail(long sysob)
        {

            ServiceHandler<long, ogetObjektDetailDto> ew = new ServiceHandler<long, ogetObjektDetailDto>(sysob);
            return ew.process(delegate(long input, ogetObjektDetailDto rval, CredentialContext ctx)
            {

                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.objekt = bo.getObjektDetails(input);

            });
        }

        /// <summary>
        /// delivers objekt detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ogetObtypDetailDto getObtypDetail(long sysobtyp)
        {

            ServiceHandler<long, ogetObtypDetailDto> ew = new ServiceHandler<long, ogetObtypDetailDto>(sysobtyp);
            return ew.process(delegate(long input, ogetObtypDetailDto rval, CredentialContext ctx)
            {
                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.obtyp = bo.getObtypDetails(input);

            });
        }

        /// <summary>
        /// delivers recalc detail
        /// </summary>
        /// <param name="sysrecalc"></param>
        /// <returns></returns>
        public ogetRecalcDetailDto getRecalcDetail(long sysrecalc)
        {

            ServiceHandler<long, ogetRecalcDetailDto> ew = new ServiceHandler<long, ogetRecalcDetailDto>(sysrecalc);
            return ew.process(delegate(long input, ogetRecalcDetailDto rval, CredentialContext ctx)
            {

                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.recalc = bo.getRecalcDetails(input);

            });
        }

        /// <summary>
        /// delivers Mycalc detail
        /// </summary>
        /// <param name="sysmycalc"></param>
        /// <returns></returns>
        public ogetMycalcDetailDto getMycalcDetail(long sysmycalc)
        {

            ServiceHandler<long, ogetMycalcDetailDto> ew = new ServiceHandler<long, ogetMycalcDetailDto>(sysmycalc);
            return ew.process(delegate(long input, ogetMycalcDetailDto rval, CredentialContext ctx)
            {

                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.mycalc = bo.getMycalcDetails(input);

            });
        }

        /// <summary>
        /// delivers rahmen detail
        /// </summary>
        /// <param name="sysrvt"></param>
        /// <returns></returns>
        public ogetRahmenDetailDto getRahmenDetail(long sysrvt)
        {

            ServiceHandler<long, ogetRahmenDetailDto> ew = new ServiceHandler<long, ogetRahmenDetailDto>(sysrvt);
            return ew.process(delegate(long input, ogetRahmenDetailDto rval, CredentialContext ctx)
            {

                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.rahmen = bo.getRahmenDetails(input);

            });
        }

        /// <summary>
        /// delivers Haendler detail
        /// </summary>
        /// <param name="sysperson"></param>
        /// <returns></returns>
        public ogetHaendlerDetailDto getHaendlerDetail(long sysperson)
        {

            ServiceHandler<long, ogetHaendlerDetailDto> ew = new ServiceHandler<long, ogetHaendlerDetailDto>(sysperson);
            return ew.process(delegate(long input, ogetHaendlerDetailDto rval, CredentialContext ctx)
            {

                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.haendler = bo.getHaendlerDetails(input);

            });
        }


        /// <summary>
        /// delivers Contact detail
        /// </summary>
        /// <param name="syscontact"></param>
        /// <returns></returns>
        public ogetContactDetailDto getContactDetail(long syscontact)
        {
            ServiceHandler<long, ogetContactDetailDto> ew = new ServiceHandler<long, ogetContactDetailDto>(syscontact);
            return ew.process(delegate(long input, ogetContactDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.Contact = bo.getContactDetails(input);
            });
        }

        /// <summary>
        /// delivers Itkonto detail
        /// </summary>
        /// <param name="sysitkonto"></param>
        /// <returns></returns>
        public ogetItkontoDetailDto getItkontoDetail(long sysitkonto)
        {

            ServiceHandler<long, ogetItkontoDetailDto> ew = new ServiceHandler<long, ogetItkontoDetailDto>(sysitkonto);
            return ew.process(delegate(long input, ogetItkontoDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.Itkonto = bo.getItkontoDetails(input);
            });
        }

        /// <summary>
        /// delivers Konto detail
        /// </summary>
        /// <param name="syskonto"></param>
        /// <returns></returns>
        public ogetKontoDetailDto getKontoDetail(long syskonto)
        {


           

            ServiceHandler<long, ogetKontoDetailDto> ew = new ServiceHandler<long, ogetKontoDetailDto>(syskonto);
            return ew.process(delegate(long input, ogetKontoDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.Konto = bo.getKontoDetails(input);
            });
        }

        /// <summary>
        /// delivers Camp detail
        /// </summary>
        /// <param name="syscamp"></param>
        /// <returns></returns>
        public ogetCampDetailDto getCampDetail(long syscamp)
        {
            ServiceHandler<long, ogetCampDetailDto> ew = new ServiceHandler<long, ogetCampDetailDto>(syscamp);
            return ew.process(delegate(long input, ogetCampDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.Camp = bo.getCampDetails(input);
            });
        }

        /// <summary>
        /// delivers Adresse detail
        /// </summary>
        /// <param name="sysadresse"></param>
        /// <returns></returns>
        public ogetFinanzierungDetailDto getFinanzierungDetail(long sysnkk)
        {
            ServiceHandler<long, ogetFinanzierungDetailDto> ew = new ServiceHandler<long, ogetFinanzierungDetailDto>(sysnkk);
            return ew.process(delegate(long input, ogetFinanzierungDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.finanzierung = bo.getFinanzierungDetails(input);
            });
        }

        public oKreditlinieDto getKreditlinieDetail(long sysklinie)
        {
            ServiceHandler<long, oKreditlinieDto> ew = new ServiceHandler<long, oKreditlinieDto>(sysklinie);
            return ew.process(delegate(long input, oKreditlinieDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.kreditlinieDto = bo.getKreditlinieDetail(input);
            });
        }


        /// <summary>
        /// delivers Adresse detail
        /// </summary>
        /// <param name="sysadresse"></param>
        /// <returns></returns>
        public ogetAdresseDetailDto getAdresseDetail(long sysadresse)
        {
            ServiceHandler<long, ogetAdresseDetailDto> ew = new ServiceHandler<long, ogetAdresseDetailDto>(sysadresse);
            return ew.process(delegate(long input, ogetAdresseDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.Adresse = bo.getAdresseDetails(input);
            });
        }

        /// <summary>
        /// delivers Itadresse detail
        /// </summary>
        /// <param name="sysitadresse"></param>
        /// <returns></returns>
        public ogetItadresseDetailDto getItadresseDetail(long sysitadresse)
        {
            ServiceHandler<long, ogetItadresseDetailDto> ew = new ServiceHandler<long, ogetItadresseDetailDto>(sysitadresse);
            return ew.process(delegate(long input, ogetItadresseDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.Itadresse = bo.getItadresseDetails(input);
            });
        }

        /// <summary>
        /// delivers Ptask detail
        /// </summary>
        /// <param name="sysptask"></param>
        /// <returns></returns>
        public ogetPtaskDetailDto getPtaskDetail(long sysptask)
        {
            ServiceHandler<long, ogetPtaskDetailDto> ew = new ServiceHandler<long, ogetPtaskDetailDto>(sysptask);
            return ew.process(delegate(long input, ogetPtaskDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.ptask = bo.getPtaskDetails(input);
            });
        }


        /// <summary>
        /// delivers getApptmt detail
        /// </summary>
        /// <param name="sysapptmt"></param>
        /// <returns></returns>
        public ogetApptmtDetailDto getApptmtDetail(long sysapptmt)
        {
            ServiceHandler<long, ogetApptmtDetailDto> ew = new ServiceHandler<long, ogetApptmtDetailDto>(sysapptmt);
            return ew.process(delegate(long input, ogetApptmtDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.apptmt = bo.getApptmtDetails(input);
            });
        }

        /// <summary>
        /// delivers Reminder detail
        /// </summary>
        /// <param name="sysreminder"></param>
        /// <returns></returns>
        public ogetReminderDetailDto getReminderDetail(long sysreminder)
        {
            ServiceHandler<long, ogetReminderDetailDto> ew = new ServiceHandler<long, ogetReminderDetailDto>(sysreminder);
            return ew.process(delegate(long input, ogetReminderDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.reminder = bo.getReminderDetails(input);
            });
        }

        /// <summary>
        /// delivers Mailmsg detail
        /// </summary>
        /// <param name="sysmailmsg"></param>
        /// <returns></returns>
        public ogetMailmsgDetailDto getMailmsgDetail(long sysmailmsg)
        {
            ServiceHandler<long, ogetMailmsgDetailDto> ew = new ServiceHandler<long, ogetMailmsgDetailDto>(sysmailmsg);
            return ew.process(delegate(long input, ogetMailmsgDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.mailmsg = bo.getMailmsgDetails(input);
            });
        }

        /// <summary>
        /// delivers Memo detail
        /// </summary>
        /// <param name="sysmailmsg"></param>
        /// <returns></returns>
        public ogetMemoDetailDto getMemoDetail(long syswfmmemo)
        {
            ServiceHandler<long, ogetMemoDetailDto> ew = new ServiceHandler<long, ogetMemoDetailDto>(syswfmmemo);
            return ew.process(delegate(long input, ogetMemoDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.memo = bo.getMemoDetails(input);
            });
        }

        /// <summary>
        /// delivers Prun detail
        /// </summary>
        /// <param name="sysprun"></param>
        /// <returns></returns>
        public ogetPrunDetailDto getPrunDetail(long sysprun)
        {
            ServiceHandler<long, ogetPrunDetailDto> ew = new ServiceHandler<long, ogetPrunDetailDto>(sysprun);
            return ew.process(delegate(long input, ogetPrunDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.prun = bo.getPrunDetails(input);
            });
        }

        /// <summary>
        /// delivers Fileatt detail
        /// </summary>
        /// <param name="sysfileatt"></param>
        /// <returns></returns>
        public ogetFileattDetailDto getFileattDetail(long sysfileatt)
        {
            ServiceHandler<long, ogetFileattDetailDto> ew = new ServiceHandler<long, ogetFileattDetailDto>(sysfileatt);
            return ew.process(delegate(long input, ogetFileattDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.fileatt = bo.getFileattDetails(input);
            });
        }

        /// <summary>
        /// delivers Fileatt detail
        /// </summary>
        /// <param name="sysfileatt"></param>
        /// <returns></returns>
        public ogetFileattDetailDto getFileattEntity(string area,long sysid)
        {
            ServiceHandler<String, ogetFileattDetailDto> ew = new ServiceHandler<String, ogetFileattDetailDto>(area);
            return ew.process(delegate(String input, ogetFileattDetailDto rval, CredentialContext ctx)
            {
                if (input == null)
                    throw new ArgumentException("No search input");


                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.fileatt = bo.getFileattDetails(input, sysid);
                
            });

          
        }

        /// <summary>
        /// delivers Dmsdoc detail
        /// </summary>
        /// <param name="sysdmsdoc"></param>
        /// <returns></returns>
        public ogetDmsdocDetailDto getDmsdocDetail(long sysdmsdoc)
        {
            ServiceHandler<long, ogetDmsdocDetailDto> ew = new ServiceHandler<long, ogetDmsdocDetailDto>(sysdmsdoc);
            return ew.process(delegate(long input, ogetDmsdocDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.dmsdoc = bo.getDmsdocDetails(input);
            });
        }

        /// <summary>
        /// delivers Dmsdoc detail
        /// </summary>
        /// <param name="sysdmsdoc"></param>
        /// <returns></returns>
        public ogetDmsdocEntityDto getDmsdocEntity(string area, long sysid)
        {
            ServiceHandler<String, ogetDmsdocEntityDto> ew = new ServiceHandler<String, ogetDmsdocEntityDto>(area);
            return ew.process(delegate(String input, ogetDmsdocEntityDto rval, CredentialContext ctx)
            {
                if (input == null)
                    throw new ArgumentException("No search input");


                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.dmsdocs = bo.getDmsdocDetails(input, sysid);

            });


        }

        /// <summary>
        /// delivers Prproduct detail
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <returns></returns>
        public ogetPrproductDetailDto getPrproductDetail(long sysprproduct)
        {
            ServiceHandler<long, ogetPrproductDetailDto> ew = new ServiceHandler<long, ogetPrproductDetailDto>(sysprproduct);
            return ew.process(delegate(long input, ogetPrproductDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.prproduct = bo.getPrproductDetails(input);
            });
        }

        /// <summary>
        /// delivers Itemcat detail
        /// </summary>
        /// <param name="sysitemcat"></param>
        /// <returns></returns>
        public ogetItemcatDetailDto getItemcatDetail(long sysitemcat)
        {
            ServiceHandler<long, ogetItemcatDetailDto> ew = new ServiceHandler<long, ogetItemcatDetailDto>(sysitemcat);
            return ew.process(delegate(long input, ogetItemcatDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.itemcat = bo.getItemcatDetails(input);
            });
        }

        /// <summary>
        /// delivers Ctlang detail
        /// </summary>
        /// <param name="sysctlang"></param>
        /// <returns></returns>
        public ogetCtlangDetailDto getCtlangDetail(long sysctlang)
        {
            ServiceHandler<long, ogetCtlangDetailDto> ew = new ServiceHandler<long, ogetCtlangDetailDto>(sysctlang);
            return ew.process(delegate(long input, ogetCtlangDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.ctlang = bo.getCtlangDetails(input);
            });
        }

        /// <summary>
        /// delivers Land detail
        /// </summary>
        /// <param name="sysland"></param>
        /// <returns></returns>
        public ogetLandDetailDto getLandDetail(long sysland)
        {
            ServiceHandler<long, ogetLandDetailDto> ew = new ServiceHandler<long, ogetLandDetailDto>(sysland);
            return ew.process(delegate(long input, ogetLandDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.land = bo.getLandDetails(input);
            });
        }

        /// <summary>
        /// delivers Branche detail
        /// </summary>
        /// <param name="sysbranche"></param>
        /// <returns></returns>
        public ogetBrancheDetailDto getBrancheDetail(long sysbranche)
        {
            ServiceHandler<long, ogetBrancheDetailDto> ew = new ServiceHandler<long, ogetBrancheDetailDto>(sysbranche);
            return ew.process(delegate(long input, ogetBrancheDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.branche = bo.getBrancheDetails(input);
            });
        }


        /// <summary>
        /// delivers Account detail
        /// </summary>
        /// <param name="sysaccount"></param>
        /// <returns></returns>
        public ogetAccountDetailDto getAccountDetail(long sysaccount)
        {
            ServiceHandler<long, ogetAccountDetailDto> ew = new ServiceHandler<long, ogetAccountDetailDto>(sysaccount);
            return ew.process(delegate(long input, ogetAccountDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
				rval.account = bo.getAccountDetails (input);
            });
        }

        public ogetFinanzDatenDto getFinanzdatenByArea(long syskd, string area, long sysid)
        {
            ServiceHandler<long[], ogetFinanzDatenDto> ew = new ServiceHandler<long[], ogetFinanzDatenDto>(null);
            return ew.process(delegate(long[] input, ogetFinanzDatenDto rval, CredentialContext ctx) {
                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());

                ogetFinanzDatenDto result = bo.getFinanzdatenByArea(syskd, area, sysid);
				if (result != null)
				{
					rval.budget1 = result.budget1;
					rval.saldo = result.saldo;
				}
                
            });

        }

        /// <summary>
        /// delivers additional customer data for this request
        /// </summary>
        /// <param name="syskd">customer</param>
        /// <param name="sysantrag">request</param>
        /// <returns>additional customer data</returns>
        public ogetZusatzdaten getZusatzdatenDetail(long syskd, long sysantrag)
        {
            long[] args = new long[2] { syskd, sysantrag };

            ServiceHandler<long[], ogetZusatzdaten> ew = new ServiceHandler<long[], ogetZusatzdaten>(args);
            return ew.process(delegate(long[] input, ogetZusatzdaten rval, CredentialContext ctx)
            {
                if (input == null || input.Length < 2 || input[0] == 0 || input[1] == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());

                ogetZusatzdaten result = bo.getZusatzdatenDetail(input[0], input[1]);
				if (result != null)
				{ 
					rval.pkz = result.pkz;
					rval.ukz = result.ukz;
				}
            });
        }

        /// <summary>
        /// delivers the pkz or ukz for the it or person 
        /// optionally for the subarea like angebot/antrag and its id
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysid"></param>
        /// <param name="subarea"></param>
        /// <param name="subsysid"></param>
        /// <returns></returns>
        public ogetZusatzdaten getZusatzdatenDetailByArea(String area, long sysid, String subarea, long subsysid)
        {
           
            ServiceHandler<long[], ogetZusatzdaten> ew = new ServiceHandler<long[], ogetZusatzdaten>(null);
            return ew.process(delegate(long[] input, ogetZusatzdaten rval, CredentialContext ctx)
            {
                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());

                ogetZusatzdaten result = bo.getZusatzdatenDetail(area,sysid,subarea,subsysid);
				if (result != null)
				{
					rval.pkz = result.pkz;
					rval.ukz = result.ukz;
				}
            });
        }

        /// <summary>
        /// delivers additional customer data for this request
        /// </summary>
        /// <param name="syskd">customer</param>
        /// <param name="sysantrag">request</param>
        /// <returns>additional customer data</returns>
        public ogetZusatzdaten getZusatzdatenDetailByAngebot(long sysit, long sysangebot)
        {
            long[] args = new long[2] { sysit, sysangebot };

            ServiceHandler<long[], ogetZusatzdaten> ew = new ServiceHandler<long[], ogetZusatzdaten>(args);
            return ew.process(delegate(long[] input, ogetZusatzdaten rval, CredentialContext ctx)
            {
                if (input == null || input.Length < 2 || input[0] == 0 || input[1] == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());

                ogetZusatzdaten result = bo.getZusatzdatenDetailByAngebot(input[0], input[1]);
				if (result != null)
				{
					rval.pkz = result.pkz;
					rval.ukz = result.ukz;
				}
            });
        }

        /// <summary>
        /// delivers Account detail
        /// </summary>
        /// <param name="sysaccount"></param>
        /// <returns></returns>
        public ogetAccountDetailDto getAccountDetailArea(String area, long sysaccount)
        {
            object[] ip = new object[2] { area, sysaccount };

            ServiceHandler<object[], ogetAccountDetailDto> ew = new ServiceHandler<object[], ogetAccountDetailDto>(ip);
            return ew.process(delegate(object[] input, ogetAccountDetailDto rval, CredentialContext ctx)
            {
                if (input[0]==null)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
				rval.account = bo.getAccountDetails ((String) input[0], (long) input[1]);
            });
        }

        /// <summary>
        /// delivers Vorgang detail
        /// </summary>
        /// <param name="sysoppo"></param>
        /// <returns></returns>
        public ogetVorgangDetailDto getVorgangDetailArea(String area, long sysId)
        {
            object[] vorgang = new object[2] { sysId, area };

            ServiceHandler<object[], ogetVorgangDetailDto> ew = new ServiceHandler<object[], ogetVorgangDetailDto>(vorgang);
            return ew.process(delegate(object[] input, ogetVorgangDetailDto rval, CredentialContext ctx)
            {
                if (((long)vorgang[0] == 0) || (vorgang[1].ToString().Length == 0))
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());

                long input_sys = (long)input[0];
                string intput_area = input[1].ToString();
				rval.vorgang = bo.getVorgangDetails (input_sys, intput_area);
            });
        }

        /// <summary>
        /// delivers WktAccount detail
        /// </summary>
        /// <param name="sysaccount"></param>
        /// <returns></returns>
        public ogetWktAccountDetailDto getWktaccountDetail(long syswkt)
        {
            ServiceHandler<long, ogetWktAccountDetailDto> ew = new ServiceHandler<long, ogetWktAccountDetailDto>(syswkt);
            return ew.process(delegate(long input, ogetWktAccountDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());

                rval.wktaccount = bo.getWktAccountDetails(input);

            });
        }

        /// <summary>
        /// delivers Partner detail
        /// </summary>
        /// <param name="syspartner"></param>
        /// <returns></returns>
        public ogetPartnerDetailDto getPartnerDetail(long syspartner)
        {
            ServiceHandler<long, ogetPartnerDetailDto> ew = new ServiceHandler<long, ogetPartnerDetailDto>(syspartner);
            return ew.process(delegate(long input, ogetPartnerDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.partner = bo.getPartnerDetails(input);
            });
        }

        /// <summary>
        /// delivers Beteiligter detail
        /// </summary>
        /// <param name="syscrmnm"></param>
        /// <returns></returns>
        public ogetBeteiligterDetailDto getBeteiligterDetail(long syscrmnm)
        {
            ServiceHandler<long, ogetBeteiligterDetailDto> ew = new ServiceHandler<long, ogetBeteiligterDetailDto>(syscrmnm);
            return ew.process(delegate(long input, ogetBeteiligterDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.beteiligter = bo.getBeteiligterDetails(input);
            });
        }

        /// <summary>
        /// delivers Adrtp detail
        /// </summary>
        /// <param name="sysadrtp"></param>
        /// <returns></returns>
        public ogetAdrtpDetailDto getAdrtpDetail(long sysadrtp)
        {
            ServiceHandler<long, ogetAdrtpDetailDto> ew = new ServiceHandler<long, ogetAdrtpDetailDto>(sysadrtp);
            return ew.process(delegate(long input, ogetAdrtpDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.adrtp = bo.getAdrtpDetails(input);
            });
        }

        /// <summary>
        /// delivers Kontotp detail
        /// </summary>
        /// <param name="sysKontotp"></param>
        /// <returns></returns>
        public ogetKontotpDetailDto getKontotpDetail(long sysKontotp)
        {
            ServiceHandler<long, ogetKontotpDetailDto> ew = new ServiceHandler<long, ogetKontotpDetailDto>(sysKontotp);
            return ew.process(delegate(long input, ogetKontotpDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.kontotp = bo.getKontotpDetails(input);
            });
        }

        /// <summary>
        /// delivers Blz detail
        /// </summary>
        /// <param name="sysblz"></param>
        /// <returns></returns>
        public ogetBlzDetailDto getBlzDetail(long sysblz)
        {
            ServiceHandler<long, ogetBlzDetailDto> ew = new ServiceHandler<long, ogetBlzDetailDto>(sysblz);
            return ew.process(delegate(long input, ogetBlzDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.blz = bo.getBlzDetails(input);
            });
        }

        /// <summary>
        /// delivers Ptrelate detail
        /// </summary>
        /// <param name="sysptrelate"></param>
        /// <returns></returns>
        public ogetPtrelateDetailDto getPtrelateDetail(long sysptrelate)
        {
            ServiceHandler<long, ogetPtrelateDetailDto> ew = new ServiceHandler<long, ogetPtrelateDetailDto>(sysptrelate);
            return ew.process(delegate(long input, ogetPtrelateDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.ptrelate = bo.getPtrelateDetails(input);
            });
        }

        /// <summary>
        /// delivers Wfexec detail
        /// </summary>
        /// <param name="sysptrelate"></param>
        /// <returns></returns>
        public ogetWfexecDetailDto getWfexecDetail(long syswfexec)
        {
            ServiceHandler<long, ogetWfexecDetailDto> ew = new ServiceHandler<long, ogetWfexecDetailDto>(syswfexec);
            return ew.process(delegate(long input, ogetWfexecDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.wfexec = bo.getWfexecDetails(input);
            });
        }

        /// <summary>
        /// delivers Crmnm detail
        /// </summary>
        /// <param name="syscrmnm"></param>
        /// <returns></returns>
        public ogetCrmnmDetailDto getCrmnmDetail(long syscrmnm)
        {
            ServiceHandler<long, ogetCrmnmDetailDto> ew = new ServiceHandler<long, ogetCrmnmDetailDto>(syscrmnm);
            return ew.process(delegate(long input, ogetCrmnmDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.crmnm = bo.getCrmnmDetails(input);
            });
        }

        /// <summary>
        /// delivers Ddlkprub / Rubriken detail
        /// </summary>
        /// <param name="sysddlkprub></param>
        /// <returns></returns>
        public ogetDdlkprubDetailDto getDdlkprubDetail(long sysddlkprub)
        {
            ServiceHandler<long, ogetDdlkprubDetailDto> ew = new ServiceHandler<long, ogetDdlkprubDetailDto>(sysddlkprub);
            return ew.process(delegate(long input, ogetDdlkprubDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.ddlkprub = bo.getDdlkprubDetails(input);
            });
        }



        /// <summary>
        /// delivers Ddlkpcol / Wertebereiche detail
        /// </summary>
        /// <param name="sysddlkpcol"></param>
        /// <returns></returns>
        public ogetDdlkpcolDetailDto getDdlkpcolDetail(long sysddlkpcol)
        {
            ServiceHandler<long, ogetDdlkpcolDetailDto> ew = new ServiceHandler<long, ogetDdlkpcolDetailDto>(sysddlkpcol);
            return ew.process(delegate(long input, ogetDdlkpcolDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.ddlkpcol = bo.getDdlkpcolDetails(input);
            });
        }

        /// <summary>
        /// delivers Ddlkppos / Werte detail
        /// </summary>
        /// <param name="sysddlkppos"></param>
        /// <returns></returns>
        public ogetDdlkpposDetailDto getDdlkpposDetail(long sysddlkppos)
        {
            ServiceHandler<long, ogetDdlkpposDetailDto> ew = new ServiceHandler<long, ogetDdlkpposDetailDto>(sysddlkppos);
            return ew.process(delegate(long input, ogetDdlkpposDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.ddlkppos = bo.getDdlkpposDetails(input);
            });
        }

        /// <summary>
        /// delivers Ddlkpspos / Werte detail
        /// </summary>
        /// <param name="sysddlkpspos"></param>
        /// <returns></returns>
        public ogetDdlkpsposDetailDto getDdlkpsposDetail(long sysddlkpspos)
        {
            ServiceHandler<long, ogetDdlkpsposDetailDto> ew = new ServiceHandler<long, ogetDdlkpsposDetailDto>(sysddlkpspos);
            return ew.process(delegate(long input, ogetDdlkpsposDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.ddlkpspos = bo.getDdlkpsposDetails(input);
            });
        }

        /// <summary>
        /// delivers Camptp / Werte detail
        /// </summary>
        /// <param name="sysCamptp"></param>
        /// <returns></returns>
        public ogetCamptpDetailDto getCamptpDetail(long sysCamptp)
        {
            ServiceHandler<long, ogetCamptpDetailDto> ew = new ServiceHandler<long, ogetCamptpDetailDto>(sysCamptp);
            return ew.process(delegate(long input, ogetCamptpDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.camptp = bo.getCamptpDetails(input);
            });
        }

        /// <summary>
        /// delivers Oppotp / Werte detail
        /// </summary>
        /// <param name="sysOppotp"></param>
        /// <returns></returns>
        public ogetOppotpDetailDto getOppotpDetail(long sysOppotp)
        {
            ServiceHandler<long, ogetOppotpDetailDto> ew = new ServiceHandler<long, ogetOppotpDetailDto>(sysOppotp);
            return ew.process(delegate(long input, ogetOppotpDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.Oppotp = bo.getOppotpDetails(input);
            });
        }

        /// <summary>
        /// delivers Crmpr / Werte detail
        /// </summary>
        /// <param name="sysCrmpr"></param>
        /// <returns></returns>
        public ogetCrmprDetailDto getCrmprDetail(long sysCrmpr)
        {
            ServiceHandler<long, ogetCrmprDetailDto> ew = new ServiceHandler<long, ogetCrmprDetailDto>(sysCrmpr);
            return ew.process(delegate(long input, ogetCrmprDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.crmpr = bo.getCrmprDetails(input);
            });
        }

        /// <summary>
        /// delivers Contacttp / Werte detail
        /// </summary>
        /// <param name="sysContacttp"></param>
        /// <returns></returns>
        public ogetContacttpDetailDto getContacttpDetail(long sysContacttp)
        {
            ServiceHandler<long, ogetContacttpDetailDto> ew = new ServiceHandler<long, ogetContacttpDetailDto>(sysContacttp);
            return ew.process(delegate(long input, ogetContacttpDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.contacttp = bo.getContacttpDetails(input);
            });
        }

        /// <summary>
        /// delivers Itemcatm / Werte detail
        /// </summary>
        /// <param name="sysItemcatm"></param>
        /// <returns></returns>
        public ogetItemcatmDetailDto getItemcatmDetail(long sysItemcatm)
        {
            ServiceHandler<long, ogetItemcatmDetailDto> ew = new ServiceHandler<long, ogetItemcatmDetailDto>(sysItemcatm);
            return ew.process(delegate(long input, ogetItemcatmDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.itemcatm = bo.getItemcatmDetails(input);
            });
        }



        /// <summary>
        /// delivers Recurr
        /// </summary>
        /// <param name="sysRecurr"></param>
        /// <returns></returns>
        public ogetRecurrDetailDto getRecurrDetail(long sysRecurr)
        {
            ServiceHandler<long, ogetRecurrDetailDto> ew = new ServiceHandler<long, ogetRecurrDetailDto>(sysRecurr);
            return ew.process(delegate(long input, ogetRecurrDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.recurr = bo.getRecurrDetails(input);
            });
        }

        /// <summary>
        /// delivers Ptype 
        /// </summary>
        /// <param name="sysPtype"></param>
        /// <returns></returns>
        public ogetPtypeDetailDto getPtypeDetail(long sysPtype)
        {
            ServiceHandler<long, ogetPtypeDetailDto> ew = new ServiceHandler<long, ogetPtypeDetailDto>(sysPtype);
            return ew.process(delegate(long input, ogetPtypeDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.ptype = bo.getPtypeDetails(input);
            });
        }

        /// <summary>
        /// delivers Prunstep 
        /// </summary>
        /// <param name="sysPrunstep"></param>
        /// <returns></returns>
        public ogetPrunstepDetailDto getPrunstepDetail(long sysPrunstep)
        {
            ServiceHandler<long, ogetPrunstepDetailDto> ew = new ServiceHandler<long, ogetPrunstepDetailDto>(sysPrunstep);
            return ew.process(delegate(long input, ogetPrunstepDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.prunstep = bo.getPrunstepDetails(input);
            });
        }

        /// <summary>
        /// delivers Pstep / Werte detail
        /// </summary>
        /// <param name="sysPstep"></param>
        /// <returns></returns>
        public ogetPstepDetailDto getPstepDetail(long sysPstep)
        {
            ServiceHandler<long, ogetPstepDetailDto> ew = new ServiceHandler<long, ogetPstepDetailDto>(sysPstep);
            return ew.process(delegate(long input, ogetPstepDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.pstep = bo.getPstepDetails(input);
            });
        }


        /// <summary>
        /// delivers Prkgroup
        /// </summary>
        /// <param name="sysPrkgroup"></param>
        /// <returns></returns>
        public ogetPrkgroupDetailDto getPrkgroupDetail(long sysPrkgroup)
        {
            ServiceHandler<long, ogetPrkgroupDetailDto> ew = new ServiceHandler<long, ogetPrkgroupDetailDto>(sysPrkgroup);
            return ew.process(delegate(long input, ogetPrkgroupDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.prkgroup = bo.getPrkgroupDetails(input);
            });
        }


        /// <summary>
        /// delivers Prkgroupm
        /// </summary>
        /// <param name="sysPrkgroupm"></param>
        /// <returns></returns>
        public ogetPrkgroupmDetailDto getPrkgroupmDetail(long sysPrkgroupm)
        {
            ServiceHandler<long, ogetPrkgroupmDetailDto> ew = new ServiceHandler<long, ogetPrkgroupmDetailDto>(sysPrkgroupm);
            return ew.process(delegate(long input, ogetPrkgroupmDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.prkgroupm = bo.getPrkgroupmDetails(input);
            });
        }


        /// <summary>
        /// delivers Prkgroupz
        /// </summary>
        /// <param name="sysPrkgroupz"></param>
        /// <returns></returns>
        public ogetPrkgroupzDetailDto getPrkgroupzDetail(long sysPrkgroupz)
        {
            ServiceHandler<long, ogetPrkgroupzDetailDto> ew = new ServiceHandler<long, ogetPrkgroupzDetailDto>(sysPrkgroupz);
            return ew.process(delegate(long input, ogetPrkgroupzDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.prkgroupz = bo.getPrkgroupzDetails(input);
            });
        }


        /// <summary>
        /// delivers Prkgroups
        /// </summary>
        /// <param name="sysPrkgroups"></param>
        /// <returns></returns>
        public ogetPrkgroupsDetailDto getPrkgroupsDetail(long sysPrkgroups)
        {
            ServiceHandler<long, ogetPrkgroupsDetailDto> ew = new ServiceHandler<long, ogetPrkgroupsDetailDto>(sysPrkgroups);
            return ew.process(delegate(long input, ogetPrkgroupsDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.prkgroups = bo.getPrkgroupsDetails(input);
            });
        }


        /// <summary>
        /// delivers Seg
        /// </summary>
        /// <param name="sysSeg"></param>
        /// <returns></returns>
        public ogetSegDetailDto getSegDetail(long sysSeg)
        {
            ServiceHandler<long, ogetSegDetailDto> ew = new ServiceHandler<long, ogetSegDetailDto>(sysSeg);
            return ew.process(delegate(long input, ogetSegDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.seg = bo.getSegDetails(input);
            });
        }


        /// <summary>
        /// delivers Segc
        /// </summary>
        /// <param name="sysSegc"></param>
        /// <returns></returns>
        public ogetSegcDetailDto getSegcDetail(long sysSegc)
        {
            ServiceHandler<long, ogetSegcDetailDto> ew = new ServiceHandler<long, ogetSegcDetailDto>(sysSegc);
            return ew.process(delegate(long input, ogetSegcDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.segc = bo.getSegcDetails(input);
            });
        }


        /// <summary>
        /// Returns all rubrik-data for a certain area (and id, if given)
        /// </summary>
        /// <param name="rubInput"></param>
        /// <returns></returns>
        public ogetRubDto getRubInfo(igetRubDto rubInput)
        {
            ServiceHandler<igetRubDto, ogetRubDto> ew = new ServiceHandler<igetRubDto, ogetRubDto>(rubInput);
            return ew.process(delegate(igetRubDto input, ogetRubDto rval, CredentialContext ctx)
            {
                if (input == null)
                    throw new ArgumentException("No rub input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.rubriken = bo.getRubInfo(input);
            });
        }

        /// <summary>
        /// Returns all exptypes
        /// </summary>
        /// <param name="expTyp"></param>
        /// <returns></returns>
        public ogetExptypDto getExptypDetails(igetExptypDto expTyp)
        {
            ServiceHandler<igetExptypDto, ogetExptypDto> ew = new ServiceHandler<igetExptypDto, ogetExptypDto>(expTyp);
            return ew.process(delegate(igetExptypDto input, ogetExptypDto rval, CredentialContext ctx)
            {
                if (input == null)
                    throw new ArgumentException("No expTyp input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.exptypes = bo.getExptypDetails(input);
            });
        }

        /// <summary>
        /// Returns all indicator detail values
        /// </summary>
        /// <param name="expVal"></param>
        /// <returns></returns>
        public ogetExpdispDto getExpdispDetails(igetExpdispDto expVal)
        {
            ServiceHandler<igetExpdispDto, ogetExpdispDto> ew = new ServiceHandler<igetExpdispDto, ogetExpdispDto>(expVal);
            return ew.process(delegate(igetExpdispDto input, ogetExpdispDto rval, CredentialContext ctx)
            {
                if (input == null)
                    throw new ArgumentException("No expTyp input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.expvalues = bo.getExpdispDetails(input);
            });
        }

		/// <summary>
		/// Returns all SLA details
		/// </summary>
		/// <param name="expVal"></param>
		/// <returns></returns>
		public ogetSlaDto getSlaDetails (igetSlaDto slaVal)
		{
			ServiceHandler<igetSlaDto, ogetSlaDto> ew = new ServiceHandler<igetSlaDto, ogetSlaDto> (slaVal);
			return ew.process (delegate (igetSlaDto input, ogetSlaDto rval, CredentialContext ctx)
			{
				if (input == null)
					throw new ArgumentException ("No slaTyp input");

				MembershipUserValidationInfo user = ctx.getMembershipInfo ();
				IEntityBo bo = BOFactoryFactory.getInstance ().getEntityBO (user);

				input.isoCode = user.ISOLanguageCode;

				rval.sladetails = bo.getSlaDetails (input);

			});
		}


        /// <summary>
        /// delivers Stickynote
        /// </summary>
        /// <param name="stickynote"></param>
        /// <returns></returns>
        public ogetStickynoteDetailDto getStickynoteDetail(long sysStickynote)
        {
            ServiceHandler<long, ogetStickynoteDetailDto> ew = new ServiceHandler<long, ogetStickynoteDetailDto>(sysStickynote);
            return ew.process(delegate(long input, ogetStickynoteDetailDto rval, CredentialContext ctx)
            {


                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.stickynote = bo.getStickynoteDetails(input);
            });



        }

        /// <summary>
        /// delivers Stickytype
        /// </summary>
        /// <param name="stickytype"></param>
        /// <returns></returns>
        public ogetStickytypeDetailDto getStickytypeDetail(long sysStickytype)
        {
            ServiceHandler<long, ogetStickytypeDetailDto> ew = new ServiceHandler<long, ogetStickytypeDetailDto>(sysStickytype);
            return ew.process(delegate(long input, ogetStickytypeDetailDto rval, CredentialContext ctx)
            {


                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.stickytype = bo.getStickytypeDetails(input);
            });
        }




        /// <summary>
        /// delivers Angebot detail
        /// </summary>
        /// <param name="sysoppo"></param>
        /// <returns></returns>
        public ogetAngebotDetailDto getAngebotDetail(long sysangebot)
        {
            ServiceHandler<long, ogetAngebotDetailDto> ew = new ServiceHandler<long, ogetAngebotDetailDto>(sysangebot);
            return ew.process(delegate(long input, ogetAngebotDetailDto rval, CredentialContext ctx)
            {
                if (sysangebot == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.angebot = bo.getAngebotDetails(input);
            });
        }

        /// <summary>
        /// delivers BN Angebot detail
        /// </summary>
        /// <param name="sysoppo"></param>
        /// <returns></returns>
        public ogetBNAngebotDetailDto getBNAngebotDetail(long sysangebot)
        {
            ServiceHandler<long, ogetBNAngebotDetailDto> ew = new ServiceHandler<long, ogetBNAngebotDetailDto>(sysangebot);
            return ew.process(delegate(long input, ogetBNAngebotDetailDto rval, CredentialContext ctx)
            {
                if (sysangebot == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.BNAngebot = bo.getBNAngebotDetails(input);
            });
        }

        /// <summary>
        /// delivers BN Antrag detail
        /// </summary>
        /// <param name="sysoppo"></param>
        /// <returns></returns>
        public ogetBNAntragDetailDto getBNAntragDetail(long sysantrag)
        {
            ServiceHandler<long, ogetBNAntragDetailDto> ew = new ServiceHandler<long, ogetBNAntragDetailDto>(sysantrag);
            return ew.process(delegate(long input, ogetBNAntragDetailDto rval, CredentialContext ctx)
            {
                if (sysantrag == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.BNAntrag = bo.getBNAntragDetails(input);
            });
        }

        /// <summary>
        /// delivers Angob detail
        /// </summary>
        /// <param name="sysangob"></param>
        /// <returns></returns>
        public ogetAngobDetailDto getAngobDetail(long sysangob)
        {
            ServiceHandler<long, ogetAngobDetailDto> ew = new ServiceHandler<long, ogetAngobDetailDto>(sysangob);
            return ew.process(delegate(long input, ogetAngobDetailDto rval, CredentialContext ctx)
            {
                if (sysangob == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.angob = bo.getAngobDetails(input);
            });
        }

        /// <summary>
        /// delivers Ob detail
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ogetObDetailDto getObDetail(long sysob)
        {
            ServiceHandler<long, ogetObDetailDto> ew = new ServiceHandler<long, ogetObDetailDto>(sysob);
            return ew.process(delegate(long input, ogetObDetailDto rval, CredentialContext ctx)
            {
                if (sysob == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.ob = bo.getObDetails(input);
            });
        }

        /// <summary>
        /// delivers Angvar detail
        /// </summary>
        /// <param name="sysoppo"></param>
        /// <returns></returns>
        public ogetAngvarDetailDto getAngvarDetail(long sysangvar)
        {
            ServiceHandler<long, ogetAngvarDetailDto> ew = new ServiceHandler<long, ogetAngvarDetailDto>(sysangvar);
            return ew.process(delegate(long input, ogetAngvarDetailDto rval, CredentialContext ctx)
            {
                if (sysangvar == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.angvar = bo.getAngvarDetails(input);
            });
        }


        /// <summary>
        /// delivers Angkalk detail
        /// </summary>
        /// <param name="sysoppo"></param>
        /// <returns></returns>
        public ogetAngkalkDetailDto getAngkalkDetail(long sysangkalk)
        {
            ServiceHandler<long, ogetAngkalkDetailDto> ew = new ServiceHandler<long, ogetAngkalkDetailDto>(sysangkalk);
            return ew.process(delegate(long input, ogetAngkalkDetailDto rval, CredentialContext ctx)
            {
                if (sysangkalk == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.angkalk = bo.getAngkalkDetails(input);
            });
        }

        /// <summary>
        /// delivers Antkalk detail
        /// </summary>
        /// <param name="sysantkalk"></param>
        /// <returns></returns>
        public ogetAntkalkDetailDto getAntkalkDetail(long sysantkalk)
        {
            ServiceHandler<long, ogetAntkalkDetailDto> ew = new ServiceHandler<long, ogetAntkalkDetailDto>(sysantkalk);
            return ew.process(delegate(long input, ogetAntkalkDetailDto rval, CredentialContext ctx)
            {
                if (sysantkalk == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.antkalk = bo.getAntkalkDetails(input);
            });
        }

        /// <summary>
        /// delivers Antrag detail
        /// </summary>
        /// <param name="sysoppo"></param>
        /// <returns></returns>
        public ogetAntragDetailDto getAntragDetail(long sysantrag)
        {
            ServiceHandler<long, ogetAntragDetailDto> ew = new ServiceHandler<long, ogetAntragDetailDto>(sysantrag);
            return ew.process(delegate(long input, ogetAntragDetailDto rval, CredentialContext ctx)
            {
                if (sysantrag == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.antrag = bo.getAntragDetails(input);
            });
        }



        // <summary>
        /// delivers Vertrag detail
        /// </summary>
        /// <param name="sysoppo"></param>
        /// <returns></returns>
        public ogetVertragDetailDto getVertragDetail(long sysvertrag)
        {
            ServiceHandler<long, ogetVertragDetailDto> ew = new ServiceHandler<long, ogetVertragDetailDto>(sysvertrag);
            return ew.process(delegate(long input, ogetVertragDetailDto rval, CredentialContext ctx)
            {
                if (sysvertrag == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.vertrag = bo.getVertragDetails(input);
            });
        }

        /// <summary>
        /// delivers System
        /// </summary>
        /// <param name="sysoppo"></param>
        /// <returns></returns>
        public ogetSystemDetailDto getSystemDetail(long syssystem)
        {
            ServiceHandler<long, ogetSystemDetailDto> ew = new ServiceHandler<long, ogetSystemDetailDto>(syssystem);
            return ew.process(delegate(long input, ogetSystemDetailDto rval, CredentialContext ctx)
            {

                rval.system = new SystemDto();
                rval.system.syssystem = 1;
            });
        }

        // <summary>
        /// delivers Vorgang detail
        /// </summary>
        /// <param name="sysoppo"></param>
        /// <returns></returns>
        public ogetVorgangDetailDto getVorgangDetail(long sysId, string area)
        {
            object[] vorgang = new object[2] { sysId, area };

            ServiceHandler<object[], ogetVorgangDetailDto> ew = new ServiceHandler<object[], ogetVorgangDetailDto>(vorgang);
            return ew.process(delegate(object[] input, ogetVorgangDetailDto rval, CredentialContext ctx)
            {
                if (((long)vorgang[0] == 0) || (vorgang[1].ToString().Length == 0))
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());

                long input_sys = (long)input[0];
                string intpu_area = input[1].ToString();

                rval.vorgang = bo.getVorgangDetails(input_sys, intpu_area);
            });
        }
        /// <summary>
        /// epic fail, wont work :(
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ogetEntityDto<T> getEntityDetail<T>(String fentity, long fkey)
        {
            ogetEntityDto<T> rval = new ogetEntityDto<T>();
            //create object from fentity....
            rval.entity = (T)System.Convert.ChangeType(new KundeDto(), Type.GetTypeCode(typeof(T)));
            return rval;

        }

        /// <summary>
        /// Returns all Products
        /// </summary>
        /// <param name="expVal"></param>
        /// <returns></returns>
        public ogetProductsDto getProducts(prKontextDto prodCtx)
        {
            ServiceHandler<prKontextDto, ogetProductsDto> ew = new ServiceHandler<prKontextDto, ogetProductsDto>(prodCtx);
            return ew.process(delegate(prKontextDto input, ogetProductsDto rval, CredentialContext ctx)
            {
                if (input == null)
                    throw new ArgumentException("No expTyp input");

                if (prodCtx.sysperole == 0)
                    prodCtx.sysperole = ctx.getMembershipInfo().sysPEROLE;
                if (prodCtx.sysbrand == 0) prodCtx.sysbrand = ctx.getMembershipInfo().sysBRAND;

                IPrismaProductBo bo = BOFactoryFactory.getInstance().getProductBo(ctx.getUserLanguange());
                List<Cic.OpenOne.Common.Model.Prisma.PRPRODUCT> plist = bo.listAvailableProducts(input);
                rval.produkte = bo.listSortedAvailableProducts(plist, input.sysprbildwelt).ToArray();
                
                rval.productData = AutoMapper.Mapper.Map<List<Cic.OpenOne.Common.Model.Prisma.PRPRODUCT>, List<Cic.One.DTO.PrproductDto>>(plist);
                rval.vartData = DAOFactoryFactory.getInstance().getEntityDao().getVarten();

            });
        }

        /// <summary>
        /// Returns RAP Zins for Product and SCORE
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public ogetRAPZinsDto getRAPZins(igetRAPZinsDto inputData)
        {
            ServiceHandler<igetRAPZinsDto, ogetRAPZinsDto> ew = new ServiceHandler<igetRAPZinsDto, ogetRAPZinsDto>(inputData);
            return ew.process(delegate(igetRAPZinsDto input, ogetRAPZinsDto rval, CredentialContext ctx)
            {
                if (input == null)
                    throw new ArgumentException("No expTyp input");

                
                //IPrismaProductBo bo = BOFactoryFactory.getInstance().getProductBo(ctx.getUserLanguange());
                Cic.OpenOne.Common.BO.IZinsBo zinsBo = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createZinsBo(Cic.OpenOne.Common.BO.ZinsBo.CONDITIONS_BANKNOW, ctx.getMembershipInfo().ISOLanguageCode);
                double[] zinsen = zinsBo.getRAPZins(input.sysprproduct, input.kundenScore, input.lz, input.amount, input.prodCtx);
                rval.rapZins = zinsen[0];
                if (zinsen.Length > 0)
                {
                    rval.rapZinsMin = zinsen[1];
                    rval.rapZinsMax = zinsen[2];
                }
            });
        }

        /// <summary>
        /// Returns all Product Parameters
        /// </summary>
        /// <param name="prodCtx"></param>
        /// <returns></returns>
        public ogetProductParameterDto getProductParameters(prKontextDto prodCtx)
        {
            ServiceHandler<prKontextDto, ogetProductParameterDto> ew = new ServiceHandler<prKontextDto, ogetProductParameterDto>(prodCtx);
            return ew.process(delegate(prKontextDto input, ogetProductParameterDto rval, CredentialContext ctx)
            {
                if (input == null)
                    throw new ArgumentException("No expTyp input");

                if (prodCtx.sysperole == 0)
                    prodCtx.sysperole = ctx.getMembershipInfo().sysPEROLE;
                if (prodCtx.sysbrand == 0) prodCtx.sysbrand = ctx.getMembershipInfo().sysBRAND;

                Cic.OpenOne.Common.DAO.IObTypDao obDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao();
                input.sysperole = obDao.getHaendlerByEmployee(input.sysperole);
                IPrismaParameterBo bo = BOFactoryFactory.getInstance().getProductParameterBo();

                List<Cic.OpenOne.Common.DTO.Prisma.ParamDto> prparams = bo.listAvailableParameter(input);

                rval.parameters = Mapper.Map<Cic.OpenOne.Common.DTO.Prisma.ParamDto[], Cic.One.DTO.ParamDto[]>(prparams.ToArray());

            });
        }

        /// <summary>
        /// delivers ZEK Anfrage detail
        /// </summary>
        /// <param name="syszek">primary key</param>
        /// <returns>ZEK info detail</returns>
        public ogetZekDto getZekDetail(long syszek)
        {
            ServiceHandler<long, ogetZekDto> ew = new ServiceHandler<long, ogetZekDto>(syszek);
            return ew.process(delegate(long input, ogetZekDto rval, CredentialContext ctx)
            {
                if (syszek == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.zek = bo.getZek(input);
            });
        }

        public ogetProcessDto getProcessDetail(long sysprocess)
        {
            ServiceHandler<long, ogetProcessDto> s = new ServiceHandler<long, ogetProcessDto>(sysprocess);
            return s.process(delegate(long input, ogetProcessDto rval, CredentialContext ctx)
            {
                if (sysprocess == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.process = bo.getProcess(input);
            });
        }

        /// <summary>
        /// Get Strasse Details
        /// </summary>
        /// <param name="sysstrasse"></param>
        /// <returns></returns>
        public ogetStrasseDto getStrasseDetail(long sysstrasse)
        {
            ServiceHandler<long, ogetStrasseDto> s = new ServiceHandler<long, ogetStrasseDto>(sysstrasse);
            return s.process(delegate(long input, ogetStrasseDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.strasse = bo.getStrasseDetails(input);
            });
        }

        /// <summary>
        /// Get Puser Details
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        public ogetPuserDetailDto getPuserDetail(long syswfuser)
        {
            ServiceHandler<long, ogetPuserDetailDto> s = new ServiceHandler<long, ogetPuserDetailDto>(syswfuser);
            return s.process(delegate(long input, ogetPuserDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.puser = bo.getPuserDetails(input);
            });
        }


        /// <summary>
        /// Get Dokval Details
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public ogetDokvalDetailDto getDokvalDetail(String area, long sysid )
        {
            object[] data = new object[2] { area, sysid };

            ServiceHandler<object[], ogetDokvalDetailDto> s = new ServiceHandler<object[], ogetDokvalDetailDto>(data);

            return s.process(delegate(object[] input, ogetDokvalDetailDto rval, CredentialContext ctx)
            {
                if (input == null)
                    throw new ArgumentException("No input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.dokval = bo.getDokvalDetails((String)input[0],(long)input[1]);
            });
        }

        /// <summary>
        /// Get Rule-Engine-Result Details
        /// </summary>
        /// <returns></returns>
        public ogetRuleSetDetailDto getRuleSetDetail(igetRuleSetDetailDto inp)
        {


            ServiceHandler<igetRuleSetDetailDto, ogetRuleSetDetailDto> s = new ServiceHandler<igetRuleSetDetailDto, ogetRuleSetDetailDto>(inp);

            return s.process(delegate(igetRuleSetDetailDto input, ogetRuleSetDetailDto rval, CredentialContext ctx)
            {
                if (input == null)
                    throw new ArgumentException("No input");

                

                int lvs = 1;
                if (input.variables != null && input.variables.Length > 0)
                    lvs += input.variables.Length;

                CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto[] vars = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto[lvs];
                vars[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto();
                vars[0].LookupVariableName = "SPRACHE";
                vars[0].VariableName = "GUI";
                vars[0].Value = ctx.getMembershipInfo().ISOLanguageCode;
                int i = 1;
                if (input.variables != null && input.variables.Length > 0)
                {
                    foreach (ContextVariableDto cv in input.variables)
                    {
                        CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto lvar = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto();
                        lvar.LookupVariableName = cv.key;
                        lvar.Value = cv.value;
                        lvar.VariableName = cv.group;
                        vars[i++] = lvar;
                    }
                }


                rval.data = BOS.getQueueData(input.sysid,input.area,input.queueName,input.ruleSetName, vars, ctx.getMembershipInfo().sysWFUSER);
                
                
            });
        }

        /// <summary>
        /// Get Checklist Details
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public ogetChklistDetailDto getChklistDetail(igetChecklistDetailDto data)
        {
            ServiceHandler<igetChecklistDetailDto, ogetChklistDetailDto> s = new ServiceHandler<igetChecklistDetailDto, ogetChklistDetailDto>(data);
            return s.process(delegate(igetChecklistDetailDto input, ogetChklistDetailDto rval, CredentialContext ctx)
            {
                if (input == null)
                    throw new ArgumentException("No input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.chklist = bo.getChklistDetails(input);
            });
        }

        /// <summary>
        /// Get Prunart Details
        /// </summary>
        /// <returns></returns>
        public ogetPrunartDetailDto getPrunartDetail(long sysprunart)
        {
            ServiceHandler<long, ogetPrunartDetailDto> s = new ServiceHandler<long, ogetPrunartDetailDto>(sysprunart);
            return s.process(delegate(long input, ogetPrunartDetailDto rval, CredentialContext ctx)
            {
                if (input == null)
                    throw new ArgumentException("No input");

                IEntityBo bo = BOFactoryFactory.getInstance().getEntityBO(ctx.getMembershipInfo());
                rval.prunart = bo.getPrunartDetails(input);
            });
        }

    }
}
