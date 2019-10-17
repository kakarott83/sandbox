// OWNER MK, 05-02-2009
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
	/// Dienstvertrag für Datenzugriffsobjekte für Adressen
    /// </summary>
    [System.CLSCompliant(true)]
    [System.ServiceModel.ServiceContract(Name = "ADRESSEDaoContract", Namespace = "http://cic-software.de/contract")]
    public interface IADRESSEDao
    {
        #region Methods
        /// <summary>
		/// Gibt eine Liste von Datentransferobjekten zurück, die den angegebenen Suchwerten und Suchparametern entsprechen.
        /// </summary>
        /// <param name="adresseSearchData">Suchwerte <see cref="Cic.OpenLease.ServiceAccess.DdOl.ADRESSESearchData" />.</param>
        /// <param name="searchParameters">Suchparameter <see cref="Cic.OpenLease.ServiceAccess.SearchParameters" />.</param>
        /// <returns>Liste von <see cref="Cic.OpenLease.ServiceAccess.DdOl.ADRESSEDto" /></returns>
        [System.ServiceModel.OperationContract]
		System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.DdOl.ADRESSEDto> Search(Cic.OpenLease.ServiceAccess.DdOl.ADRESSESearchData adresseSearchData, Cic.OpenLease.ServiceAccess.SearchParameters searchParameters);

        /// <summary>
		/// Die Methode gibt die Anzahl der gefundenen Sätze zurück, die den angegebenen Suchwerten entsprechen.
        /// </summary>
        /// <param name="adresseSearchData">Suchwerte <see cref="Cic.OpenLease.ServiceAccess.DdOl.ADRESSESearchData" />.</param>
        /// <returns>Anzahl der gefundenen Sätze</returns>
		[System.ServiceModel.OperationContract]
		int SearchCount(Cic.OpenLease.ServiceAccess.DdOl.ADRESSESearchData adresseSearchData);

        /// <summary>
        /// Gibt eine Liste mit alle Länder (Land) zurück.
        /// </summary>
        /// <returns>Liste von <see cref="Cic.OpenLease.Model.DdOl.LAND" /></returns>
        [System.ServiceModel.OperationContract]
        System.Collections.Generic.List<Cic.OpenLease.Model.DdOl.LAND> DeliverLANDList();

        /// <summary>
        /// Gibt eine Liste mit alle Länder (Land) zurück die in Adresse zugelassem sind.
        /// </summary>
        /// <returns>Liste von <see cref="Cic.OpenLease.Model.DdOl.LAND" /></returns>
        [System.ServiceModel.OperationContract]
        System.Collections.Generic.List<Cic.OpenLease.Model.DdOl.LAND> DeliverAddresseLANDList();

        /// <summary>
        /// Gibt eine Liste mit alle Staate (Staat) zurück.
        /// </summary>
        /// <returns>Liste von <see cref="Cic.OpenLease.Model.DdOl.STAAT" /></returns>
        [System.ServiceModel.OperationContract]
        System.Collections.Generic.List<Cic.OpenLease.Model.DdOl.STAAT> DeliverSTAATList();

        /// <summary>
        /// Gibt eine Liste mit alle Sprachen (CTLANG) zurück.
        /// </summary>
        /// <returns>Liste von <see cref="Cic.OpenLease.Model.DdOw.CTLANG" /></returns>
        [System.ServiceModel.OperationContract]
        System.Collections.Generic.List<Cic.OpenLease.Model.DdOw.CTLANG> DeliverCTLANGList();

        /// <summary>
        /// Gibt eine Liste mit alle Branchen (BRANCHE) zurück.
        /// </summary>
        /// <returns>Liste von <see cref="Cic.OpenLease.Model.DdOl.BRANCHE" /></returns>
        [System.ServiceModel.OperationContract]
        System.Collections.Generic.List<Cic.OpenLease.Model.DdOl.BRANCHE> DeliverBRANCHEList();
		#endregion
    }
}
