using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.Model.DdOl;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// Zusatzdaten DAO Schnittstelle
    /// </summary>
    public interface IZusatzdatenDao
    {
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
        /// <param name="pkzInput">PKZ Eingang</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Daten</returns>
        PkzDto updatePkz(PkzDto pkzInput, KundeDto kunde);

        /// <summary>
        /// UKZ erzeugen
        /// </summary>
        /// <param name="ukzInput">UKZ Eingang</param>
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
        /// <param name="pkzInput">PKZ Eingang</param>
        /// <param name="kunde">Kundendaten</param>
        /// <returns>Daten</returns>
        PkzDto updatePkzPerson(PkzDto pkzInput, KundeDto kunde);

        /// <summary>
        /// UKZ erzeugen
        /// </summary>
        /// <param name="ukzInput">UKZ Eingang</param>
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
        /// PKZ via ID holen
        /// </summary>
        /// <param name="sysid">Primary key</param>
        /// <returns>Daten</returns>
        PkzDto getPkz(long sysid);

        /// <summary>
        /// UKZ via ID holen
        /// </summary>
        /// <param name="sysid">Primary Key</param>
        /// <returns>Daten</returns>
        UkzDto getUkz(long sysid);


        /// <summary>
        /// PKZ via ID holen
        /// </summary>
        /// <param name="sysid">Primary key</param>
        /// <returns>Daten</returns>
        PkzDto getPkzPerson(long sysid);

        /// <summary>
        /// UKZ via ID holen
        /// </summary>
        /// <param name="sysid">Primary Key</param>
        /// <returns>Daten</returns>
        UkzDto getUkzPerson(long sysid);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysit"></param>
        /// <returns></returns>
        PkzDto getITPkzAktiv(long sysit);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysit"></param>
        /// <returns></returns>
        UkzDto getITUkzAktiv(long sysit);

                /// <summary>
        /// returns the youngest checked approval pkz/ukz for the person or the newest one
        /// </summary>
        /// <param name="sysperson"></param>
        /// <returns></returns>
        ZusatzdatenDto[] getPersonZusatzdaten(long sysperson);

        /// <summary>
        /// Creates/Updates or deletes the kne
        /// </summary>
        /// <param name="kne"></param>
        /// <returns></returns>
        KneDto createOrUpdateKne(KneDto kne);
    }
}
