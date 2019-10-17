// OWNER BK, 03-06-2009
using System;
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
	/// Suchwerte für Rechnungen
    /// </summary>
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract(Namespace = "http://cic-software.de/datacontract")]
    public class RNSearchData
    {
		#region Methods
		// TEST BK 0 BK, Not tested 
		/// <summary>
		/// Übernimmt die Werte einer anderen Instanz.
		/// </summary>
		public void AdoptValues(Cic.OpenLease.ServiceAccess.DdOl.RNSearchData rnSearchData)
		{
			// Check object
			if (rnSearchData == null)
			{
				// Throw 
				throw new Exception("rnSearchData");
			}

			// Set data
			this.SYSPERSON = rnSearchData.SYSPERSON;
			this.RNSearchArtConstant = rnSearchData.RNSearchArtConstant;
			this.RNSearchTypeConstant = rnSearchData.RNSearchTypeConstant;
			this.MinPaymentDate = rnSearchData.MinPaymentDate;
			this.MaxPaymentDate = rnSearchData.MaxPaymentDate;
			this.RNPaidConstant = rnSearchData.RNPaidConstant;
		}

		// TEST BK 0 BK, Not tested 
		/// <summary>
		/// Setzt alle Werte zurück.
		/// </summary>
		public void ResetValues()
		{
			// Adopt
			this.AdoptValues(new Cic.OpenLease.ServiceAccess.DdOl.RNSearchData());
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

		// TODO BK 9 BK, Check summary
        /// <summary>
        /// All = 0
        /// OutgoingMandantorIncomingCustomer = 1
        /// IncomingMandantorOutgoingCustomer = 2
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public Cic.OpenLease.Model.RNSearchArtConstants? RNSearchArtConstant
        {
            // TEST BK 0 BK, Not tested
            get;
            // TEST BK 0 BK, Not tested
            set;
        }

		// TODO BK 9 BK, Check summary
		/// <summary>
        /// All = 0
        /// WithoutReference = 1
        /// OnlyContractReference = 2
        /// ContractAndObjectReferences = 3,
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public Cic.OpenLease.Model.RNSearchTypeConstants? RNSearchTypeConstant
        {
            // TEST BK 0 BK, Not tested
            get;
            // TEST BK 0 BK, Not tested
            set;
        }

		// TODO BK 9 BK, Check summary
		/// <summary>
        /// 
        /// </summary>
		[System.Runtime.Serialization.DataMember]
        public System.DateTime? MinPaymentDate
		{
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}

		// TODO BK 9 BK, Check summary
		/// <summary>
        /// 
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public System.DateTime? MaxPaymentDate
        {
            // TEST BK 0 BK, Not tested
            get;
            // TEST BK 0 BK, Not tested
            set;
        }

		// TODO BK 9 BK, Check summary
		/// <summary>
        /// All = 0
        /// Paid = 1
        /// NotPaid = 2
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public Cic.OpenLease.Model.RNPaidConstants? RNPaidConstant
        {
            // TEST BK 0 BK, Not tested
            get;
            // TEST BK 0 BK, Not tested
            set;
        }
        #endregion
    }
}
