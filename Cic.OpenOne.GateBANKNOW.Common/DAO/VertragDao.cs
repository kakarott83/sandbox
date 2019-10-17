using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using AutoMapper;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Enums;
using Cic.OpenOne.Common.DTO;
using CIC.Database.OL.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// Vertrag DAO
    /// </summary>
    public class VertragDao : IVertragDao
    {
        /// <summary>
        /// 
        /// </summary>
        private const string QUERYVERTRAGSYSID = "select sysid from VT where sysantrag = :sysid";
        private const string QUERYVERTRAGVERTRAG = "select vertrag from VT where sysantrag = :sysid";
        private const string QUERYVERTRAGSYSVT = "select VT.sysantrag, VT.ende, VT.rw, VT.lz, VT.syskd, VT.vertrag as bezeichnung, VT.rate as aktuellerate from CIC.VT where sysid = :sysid";
        private const string QUERYGETRWGA = "Select sysrwga from VT where sysantrag = :sysid";
        private const string QUERYGETKUNDE = "Select syskd from VT where sysantrag = :sysid";
        private const string QUERYGETZUSTAND = "Select zustand from VT where sysid = :psysvt";
        private const string QUERYVERTRAGINRECHNUNG = "select distinct " +
                                                    " (CASE WHEN nvl((SELECT rn.sysrntyp FROM RN WHERE RN.sysid = " +
                                                    " (SELECT  max(rn.sysid) FROM RN WHERE RN.SysRNTYP IN " +
                                                    " (85, 97,102, 131,140, 141, 261, 318, 327, 328, 303) " +
                                                    " AND" +
                                                    " RN.Text not like '%Restrate' AND RN.StornoKZ = 0 AND RN.SysVT = VT.SysID)),0) not in (0,303) THEN 1 ELSE 0 END) " +
                                                    " from rn, vt where rn.sysvt=vt.sysid and vt.sysid=:psysid";
        private const string QUERYRWEMPENDERN = "Select count(*) from vt where  instr(vt.VERTRAG,'V') = 0 and sysvart=1 and vt.endekz=0 and vt.sysrwga = :psysperson and vt.sysantrag=:psysvt and (select count(*) from wftzust, wfzust where  wftzust.syswfzust = wfzust.syswfzust and wftzust.syslease = :psysvt and wfzust.syscode   like 'D_RWRE')=0";

        private const string QUERYISKAUFOFFERTEALLOW = "select  LISTAGG(wfzust.syscode,',') within Group (order by syscode) as syscode  from wfzust, wftzust where wftzust.syswfzust = wfzust.syswfzust and  wftzust.syslease = :psysvt group by syslease";
        /// <summary>
        /// Vertragdetails via ID auslesen
        /// </summary>
        /// <param name="sysid">Primary Key</param>
        /// <returns>Daten</returns>
        public VertragDto getVertragDetails(long sysid)
        {
            VertragDto vertragOutDto = new VertragDto();
            using (DdOlExtended context = new DdOlExtended())
            {
                VT vertrag = (from vt in context.VT
                              where vt.SYSID == sysid
                              select vt).FirstOrDefault();

                if (vertrag != null)
                {

                    vertragOutDto = Mapper.Map<VT, VertragDto>(vertrag);

                    vertragOutDto.syskd = vertrag.SYSKD;
                }
            }
            return vertragOutDto;
        }

        /// <summary>
        /// Vertragsschlüssel holen.
        /// </summary>
        /// <param name="sysid">Antrags ID</param>
        /// <returns>Vertragsschlüssel</returns>
        public long getVertragSysId(long sysid)
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysid });
                return ctx.ExecuteStoreQuery<long>(QUERYVERTRAGSYSID, parameters.ToArray()).FirstOrDefault();
                
            }
        }
        /// <summary>
        /// Vertragsnummer holen
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public String getVertragNummer(long sysid)
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysid });
                return ctx.ExecuteStoreQuery<String>(QUERYVERTRAGVERTRAG, parameters.ToArray()).FirstOrDefault();

            }
        }

        /// <summary>
        ///   Der Restsaldo (inkl. dem Restwert) des Vertrags wurde  in Rechnung gestellt ?
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public bool restsaldoInRechnung(long sysid)
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysid", Value = sysid });
                int flag = ctx.ExecuteStoreQuery<int>(QUERYVERTRAGINRECHNUNG, parameters.ToArray()).FirstOrDefault(); ;
                if (flag == 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// bool isRREChangeAllowed(long sysid, long sysperson)
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="sysperson"></param>
        /// <returns></returns>
        public bool isRREChangeAllowed(long sysid, long sysperson)
        {

            using (DdOlExtended ctx = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysvt", Value = sysid });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysperson", Value = sysperson });
                int flag = ctx.ExecuteStoreQuery<int>(QUERYRWEMPENDERN, parameters.ToArray()).FirstOrDefault();
                if (flag == 0)
                    return false;
            }
            return true;

        }

        /// <summary>
        /// bool isPerformKaufofferteAllowed(long sysid) sysid von antrag
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public bool isPerformKaufofferteAllowed(long sysid)
        {

            using (DdOlExtended ctx = new DdOlExtended())
            {
                long sysvt = getVertragSysId(sysid);
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysvt", Value = sysvt });
                string zustand = ctx.ExecuteStoreQuery<string>(QUERYGETZUSTAND, parameters.ToArray()).FirstOrDefault();
                if (!string.IsNullOrEmpty(zustand) && zustand.ToUpper().Equals("AKTIV RECOVERY")) return false;
                parameters.Clear();
                /*parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysvt", Value = sysvt });
                string wfzustSyscode = ctx.ExecuteStoreQuery<string>(QUERYISKAUFOFFERTEALLOW, parameters.ToArray()).FirstOrDefault();
                if (wfzustSyscode != null && wfzustSyscode.Contains(("OFFERTE_RÜCKZ"))) return false;*/

            }
            return true;

        }

        public long? getRwga(long sysid)
        {
            long? sysrwga = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysid });
                sysrwga = ctx.ExecuteStoreQuery<long?>(QUERYGETRWGA, parameters.ToArray()).FirstOrDefault();

            }
            return sysrwga;

        }

        public long? getKunde(long sysid)
        {
            long? kunde = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysid });
                kunde = ctx.ExecuteStoreQuery<long>(QUERYGETKUNDE, parameters.ToArray()).FirstOrDefault();

            }
            return kunde;

        }

        /// <summary>
        /// gets contract details by its id.
        /// function created so that we can change the query and thus decide what data is given in what field, customizable.
        /// </summary>
        /// <param name="sysvt">contract id</param>
        /// <returns>contract details</returns>
        public VertragDto getVertragForExtension(long sysvt)
        {
            VertragExtDto vt = null;

            EaiparDao eaiParDao = new EaiparDao();
            String query = eaiParDao.getEaiParFileByCode("VT2ANTRAG", QUERYVERTRAGSYSVT);


            using (DdOlExtended ctx = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysvt });
                vt = ctx.ExecuteStoreQuery<VertragExtDto>(query, parameters.ToArray()).FirstOrDefault();
            }
            return vt;
        }

          /// <summary>
        /// getMWST
        /// </summary>
        /// <param name="sysvt"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        public double getMWST(long sysvt, DateTime perDate)
        {


            DateTime nullDate = new DateTime(1800, 1, 1);
            string QUERYMWST = "select  MWST.PROZENT, mwstdate.sysmwstdate,mwstdate.sysmwst, mwstdate.gueltigab, " +
                "mwstdate.prozent ProzentAkt, mwstdate.sysskonto, mwstdate.mwstfibu, mwstdate.evalskonto from vt, mwst, mwstdate where VT.SYSMWST=MWST.SYSMWST and vt.sysmwst=mwstdate.sysmwst and vt.sysid=:sysvt ";

            using (DdOlExtended context = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvt", Value = sysvt });

                List<MwStDataDTO> Mehrwertsteuersaetze = context.ExecuteStoreQuery<MwStDataDTO>(QUERYMWST, parameters.ToArray()).ToList();

                return (from data in Mehrwertsteuersaetze
                        where (data.GueltigAb == null || data.GueltigAb <= perDate || data.GueltigAb <= nullDate)
                        orderby data.GueltigAb descending
                        select data.ProzentAkt).FirstOrDefault();
            }
        }

    }

}
