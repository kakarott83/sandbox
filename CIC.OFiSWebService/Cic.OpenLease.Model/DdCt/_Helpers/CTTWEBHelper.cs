// OWNER MK, 04-02-2010
namespace Cic.OpenLease.Model.DdCt
{
    #region Usings
    using System;
    using System.Linq;
    using Cic.OpenOne.Common.Util;
    #endregion

    public class CTTWEBHelper
    {

        #region Methods
        public static bool hasTranslations(DdCt.CtExtendedEntities context)
        {
            return (from cttweb in context.CTTWEB
                    select cttweb).Count()>0;
        }
        public static string DeliverTranslation(DdCt.CtExtendedEntities context, string term, string isoCode)
        {
            string TranslatedTerm = term;

            if (context == null) return TranslatedTerm;
            if (StringUtil.IsTrimedNullOrEmpty(term)) return TranslatedTerm;
            if (StringUtil.IsTrimedNullOrEmpty(isoCode)) return TranslatedTerm;

            var Query = from cttweb in context.CTTWEB
                        where cttweb.CTLANG.ISOCODE.Equals(isoCode) &&
                        cttweb.CTWEB.ORIGTERM.Equals(term)
                        select cttweb.REPLACETERM;

            string ReplacementTerm = Query.FirstOrDefault();

            if (!StringUtil.IsTrimedNullOrEmpty(ReplacementTerm))
            {
                TranslatedTerm = ReplacementTerm;
            }

            return TranslatedTerm;
        }
        #endregion

    }
}
