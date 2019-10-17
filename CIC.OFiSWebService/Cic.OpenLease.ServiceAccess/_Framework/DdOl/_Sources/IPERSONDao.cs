// OWNER MK, 25-02-2009
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
	/// Dienstvertrag für Datenzugriffsobjekte für Personen
    /// </summary>
    [System.CLSCompliant(true)]
    [System.ServiceModel.ServiceContract(Name = "PERSONDaoContract", Namespace = "http://cic-software.de/contract")]
    public interface IPERSONDao
    {
        #region Methods
        /// <summary>
		/// Gibt eine Liste von Datentransferobjekten zurück, die den angegebenen Suchwerten und Suchparametern entsprechen.
		/// Das Gesichtsfeld kann berücksichtigt werden.
		/// </summary>
        /// <param name="personSearchData">Suchwerte <see cref="Cic.OpenLease.ServiceAccess.DdOl.PERSONSearchData"/>.</param>
        /// <param name="searchParameters">Suchparameter <see cref="Cic.OpenLease.ServiceAccess.SearchParameters" />.</param>
        /// <param name="personSortData">Sortparametern <see cref="Cic.OpenLease.ServiceAccess.DdOl.PERSONSortData" />.</param>
        /// <returns>List von <see cref="Cic.OpenLease.ServiceAccess.DdOl.PERSONDto"/></returns>
        [System.ServiceModel.OperationContract]
        System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.DdOl.PERSONDto> Search(Cic.OpenLease.ServiceAccess.DdOl.PERSONSearchData personSearchData, Cic.OpenLease.ServiceAccess.SearchParameters searchParameters, PERSONSortData[] personSortData);

        /// <summary>
		/// Die Methode gibt die Anzahl der gefundenen Sätze zurück, die den angegebenen Suchwerten entsprechen.
		/// Das Gesichtsfeld kann berücksichtigt werden.
		/// </summary>
        /// <param name="personSearchData">Suchwerte <see cref="Cic.OpenLease.ServiceAccess.DdOl.PERSONSearchData"/>.</param>
        /// <returns>Anzahl der gefundenen Sätze</returns>
		[System.ServiceModel.OperationContract]
        int SearchCount(Cic.OpenLease.ServiceAccess.DdOl.PERSONSearchData personSearchData);

        /// <summary>
		/// Die Methode liefert, falls vorhanden, ein Datentransferobjekt mit der angegebenen ID.
		/// Das Gesichtsfeld kann berücksichtigt werden.
		/// </summary>
        /// <param name="sysPERSON">ID des Objektes</param>
		/// <returns><see cref="Cic.OpenLease.ServiceAccess.DdOl.PERSONDto"/></returns>
		[System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.PERSONDto Deliver(long sysPERSON);

        /// <summary>
		/// Speichert die Daten aus dem Datentransferobjekt in die Datenbank (INSERT oder UPDATE).
		/// Das Gesichtsfeld kann berücksichtigt werden.
		/// </summary>
        /// <param name="personDto"><see cref="Cic.OpenLease.ServiceAccess.DdOl.PERSONDto"/></param>
        /// <returns><see cref="Cic.OpenLease.ServiceAccess.DdOl.PERSONDto"/></returns>
		[System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.PERSONDto Save(Cic.OpenLease.ServiceAccess.DdOl.PERSONDto personDto);
        #endregion
    }
}