using System;
namespace Cic.OpenLease.ServiceAccess.DdOw
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public sealed class EAIHotDto
    {
        #region Ids
        
        [System.Runtime.Serialization.DataMember]
        public long? SYSEAIHOT
        {
            get;
            set;
        }

        public long? SYSOLTABLE
        {
            get;
            set;
        }

        #endregion

        #region Properties
        

        
        [System.Runtime.Serialization.DataMember]
        public int? FILEFLAGOUT
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public AreaConstants area
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string Description
        {
            get;
            set;
        }


        
        [System.Runtime.Serialization.DataMember]
        public string INPUTPARAMETER1
        {
            get;
            set;
        }


        
        [System.Runtime.Serialization.DataMember]
        public string INPUTPARAMETER2
        {
            get;
            set;
        }


        
        [System.Runtime.Serialization.DataMember]
        public string INPUTPARAMETER3
        {
            get;
            set;
        }


        
        [System.Runtime.Serialization.DataMember]
        public string INPUTPARAMETER4
        {
            get;
            set;
        }


        
        [System.Runtime.Serialization.DataMember]
        public string INPUTPARAMETER5
        {
            get;
            set;
        }


        
        [System.Runtime.Serialization.DataMember]
        public string OUTPUTPARAMETER1
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public DocumentStatusConstants PROZESSSTATUS
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string OUTPUTPARAMETER2
        {
            get;
            set;
        }


        
        [System.Runtime.Serialization.DataMember]
        public string OUTPUTPARAMETER3
        {
            get;
            set;
        }


        
        [System.Runtime.Serialization.DataMember]
        public string OUTPUTPARAMETER4
        {
            get;
            set;
        }


        
        [System.Runtime.Serialization.DataMember]
        public string OUTPUTPARAMETER5
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public DateTime? FINISHDATE
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public DateTime? ANGEBOTVON
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public DateTime? ANGEBOTBIS
        {
            get;
            set;
        }
        
        
        [System.Runtime.Serialization.DataMember]
        public string VERKAEUFERNAME
        {
            get;
            set;
        }

       
        #endregion
    }
}
