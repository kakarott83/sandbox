using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.One.DTO;
using Cic.One.GateWKT.DTO;
using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenLease.Model.DdOl;
using Cic.One.GateWKT.Contract;
using Cic.One.Web.BO;
using Cic.OpenOne.Common.Util.Security;
using Cic.One.GateWKT.BO;
using Cic.OpenLease.Service;
using Cic.OpenLease.ServiceAccess;
using Cic.OpenOne.Common.Model.DdOw;
using System.Globalization;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO;



namespace Cic.One.GateWKT
{
    [ServiceBehavior(Namespace = "http://cic-software.de/One")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public class wktService : IwktService
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Fetches all Tire/Rim Information for the XPRO
        /// </summary>
        /// <param name="igetTires"></param>
        /// <returns></returns>
        public ogetTiresDto getTires(igetTiresDto igetTires)
        {
            ServiceHandler<igetTiresDto, ogetTiresDto> ew = new ServiceHandler<igetTiresDto, ogetTiresDto>(igetTires);

            return ew.process(delegate(igetTiresDto input, ogetTiresDto rval)
            {

                IWktBO bo = new Cic.One.GateWKT.BO.BOFactory().getWKTBO();
                rval.result = bo.getTires(input);

            });
        }

        /// <summary>
        /// delivers a list of Obtypen for the Carselection
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public oSearchObtypDto searchObtyp(iSearchObtypDto iSearch)
        {
            ServiceHandler<iSearchObtypDto, oSearchObtypDto> ew = new ServiceHandler<iSearchObtypDto, oSearchObtypDto>(iSearch);

            return ew.process(delegate(iSearchObtypDto input, oSearchObtypDto rval, CredentialContext ctx)
            {

                IWktBO bo = new Cic.One.GateWKT.BO.BOFactory().getWKTBO();
                input.sysperole = ctx.getMembershipInfo().sysPEROLE;
                rval.result = bo.searchObtyp(input, ctx.getUserLanguange());

            });
        }

        /// <summary>
        /// delivers Angob detail
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <returns></returns>
        public ogetAngobDetailDto getAngobDetailFromObtyp(long sysobtyp)
        {
            ServiceHandler<long, ogetAngobDetailDto> ew = new ServiceHandler<long, ogetAngobDetailDto>(sysobtyp);
            return ew.process(delegate(long input, ogetAngobDetailDto rval, CredentialContext ctx)
            {
                if (input == 0)
                    throw new ArgumentException("No search input");

                IWktBO bo = new Cic.One.GateWKT.BO.BOFactory().getWKTBO();
                rval.angob = bo.getAngobDetailFromObtyp(sysobtyp);
                
            });
        }

        /// <summary>
        /// calculates all service rates
        /// </summary>
        /// <param name="icalcServices"></param>
        /// <returns></returns>
        public ocalcServicesDto calculateServices(icalcServicesDto icalcServices)
        {
            ServiceHandler<icalcServicesDto, ocalcServicesDto> ew = new ServiceHandler<icalcServicesDto, ocalcServicesDto>(icalcServices);
            return ew.process(delegate(icalcServicesDto input, ocalcServicesDto rval, CredentialContext ctx)
            {
                IWktBO bo = new Cic.One.GateWKT.BO.BOFactory().getWKTBO();
                bo.calculateServices(ctx.getMembershipInfo().sysWFUSER,input, rval);
            });
        }

        /// <summary>
        /// Calculates Rate, NovaBM, RW for WKT
        /// </summary>
        /// <param name="icalcRate"></param>
        /// <returns></returns>
        public ocalcRateDto calculateRate(icalcRateDto icalcRate)
        {
            
             ServiceHandler<icalcRateDto, ocalcRateDto> ew = new ServiceHandler<icalcRateDto, ocalcRateDto>(icalcRate);
             return ew.process(delegate(icalcRateDto input, ocalcRateDto rval, CredentialContext ctx)
             {
                 IWktBO bo = new Cic.One.GateWKT.BO.BOFactory().getWKTBO();
                 bo.calculateRate(ctx.getMembershipInfo().sysWFUSER,input, rval);
               
             });
        }

        /// <summary>
        /// fetches needed data for calculation
        /// </summary>
        /// <param name="icalcRate"></param>
        /// <returns></returns>
        public ogetCalcDataDto getCalculationData(igetCalcDataDto icalcData)
        {

            ServiceHandler<igetCalcDataDto, ogetCalcDataDto> ew = new ServiceHandler<igetCalcDataDto, ogetCalcDataDto>(icalcData);
            return ew.process(delegate(igetCalcDataDto input, ogetCalcDataDto rval, CredentialContext ctx)
            {
                IWktBO bo = new Cic.One.GateWKT.BO.BOFactory().getWKTBO();
                bo.getCalculationData(input, rval);

            });
        }

        /// <summary>
        /// calculates BSI package price from vg tab
        /// </summary>
        /// <param name="icalcRate"></param>
        /// <returns></returns>
        public ocalcBSIDto calcBSI(icalcBSIDto icalcData)
        {

            ServiceHandler<icalcBSIDto, ocalcBSIDto> ew = new ServiceHandler<icalcBSIDto, ocalcBSIDto>(icalcData);
            return ew.process(delegate(icalcBSIDto input, ocalcBSIDto rval, CredentialContext ctx)
            {
                IWktBO bo = new Cic.One.GateWKT.BO.BOFactory().getWKTBO();
                bo.calculateBSI(input, rval);

            });
        }

        /// <summary>
        /// calculates Insurance prices
        /// </summary>
        /// <param name="icalcRate"></param>
        /// <returns></returns>
        public ocalcVSDto calcVS(icalcVSDto icalc)
        {

            ServiceHandler<icalcVSDto, ocalcVSDto> ew = new ServiceHandler<icalcVSDto, ocalcVSDto>(icalc);
            return ew.process(delegate(icalcVSDto input, ocalcVSDto rval, CredentialContext ctx)
            {
                IWktBO bo = new Cic.One.GateWKT.BO.BOFactory().getWKTBO();
                bo.calculateVS(input, rval);

            });
        }

        /// <summary>
        /// Validates the selected Equipment
        /// </summary>
        /// <param name="ivalidate"></param>
        /// <returns></returns>
        public ovalidateEquipmentDto validateEquipment(ivalidateEquipmentDto ivalidate)
        {
            ServiceHandler<ivalidateEquipmentDto, ovalidateEquipmentDto> ew = new ServiceHandler<ivalidateEquipmentDto, ovalidateEquipmentDto>(ivalidate);

            return ew.process(delegate(ivalidateEquipmentDto input, ovalidateEquipmentDto rval)
            {

                IWktBO bo = new Cic.One.GateWKT.BO.BOFactory().getWKTBO();
                rval.result = bo.validateEquipment(input);

            });
        }

        /// <summary>
        /// assigns all search-result contracts to the campaign
        /// </summary>
        /// <param name="iCampaignManagementDto"></param>
        /// <returns></returns>
        public oCampaignDto assignCampaignContracts(iCampaignManagementDto icamp)
        {
            ServiceHandler<iCampaignManagementDto, oCampaignDto> ew = new ServiceHandler<iCampaignManagementDto, oCampaignDto>(icamp);

            return ew.process(delegate(iCampaignManagementDto input, oCampaignDto rval)
            {
                IWktBO bo = new Cic.One.GateWKT.BO.BOFactory().getWKTBO();
                rval.result = bo.assignCampaignContracts(input.syscamp, input.iSearch);

            });

        }

        /// <summary>
        /// removes all search-result contracts from the campaign
        /// </summary>
        /// <param name="iCampaignManagementDto"></param>
        /// <returns></returns>
        public oCampaignDto removeCampaignContracts(iCampaignManagementDto icamp)
        {
            ServiceHandler<iCampaignManagementDto, oCampaignDto> ew = new ServiceHandler<iCampaignManagementDto, oCampaignDto>(icamp);

            return ew.process(delegate(iCampaignManagementDto input, oCampaignDto rval)
            {
                IWktBO bo = new Cic.One.GateWKT.BO.BOFactory().getWKTBO();
                rval.result = bo.removeCampaignContracts(input.syscamp, input.iSearch);

            });
        }

        /// <summary>
        /// sets all campaign opportunities to checked
        /// </summary>
        /// <param name="iCampaignManagementDto"></param>
        /// <returns></returns>
        public oCampaignDto setAllCampaignOpportunitiesChecked(iCampaignManagementDto icamp)
        {
            ServiceHandler<iCampaignManagementDto, oCampaignDto> ew = new ServiceHandler<iCampaignManagementDto, oCampaignDto>(icamp);

            return ew.process(delegate(iCampaignManagementDto input, oCampaignDto rval)
            {
                IWktBO bo = new Cic.One.GateWKT.BO.BOFactory().getWKTBO();
                rval.result = bo.setAllCampaignOpportunitiesChecked(input.syscamp, input.iSearch);

            });
        }

        /// <summary>
        /// uploads all csv campaign results to the opportunities of the campaign
        /// </summary>
        /// <param name="iCampaignManagementDto"></param>
        /// <returns></returns>
        public oCampaignDto uploadCampaignResults(iCampaignManagementDto icamp)
        {
            ServiceHandler<iCampaignManagementDto, oCampaignDto> ew = new ServiceHandler<iCampaignManagementDto, oCampaignDto>(icamp);

            return ew.process(delegate(iCampaignManagementDto input, oCampaignDto rval)
            {
                IWktBO bo = new Cic.One.GateWKT.BO.BOFactory().getWKTBO();
                rval.result = bo.uploadCampaignResults(input.syscamp, input.fileData); 
            });
        }

        //unread-documents query:
        /*select count(*) from CIC.EAIHFILE,CIC.EAIHOT left outer join angebot on (angebot.sysid=eaihot.sysoltable and oltable='ANGEBOT') WHERE CIC.EAIHFILE.SYSEAIHOT=CIC.EAIHOT.SYSEAIHOT  AND EAIHOT.PROZESSSTATUS=2 AND EAIHOT.CODE = 'AIDA_PRINT'  and (FileFlagOut is null or FileFlagOut=0) 
  AND OLTABLE = 'ANGEBOT'  AND (SYSOLTABLE IN (select * from TABLE(cic.CIC_PEROLE_UTILS.GetTabIDFromPEUNI(:sysPuser, 'ANGEBOT',sysdate))) );*/
    }
}
