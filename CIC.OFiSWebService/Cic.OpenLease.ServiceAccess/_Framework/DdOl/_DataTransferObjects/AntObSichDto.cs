namespace Cic.OpenLease.ServiceAccess.DdOl
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    #endregion

    public class AntObSichDto
    {
        #region Properties
        
        [System.Runtime.Serialization.DataMember]
        public string PersonNameVornameGebDatum
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long? AntobSichSysMh
        {
            get;
            set;
        }
        #endregion
    }
}
