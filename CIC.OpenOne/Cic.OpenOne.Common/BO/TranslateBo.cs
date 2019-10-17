using System;
using System.Collections.Generic;
using System.Linq;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DAO;

namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// Enum der Übersetzungsbereiche
    /// </summary>
    public enum TranslateArea
    {
        /// <summary>
        /// verfügbare Kanäle
        /// </summary>
        BCHANNEL,
        /// <summary>
        /// Kundentyp
        /// </summary>
        KDTYP,
        /// <summary>
        /// Nutzungsart
        /// </summary>
        OBUSETYPE,
        /// <summary>
        /// Objektart
        /// </summary>
        OBART,
        /// <summary>
        /// PRPRODUCT
        /// </summary>
        PRPRODUCT,
        /// <summary>
        /// Sprachen
        /// </summary>
        CTLANG,
        /// <summary>
        /// Kantone
        /// </summary>
        STAAT,
        /// <summary>
        /// Länder und Sprachen
        /// </summary>
        LAND,
        /// <summary>
        /// Objekttyp
        /// </summary>
        OBTYP,
    }

    /// <summary>
    /// Translate BO
    /// </summary>
    public class TranslateBo : AbstractTranslateBo
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="pDao">Translate DAO</param>
        public TranslateBo(ITranslateDao pDao)
            : base(pDao)
        {
        }

        /// <summary>
        /// Übersetzen einer Liste von DropList DTO's
        /// </summary>
        /// <param name="List">Eingangsliste</param>
        /// <param name="Area">Übersetzungsbereich</param>
        /// <param name="isoCode">ISO Code</param>
        /// <returns>Ausgangsliste (übersetzt)</returns>
        public override DropListDto[] TranslateList(DropListDto[] List, TranslateArea Area, string isoCode)
        {
            List<CTLUT_Data> translations = pDao.readoutTranslationList(Area.ToString(), isoCode);
            return pDao.TranslateList(List, Area.ToString(), translations);
        }

        /// <summary>
        /// Get List of static Translation Entries
        /// </summary>
        /// <returns>Translation List</returns>
        public override List<TranslationDto> GetStaticList()
        {
            return pDao.GetStaticList();
        }

        /// <summary>
        /// Get List of static Translation Entries
        /// </summary>
        /// <returns>Translation List</returns>
        public override List<TranslationDto> GetStaticList2()
        {
            return pDao.GetStaticList2();
        }

        /// <summary>
        /// Translate a Message
        /// </summary>
        /// <param name="messageCode">ID of Message to translate</param>
        /// <param name="isoCode">Iso Code of language</param>
        /// <param name="Default">Standardwert</param>
        /// <returns></returns>
        public override String translateMessage(String messageCode, String isoCode, String Default)
        {
            try
            {
                List<MessageTranslateDto> Data = pDao.readoutMessagetranslation(messageCode, isoCode);
                if (Data != null && Data.Count > 0)
                    return Data.FirstOrDefault().ReplacementMessageText;
                else
                    return Default;
            }
            catch (Exception ex)
            {
                string text = ex.Message;
                return Default;
            }
        }
    }
}