// OWNER MK, 06-01-2009
namespace Cic.OpenLease.ServiceAccess.DdOl
{
	/// <summary>
	/// Dienstvertrag für Datenzugriffsobjekte für Anträge
	/// </summary>
    [System.CLSCompliant(true)]
    [System.ServiceModel.ServiceContract(Name = "ANTRAGDaoContract", Namespace = "http://cic-software.de/Cic.OpenLease.Service.DdOl")]
    public interface IANTRAGDao
    {
        #region Methods

        #region Deprecated
        ///// <summary>
        ///// Lievert Auflagen zum ANTRAG
        ///// MK
        ///// </summary>
        ///// <param name="sysAntrag"></param>
        ///// <param name="language"></param>
        ///// <returns></returns>
        //[System.ServiceModel.OperationContract]
        //Cic.OpenLease.Model.DdOw.DECON[] DeliverConditions(long sysAntrag, string language);

        ///// <summary>
        ///// Auflage Erfüllt-Status ändernung
        ///// MK
        ///// </summary>
        ///// <param name="sysDeCon"></param>
        ///// <param name="isFulfilled"></param>
        ///// <returns></returns>
        //[System.ServiceModel.OperationContract]
        //bool ConditionFulfilled(long sysDeCon, bool isFulfilled);

        ///// <summary>
        ///// Erzeugt und Liefert eine Liste von Antragsdokumente.
        ///// </summary>
        ///// <param name="sysAntrag">Antrag ID</param>
        ///// <param name="language">de, fr, it</param>
        ///// <param name="generateThumbnails">Bestimmt ob die Thumbnails erzeugt werden sollen.</param>
        ///// <returns></returns>
        //[System.ServiceModel.OperationContract]
        //DocumentDto[] DeliverPrintedDocuments(long sysAntrag, string language, bool generateThumbnails);
         
        //[System.ServiceModel.OperationContract]
        //Cic.OpenLease.ServiceAccess.DdOl.ANTRAGDto Deliver(long sysID);

        ///// <summary>
        ///// Die Methode zur erneuten Einreichen eines Angebotes.
        ///// JJ
        ///// </summary>
        ///// <param name="sysAntrag">sysAntrag</param>
        ///// <returns></returns>
        //[System.ServiceModel.OperationContract]
        //void Resubmit(long sysAntrag);
        #endregion

        /// <summary>
		/// Gibt eine Liste von Datentransferobjekten zurück, die den angegebenen Suchwerten und Suchparametern entsprechen.
		/// Das Gesichtsfeld kann berücksichtigt werden.
		/// </summary>
        /// <param name="antragSearchData">Suchwerte <see cref="Cic.OpenLease.ServiceAccess.DdOl.ANTRAGSearchData"/>.</param>
        /// <param name="searchParameters">Suchparameter <see cref="Cic.OpenLease.ServiceAccess.SearchParameters" />.</param>
        /// <param name="antragSortData">Sortparametern <see cref="Cic.OpenLease.ServiceAccess.DdOl.ANTRAGSortData" />.</param>
        /// <returns>Liste von <see cref="Cic.OpenLease.ServiceAccess.DdOl.ANTRAGDto"/></returns>
        [System.ServiceModel.OperationContract]
        SearchResult<ANTRAGShortDto> Search(Cic.OpenLease.ServiceAccess.DdOl.ANTRAGSearchData antragSearchData, Cic.OpenLease.ServiceAccess.SearchParameters searchParameters, ANTRAGSortData[] antragSortData);

       

        [System.ServiceModel.OperationContract]
        ANTRAGDto Deliver(long sysId);

        [System.ServiceModel.OperationContract]
        ApprovalDto DeliverApproval(long sysAntrag);

        [System.ServiceModel.OperationContract]
        BonitaetDto[] DeliverBonitaet(long sysAntrag);

 
		#endregion
    }
}
