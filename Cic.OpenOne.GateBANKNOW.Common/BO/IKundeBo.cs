using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// KundeBo Interface
    /// </summary>
    public interface IKundeBo
    {
        /// <summary>
        /// createOrUpdateKunde Methode
        /// </summary>
        /// <param name="kunde">KundeDto</param>
        /// <param name="sysperole">User Id</param>
        /// <returns>KundeDto</returns>
        KundeDto createOrUpdateKunde(KundeDto kunde, long sysperole);

        /// <summary>
        /// createOrUpdateKunde Methode
        /// </summary>
        /// <param name="kunde">KundeDto</param>
        /// <param name="sysperole">User Id</param>
        /// <returns>KundeDto</returns>
        KundeDto createOrUpdateKundePerson(KundeDto kunde, long sysperole);

        /// <summary>
        /// Adresse via Interessenten ID holen
        /// </summary>
        /// <param name="sysit">Adressen ID</param>
        /// <returns>Daten</returns>
        AdresseDto[] getAdressen(long sysit);

        /// <summary>
        /// Konten via Interessenten ID holen
        /// </summary>
        /// <param name="sysit"></param>
        /// <returns></returns>
        KontoDto[] getKonten(long sysit);

        /// <summary>
        /// getKunde Methode
        /// </summary>
        /// <param name="sysit">KundenID(SysIT)</param>
        /// <returns>KundeDto</returns>
        KundeDto getKunde(long sysit);

        /// <summary>
        /// getKunde ruft das KundeDAO auf und führt die getKunde Methode aus
        /// </summary>
        /// <param name="sysit">SYSIT des Kunden</param>
        /// <param name="sysantrag">Antrag ID</param>
        /// <returns>Liefert ein KundeDto mit der gleichen SYSIT zurück</returns>
        KundeDto getKundeViaAntragID(long sysit, long sysantrag);

        /// <summary>
        /// Merges all ITPKZ/ITUKZ-Fields into the corresponding PKZ/UKZ/PERSON for the antrag, assuming the pkz/ukz is already there
        /// </summary>
        /// <param name="syskd"></param>
        /// <param name="sysit"></param>
        /// <param name="sysantrag"></param>
        void transferITPKZUKZToPERSON(long syskd, long sysit, long sysantrag);

        /// <summary>
        /// Merges all ITPKZ/ITUKZ-Fields into the corresponding PKZ/UKZ for the antrag, assuming the pkz/ukz is already there
        /// </summary>
        /// <param name="syskd"></param>
        /// <param name="sysit"></param>
        /// <param name="sysantrag"></param>
        void transferITPKZUKZToPKZUKZ(long syskd, long sysit, long sysantrag);

        /// <summary>
        /// aktuellsten Mitantragsteller laden
        /// </summary>
        /// <param name="sysit"></param>
        /// <returns></returns>
        long getMitantragsteller(long sysit);
    }
}
