// OWNER MK, 05-02-2009
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
	/// Dienstvertrag für Datenzugriffsobjekte für Kontodaten
    /// </summary>
    [System.CLSCompliant(true)]
    [System.ServiceModel.ServiceContract(Name = "KONTODaoContract", Namespace = "http://cic-software.de/contract")]
    public interface IKONTODao
    {
        #region Methods
        /// <summary>
		/// Gibt eine Liste von Datentransferobjekten zurück, die den angegebenen Suchwerten und Suchparametern entsprechen.
        /// </summary>
        /// <param name="kontoSearchData">Suchwerte <see cref="Cic.OpenLease.ServiceAccess.DdOl.KONTOSearchData"/>.</param>
        /// <param name="searchParameters">Suchparameter <see cref="Cic.OpenLease.ServiceAccess.SearchParameters" />.</param>
        /// <returns>Liste von <see cref="Cic.OpenLease.ServiceAccess.DdOl.KONTODto"/></returns>
        [System.ServiceModel.OperationContract]
		System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.DdOl.KONTODto> Search(Cic.OpenLease.ServiceAccess.DdOl.KONTOSearchData kontoSearchData, Cic.OpenLease.ServiceAccess.SearchParameters searchParameters);

        /// <summary>
		/// Die Methode gibt die Anzahl der gefundenen Sätze zurück, die den angegebenen Suchwerten entsprechen.
        /// </summary>
        /// <param name="kontoSearchData">Suchwerte <see cref="Cic.OpenLease.ServiceAccess.DdOl.KONTOSearchData"/>.</param>
        /// <returns>Anzahl der gefundenen Sätze</returns>
		[System.ServiceModel.OperationContract]
		int SearchCount(Cic.OpenLease.ServiceAccess.DdOl.KONTOSearchData kontoSearchData);
		#endregion
    }
}
