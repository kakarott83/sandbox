namespace Cic.OpenLease.ServiceAccess.Merge.OlClient
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    #endregion

    [System.Runtime.Serialization.DataContract]
    public class DocumentUploadDto
    {
        
        [System.Runtime.Serialization.DataMember]
        public long sysid
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public String searchterms
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public String remark
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public String filename
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public byte[] filedata
        {
            get;
            set;
        }
    }
}
