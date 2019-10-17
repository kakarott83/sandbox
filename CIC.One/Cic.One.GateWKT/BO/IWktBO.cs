using System;
using Cic.One.DTO;
namespace Cic.One.GateWKT.BO
{
    public interface IWktBO
    {

        /// <summary>
        /// Searches for Obtypes for the carselector
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        oSearchDto<ObtypDto> searchObtyp(iSearchObtypDto input, String language);

        /// <summary>
        /// Calculates Rate, NovaBM, RW for WKT
        /// Schritte:
        /// 1) novabonusmalus berechnen
        /// 2) bg = bgintern+provision+novabonusmalus
        /// 3) rw-p-berechnung mit sarvprozent-angabe
        /// 4) gesamt-rw-prozent = sap+rwzuabp, ausser crvp>, dann crvp
        /// 5) rate berechnen
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <param name="icalcRate"></param>
        /// <returns></returns>
        void calculateRate(long syswfuser, Cic.One.GateWKT.DTO.icalcRateDto input, Cic.One.DTO.ocalcRateDto rval);

        /// <summary>
        /// fetches the calculationdata
        /// </summary>
        /// <param name="input"></param>
        /// <param name="rval"></param>
        void getCalculationData(igetCalcDataDto input,ogetCalcDataDto rval);

         /// <summary>
        /// calculates all service rates
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <param name="icalcServices"></param>
        /// <param name="rval"></param>
        void calculateServices(long syswfuser, Cic.One.GateWKT.DTO.icalcServicesDto icalcServices, Cic.One.DTO.ocalcServicesDto rval);


        /// <summary>
        /// Fetches all available object infos for the obtyp id
        ///  including additional eurotaxdata if available
        ///  including manual vehicle (fztyp) configured data if available
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <returns></returns>
        Cic.One.DTO.AngobDto getAngobDetailFromObtyp(long sysobtyp);

        /// <summary>
        /// returns all tire and rims information for the vehicle
        /// </summary>
        /// <param name="igetTires"></param>
        /// <returns></returns>
        Cic.OpenLease.ServiceAccess.DdOl.TireInfoDto getTires(Cic.One.GateWKT.DTO.igetTiresDto igetTires);

         /// <summary>
        /// calculates BSI price
        /// </summary>
        /// <param name="input"></param>
        /// <param name="rval"></param>
        void calculateBSI(icalcBSIDto input, ocalcBSIDto rval);

        /// <summary>
        /// calculates insurance price
        /// </summary>
        /// <param name="input"></param>
        /// <param name="rval"></param>
        void calculateVS(icalcVSDto input, ocalcVSDto rval);

        /// <summary>
        /// Validates the equipment for the vehicle
        /// </summary>
        /// <param name="ivalidate"></param>
        EquipmentValidationDto validateEquipment(ivalidateEquipmentDto ivalidate);

        /// <summary>
        /// assigns all search-result contracts to the campaign
        /// </summary>
        /// <param name="syscamp"></param>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        String assignCampaignContracts(long syscamp, iSearchDto iSearch);

         /// <summary>
        /// removes all search-result contracts from the campaign
        /// </summary>
        /// <param name="syscamp"></param>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        String removeCampaignContracts(long syscamp, iSearchDto iSearch);
        
         /// <summary>
        /// sets all campaign opportunities to checked
        /// </summary>
        /// <param name="syscamp"></param>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        String setAllCampaignOpportunitiesChecked(long syscamp, iSearchDto iSearch);

        /// <summary>
        /// uploads all csv campaign results to the opportunities of the campaign
        /// </summary>
        /// <param name="syscamp"></param>
        /// <param name="fileData"></param>
        /// <returns></returns>
        String uploadCampaignResults(long syscamp, FileattDto fileData);
    }
}
