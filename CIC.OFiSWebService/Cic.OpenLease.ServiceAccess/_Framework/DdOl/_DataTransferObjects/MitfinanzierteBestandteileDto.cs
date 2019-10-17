namespace Cic.OpenLease.ServiceAccess.DdOl
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    #endregion

    public class MitfinanzierteBestandteileDto
    {
        
        #region Properties

        
        [System.Runtime.Serialization.DataMember]
        public string FsArt
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string FsTyp
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal FsPreis
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal Subvention
        {
            get;
            set;
        }

        
        public bool isSubvention
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal FsPreisNetto
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal FsPreisUst
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public int? NEEDED
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public int? DISABLED
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long? SysFsTyp
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public int? FIXVAROPTION
        {
            get;
            set;
        }


        
        [System.Runtime.Serialization.DataMember]
        public int? FIXVARDEFAULT
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string ARTCODE
        {
            get;
            set;
        }

     

        #endregion
    }
    
}
