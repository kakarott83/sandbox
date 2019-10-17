using System.ServiceModel;
using Cic.One.DTO;
using Cic.One.GateWKT.DTO;
using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.GateWKT.Contract
{
    /// <summary>
    /// Das Interface IxproSearchService stellt die Methoden für Xpros bereit
    /// </summary>
    [ServiceContract(Name = "IwktService", Namespace = "http://cic-software.de/One")]
    public interface IwktService
    {
       
        /// <summary>
        /// Fetches all Tire/Rim Information for the XPRO
        /// </summary>
        /// <param name="igetTires"></param>
        /// <returns></returns>
        [OperationContract]
        ogetTiresDto getTires(igetTiresDto igetTires);

        /// <summary>
        /// delivers Angob detail
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <returns></returns>
        [OperationContract]
        ogetAngobDetailDto getAngobDetailFromObtyp(long sysobtyp);

        /// <summary>
        /// delivers a list of Obtypen for the Carselection
        /// </summary>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        [OperationContract]
        oSearchObtypDto searchObtyp(iSearchObtypDto iSearch);

         /// <summary>
        /// Calculates Rate, NovaBM, RW for WKT
        /// </summary>
        /// <param name="icalcRate"></param>
        /// <returns></returns>
        [OperationContract]
        ocalcRateDto calculateRate(icalcRateDto icalcRate);

        /// <summary>
        /// calculates all service rates
        /// </summary>
        /// <param name="icalcServices"></param>
        /// <returns></returns>
        [OperationContract]
        ocalcServicesDto calculateServices(icalcServicesDto icalcServices);

        /// <summary>
        /// fetches needed data for calculation
        /// </summary>
        /// <param name="icalcRate"></param>
        /// <returns></returns>
        [OperationContract]
        ogetCalcDataDto getCalculationData(igetCalcDataDto icalcData);

        /// <summary>
        /// calculates BSI package price from vg tab
        /// </summary>
        /// <param name="icalcRate"></param>
        /// <returns></returns>
        [OperationContract]
        ocalcBSIDto calcBSI(icalcBSIDto icalcData);

        /// <summary>
        /// calculates Insurance prices
        /// </summary>
        /// <param name="icalcRate"></param>
        /// <returns></returns>
        [OperationContract]
        ocalcVSDto calcVS(icalcVSDto icalc);

        /// <summary>
        /// Validates the selected Equipment
        /// </summary>
        /// <param name="ivalidate"></param>
        /// <returns></returns>
        [OperationContract]
        ovalidateEquipmentDto validateEquipment(ivalidateEquipmentDto ivalidate);


        /// <summary>
        /// assigns all search-result contracts to the campaign
        /// </summary>
        /// <param name="icamp"></param>
        /// <returns></returns>
        [OperationContract]
        oCampaignDto assignCampaignContracts(iCampaignManagementDto icamp);

        /// <summary>
        /// removes all search-result contracts from the campaign
        /// </summary>
        /// <param name="icamp"></param>
        /// <returns></returns>
        [OperationContract]
        oCampaignDto removeCampaignContracts(iCampaignManagementDto icamp);

        /// <summary>
        /// sets all campaign opportunities to checked
        /// </summary>
        /// <param name="icamp"></param>
        /// <returns></returns>
        [OperationContract]
        oCampaignDto setAllCampaignOpportunitiesChecked(iCampaignManagementDto icamp);

        /// <summary>
        /// uploads all csv campaign results to the opportunities of the campaign
        /// </summary>
        /// <param name="icamp"></param>
        /// <returns></returns>
        [OperationContract]
        oCampaignDto uploadCampaignResults(iCampaignManagementDto icamp);
    }
}