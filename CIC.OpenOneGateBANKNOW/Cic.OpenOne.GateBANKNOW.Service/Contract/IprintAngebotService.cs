using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Cic.OpenOne.GateBANKNOW.Service.Contract
{
    /// <summary>
    /// Das Interface printAngebotService stellt die Methoden bereit zur Lieferung einer Liste aller vorhandenen (zur Auswahl stehenden) Dokumente und zur erstellung eines Druckauftrags.
    /// </summary>
    [ServiceContract(Name = "IprintAngebotService", Namespace = "http://cic-software.de/GateBANKNOW")]
    public interface IprintAngebotService
    {
        /// <summary>
        /// Liefert eine Liste der verfügbaren Dokumente im Kontext (20100914_Logik_Druckmatrix_v2)
        /// </summary>
        /// <param name="input">ilistAvailableDokumenteDto</param>
        /// <returns>olistAvailableDokumenteDto</returns>
        [OperationContract]
        DTO.olistAvailableDokumenteDto listAvailableDokumente(DTO.ilistAvailableDokumenteDto input);

        /// <summary>
        /// Erstellt Druckauftrag für ausgewählte Dokumente
        /// </summary>
        /// <param name="input">iprintCheckedDokumenteDto</param>
        /// <returns>oprintCheckedDokumenteDto</returns>
        [OperationContract]
        DTO.oprintCheckedDokumenteDto printCheckedDokumente(DTO.iprintCheckedDokumenteDto input);

        /// <summary>
        /// Checkt ob der letzte Druckauftrag fertig ist
        /// </summary>
        /// <param name="input">iprintCheckedDokumenteDto</param>
        /// <returns>oprintCheckedDokumenteDto</returns>
        DTO.oprintCheckedDokumenteDto checkPrintCheckedDokumente(DTO.iprintCheckedDokumenteDto input);
    }
}
 