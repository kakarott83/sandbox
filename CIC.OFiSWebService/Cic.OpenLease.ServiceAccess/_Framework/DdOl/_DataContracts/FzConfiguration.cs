namespace Cic.OpenLease.ServiceAccess.DdOl
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    #endregion

    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public class FzConfiguration
    {
        #region Properties
        [System.Runtime.Serialization.DataMember]
        public string Eurotaxnummer
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public string Bezeichnung
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public DateTime GUELTIGVON
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public string OKA
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public string Sas
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public decimal Nova
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public decimal Verbrauch
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public FuelTypeConstants Treibstoff
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public decimal Ccm
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public decimal KW
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public decimal CO2
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public decimal NOx
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public decimal LPNetto
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public decimal LPBrutto
        {
            get;
            set;
        }

    
        #endregion




    }
}