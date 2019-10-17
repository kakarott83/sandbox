// OWNER JJ, 27-01-2010
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class VSARTDto
    {
        public VSARTDto()
        {
        }
        public VSARTDto(VSARTDto org)
        {
            this.SYSVSART = org.SYSVSART;
            this.BESCHREIBUNG = org.BESCHREIBUNG;
            this.CODE = org.CODE;
            this.DISABLED = org.DISABLED;
            this.NEEDED = org.NEEDED;
            this.SYSVSART = org.SYSVSART;
            this.VSLIST = org.VSLIST;
            
        }
        #region Ids properties
        
        [System.Runtime.Serialization.DataMember]
        public long SYSVSART
        {
            get;
            set;
        }        
        #endregion       

        #region Properties
        
        [System.Runtime.Serialization.DataMember]
        public string CODE
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string BESCHREIBUNG
        {
            get;
            set;
        }      
        

        
        [System.Runtime.Serialization.DataMember]
        public int? NEEDED
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public int? DISABLED
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public PERSONDto[] VSLIST
        {
            get;
            set;
        }
        #endregion 
    }
}