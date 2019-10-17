// OWNER MK, 05-03-2009
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
	/// Datentransferobjekt für Schnellkalkulationen
    /// </summary>
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract(Namespace = "http://cic-software.de/datacontract")]
    public class MYCALCDto
    {
		#region Constructors
		// TEST BK 0 BK, Not tested
		/// <summary>
		/// Konstruktor, Grundbestandteil
		/// </summary>
		public MYCALCDto(Cic.OpenLease.Model.DdOl.MYCALC mycalc)
        {
            MYCALC = mycalc;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Grundbestandteil <see cref="Cic.OpenLease.Model.DdOl.MYCALC"/>.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public Cic.OpenLease.Model.DdOl.MYCALC MYCALC
        {
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
		}
        #endregion
    }
}
