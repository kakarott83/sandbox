using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.Util.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cic.OpenLease.Service
{
    class QuoteInfo
    {
        public String bezeichnung { get; set; }
        public long sysquote { get; set; }
    }
    /// <summary>
    /// DAO for accessing the QUOTE Table
    /// </summary>
    [System.CLSCompliant(true)]
    public class QUOTEDao : IQuoteDao
    {
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Quote Identifiers used for AIDA Frontend
        /// they are all mandatory in the db!
        /// </summary>
        public const string QUOTE_NOVA_MARGIN = "NOVA_MARGIN";
        public const string QUOTE_NOVA_CO2_MARGIN = "NOVA_CO2_MARGIN";
        public const string QUOTE_NOVA_NOX_MARGIN = "NOVA_NOX_MARGIN";
        public const string QUOTE_NOVA_PARTICLES_MARGIN = "NOVA_PARTICLES_MARGIN";
        public const string QUOTE_NOVA_ZUSCHLAG = "NoVA_Zuschlag";
        public const string QUOTE_AufschlagGT = "SFAufschlagAnteilGroesserStandard";
        public const string QUOTE_AufschlagLT = "SFAufschlagAnteilKleinerStandard";
        public const string QUOTE_MAX_BEARBEITUNGSGEBUEHR = "MAX_BEARBEITUNGSGEBUEHR";
        public const string QUOTE_VSSTEUERPERSONENBEZOGEN = "VERSICHERUNGSSTEUER_PERSONENBEZOGEN";
        public const string QUOTE_VSSTEUER = "VERSICHERUNGSSTEUER";

        public const string QUOTE_MOTORVS_MIN_ALPHABET = "MOTORVS_Min_Alphabet";
        public const string QUOTE_MOTORVS_MAX_ALPHABET = "MOTORVS_Max_Alphabet";

        public const string QUOTE_RSV_MAXEIN_PREFIX = "RSV_MAXEINALTER_";
        public const string QUOTE_RSV_MAXABL_PREFIX = "RSV_MAXABLAUFALTER_";
        public const string QUOTE_RSV_MINEIN_PREFIX = "RSV_MINEINALTER_";
        public const string QUOTE_RSV_MAXVAL_PREFIX = "RSV_MAXVALUE_";


        public const string QUOTE_GAP_MAXEIN_PREFIX = "GAP_MAXEINALTER_";
        public const string QUOTE_GAP_MAXEND_PREFIX = "GAP_MAXENDALTER_";
        public const string QUOTE_GAP_MINEIN_PREFIX = "GAP_MINEINALTER_";


        public const string QUOTE_sysQuoteOb = "Restwert_Fallback"; //KM_Obergrenze ID
        public const string QUOTE_RisikoaufschlagReifen = "REIFEN_RISIKOAUFSCHLAG";
        public const string QUOTE_MinderKMToleranzgrenzeBezeichnung = "Minder_KM_Toleranzgrenze";
        public const string QUOTE_MehrKMToleranzgrenzeBezeichnung = "Mehr_KM_Toleranzgrenze";

        public const string QUOTE_KM_TOLERANZGRENZE = "KM_TOLERANZGRENZE";
        public const string QUOTE_MEHR_KM_SATZ = "MEHR_KM_SATZ";
        public const string QUOTE_MINDER_KM_SATZ = "MINDER_KM_SATZ";

        public const string QUOTE_LEAKALK_DEPOTABSCHLAG = "LEAKALK_DEPOTABSCHLAG";//Abschlag leasingprodukte in %
        public const string QUOTE_MAX_MVZ_VORVERTRAG = "MAX_MVZ_VORVERTRAG";//Summe der MVZ von allen Vorverträgen und akt Angebot darf diesen Satz in % nicht übersteigen
        public const string QUOTE_AUFLOESEANZEIGEBIS = "AUFLOESEANZEIGEBIS";//Anzeige Auflösewerte bis x Monate vor Vertragsende
        public const string QUOTE_MEHRKMSATZALPHABET = "Mehr_KM_Satz_Alphabet";
        public const string QUOTE_MINDERKMSATZALPHABET = "Minder_KM_Satz_Alphabet";
        public const string QUOTE_RWDEFAULTALPHABET = "RW_Default_Min_Alphabet";

        public const string QUOTE_NOVAABZUG_OTHER = "NOVAABZUG_OTHER";
        public const string QUOTE_NOVAABZUG_DIESEL = "NOVAABZUG_DIESEL";
        public const string QUOTE_NOVAABZUG_HYBRID = "NOVAABZUG_HYBRID";

        public const string QUOTE_WECHSELRHYTHMUSDEF_ALPHABET = "Wechselrhythmus_Def_Alphabet";
        public const string QUOTE_WECHSELRHYTHMUSMIN_ALPHABET = "Wechselrhythmus_Min_Alphabet";
        public const string QUOTE_WECHSELRHYTHMUSMAX_ALPHABET = "Wechselrhythmus_Max_Alphabet";
        public const string QUOTE_WECHSELRHYTHMUSSTEP_ALPHABET = "Wechselrhythmus_Step_Alphabet";
        
        public const string QUOTE_REIFENKORREKTUR_RISIKO_ALPHABET = "Reifenkorrektur_Risiko_Alphabet";
        public const string QUOTE_REIFENKORREKTUR_FIXLIMITIERT_ALPHABET = "Reifenkorrektur_FixLimitiert_Alphabet";
        public const string QUOTE_REIFENKORREKTUR_FIXUNLIMITIERT_ALPHABET = "Reifenkorrektur_FixUnlimitiert_Alphabet";
        public const string QUOTE_REIFENKORREKTUR_VARIABEL_ALPHABET = "Reifenkorrektur_Variabel_Alphabet";
		public const string QUOTE_ANGEBOT_GUELTIG_BIS_ALPHABET = "Angebot_GueltigBis_Alphabet";

        public const string QUOTE_B2B_MAX_ADD_Listenpreis = "B2B_MAX_ADD_Listenpreis";
        public const string QUOTE_B2B_PROV_MIN_LZ = "PROV_MIN_LZ";
    

        private IQuoteDao dao = CommonDaoFactory.getInstance().getQuoteDao(); 
        

        public QUOTEDao()
        {
        }

        /// <summary>
        /// Delivers the configured Quote for the given Quote id
        /// </summary>
        /// <param name="sysQuote">PK of table QUOTE</param>
        /// <returns></returns>
        public static decimal deliverQuotePercentValue(long sysQuote)
        {
            QUOTEDao qd = new QUOTEDao();
            return (decimal)qd.getQuote(sysQuote);
        }

        public static decimal deliverQuotePercentValueByName(string bezeichnung)
        {
            QUOTEDao qd = new QUOTEDao();
            return (decimal)qd.getQuote(bezeichnung);

        }

        public double getQuote(string name)
        {
            return dao.getQuote(name);
        }

        public double getQuote(long sysquote)
        {
            return dao.getQuote(sysquote);
        }

        public double getQuote(long sysquote, DateTime perDate)
        {
            return dao.getQuote(sysquote,perDate);
        }

        public double getQuote(string name, DateTime perDate)
        {
            return dao.getQuote(name,perDate);
        }

        public List<OpenOne.Common.DTO.QuoteInfoDto> getQuotes()
        {
            return dao.getQuotes();
        }

        public bool exists(string name, DateTime perDate)
        {
            return dao.exists(name, perDate);
        }
    }
}