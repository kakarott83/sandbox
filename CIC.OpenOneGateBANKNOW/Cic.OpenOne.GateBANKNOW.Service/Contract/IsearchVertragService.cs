using System.ServiceModel;

namespace Cic.OpenOne.GateBANKNOW.Service.Contract
{
    /// <summary>
    /// Das Interface searchVertragService stellt die Methoden für die Vertragssuche bereit
    /// </summary>
    [ServiceContract(Name = "IsearchVertragService", Namespace = "http://cic-software.de/GateBANKNOW")]

    public interface IsearchVertragService
    {
        /// <summary>
        /// Findet Verträge anhand Filterbedingung und berücksichtigt Sortierung und Pagination
        /// </summary>
        /// <param name="input">isearchVertragDto</param>
        /// <returns>osearchVertragDto</returns>
        [OperationContract]
        DTO.osearchVertragDto searchVertrag(DTO.isearchVertragDto input);

        /// <summary>
        /// Liefert alle relevanten Vertagsdaten
        /// </summary>
        /// <param name="input">igetVertragDetailDto</param>
        /// <returns>ogetVertragDetailDto</returns>
        [OperationContract]
        DTO.ogetVertragDetailDto getVertragDetail(DTO.igetVertragDetailDto input);




        /// <summary>
        /// Exportiert eine Liste von laufenden Verträgen 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        DTO.ocreateListenExportDto createListenExport(DTO.icreateListenExportDto input);

        /// <summary>
        /// Listet Dateien mit Vertragslisten auf
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        DTO.olistInboxServicesDto listInboxServices(DTO.ilistInboxServicesDto input);

        /// <summary>
        /// Holt eine Fertige Liste als Excel-Datei 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        DTO.ogetListeExportDto getListeExport(DTO.igetListeExportDto input);

        /// <summary>
        /// Löscht eine einzelne Vertragsliste 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationContract]
        DTO.odeleteListeExportDto deleteListeExport(DTO.ideleteListeExportDto input);

        /// <summary>
        /// 2.3.4 Umsetzung der Änderung des Restwertrechnungsempfänger
        /// true when change of restwert rechnung empfänger allowed
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        [OperationContract]
        bool isRREChangeAllowed(long sysid);
    }
}