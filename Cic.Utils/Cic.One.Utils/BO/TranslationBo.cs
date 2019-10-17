
namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// TranslationBo-Klasse
    /// </summary>
    public class TranslationBo
    {
        /*
        	STAATDtoLoop.STAAT1 = Cic.OpenLease.Model.DdCt.CTTWEBHelper.DeliverTranslation(context, STAATDtoLoop.STAAT1, ServiceValidator.ISOLanguageCode);
		public static string DeliverTranslation(DdCt.CtExtendedEntities context, string term, string isoCode)
        {
            string TranslatedTerm = term;

            if (context == null) return TranslatedTerm;
            if (Cic.Basic.StringHelper.IsTrimedNullOrEmpty(term)) return TranslatedTerm;
            if (Cic.Basic.StringHelper.IsTrimedNullOrEmpty(isoCode)) return TranslatedTerm;

            var Query = from cttweb in context.CTTWEB
                        where cttweb.CTLANG.ISOCODE.Equals(isoCode) &&
                        cttweb.CTWEB.ORIGTERM.Equals(term)
                        select cttweb.REPLACETERM;

            string ReplacementTerm = Query.FirstOrDefault();

            if (!Cic.Basic.StringHelper.IsTrimedNullOrEmpty(ReplacementTerm))
            {
                TranslatedTerm = ReplacementTerm;
            }

            return TranslatedTerm;
        }
         * */
    }
}