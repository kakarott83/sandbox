namespace Cic.OpenLease.ServiceAccess.DdOl
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;



    public class EinreichungDto
    {

        #region properties

        
        [System.Runtime.Serialization.DataMember]
        public long SYSANGEBOT
        {
            get;
            set;
        }

        /*
        [System.Runtime.Serialization.DataMember]
        public string KOMMENTAR
        {
            get;
            set;
        }*/

       
        [System.Runtime.Serialization.DataMember]
        public string WERBECODE
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public int INFOMAILFLAG
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public int INFOTELFLAG
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public int INFOSMSFLAG
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public int SCHUFAFLAG
        {
            get;
            set;
        }
        [System.Runtime.Serialization.DataMember]
        public int BWAZUSTFLAG
        {
            get;
            set;
        }

        /*[System.Runtime.Serialization.DataMember]
        public int KUNDEUNTERSCHRIEBEN
        {
            get;
            set;
        }*/

        
        [System.Runtime.Serialization.DataMember]
        public int TRADEONOWNACCOUNT
        {
            get;
            set;
        }

        /*
        [System.Runtime.Serialization.DataMember]
        public string EINREICHUNGVTART
        {
            get;
            set;
        }*/
        
       /* [System.Runtime.Serialization.DataMember]
        public string VORVTNUMMER
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string VORKENNZEICHEN
        {
            get;
            set;
        }*/
       /* 
        [System.Runtime.Serialization.DataMember]
        public string FREMDVERSICHERUNG
        {
            get;
            set;
        }


        
        [System.Runtime.Serialization.DataMember]
        public DateTime ANGOBLIEFERUNG
        {
            get;
            set;
        }
        */

        [System.Runtime.Serialization.DataMember]
        public int? SCHUFAFLAGMA
        {
            get;
            set;
        }


        [System.Runtime.Serialization.DataMember]
        public int? SCHUFAFLAGIH
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public int RESUBMIT { get; set; }

        [System.Runtime.Serialization.DataMember]
        public string SCHUFAFLAGIHNUM
        {
            get;
            set;
        }
        #endregion
    }

}