// OWNER MK, 25-02-2009
using System;
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
	/// Suchwerte für Personen
    /// </summary>
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract(Namespace = "http://cic-software.de/datacontract")]
    public class PERSONSearchData
    {
		#region Methods
		// TEST BK 0 BK, Not tested 
		/// <summary>
		/// Übernimmt die Werte einer anderen Instanz.
		/// </summary>
		public void AdoptValues(Cic.OpenLease.ServiceAccess.DdOl.PERSONSearchData personSearchData)
		{
			// Check object
			if (personSearchData == null)
			{
				// Throw 
                throw new Exception("personSearchData");
			}

			// Set data
			this.IsPrivate = personSearchData.IsPrivate;
			this.IsActive = personSearchData.IsActive;
			this.IsCustomer = personSearchData.IsCustomer;
			this.FreeText = personSearchData.FreeText;
			this.CODE = personSearchData.CODE;
			this.NAME = personSearchData.NAME;
			this.VORNAME = personSearchData.VORNAME;
			this.STRASSE = personSearchData.STRASSE;
			this.PLZ = personSearchData.PLZ;
			this.ORT = personSearchData.ORT;
		}

		// TEST BK 0 BK, Not tested 
		/// <summary>
		/// Setzt alle Werte zurück.
		/// </summary>
		public void ResetValues()
		{
			// Adopt
			this.AdoptValues(new Cic.OpenLease.ServiceAccess.DdOl.PERSONSearchData());
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
        /// Status, Kunde
        /// Werte gleich null werden bei der Suche ignoriert.
        /// </summary>
		[System.Runtime.Serialization.DataMember]
		public bool? IsCustomer
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
		public string CODE
		{
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}

        /// <summary>
        /// Name
		/// Werte gleich null werden bei der Suche ignoriert.
		/// </summary>
        [System.Runtime.Serialization.DataMember]
        public string NAME
        {
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}

        /// <summary>
        /// Vorname
		/// Werte gleich null werden bei der Suche ignoriert.
		/// </summary>
        [System.Runtime.Serialization.DataMember]
        public string VORNAME
        {
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}

        /// <summary>
        /// Strasse
		/// Werte gleich null werden bei der Suche ignoriert.
		/// </summary>
		[System.Runtime.Serialization.DataMember]
		public string STRASSE
		{
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}

        /// <summary>
        /// Postleitzahl
		/// Werte gleich null werden bei der Suche ignoriert.
		/// </summary>
        [System.Runtime.Serialization.DataMember]
        public string PLZ
        {
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}

        /// <summary>
        /// Ort
		/// Werte gleich null werden bei der Suche ignoriert.
		/// </summary>
        [System.Runtime.Serialization.DataMember]
        public string ORT
        {
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}
		#endregion
    }
}
