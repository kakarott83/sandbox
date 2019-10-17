// OWNER MK, 04-02-2010
namespace Cic.OpenLease.Service
{
    #region Using
    using System.Linq;
    using CIC.Database.OW.EF6.Model;


    using System;
    using Cic.OpenOne.Common.Model.DdOw;
    using Cic.OpenOne.Common.Model.DdCt;
    using Cic.OpenOne.Common.Util;

    #endregion

    public class CTTWEBHelper
    {

        #region Methods
        public static bool hasTranslations(DdCtExtended context)
        {
            return (from cttweb in context.CTTWEB
                    select cttweb).Count()>0;
        }
        public static string DeliverTranslation(DdCtExtended context, string term, string isoCode)
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
