// OWNER MK, 05-02-2009
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
	/// Dienstvertrag für Datenzugriffsobjekte für Ansprechpartner
    /// </summary>
    [System.CLSCompliant(true)]
    [System.ServiceModel.ServiceContract(Name = "PARTNERDaoContract", Namespace = "http://cic-software.de/contract")]
    public interface IPARTNERDao
    {
        #region Methods
        /// <summary>
		/// Gibt eine Liste von Datentransferobjekten zurück, die den angegebenen Suchwerten und Suchparametern entsprechen.
        /// </summary>
        /// <param name="partnerSearchData">Suchwerte <see cref="Cic.OpenLease.ServiceAccess.DdOl.PARTNERSearchData"/>.</param>
        /// <param name="searchParameters">Suchparameter <see cref="Cic.OpenLease.ServiceAccess.SearchParameters" />.</param>
        /// <returns>List von <see cref="Cic.OpenLease.ServiceAccess.DdOl.PARTNERDto"/></returns>
        [System.ServiceModel.OperationContract]
		System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.DdOl.PARTNERDto> Search(Cic.OpenLease.ServiceAccess.DdOl.PARTNERSearchData partnerSearchData, Cic.OpenLease.ServiceAccess.SearchParameters searchParameters);

        /// <summary>
		/// Die Methode gibt die Anzahl der gefundenen Sätze zurück, die den angegebenen Suchwerten entsprechen.
        /// </summary>
        /// <param name="partnerSearchData">Suchwerte <see cref="Cic.OpenLease.ServiceAccess.DdOl.PARTNERSearchData"/>.</param>
        /// <returns>Anzahl der gefundenen Sätze</returns>
		[System.ServiceModel.OperationContract]
		int SearchCount(Cic.OpenLease.ServiceAccess.DdOl.PARTNERSearchData partnerSearchData);

        /// <summary>
		/// Die Methode liefert, falls vorhanden, ein Datentransferobjekt mit der angegebenen ID.
		/// </summary>
        /// <param name="sysPARTNER">ID des Objektes</param>
        /// <returns><see cref="Cic.OpenLease.ServiceAccess.DdOl.PARTNERDto"/></returns>
		[System.ServiceModel.OperationContract]
		Cic.OpenLease.ServiceAccess.DdOl.PARTNERDto Deliver(long sysPARTNER);
		#endregion
    }
}
