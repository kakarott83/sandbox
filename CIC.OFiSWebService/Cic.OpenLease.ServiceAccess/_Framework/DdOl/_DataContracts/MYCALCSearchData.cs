// OWNER MK, 05-03-2009
using System;
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
	/// Suchwerte für Schnellkalkulationen
    /// </summary>
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract(Namespace = "http://cic-software.de/datacontract")]
    public class MYCALCSearchData
    {
		#region Methods
		// TEST BK 0 BK, Not tested 
		/// <summary>
		/// Übernimmt die Werte einer anderen Instanz.
		/// </summary>
		public void AdoptValues(Cic.OpenLease.ServiceAccess.DdOl.MYCALCSearchData mycalcSearchData)
		{
			// Check object
			if (mycalcSearchData == null)
			{
				// Throw 
                throw new Exception("mycalcSearchData");
			}

			// Set data
			this.SYSPERSON = mycalcSearchData.SYSPERSON;
			this.SYSVTTYPArray = mycalcSearchData.SYSVTTYPArray;
			this.FreeText = mycalcSearchData.FreeText;
			this.BEMERKUNG1 = mycalcSearchData.BEMERKUNG1;
			this.CustomerCode = mycalcSearchData.CustomerCode;
			this.NAME = mycalcSearchData.NAME;
			this.VORNAME = mycalcSearchData.VORNAME;
		}

		// TEST BK 0 BK, Not tested 
		/// <summary>
		/// Setzt alle Werte zurück.
		/// </summary>
		public void ResetValues()
		{
			// Adopt
			this.AdoptValues(new Cic.OpenLease.ServiceAccess.DdOl.MYCALCSearchData());
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
        /// Bemerkung 1
		/// Werte gleich null werden bei der Suche ignoriert.
		/// </summary>
        [System.Runtime.Serialization.DataMember]
        public string BEMERKUNG1
        {
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}

        /// <summary>
        /// Kundencode
		/// Werte gleich null werden bei der Suche ignoriert.
		/// </summary>
		[System.Runtime.Serialization.DataMember]
		public string CustomerCode
		{
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}

        /// <summary>
        /// Kundenname
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
        /// Kundenvorname
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
        #endregion
    }
}
