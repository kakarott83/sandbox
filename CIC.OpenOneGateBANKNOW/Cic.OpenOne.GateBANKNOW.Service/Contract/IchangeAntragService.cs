using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Cic.OpenOne.GateBANKNOW.Service.Contract
{
    /// <summary>
    /// Das Interface changeAntragService stellt die Methoden zum kopieren und löschen eines Antrags zur verfügung.
    /// </summary>
    [ServiceContract(Name = "IchangeAntragService", Namespace = "http://cic-software.de/GateBANKNOW")]
    public interface IchangeAntragService
    {
        /// <summary>
        /// Kopiert Persistenzobjekte des Antrags
        /// </summary>
        /// <param name="input">icopyAntragDto</param>
        /// <returns>ocopyAntragDto</returns>
        [OperationContract]
        DTO.ocopyAntragDto copyAntrag(DTO.icopyAntragDto input);

        /// <summary>
        /// Kopiert Persistenzobjekte des Antrags by sysId
        /// </summary>
        /// <param name="sysId">Primary key</param>
        /// <returns>ocopyAntragDto</returns>
        [OperationContract]
        DTO.ocopyAntragDto copyAntragById(long sysId);

        /// <summary>
        /// Löscht Antrag und abhängige Tabellen
        /// </summary>
        /// <param name="input">ideleteAntragDto</param>
        /// <returns>odeleteAntragDto</returns>
        [OperationContract]
        DTO.odeleteAntragDto deleteAntrag(DTO.ideleteAntragDto input);

        /// <summary>
        /// Voraussichtlichen Liefertermin ändern
        /// </summary>
        /// <param name="input">isetAblieferdatumDto</param>
        /// <returns>osetAblieferdatumDto</returns>
        [OperationContract]
        DTO.osetAblieferdatumDto setAblieferdatum(DTO.isetAblieferdatumDto input);

        /// <summary>
        /// Stammnummer ändern (*) ((*) Abweichende Änderungsmöglichkeit vom Zustandskonzept, daher via Setter Methoden)
        /// </summary>
        /// <param name="input">isetStammnummerDto</param>
        /// <returns>osetStammnummerDto</returns>
        [OperationContract]
        DTO.osetStammnummerDto setStammnummer(DTO.isetStammnummerDto input);

        /// <summary>
        /// Chassisnummer ändern (*) ((*) Abweichende Änderungsmöglichkeit vom Zustandskonzept, daher via Setter Methoden)
        /// </summary>
        /// <param name="input">isetChassisnummerDto</param>
        /// <returns>osetChassisnummerDto</returns>
        [OperationContract]
        DTO.osetChassisnummerDto setChassisnummer(DTO.isetChassisnummerDto input);

        /// <summary>
        /// Kontrollschild ändern (*) ((*) Abweichende Änderungsmöglichkeit vom Zustandskonzept, daher via Setter Methoden)
        /// </summary>
        /// <param name="input">isetKontrollschildDto</param>
        /// <returns>osetKontrollschildDto</returns>
        [OperationContract]
        DTO.osetKontrollschildDto setKontrollschild(DTO.isetKontrollschildDto input);

        /// <summary>
        /// Farbe ändern (*) ((*) Abweichende Änderungsmöglichkeit vom Zustandskonzept, daher via Setter Methoden)
        /// </summary>
        /// <param name="input">isetFarbeDto</param>
        /// <returns>osetFarbeDto</returns>
        [OperationContract]
        DTO.osetFarbeDto setFarbe(DTO.isetFarbeDto input);
    }
}
