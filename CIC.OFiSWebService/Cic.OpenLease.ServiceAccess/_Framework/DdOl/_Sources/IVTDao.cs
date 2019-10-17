// OWNER MK, 06-01-2009
using System;
namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
	/// Dienstvertrag für Datenzugriffsobjekte für Verträge
    /// </summary>
    [System.CLSCompliant(true)]
    [System.ServiceModel.ServiceContract(Name = "VTDaoContract", Namespace = "http://cic-software.de/Cic.OpenLease.Service.DdOl")]
    public interface IVTDao
    {
        #region Methods

         /// <summary>
        /// Prints a quick overview document
        /// </summary>
        /// <param name="printData"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        String createHtmlReport(PrintDto printData);

        /// <summary>
		/// Gibt eine Liste von Datentransferobjekten zurück, die den angegebenen Suchwerten und Suchparametern entsprechen.
		/// Das Gesichtsfeld kann berücksichtigt werden.
        /// </summary>
        /// <param name="vtSearchData">Suchwerte <see cref="Cic.OpenLease.ServiceAccess.DdOl.VTSearchData"/>.</param>
        /// <param name="searchParameters">Suchparameter <see cref="Cic.OpenLease.ServiceAccess.SearchParameters" />.</param>
        /// <param name="vtSortData">Sortparametern <see cref="Cic.OpenLease.ServiceAccess.DdOl.VTSortData" />.</param>
        /// <returns>List von <see cref="Cic.OpenLease.ServiceAccess.DdOl.VTDto"/></returns>
        [System.ServiceModel.OperationContract]
        SearchResult<VTShortDto> Search(Cic.OpenLease.ServiceAccess.DdOl.VTSearchData vtSearchData, Cic.OpenLease.ServiceAccess.SearchParameters searchParameters, VTSortData[] vtSortData);

        [System.ServiceModel.OperationContract]
        bool isExtendable(long sysId);

        [System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.VTDto Deliver(long sysId);
        #region Deprecated
        ///// <summary>
        ///// Die Methode liefert ein Buchwert PDF dokument.
        ///// MK
        ///// </summary>
        ///// <param name="sysVt">Vertrags Id</param>
        ///// <param name="language">Vertrags Id</param>
        ///// <param name="generateThumbnails">Vertrags Id</param>
        ///// <returns><see cref="Cic.OpenLease.ServiceAccess.DdOl.VTAssetsValueDto"/></returns>        
        //[System.ServiceModel.OperationContract]
        //VTAssetsValueDto DeliverAssetValueDocument(long sysVt, string language, bool generateThumbnails);

        ///// <summary>
        ///// Die methode liefert den Jahresumsatz der angemeldeten Rolle über das Gesichtsfeld.
        ///// </summary>
        ///// <param name="year">Jahr</param>
        ///// <returns>double</returns>
        //[System.ServiceModel.OperationContract]
        //decimal DeliverAnnualSales(int year);
        #endregion

        #endregion
    }
}
