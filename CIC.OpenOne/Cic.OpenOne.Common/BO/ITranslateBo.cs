using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// Translate BO Schnittstelle
    /// </summary>
    public interface ITranslateBo
    {
        /// <summary>
        /// Übersetze Liste
        /// </summary>
        /// <param name="List">Eingangsliste</param>
        /// <param name="Area">Übersetzungsbereich</param>
        /// <param name="isoCode">ISO Code</param>
        /// <returns>Ausgangsliste (übersetzt)</returns>
        DropListDto[] TranslateList(DropListDto[] List, TranslateArea Area, string isoCode);


        /// <summary>
        /// Get List of static Translation Entries
        /// </summary>
        /// <returns>Translation List</returns>
        List<TranslationDto> GetStaticList();

        /// <summary>
        /// Get List of static Translation Entries for One Web
        /// </summary>
        /// <returns>Translation List</returns>
        List<TranslationDto> GetStaticList2();

        /// <summary>
        /// Translate a Message
        /// </summary>
        /// <param name="messageCode">ID of Message to translate</param>
        /// <param name="isoCode">Iso Code of language</param>
        /// <param name="Default">Standardwert</param>
        /// <returns></returns>
        String translateMessage(String messageCode, String isoCode, String Default);
    }
}
