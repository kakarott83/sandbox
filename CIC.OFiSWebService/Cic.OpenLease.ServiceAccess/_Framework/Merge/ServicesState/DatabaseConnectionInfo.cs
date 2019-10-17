namespace Cic.OpenLease.ServiceAccess.Merge.ServicesState
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
using System.Runtime.Serialization;
    #endregion

    public class DatabaseConnectionInfo
    {
        #region Properties
        [DataMember]
        public string SId
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
        public bool DirectConnection
        {
            get;
            set;
        }

        [DataMember]
        public int Port
        {
            get;
            set;
        }

        [DataMember]
        public string ClientVersion
        {
            get;
            set;
        }

        [DataMember]
        public string Name
        {
            get;
            set;
        }
        #endregion
    }
}
