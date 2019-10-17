using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Cic.OpenOne.GateBANKNOW.Service.Contract
{
    /// <summary>
    /// Das Interface searchAngebotService stellt die Methoden für die Angebotssuche bereit
    /// </summary>
    [ServiceContract(Name = "IsearchAngebotService", Namespace = "http://cic-software.de/GateBANKNOW")]
    public interface IsearchAngebotService
    {
        /// <summary>
        /// Findet Angebote anhand Filterbedingung und berücksichtigt Sortierung und Pagination
        /// </summary>
        /// <param name="input">isearchAngebotDto</param>
        /// <returns>osearchAngebotDto</returns>
        [OperationContract]
        DTO.osearchAngebotDto searchAngebot(DTO.isearchAngebotDto input);

        /// <summary>
        /// Liefert alle relevanten Angebotsdaten
        /// </summary>
        /// <param name="input">igetAngebotDetailDto</param>
        /// <returns>ogetAngebotDetailDto</returns>
        [OperationContract]
        DTO.ogetAngebotDetailDto getAngebotDetail(DTO.igetAngebotDetailDto input);
    }
}
