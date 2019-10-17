using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Cic.OpenOne.GateBANKNOW.Service.Contract
{
    /// <summary>
    /// Das Interface searchAntragService stellt die Methoden für die Antragssuche bereit
    /// </summary>
    [ServiceContract(Name = "IsearchAntragService", Namespace = "http://cic-software.de/GateBANKNOW")]
    public interface IsearchAntragService
    {
        /// <summary>
        /// Findet Anträge anhand Filterbedingung und berücksichtigt Sortierung und Pagination
        /// </summary>
        /// <param name="input">isearchAntragDto</param>
        /// <returns>osearchAntragDto</returns>
        [OperationContract]
        DTO.osearchAntragDto searchAntrag(DTO.isearchAntragDto input);

        /// <summary>
        /// Liefert alle relevanten Antragsdaten
        /// </summary>
        /// <param name="input">igetAntragDetailDto</param>
        /// <returns>ogetAntragDetailDto</returns>
        [OperationContract]
        DTO.ogetAntragDetailDto getAntragDetail(DTO.igetAntragDetailDto input);
    }
}
