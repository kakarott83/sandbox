// OWNER MK, 06-01-2009
using System;
using System.Collections.Generic;

namespace Cic.OpenLease.ServiceAccess.DdOl
{

    /// <summary>
    /// Dienstvertrag für Datenzugriffsobjekte für Angebote
    /// </summary>
    [System.CLSCompliant(true)]
    [System.ServiceModel.ServiceContract(Name = "ANGEBOTDaoContract", Namespace = "http://cic-software.de/Cic.OpenLease.Service.DdOl")]
    public interface IANGEBOTDao
    {
        #region Methods

        [System.ServiceModel.OperationContract]
        String createHtmlReport(PrintDto printData);

        [System.ServiceModel.OperationContract]
        TireInfoDto deliverTireGUIData(String eurotaxNr, String reifencodeVorne, String reifencodeHinten, String reifencodeVorneSommer, String reifencodeHintenSommer);

        [System.ServiceModel.OperationContract]
        FlowDto[] DeliverGuardeanMessages(long sysAngebot);

        [System.ServiceModel.OperationContract]
        ValidationResultDto[] validateAngebot(long sysangebot, ValidationStatus[] validations);

        [System.ServiceModel.OperationContract]
        List<ValidationResultDto> validateAngebotFields(long sysid);

        [System.ServiceModel.OperationContract]
        AuflagenInfoDto[] DeliverAuflagen(long sysAngebot);

        [System.ServiceModel.OperationContract]
        bool isBrandDealer(String brand);

        [System.ServiceModel.OperationContract]
        ANGEBOTDto CloneAngebot(ANGEBOTDto AngebotDto);

        [System.ServiceModel.OperationContract]
        List<SubventionDto> DeliverSubventionen(Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTDto angebotDto);

        [System.ServiceModel.OperationContract]
        long  saveBinaryData(byte[] data);

        [System.ServiceModel.OperationContract]
        byte[] deliverBinaryData(long SYSWFDADOC);

        [System.ServiceModel.OperationContract]
        SubmitStatus Submit(EinreichungDto einreichungDto);

      /*  [System.ServiceModel.OperationContract]
        void Resubmit(EinreichungDto einreichungDto);*/

        [System.ServiceModel.OperationContract]
        AngebotCancelStatus Cancel(long sysAngebot, string cancelReason);


       

        [System.ServiceModel.OperationContract]
        ApprovalDto DeliverApproval(long sysAngebot);

        [System.ServiceModel.OperationContract]
        BonitaetDto[] DeliverBonitaet(long sysAngebot);

        [System.ServiceModel.OperationContract]
        decimal[] DeliverErsatzfahrzeugFsPrices();

        [System.ServiceModel.OperationContract]
        decimal[] DeliverManagementFeeFsPrices();


        [System.ServiceModel.OperationContract]
        WRRateDto DeliverWRRATE(long sysObTyp, long lz, decimal ll, int variabel, decimal rn, System.DateTime perDate, decimal nachlass, long sysprproduct, int? SPECIALCALCSTATUS);

        [System.ServiceModel.OperationContract]
        MitantragstellerDto SaveMitantragsteller(MitantragstellerDto mitrantragsteller);

        [System.ServiceModel.OperationContract]
        MitantragstellerDto[] DeliverMitantragsteller(long sysAngebot);

        [System.ServiceModel.OperationContract]
        void DeleteMitantragsteller(long sysId);

        [System.ServiceModel.OperationContract]
        TiresAndRimsCalculationDto CalculateTires(TiresAndRimsCalculationDto TiresAndRimsCalculationDto);

        [System.ServiceModel.OperationContract]
        MehrMinderKmDto DeliverMehrMinderKm(decimal listenPreis, decimal sonderausstattung, decimal paketeBrutto, long sysprproduct);

