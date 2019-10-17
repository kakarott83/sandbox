using System.Collections.Generic;
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    [System.Serializable]
    public class InsuranceResultDto
    {
        //Ergebnisse
        [System.Runtime.Serialization.DataMember]
        public decimal Gesamtfinanzierungsrate
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public decimal rateNeu
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal Praemie
        {
            get;
            set;
        }

        /// <summary>
        /// Default vor Abzüge expl. Subvention, inkl SteuerN
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal Praemie_Default
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal Praemie_Subvention
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal Praemie_SubventionUst
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public decimal Praemie_SubventionNetto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal Provision
        {
            get;
            set;
        }

        
        
        [System.Runtime.Serialization.DataMember]
        public decimal Versicherungssteuer
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal Netto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal Motorsteuer
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
        public long sysvstyppos
        {
            get;
            set;
        }

        //only used for saving back
        public long sysangvspos
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public List<InsuranceResultDto> positions
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public double KOSTENABSCHL { get; set; }
        [System.Runtime.Serialization.DataMember]
        public double KOSTENVERW { get; set; }
        [System.Runtime.Serialization.DataMember]
        public double KOSTENSONST { get; set; }
        [System.Runtime.Serialization.DataMember]
        public double PRAEMIELS { get; set; }
        [System.Runtime.Serialization.DataMember]
        public double PRAEMIEVS { get; set; }
    

    }
}
