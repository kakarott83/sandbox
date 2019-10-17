using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Logging;
using CIC.Database.OL.EF4.Model;

namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// Korrektur Data Access Object
    /// </summary>
    public class KorrekturDao : IKorrekturDao
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const string QUERYKORRTYP = "select * from korrtyp";
        private const string QUERYKORREKTUR = "select * from korrektur where syskorrtyp = :syskorrtyp";

        /// <summary>
        /// Standard Constructor
        /// Database access Object for Korrektur
        /// </summary>
        public KorrekturDao()
        {
        }

        /// <summary>
        /// Get all KORRTYP
        /// </summary>
        /// <returns></returns>
        virtual public List<KORRTYP> getKorrekturTypen()
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {
                return ctx.ExecuteStoreQuery<KORRTYP>(QUERYKORRTYP, null).ToList();
            }
        }

        /// <summary>
        /// Get KORREKTUR for korrtyp
        /// </summary>
        /// <returns></returns>
        virtual public List<KORREKTUR> getKorrekturen(long syskorrtyp)
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {
                object[] pars = { new Devart.Data.Oracle.OracleParameter { ParameterName = "syskorrtyp", Value = syskorrtyp } };
                return ctx.ExecuteStoreQuery<KORREKTUR>(QUERYKORREKTUR, pars).ToList();
            }
        }
    }
}