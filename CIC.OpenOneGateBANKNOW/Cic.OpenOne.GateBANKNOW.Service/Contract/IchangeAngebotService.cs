using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Cic.OpenOne.GateBANKNOW.Service.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.Contract
{
    /// <summary>
    /// Das Interface changeAngebotService stellt die Methoden zum Angebot kopieren und löschen bereit.
    /// </summary>
    [ServiceContract(Name = "IchangeAngebotService", Namespace = "http://cic-software.de/GateBANKNOW")]
    public interface IchangeAngebotService
    {
        /// <summary>
        /// Kopiert Persistenzobjekte des Angebots und aller Angebotsvarianten
        /// </summary>
        /// <param name="input">icopyAngebotDto</param>
        /// <returns>ocopyAngebotDto</returns>
        [OperationContract]
        ocopyAngebotDto copyAngebot(DTO.icopyAngebotDto input);

        /// <summary>
        /// Kopiert Persistenzobjekte des Angebots und aller Angebotsvarianten
        /// </summary>
        /// <param name="sysid">Primary key</param>
        /// <returns>ocopyAngebotDto</returns>
        [OperationContract]
        ocopyAngebotDto copyAngebotById(long sysid);

        /// <summary>
        /// Löscht Angebot und alle Angebotsvarianten sowie abhängige Tabellen
        /// </summary>
        /// <param name="input">ideleteAngebotDto</param>
        /// <returns>odelteAngebotDto</returns>
        [OperationContract]
        odeleteAngebotDto deleteAngebot(DTO.ideleteAngebotDto input);
    }
}
