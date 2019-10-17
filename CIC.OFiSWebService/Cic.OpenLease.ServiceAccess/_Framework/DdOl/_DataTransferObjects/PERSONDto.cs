// OWNER JJ, 27-01-2010
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class PERSONDto
    {
        public PERSONDto()
        {
        }

        public PERSONDto(PERSONDto org)
        {
            this.CODE = org.CODE;
            this.DISABLED = org.DISABLED;
            this.NAME = org.NAME;
            this.NEEDED = org.NEEDED;
            this.PRIVATFLAG = org.PRIVATFLAG;
            this.SYSPERSON = org.SYSPERSON;
            this.SYSPUSER = org.SYSPUSER;
            this.VORNAME = org.VORNAME;
            this.VSTYPLIST = org.VSTYPLIST;
        }
        #region Ids properties
        
        [System.Runtime.Serialization.DataMember]
        public long? SYSPERSON
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long? SYSPUSER
        {
            get;
            set;
        }        
        #endregion       

        #region Flag properties
        
        [System.Runtime.Serialization.DataMember]
        public bool PRIVATFLAG
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
        public string NAME
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
        public VSTYPDto[] VSTYPLIST
        {
            get;
            set;
        }
        #endregion
    }
}