        [System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.MehrMinderKmDto DeliverMehrMinderKmVorvertrag(long sysvorvt);

        [System.ServiceModel.OperationContract]
        VSARTDto[] DeliverInsuranceTree(long sysObTyp, long sysObArt, long sysKdTyp, long sysPrProduct);

        [System.ServiceModel.OperationContract]
        System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.DdOl.VSARTDto> DeliverVSART(long sysObTyp, long sysObArt, long SysKdTyp, long sysPrProduct);

        [System.ServiceModel.OperationContract]
        System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.DdOl.PERSONDto> DeliverVSPERSON(long SysVSART, long sysObTyp, long sysObArt, long SysKdTyp);

        [System.ServiceModel.OperationContract]
        System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.DdOl.VSTYPDto> DeliverVSTYP(long SysVSART, long SysPERSON, long sysObTyp, long sysObArt, long SysKdTyp);

       

        [System.ServiceModel.OperationContract]
        List<InsuranceResultDto> DeliverVS(List<InsuranceParameterDto> insuranceParams);

        [System.ServiceModel.OperationContract]
        ProvisionDto DeliverProvision(ProvisionDto param);

        [System.ServiceModel.OperationContract]
        ProvisionDto DeliverWartungReparaturProvision(ProvisionDto param);

        [System.ServiceModel.OperationContract]
        ProvisionDto DeliverRestschuldProvision(ProvisionDto param);

        [System.ServiceModel.OperationContract]
        ProvisionDto DeliverKaskoProvision(ProvisionDto param);

        [System.ServiceModel.OperationContract]
        ProvisionDto DeliverBearbeitungsgebuehrProvision(ProvisionDto param);

        [System.ServiceModel.OperationContract]
        ProvisionDto DeliverHaftpflichtProvision(ProvisionDto param);

        [System.ServiceModel.OperationContract]
        ProvisionDto DeliverAbschlussProvision(ProvisionDto param);

        /// <summary>
        /// Delivers all provision rates for the given rank
        /// </summary>
        /// <param name="provsteprank"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        PROVTARIFDto[] DeliverTarife(int provsteprank);

        
        /// <summary>
        /// Delivers all provision rates for the given rank and user perole
        /// </summary>
        /// <param name="provsteprank"></param>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        PROVTARIFDto[] DeliverTarifeForRole(int provsteprank, long sysperole);

        /// <summary>
        /// Delivers the abschluss provision tarif list
        /// (Dropdown in finance-gui for selection of abschluss provision tarif)
        /// </summary>
        /// <param name="sysPrProduct"></param>
        /// <param name="sysObtyp"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        PROVTARIFDto[] DeliverAbschlussProvisionsTarife(long sysPrProduct, long sysObtyp);

        /// <summary>
        /// Delivers the abschluss provision tarif list
        /// (Dropdown in finance-gui for selection of abschluss provision tarif)
        /// </summary>
        /// <param name="sysPrProduct"></param>
        /// <param name="sysObtyp"></param>
        /// <param name="sysPerole"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        PROVTARIFDto[] DeliverAbschlussProvisionsTarifeForRole(long sysPrProduct, long sysObtyp, long sysPerole);

        [System.ServiceModel.OperationContract]
        PetrolPriceDto CalculatePetrol(int lz, long ll, double consTot, FuelTypeConstants fuelType, long sysfstyp, decimal nachlass);

        [System.ServiceModel.OperationContract]
        PetrolLieferantDto[] DeliverPetrolLieferanten(FuelTypeConstants fuelType);

        [System.ServiceModel.OperationContract]
        AnAbmeldePriceDto CalculateAnAbmelde(int lz, bool kennzeichenInkl, long sysOBTYP, decimal nachlass);

        [System.ServiceModel.OperationContract]
        ErsatzfahrzeugPriceDto CalculateErsatzfahrzeug(int lz, int countDays, decimal price, decimal nachlass);

        [System.ServiceModel.OperationContract]
        ManagementFeeDto CalculateManagementFee(int lz, decimal price, decimal nachlass, long sysprproduct, long sysobtyp, long? SPECIALCALCSTATUS);

       

        [System.ServiceModel.OperationContract]
        List<FzConfiguration> GetSa3FzConfiguration(string sa3XmlData);

        [System.ServiceModel.OperationContract]
        ANGEBOTDto DeliverAngebotDtoFromSa3FromSchwacke(string xmlData, string schwacke,ANGEBOTDto AngebotDto);

       
        /// <summary>
        /// Creates Angebot-Object from a object-id (used by carconfigurator)
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <param name="AngebotDto"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        ANGEBOTDto DeliverAngebotDtoFromCarConfigurator(long sysobtyp,ANGEBOTDto AngebotDto);

       

       

        
        [System.ServiceModel.OperationContract]
        TechnicalDataDto DeliverTechnicalDataExtendedFromObTyp(long sysObTyp, long sysObArt, TechnicalDataDto TechnicalDataDto);

       

        [System.ServiceModel.OperationContract]
        PurchasePriceDto DeliverPurchasePrice(PurchasePriceDto PurchasePriceDto);

        [System.ServiceModel.OperationContract]
        CalculationDto Calculate(CalculationDto CalculationDto);

        [System.ServiceModel.OperationContract]
        string DeliverEurotaxNr(string okaCode, string optionCode1, string optionCode2);

        


       
       

        [System.ServiceModel.OperationContract]
        ObjectContextDto DeliverObjectContext(string eurotaxNr, bool isFromSa3, bool checkMultifranchise, int contractext);

         /// <summary>
        /// Liefert Hersteller/Fahrzeugtextinformationen
        /// used in AIDA-Gui:
        ///  * fill Kategorie-List (from getObjectArts)
        ///  * after carconfig or when from sa3
        /// </summary>
        /// <param name="sysObTyp"></param>
        /// <param name="isFromSa3"></param>
        /// <param name="checkMultifranchise"></param>
        /// <param name="contractext"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.ObjectContextDto DeliverObjectContextFromObTyp(long sysObTyp, bool isFromSa3, bool checkMultifranchise, int contractext);

        
        

        [System.ServiceModel.OperationContract]
        SearchResult<ANGEBOTShortDto> Search(Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTSearchData angebotSearchData, Cic.OpenLease.ServiceAccess.SearchParameters searchParameters, Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTSortData[] angebotSortData);

        //System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTShortDto> Search(Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTSearchData angebotSearchData, Cic.OpenLease.ServiceAccess.SearchParameters searchParameters, ANGEBOTSortData[] angebotSortData);

        
        /// <summary>
        /// Die Methode liefert, falls vorhanden, ein Datentransferobjekt mit der angegebenen ID.
        /// </summary>
        /// <param name="sysID">ID des Objektes</param>
        /// <returns><see cref="Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTDto"/></returns>
        [System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTDto Deliver(long sysID);

        /// <summary>
        /// Speichert die Daten aus dem Datentransferobjekt in die Datenbank (INSERT oder UPDATE).
        /// Das Gesichtsfeld kann berücksichtigt werden.
        /// </summary>
        /// <param name="angebotDto"><see cref="Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTDto"/></param>
        /// <returns><see cref="Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTDto"/></returns>
        [System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTDto Save(Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTDto angebotDto);

       

        /// <summary>
        /// Die Methode fordert eine neu Sonderkalkulation an
        /// </summary>
        /// <param name="AngebotDto">Angebot der Sonderkalkulation</param>
        /// <returns>gespeichertes Angebot</returns>
        [System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTDto RequestSpecialCalculation(Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTDto AngebotDto);

        /// <summary>
        /// Delivers a list of all vendors in the same or deeper level as the current logged in role
        /// </summary>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        TransferVendorDto[] DeliverTransferVendors();

        /// <summary>
        /// Delivers a list of all vendors in the same or deeper level as the given person
        /// </summary>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        TransferVendorDto[] DeliverTransferVendorsProvision(long sysberataddb);

        /// <summary>
        /// Connects the offer to a different vendor
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="param"></param>
        [System.ServiceModel.OperationContract]
        void TransferOfferToVendor(long sysid, TransferVendorDto param);

        /// <summary>
        /// Die Methode aktiviert die neue Sonderkalkuation 
        /// </summary>
        /// <param name="AngebotDto">Angebot der Sonderkalkulation</param>
        /// <returns>gespeichertes Angebot</returns>
        [System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTDto PerformSpecialCalculation(Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTDto AngebotDto);

        [System.ServiceModel.OperationContract]
        ANGEBOTDto Copy(long sysId);

        [System.ServiceModel.OperationContract]
        ANGEBOTDto CopyForSpecialCalc(long sysId);

        [System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTDto CopyToResubmit(long SYSID);

       

        [System.ServiceModel.OperationContract]
        MitfinanzierteBestandteileDto[] DeliverMitfinanzierteBestandteileProduct(long sysprproduct, long sysobtyp, long sysprkgroup, long syskdtyp, long sysobart, long lz, long ll);

        [System.ServiceModel.OperationContract]
        MitfinanzierteBestandteileDto[] DeliverAdditionalServicesForProduct(long sysprproduct, long sysobtyp, long sysprkgroup, long syskdtyp, long sysobart, long lz, long ll, bool mitfinanziert);

        [System.ServiceModel.OperationContract]
        void AngebotZustandKalkuliert(long sysAngebot);



        [System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.ControlDto ControlProductKdtyp(long sysprproduct, long syskdtyp);

         /// <summary>
        /// Validates the RSDV against quoted rules
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        List<String> validateRSDV(ANGEBOTDto dto);

        /// <summary>
        /// Validates the GAP against quoted rules
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        List<String> validateGAP(ANGEBOTDto dto);

        [System.ServiceModel.OperationContract]
        InsuranceResultDto DeliverVSData(InsuranceParameterDto param);
      /*  [System.ServiceModel.OperationContract]
        ANGEBOTDto DeliverAngebotDtoFromConfigurationManager2(Guid configurationIdentifier);*/
        #endregion

        /// <summary>
        /// Validates a contract before extension
        /// </summary>
        /// <param name="sysvt"></param>
        /// <param name="contractext"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        List<ValidationResultDto> validateVTExtension(long sysvt, int contractext);

        /// <summary>
        /// Creates a new Extension Offer from a Contract
        /// </summary>
        /// <param name="sysvt"></param>
        /// <param name="contractext"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        ANGEBOTDto getAngebotFromVertrag(long sysvt, int contractext);

        /// <summary>
        /// load MA
        /// </summary>
        /// <param name="sysvt"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        MitantragstellerDto[] DeliverMitantragstellerFromVt(long sysvt);

        /// <summary>
        /// Submits an extension offer
        /// </summary>
        /// <param name="einreichungDto"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        SubmitStatus SubmitExtension(EinreichungDto einreichungDto);


         /// <summary>
        /// Returns a descriptor for the GUI to prevalidate/layout the iban depending on country-code
        /// </summary>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        List<IBANInfoDto> getIBANInformation();

        /// <summary>
        /// Validates IBAN
        /// Extracts BLZ from IBAN
        /// Searches Bankname from BLZ
        /// Validates bic to blz
        /// </summary>
        /// <param name="iban"></param>
        /// <param name="bic"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        IBANValidationError checkIBANandBIC(String iban, String bic);

        /// <summary>
        /// Validates the Mandate, returning a error status when mandat was not valid and also creating a new mandate when an invalid mandate was found
        /// </summary>
        /// <param name="sysangebot"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        SubmitStatus validateMandat(long sysangebot);

        /// <summary>
        /// Returns the id of previous contracts for the offer
        /// </summary>
        /// <param name="sysangebot"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        String[] getVorvertraege(long sysangebot);

                /// <summary>
        /// returns true, if the IT has a currently submitted offer or is a sichtyp of another submitted offer
        /// </summary>
        /// <param name="sysit"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        bool hasSubmittedOffer(long sysit);


        /// <summary>
        /// Creates Angebot-Object from a object-id (used by carconfigurator hek)
        /// </summary>
        /// <param name="sysob"></param>
        /// <param name="AngebotDto"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        ANGEBOTDto DeliverAngebotDtoFromCarConfiguratorSysob(long sysob,ANGEBOTDto AngebotDto);
    }
}
