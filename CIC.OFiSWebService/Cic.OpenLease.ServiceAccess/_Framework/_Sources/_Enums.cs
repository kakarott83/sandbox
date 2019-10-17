// OWNER MK, 08-07-2009
namespace Cic.OpenLease.ServiceAccess
{
    public enum RgGebVersion
    {
        Einmalig = 0,
        Mitfinanziert,
        Rahmenvertrag
    }

    /// <summary>
    /// Sorttierreihenfolge fur die [...]SortData Objekte.
    /// MK
    /// </summary>
    /// 
    public enum SortOrderConstants : int
    {
        Asc = 0,
        Desc = 1
    }

    public enum TiresCalcModes
    {
        Mw = 1,
        Ma = 2,
    }

    public enum TiresAndRimsCalcModes
    {
        Variabel = 0,
        FixUnlimitiert = 1,
        FixLimitiert = 2
    }

    public enum RangslValues : int
    {
        Leasing = 0,
        Credit = 1,
    }

    public enum OfferTypeConstants
    {
        Manual,
        Sa3,
        CarConfigurator,
        HEK,
        VAP
    }

    public enum ITTypeConstants
    {
        Manual,
        Sa3,
        VAP
       
    }

    public enum FuelTypeConstants
    {
        Undefined = 0,
        UnleadedPetrolAndEthanol = 14,
        Diesel = 3,
        UnleadedPetrol = 11,
        Petrol = 8,
        Gas = 10,
        Electricity = 4
    }

    public enum FahrzeugArt
    {
        PKW = 0,
        LKW = 1,
        MOTORRAD = 2
    }

    public enum AreaConstants
    {
        It,
        Angebot,
        Antrag,
        Alle,
        AngebotEinreichen,
        Ruecknahmeprotokoll,
        Vertragsuebersicht,
        VT
    }

    

    public enum DocumentStatusConstants
    {
        Pending = 0,
        Working = 1,
        Ready = 2
    }

    public enum EaiHotStatusConstants
    {
        Pending = 0,
        Working = 1,
        Ready = 2
    }

