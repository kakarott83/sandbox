using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// Adressen DAO Schnittstelle
    /// </summary>
    public interface IAdresseDao
    {
        /// <summary>
        /// Neue Adresse erzeugen
        /// </summary>
        /// <param name="adresseInput">Eingangsdaten</param>
        /// <returns>Neu erzeugtes Objekt</returns>
        AdresseDto createAdresse(AdresseDto adresseInput);

        /// <summary>
        /// Bestehende Adresse ändern
        /// </summary>
        /// <param name="adresseInput">Eingangsdaten</param>
        /// <returns>gespeicherte Daten</returns>
        AdresseDto updateAdresse(AdresseDto adresseInput);

        /// <summary>
        /// removes the address for the person
        /// </summary>
        /// <param name="sysperson"></param>
        void deleteAdresse(long sysperson);

        /// <summary>
        /// Adresse via ID holen
        /// </summary>
        /// <param name="sysid">Primärschlüssel</param>
        /// <returns>Adressdaten</returns>
        AdresseDto getAdresse(long sysid);

        /// <summary>
        /// Holt den Datensatz zu Postleitzahl
        /// </summary>
        /// <param name="plz">Postleitzahl</param>
        /// <returns>PlzDtoArray</returns>
        PlzDto[] getPlz(string plz);
    }
}
