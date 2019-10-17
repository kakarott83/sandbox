// OWNER JJ, 27-01-2010
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class PEROLEDto
    {
        #region Ids properties
        
        [System.Runtime.Serialization.DataMember]
        public long? SYSPEROLE
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long? SYSPERSON
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long? SYSPARENT
        {
            get;
            set;
        } 
        #endregion       

        #region Properties
        
        [System.Runtime.Serialization.DataMember]
        public string NAME
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string DESCRIPTION
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public PERSONDto PERSONDto
        {
            get;
            set;
        }


        
        [System.Runtime.Serialization.DataMember]
        public PEROLEDto[] PEROLEDDtoArray
        {
            get;
            set;
        }
        #endregion
    }
}