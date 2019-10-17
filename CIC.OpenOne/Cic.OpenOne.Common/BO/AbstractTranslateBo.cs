using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// Abstrakte Übersetzungs BO Klasse
    /// </summary>
    public abstract class AbstractTranslateBo : ITranslateBo
    {
        /// <summary>
        /// Übersetzungs BO via Schnittstelle
        /// </summary>
        protected ITranslateDao pDao;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="pDao"></param>
        public AbstractTranslateBo(ITranslateDao pDao)
        {
            this.pDao = pDao;
        }

        /// <summary>
        /// Übersetze Liste
        /// </summary>
        /// <param name="List">Eingangsliste</param>
        /// <param name="Area">Übersetzungsbereich</param>
        /// <param name="isoCode">ISO Code</param>
        /// <returns>Ausgangsliste (übersetzt)</returns>
        public abstract DropListDto[] TranslateList(DropListDto[] List, TranslateArea Area, string isoCode);

        /// <summary>
        /// Get List of static Translation Entries
        /// </summary>
        /// <returns>Translation List</returns>
        public abstract List<TranslationDto> GetStaticList();

        /// <summary>
        /// Get List of static Translation Entries for One Web
        /// </summary>
        /// <returns>Translation List</returns>
        public abstract List<TranslationDto> GetStaticList2();

        /// <summary>
        /// Translate a Message
        /// </summary>
        /// <param name="messageCode">ID of Message to translate</param>
        /// <param name="isoCode">Iso Code of language</param>
        /// <param name="Default">Standardwert</param>
        /// <returns></returns>
        public abstract String translateMessage(String messageCode, String isoCode, String Default);
    }
}
