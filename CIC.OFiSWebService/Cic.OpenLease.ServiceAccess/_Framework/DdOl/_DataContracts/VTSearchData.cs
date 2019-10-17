// OWNER MK, 05-03-2009
using System;
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
	/// Suchwerte für Verträge
    /// </summary>
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract(Namespace = "http://cic-software.de/datacontract")]
    public class VTSearchData
    {
		#region Methods
		// TEST BK 0 BK, Not tested 
		/// <summary>
		/// Übernimmt die Werte einer anderen Instanz.
		/// </summary>
		public void AdoptValues(Cic.OpenLease.ServiceAccess.DdOl.VTSearchData vtSearchData)
		{
			// Check object
			if (vtSearchData == null)
			{
				// Throw 
				throw new Exception("vtSearchData");
			}

			// Set data
			
			this.VERTRAG = vtSearchData.VERTRAG;
			this.Person = vtSearchData.Person;
            this.Obhalter = vtSearchData.Obhalter;
            this.Verkaeufer = vtSearchData.Verkaeufer;
			
		}

		// TEST BK 0 BK, Not tested 
		/// <summary>
		/// Setzt alle Werte zurück.
		/// </summary>
		public void ResetValues()
		{
			// Adopt
			this.AdoptValues(new Cic.OpenLease.ServiceAccess.DdOl.VTSearchData());
		}
		#endregion
		
		#region Properties
        
        /// <summary>
        /// Code
		/// Werte gleich null werden bei der Suche ignoriert.
		/// </summary>
        [System.Runtime.Serialization.DataMember]
        public string VERTRAG
        {
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}

        /// <summary>
        /// Kunde (PERSON), Code
		/// Werte gleich null werden bei der Suche ignoriert.
        /// Zurzeit nur suchen in PERSON
		/// </summary>
		[System.Runtime.Serialization.DataMember]
		public string Person
		{
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}

        [System.Runtime.Serialization.DataMember]
        public string Obhalter
        {
            // TEST BK 0 BK, Not tested
            get;
            // TEST BK 0 BK, Not tested
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public string Verkaeufer
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
