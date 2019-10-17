namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    [System.Serializable]
    public class ProvisionDto
    {
        /// <summary>
        /// Internally used
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public long sysPrhgroup
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public int laufzeit
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public long sysprproduct
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public bool noProvision
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long sysobtyp
        {
            get;
            set;
        }
        /// <summary>
        /// Input für Kasko, Haftpflicht
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal versicherungspraemieexkl
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal wunschprovision
        {
            get;
            set;
        }

        /// <summary>
        /// Input für Restschuldprovision
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal versicherungspraemiegesamt
        {
            get;
            set;
        }

        /// <summary>
        /// Input für Bearbeitungsprovision, NETTO
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal bearbeitungsgebuehr
        {
            get;
            set;
        }

        /// <summary>
        /// Ergebnis-Provision
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal provision
        {
            get;
            set;
        }

       /// <summary>
       /// Zusatzergebnis bei Abschlussprovision
       /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal sfAufschlag
        {
            get;
            set;
        }

        public decimal deltaStandard
        {
            get;
            set;
        }

        /// <summary>
        /// Zusatzergebnis bei Berechnung Abschlussprovision
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal zugangsProvision
        {
            get;
            set;
        }
       

        
        [System.Runtime.Serialization.DataMember]
        public long sysPerole
        {
            get;
            set;
        }


        
        [System.Runtime.Serialization.DataMember]
        public long sysBrand
        {
            get;
            set;
        }


        
        [System.Runtime.Serialization.DataMember]
        public int rank
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long sysProvTarif
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal finanzierungssumme
        {
            get;
            set;
        }

        
        public PRPARAMDto prparam
        {
            get;
            set;
        }
        public long sysVstyp{ get; set; }

    }
}
