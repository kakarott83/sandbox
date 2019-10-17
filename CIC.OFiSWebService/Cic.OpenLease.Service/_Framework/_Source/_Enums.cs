using System.ComponentModel;
namespace Cic.OpenLease.Service
{
    #region Enumerators
    public enum VehicleTypeConstants:int
    {
        DemonstrationCar = 1,
        CompanyCar = 2,
        BmwEmployeeCar = 3,
        NewCar = 4,
        UsedCar = 5
    }

    public enum OptionTypeConstants : int
    {
        Package=0,
        Option=1,
        OriginalAccessory=2,
        DealerAccessory=3,
        Miscellaneous=4,
        Ueberfuehrung=5,
        Zulassung=6
    }

    public enum OfferTypeConstants:int
    {
        Manual=0,
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

    public enum NumberPlateConstants
    {
        WithNumberPlate = 1,
        WithoutNumberPlate = 2
    }

    

    public enum AngebotZustand
    {
        [Description("")]
        Undefined = 0,
        [Description("Neu")]
        Neu,
        [Description("Kalkuliert")]
        Kalkuliert,
        [Description("Gedruckt")]
        Gedruckt,
        [Description("Einreichen")]
        Einreichen,
        [Description("Eingereicht")]
        Eingereicht,
        [Description("Stornieren")]
        Stornieren,
        [Description("Storniert")]
        Storniert,
        [Description("Wiedereinreichen")]
        Wiedereinreichen,
        [Description("Wiedereingereicht")]
        Wiedereingereicht,
        [Description("Storno Wiedereinreichung")]
        StornoWiedereinreichung,
        //Relevant HCE
        [Description("Genehmigt")]
        Genehmigt,
        //Relevant HCE
        [Description("Abgelehnt")]
        Abgelehnt,
        [Description("Abgelehnt mit Auflagen")]
        AbgelehntMitAuflagen,
        [Description("Abgelaufen")]
        Abgelaufen,
        [Description("Antrag anlegen")]
        Antraganlegen,
        [Description("BONITAETSPRUEFUNG")]
        BONITAETSPRUEFUNG,
        [Description("NeuResubmit")] //only needed in Resubmit temporal state
        NeuResubmit,
        [Description("Zugesagt mit Auflagen")]
        ZugesagtMitAuflagen,
        //Relevant HCE
        [Description("Zusätzl. Infos notwendig")]
        ZusatzinformationBenoetigt,
        //Relevant HCE
        [Description("Genehmigt mit Auflagen")]
        GenehmigtMitAuflagen,
        //Relevant HCE
        [Description("Antragsänderung erfdl.")]
        AntragsaenderungErforderlich,
        //Relevant HCE
        [Description("Widerrufen")]
        Widerrufen,
        //Relevant HCE
        [Description("Genehmigt (automatisch)")]
        GenehmigtAutomatisch,
        [Description("In Bearbeitung")]
        InBearbeitung
            
        
        

    }

    public enum AntragZustand
    {
        [Description("")]
        Undefined = 0,
        
        [Description("WERTBERICHTIGT!")]
        WERTBERICHTIGT,
        
        [Description("STORNO WIEDEREINREICHUNG")]
        STORNOWIEDEREINREICHUNG,
        [Description("BEDINGUNG NICHT ERFÜLLT")]
        BEDINGUNGNICHTERFUELLT,
        [Description("ÜBERGEBEN")]
        UEBERGEBEN,
        [Description("GENEHMIGT")]
        GENEHMIGT,
        [Description("IRRTÜMLICH ANGELEGT")]
        IRRTÜMLICHANGELEGT,
        [Description("ABGELEHNT")]
        ABGELEHNT,
        [Description("NORMALABLAUF")]
        NORMALABLAUF,
        [Description("AVORZEITIGER ABLAUF")]
        VORZEITIGERABLAUF,
        [Description("AKTIVIERT")]
        AKTIVIERT,
       
        
        [Description("ABONITAETSPRUEFUNG")]
        BONITAETSPRUEFUNG,
        [Description("STORNO")]
        STORNO,
        [Description("ABGELEHNT MIT AUFLAGEN")]
        ABGELEHNTMITAUFLAGEN,
        [Description("ANTRAG ANLEGEN")]
        ANTRAGANLEGEN,
        [Description("ABGESCHLOSSEN")]
        ABGESCHLOSSEN,
        [Description("VORPRÜFUNG")]
        VORPRÜFUNG,
        [Description("RISIKOPRÜFUNG")]
        RISIKOPRÜFUNG,
        [Description("NACHBEARBEITUNG")]
        NACHBEARBEITUNG,
        [Description("NEU")]
        NEU

    }

    public enum KundeUnterschrieben
    {
        ja = 1,
        nein = 0
    }

    public enum TradeOnOwnAccount
    {
        eigenerechnung = 1,
        fremderechnung = 0
    }
    #endregion
}