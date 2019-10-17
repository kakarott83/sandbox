using AutoMapper;
using Cic.OpenOne.Common.Model.DdCt;
using Cic.OpenOne.Common.Model.DdIc;
using Cic.OpenOne.Common.Model.DdOd;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using CIC.Database.OD.EF6.Model;
using CIC.Database.OL.EF6.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    public class TransactionRisikoDao : ITransactionRisikoDao
    {

        const String VGQUERY = " SELECT * FROM VG " +
                                " WHERE SYSVG = " +
                                " (SELECT OBTYP.SYSVGLGD " +
                                " FROM OBTYP " +
                                " WHERE OBTYP.SYSVGLGD        > 0 " +
                                " START WITH OBTYP.SYSOBTYP = :p1 " +
                                " CONNECT BY PRIOR OBTYP.SYSOBTYPP = OBTYP.SYSOBTYP " +
                                " AND ROWNUM  = 1" +
                                ") ";


        const String QUERYSCOREBEZEICHNUNG = " SELECT SCOREBEZEICHNUNG " +
                                                " FROM risikokl rk, " +
                                                " deoutexec dx, " +
                                                " dedetail dd " +
                                                " WHERE dx.SYSDEOUTEXEC = dd.SYSDEOUTEXEC " +
                                                " AND dd.RISIKOKLASSEID =rk.sysrisikokl " +
                                                " AND dd.ANTRAGSTELLER = 1 " +
                                                " AND dx.sysauskunft    = " +
                                                " (SELECT auskunft.sysauskunft " +
                                                " FROM auskunft, " +
                                                " deenvinp, " +
                                                " deinpexec " +
                                                " WHERE auskunft.statusnum         = 0 " +
                                                " AND auskunft.area                  = 'ANTRAG' " +
                                                " AND auskunft.sysauskunfttyp        = 3 " +
                                                " AND deenvinp.flagbonitaetspruefung = 1 " +
                                                " AND auskunft.sysauskunft           =deinpexec.sysauskunft " +
                                                " AND deenvinp.sysdeinpexec          =deinpexec.sysdeinpexec " +
                                                " AND auskunft.sysid                 = :psysid " +
                                                " )";


        const String QUERYAUSFALLWVG = "select v.sysvg from vg v,vgtype t where v.sysvgtype=t.sysvgtype and t.NAME='PD'";

        private const string QUERYRISKFLAG = " Select count(wfmmemo.syswfmmemo) ANZAHL " +
                                                " FROM wfmmemo " +
                                                " WHERE syswfmmkat=22 " +
                                                " AND syswfmtable =117 " +
                                                " AND syslease    =:p1 " +
                                                " AND pdec902    = 1 " +
                                                " AND pdec901    IN (0,1,2) ";

        private const string QUERYFLAGPAKET1 = " select count(*) from (select vsart.sysvsart " +
                                               " from antvs, vstyp, vsart " +
                                               " where antvs.sysantrag=:p1 " +
                                               " and antvs.sysvstyp=vstyp.sysvstyp " +
                                               " and vsart.sysvsart=vstyp.sysvsart " +
                                               " and vsart.sysvsart in (1))";

        private const string QUERYFLAGPAKET2 = " select count(*) from (select vsart.sysvsart " +
                                               " from antvs, vstyp, vsart " +
                                               " where antvs.sysantrag=:p1 " +
                                               " and antvs.sysvstyp=vstyp.sysvstyp " +
                                               " and vsart.sysvsart=vstyp.sysvsart " +
                                               " and vsart.sysvsart in (2))";

        private const string QUERYRWEUROTAX = " SELECT count(sysvgrw) " +
                                              "   FROM " +
                                              "     ( " +
                                              "     select sysvgrw from obtyp left outer join vg on vg.sysvg=sysvgrw  " +
                                              "     where sysvgrw != 0 start with sysobtyp=:p1 " +
                                              "     connect by prior sysobtypp=sysobtyp " +
                                              "     ) ";


        private const string QUERYRWBANKNOW = " SELECT count(sysvgrw) " +
                                                " FROM " +
                                                " ( " +
                                                " select sysvgrw from obtyp left outer join vg on vg.sysvg=sysvgrw " +
                                                " where sysvgrw != 0 start with sysobtyp=:p1 " +
                                                " connect by prior sysobtypp=sysobtyp " +
                                                " ) ";

        private const string QUERYMWCLUSTER = "  SELECT upper(NAME) " +
                                              "   FROM VG " +
                                              "   WHERE SYSVG =  " +
                                              "     (SELECT OBTYP.SYSVGLGD  " +
                                              "     FROM OBTYP  " +
                                              "     WHERE OBTYP.SYSVGLGD        > 0  " +
                                              "       START WITH OBTYP.SYSOBTYP =  " +
                                              "       (SELECT SYSOBTYP FROM ANTOB WHERE SYSANTRAG=:p1 " +
                                              "       ) " +
                                              "       CONNECT BY PRIOR OBTYP.SYSOBTYPP = OBTYP.SYSOBTYP " +
                                              "     AND ROWNUM                         = 1 " +
                                              "     ) ";
        #region SIMULATION

        private const string INPUTDATENDEQUERY = @"SELECT auskunft.sysauskunft  
	                                                                    FROM auskunft,  
	                                                                    deenvinp,  
	                                                                    deinpexec  
	                                                                    WHERE  
	                                                                    auskunft.area                  = 'ANTRAG'  
	                                                                    AND auskunft.sysauskunfttyp        = 3  
	                                                                    AND deenvinp.flagbonitaetspruefung = 1  
	                                                                    AND auskunft.sysid = :psysid   
	                                                                    AND auskunft.statusnum = 0
	                                                                    AND auskunft.sysauskunft           =deinpexec.sysauskunft  
	                                                                    AND deenvinp.sysdeinpexec          =deinpexec.sysdeinpexec 
	                                                                    order by auskunft.sysauskunft desc";

        private const string KUNDEIDBYKUNDE = "select code from kd where sysperson = :psysKunde";

        private const string DEDEFRUL = "select LISTAGG(externcode,',') within Group (order by externcode) as externcode from DEDEFRUL where sysdedefrul in (select sysdedefrul from derul where sysdedetail in (select sysdedetail from dedetail where sysdeoutexec in (select sysdeoutexec from deoutexec where sysauskunft = :psysauskunft)))";

        //da muss der neueste Datensatz aus Kremo geholt werden
        private const string BUDGETQUERY = "select * from Kremo where sysantrag=:sysantrag order by syskremo desc ";




        public string getKundenID(long sysKunde)
        {
            string kundenid = "";
            using (DdCtExtended context = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysKunde", Value = sysKunde });
                kundenid = context.ExecuteStoreQuery<string>(KUNDEIDBYKUNDE, parameters.ToArray()).FirstOrDefault();
            }
            return kundenid;
        }

        /// <summary>
        /// Input Daten für Decision Engine / Alle nicht für die Risikoprüfung relevanten Daten werden aus der Auskunft zur letzten Bonitätsprüfung geholt. 
        /// </summary>
        /// <param name="kundenid"></param>
        /// <returns></returns>
        public DecisionEngineInDto getInputDatenDE(long sysantrag)
        {
            IDecisionEngineDBDao dedao = new DecisionEngineDBDao();
            DecisionEngineInDto deInDto = new DecisionEngineInDto();
            long sysauskunft = 0;

            using (DdCtExtended context = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysid", Value = sysantrag });
                sysauskunft = context.ExecuteStoreQuery<long>(INPUTDATENDEQUERY, parameters.ToArray()).FirstOrDefault();


                if (sysauskunft != 0)
                {
                    deInDto = dedao.FindBySysId(sysauskunft);


                }

            }
            return deInDto;
        }



        /// <summary>
        /// RISKFLAG /  risikoprüfungsrelevanten Daten für DecisionEngine I_C_Riskflag
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public int getRiskFlag(long sysid)
        {
            int riskflag;
            using (DdCtExtended context = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = sysid });
                riskflag = context.ExecuteStoreQuery<int>(QUERYRISKFLAG, parameters.ToArray()).FirstOrDefault();
                if (riskflag > 0) riskflag = 1;

            }
            return riskflag;
        }

        /// <summary>
        ///  Flag_Paket 1 / risikoprüfungsrelevanten Daten für DecisionEngine I_C_PPI_Flag_Paket1
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public int getPPI_Flag_Paket1(long sysid)
        {
            int flagPaket1;
            using (DdCtExtended context = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = sysid });
                flagPaket1 = context.ExecuteStoreQuery<int>(QUERYFLAGPAKET1, parameters.ToArray()).FirstOrDefault();

            }
            return flagPaket1;
        }

        /// <summary>
        ///  Flag_Paket 2 / risikoprüfungsrelevanten Daten für DecisionEngine I_C_PPI_Flag_Paket2
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public double? getPPI_Flag_Paket2(long sysid)
        {
            double? flagPaket2;
            using (DdCtExtended context = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = sysid });
                flagPaket2 = context.ExecuteStoreQuery<double?>(QUERYFLAGPAKET2, parameters.ToArray()).FirstOrDefault();

            }
            return flagPaket2;
        }

        /// <summary>
        /// Budget Daten aus KREMO für Simulation / risikoprüfungsrelevanten Daten für DecisionEngine 
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public Budget4DESimDto getBudget4DESim(long sysid)
        {
            Budget4DESimDto budget4DESimDto = new Budget4DESimDto();

            using (DdCtExtended context = new DdCtExtended())
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
        /// 
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <returns></returns>
        public bool getRwbruttoFlag(long sysobtyp)
        {

            using (DdCtExtended context = new DdCtExtended())
            {
                int rweurotoxflag;
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = sysobtyp });
                rweurotoxflag = context.ExecuteStoreQuery<int>(QUERYRWEUROTAX, parameters.ToArray()).FirstOrDefault();
                if (rweurotoxflag == 0) return true;
            }


            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <returns></returns>
        public bool getRwbruttoBankNowFlag(long sysobtyp)
        {

            using (DdCtExtended context = new DdCtExtended())
            {
                int rweurotoxflag;
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = sysobtyp });
                rweurotoxflag = context.ExecuteStoreQuery<int>(QUERYRWBANKNOW, parameters.ToArray()).FirstOrDefault();
                if (rweurotoxflag == 1) return true;
            }


            return false;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public string getMarktwert_Cluster(long sysid)
        {
            string marktwert_cluster;
            using (DdCtExtended context = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = sysid });
                marktwert_cluster = context.ExecuteStoreQuery<string>(QUERYMWCLUSTER, parameters.ToArray()).FirstOrDefault();

            }

            return marktwert_cluster;
        }


        public int getZustandFromObject(long sysid)
        {

            using (DdOlExtended context = new DdOlExtended())
            {
                int zustand = (int)(from obart in context.OBART where obart.SYSOBART == sysid select obart.TYP).FirstOrDefault();
                return zustand;
            }
            
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public int getGetroffenenRegelnAnzahl(long sysAuskunft)
        {

            IDecisionEngineDBDao dedao = new DecisionEngineDBDao();
            int anzahl = dedao.getGetroffenenRegelnAnzahl(sysAuskunft);

            return anzahl;


        }

        public void updateKalkMitVariantenDaten(AngAntKalkDto kalk)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                ANTKALK antkalk = (from ak in context.ANTKALK where ak.ANTRAG.SYSID == kalk.syskalk select ak).FirstOrDefault();

                if (antkalk != null)
                {
                    antkalk.RATEBRUTTO = (decimal?)kalk.rateBrutto;
                    antkalk.LZ = kalk.lz;
                    antkalk.RWBRUTTO = (decimal?)kalk.rwBrutto;
                    antkalk.SZBRUTTO = (decimal?)kalk.szBrutto;
                }

                context.SaveChanges();


            }

            //deleteVarianteUndDERulsByAntrag(kalk.syskalk);


        }



        /// <summary>
        /// Die getroffenen Regeln aus der DecisionEngine Simulation werden zu der jeweiligen Variante in der Tabelle DDLKPSPOS abgelegt
        /// </summary>
        /// <param name="syskalk"></param>
        /// <param name="sysAuskunft"></param>
        public void saveVariantenDERuls(long sysantkalkvar, string[] simulationDERules)
        {
            if (simulationDERules != null)
            {
                List<string> regeln = simulationDERules.ToList();
                //Insert in DDLKPSPOS  area = ANTKALK    sysid = SYSKALK   value = RPOO1
                using (Cic.OpenOne.Common.Model.DdOd.DdOdExtended context = new Cic.OpenOne.Common.Model.DdOd.DdOdExtended())
                {


                    foreach (string s in regeln)
                    {
                        DDLKPSPOS ddlkpspos = new DDLKPSPOS();
                        ddlkpspos.AREA = "ANTKALKVAR";
                        ddlkpspos.SYSID = sysantkalkvar;
                        ddlkpspos.ACTIVEFLAG = 1;
                        ddlkpspos.VALUE = s;
                        context.DDLKPSPOS.Add(ddlkpspos);

                    }
                    context.SaveChanges();
                }
            }

        }

        /// <summary>
        /// Die getroffenen Regeln aus der DecisionEngine Simulation werden zu der jeweiligen Variante in der Tabelle DDLKPSPOS abgelegt
        /// </summary>
        /// <param name="syskalk"></param>
        /// <param name="sysAuskunft"></param>
        public void saveVariantenDERuls(long sysantkalkvar, long sysAuskunft)
        {
            IDecisionEngineDBDao dedao = new DecisionEngineDBDao();
            List<string> regeln = dedao.getGetroffenenRegelnCode(sysAuskunft);
            //Insert in DDLKPSPOS  area = ANTKALK    sysid = SYSKALK   value = RPOO1
            using (Cic.OpenOne.Common.Model.DdOd.DdOdExtended context = new Cic.OpenOne.Common.Model.DdOd.DdOdExtended())
            {


                foreach (string s in regeln)
                {
                    DDLKPSPOS ddlkpspos = new DDLKPSPOS();
                    ddlkpspos.AREA = "ANTKALKVAR";
                    ddlkpspos.SYSID = sysantkalkvar;
                    ddlkpspos.ACTIVEFLAG = 1;
                    ddlkpspos.VALUE = s;
                    context.DDLKPSPOS.Add(ddlkpspos);

                }
                context.SaveChanges();
            }

        }

        /// <summary>
        /// Sofern eine erneute Kalkulation durchgeführt wird muss die bestehende Verknüpfung aus ANTVAR, ANTKALK und DDLKSPOS entfernt werden und erneut angelegt werden.
        /// </summary>
        /// <param name="syskalk"></param>
        public void deleteVarianteDERuls(long syskalk)
        {
            List<DDLKPSPOS> ddlksposList = new List<DDLKPSPOS>();
            using (Cic.OpenOne.Common.Model.DdOd.DdOdExtended context = new Cic.OpenOne.Common.Model.DdOd.DdOdExtended())
            {
                ddlksposList = context.DDLKPSPOS.Where((p) => p.AREA == "ANTKALKVAR" && p.SYSID == syskalk).ToList();
                foreach (DDLKPSPOS ddlkpspos in ddlksposList)
                {
                    context.DDLKPSPOS.Attach(ddlkpspos);
                    context.DeleteObject(ddlkpspos);
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Sofern eine erneute Kalkulation durchgeführt wird muss die bestehende Verknüpfung aus ANTVAR, ANTKALK und DDLKSPOS entfernt werden und erneut angelegt werden.
        /// </summary>
        /// <param name="sysid"></param>
        public void deleteVarianteUndDERulsByAntrag(long sysid)
        {
            List<ANTKALKVAR> antkalkvarList = new List<ANTKALKVAR>();
            List<DDLKPSPOS> ddlksposList = new List<DDLKPSPOS>();

            using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended ctx = new Cic.OpenOne.Common.Model.DdOl.DdOlExtended())
            {
                antkalkvarList = ctx.ANTKALKVAR.Where((p) => p.SYSKALK == sysid).ToList();

            }


            using (Cic.OpenOne.Common.Model.DdOd.DdOdExtended context = new Cic.OpenOne.Common.Model.DdOd.DdOdExtended())
            {
                foreach (ANTKALKVAR antkalkvar in antkalkvarList)
                {
                    ddlksposList = context.DDLKPSPOS.Where((p) => p.AREA == "ANTKALKVAR" && p.SYSID == antkalkvar.SYSANTKALKVAR).ToList();
                    foreach (DDLKPSPOS ddlkpspos in ddlksposList)
                    {
                        context.DDLKPSPOS.Attach(ddlkpspos);
                        context.DeleteObject(ddlkpspos);
                        context.SaveChanges();
                    }


                }

            }

            using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended ctx = new Cic.OpenOne.Common.Model.DdOl.DdOlExtended())
            {
                foreach (ANTKALKVAR antkalkvar in antkalkvarList)
                {
                    ctx.ANTKALKVAR.Attach(antkalkvar);
                    ctx.DeleteObject(antkalkvar);
                    ctx.SaveChanges();
                }


            }
        }


        /// <summary>
        /// Die DDLKSPOS dient auch als Kriterium für die Anzeige im B2B, sofern ein Finanzierungsvorschlag vorhanden ist und keine DDLKPSPOS Satz dazu existiert, wird die Variante im B2B angezeigt. Andernfalls wird die Variante nicht angezeigt.
        /// </summary>
        /// <param name="syskalk"></param>
        /// <returns></returns>
        public bool gibtDERuls4Variante(long syskalk)
        {

            using (Cic.OpenOne.Common.Model.DdOd.DdOdExtended context = new Cic.OpenOne.Common.Model.DdOd.DdOdExtended())
            {
                List<DDLKPSPOS> ddlksposList = context.DDLKPSPOS.Where((p) => p.AREA == "ANTKALKVAR" && p.SYSID == syskalk).ToList();
                if (ddlksposList != null && ddlksposList.Count > 0) return true;
            }
            return false;
        }



        #endregion SIMULATION




        #region TRANSACTIONRISK

        /// <summary>
        ///  Ermitteln der Wertegruppe des Objekts
        /// </summary>
        /// <param name="sysObTyp"></param>
        /// <returns></returns>
        public VG getSysVGByObTyp(long sysObTyp)
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {
                DbParameter[] pars = { new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = sysObTyp }, };

                //Execute Query
                return ctx.ExecuteStoreQuery<VG>(VGQUERY, pars).FirstOrDefault();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public string getScoreBezeichnungByAntragId(long sysid)
        {
            string scorebezeichnung = "";
            using (DdCtExtended context = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysid", Value = sysid });
                scorebezeichnung = context.ExecuteStoreQuery<String>(QUERYSCOREBEZEICHNUNG, parameters.ToArray()).FirstOrDefault();
            }
            return scorebezeichnung;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public double getAusfallwvgByAntragId(long sysid)
        {
            double ausfallwvg = 0;
            using (DdCtExtended context = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysid", Value = sysid });
                ausfallwvg = context.ExecuteStoreQuery<double>(QUERYAUSFALLWVG, null).FirstOrDefault();
            }
            return ausfallwvg;
        }




        /// <summary>
        /// Daten aus dem Antrag für ELInDto / TODO : nicht in einsatz 
        /// </summary>
        /// <param name="antrag"></param>
        /// <returns></returns>
        public ELInDto getElInDtoByAntrag(AntragDto antrag)
        {
            ELInDto eLInDto = new ELInDto();
            eLInDto.sysid = antrag.sysid;
            eLInDto.barkaufpreis = antrag.kalkulation.angAntKalkDto.bginternbrutto;
            eLInDto.anzahlung = antrag.kalkulation.angAntKalkDto.sz;
            eLInDto.anz_bkp = antrag.sysid;
            eLInDto.finanzierungsbetrag = antrag.kalkulation.angAntKalkDto.bginternbrutto - antrag.kalkulation.angAntKalkDto.szBrutto;
            eLInDto.laufzeit = antrag.kalkulation.angAntKalkDto.lz;
            //eLInDto.beginn = antrag.kalkulation.angAntKalkDto.;
            eLInDto.syskalk = antrag.kalkulation.angAntKalkDto.syskalk;
            eLInDto.sysantrag = antrag.sysid;
            eLInDto.antrag = antrag.antrag;
            eLInDto.restwert = antrag.kalkulation.angAntKalkDto.rwBrutto;
            eLInDto.zins = antrag.kalkulation.angAntKalkDto.zins;
            eLInDto.scorebezeichnung = getScoreBezeichnungByAntragId(antrag.sysid);
            eLInDto.sysobtyp = antrag.angAntObDto.sysobtyp;
            eLInDto.kmStand = antrag.angAntObDto.ubnahmeKm;
            eLInDto.jahresKm = antrag.angAntObDto.jahresKm;
            eLInDto.erstzulassung = (DateTime)antrag.angAntObDto.erstzulassung;
            //eLInDto.zubehoer = antrag.angAntObDto.;
            //eLInDto.minLz = antrag.sysid;
            eLInDto.mwst = antrag.sysid;
            VG vg = getSysVGByObTyp(eLInDto.sysobtyp);
            if (vg != null)
            {
                eLInDto.sysvg = vg.SYSVG;
                eLInDto.vgName = vg.NAME;
            }

            IAngAntDao angAntDao = CommonDaoFactory.getInstance().getAngAntDao();
            AngAntObDto obData = angAntDao.getObjektdaten(eLInDto.sysobtyp);
            if (eLInDto.erstzulassung == null)
                eLInDto.erstzulassung = (obData.erstzulassung) != null ? (DateTime)obData.erstzulassung : DateTime.Now;
            eLInDto.zubehoer = obData.zubehoer;
            eLInDto.schwacke = obData.schwacke;
            //eLInDto.alter_Fhz_Mt= antrag.sysid;   




            //eLInDto.rate = antrag.kalkulation.angAntKalkDto.rateBrutto;
            //eLInDto.syskdtyp = antrag
            //eLInDto.sysbrand 
            //eLInDto.scorewert 
            //eLInDto.ausfallwvg  = getAusfallwvgByAntragId(antrag.sysid);
            //eLInDto. zinsertrag 
            //eLInDto.sysob 

            return eLInDto;
        }

        /// <summary>
        /// Variante save / TODO DB EDMX ANTVAR
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="angAntKalkDto"></param>
        /// <param name="rang"></param>
        public void saveVariante(long sysid, AngAntKalkDto angAntKalkDto, int? rang, string[] simulationDeRules)
        {
            long sysantkalkvar = 0;
            using (DdOlExtended context = new DdOlExtended())
            {
                ANTKALKVAR antkalkvar = context.ANTKALKVAR.Where((p) => p.SYSKALK == sysid && p.RANG == rang).FirstOrDefault();
                if (antkalkvar == null)
                {
                    antkalkvar = new ANTKALKVAR();
                    context.ANTKALKVAR.Add(antkalkvar);
                    antkalkvar.SYSKALK = sysid;

                }



                Mapper.Map<AngAntKalkDto, ANTKALKVAR>(angAntKalkDto, antkalkvar);
                antkalkvar.RANG = rang;
                antkalkvar.SYSKALK = sysid;
                context.SaveChanges();
                sysantkalkvar = antkalkvar.SYSANTKALKVAR;

            }

            if (sysantkalkvar != 0)
            {
                deleteVarianteDERuls(sysantkalkvar);
                saveVariantenDERuls(sysantkalkvar, simulationDeRules);
            }
        }




        /// <summary>
        /// Liefert die angegebene Variante (ANTKALKVAR mit Rang) für die jeweilige Finanzierungsvorschlagvariante
        /// R11 CR 149
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="rang"></param>
        /// <param name="mitRisikoFilter"></param>
        /// <returns></returns>
        public AngAntKalkDto getVariante(long sysid, long rang, bool mitRisikoFilter)
        {
            AngAntKalkDto variante = new AngAntKalkDto();
            using (DdOlExtended context = new DdOlExtended())
            {
                ANTKALKVAR antkalkvar = (from kalk in context.ANTKALKVAR
                                         where kalk.SYSKALK == sysid && kalk.RANG == rang
                                         select kalk).FirstOrDefault();
                if (antkalkvar != null)
                {
                    if (gibtDERuls4Variante(antkalkvar.SYSANTKALKVAR) && mitRisikoFilter) return null;
                    variante = Mapper.Map<ANTKALKVAR, AngAntKalkDto>(antkalkvar);
                    return variante;
                }
                return null;
            }


        }


        public void deleteStaffel(long sysid)
        {
            using (DdOlExtended context = new DdOlExtended())
            {

                const string ANTOBSLQUERY = " SELECT * from CIC.ANTOBSL where sysvt = :sysvt ";
                const string ANTOBSLPOSQUERY = "SELECT * from CIC.ANTOBSLPOS where sysvtobsl = :psysantobsl ";

                List<Devart.Data.Oracle.OracleParameter> p = new List<Devart.Data.Oracle.OracleParameter>();
                p.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvt", Value = sysid });
                List<ANTOBSL> antobslliste = context.ExecuteStoreQuery<ANTOBSL>(ANTOBSLQUERY, p.ToArray()).ToList();
                p.Clear();
                //delete old entities


                foreach (ANTOBSL antobsl in antobslliste)
                {
                    p.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysantobsl", Value = antobsl.SYSID });
                    List<ANTOBSLPOS> ANTOBSLPOSList = context.ExecuteStoreQuery<ANTOBSLPOS>(ANTOBSLPOSQUERY, p.ToArray()).ToList();
                    p.Clear();

                    foreach (ANTOBSLPOS toDel in ANTOBSLPOSList)
                    {
                        context.ANTOBSLPOS.Attach(toDel);
                        context.DeleteObject(toDel);
                    }

                    context.ANTOBSL.Attach(antobsl);
                    context.DeleteObject(antobsl);
                    context.SaveChanges();
                }

            }
        }



        public void saveStaffel(List<InsertParamDto> Liste, InsertParamDto obslparameter, long sysid)
        {


            const string INSERTSTAFFEL = " INSERT INTO CIC.ANTOBSLPOS " +
                                       " (rang,anzahl,sysvtobsl, valuta, zinssatz,  " +
                                       "  betrag, betrag1, betrag2, betrag3, betrag4, betrag5, betrag6, betrag7, betrag8,betrag9,  betrag10, " +
                                       "  zins, flagSR1, flagSR2, flagSR3, sysrnpos, sysrn, faktor, betragINT,betragKORR , RAPZINSSUMME, TILGUNGVIRTUAL, FlagSR4, ZINSENVIRTUAL, BETRAGKORRP,ZINS10 )" +
                                       " values" +
                                       " (:rang,:anzahl,:sysvtobsl, :valuta, :zinssatz,  " +
                                       "  :betrag, :betrag1, :betrag2, :betrag3, :betrag4, :betrag5, :betrag6, :betrag7, :betrag8,:betrag9,  :betrag10, " +
                                       "  :zins, :flagSR1, :flagSR2, :flagSR3, :sysrnpos, :sysrn, :faktor, :betragINT, :betragKORR, :RAPZINSSUMME, :TILGUNGVIRTUAL, :FlagSR4, :ZINSENVIRTUAL, :BETRAGKORRP, :ZINS10 )";

            const string INSERTOBSL = " INSERT INTO CIC.ANTOBSL " +
                                      " ( SysVt, SysSLTYP, Bezeichnung, Faellig, LZ, PPY, ZINS, RANG, SYSCREATE )" +
                                      " values" +
                                      "( :SysVt, :SysSLTYP, :Bezeichnung, :Faellig, :LZ, :PPY, :ZINS, :RANG,SYSDATE )";

            const string ANTOBSLQUERY = " SELECT sysid from CIC.ANTOBSL where sysvt = :sysvt order by sysid desc ";


            using (DdCtExtended context = new DdCtExtended())
            {



                context.ExecuteStoreCommand(INSERTOBSL, obslparameter.parameters.ToArray());


                List<Devart.Data.Oracle.OracleParameter> p = new List<Devart.Data.Oracle.OracleParameter>();
                p.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvt", Value = sysid });
                long sysantobsl = context.ExecuteStoreQuery<long>(ANTOBSLQUERY, p.ToArray()).FirstOrDefault();
                p.Clear();

                foreach (InsertParamDto insertparam in Liste)
                {
                    insertparam.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvtobsl", Value = sysantobsl });
                    context.ExecuteStoreCommand(INSERTSTAFFEL, insertparam.parameters.ToArray());
                    insertparam.parameters.Clear();
                }


                obslparameter.parameters.Clear();
            }


        }

        public double? evalRestwert(string scoreTR_RWFormel, long sysID, int ALZ)
        {
            double? rval = null;
            //TR_RW_xxx --> entspricht der Scorekarte / Variable :pALZ1, :pALZ2, :pANZ, :pANTRAGID
            using (DdOlExtended ctx = new DdOlExtended())
            {
                try
                {
                    if (scoreTR_RWFormel != "")
                    {

                        List<Devart.Data.Oracle.OracleParameter> p = new List<Devart.Data.Oracle.OracleParameter>();
                        p.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pANTRAGID", Value = sysID });
                        p.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pALZ", Value = ALZ });

                        rval = ctx.ExecuteStoreQuery<double?>(scoreTR_RWFormel, p.ToArray()).FirstOrDefault();
                    }



                }
                catch
                {
                    return null;
                }


            }

            return rval;

        }


        public double? evalTR4Scorekarte(string scoreTRFormel, double betrag, double sz, double trbetrag, double pint, double pslot, long sysID)
        {
            double? rval = null;
            //TR_RW_xxx --> entspricht der Scorekarte / Variable :pALZ1, :pALZ2, :pANZ, :pANTRAGID
            using (DdOlExtended ctx = new DdOlExtended())
            {
                try
                {

                    if (scoreTRFormel != "")
                    {

                        List<Devart.Data.Oracle.OracleParameter> p = new List<Devart.Data.Oracle.OracleParameter>();
                        p.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pBETRAG", Value = betrag });
                        p.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pSZ", Value = sz });
                        p.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pTRBETRAG", Value = trbetrag });
                        p.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pINT", Value = pint });
                        p.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pSLOP", Value = pslot });
                        rval = ctx.ExecuteStoreQuery<double>(scoreTRFormel, p.ToArray()).FirstOrDefault();
                    }



                }
                catch
                {
                    return null;
                }

            }
            return rval;

        }


        public int? evalEL_KALKFLAG(long sysid, string query)
        {
            int? rval = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                try
                {

                    if (query != "")
                    {

                        List<Devart.Data.Oracle.OracleParameter> p = new List<Devart.Data.Oracle.OracleParameter>();
                        p.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pANTRAGID", Value = sysid });

                        rval = ctx.ExecuteStoreQuery<int>(query, p.ToArray()).FirstOrDefault();
                    }



                }
                catch
                {
                    return null;
                }
            }

            return rval;

        }


        public void saveTROutput(long sysid, string output)
        {

            using (Cic.OpenOne.Common.Model.DdOd.DdOdExtended context = new Cic.OpenOne.Common.Model.DdOd.DdOdExtended())
            {

                List<DDLKPSPOS> ddlksposList = context.DDLKPSPOS.Where((p) => p.SYSID == sysid && p.AREA == "ANTKALK").ToList();
                foreach (DDLKPSPOS ddlkpsposItem in ddlksposList)
                {
                    context.DDLKPSPOS.Attach(ddlkpsposItem);
                    context.DeleteObject(ddlkpsposItem);
                }
                context.SaveChanges();

                DDLKPSPOS ddlkpspos = new DDLKPSPOS();
                ddlkpspos.AREA = "ANTKALK";
                ddlkpspos.SYSID = sysid;
                ddlkpspos.CONTENT = output;
                ddlkpspos.ACTIVEFLAG = 1;
                context.DDLKPSPOS.Add(ddlkpspos);
                context.SaveChanges();





            }
        }




        public String getCodeAusVart(long sysvart)
        {
            String rval = "";
            using (DdCtExtended context = new DdCtExtended())
            {

                String QUERY = "select code from vart where sysvart=:psysvart";
                List<Devart.Data.Oracle.OracleParameter> p = new List<Devart.Data.Oracle.OracleParameter>();
                p.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysvart", Value = sysvart });
                rval = context.ExecuteStoreQuery<String>(QUERY, p.ToArray()).FirstOrDefault();

            }

            return rval;
        }



        #endregion TransactionRisk
    }
}

