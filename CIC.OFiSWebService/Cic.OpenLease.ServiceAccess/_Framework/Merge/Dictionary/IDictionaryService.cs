// OWNER JJ, 09-12-2009
using System;

namespace Cic.OpenLease.ServiceAccess.Merge.Dictionary
{
    /// <summary>
    /// </summary>
    [System.CLSCompliant(true)]
    [System.ServiceModel.ServiceContract(Name = "DictionaryService", Namespace = "http://cic-software.de/Cic.OpenLease.Service.Merge.DictionaryService")]
    public interface IDictionaryService
    {
        #region Methods
        /// <summary>
        /// Returns a list of LANDDto data objects.
        /// </summary>
        /// <returns>List of <see cref="Cic.OpenLease.ServiceAccess.Merge.Dictionary.LANDDto" /></returns>
        [System.ServiceModel.OperationContract]
        System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.LANDDto> DeliverLANDDtoList();
        /// <summary>
        /// Returns a list of LANDDto data objects.
        /// </summary>
        /// <returns>List of <see cref="Cic.OpenLease.ServiceAccess.Merge.Dictionary.LANDDto" /></returns>
        [System.ServiceModel.OperationContract]
        System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.LANDDto> DeliverAddressLand();
        /// <summary>
        /// Returns a list of STAATDto data objects.
        /// </summary>
        /// <returns>List of <see cref="Cic.OpenLease.ServiceAccess.Merge.Dictionary.STAATDto" /></returns>
        [System.ServiceModel.OperationContract]
        System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.STAATDto> DeliverSTAATDtoList();
        /// <summary>
        /// Returns a list of CTLANGDto data objects.
        /// </summary>
        /// <returns>List of <see cref="Cic.OpenLease.ServiceAccess.Merge.Dictionary.CTLANGDto" /></returns>
        [System.ServiceModel.OperationContract]
        System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.CTLANGDto> DeliverCTLANGDtoList();
        /// <summary>
        /// Returns a list of BRANCHEDto data objects.
        /// </summary>
        /// <returns>List of <see cref="Cic.OpenLease.ServiceAccess.Merge.Dictionary.BRANCHEDto" /></returns>
        [System.ServiceModel.OperationContract]
        System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.BRANCHEDto> DeliverBRANCHEDtoList();
        /// <summary>
        /// Returns a list of PLZDto data objects.
        /// </summary>
        /// <returns>List of <see cref="Cic.OpenLease.ServiceAccess.Merge.Dictionary.PLZDto" /></returns>
        [System.ServiceModel.OperationContract]
        System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.PLZDto> DeliverPLZDtoList();
        /// <summary>
        /// Returns a list of BLZDto data objects.
        /// </summary>
        /// <returns>List of <see cref="Cic.OpenLease.ServiceAccess.Merge.Dictionary.BLZDto" /></returns>
        [System.ServiceModel.OperationContract]
        System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.BLZDto> DeliverBLZDtoList();
        /// <summary>
        /// Returns a list of BERUFDto data objects.
        /// </summary>
        /// <returns>List of <see cref="Cic.OpenLease.ServiceAccess.Merge.Dictionary.BERUFDto" /></returns>
        [System.ServiceModel.OperationContract]
        System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.BERUFDto> DeliverBERUFDtoList();
        /// <summary>
        /// Returns a list of KDTYPDto data objects.
        /// </summary>
        /// <returns>List of <see cref="Cic.OpenLease.ServiceAccess.Merge.Dictionary.KDTYPDto" /></returns>
        [System.ServiceModel.OperationContract]
        System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.KDTYPDto> DeliverKDTYPDtoList();
        /// <summary>
        /// Returns a list of PLZDto data objects filtered with PLZ.
        /// </summary>
        /// <returns>List of <see cref="Cic.OpenLease.ServiceAccess.Merge.Dictionary.PLZDto" /></returns>
        [System.ServiceModel.OperationContract]
        System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.PLZDto> SearchORT(string plz);
        /// <summary>
        /// Returns a list of BLZDto data objects filtered with BLZ and BIC.
        /// </summary>
        /// <returns>List of <see cref="Cic.OpenLease.ServiceAccess.Merge.Dictionary.BLZDto" /></returns>
        [System.ServiceModel.OperationContract]
        System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.BLZDto> SearchBANKNAME(string blz, string bic);

        [System.ServiceModel.OperationContract]
        DictionaryDto[] DeliverDictionary(string dictionaryName);
        #endregion

        /// <summary>
        /// Delivers all RECHTSFORMCODE entries
        /// </summary>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.DropListDto> DeliverRECHTSFORMList();

        /// <summary>
        /// Delivers all HREGISTERART entries
        /// </summary>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.DropListDto> DeliverHREGISTERARTList();

        /// <summary>
        /// Generic ddlkppos list get method
        /// </summary>
        /// <param name="code"></param>
        /// <param name="domainid"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.DropListDto> DeliverDDLKPPOSList(String code, String domainid);

        /// <summary>
        /// Delivers all VARTs
        /// </summary>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.Merge.Dictionary.DropListDto> DeliverVARTList();
    }
}