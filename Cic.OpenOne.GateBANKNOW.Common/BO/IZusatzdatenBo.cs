using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.Model.DdOl;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Zusatzdaten BO Schnittstelle
    /// </summary>
    public interface IZusatzdatenBo
    {
        /// <summary>
        /// Zusatzdaten erzeugen oder ändern
        /// </summary>
        /// <param name="zusatzdaten">Eingangsdaten</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Daten</returns>
        ZusatzdatenDto createOrUpdateZusatzdaten(ZusatzdatenDto zusatzdaten, KundeDto kunde);

        /// <summary>
        /// Neue Zusatzdaten erzeugen oder bestehende Ändern
        /// </summary>
        /// <param name="zusatzdaten">Zusatzdaten</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Zusatzdaten Rückgabe</returns>
        ZusatzdatenDto createOrUpdateZusatzdatenPerson(ZusatzdatenDto zusatzdaten, KundeDto kunde);

        /// <summary>
        /// PKZ erzeugen
        /// </summary>
        /// <param name="pkzInput">PKZ Eingang</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Daten</returns>
        PkzDto createPkz(PkzDto pkzInput, KundeDto kunde);

        /// <summary>
        /// PKZ ändern
        /// </summary>
        /// <param name="pkzInput">PKZ Eingabe</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Daten</returns>
        PkzDto updatePkz(PkzDto pkzInput, KundeDto kunde);

        /// <summary>
        /// UKZ erzeugen
        /// </summary>
        /// <param name="ukzInput">UKZ Eingabe</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Daten</returns>
        UkzDto createUkz(UkzDto ukzInput, KundeDto kunde);

        /// <summary>
        /// UKZ ändern
        /// </summary>
        /// <param name="ukzInput">UKZ Eingabe</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Daten</returns>
        UkzDto updateUkz(UkzDto ukzInput, KundeDto kunde);


        /// <summary>
        /// PKZ erzeugen
        /// </summary>
        /// <param name="pkzInput">PKZ Eingang</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Daten</returns>
        PkzDto createPkzPerson(PkzDto pkzInput, KundeDto kunde);

        /// <summary>
        /// PKZ ändern
        /// </summary>
        /// <param name="pkzInput">PKZ Eingabe</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Daten</returns>
        PkzDto updatePkzPerson(PkzDto pkzInput, KundeDto kunde);

        /// <summary>
        /// UKZ erzeugen
        /// </summary>
        /// <param name="ukzInput">UKZ Eingabe</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Daten</returns>
        UkzDto createUkzPerson(UkzDto ukzInput, KundeDto kunde);

        /// <summary>
        /// UKZ ändern
        /// </summary>
        /// <param name="ukzInput">UKZ Eingabe</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Daten</returns>
        UkzDto updateUkzPerson(UkzDto ukzInput, KundeDto kunde);

        /// <summary>
        /// Zusatzdaten via ID holen
        /// </summary>
        /// <param name="sysid">Primary key-Liste</param>
        /// <param name="syskd">KundenID</param>
        /// <returns>Daten</returns>
        ZusatzdatenDto getZusatzdaten(long[] sysid, int syskd);

        /// <summary>
        ///  PKZ/UKZ aus dem letzten Antrag im Zustand 'Vertrag aktiviert' 
        /// </summary>
        /// <param name="sysit"></param>
        /// <param name="kdtyp"></param>
        /// <returns></returns>
        ZusatzdatenDto getZusatzdatenAktiv(long sysit, int kdtyp);
    }
}
