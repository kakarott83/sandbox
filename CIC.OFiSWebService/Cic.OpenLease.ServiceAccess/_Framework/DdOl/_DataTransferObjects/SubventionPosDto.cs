// OWNER JJ, 27-01-2010
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class SubventionPosDto
    {

        
        [System.Runtime.Serialization.DataMember]
        public long SYSPRSUBVPOS
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long SYSPERSON
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string VORNAME
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string NACHNAME
        {
            get;
            set;
        }
       
        /// <summary>
        /// Brutto Subventionsbetrag
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? VALUE
        {
            get;
            set;
        }

        /// <summary>
        /// Netto Subventionsbetrag
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? VALUE_NETTO
        {
            get;
            set;
        }

        /// <summary>
        /// Ust Subventionsbetrag
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? VALUE_UST
        {
            get;
            set;
        }
      

    }
}