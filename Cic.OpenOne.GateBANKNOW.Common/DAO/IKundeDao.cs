using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// Interface von KundeDao
    /// </summary>
    public interface IKundeDao
    {
        /// <summary>
        /// createKunde Methode
        /// </summary>
        /// <param name="kundeInput">KundeDto</param>
        /// <param name="sysperole">User Id</param>
        /// <returns>KundeDto</returns>
        KundeDto createKunde(KundeDto kundeInput, long sysperole);

        /// <summary>
        /// createKunde Methode
        /// </summary>
        /// <param name="kundeInput">KundeDto</param>
        /// <param name="sysperole">User Id</param>
        /// <returns>KundeDto</returns>
        KundeDto createKundePerson(KundeDto kundeInput, long sysperole);

        /// <summary>
        /// updateKunde Methode
        /// </summary>
        /// <param name="kundeInput">KundeDto</param>
        /// <returns>KundeDto</returns>
        KundeDto updateKunde(KundeDto kundeInput);

        /// <summary>
        /// updateKunde Methode
        /// </summary>
        /// <param name="kundeInput">KundeDto</param>
        /// <returns>KundeDto</returns>
        KundeDto updateKundePerson(KundeDto kundeInput);

        /// <summary>
        /// getKunde Methode
        /// </summary>
        /// <param name="sysit">SYSIT</param>
        /// <returns>KundeDto</returns>
        KundeDto getKunde(long sysit);

         /// <summary>
        /// getKunde holt zu einer SYSIT den passenden Datensatz und liefert ihn mit zum Vertrag passenden Adressdaten zurück
        /// 
        /// 1. erstes PKZ/UKZ zum Antrg passend.
        /// 2. Ganz am Schluß PKZ/UKZ. Wenn Zustand 'Final' Mappen von PKZ/UKZ auf Kunden DTO.
        /// </summary>
        /// <param name="sysit">SYSIT des gewünschten Datensatzes</param>
        /// <param name="sysantrag">Antrag ID</param>
        /// <returns>KundeDto mit der gleichen SYSIT</returns>
        KundeDto getKundeViaAntragID(long sysit, long sysantrag);

        /// <summary>
        /// getKunde holt zu einer SYSKD den passenden Datensatz und liefert ihn zurück
        /// </summary>
        /// <param name="syskd">SYSKD des gewünschten Datensatzes</param>
        /// <returns>KundeDto mit der gleichen SYSKD</returns>
        KundeDto getKundeBySysKd(long syskd);

        /// <summary>
        /// Adresse via Interessenten ID holen
        /// </summary>
        /// <param name="sysit">Interessenten ID</param>
        /// <returns>Daten</returns>
        AdresseDto[] getAdressen(long sysit);

        /// <summary>
        /// Kontendaten via Interessenten ID holen
        /// </summary>
        /// <param name="sysit">Interessenten ID</param>
        /// <returns>Daten</returns>
        KontoDto[] getKonten(long sysit);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysantrag"></param>
        /// <param name="sysit"></param>
        /// <returns></returns>
        KundePlusDto getKundebySysAntrag(long? sysantrag, long? sysit);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysantrag"></param>
        /// <param name="sysit"></param>
        /// <returns></returns>
        KundePlusDto getItPlusbySysAntrag(long? sysantrag, long? sysit);


        /// <summary>
        /// altePKZLeerenBysysAntrag
        /// </summary>
        /// <param name="sysantrag"></param>
        /// <param name="sysit"></param>
        void altePKZLeerenBysysAntrag(long sysantrag, long sysAktuellIt);


        /// <summary>
        /// getKunde holt zu einer SYSIT den passenden Datensatz und liefert ihn zurück
        /// </summary>
        /// <param name="sysit">SYSIT des gewünschten Datensatzes</param>
        /// <param name="sysangebot">id des Angebots für pkz</param>
        /// <returns>KundeDto mit der gleichen SYSIT</returns>
        KundeDto getKunde(long sysit, long sysangebot);

        /// <summary>
        /// Merges all ITPKZ/ITUKZ-Fields into the corresponding PKZ/UKZ for the antrag, assuming the pkz/ukz is already there
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
