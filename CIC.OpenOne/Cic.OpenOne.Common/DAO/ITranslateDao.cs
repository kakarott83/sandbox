using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// Übersetzungs Daten Zugriffs Object Schnittstelle
    /// </summary>
    public interface ITranslateDao
    {
        /// <summary>
        /// Die Übersetzungsdaten einlesen
        /// </summary>
        /// <param name="Area">Area</param>
        /// <param name="isoCode">ISO Code</param>
        /// <returns>Übersetzungen in der Liste</returns>
        List<CTLUT_Data> readoutTranslationList(String Area, String isoCode);

        /// <summary>
        /// Übersetzungsdatensatz nach Bezeichner holen
        /// </summary>
        /// <param name="OrigTerm">Originaler bezeichenr</param>
        /// <param name="Area">Ünbersetzungbereich</param>
        /// <param name="translations">Übersetzungsliste</param>
        /// <returns>Datensatz</returns>
        CTLUT_Data RetrieveEntry(string OrigTerm, string Area, List<CTLUT_Data> translations);

        /// <summary>
        /// Übersetzungsdatensatz nach Schlüssel ID holen
        /// </summary>
        /// <param name="ID">Schlüssel ID</param>
        /// <param name="Area">Übersetzungsbereich</param>
        /// <param name="translations">Übersetzungsliste</param>
        /// <returns>Datensatz</returns>
        CTLUT_Data RetrieveEntry(long ID, string Area, List<CTLUT_Data> translations);

        /// <summary>
        /// Liste übersetzen
        /// </summary>
        /// <param name="List">Dropdown Liste</param>
        /// <param name="Area">Übersetzungsbereich</param>
        /// <param name="translations">Übersetzungsliste</param>
        /// <returns>Übersetzte Liste</returns>
        DropListDto[] TranslateList(DropListDto[] List, string Area, List<CTLUT_Data> translations);

        /// <summary>
        /// Get List of static Translation Entries
        /// </summary>
        /// <returns>Translation List</returns>
        List<TranslationDto> GetStaticList();

        /// <summary>
        /// Get List of static Translation Entries for OneWeb
        /// </summary>
        /// <returns>Translation List</returns>
        List<TranslationDto> GetStaticList2();

        /// <summary>
        /// Get List of static Translation Entries
        /// </summary>
        /// <returns>Translation List</returns>
        List<MessageTranslateDto> readoutMessagetranslation(String MessageCode, String isoCode);
    }
}
