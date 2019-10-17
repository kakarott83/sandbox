// OWNER MK, 18-11-2009
// TODO JJ 10 JJ, Change namespace -> add DdOl
namespace Cic.OpenLease.ServiceAccess
{
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract]
    public class MyCalcDto
    {
        // Ids
        [System.Runtime.Serialization.DataMember]
        public long SYSMYCALC
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public long SYSPRPRODUCT
        {
            get;
            set;
        }

        // Calculation
        [System.Runtime.Serialization.DataMember]
        public decimal BGEXTERN
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public decimal ZINS
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public decimal ZINSEFF
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public int PPY
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public decimal SZ
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public decimal SZP
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public int LZ
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public decimal RW 
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public decimal RWP
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public decimal RATE
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public long JAHRESKM
        {
            get;
            set;
        }

        // Person
        [System.Runtime.Serialization.DataMember]
        public string MATCHCODE
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public string ANREDE
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public string VORNAME
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public string NAME
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public string TELEFON
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public string HANDY
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public string EMAIL
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public string NOTIZEN
        {
            get;
            set;
        }

        // Objekt
        [System.Runtime.Serialization.DataMember]
        public string OBJEKT
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public string BEMERKUNG2
        {
            get;
            set;
        }

        // Indentification
        [System.Runtime.Serialization.DataMember]
        public string BEMERKUNG1
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public System.DateTime ERSTELLTAM
        {
            get;
            set;
        }
    }
}
