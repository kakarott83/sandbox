// OWNER MK, 05-03-2009
using System;
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
	/// Suchwerte für Anträge
    /// </summary>
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract(Namespace = "http://cic-software.de/datacontract")]
    public class ANTRAGSearchData
    {
		#region Methods
		// TEST BK 0 BK, Not tested 
		/// <summary>
		/// Übernimmt die Werte einer anderen Instanz.
		/// </summary>
		public void AdoptValues(Cic.OpenLease.ServiceAccess.DdOl.ANTRAGSearchData antragSearchData)
		{
			// Check object
			if (antragSearchData == null)
			{
				// Throw 
                throw new Exception("antragSearchData");
			}

			// Set data
			
			this.SYSKD = antragSearchData.SYSKD;		
			this.ANTRAG1 = antragSearchData.ANTRAG1;
			this.Person = antragSearchData.Person;
			this.Verkaufer = antragSearchData.Verkaufer;
		}

		// TEST BK 0 BK, Not tested 
		/// <summary>
		/// Setzt alle Werte zurück.
		/// </summary>
		public void ResetValues()
		{
			// Adopt
			this.AdoptValues(new Cic.OpenLease.ServiceAccess.DdOl.ANTRAGSearchData());
		}
		#endregion
		
		#region Properties
        

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
        /// Code
		/// Werte gleich null werden bei der Suche ignoriert.
		/// </summary>
        [System.Runtime.Serialization.DataMember]
        public string ANTRAG1
        {
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}

        /// <summary>
		/// Kunden (PERSON), Code	
        /// </summary>
		[System.Runtime.Serialization.DataMember]
		public string Person
		{
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}


        /// <summary>
        /// Kunden (VERKAUFER), Code	
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public string Verkaufer
        {
            // TEST BK 0 BK, Not tested
            get;
            // TEST BK 0 BK, Not tested
            set;
        }

        /// <summary>
        /// Kunde (IT/PERSON)
        /// Werte gleich null werden bei der Suche ignoriert.
        /// Zurzeit nur suchen in IT
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public long? SYSIT
        {
            get;
            set;
        }


		#endregion
    }
}
