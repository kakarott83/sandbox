// OWNER MK, 05-03-2009
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
	/// Suchwerte für Angebote
    /// </summary>
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract(Namespace = "http://cic-software.de/datacontract")]
    public class ANGEBOTSearchData
    {
		#region Methods
		/// <summary>
		/// Übernimmt die Werte einer anderen Instanz.
		/// </summary>
		public void AdoptValues(Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTSearchData angebotSearchData)
		{
			// Check object
			if (angebotSearchData == null)
			{
				// Throw 
				throw new Cic.Basic.ApplicationArgumentIsNull("angebotSearchData");
			}

			// Set data
			this.SYSIT = angebotSearchData.SYSIT;
			this.SYSKD = angebotSearchData.SYSKD;
			this.IsActive = angebotSearchData.IsActive;
            this.AngebotState = angebotSearchData.AngebotState;
			this.SYSVTTYPArray = angebotSearchData.SYSVTTYPArray;
			this.FreeText = angebotSearchData.FreeText;
			this.ANGEBOT1 = angebotSearchData.ANGEBOT1;
			this.PersonItCODE = angebotSearchData.PersonItCODE;
			this.PersonItNAME = angebotSearchData.PersonItNAME;
			this.PersonItVORNAME = angebotSearchData.PersonItVORNAME;
		}

		// TEST BK 0 BK, Not tested 
		/// <summary>
		/// Setzt alle Werte zurück.
		/// </summary>
		public void ResetValues()
		{
			// Adopt
			this.AdoptValues(new Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTSearchData());
		}
		#endregion
		
		#region Properties
        /// <summary>
		/// Kunden-ID (IT)
		/// Werte gleich null werden bei der Suche ignoriert.
        /// </summary>
		[System.Runtime.Serialization.DataMember]
		public long? SYSIT
		{
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}

        /// <summary>
		/// Kunden-ID (PERSON)
		/// Werte gleich null werden bei der Suche ignoriert.
        /// </summary>
		[System.Runtime.Serialization.DataMember]
		public long? SYSKD
		{
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}

        /// <summary>
		/// Status, Aktiv
		/// Werte gleich null werden bei der Suche ignoriert.
        /// </summary>
		[System.Runtime.Serialization.DataMember]
		public bool? IsActive
		{
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}

        /// <summary>
        /// Angebot Status
        /// <see cref="Cic.OpenLease.Model.DdOl.ANGEBOT.AngebotStatuses"/>
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public Cic.OpenLease.Model.DdOl.ANGEBOT.AngebotStatuses? AngebotState
        {
            // TEST BK 0 BK, Not tested
            get;
            // TEST BK 0 BK, Not tested
            set;
        }

        /// <summary>
		/// Vertragstyp-IDs
        /// </summary>
		[System.Runtime.Serialization.DataMember]
		public long[] SYSVTTYPArray
		{
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}

        /// <summary>
        /// Noch nicht in Verwendung.
        /// </summary>
		[System.Runtime.Serialization.DataMember]
        public string FreeText
        {
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}

        /// <summary>
        /// Code
		/// Werte gleich null werden bei der Suche ignoriert.
		/// </summary>
        [System.Runtime.Serialization.DataMember]
        public string ANGEBOT1
        {
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}

        /// <summary>
		/// Kunde (IT/PERSON), Code
		/// Werte gleich null werden bei der Suche ignoriert.
        /// Zurzeit nur suchen in IT
		/// </summary>
		[System.Runtime.Serialization.DataMember]
		public string PersonItCODE
		{
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}

        /// <summary>
		/// Kunde (IT/PERSON), Name
		/// Werte gleich null werden bei der Suche ignoriert.
        /// Zurzeit nur suchen in IT
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public string PersonItNAME
        {
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}

        /// <summary>
		/// Kunde (IT/PERSON), Vorname
		/// Werte gleich null werden bei der Suche ignoriert.
        /// Zurzeit nur suchen in IT
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public string PersonItVORNAME
        {
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}
		#endregion
    }
}
