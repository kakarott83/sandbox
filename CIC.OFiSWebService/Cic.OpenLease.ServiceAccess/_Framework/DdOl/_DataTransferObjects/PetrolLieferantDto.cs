namespace Cic.OpenLease.ServiceAccess.DdOl
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;


    public class PetrolLieferantDto
    {

        #region Ids properties

        
        [System.Runtime.Serialization.DataMember]
        public long? SYSFSTYP
        {
            get;
            set;
        }
        #endregion

        #region Properties
        
        [System.Runtime.Serialization.DataMember]
        public string BEZEICHNUNG
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string BESCHREIBUNG
        {
            get;
            set;
        }

        #endregion
    }
}