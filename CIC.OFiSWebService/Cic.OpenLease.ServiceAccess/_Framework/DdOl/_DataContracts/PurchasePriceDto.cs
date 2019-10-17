namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    [System.Serializable]
    public class PurchasePriceDto
    {
        public enum MessageCodes
        {
            Mvz001 = 0,
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


        public enum CalculationSources
        {
            ListenpreisBrutto = 0,
            ListenpreisRabattO,
            ListenpreisRabattOP,
            SonderausstattungBrutto,
            SonderausstattungRabattO,
            SonderausstattungRabattOP,
            PaketeBrutto,
            PaketeRabattO,
            PaketeRabattOP,
            HerstellerzubehorBrutto,
            HerstellerzubehorRabattO,
            HerstellerzubehorRabattOP,
            HandlerzubehorBrutto,
            HandlerzubehorRabattO,
            HandlerzubehorRabattOP,
            KaufpreisRabattBrutto,
            KaufpreisRabattO,
            KaufpreisRabattOP,
            KaufpreisExternBrutto,
            Initialize,
            SonderausstattungUser,
            ResetNova,
            RabattfaehigBetragRabatt,
            RabattfaehigBetragRabattP

        }

        
        [System.Runtime.Serialization.DataMember]
        public CalculationSources CalculationSource
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal ListenpreisBrutto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal ListenpreisRabattO
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal ListenpreisRabattOP
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public decimal RabattfaehigbetragBrutto
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public decimal RabattfaehigbetragRabatt
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public decimal RabattfaehigbetragRabattBrutto
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public decimal RabattfaehigbetragRabattP
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public decimal RabattfaehigbetragExtern
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal ListenpreisExternBrutto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal ListenpreisExternUst
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal ListenpreisExtern
        {
            get;
            set;
        }
        /// <summary>
        /// Sonderausstattung aus SA3/CarConfigurator
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal SonderausstattungDefault
        {
            get;
            set;
        }
        /// <summary>
        /// Sonderausstattung Eingabe Haendler
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal SonderausstattungUser
        {
            get;
            set;
        }
        /// <summary>
        /// Gesamtsumme Sonderausstattung (SonderausstattungDefault+SonderausstattungUser)
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal SonderausstattungBrutto
        {
            get;
            set;
        }
        /// <summary>
        /// Rabatt Betrag auf Gesamtsumme Sonderausstattung
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal SonderausstattungRabattO
        {
            get;
            set;
        }
        /// <summary>
        /// Rabatt prozent auf Gesamtsumme Sonderausstattung
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal SonderausstattungRabattOP
        {
            get;
            set;
        }
        /// <summary>
        /// Gesamtsumme Sonderausstattung inkl Nachlass Brutto
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal SonderausstattungExternBrutto
        {
            get;
            set;
        }
        /// <summary>
        /// Ust auf Gesamtsumme Sonderausstattung inkl Nachlass
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal SonderausstattungExternUst
        {
            get;
            set;
        }
        /// <summary>
        /// Gesamtsumme Sonderausstattung inkl Nachlass Netto
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal SonderausstattungExtern
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal PaketeBrutto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal PaketeRabattO
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal PaketeRabattOP
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal PaketeExternBrutto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal PaketeExternUst
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal PaketeExtern
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal HerstellerzubehorBrutto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal HerstellerzubehorRabattO
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal HerstellerzubehorRabattOP
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal HerstellerzubehorExternBrutto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal HerstellerzubehorExternUst
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal HerstellerzubehorExtern
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal HandlerzubehorBrutto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal HandlerzubehorRabattO
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal HandlerzubehorRabattOP
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal HandlerzubehorExternBrutto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal HandlerzubehorExternUst
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal HandlerzubehorExtern
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

        [System.Runtime.Serialization.DataMember]
        public decimal Kaufpreis
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal KaufpreisRabattO
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public decimal KaufpreisRabattBrutto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal KaufpreisRabattOP
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal KaufpreisExternBrutto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal KaufpreisExternUst
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal KaufpreisExtern
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public bool NovaBefreiung
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal NovaZuAbBrutto
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal NovaZuAbBruttoOrg
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal NovaZuAbUst
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal NovaZuAb
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
        public long? SysObTyp
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long? SysObArt { get; set; }

        
        [System.Runtime.Serialization.DataMember]
        public bool NovaBefreiungAlt
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long SYSVART { get; set; }

        // Price
        
        [System.Runtime.Serialization.DataMember]
        public decimal NovaBrutto
        {
            get;
            set;
        }

        // Tax amount
        
        [System.Runtime.Serialization.DataMember]
        public decimal NovaUst
        {
            get;
            set;
        }

        // Price
        
        [System.Runtime.Serialization.DataMember]
        public decimal NovaNetto
        {
            get;
            set;
        }

        // Neu für CO2-Reifen:
       
        [System.Runtime.Serialization.DataMember]
        public decimal NovaSatz
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public decimal NovaSatzDef
        {
            get;
            set;
        }

        /// <summary>
        /// g/km
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal CO2
        {
            get;
            set;
        }
        /// <summary>
        /// g/km
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal CO2Def
        {
            get;
            set;
        }
        /// <summary>
        /// mg/km
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal NOX
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public decimal NOXDef
        {
            get;
            set;
        }
        /// <summary>
        /// Particle mass in g/km
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal Particles
        {
            get;
            set;
        }
        /// <summary>
        /// Default Particle mass in g/km
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal ParticlesDef
        {
            get;
            set;
        }
        // Fuel consumption (avg)
        [System.Runtime.Serialization.DataMember]
        public decimal Verbrauch
        {
            get;
            set;
        }
        // Fuel consumption (avg)
        [System.Runtime.Serialization.DataMember]
        public decimal VerbrauchDef
        {
            get;
            set;
        }

        // Fuel type
       
        [System.Runtime.Serialization.DataMember]
        public FuelTypeConstants Antriebsart
        {
            get;
            set;
        }
        // Hybrid
        
        [System.Runtime.Serialization.DataMember]
        public bool Hybrid
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public bool HybridDef
        {
            get;
            set;
        }
        //Ende Neu für CO2-Reifen       
        [System.Runtime.Serialization.DataMember]
        public System.DateTime Lieferdatum
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public decimal ListenpreisNettoNetto
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public decimal ueberfuehrungskostenBrutto
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public decimal zulassungskostenBrutto
        {
            get;
            set;
        }

        /// <summary>
        /// When true interpret the given rabatt/rabattp as netto
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public bool NettoFlag
        {
            get;
            set;
        }

        /// <summary>
        /// ERINKL
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public int? ANGOBERINKLMWST
        {
            get;
            set;
        
        }

        [System.Runtime.Serialization.DataMember]
        public OfferTypeConstants CONFIGSOURCE
        {
            get;
            set;
        }
    }
}
