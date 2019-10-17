namespace Cic.OpenLease.ServiceAccess.Merge.ServicesState
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Runtime.Serialization;
    #endregion

    public class DatabaseInstanceInfo
    {
        #region Properties
        [DataMember]
        public string InstanceName
        {
            get;
            set;
        }

        [DataMember]
        public string HostName
        {
            get;
            set;
        }

        [DataMember]
        public string Version
        {
            get;
            set;
        }
        #endregion
    }
}
