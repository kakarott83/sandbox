// OWNER MK,26-08-2009
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
    /// Dienstvertrag für Datenzugriffsobjekte für Angebot Kalkulationen
    /// MK
    /// </summary>
    [System.CLSCompliant(true)]
    [System.ServiceModel.ServiceContract(Name = "ANGKALKDaoContract", Namespace = "http://cic-software.de/contract")]
    public interface IANGKALKDao
    {
        #region Methods
        /// <summary>
        /// Gibt eine Liste von Datentransferobjekten zurück, die den angegebenen Suchwerten und Suchparametern entsprechen.
        /// MK
        /// </summary>
        /// <param name="angKalkSearchData">Suchwerte <see cref="Cic.OpenLease.ServiceAccess.DdOl.ANGKALKSearchData"/>.</param>
        /// <param name="searchParameters">Suchparameter <see cref="Cic.OpenLease.ServiceAccess.SearchParameters" />.</param>
        /// <returns>Liste von <see cref="Cic.OpenLease.ServiceAccess.DdOl.ANGKALKDto"/></returns>
        [System.ServiceModel.OperationContract]
        System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.DdOl.ANGKALKDto> Search(Cic.OpenLease.ServiceAccess.DdOl.ANGKALKSearchData angKalkSearchData, Cic.OpenLease.ServiceAccess.SearchParameters searchParameters);

        /// <summary>
        /// Die Methode gibt die Anzahl der gefundenen Sätze zurück, die den angegebenen Suchwerten entsprechen.
        /// Das Gesichtsfeld kann berücksichtigt werden.
        /// </summary>
        /// <param name="angebotSearchData">Suchwerte <see cref="Cic.OpenLease.ServiceAccess.DdOl.ANGKALKSearchData"/>.</param>
        /// <returns>Anzahl der gefundenen Sätze</returns>
        [System.ServiceModel.OperationContract]
        int SearchCount(Cic.OpenLease.ServiceAccess.DdOl.ANGKALKSearchData angebotSearchData);

        /// <summary>
        /// Die Methode liefert, falls vorhanden, ein Datentransferobjekt mit der angegebenen ID.
        /// </summary>
        /// <param name="sysID">ID des Objektes</param>
        /// <returns><see cref="Cic.OpenLease.ServiceAccess.DdOl.ANGKALKDto"/></returns>
        [System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.ANGKALKDto Deliver(long sysID);

        /// <summary>
        /// Speichert die Daten aus dem Datentransferobjekt in die Datenbank (INSERT oder UPDATE).
        /// </summary>
        /// <param name="angKalkDto"><see cref="Cic.OpenLease.ServiceAccess.DdOl.ANGKALKDto"/></param>
        /// <returns><see cref="Cic.OpenLease.ServiceAccess.DdOl.ANGKALKDto"/></returns>
        [System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.ANGKALKDto Save(Cic.OpenLease.ServiceAccess.DdOl.ANGKALKDto angKalkDto);

        /// <summary>
        /// Die Methode löscht die Daten (aus dem Datentransferobjekt) aus der Datenbank.
        /// Mit Benutzer Validierung (RFG.LÖSCHEN erforderlich).        
        /// MK
        /// </summary>
        /// <param name="angKalkDto"><see cref="Cic.OpenLease.ServiceAccess.DdOl.ANGKALKDto"/></param>
        [System.ServiceModel.OperationContract]
        void Delete(Cic.OpenLease.ServiceAccess.DdOl.ANGKALKDto angKalkDto);

        /// <summary>
        /// Kopiert ein ANGKALK mit alle ihren abhängigen Daten.
        /// </summary>
        /// <param name="angKalkDto"><see cref="Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTDto"/></param>
        /// <returns><see cref="Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTDto"/></returns>
        [System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.ANGKALKDto Copy(Cic.OpenLease.ServiceAccess.DdOl.ANGKALKDto angKalkDto);
        #endregion
    }
}
