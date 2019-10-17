// OWNER MK, 06-01-2009
using Cic.OpenLease.ServiceAccess.Merge.OlClient;
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
	/// Dienstvertrag für Datenzugriffsobjekte für Schnellkalkulationen
    /// </summary>
    [System.CLSCompliant(true)]
    [System.ServiceModel.ServiceContract(Name = "MYCALCDaoContract", Namespace = "http://cic-software.de/contract")]
	public interface IMYCALCDao
    {
        #region Methods
        /// <summary>
        /// Erzeugt und Liefert ein Dokumente.
        /// </summary>
        /// <param name="sysMyCalc"></param>
        /// <param name="language">de, fr, it</param>
        /// <param name="generateThumbnails">Bestimmt ob die Thumbnails erzeugt werden sollen.</param>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        DocumentDto DeliverPrintDocument(long sysMyCalc, string language, bool generateThumbnails);

        /// <summary>
        /// Übergabe einer Schnellkalkulation an ein neues Angebot
        /// </summary>
        /// <param name="sysMyCalc">Schnellkalkulation ID</param>
        /// <returns>ANGEBOTDto</returns>
        [System.ServiceModel.OperationContract]
        ANGEBOTDto MYCALC2ANGEBOT(long sysMyCalc);

        /// <summary>
		/// Gibt eine Liste von Datentransferobjekten zurück, die den angegebenen Suchwerten und Suchparametern entsprechen.
        /// </summary>
        /// <param name="mycalcSearchData">Suchwerte <see cref="Cic.OpenLease.ServiceAccess.DdOl.MYCALCSearchData"/>.</param>
        /// <param name="searchParameters">Suchparameter <see cref="Cic.OpenLease.ServiceAccess.SearchParameters" />.</param>
        /// <returns>List von <see cref="Cic.OpenLease.ServiceAccess.DdOl.MYCALCDto"/></returns>
        [System.ServiceModel.OperationContract]
		System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.DdOl.MYCALCDto> Search(Cic.OpenLease.ServiceAccess.DdOl.MYCALCSearchData mycalcSearchData, Cic.OpenLease.ServiceAccess.SearchParameters searchParameters);

        /// <summary>
		/// Die Methode gibt die Anzahl der gefundenen Sätze zurück, die den angegebenen Suchwerten entsprechen.
        /// </summary>
        /// <param name="mycalcSearchData">Suchwerte <see cref="Cic.OpenLease.ServiceAccess.DdOl.MYCALCSearchData"/>.</param>
        /// <returns>Anzahl der gefundenen Sätze</returns>
		[System.ServiceModel.OperationContract]
		int SearchCount(Cic.OpenLease.ServiceAccess.DdOl.MYCALCSearchData mycalcSearchData);

        /// <summary>
        /// Die Methode liefert ein Objekt mit einer bestimmten Id.
        /// </summary>
        /// <param name="sysID">ID des Objektes</param>
		/// <returns><see cref="Cic.OpenLease.ServiceAccess.DdOl.MYCALCDto"/></returns>
		[System.ServiceModel.OperationContract]
		Cic.OpenLease.ServiceAccess.DdOl.MYCALCDto Deliver(long sysID);

        /// <summary>
		/// Speichert die Daten aus dem Datentransferobjekt in die Datenbank (INSERT oder UPDATE).
		/// </summary>
        /// <param name="mycalcDto"><see cref="Cic.OpenLease.ServiceAccess.DdOl.MYCALCDto"/></param>
        /// <returns><see cref="Cic.OpenLease.ServiceAccess.DdOl.MYCALCDto"/></returns>
		[System.ServiceModel.OperationContract]
		Cic.OpenLease.ServiceAccess.DdOl.MYCALCDto Save(Cic.OpenLease.ServiceAccess.DdOl.MYCALCDto mycalcDto);

        /// <summary>
        /// Die Methode löscht die Daten (aus dem Datentransferobjekt) aus der Datenbank.
        /// Mit Benutzer Validierung (RFG.LÖSCHEN erforderlich).
        /// JJ
        /// </summary>
		/// <param name="mycalcDto"><see cref="Cic.OpenLease.ServiceAccess.DdOl.MYCALCDto"/></param>
        [System.ServiceModel.OperationContract]
        void Delete(Cic.OpenLease.ServiceAccess.DdOl.MYCALCDto mycalcDto);
		#endregion
    }
}
