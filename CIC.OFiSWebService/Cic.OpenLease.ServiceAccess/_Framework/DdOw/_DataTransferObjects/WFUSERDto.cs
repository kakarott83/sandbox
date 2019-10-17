// OWNER JJ, 27-01-2010
namespace Cic.OpenLease.ServiceAccess.DdOw
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class WFUSERDto
    {
        #region Ids properties
        
        [System.Runtime.Serialization.DataMember]
        public long? SYSWFUSER
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
        #endregion
    }
}