    /// <summary>
    /// Service Ausnahme Fehler Kodierung Enumeration.
    /// Die Service sind wie folgt Kodiert:
    ///     3xxxxx - Service Ausnahmen
    ///     301xxx - Allgemeine Fehler
    ///     302xxx - Membership/Rechte Fehler
    ///     303xxx - Kalkulation Fehler
    ///     304xxx - Prisma Fehler
    ///     305xxx - OpenLease Fehler
    ///     4xxxxx - Method 
    /// </summary>
    public enum ServiceCodes : int
    {
        /// <summary>
        /// Allgemeine Ausnahme.
        /// MK
        /// </summary>
        GeneralGeneric = 301001,
        /// <summary>
        /// MembershipProvider Initialisierung Ausnahme.
        /// MK
        /// </summary>
        GeneralMembershipProvider = 301002,
        /// <summary>
        /// Select Ausnahme.
        /// MK
        /// </summary>
        GeneralSelect = 301003,
        /// <summary>
        /// Save (Update und Insert) Ausnahme.
        /// MK
        /// </summary>
        GeneralSave = 301004,
        /// <summary>
        /// Delete Ausnahme.
        /// MK
        /// </summary>
        GeneralDelete = 301005,
        /// <summary>
        /// Insert Ausnahme.
        /// MK
        /// </summary>
        GeneralInsert = 301006,
        /// <summary>
        /// Update Ausnahme.
        /// MK
        /// </summary>
        GeneralUpdate = 301007,
        /// <summary>
        /// Keine Rechte für editieren.
        /// MK
        /// </summary>
        GeneralNotValid = 301008,
        /// <summary>
        /// Nicht gültig Ausnahme.
        /// JJ
        /// </summary>
        SecurityEditNotAllowed = 302010,
        /// <summary>
        /// Keine Rechte für einfügen.
        /// MK
        /// </summary>
        SecurityInsertNotAllowed = 302011,
        /// <summary>
        /// Keine Rechte für löschen.
        /// MK
        /// </summary>
        SecurityDeleteNotAllowed = 302012,
        /// <summary>
        /// Kein Message Header gefunden
        /// MK
        /// </summary>
        SecurityNoMessageHeader = 302013,
        /// <summary>
        /// Benutzer nicht gültig. Grund unbekannt.
        /// MK
        /// </summary>
        SecurityUserNotValid = 302001,
        /// <summary>
        /// Benutzer Name nicht gültig.
        /// MK
        /// </summary>
        SecurityUserNameNotValid = 302002,
        /// <summary>
        /// Benutzer Kennwort nicht gültig.
        /// MK
        /// </summary>
        SecurityPasswordNotValid = 302003,
        /// <summary>
        /// Benutzer Rolle nicht gültig.
        /// MK
        /// </summary>
        SecurityValidRoleNotFound = 302004,
        /// <summary>
        /// Benutzer WFUSER nicht gültig.
        /// MK
        /// </summary>
        SecurityValidWorkflowUserNotFound = 302005,
        /// <summary>
        /// Benutzer PERSON nicht gültig.
        /// MK
        /// </summary>
        SecurityValidPersonNotFound = 302006,
        /// <summary>
        /// Benutzer unterdrückt.
        /// MK
        /// </summary>
        SecurityUserDisabled = 302007,
        /// <summary>
        /// Keine Rechte für sehen.
        /// MK
        /// </summary>
        SecurityViewNotAllowed = 302008,
        /// <summary>
        /// Keine Rechte für ausführen.
        /// MK
        /// </summary>
        SecurityExecuteNotAllowed = 302009,
        /// <summary>
        /// Valid BRAND not found.
        /// JJ
        /// </summary>
        SecurityValidBrandNotFound = 302010,
        /// <summary>
        /// Prisma Allgemeine Ausnahme.
        /// JJ
        /// </summary>
        PrismaGeneral = 304001,
        /// <summary>
        /// Nicht in gesichtsfeld.
        /// MK
        /// </summary>
        PrismaNotInSightField = 304002,
        /// <summary>
        /// OpenLease Fehler 
        /// MK
        /// </summary>
        GenericOpenLease = 305002,

