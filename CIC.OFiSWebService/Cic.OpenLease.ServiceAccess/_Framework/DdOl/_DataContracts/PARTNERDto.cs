// OWNER BK, 05-03-2009
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
    /// Datentransferobjekt für Ansprechpartner
    /// </summary>
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract(Namespace = "http://cic-software.de/datacontract")]
    public class PARTNERDto
    {
        #region Constructors
		/// <summary>
		/// Konstruktor, ohne Bestandteile
		/// </summary>
		public PARTNERDto()
            : this(null, null)
        {
        }

		// TEST BK 0 BK, Not tested
		/// <summary>
		/// Konstruktor, Grundbestandteil
		/// </summary>
		public PARTNERDto(Cic.OpenLease.Model.DdOl.PARTNER partner)
			: this(partner, null)
		{
		}

		// TEST BK 0 BK, Not tested
		/// <summary>
		/// Konstruktor, alle Bestandteile
		/// </summary>
		public PARTNERDto(Cic.OpenLease.Model.DdOl.PARTNER partner, Cic.OpenLease.Model.DdOl.LAND land)
        {
			// Set properties
			this.PARTNER = partner;
			this.LAND = land;
		}
        #endregion

        #region Properties
        /// <summary>
        /// Grundbestandteil <see cref="Cic.OpenLease.Model.DdOl.PARTNER"/>.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
		public Cic.OpenLease.Model.DdOl.PARTNER PARTNER
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
		#endregion
    }
}
