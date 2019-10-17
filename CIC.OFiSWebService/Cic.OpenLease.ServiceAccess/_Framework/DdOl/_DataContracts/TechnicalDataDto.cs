namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    [System.Serializable]
    public class TechnicalDataDto
    {
        
        [System.Runtime.Serialization.DataMember]
        public decimal CO2Emission
        {
            get;
            set;
        }

        // Diesel particulate filter
        
        [System.Runtime.Serialization.DataMember]
        public string DPF
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

        // Nox
        
        [System.Runtime.Serialization.DataMember]
        public decimal NOXEmission
        {
            get;
            set;
        }

        // Particles
        
        [System.Runtime.Serialization.DataMember]
        public decimal Particles
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

        // Fuel consumption (avg)
        
        [System.Runtime.Serialization.DataMember]
        public decimal Verbrauch
        {
            get;
            set;
        }

        // KW/PS
        
        [System.Runtime.Serialization.DataMember]
        public string Leistung
        {
            get;
            set;
        }

        // KW
        
        [System.Runtime.Serialization.DataMember]
        public long Kw
        {
            get;
            set;
        }

        // PS
        
        [System.Runtime.Serialization.DataMember]
        public long Ps
        {
            get;
            set;
        }

        // CCM
        
        [System.Runtime.Serialization.DataMember]
        public long Ccm
        {
            get;
            set;
        }

        // Transmission type
        
        [System.Runtime.Serialization.DataMember]
        public string Getriebeart
        {
            get;
            set;
        }

        // Automatic transmission flag
        
        [System.Runtime.Serialization.DataMember]
        public int Automatic
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

        
        [System.Runtime.Serialization.DataMember]
        public decimal ListenpreisBrutto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal ListenpreisBruttoexklNoVa
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal SonderausstattungBrutto
        {
            get;
            set;
        }


        
        [System.Runtime.Serialization.DataMember]
        public decimal SonderausstattungBruttoexklNoVa
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
        public decimal PaketeBruttoexklNoVa 
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

        
        [System.Runtime.Serialization.DataMember]
        public string Kraftstoff 
        { 
            get; 
            set; 
        }

        
        [System.Runtime.Serialization.DataMember]
        public bool NovaBefreiungAlt
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long  SYSVART
        {
            get;
            set;
        }

      

        public void reset()
        {
            CO2Emission = 0;
            Particles = 0;
            NOXEmission = 0;
            Verbrauch = 0;
            Ccm = 0;
            Ps = 0;
            Kw = 0;
            Leistung = "";
            DPF = "";
            NovaSatz = 0;
            kwe = 0;
            pse = 0;
            kwgesamt = 0;
            psgesamt = 0;
            kwh = 0;
            eek = "";
            reichweite = 0;
        }


        
        [System.Runtime.Serialization.DataMember]
        public bool fetchListenpreis
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public bool isMotorrad
        {
            get;
            set;
        }

        // Fuel type
        
        [System.Runtime.Serialization.DataMember]
        public long SYSMART
        {
            get;
            set;
        }
        // true if it is an BMW i Model
        
        [System.Runtime.Serialization.DataMember]
        public bool isBMWi
        {
            get;
            set;
        }


        /// <summary>
        /// KW E-Motor
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public int kwe { get; set; }
        /// <summary>
        /// PS E-Motor
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public int pse { get; set; }
        [System.Runtime.Serialization.DataMember]
        public int kwgesamt { get; set; }
        [System.Runtime.Serialization.DataMember]
        public int psgesamt { get; set; }
        /// <summary>
        /// Energieeffizienzklasse
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public string eek { get; set; }
        [System.Runtime.Serialization.DataMember]
        public int reichweite { get; set; }
        /// <summary>
        /// Energieverbrauch
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public int kwh { get; set; }

        public long sysobtyp { get; set; }

        [System.Runtime.Serialization.DataMember]
        public decimal ListenpreisNettoNetto
        {
            get;
            set;
        }
    }
    public class ExtTechInfo
    {
        /// <summary>
        /// KW E-Motor
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public int kwe { get; set; }
        /// <summary>
        /// PS E-Motor
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public int pse { get; set; }
        [System.Runtime.Serialization.DataMember]
        public int kwgesamt { get; set; }
        [System.Runtime.Serialization.DataMember]
        public int psgesamt { get; set; }
        /// <summary>
        /// Energieeffizienzklasse
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public string eek { get; set; }
        [System.Runtime.Serialization.DataMember]
        public int reichweite { get; set; }
        /// <summary>
        /// Energieverbrauch
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public int kwh { get; set; }
    }
}
