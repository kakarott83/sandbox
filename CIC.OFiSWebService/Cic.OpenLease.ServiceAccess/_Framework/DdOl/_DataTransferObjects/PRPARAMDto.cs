// OWNER MK, 16-03-2010
using System;
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public class PRPARAMDto
    {
        #region Properties
        
        [System.Runtime.Serialization.DataMember]
        public string NAME
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long SYSPRPARAM
        {
            get;
            set;
        }

        
        
        [System.Runtime.Serialization.DataMember]
        public string DESCRIPTION
        {
            get;
            set;
        }


        
        [System.Runtime.Serialization.DataMember]
        public int? TYP
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? STEPSIZE
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? MINVALN
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? MAXVALN
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? DEFVALN
        {
            get;
            set;
        }
        
        
        [System.Runtime.Serialization.DataMember]
        public decimal? MINVALP
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? MAXVALP
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public decimal? DEFVALP
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public System.DateTime? MINVALD
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public System.DateTime? MAXVALD
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public System.DateTime? DEFVALD
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public int? VISIBILITYFLAG
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public int? DISABLEDFLAG
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long SYSPRFLD
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string PRFLDNAME
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string PRFLDOBJECTMETA
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public long SYSPRPARSET
        {
            get;
            set;
        }

        #endregion
    }
}
