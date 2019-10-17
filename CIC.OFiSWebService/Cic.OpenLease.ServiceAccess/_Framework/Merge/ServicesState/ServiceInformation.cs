namespace Cic.OpenLease.ServiceAccess.Merge.ServicesState
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Runtime.Serialization;
    #endregion

    public class ServiceInformation
    {
        #region Properties
        [DataMember]
        public string ServiceVersion
        {
            get;
            set;
        }

        [DataMember]
        public string OpenLeaseDatabaseVersion
        {
            get;
            set;
        }

        [DataMember]
        public DatabaseInstanceInfo DatabaseInstance
        {
            get;
            set;
        }

        [DataMember]
        public DatabaseConnectionInfo DatabaseConnection
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public ServiceInformation()
        {
            this.DatabaseInstance = new DatabaseInstanceInfo();
            this.DatabaseConnection = new DatabaseConnectionInfo();
        }
        #endregion
    }
}
