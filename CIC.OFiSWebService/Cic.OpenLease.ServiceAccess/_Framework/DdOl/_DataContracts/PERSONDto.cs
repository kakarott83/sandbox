// OWNER MK, 26-02-2009
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
    /// Datentransferobjekt für Personen
    /// </summary>
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract(Namespace = "http://cic-software.de/datacontract")]
    public class PERSONDto
    {
		#region Constructors
        // TEST BK 0 BK, Not tested
		/// <summary>
		/// Konstruktor, ohne Bestandteile
		/// </summary>
		public PERSONDto()               
        {
        }

		// TEST BK 0 BK, Not tested
		/// <summary>
		/// Konstruktor, Grundbestandteil
		/// </summary>
		public PERSONDto(Cic.OpenLease.Model.DdOl.PERSON person)
            : this(person, null, null, null, null, null, null, null, null, null, null, null, null)
        {
        }

		// TEST BK 0 BK, Not tested
		/// <summary>
		/// Konstruktor, alle Bestandteile
		/// </summary>
		public PERSONDto(Cic.OpenLease.Model.DdOl.PERSON person, Cic.OpenLease.Model.DdOl.HOBBY hobby, Cic.OpenLease.Model.DdOl.KONTO konto, Cic.OpenLease.Model.DdOl.LAND land, Cic.OpenLease.Model.DdOl.LAND landNationality, Cic.OpenLease.Model.DdOw.CTLANG ctlang, Cic.OpenLease.Model.DdOl.WAEHRUNG waehrung, Cic.OpenLease.Model.DdOl.BRANCHE branche, Cic.OpenLease.Model.DdOl.STAAT staat, Cic.OpenLease.Model.DdOl.BLZ blz, Cic.OpenLease.Model.DdOl.KONZERN konzern, Cic.OpenLease.Model.DdOl.UKZ ukz, Cic.OpenLease.Model.DdOl.PARTNER partner)
        {
			// Set properties
			this.PERSON = person;
			this.HOBBY = hobby;
			this.KONTO = konto;
            this.LAND = land;
			this.LANDNationality = landNationality;
			this.CTLANG = ctlang;
            this.WAEHRUNG = waehrung;
            this.BRANCHE = branche;
            this.STAAT = staat;
            this.BLZ = blz;
            this.KONZERN = konzern;
            this.UKZ = ukz;
            this.PARTNER = partner;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Grundbestandteil <see cref="Cic.OpenLease.Model.DdOl.PERSON"/>.
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
        /// Bestandteil <see cref="Cic.OpenLease.Model.DdOl.HOBBY"/>.
        /// Ergänzungsdaten zur Person (Verknüpfung 1:1)
        /// </summary>
		[System.Runtime.Serialization.DataMember]
		public Cic.OpenLease.Model.DdOl.HOBBY HOBBY
		{
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}

        /// <summary>
        /// Bestandteil <see cref="Cic.OpenLease.Model.DdOl.KONTO"/>.
        /// Kontodaten
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public Cic.OpenLease.Model.DdOl.KONTO KONTO
        {
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}

        /// <summary>
        /// Bestandteil <see cref="Cic.OpenLease.Model.DdOl.LAND"/>.
        /// Staat
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public Cic.OpenLease.Model.DdOl.LAND LAND
        {
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}

        /// <summary>
        /// Bestandteil <see cref="Cic.OpenLease.Model.DdOl.LAND"/>.
        /// Nationalität
        /// </summary>
		[System.Runtime.Serialization.DataMember]
		public Cic.OpenLease.Model.DdOl.LAND LANDNationality
		{
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}

        /// <summary>
        /// Bestandteil <see cref="Cic.OpenLease.Model.DdOw.CTLANG"/>.
        /// Sprache
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public Cic.OpenLease.Model.DdOw.CTLANG CTLANG
        {
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}

        /// <summary>
        /// Bestandteil <see cref="Cic.OpenLease.Model.DdOl.WAEHRUNG"/>.
        /// Währung
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public Cic.OpenLease.Model.DdOl.WAEHRUNG WAEHRUNG
        {
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}

        /// <summary>
        /// Bestandteil<see cref="Cic.OpenLease.Model.DdOl.BRANCHE"/>.
		/// Branche
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public Cic.OpenLease.Model.DdOl.BRANCHE BRANCHE
        {
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}

        /// <summary>
        /// Bestandteil <see cref="Cic.OpenLease.Model.DdOl.STAAT"/>.
		/// Bundesland, Kanton
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public Cic.OpenLease.Model.DdOl.STAAT STAAT
        {
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}

        /// <summary>
        /// Bestandteil <see cref="Cic.OpenLease.Model.DdOl.BLZ"/>.
		/// Bank
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public Cic.OpenLease.Model.DdOl.BLZ BLZ
        {
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}

        /// <summary>
        /// Bestandteil <see cref="Cic.OpenLease.Model.DdOl.KONZERN"/>.
		/// Konzerndaten
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public Cic.OpenLease.Model.DdOl.KONZERN KONZERN
        {
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}

        /// <summary>
        /// Bestandteil <see cref="Cic.OpenLease.Model.DdOl.UKZ"/>.
        /// Unternehmenskennzahlen
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public Cic.OpenLease.Model.DdOl.UKZ UKZ
        {
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}

        /// <summary>
        /// Bestandteil <see cref="Cic.OpenLease.Model.DdOl.PARTNER"/>.
        /// Ansprechpartner
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public Cic.OpenLease.Model.DdOl.PARTNER PARTNER
        {
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}
        #endregion
    }
}
