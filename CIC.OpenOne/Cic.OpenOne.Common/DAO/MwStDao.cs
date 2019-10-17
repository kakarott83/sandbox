using System;
using System.Collections.Generic;
using System.Linq;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// Mehrwertsteuerermittlungs DAO
    /// </summary>
    public class MwStDao : IMwStDao
    {
        const String GETMWSTBYVART = "select  MWST.PROZENT, mwstdate.sysmwstdate,mwstdate.sysmwst, mwstdate.gueltigab, mwstdate.prozent ProzentAkt, mwstdate.sysskonto, mwstdate.mwstfibu, mwstdate.evalskonto from vart, mwst, mwstdate where VART.SYSMWST=MWST.SYSMWST and vart.sysmwst=mwstdate.sysmwst and vart.sysvart=:sysvart";

        const String GETMWSTGLOBAL = "SELECT mwst.PROZENT, mwstdate.sysmwstdate,mwstdate.sysmwst, mwstdate.gueltigab, mwstdate.prozent ProzentAkt, mwstdate.sysskonto, mwstdate.mwstfibu, mwstdate.evalskonto FROM mwst, mwstdate, LSADD WHERE lsadd.sysmwst = mwst.sysmwst AND LSADD.sysmwst = mwstdate.sysmwst AND LSADD.syslsadd = :sysls";

        private DateTime nullDate = new DateTime(1800, 1, 1);

        /// <summary>
        /// Ermitteln der Mehrwertsteuer aus der Vertragsart
        /// </summary>
        /// <param name="sysvart">Vertragsart ID</param>
        /// <param name="perDate">Datum der Gültigkeit</param>
        /// <returns>Mehrwertsteuer</returns>
        virtual public double getMehrwertSteuer(long sysvart, DateTime perDate)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvart", Value = sysvart });

                List<MwStDataDTO> Mehrwertsteuersaetze = context.ExecuteStoreQuery<MwStDataDTO>(GETMWSTBYVART, parameters.ToArray()).ToList();

                return (from data in Mehrwertsteuersaetze
                        where (data.GueltigAb == null || data.GueltigAb <= perDate || data.GueltigAb <= nullDate)
                        orderby data.GueltigAb descending
                        select data.ProzentAkt).FirstOrDefault();
            }
        }

        /// <summary>
        /// Returns the global Ust Value of MWST-Table, defined by the code in the Configsection AIDA/GENERAL/USTCODE
        /// </summary>
        /// <param name="sysls">Mandanten ID</param>
        /// <param name="perDate">Datum der Gültigkeit</param>
        /// <returns>Umsatzsteuer</returns>
        virtual public double getGlobalUst(long sysls, DateTime perDate)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysls", Value = sysls });

                List<MwStDataDTO> Mehrwertsteuersaetze = context.ExecuteStoreQuery<MwStDataDTO>(GETMWSTGLOBAL, parameters.ToArray()).ToList();

                return (from data in Mehrwertsteuersaetze
                        where (data.GueltigAb == null || data.GueltigAb <= perDate || data.GueltigAb <= nullDate)
                        orderby data.GueltigAb descending
                        select data.ProzentAkt).FirstOrDefault();
            }
        }
    }
}