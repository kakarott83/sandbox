using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// AdresseBo liefert und empfängt das AdresseDto
    /// </summary>
    public class AdresseBo : AbstractAdresseBo
    {
        /// <summary>
        /// Construktor
        /// </summary>
        /// <param name="aDao"></param>
        public AdresseBo(IAdresseDao aDao)
            : base(aDao)
        {
        }

        /// <summary>
        /// createOrUpdateAdresse entscheidet zwischen einem Create oder einem Update der Anfrage
        /// </summary>
        /// <param name="adresse">AdresseDto mit einer SYSADRESSE</param>
        /// <returns>AdresseDto</returns>
        public override AdresseDto createOrUpdateAdresse(AdresseDto adresse)
        {
            if (adresse.sysadresse == 0)
            {
                return createAdresse(adresse);

            }
            else
            {
                return updateAdresse(adresse);
            }
        }

        /// <summary>
        /// createAdresse leitet das AdresseDto weiter an das AdresseDAO
        /// </summary>
        /// <param name="adresse">AdresseDto mit einer SYSADRESSE = 0</param>
        /// <returns>AdresseDTO mit einer SYSADRESSE > 0</returns>
        public AdresseDto createAdresse(AdresseDto adresse)
        {
            return this.adresseDao.createAdresse(adresse);
        }

        /// <summary>
        /// updateAdresse leitet das AdresseDto weiter an das AdresseDAO
        /// </summary>
        /// <param name="adresse">AdresseDto mit einer SYSADRESSE > 0</param>
        /// <returns>AdresseDto mit der gleichen SYSADRESSE</returns>
        public AdresseDto updateAdresse(AdresseDto adresse)
        {
            return adresseDao.updateAdresse(adresse);

        }

        /// <summary>
        /// Liefert Ort, Kanton und Land für die Postleitzahl
        /// </summary>
        /// <param name="plz">Postleitzahl</param>
        /// <returns>PlzDto</returns>
        public override PlzDto[] findOrtByPlz(string plz)
        {
            return adresseDao.getPlz(plz);
        }

        /// <summary>
        /// removes the adress for the person
        /// </summary>
        /// <param name="sysperson"></param>
        public override void deleteAdresse(long sysperson)
        {
            adresseDao.deleteAdresse(sysperson);
        }
    }
}
