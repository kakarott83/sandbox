// OWNER BK, 03-06-2009
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
    /// Datentransferobjekt für Rechnungen
    /// </summary>
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract(Namespace = "http://cic-software.de/datacontract")]
    public class RNDto
    {
		#region Constructors
        // TEST BK 0 BK, Not tested
        public RNDto()               
        {
        }

		// TEST BK 0 BK, Not tested
		/// <summary>
		/// Konstruktor, Grundbestandteil
		/// </summary>
		public RNDto(Cic.OpenLease.Model.DdOl.RN rn)
            : this(rn, null)
        {
        }

		// TEST BK 0 BK, Not tested
		/// <summary>
		/// Konstruktor, alle Bestandteile
		/// </summary>
		public RNDto(Cic.OpenLease.Model.DdOl.RN rn, double? outstandingItems)
        {
			// Set properties
			this.RN = rn;
            this.OutstandingItems = outstandingItems;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Grundbestandteil <see cref="Cic.OpenLease.Model.DdOl.RN"/>.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public Cic.OpenLease.Model.DdOl.RN RN
        {
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}

		// TODO BK 9 BK, Add summary
        /// <summary>
        /// 
        /// </summary>
		[System.Runtime.Serialization.DataMember]
		public double? OutstandingItems  
		{
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}
        #endregion
    }
}
