using AutoMapper;
using Cic.OpenOne.Common.Model.DdIc;
using Cic.OpenOne.Common.Model.DdOd;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using CIC.Database.OW.EF6.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    public class KundenRisikoDao : IKundenRisikoDao
    {

        private const string ZINSERTRAG_QUERY = "select vg.sysvg from vg,vgtype where vg.sysvgtype=vgtype.sysvgtype and vgtype.name='Expected Loss' and vg.name='ERW_ZINSERTRAG'";
        private const string LZ_QUERY = "select v1 from cic.vgvalues_v where sysvg=(select vg.sysvg from vg,vgtype where vg.sysvgtype=vgtype.sysvgtype and vgtype.name='Expected Loss' and vg.name='LAUFZEIT') order by y1";
        private const string KREDITBETRAG_QUERY = "select person.gebdatum,ratebrutto,rsvgesamt,case when antrag.vertriebsweg='Barkredit direkt' then 'KFD' when antrag.vertriebsweg='Barkredit vermittelt' then 'KFV' when antrag.vertriebsweg='MA Barkredit direkt' then 'MAKFD' else '' end channel, antkalk.bginternbrutto,antkalk.szbrutto, antkalk.zins,antkalk.lz,antkalk.bgexternbrutto from antkalk,antrag,person where person.sysperson=antrag.syskd and antkalk.sysantrag=antrag.sysid and antkalk.SYSANTRAG=:sysid";
        private const string FINFAEHIG_QUERY = "select trim(attribut) from antrag where sysid=:sysid";
        private const string CREDITLIMIT_QUERY = "select antkalkvar.provision, antkalkvar.provisionp, antkalkvar.ratebrutto rate, antkalkvar.ratenabsicherung, antkalkvar.status,antkalkvar.laufzeit,antkalkvar.bginternbrutto creditLimit,antkalkvar.sysprproduct from antkalkvar,antkalk where antkalkvar.syskalk=antkalk.syskalk and antkalk.sysantrag=:sysid order by antkalkvar.rang, antkalkvar.laufzeit";
        
        private const string HAS_MA_QUERY = "select 1 as IsMa from antobsich where  rang in (120,130,800) and sysantrag=:sysid";
        private const string BUDGETQUERY = "select * from Kremo where sysantrag=:sysantrag order by syskremo desc ";

        private const string DEENGINE_VALUE_QUERY = @"SELECT dd.FAKTORZ ZFAKTOR, dd.PD,dd.CLUSTERVALUE,dd.FREIBETRAG,dd.SCOREBEZEICHNUNG, dd.SCORETOTAL, nvl(dd.BUDGETPUFFER,0)  BUDGETPUFFER 
                                                            FROM risikokl rk,
                                                              deoutexec dx,
                                                              dedetail dd
                                                            WHERE dx.SYSDEOUTEXEC = dd.SYSDEOUTEXEC
                                                            AND dd.RISIKOKLASSEID =rk.sysrisikokl
                                                            AND dd.ANTRAGSTELLER = 1
                                                            AND dx.sysauskunft    =
                                                              (SELECT max(auskunft.sysauskunft)
                                                              FROM auskunft,
                                                                deenvinp,
                                                                deinpexec
                                                              WHERE auskunft.statusnum         = 0
                                                              AND auskunft.area                  = 'ANTRAG'
                                                              AND auskunft.sysauskunfttyp        = 3
                                                              AND deenvinp.flagbonitaetspruefung = 1
                                                              AND auskunft.sysauskunft           =deinpexec.sysauskunft
                                                              AND deenvinp.sysdeinpexec          =deinpexec.sysdeinpexec
                                                              AND auskunft.sysid                 =:sysid
                                                              )";

        /// <summary>
        /// Budget Daten aus KREMO für Simulation / risikoprüfungsrelevanten Daten für DecisionEngine 
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public Budget4DESimDto getBudget4DESim(long sysid)
        {
            Budget4DESimDto budget4DESimDto = new Budget4DESimDto();

            using (DdOlExtended context = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysantrag", Value = sysid });
                CIC.Database.OL.EF6.Model.KREMO kremo = context.ExecuteStoreQuery<CIC.Database.OL.EF6.Model.KREMO>(BUDGETQUERY, parameters.ToArray()).FirstOrDefault();

                if (kremo != null)
                {
                    budget4DESimDto.budget1 = kremo.BUDGET1;
                    budget4DESimDto.saldo = kremo.SALDO;
                    budget4DESimDto.saldo2 = kremo.SALDO2;
                    budget4DESimDto.fehlercode = Convert.ToDecimal(kremo.FEHLERCODE);
                }

            }
            return budget4DESimDto;

        }

        /// <summary>
        /// Returns the needed DE Values for EL Calculation
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public DEValues4KR getDecisionValues(long sysid)
        {

            using (DdOlExtended context = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysid });
                return context.ExecuteStoreQuery<DEValues4KR>(DEENGINE_VALUE_QUERY, parameters.ToArray()).FirstOrDefault();
            }
        }

        /// <summary>
        /// returns all available laufzeiten
        /// </summary>
        /// <returns></returns>
        public List<int> getLaufzeiten()
        {

            using (DdOlExtended context = new DdOlExtended())
            {

                return context.ExecuteStoreQuery<int>(LZ_QUERY, null).ToList();
            }
        }
        /// <summary>
        /// Returns sysvg for zinsertrag calc
        /// </summary>
        /// <returns></returns>
        public long getZinsertragWertegruppe()
        {

            using (DdOlExtended context = new DdOlExtended())
            {

                return context.ExecuteStoreQuery<long>(ZINSERTRAG_QUERY, null).FirstOrDefault();
            }
        }
        /// <summary>
        /// Returns the kredit value for the proposal
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public ELCalcVars getAntragDaten(long sysid)
        {

            using (DdOlExtended context = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysid });
                return context.ExecuteStoreQuery<ELCalcVars>(KREDITBETRAG_QUERY, parameters.ToArray()).FirstOrDefault();
            }
        }

        /// <summary>
        /// liefert alle gespeicherten Kalkulationsvarianten für getCreditLimits die von der RuleEngine gespeichert wurden
        /// </summary>
        /// <param name="sysantrag"></param>
        /// <returns></returns>
        public List<ProductCreditLimitFetchDto> fetchCreditLimits(long sysantrag)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysantrag });
                return context.ExecuteStoreQuery<ProductCreditLimitFetchDto>(CREDITLIMIT_QUERY, parameters.ToArray()).ToList();
                
            }
        }

        /// <summary>
        /// Returns true when creditlimits are available
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public bool isFinanzierungsfähig(long sysid)
        {

            using (DdOlExtended context = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysid });
                String attribut = context.ExecuteStoreQuery<String>(FINFAEHIG_QUERY, parameters.ToArray()).FirstOrDefault();
                return "Finanzierungsvorschlag".Equals(attribut);
            }
        }


        /// <summary>
        /// Returns true if Antrag has a MA
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public bool hasMA(long sysid)
        {

            using (DdOlExtended context = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysid });
                int ma = context.ExecuteStoreQuery<int>(HAS_MA_QUERY, parameters.ToArray()).FirstOrDefault();
                if (ma > 0)
                    return true;
                return false;
            }
        }


        /// <summary>
        /// Calculates expected Loss
        /// </summary>
        /// <param name="flags"></param>
        /// <param name="lz"></param>
        /// <param name="pd"></param>
        /// <param name="kreditbetrag"></param>
        /// <param name="zinsertrag"></param>
        /// <returns></returns>
        public double? calculateEL(Flags4KR flags, long lz, double pd, double kreditbetrag,long sysantrag)
        {
            long lznew = lz;
            if (flags.ELLZLIMIT > 0 && lz > flags.ELLZLIMIT)
                lznew = (long)flags.ELLZLIMIT;

            using (DdOlExtended ctx = new DdOlExtended())
            {
                try
                {

                    if (flags.FORMEL_EL!=null && flags.FORMEL_EL != "")
                    {

                        List<Devart.Data.Oracle.OracleParameter> p = new List<Devart.Data.Oracle.OracleParameter>();
                        p.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pK", Value = flags.K });
                        p.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pLZ", Value = lznew });
                        p.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pA", Value = flags.a });
                        p.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pB", Value = flags.b });
                        p.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pC", Value = flags.c });
                        p.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pD", Value = flags.d });
                        p.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pLGD", Value = flags.LGD });
                        p.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pBETRAG", Value = kreditbetrag });
                        p.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pPD", Value = pd });
                        p.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysid", Value = sysantrag});

                        return ctx.ExecuteStoreQuery<double>(flags.FORMEL_EL, p.ToArray()).FirstOrDefault();
                    }
                    else
                    {
                        
                            return pd * ((flags.K * lznew) * flags.a + flags.b) * (lznew * flags.c + flags.d) * flags.LGD * kreditbetrag;
                    }


                }
                catch
                {
                    return null;
                }

            }
        }


        /// <summary>
        /// Calculates Profitability
        /// </summary>
        /// <param name="flags"></param>
        /// <param name="lz"></param>
        /// <param name="pd"></param>
        /// <param name="kreditbetrag"></param>
        /// <param name="zinsertrag"></param>
        /// <param name="el"></param>
        /// <returns></returns>
        public double? calculateELPROF(Flags4KR flags, long lz, double pd, double kreditbetrag, double zinsertrag, double el, long sysantrag)
        {

            using (DdOlExtended ctx = new DdOlExtended())
            {
                try
                {

                    if (flags.FORMEL_PROF!=null && flags.FORMEL_PROF != "")
                    {

                        List<Devart.Data.Oracle.OracleParameter> p = new List<Devart.Data.Oracle.OracleParameter>();
                        p.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pZINSERTRAG", Value = zinsertrag });
                        p.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pEL", Value = el });
                        p.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysid", Value = sysantrag });
                        p.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pUSEKREDIT", Value = kreditbetrag });
                        p.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pLZ", Value = lz });

                        return ctx.ExecuteStoreQuery<double>(flags.FORMEL_PROF, p.ToArray()).FirstOrDefault();
                    }
                    else
                    {
                        return (zinsertrag - el)/kreditbetrag * 100;
                    }


                }
                catch
                {
                    return null;
                }

            }
        }

        /// <summary>
        /// CreateUpdateLogDump
        /// </summary>
        /// <param name="soaptext">soaptext</param>
        /// <param name="entryCode">entryCode</param>
        /// <param name="entity">entity</param>
        /// <param name="id">id</param>
        public void CreateUpdateLogDump(String soaptext, String entryCode, String entity, long id)
        {
            if (id != 0 && !entryCode.Equals("") && !entity.Equals(""))
            {
                using (DdOwExtended context = new DdOwExtended())
                {
                    DateTime currentDate = DateTime.Now;
                    LOGDUMP logdump = new LOGDUMP();
                    context.LOGDUMP.Add(logdump);
                    string description = entity.ToUpper() + "_" + id + "_" + entryCode.ToUpper();
                   
                    logdump.DESCRIPTION = description;
                    logdump.DUMPVALUE = soaptext;
                    logdump.DUMPDATE = currentDate;
                    logdump.INPUTFLAG = 0;
                    logdump.DUMPTIME = DateTimeHelper.DateTimeToClarionTime(currentDate);
                    logdump.AREA = entity;
                    logdump.SYSID = id;

                    context.SaveChanges();
                }
            }
        }




    }
}

