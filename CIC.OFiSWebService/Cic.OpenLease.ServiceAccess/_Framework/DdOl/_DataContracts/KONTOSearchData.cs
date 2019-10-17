﻿// OWNER MK, 05-02-2009
using System;
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
	/// Suchwerte für Kontodaten
    /// </summary>
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract(Namespace = "http://cic-software.de/datacontract")]
    public class KONTOSearchData
    {
		#region Methods
		// TEST BK 0 BK, Not tested 
		/// <summary>
		/// Übernimmt die Werte einer anderen Instanz.
		/// </summary>
		public void AdoptValues(Cic.OpenLease.ServiceAccess.DdOl.KONTOSearchData kontoSearchData)
		{
			// Check object
			if (kontoSearchData == null)
			{
				// Throw 
                throw new Exception("kontoSearchData");
			}

			// Set data
			this.SYSPERSON = kontoSearchData.SYSPERSON;
			this.IsActive = kontoSearchData.IsActive;
		}

		// TEST BK 0 BK, Not tested 
		/// <summary>
		/// Setzt alle Werte zurück.
		/// </summary>
		public void ResetValues()
		{
			// Adopt
			this.AdoptValues(new Cic.OpenLease.ServiceAccess.DdOl.KONTOSearchData());
		}
		#endregion
		
		#region Properties
        /// <summary>
        /// Personen-ID
        /// Werte gleich null werden bei der Suche ignoriert.
        /// </summary>
		[System.Runtime.Serialization.DataMember]
        public long? SYSPERSON
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
		#endregion
    }
}
