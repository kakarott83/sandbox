// OWNER BK, 05-03-2009
using System;
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
    /// Datentransferobjekt für Kontodaten
    /// </summary>
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract(Namespace = "http://cic-software.de/datacontract")]
    public class KONTODto
    {
        #region Constructors
		// TEST BK 0 BK, Not tested
		/// <summary>
		/// Konstruktor, Grundbestandteil
		/// </summary>
		public KONTODto(Cic.OpenLease.Model.DdOl.KONTO konto)
			: this(konto, null, null)
        {
        }

		// TEST BK 0 BK, Not tested
		/// <summary>
		/// Konstruktor, alle Bestandteile
		/// </summary>
		public KONTODto(Cic.OpenLease.Model.DdOl.KONTO konto, Cic.OpenLease.Model.DdOl.PERSON person, Cic.OpenLease.Model.DdOl.BLZ blz)
        {
			// Check object
			if (konto == null)
            {
                throw new Exception("konto");
            }

			// Set properties
			this.KONTO = konto;
			this.BLZ = blz;
            this.PERSON = person;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Grundbestandteil <see cref="Cic.OpenLease.Model.DdOl.KONTO"/>.
        /// </summary>
        [System.Runtime.Serialization.DataMember]
		public Cic.OpenLease.Model.DdOl.KONTO KONTO
        {
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
        }

        /// <summary>
        /// Bestandteil <see cref="Cic.OpenLease.Model.DdOl.BLZ"/>.
        /// Bankdaten
        /// </summary>
        [System.Runtime.Serialization.DataMember]
		public Cic.OpenLease.Model.DdOl.BLZ BLZ
        {
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
        }

        /// <summary>
        /// Bestandteil <see cref="Cic.OpenLease.Model.DdOl.PERSON"/>.
        /// Kontoinhaber
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public Cic.OpenLease.Model.DdOl.PERSON PERSON
        {
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
        }
        #endregion
    }
}
