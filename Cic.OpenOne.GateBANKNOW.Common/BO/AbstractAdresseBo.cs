using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Abstraktes Adressen Business Objekt
    /// </summary>
    public abstract class AbstractAdresseBo : IAdresseBo
    {
        /// <summary>
        /// Data Access Object for Offer/Application
        /// </summary>
        protected IAdresseDao adresseDao;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="adresseDao"></param>
        public AbstractAdresseBo(IAdresseDao adresseDao)
        {
            this.adresseDao = adresseDao;
        }

        /// <summary>
        /// Erstellen oder Ändern eines Adresse Objekts
        /// </summary>
        /// <param name="adresse">Adresse Datenübertragungsobjekt</param>
        /// <returns>Rückgabe des neuen oder geänderten Adressen Objekts</returns>
        public abstract AdresseDto createOrUpdateAdresse(AdresseDto adresse);

        /// <summary>
        /// Liefert Ort, Kanton und Land für die Postleitzahl
        /// </summary>
        /// <param name="plz">Postleitzahl</param>
        /// <returns>PlzDto</returns>
        public abstract PlzDto[] findOrtByPlz(string plz);

        /// <summary>
        /// removes the adress for the person
        /// </summary>
        /// <param name="sysperson"></param>
        public abstract void deleteAdresse(long sysperson);
    }
}
