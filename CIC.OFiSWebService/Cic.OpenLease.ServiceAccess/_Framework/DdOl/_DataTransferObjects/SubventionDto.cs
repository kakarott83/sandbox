// OWNER JJ, 27-01-2010
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class SubventionDto
    {


        
        [System.Runtime.Serialization.DataMember]
        public string NAME
        {
            get;
            set;
        }

        /// <summary>
        /// Ausgangsbetrag der subventioniert wird
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? DEFAULTVALUE
        {
            get;
            set;
        }
       
        /// <summary>
        /// Max zu Subventionierender Betrag
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? CURRENTVALUE
        {
            get;
            set;
        }

        /// <summary>
        /// Subventionierter Betrag Brutto
        /// </summary>
        
        [System.Runtime.Serialization.DataMember]
        public decimal? VALUE
        {
            get;
            set;
        }


        
        [System.Runtime.Serialization.DataMember]
        public SubventionPosDto[] SUBVENTIONSGEBER
        {
            get;
            set;
        }
      

    }
}