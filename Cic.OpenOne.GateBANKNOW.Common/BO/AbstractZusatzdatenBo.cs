using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.Model.DdOl;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Abkstrakte Zusatzdaten Business Objekt Klasse 
    /// </summary>
    public abstract class AbstractZusatzdatenBo : IZusatzdatenBo
    {
        /// <summary>
        /// Zusatzdaten DAO
        /// </summary>
        protected IZusatzdatenDao zusatzdatenDao;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="zusatzdatenDao">Zusatzdaten DAO</param>
        public AbstractZusatzdatenBo(IZusatzdatenDao zusatzdatenDao)
        {
            this.zusatzdatenDao = zusatzdatenDao;
        }

        /// <summary>
        /// Neue Zusatzdaten erzeugen oder bestehende Ändern
        /// </summary>
        /// <param name="zusatzdaten">Zusatzdaten</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Zusatzdaten Rückgabe</returns>
        public abstract ZusatzdatenDto createOrUpdateZusatzdaten(ZusatzdatenDto zusatzdaten, KundeDto kunde);

        /// <summary>
        /// Neue Zusatzdaten erzeugen oder bestehende Ändern
        /// </summary>
        /// <param name="zusatzdaten">Zusatzdaten</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Zusatzdaten Rückgabe</returns>
        public abstract ZusatzdatenDto createOrUpdateZusatzdatenPerson(ZusatzdatenDto zusatzdaten, KundeDto kunde);

        /// <summary>
        /// Erzeuge neue PKZ
        /// </summary>
        /// <param name="pkzInput">PKZ-Daten</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>PKZ-Daten</returns>
        public abstract PkzDto createPkz(PkzDto pkzInput, KundeDto kunde);

        /// <summary>
        /// Ändern der PKZ
        /// </summary>
        /// <param name="pkzInput">Neue PKZ-Daten</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Geänderte PKZ-Daten</returns>
        public abstract PkzDto updatePkz(PkzDto pkzInput, KundeDto kunde);

        /// <summary>
        /// Erzeuge neue UKZ
        /// </summary>
        /// <param name="ukzInput">UKZ-Daten</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Neue UKZ-Daten</returns>
        public abstract UkzDto createUkz(UkzDto ukzInput, KundeDto kunde);

        /// <summary>
        /// Ändern einer UKZ
        /// </summary>
        /// <param name="ukzInput">UKZ-Daten</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Neue UKZ-Daten</returns>
        public abstract UkzDto updateUkz(UkzDto ukzInput, KundeDto kunde);

        /// <summary>
        /// Erzeuge neue PKZ
        /// </summary>
        /// <param name="pkzInput">PKZ-Daten</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>PKZ-Daten</returns>
        public abstract PkzDto createPkzPerson(PkzDto pkzInput, KundeDto kunde);

        /// <summary>
        /// Ändern der PKZ
        /// </summary>
        /// <param name="pkzInput">Neue PKZ-Daten</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Geänderte PKZ-Daten</returns>
        public abstract PkzDto updatePkzPerson(PkzDto pkzInput, KundeDto kunde);

        /// <summary>
        /// Erzeuge neue UKZ
        /// </summary>
        /// <param name="ukzInput">UKZ-Daten</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Neue UKZ-Daten</returns>
        public abstract UkzDto createUkzPerson(UkzDto ukzInput, KundeDto kunde);

        /// <summary>
        /// Ändern einer UKZ
        /// </summary>
        /// <param name="ukzInput">UKZ-Daten</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Neue UKZ-Daten</returns>
        public abstract UkzDto updateUkzPerson(UkzDto ukzInput, KundeDto kunde);

        /// <summary>
        /// Zusatzdaten auslesen via ID Liste
        /// </summary>
        /// <param name="sysid">Liste mit Primärschlüsseln</param>
        /// <param name="kdtyp">Kundentyp ID</param>
        /// <returns>Zusatzdaten</returns>
        public abstract ZusatzdatenDto getZusatzdaten(long[] sysid, int kdtyp);

        /// <summary>
        ///  PKZ/UKZ aus dem letzten Antrag im Zustand 'Vertrag aktiviert' 
        /// </summary>
        /// <param name="sysit"></param>
        /// <param name="kdtyp"></param>
        /// <returns></returns>
        public abstract ZusatzdatenDto getZusatzdatenAktiv(long sysit, int kdtyp);
    }
}
