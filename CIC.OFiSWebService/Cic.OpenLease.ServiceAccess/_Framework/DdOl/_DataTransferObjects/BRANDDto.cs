// OWNER JJ, 27-01-2010
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class BRANDDto
    {
        #region Ids properties
        
        [System.Runtime.Serialization.DataMember]
        public long? SYSBRAND
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
        public PEROLEDto[] VpPEROLEDtoArray
        {
            get;
            set;
        }
        #endregion
    }
}