// OWNER MK, 24-02-2010
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public class OBTYPDto
    {
        #region Properties
        
        [System.Runtime.Serialization.DataMember]
        public long SYSOBTYP
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
        public string FABRIKAT
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
        public long SYSVGRW
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long SYSVGWR
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long SYSVGRF
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string NOOBJECTS
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public FahrzeugArt FAHRZEUGART
        {
            get;
            set;
        }
        #endregion
    }
}
