// OWNER MK, 26-08-2009
using System;
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
    /// Datentransferobjekt für Angebotkalkulation
    /// </summary>
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract(Namespace = "http://cic-software.de/datacontract")]
    public class ANGKALKDto
    {
        #region Constructors
        // TEST BK 0 BK, Not tested
        /// <summary>
        /// Konstruktor, Grundbestandteil
        /// </summary>
        public ANGKALKDto(Cic.OpenLease.Model.DdOl.ANGKALK angkalk)
            : this(angkalk, null, null, null, null, null)
        {
        }

        // TEST BK 0 BK, Not tested
        /// <summary>
        /// Konstruktor, alle Bestandteile
        /// </summary>
        public ANGKALKDto(Cic.OpenLease.Model.DdOl.ANGKALK angKalk, Cic.OpenLease.Model.DdOl.OBTYP obTyp, Cic.OpenLease.Model.DdOl.PRPRODUCT prProduct, Cic.OpenLease.Model.DdOl.ANGOB angOb, Cic.OpenLease.Model.DdOl.ANGOBINI angObIni, Cic.OpenLease.Model.DdOl.ANGOBAUST[] andObAusts)
        {
            // Check object
            if (angKalk == null)
            {
                throw new Exception("angKalk");
            }

            // Set properties
            this.ANGKALK = angKalk;
            this.ANGOB = angOb;
            this.OBTYP = obTyp;
            this.PRPRODUCT = prProduct;
            this.ANGOBINI = angObIni;
            this.ANGOBAUSTS = andObAusts;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Grundbestandteil <see cref="Cic.OpenLease.Model.DdOl.ANGEBOT"/>.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public Cic.OpenLease.Model.DdOl.ANGKALK ANGKALK
        {
            // TEST BK 0 BK, Not tested
            get;
            // TEST BK 0 BK, Not tested
            set;
        }

        /// <summary>
        /// Objekttyp
        /// MK
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public Cic.OpenLease.Model.DdOl.OBTYP OBTYP
        {
            // TEST BK 0 BK, Not tested
            get;
            // TEST BK 0 BK, Not tested
            set;
        }

        /// <summary>
        /// Produkt
        /// MK
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public Cic.OpenLease.Model.DdOl.PRPRODUCT PRPRODUCT
        {
            // TEST BK 0 BK, Not tested
            get;
            // TEST BK 0 BK, Not tested
            set;
        }

        /// <summary>
        /// Angebotsobjekt
        /// MK
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public Cic.OpenLease.Model.DdOl.ANGOB ANGOB
        {
            // TEST BK 0 BK, Not tested
            get;
            // TEST BK 0 BK, Not tested
            set;
        }

        /// <summary>
        /// Angebotsobjekt zusätzliche Daten
        /// MK
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public Cic.OpenLease.Model.DdOl.ANGOBINI ANGOBINI
        {
            // TEST BK 0 BK, Not tested
            get;
            // TEST BK 0 BK, Not tested
            set;
        }

        /// <summary>
        /// Angebotsobjektaustatung zusätzliche Daten
        /// MK
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public Cic.OpenLease.Model.DdOl.ANGOBAUST[] ANGOBAUSTS
        {
            // TEST BK 0 BK, Not tested
            get;
            // TEST BK 0 BK, Not tested
            set;
        }


        #endregion
    }
}
