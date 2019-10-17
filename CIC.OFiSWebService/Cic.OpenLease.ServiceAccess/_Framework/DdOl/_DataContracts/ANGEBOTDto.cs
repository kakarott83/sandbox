// OWNER MK, 05-03-2009
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
    /// Datentransferobjekt für Angebote
    /// </summary>
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract(Namespace = "http://cic-software.de/datacontract")]
    public class ANGEBOTDto
    {
		#region Constructors
		// TEST BK 0 BK, Not tested
		/// <summary>
		/// Konstruktor, Grundbestandteil
		/// </summary>
		public ANGEBOTDto(Cic.OpenLease.Model.DdOl.ANGEBOT angebot)
            : this(angebot, null, null)
        {
        }

		// TEST BK 0 BK, Not tested
		/// <summary>
		/// Konstruktor, alle Bestandteile
		/// </summary>
		public ANGEBOTDto(Cic.OpenLease.Model.DdOl.ANGEBOT angebot, Cic.OpenLease.Model.DdOl.PERSON customerPerson, Cic.OpenLease.Model.DdOl.IT it)
        {
			// Check object
			if (angebot == null)
            {
                throw new Cic.Basic.ApplicationArgumentIsNull("angebot");
            }

			// Set properties
			this.ANGEBOT = angebot;
			this.PERSON = customerPerson;
            this.IT = it;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Grundbestandteil <see cref="Cic.OpenLease.Model.DdOl.ANGEBOT"/>.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public Cic.OpenLease.Model.DdOl.ANGEBOT ANGEBOT
        {
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
        }

        /// <summary>
        /// Bestandteil <see cref="Cic.OpenLease.Model.DdOl.PERSON"/>.
        /// Kunde (PERSON)
        /// </summary>
        [System.Runtime.Serialization.DataMember]
		public Cic.OpenLease.Model.DdOl.PERSON PERSON
        {
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}

        /// <summary>
        /// Bestandteil <see cref="Cic.OpenLease.Model.DdOl.IT"/>.
        /// Kunde (IT)
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public Cic.OpenLease.Model.DdOl.IT IT
        {
            // TEST BK 0 BK, Not tested
            get;
            // TEST BK 0 BK, Not tested
            set;
        }

        /// <summary>        
        /// Number of elements ANGKALK
        /// JJ
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public int ANGKALKCount
        {
            // TEST BK 0 BK, Not tested
            get;
            // TEST BK 0 BK, Not tested
            set;
        }
        #endregion
    }
}
