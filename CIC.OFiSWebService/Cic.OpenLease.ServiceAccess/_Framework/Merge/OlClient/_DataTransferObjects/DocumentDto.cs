namespace Cic.OpenLease.ServiceAccess.Merge.OlClient
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    #endregion

    [System.Runtime.Serialization.DataContract]
    public class DocumentDto
    {
        #region Properties
        [System.Runtime.Serialization.DataMember]
        public long SysEaiHot
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public String Description
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public DocumentStatusConstants Status
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public EaiHFileDto[] Files
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public EaiErrorDto[] Errors
        {
            get;
            set;
        }
        #endregion
    }
}
