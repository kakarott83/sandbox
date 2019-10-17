// OWNER BK, 03-06-2009
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
	/// Dienstvertrag für Datenzugriffsobjekte für Rechnungen
    /// </summary>
    [System.CLSCompliant(true)]
    [System.ServiceModel.ServiceContract(Name = "RNDaoContract", Namespace = "http://cic-software.de/contract")]
    public interface IRNDao
    {
        #region Methods
        /// <summary>
		/// Gibt eine Liste von Datentransferobjekten zurück, die den angegebenen Suchwerten und Suchparametern entsprechen.
        /// </summary>
        /// <param name="rnSearchData">Suchwerte <see cref="Cic.OpenLease.ServiceAccess.DdOl.RNSearchData"/>.</param>
        /// <param name="searchParameters">Suchparameter <see cref="Cic.OpenLease.ServiceAccess.SearchParameters" />.</param>
        /// <returns>List von <see cref="Cic.OpenLease.ServiceAccess.DdOl.RNDto"/></returns>
        [System.ServiceModel.OperationContract]
        System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.DdOl.RNDto> Search(Cic.OpenLease.ServiceAccess.DdOl.RNSearchData rnSearchData, Cic.OpenLease.ServiceAccess.SearchParameters searchParameters);

        /// <summary>
		/// Die Methode gibt die Anzahl der gefundenen Sätze zurück, die den angegebenen Suchwerten entsprechen.
        /// </summary>
        /// <param name="rnSearchData">Suchwerte <see cref="Cic.OpenLease.ServiceAccess.DdOl.RNSearchData"/>.</param>
        /// <returns>Anzahl der gefundenen Sätze</returns>
        [System.ServiceModel.OperationContract]
        int SearchCount(Cic.OpenLease.ServiceAccess.DdOl.RNSearchData rnSearchData);
        #endregion
    }
}