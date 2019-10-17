using System;
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    [System.Serializable]
    public class ObjectContextDto
    {
        
        [System.Runtime.Serialization.DataMember]
        public OBARTDto[] ObjectArts
        {
            get;
            set;
        }

        /*
        [System.Runtime.Serialization.DataMember]
        public OBKATDto[] ObjectCategories
        {
            get;
            set;
        }*/

        
        [System.Runtime.Serialization.DataMember]
        public OBTYPDto ObjectType
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public bool IsEurotaxNrValid
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string Hersteller
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public string Fabrikat
        {
            get;
            set;
        }

        
        [System.Runtime.Serialization.DataMember]
        public bool isMotorrad
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public bool isFromSa3
        {
            get;
            set;
        }
        
        [System.Runtime.Serialization.DataMember]
        public long sysBrand
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public DateTime? baujahrVon
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public DateTime? baujahrBis
        {
            get;
            set;
        }
        
    }
}
