using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Adresse BO Schnittstelle
    /// </summary>
    public interface IAdresseBo
    {
        /// <summary>
        /// Neue Adresse erstellen oder vorhandene Ändern
        /// </summary>
        /// <param name="adresse">Adressdaten</param>
        /// <returns>Neue oder geänderte Adressdaten</returns>
        AdresseDto createOrUpdateAdresse(AdresseDto adresse);

        /// <summary>
        /// Liefert Ort, Kanton und Land für die Postleitzahl
        /// </summary>
        /// <param name="plz">Postleitzahl</param>
        /// <returns>PlzDto</returns>
        PlzDto[] findOrtByPlz(string plz);

        /// <summary>
        /// removes the adress for the person
        /// </summary>
        /// <param name="sysperson"></param>
        void deleteAdresse(long sysperson);
    }
}
