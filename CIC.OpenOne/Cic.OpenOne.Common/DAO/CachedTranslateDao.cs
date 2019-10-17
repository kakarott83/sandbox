using System;
using System.Collections.Generic;
using System.Reflection;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Logging;

namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// UebersetzungsDao
    /// </summary>
    public class CachedTranslateDao : TranslateDao
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static CacheDictionary<String, List<CTLUT_Data>> readoutTransCache = CacheFactory<String, List<CTLUT_Data>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Translation), CacheCategory.Translation);
        private static CacheDictionary<String, List<TranslationDto>> staticListCache = CacheFactory<String, List<TranslationDto>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Translation), CacheCategory.Translation);
        private static CacheDictionary<String, DropListDto[]> ddlkpposCache = CacheFactory<String, DropListDto[]>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Translation), CacheCategory.Translation);
        private static CacheDictionary<String, List<MessageTranslateDto>> msgTranslationCache = CacheFactory<String, List<MessageTranslateDto>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Translation), CacheCategory.Translation);

        /// <summary>
        /// CachedTranslateDao Constructor
        /// </summary>
        public CachedTranslateDao()
        {
        }

        /// <summary>
        /// Read the entire Translation List 
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        override public List<CTLUT_Data> readoutTranslationList(String Area, String isoCode)
        {
            String key = Area + "_" + isoCode;
            if (!readoutTransCache.ContainsKey(key))
            {
                readoutTransCache[key] = base.readoutTranslationList(Area, isoCode);
            }
            return readoutTransCache[key];
        }

        /// <summary>
        /// Get List of static Translation Entries
        /// </summary>
        /// <returns>Translation List</returns>
        override public List<TranslationDto> GetStaticList()
        {
            String key = "STATICLIST";
            if (!staticListCache.ContainsKey(key))
            {
                staticListCache[key] = base.GetStaticList();
            }
            return staticListCache[key];
        }

        /// <summary>
        /// Get List of static Translation Entries for One Web
        /// </summary>
        /// <returns>Translation List</returns>
        override public List<TranslationDto> GetStaticList2()
        {
            String key = "STATICLIST2";
            if (!staticListCache.ContainsKey(key))
            {
                staticListCache[key] = base.GetStaticList2();
            }
            return staticListCache[key];
        }

        /// <summary>
        /// finds a dictionary entry in DDLKPPOS by its CODE
        /// </summary>
        /// <param name="code">the CODE of the dictionary entries</param>
        /// <param name="isoCode">the isoCode of the language</param>
        /// <param name="domainId">the domainId</param>
        /// <returns>array of DropListDtos</returns>
        override public DropListDto[] findByDDLKPPOSCode(DDLKPPOSType code, String isoCode, String domainId)
        {
            String key = code + "_" + isoCode + "_" + domainId;
            if (!ddlkpposCache.ContainsKey(key))
            {
                ddlkpposCache[key] = base.findByDDLKPPOSCode(code, isoCode, domainId);
            }
            return ddlkpposCache[key];
        }

        /// <summary>
        /// Get List of static Translation Entries
        /// </summary>
        /// <returns>Translation List</returns>
        override public List<MessageTranslateDto> readoutMessagetranslation(String MessageCode, String isoCode)
        {
            String key = MessageCode + "_" + isoCode;
            if (!msgTranslationCache.ContainsKey(key))
            {
                msgTranslationCache[key] = base.readoutMessagetranslation(MessageCode, isoCode);
            }
            return msgTranslationCache[key];
        }
    }
}