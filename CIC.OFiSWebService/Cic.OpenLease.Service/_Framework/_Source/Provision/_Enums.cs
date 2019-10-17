using System.ComponentModel;
namespace Cic.OpenLease.Service.Provision
{
    public enum ProvisionTypeConstants
    {
        Abschluss = 10,
        Zugang = 20,
        WartungReparatur = 30,
        Kasko = 40,
        Restschuld = 50,
        Bearbeitungsgebuehr = 60,
        Haftpflicht = 70,
        GAP = 80
    }

    public enum SubventionTypeConstants
    {
       
       
      
       
      
      
        [Description("SUBVENTION_VERSICHERUNG")]
        Versicherung=1,

        [Description("LEAKALK_RESTSCHULD_NACHLASS")]
        Restschuld=2,

        [Description("LEAKALK_HAFTPFL_NACHLASS")]
        Haftpflicht=3,
        [Description("LEAKALK_KASKO_NACHLASS")]
        Kasko=4,
        [Description("LEAKALK_INSASSEN_NACHLASS")]
        IUV=5,
        [Description("LEAKALK_RECHTSCHUTZ_NACHLASS")]
        Rechtsschutz=6,

        //Implizit
        [Description("LEAKALK_BEARBGEBUEHR_NACHLASS")]
        Gebuehr=7,
        [Description("LEAKALK_ZINSNOMINAL_NACHLASS")] //LEAKALK_RATE")]
        Rate=8,
        [Description("LEAKALK_ZINSEFF_NACHLASS")] //LEAKALK_RATE")]
        RateKredit=9,
        [Description("LEAKALK_RWKALK")]
        Restwert=10,
        [Description("LEAKALK_WARTUNG_NACHLASS")]
        Maintenance=11,
        [Description("LEAKALK_PETROL_NACHLASS")]
        FuelPrice=12,
        [Description("LEAKALK_ANMELDG_NACHLASS")]
        Anabmeldung=13,
        [Description("LEAKALK_ERSATZFZG_NACHLASS")]
        RepCarRate=14,
        [Description("LEAKALK_REIFEN_NACHLASS")]
        TiresPrice=15,
        [Description("LEAKALK_SONSTDL_NACHLASS")]
        Sonstige=16,

        [Description("SUBVENTION_MITFIN")]
        MitFin=17,

        [Description("SUBVENTION_RGG")]
        RgGebuehr=18,
        [Description("SUBVENTION_GAP")]
        GAP = 19,
        [Description("LEAKALK_RWKALKP")]
        RestwertProzent = 20
    }
}