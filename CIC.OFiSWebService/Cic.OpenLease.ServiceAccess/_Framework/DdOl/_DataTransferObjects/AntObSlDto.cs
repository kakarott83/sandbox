namespace Cic.OpenLease.ServiceAccess.DdOl
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    #endregion

    public class AntObSlDto
    {
        #region Properties
        
        [System.Runtime.Serialization.DataMember]
        public string Bezeichnung
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal Betrag
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public int? Rang
        {
            get;
            set;
        }
        #endregion
    }
}
