using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Cic.OpenOne.GateBANKNOW.Service.Contract
{
    /// <summary>
    /// Die Schnnittstelle Schnellkalkulation definiert alle Methoden für die Schnellkalkulator-GUI wie
    /// Produktliste, Serviceliste, Parameterliste, Kalkulation, save/update der Schnellkalkulation
    /// </summary>
    
    [ServiceContract(Name = "IcreateOrUpdateSchnellkalkulationService", Namespace = "http://cic-software.de/GateBANKNOW")]
    public interface IcreateOrUpdateSchnellkalkulationService
    {
        /// <summary>
        /// Liefert eine Liste der verfügbaren Produkte im Kontext
        /// </summary>
        /// <param name="input">ilistAvailableProdukteDto</param>
        /// <returns>olistAvailableProdukteDto</returns>
        [OperationContract]
        DTO.olistAvailableProdukteDto listAvailableProdukte(DTO.ilistAvailableProdukteDto input);

        /// <summary>
        /// Liefert eine Liste der verfügbaren Services im Kontext
        /// </summary>
        /// <param name="input">ilistAvailableServicesDto</param>
        /// <returns>olistAvailableServciesDto</returns>
        [OperationContract]
        DTO.olistAvailableServicesDto listAvailableServices(DTO.ilistAvailableServicesDto input);

        /// <summary>
        /// Liefert eine Liste aller Parameter und Eckwerte des Produkts
        /// </summary>
        /// <param name="input">igetParameterDto</param>
        /// <returns>ogetParameterDto</returns>
        [OperationContract]
        DTO.ogetParameterDto getParameter(DTO.igetParameterDto input);

        /// <summary>
        /// Löst die aktuelle Schnellkalkulation auf und liefert die Berechnungsergebnisse
        /// </summary>
        /// <param name="input">isolveKalkulationDto</param>
        /// <returns>osoleKalkulationDto</returns>
        [OperationContract]
        DTO.osolveSchnellkalkulationDto solveKalkulation(DTO.isolveSchnellkalkulationDto input);

        /// <summary>
        /// Erzeugt/Ändert Persistenzobjekt der Schnellkalkulation
        /// </summary>
        /// <param name="input">icreateKalkulationDto</param>
        /// <returns>ocreateKalkulationDto</returns>
        [OperationContract]
        DTO.ocreateKalkulationDto createOrUpdateKalkulation(DTO.icreateKalkulationDto input);

        /// <summary>
        /// Speichert Persistenzobjekt der Schnellkalkulation
        /// </summary>
        /// <param name="input">icreateKalkulationDto</param>
        /// <returns>ocreateKalkulationDto</returns>
        [OperationContract]
        DTO.osaveKalkulationDto saveKalkulation(DTO.isaveKalkulationDto input);

                /// <summary>
        /// Prüft die Kalkulation
        /// </summary>
        /// <param name="input">icheckAngebotDto</param>
        /// <returns>ocheckAngebotDto</returns>
        [OperationContract]
        Service.DTO.ocheckAntAngDto checkKalkulation(DTO.icheckKalkulationDto input);
    }
}
