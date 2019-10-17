// OWNER BK, 15-06-2009
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
	/// Suchwerte für Interessenten
    /// </summary>
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract(Namespace = "http://cic-software.de/datacontract")]
    public class ITSearchData
	{
		#region Methods
		// TEST BK 0 BK, Not tested 
		/// <summary>
		/// Übernimmt die Werte einer anderen Instanz.
		/// </summary>
		public void AdoptValues(Cic.OpenLease.ServiceAccess.DdOl.ITSearchData itSearchData)
		{
			// Check object
			if (itSearchData == null)
			{
				// Throw 
				throw new Cic.Basic.ApplicationArgumentIsNull("itSearchData");
			}

			// Set data
			this.IsPrivate = itSearchData.IsPrivate;
			this.FreeText = itSearchData.FreeText;
			this.CODE = itSearchData.CODE;
			this.NAME = itSearchData.NAME;
			this.VORNAME = itSearchData.VORNAME;
			this.ZUSATZ = itSearchData.ZUSATZ;
			this.STRASSE = itSearchData.STRASSE;
			this.HSNR = itSearchData.HSNR;
			this.PLZ = itSearchData.PLZ;
			this.ORT = itSearchData.ORT;
		}

		// TEST BK 0 BK, Not tested 
		/// <summary>
		/// Setzt alle Werte zurück.
		/// </summary>
		public void ResetValues()
		{
			// Adopt
			this.AdoptValues(new Cic.OpenLease.ServiceAccess.DdOl.ITSearchData());
		}
		#endregion

		#region Properties
        /// <summary>
        /// Status, Privat
        /// Werte gleich null werden bei der Suche ignoriert.
        /// </summary>
		[System.Runtime.Serialization.DataMember]
		public bool? IsPrivate
		{
			// TESTED BY ITSearchDataTestFixture.CheckProperties
			get;
			// TESTED BY ITSearchDataTestFixture.CheckProperties
			set;
		}

        /// <summary>
        /// Noch nicht in Verwendung.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public string FreeText
        {
			// TESTED BY ITSearchDataTestFixture.CheckProperties
			get;
			// TESTED BY ITSearchDataTestFixture.CheckProperties
			set;
        }

        /// <summary>
        /// Code
		/// Werte gleich null werden bei der Suche ignoriert.
		/// </summary>
        [System.Runtime.Serialization.DataMember]
        public string CODE
        {
			// TESTED BY ITSearchDataTestFixture.CheckProperties
			get;
			// TESTED BY ITSearchDataTestFixture.CheckProperties
			set;
        }

        /// <summary>
        /// Name
		/// Werte gleich null werden bei der Suche ignoriert.
		/// </summary>
        [System.Runtime.Serialization.DataMember]
        public string NAME
        {
			// TESTED BY ITSearchDataTestFixture.CheckProperties
			get;
			// TESTED BY ITSearchDataTestFixture.CheckProperties
			set;
        }

        /// <summary>
        /// Vorname
		/// Werte gleich null werden bei der Suche ignoriert.
		/// </summary>
        [System.Runtime.Serialization.DataMember]
        public string VORNAME
        {
			// TESTED BY ITSearchDataTestFixture.CheckProperties
			get;
			// TESTED BY ITSearchDataTestFixture.CheckProperties
			set;
        }

        /// <summary>
        /// Namenszusatz
		/// Werte gleich null werden bei der Suche ignoriert.
		/// </summary>
		[System.Runtime.Serialization.DataMember]
		public string ZUSATZ
		{
			// TESTED BY ITSearchDataTestFixture.CheckProperties
			get;
			// TESTED BY ITSearchDataTestFixture.CheckProperties
			set;
		}

        /// <summary>
        /// Strasse
		/// Werte gleich null werden bei der Suche ignoriert.
		/// </summary>
        [System.Runtime.Serialization.DataMember]
        public string STRASSE
        {
			// TESTED BY ITSearchDataTestFixture.CheckProperties
			get;
			// TESTED BY ITSearchDataTestFixture.CheckProperties
			set;
        }

        /// <summary>
        /// Hausnummer
		/// Werte gleich null werden bei der Suche ignoriert.
		/// </summary>
		[System.Runtime.Serialization.DataMember]
        public string HSNR
        {
			// TESTED BY ITSearchDataTestFixture.CheckProperties
			get;
			// TESTED BY ITSearchDataTestFixture.CheckProperties
			set;
        }

        /// <summary>
        /// Postleitzahl
		/// Werte gleich null werden bei der Suche ignoriert.
		/// </summary>
        [System.Runtime.Serialization.DataMember]
        public string PLZ
        {
			// TESTED BY ITSearchDataTestFixture.CheckProperties
			get;
			// TESTED BY ITSearchDataTestFixture.CheckProperties
			set;
        }

        /// <summary>
        /// Ort
		/// Werte gleich null werden bei der Suche ignoriert.
		/// </summary>
        [System.Runtime.Serialization.DataMember]
        public string ORT
        {
			// TESTED BY ITSearchDataTestFixture.CheckProperties
			get;
			// TESTED BY ITSearchDataTestFixture.CheckProperties
			set;
        }
        #endregion
    }
}
