// CREATOR MK, 15-09-2009
using Cic.OpenLease.ServiceAccess.DdOl;
using System;
namespace Cic.OpenLease.ServiceAccess.Merge.Prisma
{
    [System.CLSCompliant(true)]
    [System.ServiceModel.ServiceContract(Name = "PrismaContract", Namespace = "http://cic-software.de/Cic.OpenLease.Service.Merge.Prisma")]
    public interface IPrismaService
    {
        #region Methods
        /// <summary>
        /// Verfügbarkeit von Produkten ermitteln.
        /// MK
        /// </summary>
        /// <returns>Liste von <see cref="Cic.OpenLease.ServiceAccess.DdOl.PRPRODUCTDto"/></returns>
        [System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.PRPRODUCTDto[] DeliverAvailablePrProducts(long sysObTyp, long sysObArt, DateTime lieferdatum, long sysmart, long syskdtyp);

        [System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.PRPRODUCTDto[] DeliverAvailablePrProductsFiltered(long sysObTyp, long sysObArt, PrParamFilter[] filter, DateTime lieferdatum, long sysmart, long syskdtyp);

        
        [System.ServiceModel.OperationContract]
        decimal DeliverZinsBasis(long sysPrProduct, long term, long amound);

        [System.ServiceModel.OperationContract]
        decimal DeliverZins(long sysPrProduct, long term, long amound, long sysObTyp, long sysObArt, long sysPrkGroup, long sysINTTYPE);
        
        [System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.PrFldArtDto[] DeliverPrFldArt();
        /// <summary>
        /// Verfügbarkeit von Aktion ermitteln.
        /// MK
        /// </summary>
        /// <param name="sysObTyp">The sys ob typ.</param>
        /// <param name="sysObArt">The sys ob art.</param>
        /// <param name="sysKalkTyp">The KALKTYP:SYSKALKTYP.</param>
        /// <param name="lieferdatum">Lieferdatum.</param>
        /// <param name="sysmart">Sysmart</param>
        /// <returns>
        /// Liste von <see cref="Cic.OpenLease.ServiceAccess.DdOl.PRPRODUCTDto"/>
        /// </returns>
        [System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.PRPRODUCTDto[] DeliverAvailablePrProductActions(long sysObTyp, long sysObArt, long sysKalkTyp, DateTime lieferdatum, long sysmart);

        [System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.PRPRODUCTDto[] DeliverAvailablePrProductActionsFiltered(long sysObTyp, long sysObArt, long sysKalkTyp, PrParamFilter[] filter, DateTime lieferdatum, long sysmart);

        
        [System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.PRPARAMDto[] DeliverPrParams(long sysPrProdukt, long sysObTyp, long sysobart, long syskdtyp);

        [System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.OBTYPDto[] SearchObTyp(string description, SearchParameters searchParameters);

        [System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.OBARTDto[] SearchObArt(string description);

        [System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.OBKATDto[] SearchObKat(string description);

        [System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.INTTYPEDto[] DeliverINTTYPE(long sysPrProdukt);

        [System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.ObArtZinsQuickDto[] DeliverObArtZinsQuick();

        /// <summary>
        /// Liefert 2 Listen für Neu- und Verlängerungsprodukte
        /// </summary>
        /// <param name="sysVt"></param>
        /// <returns></returns>
        [System.ServiceModel.OperationContract]
        Cic.OpenLease.ServiceAccess.DdOl.ExtensionProductDto DeliverExtensionProducts(long sysVt);
        
        #endregion
    }
}