namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    [System.Serializable]
    public class CalculationDto
    {
        #region Enums
        public enum MessageCodes
        {
            NoError = -1,
            Mvz001 = 0,
            Dpt001 = 1,
            Dpt002 = 2,
            Rt001 = 3,
            Zns001 = 4,
            Rw001 = 5,
            PrProd001 = 6,
            ObTyp001 = 7,
            ObArt001 = 8,
            ObKat001 = 9,
            Lz001 = 10,
            AnschVert001 = 11,
            KalkTyp001 = 12,
            ProvTarif = 13,
            ZnsEff001 = 14,
            Ll001 = 15,
            Rw002 = 16,
            Lz002 = 17
        }

        public enum CalculationSources
        {
            Mietvorauszahlung = 0,
            MietvorauszahlungP,
            Depot,
            DepotP,
            Restwert,
            RestwertP,
            Laufzeit,
            Laufleistung,
            Verzinsungsart,
            ZinsEff,
            ZinsEffRecursion,
            ZinsNominal,
            Rate,
            RGGebuehr,
            Wunschprovision,
            ProductChange,
            RateRecursion
        }
        public enum CalculationTargets
        {
            Mietvorauszahlung = 0,
            Rabatt,
            Restwert
        }

        public enum SubventionCalcModes
        {
            None = 0,
            Zins = 1,
            Restwert = 2,
            RSV = 3,
            RGG = 4
        }

        #endregion
        
        [System.Runtime.Serialization.DataMember]
        public int SubventionCalcMode
        {
            get;
            set;
        }
        
        
        [System.Runtime.Serialization.DataMember]
        public decimal Subvention_Zins
        {
            get;
            set;
        }
        
        public int SubventionCalc
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal Subvention_Restwert
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal Subvention_RGG
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal Subvention_RSV
    {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public bool DontUseCustomResVal
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public CalculationSources CalculationSource
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public MessageCodes MessageCode
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string Message
        {
            get;
            set;
        }
        /// <summary>
        /// Vor Rabatt inkl. nova und zuschlag
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal ListenpreisBrutto
        {
            get;
            set;
        }
        /// <summary>
        /// Vor Rabatt inkl. nova und zuschlag
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal SonzubBrutto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal KaufpreisBrutto
        {
            get;
            set;
        }
        public decimal KaufpreisBruttoOrg
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal KaufpreisUst
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal KaufpreisNetto
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal SARVNETTO
        {
            get;
            set;
        }
        
        
        [System.Runtime.Serialization.DataMember]
        public decimal MitfinanzierterBestandteilBrutto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal MitfinanzierterBestandteilUst
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal MitfinanzierterBestandteilNetto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal MitfinanzierterBestandteil_Default
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal AnschaffungswertBrutto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal AnschaffungswertUst
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal AnschaffungswertNetto
        {
            get;
            set;
        }

        /// <summary>
        /// ANGKALKBGINTERNBRUTTO
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal FinanzierungssummeBrutto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal FinanzierungssummeUst
        {
            get;
            set;
        }

        
        /// <summary>
        /// ANGKALKBGINTERN
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal FinanzierungssummeNetto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal MietvorauszahlungBrutto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal MietvorauszahlungBruttoP
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public decimal MietvorauszahlungP
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal MietvorauszahlungUst
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal MietvorauszahlungNetto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal DepotBrutto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal DepotBruttoP
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal RestwertBrutto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal RestwertBruttoP
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal RestwertUst
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal RestwertNetto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal RestwertvorschlagBrutto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal RestwertvorschlagBruttoP
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal RestwertvorschlagtUst
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal RestwertvorschlagNetto
        {
            get;
            set;
        }
        /// <summary>
        /// Refizins
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal Zinssatz
        {
            get;
            set;
        }

        /// <summary>
        /// ANGKALKRATEBRUTTO
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal MonatlicheRate
        {
            get;
            set;
        }
        //Unrounded
        public decimal MonatlicheRateRaw
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public decimal MonatlicheRateKredit
        {
            get;
            set;
        }

        /// <summary>
        /// Default Rate, aus ANGKALKRATE_DEFAULT
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal MonatlicheRate_Default
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal GesamtKostenBrutto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal GesamtKostenNetto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal GesamtKostenUst
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public int Verzinsungsart
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public int Laufzeit
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal Laufleistung
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal Variante
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string EurotaxNr
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long SysObArt
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long SysObKat
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long SysPrProduct
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long SysObTyp
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long SysPrkGroup
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long SysPrhGroup
        {
            get;
            set;
        }


        
        [System.Runtime.Serialization.DataMember]
        public System.DateTime Erstzulassungsdatum
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public System.DateTime Lieferdatum
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long UbNahmeKm
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal Verrechnung
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal Bearbeitungsgebuehr
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal BearbeitungsgebuehrNachlass
        {
            get;
            set;
        }

        
        
        [System.Runtime.Serialization.DataMember]
        public decimal MehrKMSatz
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal MinderKMSatz
        {
            get;
            set;
        }

        /// <summary>
        /// ANGKALKRATE
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal MonatlicheRateNetto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal MonatlicheRateUst
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal RestschuldVersicherung
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public decimal GAPVersicherung
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public decimal GAPVersicherungDefault
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal RestschuldVersicherungDefault
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long SYSVSTYPRSV
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public long SYSVSTYPGAP
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public long lzgap
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public bool hasRSV
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public bool hasGAP
        {
            get;
            set;
        }


        
        [System.Runtime.Serialization.DataMember]
        public decimal DepotNetto
        {
            get;
            set;
        }

        //Output
        
        [System.Runtime.Serialization.DataMember]
        public decimal RgGebNetto
        {
            get;
            set;
        }

        //Output
        
        [System.Runtime.Serialization.DataMember]
        public decimal RgGebBrutto
        {
            get;
            set;
        }

        //Output
        
        [System.Runtime.Serialization.DataMember]
        public decimal RgGebUst
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public RgGebVersion RgGebVersion
        {
            get;
            set;
        }

        //Output
        
        [System.Runtime.Serialization.DataMember]
        public decimal RgGeb_Default
        {
            get;
            set;
        }
        //Input
        
        [System.Runtime.Serialization.DataMember]
        public bool RgGebFrei
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public bool Kasko
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal KaskoRate
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public bool HP
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal HPRate
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long SysProvTariff
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal Wunschprovision
        {
            get;
            set;
        }
        
        
        [System.Runtime.Serialization.DataMember]
        public decimal RSVProvision
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public decimal GAPProvision
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public AbschlussProvisionDto AbschlussProvision
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public ZugangProvisionDto ZugangProvision
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal SFAufschlag
        {
            get;
            set;
        }

        /// <summary>
        /// Kalkulatorischer Zins
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal Zins
        {
            get;
            set;
        }

        /// <summary>
        /// Effektivzins
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal ZinsEff
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal Restkaufpreis
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal CrvNetto
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal sfBaseNetto
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal CrvBrutto
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal sfBaseBrutto
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal CrvProzent
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal sfBaseProzent
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal CrvUst
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal sfBaseUst
        {
            get;
            set;
        }

        /// <summary>
        /// Nominalzins (als Eingabeparameter)
        /// </summary>
        public decimal ZinsNominal { get; set; }

        
        [System.Runtime.Serialization.DataMember]
        public decimal Kreditbetrag
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal GesamtbelastungBrutto
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal GesamtbelastungNetto
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal GesamtbelastungUst
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public bool isSA3
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public bool isVAP
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public bool isECOM
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public GebuhrenDto Gebuehren
        {
            get;
            set;
        }

        /// <summary>
        /// Vor Rabatt inkl. nova und zuschlag
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal PaketeBrutto
        {
            get;
            set;
        }
        /// <summary>
        /// Nach Rabatt inkl. nova und zuschlag
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal ANGOBPAKETEEXTERNBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Nach Rabatt inkl. nova und zuschlag
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal ANGOBSONZUBEXTERNBRUTTO
        {
            get;
            set;
        }
        /// <summary>
        /// Nach Rabatt inkl. nova und zuschlag
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal ANGOBGRUNDEXTERNBRUTTO
        {
            get;
            set;
        }

        /// <summary>
        /// Netto Netto  Grundpreis
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal ANGOBGRUNDEXKLN
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public int? SPECIALCALCSTATUS
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public bool isIM
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal NovaBonusMalus
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal NovaSatz
        {
            get;
            set;
        }

      
        
        /// <summary>
        /// Wird in BMWCalculate berechnet
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal NovaBetrag
        {
            get;
            set;
        }

        /// <summary>
        /// Wird in BMWCalculate berechnet
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal ListenpreisUst
        {
            get;
            set;
        }

        /// <summary>
        /// Wird in BMWCalculate berechnet
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal NovaAufschlag
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public bool hasPouvoirMessage
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal pouvoirMin
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal pouvoirMax
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal pouvoirEnteredValue
        {
            get;
            set;
        }
        /// <summary>
        /// Kalkulatorischer Zins Ticket 4056
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal Zins2
        {
            get;
            set;
        }

       [System.Runtime.Serialization.DataMember]
        public FuelTypeConstants Antriebsart
        {
            get;
            set;
        }
       [System.Runtime.Serialization.DataMember]
        public bool Hybrid
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public string Kraftstoff
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public decimal CO2
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public decimal NOX
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public decimal Particles
        {
            get;
            set;
        }
       
        
        [System.Runtime.Serialization.DataMember]
        public bool defaults
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public InsuranceParameterDto rsdvParam
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public InsuranceResultDto rsdvResult
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public ProvisionDto rsdvProvision
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public InsuranceParameterDto gapParam
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public InsuranceResultDto gapResult
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public ProvisionDto gapProvisionDto
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public bool restwertgarantie
        {
            get;
            set;
        }

        //AIDA 1.1-Fields:

       

        /// <summary>
        /// Default-Restwert, aus ANGKALKRWKALKBRUTTO_DEFAULT
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal RestwertBrutto_Default
        {
            get;
            set;
        }
        /// <summary>
        /// Default-Rate, aus ANGKALKRWKALKBRUTTOP_DEFAULT
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal RestwertBruttoP_Default
        {
            get;
            set;
        }
        /// <summary>
        /// Default-Gebühr, aus ANGKALKGEBUEHR_DEFAULT
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal Bearbeitungsgebuehr_Default
        {
            get;
            set;
        }
        /// <summary>
        /// Default-Zinseff, aus ANGKALKZINSEFF_DEFAULT*
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal ZinsEff_Default
        {
            get;
            set;
        }
        /// <summary>
        /// Default-Zins, aus ANGKALKZINS_DEFAULT*
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal ZinsNominal_Default
        {
            get;
            set;
        }
        /// <summary>
        /// Vorvertragsnummer
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public long? SYSVORVT
        {
            get;
            set;
        }

        
        /// <summary>
        /// Perole für die Abschlussprovision
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public long APROVPEROLE
        {
            get;
            set;
        }

        /// <summary>
        /// Nettowert als Ausgangsbasis für eine Source verwenden
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public bool useNettoSource
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public int? erinklmwst
        {
            get;
            set;
        }
         [System.Runtime.Serialization.DataMember]
        public long sysKdTyp
        {
            get;
            set;
        }
         [System.Runtime.Serialization.DataMember]
         public int IgnoreInsurances
         {
             get;
             set;
         }
         [System.Runtime.Serialization.DataMember]
         public decimal Subvention_Zins2
         {
             get;
             set;
         }

         [System.Runtime.Serialization.DataMember]
         public decimal ZinsBasis
         {
             get;
             set;
         }
         [System.Runtime.Serialization.DataMember]
         public decimal ZinsAktion
         {
             get;
             set;
         }

        [System.Runtime.Serialization.DataMember]
        public CalculationTargets CalculationTarget
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public decimal Rabatt
        {
            get;
            set;
        }

        public bool skipRWCheck
        {
            get;
            set;
        }
        public bool skipMVZCheck
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public decimal MonatlicheRateCalced
        {
            get;
            set;
        }
        /// <summary>
        /// Rabattierfähiger Betrag
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal ANGOBGRUNDEXTERN 
        {
            get;
            set;
        }
        /// <summary>
        /// Rabattierfähiger Betrag
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal ANGOBAHKEXTERNBRUTTO
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public decimal ANGKALKBWFEHLER
        {
            get;
            set;
        }

    }
}
