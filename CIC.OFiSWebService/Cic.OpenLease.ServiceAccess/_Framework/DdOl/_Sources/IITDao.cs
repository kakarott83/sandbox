// OWNER BK, 15-06-2009
using System;
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
	/// Dienstvertrag für Datenzugriffsobjekte für Interessenten
    /// </summary>
    [System.CLSCompliant(true)]
    [System.ServiceModel.ServiceContract(Name = "ITDaoContract", Namespace = "http://cic-software.de/Cic.OpenLease.Service.DdOl")]
    public interface IITDao
    {
        #region Methods
        /// <summary>
		/// Gibt eine Liste von Datentransferobjekten zurück, die den angegebenen Suchwerten und Suchparametern entsprechen.
		/// Das Gesichtsfeld kann berücksichtigt werden.
		/// </summary>
        /// <param name="itSearchData">Suchwerte <see cref="Cic.OpenLease.ServiceAccess.DdOl.ITSearchData"/>.</param>
        /// <param name="searchParameters">Suchparameter <see cref="Cic.OpenLease.ServiceAccess.SearchParameters" />.</param>
        /// <param name="itSortData">Sortparametern <see cref="Cic.OpenLease.ServiceAccess.DdOl.ITSortData" />.</param>
        /// <returns>Liste von <see cref="Cic.OpenLease.ServiceAccess.DdOl.ITDto"/></returns>
        [System.ServiceModel.OperationContract]
        SearchResult<ITShortDto> Search(Cic.OpenLease.ServiceAccess.DdOl.ITSearchData itSearchData, Cic.OpenLease.ServiceAccess.SearchParameters searchParameters, Cic.OpenLease.ServiceAccess.DdOl.ITSortData[] itSortData);



        /// <summary>
        /// Die Methode liefert, falls vorhanden, ein Datentransferobjekt mit der angegebenen ID.
        /// Das Gesichtsfeld kann berücksichtigt werden.
        /// </summary>
        /// <param name="sysIT">ID des Objektes</param>
        /// <param name="sysANGEBOT">ID des Angebotes, optional für KNE bei Hauptantragsteller</param>
        /// <returns><see cref="Cic.OpenLease.ServiceAccess.DdOl.ITDto"/></returns>
        [System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.ITDto Deliver(long sysIT, long sysANGEBOT);

        /// <summary>
		/// Speichert die Daten aus dem Datentransferobjekt in die Datenbank (INSERT oder UPDATE).
		/// Das Gesichtsfeld kann berücksichtigt werden.
		/// </summary>
        /// <param name="itDto"><see cref="Cic.OpenLease.ServiceAccess.DdOl.ITDto"/></param>
        /// <returns><see cref="Cic.OpenLease.ServiceAccess.DdOl.ITDto"/></returns>
		[System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.ITDto Save(Cic.OpenLease.ServiceAccess.DdOl.ITDto itDto);

        /// <summary>
        /// Die Methode löscht die Daten (aus dem Datentransferobjekt) aus der Datenbank.
        /// Interessenten dürfen gelöscht werden, wenn es keine Angebote existieren.
        /// Mit Benutzer Validierung (RFG.LÖSCHEN erforderlich).
        /// </summary>
        /// <param name="sysIT">ID des Objektes</param>
        [System.ServiceModel.OperationContract]
        void Delete(long sysIT);


        /// <summary>
        /// Deletes the ITKNE
        /// </summary>
        /// <param name="kne"></param>
        [System.ServiceModel.OperationContract]
        void DeleteKNE(ITKNEDto kne);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.ITDto DeliverItDtoFromSa3(string xmlData);

        [System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.VorgaengeDto searchVorgaenge(long sysIT, int anzahlMax);


        /// <summary>
        /// Saves the IT and the Offer Mandat reference (MANDAT, ANGEBOT.EINZUG)
        /// </summary>
        /// <param name="itDto"></param>
        /// <param name="angebotDto"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.ITDto SaveKonto(Cic.OpenLease.ServiceAccess.DdOl.ITDto itDto, Cic.OpenLease.ServiceAccess.DdOl.ANGEBOTDto angebotDto);

        /// <summary>
        /// determines if konto may be changed
        /// only true when no active contract
        /// </summary>
        /// <param name="sysAngebot"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        bool changeBankdatenAllowed(long sysAngebot);
        /// <summary>
        /// updates the konto info in IT from available global mandate or person konto
        /// </summary>
        /// <param name="sysit"></param>
        /// <param name="sysvsart"></param>
        [System.ServiceModel.OperationContract]
        void updateITBankdaten(long sysit, long sysvsart);

        /// <summary>
        /// returns the default signcity for the current salesperson
        /// </summary>
        [System.ServiceModel.OperationContract]
        String getDefaultSigncity();

        /// <summary>
        /// delivers the current active konto used for the offer
        /// </summary>
        /// <param name="sysAngebot"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.BankdatenDto DeliverBankdaten(long sysAngebot);
        /// <summary>
        /// saves the currently used konto for a already submitted offer
        /// may create a konto and global-mandate
        /// </summary>
        /// <param name="bankdaten"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.BankdatenDto SaveBankdaten(Cic.OpenLease.ServiceAccess.DdOl.BankdatenDto bankdaten);
        #endregion
    }
}