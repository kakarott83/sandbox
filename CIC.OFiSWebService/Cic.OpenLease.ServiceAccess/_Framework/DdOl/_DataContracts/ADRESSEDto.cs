// OWNER BK, 05-03-2009
using System;
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
    /// Datentransferobjekt für Adressen
    /// </summary>
    [System.CLSCompliant(true)]
    [System.Runtime.Serialization.DataContract(Namespace = "http://cic-software.de/datacontract")]
    public class ADRESSEDto
    {
        #region Constructors
		// TEST BK 0 BK, Not tested
		/// <summary>
		/// Konstruktor, Grundbestandteil
		/// </summary>
		/// <param name="adresse"><see cref="Cic.OpenLease.Model.DdOl.ADRESSE"/></param>
		public ADRESSEDto(Cic.OpenLease.Model.DdOl.ADRESSE adresse)
        {
			// Check object
			if (adresse == null)
            {
                throw new Exception("adresse");
            }

			// Set properties
			this.ADRESSE = adresse;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Grundbestandteil <see cref="Cic.OpenLease.Model.DdOl.ADRESSE"/>
        /// </summary>
        [System.Runtime.Serialization.DataMember]
		public Cic.OpenLease.Model.DdOl.ADRESSE ADRESSE
        {
			// TEST BK 0 BK, Not tested
			get;
			// TEST BK 0 BK, Not tested
			set;
        }
        #endregion
    }
}