        AngebotSearchFailed = 400001,
        AngebotSearchCountFailed = 400002,
        AngebotDeliverFailed = 400003,
        AngebotSaveFailed = 400004,
        AngebotCopyFailed = 400005,
        AngebotDeleteFailed = 400006,
        Angebot2AntragFailed = 400007,
        CheckAngebot2AntragFailed = 400008,
        CheckAngebot2AntragsFaield = 400009,
        DeleteMitantragstellerFailed = 400010,
        SaveMitantragstellerFailed = 400011,
        DeliverMitantragstellerFailed = 400012,
        SaveTmpAngebotDtoFailed = 400013,
        DeleteTmpAngebotDtoFailed = 400014,
        DeliverTmpAngebotDtoFailed = 400015,
        DeliverMitfinanzierteBestandteileFailed = 400016,
        DeliverPurchasePriceFailed = 400017,
        BmwCalculateFailed = 400018,
        DeliverGebuhrenFailed = 400019,
        DeliverMehrMinderKmFailed = 400020,
        CalculatePetrolFailed = 400021,
        CalculateAnAbmeldeFailed = 400022,
        DeliverErsatzfahrzeugFsPricesFailed = 400023,
        DeliverManagementFeeFsPricesFailed = 400024,
        DeliverTiresFsPricesFailed = 400025,
        CalculateErsatzfahrzeugFailed = 400026,
        CalculateManagementFeeFailed = 400027,
        DeliverVSARTFailed = 400028,
        DeliverVSPERSONFailed = 400029,
        DeliverVSTYPFailed = 400030,
        DeliverVSDataFailed = 400031,
        DeliverTarifeFailed = 400032,
        DeliverAbschlussProvisionFailed = 400033,
        DeliverHaftpflichtProvisionFailed = 400034,
        DeliverBearbeitungsgebuehrProvisionFailed = 400035,
        DeliverKaskoProvisionFailed = 400036,
        DeliverRestschuldProvisionFailed = 400037,
        DeliverWartungReparaturProvisionFailed = 400038,
        DeliverProvisionFailed = 400039,
        DeliverAngebotDtoFromSa3Failed = 400040,
        DeliverAngebotDtoFromConfigurationManagerFailed = 400041,
        DeliverBmwTechnicalDataFailed = 400042,
        DeliverObjectContextFailed = 400043,
        DeliverEurotaxNrFailed = 400044,
        DeliverEurotaxTiresFailed = 400045,
        DeliverEurotaxRimsFailed = 400046,
        DeliverTiresFailed = 400047,
        DeliverRimsFailed = 400048,
        DeliverTiresPricesFailed = 400049,
        CalculateTiresFailed = 400050,
        DeliverWRRATEFailed = 400051,
        DeliverApprovalFailed = 400052,
        DeliverBonitaetFailed = 400053,
        ItSearchFailed = 400054,
        ItSearchCountFailed = 400055,
        ItDeliverFailed = 400056,
        ItSaveFailed = 400057,
        ItDeleteFailed = 400058,
        AntragDeliverFailed = 400059,
        AntragSearchFailed = 400060,
        AntragSearchCountFailed = 400061,
        OlClientExecuteFailed = 400062,
        DeliverDocumentsListFailed = 400063,
        PrintDocumentFailed = 400064,
        DeliverPrintedDocumentFailed = 400065,
        DeliverPrintedDocumentsFailed = 400066,
        DeliverDictionaryFailed = 400067,
        DeliverLANDDtoListFailed = 400068,
        DeliverAddressLandFailed = 400069,
        DeliverSTAATDtoListFailed = 400070,
        DeliverKDTYPDtoListFailed = 400071,
        DeliverCTLANGDtoListFailed = 400072,
        DeliverBRANCHEDtoListFailed = 400073,
        DeliverPLZDtoListFailed = 400074,
        DeliverBLZDtoListFailed = 400075,
        DeliverBERUFDtoListFailed = 400076,
        SearchORTFailed = 400078,
        SearchBANKNAMEFailed = 400079,
        AngebotSubmitFailed = 400080,
        AngebotResubmitFailed = 400081,
        AngebotCancelFailed = 400082,
        SCalcRequestFailed =400083,
        SCalcPerformedFailed = 400084,
        DeliverTransferVendorsFailed = 400085,
        TransferOfferToVendorFailed = 400086,
        AngebotZustandInit = 400087,
        AngebotZustandKalkuliert = 400088,
        DeliverBinaryDataFailed = 400089,
        SaveBinaryDataFailed = 400090,
        EAIHotSearchFailed = 400091,
        DeliverSubventionenFailed = 400092,
        DeliverITDtoFromSa3Failed = 400093,
        DeliverFzDataFromSa3Failed = 400094,
        SaveAngebotFailedProduct = 400095,
        SelectionProductInAngebot = 400100,
        IsBrandDealer = 400096,
        DeliverVorgaengeDtoFailed = 400097,
        ValidateAngebotSave = 400098,
        DeliverGuardeanMessages = 400099,
        getAngebotFromVertragFailed = 400100,
        validateVTExtensionFailed = 400101,
        getItFromVTPersonFailed = 400102,
        VertragDeliverFailed = 400103
    }

    public enum Rfgs
    {
        Angebot_Web = 0,
        AngebotPrint,
        AngebotChgeVerk,
        AngebotEinreichen,
        AngebotSonderkalk,
        AngebotChgeAnz,
        AngebotChgeVersRate,
        Interessent_Web
    }
}
