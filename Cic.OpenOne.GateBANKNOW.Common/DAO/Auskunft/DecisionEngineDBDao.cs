using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.Common.Model.DdDe;
using Cic.OpenOne.Common.Model.DdOl;
using System.Xml.Linq;
using CIC.Database.OL.EF6.Model;
using CIC.Database.DE.EF6.Model;
using CIC.Database.OW.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{

    public class ExtraSalesFelder
    {
        public decimal SYSVARTABL { get; set; }
        public decimal VTDAUERABLTOTAL { get; set; }
        public decimal ANZABLVTPERVART { get; set; }
        public decimal VTDAUERABLPERVART { get; set; }
        public decimal VTDAUERABL { get; set; }



    }

    public class ExtraDOLFelder
    {
        public DateTime? DATUMERSTERANTRAG { get; set; }
        public DateTime? DATUMLETZTERANTRAG { get; set; }
        public decimal ANZMAHN1AVG6M { get; set; }
        public decimal ANZMAHN2AVG6M { get; set; }
        public decimal ANZMAHN3AVG6M { get; set; }
        public decimal ANZZAHLAVG12M { get; set; }
        public decimal RUECKSTANDAVG { get; set; }
        public decimal BUCHSALDOAVG { get; set; }

    }

    public class ExtraZEKFelder
    {
        public decimal? ANZZEKMELDPOS { get; set; }
        public decimal? ANZZEKMELDNEG { get; set; }
    }

    /// <summary>
    /// Decision Engine DB Data Access Object
    /// </summary>
    public class DecisionEngineDBDao : IDecisionEngineDBDao
    {
        ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const string Auflagen = "Auflagen";
        private const string BP = "BP";
        private const string VP = "VP";
        private const string FP = "FP";
        private const string RP = "RP";
        private const string Formalitaeten = "Formalitaeten";

        private const string RATINGQUERY = "select rating.sysrating from rating where area = :parea and sysid=:psysid and flag1=0";
        private const string RATINGSIMULQUERY = "select * from ratingsimul where sysrating = :psysrating";

        private const string DERULANZAHLQUERY = "select count(*) from derul where sysdedetail in (select sysdedetail from dedetail where sysdeoutexec in (select sysdeoutexec from deoutexec where sysauskunft = :psysauskunft))";
        private const string DERULQUERY = "select * from derul where sysdedetail in (select sysdedetail from dedetail where sysdeoutexec in (select sysdeoutexec from deoutexec where sysauskunft = :psysauskunft))";

        private const string DERULEXTERNCODEQUERY = "select externcode from dedefrul where sysdedefrul in (select derul.sysdedefrul from derul  where sysdedetail in (select sysdedetail from dedetail where sysdeoutexec in (select sysdeoutexec from deoutexec where sysauskunft = :psysauskunft)))";




        /// <summary>
        /// Map DecisionEngineInDto to entities and save to database
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto"></param>
        public void SaveDecisionEngineInput(long sysAuskunft, DecisionEngineInDto inDto)
        {
            using (DdDeExtended context = new DdDeExtended())
            {
                DEINPEXEC deInputExec = new DEINPEXEC();
                deInputExec.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                context.DEINPEXEC.Add(deInputExec);

                DEENVINP deEnvInp = Mapper.Map<DecisionEngineInDto, DEENVINP>(inDto);
                deEnvInp.DEINPEXEC = deInputExec;
                context.DEENVINP.Add(deEnvInp);

                DECONTRACT deContract = Mapper.Map<DecisionEngineInDto, DECONTRACT>(inDto);
                deContract.DEINPEXEC = deInputExec;
                context.DECONTRACT.Add(deContract);

                DEOBJECT deObject = Mapper.Map<DecisionEngineInDto, DEOBJECT>(inDto);
                deObject.DEINPEXEC = deInputExec;
                context.DEOBJECT.Add(deObject);

                DEPARTNER dePartner = Mapper.Map<DecisionEngineInDto, DEPARTNER>(inDto);
                dePartner.DEINPEXEC = deInputExec;
                context.DEPARTNER.Add(dePartner);

                DESALES deSales = Mapper.Map<DecisionEngineInDto, DESALES>(inDto);
                deSales.DEINPEXEC = deInputExec;
                context.DESALES.Add(deSales);

                foreach (RecordRRDto RRDto in inDto.RecordRRDto)
                {
                    DEOPENLEASE deOpenLease = Mapper.Map<RecordRRDto, DEOPENLEASE>(RRDto);
                    deOpenLease.DEINPEXEC = deInputExec;
                    context.DEOPENLEASE.Add(deOpenLease);

                    DEZEK deZek = Mapper.Map<RecordRRDto, DEZEK>(RRDto);
                    deZek.DEINPEXEC = deInputExec;
                    context.DEZEK.Add(deZek);

                    //BNR14 EDMX / TODO 
                    //DEARBEIT deArbeit = Mapper.Map<RecordRRDto, DEARBEIT>(RRDto);
                    //deArbeit.DEINPEXEC = deInputExec;
                    //context.DEARBEIT.Add(deArbeit);

                    DEADRBONI deAdrBoni = Mapper.Map<RecordRRDto, DEADRBONI>(RRDto);
                    deAdrBoni.DEINPEXEC = deInputExec;
                    context.DEADRBONI.Add(deAdrBoni);

                    DEAPPLICANT deApp = Mapper.Map<RecordRRDto, DEAPPLICANT>(RRDto);
                    deApp.DEINPEXEC = deInputExec;
                    context.DEAPPLICANT.Add(deApp);
                }
                context.SaveChanges();

                //BNR4 WORKAROUND-EDMX
                savedeArbeitInput(inDto.RecordRRDto, deInputExec);

            }

        }

        /// <summary>
        /// Map DecisionEngineOutDto to entities and save to database
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto"></param>
        public void SaveDecisionEngineOutput(long sysAuskunft, DecisionEngineOutDto outDto)
        {
            using (DdDeExtended context = new DdDeExtended())
            {
                DEOUTEXEC deOutExec;
                DEENVOUT deEnvOut;
                DEDECISION deDecision;
                DESONST deSonst;
                DEDETAIL deDetail;
                String[] auflagen;
                String[] bp;
                String[] vp;
                String[] fp;
                String[] rp;
                String[] formalitaeten;

                // check if DEOUTEXEC already exists
                AUSKUNFT Auskunft = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                if (Auskunft != null && !context.Entry(Auskunft).Collection(f => f.DEOUTEXECList).IsLoaded)
                        context.Entry(Auskunft).Collection(f => f.DEOUTEXECList).Load();
                
                // Update if DEOUTEXEC already exists / Create new objects if not
                if (Auskunft.DEOUTEXECList.Count() > 0)
                {
                    // OUTPUTSÄTZE WURDEN BEREITS ANGELEGT, ES MUSS NICHTS MEHR ANGELEGT / UPGEDATET WERDEN!!!
                }
                else
                {
                    // NEW
                    deOutExec = new DEOUTEXEC { AUSKUNFT = Auskunft };
                    context.DEOUTEXEC.Add(deOutExec);

                    deEnvOut = new DEENVOUT { DEOUTEXEC = deOutExec };

                    deDecision = new DEDECISION { DEOUTEXEC = deOutExec };

                    deSonst = new DESONST { DEOUTEXEC = deOutExec };

                    // Fill entities with values from outDto
                    MyUpdateDecisionEngineOutObjects(deEnvOut, deDecision, deSonst, outDto);

                    // Add objects to context
                    context.DEENVOUT.Add(deEnvOut);
                    context.DEDECISION.Add(deDecision);
                    context.DESONST.Add(deSonst);
                    if (outDto.RecordRRResponseDto != null)
                    {
                        for (int i = 0; i < outDto.RecordRRResponseDto.Length; i++)
                        {
                            deDetail = new DEDETAIL
                            {
                                DEOUTEXEC = deOutExec,
                                ANTRAGSTELLER = (int?)outDto.RecordRRResponseDto[i].DET_Antragsteller,
                                SCORECODE = outDto.RecordRRResponseDto[i].DEC_Scorekarte_Code,
                                SCOREBEZEICHNUNG = outDto.RecordRRResponseDto[i].DEC_Scorekarte_Bezeichnung,
                                SCORETOTAL = (int?)outDto.RecordRRResponseDto[i].DEC_Scorewert_Total,
                                RISIKOKLASSEID = (int?)outDto.RecordRRResponseDto[i].DEC_Risikoklasse_ID,
                                RISIKOKLASSEBEZEICHNUNG = outDto.RecordRRResponseDto[i].DEC_Risikoklasse_Bezeichnung,
                                PD = outDto.RecordRRResponseDto[i].DEC_Score_PD
                            };

                            if (i == 0)
                            {
                                deDetail.FAKTORZ = outDto.DEC_FaktorZ;
                                deDetail.FREIBETRAG = outDto.DEC_Freibetrag;
                                deDetail.MINDESTMIETE = outDto.DEC_Mindestmiete;
                                deDetail.FAKTORBP = outDto.DEC_FaktorBP;
                                deDetail.KUNDENGRUPPE = outDto.DEC_Kundengruppe;
                                deDetail.FRAUD = (int?)outDto.DEC_Fraud_Flag;
                                deDetail.FRAUDSCORE = (int?)outDto.DEC_Fraud_Score;
                                deDetail.CLUSTERVALUE = outDto.DEC_TR_Segment;
                                deDetail.EXTBETRKOSTEN = outDto.DEC_Betreuungskosten; // BNR13

                            }
                            context.DEDETAIL.Add(deDetail);


                            // Add DERUL and DECON
                            if (!string.IsNullOrEmpty(outDto.RecordRRResponseDto[i].DEC_Liste_getroffeneRegeln_Auflagen))
                            {
                                auflagen = outDto.RecordRRResponseDto[i].DEC_Liste_getroffeneRegeln_Auflagen.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                                MyAddDECon(deDetail, context, Auflagen, auflagen);
                            }
                            if (!string.IsNullOrEmpty(outDto.RecordRRResponseDto[i].DEC_Liste_getroffeneRegeln_BP))
                            {
                                bp = outDto.RecordRRResponseDto[i].DEC_Liste_getroffeneRegeln_BP.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                                MyAddDERegel(deDetail, context, BP, bp);
                            }
                            if (!string.IsNullOrEmpty(outDto.RecordRRResponseDto[i].DEC_Liste_getroffeneRegeln_Formalit))
                            {
                                formalitaeten = outDto.RecordRRResponseDto[i].DEC_Liste_getroffeneRegeln_Formalit.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                                MyAddDECon(deDetail, context, Formalitaeten, formalitaeten);
                            }
                            if (!string.IsNullOrEmpty(outDto.RecordRRResponseDto[i].DEC_Liste_getroffeneRegeln_RP))
                            {
                                rp = outDto.RecordRRResponseDto[i].DEC_Liste_getroffeneRegeln_RP.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                                MyAddDERegel(deDetail, context, RP, rp);
                            }
                            if (!string.IsNullOrEmpty(outDto.RecordRRResponseDto[i].DEC_Liste_getroffeneRegeln_VP))
                            {
                                vp = outDto.RecordRRResponseDto[i].DEC_Liste_getroffeneRegeln_VP.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                                MyAddDERegel(deDetail, context, VP, vp);
                            }
                            if (!string.IsNullOrEmpty(outDto.RecordRRResponseDto[i].DEC_Liste_getroffeneRegeln_FP))
                            {
                                fp = outDto.RecordRRResponseDto[i].DEC_Liste_getroffeneRegeln_FP.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                                MyAddDERegel(deDetail, context, FP, fp);
                            }

                            //Add DERULCON
                            MyAddDERegelCon(deDetail, context);

                            // Add DESCORE
                            MyAddDEScore(deDetail, context, outDto.RecordRRResponseDto[i].SC_ID_1, outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_1, outDto.RecordRRResponseDto[i].SC_Bezeichnung_1, outDto.RecordRRResponseDto[i].SC_Eingabewert_1, (int?)outDto.RecordRRResponseDto[i].SC_Resultatwert_1);
                            MyAddDEScore(deDetail, context, outDto.RecordRRResponseDto[i].SC_ID_2, outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_2, outDto.RecordRRResponseDto[i].SC_Bezeichnung_2, outDto.RecordRRResponseDto[i].SC_Eingabewert_2, (int?)outDto.RecordRRResponseDto[i].SC_Resultatwert_2);
                            MyAddDEScore(deDetail, context, outDto.RecordRRResponseDto[i].SC_ID_3, outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_3, outDto.RecordRRResponseDto[i].SC_Bezeichnung_3, outDto.RecordRRResponseDto[i].SC_Eingabewert_3, (int?)outDto.RecordRRResponseDto[i].SC_Resultatwert_3);
                            MyAddDEScore(deDetail, context, outDto.RecordRRResponseDto[i].SC_ID_4, outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_4, outDto.RecordRRResponseDto[i].SC_Bezeichnung_4, outDto.RecordRRResponseDto[i].SC_Eingabewert_4, (int?)outDto.RecordRRResponseDto[i].SC_Resultatwert_4);
                            MyAddDEScore(deDetail, context, outDto.RecordRRResponseDto[i].SC_ID_5, outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_5, outDto.RecordRRResponseDto[i].SC_Bezeichnung_5, outDto.RecordRRResponseDto[i].SC_Eingabewert_5, (int?)outDto.RecordRRResponseDto[i].SC_Resultatwert_5);
                            MyAddDEScore(deDetail, context, outDto.RecordRRResponseDto[i].SC_ID_6, outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_6, outDto.RecordRRResponseDto[i].SC_Bezeichnung_6, outDto.RecordRRResponseDto[i].SC_Eingabewert_6, (int?)outDto.RecordRRResponseDto[i].SC_Resultatwert_6);
                            MyAddDEScore(deDetail, context, outDto.RecordRRResponseDto[i].SC_ID_7, outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_7, outDto.RecordRRResponseDto[i].SC_Bezeichnung_7, outDto.RecordRRResponseDto[i].SC_Eingabewert_7, (int?)outDto.RecordRRResponseDto[i].SC_Resultatwert_7);
                            MyAddDEScore(deDetail, context, outDto.RecordRRResponseDto[i].SC_ID_8, outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_8, outDto.RecordRRResponseDto[i].SC_Bezeichnung_8, outDto.RecordRRResponseDto[i].SC_Eingabewert_8, (int?)outDto.RecordRRResponseDto[i].SC_Resultatwert_8);
                            MyAddDEScore(deDetail, context, outDto.RecordRRResponseDto[i].SC_ID_9, outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_9, outDto.RecordRRResponseDto[i].SC_Bezeichnung_9, outDto.RecordRRResponseDto[i].SC_Eingabewert_9, (int?)outDto.RecordRRResponseDto[i].SC_Resultatwert_9);
                            MyAddDEScore(deDetail, context, outDto.RecordRRResponseDto[i].SC_ID_10, outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_10, outDto.RecordRRResponseDto[i].SC_Bezeichnung_10, outDto.RecordRRResponseDto[i].SC_Eingabewert_10, (int?)outDto.RecordRRResponseDto[i].SC_Resultatwert_10);
                            MyAddDEScore(deDetail, context, outDto.RecordRRResponseDto[i].SC_ID_11, outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_11, outDto.RecordRRResponseDto[i].SC_Bezeichnung_11, outDto.RecordRRResponseDto[i].SC_Eingabewert_11, (int?)outDto.RecordRRResponseDto[i].SC_Resultatwert_11);
                            MyAddDEScore(deDetail, context, outDto.RecordRRResponseDto[i].SC_ID_12, outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_12, outDto.RecordRRResponseDto[i].SC_Bezeichnung_12, outDto.RecordRRResponseDto[i].SC_Eingabewert_12, (int?)outDto.RecordRRResponseDto[i].SC_Resultatwert_12);
                            MyAddDEScore(deDetail, context, outDto.RecordRRResponseDto[i].SC_ID_13, outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_13, outDto.RecordRRResponseDto[i].SC_Bezeichnung_13, outDto.RecordRRResponseDto[i].SC_Eingabewert_13, (int?)outDto.RecordRRResponseDto[i].SC_Resultatwert_13);
                            MyAddDEScore(deDetail, context, outDto.RecordRRResponseDto[i].SC_ID_14, outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_14, outDto.RecordRRResponseDto[i].SC_Bezeichnung_14, outDto.RecordRRResponseDto[i].SC_Eingabewert_14, (int?)outDto.RecordRRResponseDto[i].SC_Resultatwert_14);
                            MyAddDEScore(deDetail, context, outDto.RecordRRResponseDto[i].SC_ID_15, outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_15, outDto.RecordRRResponseDto[i].SC_Bezeichnung_15, outDto.RecordRRResponseDto[i].SC_Eingabewert_15, (int?)outDto.RecordRRResponseDto[i].SC_Resultatwert_15);
                            MyAddDEScore(deDetail, context, outDto.RecordRRResponseDto[i].SC_ID_16, outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_16, outDto.RecordRRResponseDto[i].SC_Bezeichnung_16, outDto.RecordRRResponseDto[i].SC_Eingabewert_16, (int?)outDto.RecordRRResponseDto[i].SC_Resultatwert_16);
                            MyAddDEScore(deDetail, context, outDto.RecordRRResponseDto[i].SC_ID_17, outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_17, outDto.RecordRRResponseDto[i].SC_Bezeichnung_17, outDto.RecordRRResponseDto[i].SC_Eingabewert_17, (int?)outDto.RecordRRResponseDto[i].SC_Resultatwert_17);
                            MyAddDEScore(deDetail, context, outDto.RecordRRResponseDto[i].SC_ID_18, outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_18, outDto.RecordRRResponseDto[i].SC_Bezeichnung_18, outDto.RecordRRResponseDto[i].SC_Eingabewert_18, (int?)outDto.RecordRRResponseDto[i].SC_Resultatwert_18);
                            MyAddDEScore(deDetail, context, outDto.RecordRRResponseDto[i].SC_ID_19, outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_19, outDto.RecordRRResponseDto[i].SC_Bezeichnung_19, outDto.RecordRRResponseDto[i].SC_Eingabewert_19, (int?)outDto.RecordRRResponseDto[i].SC_Resultatwert_19);
                            MyAddDEScore(deDetail, context, outDto.RecordRRResponseDto[i].SC_ID_20, outDto.RecordRRResponseDto[i].SC_Berechnungsvariablen_20, outDto.RecordRRResponseDto[i].SC_Bezeichnung_20, outDto.RecordRRResponseDto[i].SC_Eingabewert_20, (int?)outDto.RecordRRResponseDto[i].SC_Resultatwert_20);
                        }
                    }
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// saveRatingergebnis
        /// </summary>
        /// <param name="outDto">outDto</param>
        /// <param name="sysAuskunft">sysAuskunft</param>
        public void saveRatingergebnis(DecisionEngineOutDto outDto, long sysAuskunft)
        {
            string area = "";
            long? sysid = null;
            long syswfuser = 0;
            long sysprkgroup = 0;
            long ratingsimulid = 0;

            using (DdDeExtended context = new DdDeExtended())
            {

                AUSKUNFT auskunft = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                if (auskunft != null)
                {
                    area = auskunft.AREA;
                    sysid = auskunft.SYSID;
                    syswfuser = auskunft.SYSWFUSER != null ? (long)auskunft.SYSWFUSER : 0;

                }
            }
            // BRNEUEN  cr 84 / BNRNEUN-335
            try
            {
                using (DdOlExtended context = new DdOlExtended())
                {
                    var query = from prkgr in context.PRKGROUP
                                where prkgr.NAME.ToUpper().Trim().Equals(outDto.DEC_Kundengruppe.ToUpper().Trim())
                                select prkgr;
                    PRKGROUP prkgroup = query.FirstOrDefault();
                    if (prkgroup != null)
                    {
                        sysprkgroup = prkgroup.SYSPRKGROUP;
                    }
                    else
                    {
                        _log.Warn("Keine PRKGROUP für DEC_Kundengruppe " + outDto.DEC_Kundengruppe + " vorhanden");
                    }

                }
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim Save von Ratingsimul. Bei PRKGROUP  Error Message: " + ex.Message);
                throw ex;
            }


            try
            {
                using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended context = new Cic.OpenOne.Common.Model.DdOw.DdOwExtended())
                {

                    if (outDto.RecordRRResponseDto != null && outDto.RecordRRResponseDto.Length > 0)
                    {
                        List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "parea", Value = area });
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysid", Value = sysid });

                        long sysrating = context.ExecuteStoreQuery<long>(RATINGQUERY, parameters.ToArray()).FirstOrDefault();
                        if (sysrating>0)
                        {
                             
                            var ratingsimulvar = from rs in context.RATINGSIMUL
                                                 where rs.SYSRATING == sysrating
                                                 select rs;

                            RATINGSIMUL ratingsimul = ratingsimulvar.FirstOrDefault();



                            if (ratingsimul == null)
                            {
                                ratingsimul = new RATINGSIMUL();
                                ratingsimul.SYSRATING= sysrating;
                                context.RATINGSIMUL.Add(ratingsimul);
                                ratingsimul.SYSCRTDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                                ratingsimul.SYSCRTTIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                                ratingsimul.SYSCRTUSER = syswfuser;

                            }
                            ratingsimul.RKLCODESIM = outDto.RecordRRResponseDto[0].DEC_Risikoklasse_Bezeichnung;
                            ratingsimul.RKLCODE = outDto.RecordRRResponseDto[0].DEC_Risikoklasse_Bezeichnung;
                            ratingsimul.SCORETOTALSIM = outDto.RecordRRResponseDto[0].DEC_Scorewert_Total.ToString();
                            ratingsimul.SCORETOTAL = outDto.RecordRRResponseDto[0].DEC_Scorewert_Total.ToString();
                            ratingsimul.SCOREKARTESIM = outDto.RecordRRResponseDto[0].DEC_Scorekarte_Bezeichnung;
                            ratingsimul.SCOREKARTE = outDto.RecordRRResponseDto[0].DEC_Scorekarte_Bezeichnung;
                            ratingsimul.SYSCHGDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                            ratingsimul.SYSCHGTIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                            ratingsimul.SYSCHGUSER = syswfuser;
                            //BRNEUN CR84
                            ratingsimul.AREA = "PRKGROUP";
                            ratingsimul.SYSID = sysprkgroup;
                            ratingsimul.AREASIM = "PRKGROUP";
                            ratingsimul.SYSIDSIM = sysprkgroup;

                            //BRZEHN IN DB und EDMX : BRZEHN CR 132 FRAUD 3.5.3 Anpassung Ratingsimul BNRZEHN-1763
                            ratingsimul.SCORETOTALFRAUD = outDto.DEC_Fraud_Score.ToString();
                            ratingsimul.SCORETOTALFRAUDSIM = outDto.DEC_Fraud_Score.ToString();

                            context.SaveChanges();
                            ratingsimulid = ratingsimul.SYSRATINGSIMUL;



                        }



                    }

                }

            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim Save von Ratingsimul. Error Message: " + ex.Message);
                throw ex;
            }


        }


        /// <summary>
        /// Get entities from database by SysAuskunft and map entities to DecisionEngineInDto
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>DecisionEngineInDto, filled with values from database</returns>
        public DecisionEngineInDto FindBySysId(long sysAuskunft)
        {
            DecisionEngineInDto inDto = new DecisionEngineInDto();
            DEINPEXEC deinp;
            DEENVINP deEnvInp;
            DEARBEIT[] deArbeit;
            DEZEK[] deZek;
            DEADRBONI[] deAdrBon;
            DEAPPLICANT[] app;
            DEOBJECT deObj;
            DEOPENLEASE[] deOl;
            DEPARTNER part;
            DESALES sales;
            DECONTRACT contract;
            DEBUDGET[] debudget;
            DEFRAUD defraud;

            try
            {
                using (DdDeExtended context = new DdDeExtended())
                {
                    var query = from Auskunft in context.AUSKUNFT
                                from deInpExe in Auskunft.DEINPEXECList
                                where Auskunft.SYSAUSKUNFT == sysAuskunft
                                select deInpExe;
                    deinp = query.First();

                    // object, sales, partner, contract, envelope darf nur einmal vorhanden sein (NR) --> .Single()
                    // zek, ol, arbeit, adrBoni, applicant darf einmal (1 Antragsteller) oder zweimal (2 Antragsteller) vorhanden sein (RR)
                    // Bei 2 AS entspricht der Datensatz mit der kleineren sysId dem ersten AS, deswegen werden die 5 Tabellen ASC sortiert.
                    context.Entry(deinp).Collection(f => f.DEENVINPList).Load();
                    
                    deEnvInp = deinp.DEENVINPList.Single();
                    context.Entry(deinp).Collection(f => f.DEOBJECTList).Load();
                    
                    deObj = deinp.DEOBJECTList.Single();
                    context.Entry(deinp).Collection(f => f.DEPARTNERList).Load();
                    
                    part = deinp.DEPARTNERList.Single();
                    context.Entry(deinp).Collection(f => f.DESALESList).Load();
                    
                    sales = deinp.DESALESList.Single();
                    context.Entry(deinp).Collection(f => f.DECONTRACTList).Load();
                    
                    contract = deinp.DECONTRACTList.Single();
                    context.Entry(deinp).Collection(f => f.DEARBEITList).Load();
                    
                    deArbeit = deinp.DEARBEITList.OrderBy(rec => rec.SYSDEARBEIT).ToArray();
                    context.Entry(deinp).Collection(f => f.DEADRBONIList).Load();
                    
                    deAdrBon = deinp.DEADRBONIList.OrderBy(rec => rec.SYSDEADRBONI).ToArray();
                    context.Entry(deinp).Collection(f => f.DEAPPLICANTList).Load();
                    
                    app = deinp.DEAPPLICANTList.OrderBy(rec => rec.SYSDEAPPLICANT).ToArray();
                    context.Entry(deinp).Collection(f => f.DEOPENLEASEList).Load();
                    
                    deOl = deinp.DEOPENLEASEList.OrderBy(rec => rec.SYSDEOPENLEASE).ToArray();
                    context.Entry(deinp).Collection(f => f.DEZEKList).Load();
                    
                    deZek = deinp.DEZEKList.OrderBy(rec => rec.SYSDEZEK).ToArray();
                    context.Entry(deinp).Collection(f => f.DEBUDGETList).Load();
                    
                    debudget = deinp.DEBUDGETList.OrderBy(rec => rec.SYSDEBUDGET).ToArray();

                    // falls zek || ol || arbeit || adrBoni || applicant doppelt, wird RecordRRDto mit Länge 2 initialisiert
                    if (deArbeit.Count() == 2 || app.Count() == 2 || deAdrBon.Count() == 2 || deOl.Count() == 2 || deZek.Count() == 2)
                    {
                        inDto.RecordRRDto = new RecordRRDto[2];
                    }
                    else
                    {
                        inDto.RecordRRDto = new RecordRRDto[1];
                    }

                    string[] defraudArray;
                    context.Entry(deinp).Collection(f => f.DEFRAUDList).Load();
                    
                    if (deinp.DEFRAUDList != null && deinp.DEFRAUDList.Count != 0)
                    {
                        defraud = deinp.DEFRAUDList.Single();
                        defraudArray = MygetDEFraudValues(defraud.VALUE);
                    }
                    else
                        throw new ApplicationException("Exception Decision Engine DEFRAUD  is not found for : SYSDEINPEXEC =" + deinp.SYSDEINPEXEC);

                    MyMapEntitiesToInDto(deEnvInp, part, sales, contract, deObj, deArbeit, app, deAdrBon, deZek, deOl, inDto, inDto.RecordRRDto.Length, debudget, defraudArray);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim Laden von DecisionEngine-Daten. Error Message: " + ex.Message);
                throw ex;
            }
            return inDto;
        }


        /// <summary>
        /// getGetroffenenRegeln
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public List<DERUL> getGetroffenenRegeln(long sysAuskunft)
        {
            try
            {
                using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended context = new Cic.OpenOne.Common.Model.DdOw.DdOwExtended())
                {

                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysauskunft", Value = sysAuskunft });

                    List<DERUL> deruls = context.ExecuteStoreQuery<DERUL>(DERULQUERY, parameters.ToArray()).ToList();
                    if (deruls != null)
                    {
                        return deruls;
                    }

                }
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim suche von getroffen Regeln. Error Message: " + ex.Message);
                throw ex;
            }

            return null;
        }


        /// <summary>
        /// getGetroffenenRegeln
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public List<string> getGetroffenenRegelnCode(long sysAuskunft)
        {
            try
            {
                using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended context = new Cic.OpenOne.Common.Model.DdOw.DdOwExtended())
                {

                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysauskunft", Value = sysAuskunft });

                    List<string> deruls = context.ExecuteStoreQuery<string>(DERULEXTERNCODEQUERY, parameters.ToArray()).ToList();
                    if (deruls != null)
                    {
                        return deruls;
                    }

                }
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim suche von ExternCode der getroffen Regeln. Error Message: " + ex.Message);
                throw ex;
            }

            return null;
        }


        public int getGetroffenenRegelnAnzahl(long sysAuskunft)
        {
            int count = 0;
            try
            {
                using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended context = new Cic.OpenOne.Common.Model.DdOw.DdOwExtended())
                {

                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysauskunft", Value = sysAuskunft });

                    count = context.ExecuteStoreQuery<int>(DERULANZAHLQUERY, parameters.ToArray()).FirstOrDefault();


                }
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim suche von ExternCode der getroffen Regeln. Error Message: " + ex.Message);
                throw ex;
            }

            return count;
        }


        #region Private Methods

        /// <summary>
        /// Mapping Entities -> DecisionEngineInDto
        /// </summary>
        /// <param name="deEnvInp"></param>
        /// <param name="partner"></param>
        /// <param name="sales"></param>
        /// <param name="contract"></param>
        /// <param name="deObject"></param>
        /// <param name="deArbeitArray"></param>
        /// <param name="deAppArray"></param>
        /// <param name="deAdrBonArray"></param>
        /// <param name="deZekArray"></param>
        /// <param name="deOLArray"></param>
        /// <param name="inDto"></param>
        /// <param name="rrDtoLength"></param>
        /// <param name="debudgetArray"></param>
        /// <param name="defraudArray"></param>
        private void MyMapEntitiesToInDto(DEENVINP deEnvInp, DEPARTNER partner, DESALES sales, DECONTRACT contract, DEOBJECT deObject,
                                          DEARBEIT[] deArbeitArray, DEAPPLICANT[] deAppArray, DEADRBONI[] deAdrBonArray, DEZEK[] deZekArray,
                                          DEOPENLEASE[] deOLArray, DecisionEngineInDto inDto, int rrDtoLength, DEBUDGET[] debudgetArray, string[] defraudArray)
        {
            inDto.FlagBonitaetspruefung = deEnvInp.FLAGBONITAETSPRUEFUNG;
            inDto.FlagRisikopruefung = deEnvInp.FLAGRISIKOPRUEFUNG;
            inDto.FlagVorpruefung = deEnvInp.FLAGVORPRUEFUNG;
            inDto.InquiryCode = deEnvInp.INQUIRYCODE;
            inDto.InquiryDate = deEnvInp.INQUIRYDATE;
            inDto.InquiryTime = deEnvInp.INQUIRYTIME;
            inDto.OrganizationCode = deEnvInp.ORGANIZATIONCODE;
            inDto.ProcessCode = deEnvInp.PROCESSCODE;

            inDto.Erfassung = deEnvInp.ERFASSUNG;

            // ProcessVersion wird jetzt (ab 2011-10-06) im AdminBox gepflegt,
            // wird also doch übergeben. LayoutVersion wird nicht übergeben.
            inDto.ProcessVersion = deEnvInp.PROCESSVERSION;

            inDto.Erfassungskanal = contract.ERFASSUNGSKANAL;
            inDto.Geschaeftsart = contract.GESCHAEFTSART;
            inDto.KKG_Pflicht = (decimal?)contract.KKGPFLICHT;
            inDto.Laufzeit = (decimal?)contract.LAUFZEIT;
            inDto.Nutzungsart = contract.NUTZUNGSART;
            inDto.PPI_Flag_Paket1 = (decimal?)contract.PPIFLAGPAKET1;
            inDto.PPI_Flag_Paket2 = (decimal?)contract.PPIFLAGPAKET2;
            //BNRSZ-1371
            inDto.PPI_Betrag = (decimal?)contract.PPIBETRAG; 
            inDto.Riskflag = (decimal?)contract.RISKFLAG;
            inDto.Vertragsart = (decimal?)contract.VERTRAGSART;
            inDto.Anzahlung_ErsteRate = contract.ANZAHLUNERSTERATE;

            //BNRSZ
            inDto.Erneute_Pruefung = (decimal?)contract.ERNEUTEPRUEFUNG;
            inDto.Finanzierungsbetragbew = contract.FINANZIERUNGSBETRAGBEW;
            inDto.PPI_Betrag_Bewilligt = contract.PPIBEW;
            inDto.Toleranzriskdec = contract.TOLERANZRISKDEC;


            inDto.Finanzierungsbetrag = contract.FINANZIERUNGSBETRAG;
            inDto.Kaution = contract.KAUTION;
            inDto.Rate = contract.RATE;
            inDto.Restwert = contract.RESTWERT;
            inDto.Zinssatz = contract.ZINSSATZ;

            inDto.AuszahlungsArt = contract.AUSZAHLUNGSART;
            inDto.BuchwertGarantie = contract.BUCHWERTGARANTIE;
            inDto.sysVTTYP = contract.VTTYP;
            inDto.Umschreibung = contract.UMSCHREIBUNG;
            //BNR10
            inDto.RandomNumber = (decimal?)contract.RANDOMNUMBER;
            //BNR11
            inDto.RW_Verl = (decimal?)contract.RWVERLFLAG;
            inDto.Vertragsversion_NEU = (decimal?)contract.FLAGNEULV;
            //BNR13
            inDto.Restwertgarant = getRWGA(contract.SYSDECONTRACT);

            inDto.Inverkehrssetzung = (DateTime?)deObject.INVERKEHRSSETZUNG;
            inDto.KM_prJahr = (decimal?)deObject.KMPROJAHR;
            inDto.KM_Stand = (decimal?)deObject.KMSTAND;
            inDto.Zustand = (decimal?)deObject.ZUSTAND;
            inDto.Marktwert_Cluster = deObject.CLUSTEROBJEKT != null ? deObject.CLUSTEROBJEKT.Trim().ToUpper() : deObject.CLUSTEROBJEKT;
            inDto.Expected_Loss = deObject.EWBBETRAG;
            inDto.Expected_Loss_Prozent = deObject.EWBPROC;
            inDto.Expected_Loss_LGD = deObject.EWBLGD;
            inDto.Profitabilitaet_Prozent = deObject.EWBPROF;
            inDto.VIN_Nummer = deObject.FIDENT;
            inDto.Restwertabsicherung = deObject.RESTWERTABSICHERUNG;
            
            inDto.Ausstattung = deObject.AUSSTATTUNG;

            inDto.Fahrzeugpreis_Eurotax = deObject.FAHRZEUGPREISEUROTAX;
            inDto.Katalogpreis_Eurotax = deObject.KATALOGPREISEUROTAX;
            inDto.Marke = deObject.MARKE != null ? deObject.MARKE.ToUpper() : deObject.MARKE;
            inDto.Modell = deObject.MODELL != null ? deObject.MODELL.ToUpper() : deObject.MODELL;
            inDto.Objektart = deObject.OBJEKTART;
            inDto.Restwert_Banknow = deObject.RESTWERTBANK;
            inDto.Restwert_Eurotax = deObject.RESTWERTEUROTAX;
            inDto.Stammnummer = deObject.STAMMNUMMER;
            inDto.Zubehoerpreis = deObject.ZUBEHOERPREIS;
            //BNRNEUN-1746 
            inDto.Ciconeversion = deEnvInp.CICONEVERSION;

            // Vertriebs-Partner
            inDto.Demolimit = partner.DEMOLIMIT;
            inDto.Demoengagement = partner.DEMOENGAGEMENT;
            inDto.Eventualdemoengagement = partner.EVENTUALDEMOENGAGEMENT;

            inDto.Anz_Antraege = (decimal?)partner.ANZANTRAEGE;
            inDto.Anz_lfd_Vertraege = (decimal?)partner.ANZLFDVERTRAEGE;
            inDto.Anz_pendente_Antraege = (decimal?)partner.ANZPENDENTEANTRAEGE;
            inDto.Anz_Vertraege = (decimal?)partner.ANZVERTRAEGE;
            inDto.flagAktiv = (decimal?)partner.FLAGAKTIV;
            inDto.flagEPOS = (decimal?)partner.FLAGEPOS;
            inDto.flagVSB = (decimal?)partner.FLAGVSB;
            inDto.VertriebspartnerID = (decimal?)partner.VERTRIEBSPARTNERID;
            inDto.Eventualrestwertengagement = partner.EVENTUALRESTWERTENGAGEMENT;
            inDto.Eventualvolumenengagement = partner.EVENTUALVOLUMENENGAGEMENT;
            inDto.Garantenlimite = partner.GARANTENLIMITE;
            inDto.PLZ = partner.PLZ;
            inDto.Restwertengagement = partner.RESTWERTENGAGEMENT;
            inDto.Volumenengagement = partner.VOLUMENENGAGEMENT;
            inDto.Rechtsform = partner.RECHTSFORM;
            inDto.Sprache = partner.SPRACHE;
            inDto.Vertriebspartnerart = partner.VERTRIEBSPARTNERART;
            inDto.UIDNummer = partner.UIDNUMMER;
            inDto.Strategic_Account = partner.STRATEGICACCOUNT;
            //BNRACHT-615
            inDto.Badlisteintrag = (decimal?)partner.BADLISTEINTRAG;
            //BNR10 
            inDto.Vermittlerart = partner.VERMITTLERART;

            // Fraud

            if (defraudArray != null)
                inDto.I_F_Values = defraudArray;






            // sales
            inDto.Anz_Abloesen = (decimal?)sales.ANZABLOESEN;
            inDto.Anz_Eigenabloesen = (decimal?)sales.ANZEIGENABLOESEN;
            inDto.Anz_Fremdabloesen = (decimal?)sales.ANZFREMDABLOESEN;
            inDto.Name_Abloesebank_1 = sales.NAMEABLOESEBANK1 != null ? sales.NAMEABLOESEBANK1.ToUpper() : sales.NAMEABLOESEBANK1;
            inDto.Name_Abloesebank_2 = sales.NAMEABLOESEBANK2 != null ? sales.NAMEABLOESEBANK2.ToUpper() : sales.NAMEABLOESEBANK2;
            inDto.Name_Abloesebank_3 = sales.NAMEABLOESEBANK3 != null ? sales.NAMEABLOESEBANK3.ToUpper() : sales.NAMEABLOESEBANK3;
            inDto.Name_Abloesebank_4 = sales.NAMEABLOESEBANK4 != null ? sales.NAMEABLOESEBANK4.ToUpper() : sales.NAMEABLOESEBANK4;
            inDto.Name_Abloesebank_5 = sales.NAMEABLOESEBANK5 != null ? sales.NAMEABLOESEBANK5.ToUpper() : sales.NAMEABLOESEBANK5;
            inDto.Summe_Abloesen = sales.SUMMEABLOESEN;
            inDto.Summe_Eigenabloesen = sales.SUMMEEIGENABLOESEN;

            //BNR13  TODO IN EDMX UND DB
            ExtraSalesFelder extraSalesFelder = new ExtraSalesFelder();
            extraSalesFelder = getExtraSalesFelder(sales.SYSDESALES);
            inDto.Abl_Produkt = extraSalesFelder.SYSVARTABL;
            inDto.Abl_Dauer_Total = extraSalesFelder.VTDAUERABLTOTAL;
            inDto.Abl_Anz_Vertragsart = extraSalesFelder.ANZABLVTPERVART;
            inDto.Abl_Dauer_Vertragsart = extraSalesFelder.VTDAUERABLPERVART;
            inDto.Abl_LZ_Vorvertrag_Vertragsart = extraSalesFelder.VTDAUERABL;


            inDto.Validabl = sales.VALIDABL != null ? (decimal)sales.VALIDABL : 0;

            for (int loop = 0; loop < rrDtoLength; loop++)
            {
                RecordRRDto rrDto = new RecordRRDto();
                DEOPENLEASE deOL = null;
                DEZEK deZek = null;
                DEARBEIT deArbeit = null;
                DEADRBONI deAdrBon = null;
                DEAPPLICANT deApp = null;
                DEBUDGET debudget = null;

                // DEOPENLEASE
                switch (loop)
                {
                    case 0:
                        deOL = deOLArray.First();
                        deZek = deZekArray.First();
                        deArbeit = deArbeitArray.First();
                        deAdrBon = deAdrBonArray.First();
                        deApp = deAppArray.First();
                        debudget = debudgetArray.First();
                        break;
                    case 1:
                        if (deOLArray.Count() == 2)
                            deOL = deOLArray.ElementAt(1);
                        if (deZekArray.Count() == 2)
                            deZek = deZekArray.ElementAt(1);
                        if (deArbeitArray.Count() == 2)
                            deArbeit = deArbeitArray.ElementAt(1);
                        if (deAdrBonArray.Count() == 2)
                            deAdrBon = deAdrBonArray.ElementAt(1);
                        if (deAppArray.Count() == 2)
                            deApp = deAppArray.ElementAt(1);
                        if (debudgetArray.Count() == 2)
                            debudget = debudgetArray.ElementAt(1);
                        break;
                }

                if (deOL != null)
                {
                    rrDto.OL_Anz_Annulierungen_l12M = (decimal?)deOL.ANZANNULIERUNGEN12M;
                    rrDto.OL_Anz_Antraege = (decimal?)deOL.ANZANTRAEGE;
                    rrDto.OL_Anz_KundenIDs = (decimal?)deOL.ANZKUNDENIDS;
                    rrDto.OL_Anz_lfd_Vertraege = (decimal?)deOL.ANZLFDVERTRAEGE;
                    rrDto.OL_Anz_Mahnungen_1 = (decimal?)deOL.ANZMAHNUNGEN1;
                    rrDto.OL_Anz_Mahnungen_2 = (decimal?)deOL.ANZMAHNUNGEN2;
                    rrDto.OL_Anz_Mahnungen_3 = (decimal?)deOL.ANZMAHNUNGEN3;
                    rrDto.OL_Anz_Mehrfachantraege = (decimal?)deOL.ANZMEHRFACHANTRAEGE;
                    rrDto.OL_Anz_OP = (decimal?)deOL.ANZOP;
                    rrDto.OL_Anz_Stundungen = (decimal?)deOL.ANZSTUNDUNGEN;
                    rrDto.OL_Anz_Vertraege = (decimal?)deOL.ANZVERTRAEGE;
                    rrDto.OL_Anz_Vertraege_im_Recovery = (decimal?)deOL.ANZVERTRAEGEIMRECOVERY;
                    rrDto.OL_Anz_Verzichte_l12M = (decimal?)deOL.ANZVERZICHTE12M;
                    rrDto.OL_Anz_Zahlungsvereinbarungen = (decimal?)deOL.ANZZAHLUNGSVEREINBARUNGEN;
                    rrDto.OL_Dauer_Kundenbeziehung = (decimal?)deOL.DAUERKUNDENBEZIEHUNG;
                    rrDto.OL_Effektive_Kundenbeziehung = (decimal?)deOL.EFFEKTIVEKUNDENBEZIEHUNG;
                    rrDto.OL_Maximale_akt_RKlasse_des_Kunden = (decimal?)deOL.MAXAKTRKLASSEKUNDE;
                    rrDto.OL_Maximaler_Bandlisteintrag = (decimal?)deOL.MAXBADLISTEINTRAG;
                    rrDto.OL_Maximale_Mahnstufe = (decimal?)deOL.MAXIMALEMAHNSTUFE;
                    rrDto.OL_Maximale_Risikoklasse_des_Kunden = (decimal?)deOL.MAXRISIKOKLASSEKUNDE;
                    rrDto.OL_Status = (decimal?)deOL.STATUS;
                    rrDto.OL_Engagement = deOL.ENGAGEMENT;
                    rrDto.OL_Eventualengagement = deOL.EVENTUALENGAGEMENT;
                    rrDto.OL_Gesamtengagement = deOL.GESAMTENGAGEMENT;
                    rrDto.OL_Haushaltsengagement = deOL.HAUSHALTENGAGEMENT;
                    rrDto.OL_Letzte_Miete = deOL.LETZTEMIETE;
                    rrDto.OL_Letzte_Nationalitaet = deOL.LETZTENATIONALITAET;
                    rrDto.OL_Letzter_Bonus = deOL.LETZTERBONUS;
                    rrDto.OL_Letztes_Haupteinkommen = deOL.LETZTESHAUPTEINKOMMEN;
                    rrDto.OL_Letztes_Nebeneinkommen = deOL.LETZTESNEBENEINKOMMEN;
                    rrDto.OL_Letztes_Zusatzeinkommen = deOL.LETZTESZUSATZEINKOMMEN;
                    rrDto.OL_Minimales_Datum_Kunde = deOL.MINDATUMKUNDESEIT;
                    rrDto.OL_OpenLease_Datum_der_Anmeldung = deOL.OPENLEASEDATUMAUSKUNFT;
                    rrDto.OL_Summe_OP = deOL.SUMMEOP;
                    rrDto.OL_Letztes_Arbeitsverhaeltnis = deOL.LETZTESARBEITSVERHAELTNIS;
                    rrDto.OL_Letztes_Wohnverhaeltnis = deOL.LETZTESWOHNVERHAELTNIS;
                    rrDto.OL_Letzter_Zivilstand = deOL.LETZTERZIVILSTAND;
                    rrDto.OL_Anz_manAblehnungen_l6M = (decimal?)deOL.ANZMANABL6M;
                    rrDto.OL_Anz_manAblehnungen_l12M = (decimal?)deOL.ANZMANABL12M;
                    rrDto.OL_Anz_Vertraege_mit_Spezialfall = (decimal?)deOL.ANZVTSPEZ;
                    rrDto.OL_Anz_lfd_Vertraege_mit_Spezialfall = (decimal?)deOL.ANZVTSPEZLFD;
                    rrDto.OL_Datum_Mahnung_1 = deOL.DATEMAHN1;
                    rrDto.OL_Datum_Mahnung_2 = deOL.DATEMAHN2;
                    rrDto.OL_Datum_Mahnung_3 = deOL.DATEMAHN3;
                    rrDto.OL_Datum_letzte_Stundung = deOL.DATESTUNDUNGEN;
                    rrDto.OL_Datum_letzte_ZVB = deOL.DATEZVB;
                    rrDto.OL_Anzahl_Aufstockungssperren = (decimal?)deOL.ANZAUFSTOCKSTOP;
                    rrDto.OL_Datum_Aufstockungssperre = deOL.DATEAUFSTOCKSTOP;
                    rrDto.OL_Letzte_Einkommensart = deOL.LETZTEEINKOMMENART;
                    //BNR10
                    rrDto.OL_Gbezeichnung = (decimal?)deOL.GBEZIEHUNG;
                    rrDto.OL_Ratenpausen = (decimal?)deOL.RATENPAUSEN;

                    //BNR13
                    ExtraDOLFelder extraDOLFelder = new ExtraDOLFelder();
                    extraDOLFelder = getExtraDOLFelder(deOL.SYSDEOPENLEASE);
                    rrDto.OL_Datum_erster_Antrag = extraDOLFelder.DATUMERSTERANTRAG;
                    rrDto.OL_Datum_letzter_Antrag = extraDOLFelder.DATUMLETZTERANTRAG;
                    rrDto.OL_Anzahl_Mahnstufe1_L6M = extraDOLFelder.ANZMAHN1AVG6M;
                    rrDto.OL_Anzahl_Mahnstufe2_L6M = extraDOLFelder.ANZMAHN2AVG6M;
                    rrDto.OL_Anzahl_Mahnstufe3_L6M = extraDOLFelder.ANZMAHN3AVG6M;
                    rrDto.OL_Anzahl_Einzahlungen_L12M = extraDOLFelder.ANZZAHLAVG12M;
                    rrDto.OL_Zahlungsrueckstand = extraDOLFelder.RUECKSTANDAVG;
                    rrDto.OL_Saldoreduktion_L12M = extraDOLFelder.BUCHSALDOAVG;





                }
                //BRNEUN /   BNRNEUN-1746 
                if (debudget != null)
                {
                    rrDto.BU_AnfrageDatum = (DateTime)debudget.ANFRAGEDATUM;
                    rrDto.BU_Grundbetrag = debudget.GRUNDBETRAG;
                    rrDto.BU_Krankenkasse = debudget.KRANKENKASSE;
                    rrDto.BU_Quellsteuer = debudget.QUELLSTEUER;
                    rrDto.BU_Sozialauslagen = debudget.SOZIALAUSLAGEN;
                    rrDto.BU_Budgetueberschuss = debudget.BUDGETUEBERSCHUSS;
                    rrDto.BU_Budgetueberschuss_gesamt = debudget.BUDGETUEBERSCHUSSGESAMT;
                    rrDto.BU_Kremocode = (decimal?)debudget.KREMOCODE;
                    //BNR10
                    rrDto.BU_Einknettoberech = debudget.EINKNETTOBERECH;
                    rrDto.BU_Einknettoberech2 = debudget.EINKNETTOBERECH2;
                    //BNR13
                    rrDto.BU_Betreuungskosten_Extern = debudget.EXTBETRKOSTENTAT;
                    rrDto.BU_Arbeitswegpauschale = debudget.ARBEITSWEGAUSLAGE;
                    rrDto.BU_Krankenkassenpraemie = debudget.KRANKENKASSETAT;




                }
                // ZEK                
                if (deZek != null)
                {
                    rrDto.ZEK_Anz_lfd_ZEK_Eng = (decimal?)deZek.ANZLFDZEKENG;
                    rrDto.ZEK_Anz_lfd_ZEK_Eng_Bardarlehen = (decimal?)deZek.ANZLFDZEKENGBARDARLEHEN;
                    rrDto.ZEK_Anz_lfd_ZEK_Eng_Festkredit = (decimal?)deZek.ANZLFDZEKENGFESTKREDIT;
                    rrDto.ZEK_Anz_lfd_ZEK_Eng_Kartenengagement = (decimal?)deZek.ANZLFDZEKENGKARTEN;
                    rrDto.ZEK_Anz_lfd_ZEK_Eng_Kontokorrent = (decimal?)deZek.ANZLFDZEKENGKONTOKORRENT;
                    rrDto.ZEK_Anz_lfd_ZEK_Eng_Leasing = (decimal?)deZek.ANZLFDZEKENGLEASING;
                    rrDto.ZEK_Anz_lfd_ZEK_Eng_Teilz = (decimal?)deZek.ANZLFDZEKENGTEILZVERTRAG;

                    rrDto.ZEK_Anz_lfd_ZEK_FremdGes = (decimal?)deZek.ANZLFDZEKFREMDGES;
                    rrDto.ZEK_Anz_lfd_ZEK_EigenGes = (decimal?)deZek.ANZLFDZEKEIGENGES;

                    rrDto.ZEK_Anz_ZEK_AmtsMelden_01_05 = (decimal?)deZek.ANZZEKAMTSMELDEN0105;
                    rrDto.ZEK_Anz_ZEK_AmtsMelden_01_05_l12M = (decimal?)deZek.ANZZEKAMTSMELDEN010512M;

                    rrDto.ZEK_Anz_lfd_ZEK_Eng_BCode_03 = (decimal?)deZek.ANZZEKENGMBCODE03;
                    rrDto.ZEK_Anz_lfd_ZEK_Eng_BCode_03l12M = (decimal?)deZek.ANZZEKENGMBCODE0312M;
                    rrDto.ZEK_Anz_lfd_ZEK_Eng_BCode_03l24M = (decimal?)deZek.ANZZEKENGMBCODE0324M;
                    rrDto.ZEK_Anz_lfd_ZEK_Eng_BCode_03l36M = (decimal?)deZek.ANZZEKENGMBCODE0336M;
                    rrDto.ZEK_Anz_lfd_ZEK_Eng_BCode_04 = (decimal?)deZek.ANZZEKENGMBCODE04;
                    rrDto.ZEK_Anz_lfd_ZEK_Eng_BCode_04l12M = (decimal?)deZek.ANZZEKENGMBCODE0412M;
                    rrDto.ZEK_Anz_lfd_ZEK_Eng_BCode_04l24M = (decimal?)deZek.ANZZEKENGMBCODE0424M;
                    rrDto.ZEK_Anz_lfd_ZEK_Eng_BCode_04l36M = (decimal?)deZek.ANZZEKENGMBCODE0436M;
                    rrDto.ZEK_Anz_lfd_ZEK_Eng_BCode_0506 = (decimal?)deZek.ANZZEKENGMBCODE0506;

                    rrDto.ZEK_Anz_ZEK_Ges_m_AblCode = (decimal?)deZek.ANZZEKGESMABLCODE;
                    rrDto.ZEK_Anz_ZEK_Ges_m_AblCode_04_l12M = (decimal?)deZek.ANZZEKGESMABLCODE0412M;
                    rrDto.ZEK_Anz_ZEK_Ges_m_AblCode_04_l24M = (decimal?)deZek.ANZZEKGESMABLCODE0424M;
                    rrDto.ZEK_Anz_ZEK_Ges_m_AblCode_07_l12M = (decimal?)deZek.ANZZEKGESMABLCODE0712M;
                    rrDto.ZEK_Anz_ZEK_Ges_m_AblCode_07_l24M = (decimal?)deZek.ANZZEKGESMABLCODE0724M;
                    rrDto.ZEK_Anz_ZEK_Ges_m_AblCode_09_l12M = (decimal?)deZek.ANZZEKGESMABLCODE0912M;
                    rrDto.ZEK_Anz_ZEK_Ges_m_AblCode_09_l24M = (decimal?)deZek.ANZZEKGESMABLCODE0924M;
                    rrDto.ZEK_Anz_ZEK_Ges_m_AblCode_10_l12M = (decimal?)deZek.ANZZEKGESMABLCODE1012M;
                    rrDto.ZEK_Anz_ZEK_Ges_m_AblCode_10_l24M = (decimal?)deZek.ANZZEKGESMABLCODE1024M;
                    rrDto.ZEK_Anz_ZEK_Ges_m_AblCode_13_l12M = (decimal?)deZek.ANZZEKGESMABLCODE1312M;
                    rrDto.ZEK_Anz_ZEK_Ges_m_AblCode_13_l24M = (decimal?)deZek.ANZZEKGESMABLCODE1324M;
                    rrDto.ZEK_Anz_ZEK_Ges_m_AblCode_14_l12M = (decimal?)deZek.ANZZEKGESMABLCODE1412M;
                    rrDto.ZEK_Anz_ZEK_Ges_m_AblCode_14_l24M = (decimal?)deZek.ANZZEKGESMABLCODE1424M;
                    rrDto.ZEK_Anz_ZEK_Ges_m_AblCode_99_l12M = (decimal?)deZek.ANZZEKGESMABLCODE9912M;
                    rrDto.ZEK_Anz_ZEK_Ges_m_AblCode_99_l24M = (decimal?)deZek.ANZZEKGESMABLCODE9924M;
                    rrDto.ZEK_Anz_ZEK_Ges_m_AblCode_05060812 = (decimal?)deZek.ANZZEKGESMABLCODE05060812;
                    rrDto.ZEK_Anz_ZEK_Ges_m_AblCode_040709 = (decimal?)deZek.ANZZEKGESMABLCODE040709;

                    rrDto.ZEK_Anz_ZEK_KMeld_m_ErCode_21_l12M = (decimal?)deZek.ANZZEKKMELDMERCODE2112M;
                    rrDto.ZEK_Anz_ZEK_KMeld_m_ErCode_21_l24M = (decimal?)deZek.ANZZEKKMELDMERCODE2124M;
                    rrDto.ZEK_Anz_ZEK_KMeld_m_ErCode_21_l36M = (decimal?)deZek.ANZZEKKMELDMERCODE2136M;
                    rrDto.ZEK_Anz_ZEK_KMeld_m_ErCode_21_l48M = (decimal?)deZek.ANZZEKKMELDMERCODE2148M;
                    rrDto.ZEK_Anz_ZEK_KMeld_m_ErCode_21_l60M = (decimal?)deZek.ANZZEKKMELDMERCODE2160M;
                    rrDto.ZEK_Anz_ZEK_KMeld_m_ErCode_22_l12M = (decimal?)deZek.ANZZEKKMELDMERCODE2212M;
                    rrDto.ZEK_Anz_ZEK_KMeld_m_ErCode_22_l24M = (decimal?)deZek.ANZZEKKMELDMERCODE2224M;
                    rrDto.ZEK_Anz_ZEK_KMeld_m_ErCode_22_l36M = (decimal?)deZek.ANZZEKKMELDMERCODE2236M;
                    rrDto.ZEK_Anz_ZEK_KMeld_m_ErCode_22_l48M = (decimal?)deZek.ANZZEKKMELDMERCODE2248M;
                    rrDto.ZEK_Anz_ZEK_KMeld_m_ErCode_22_l60M = (decimal?)deZek.ANZZEKKMELDMERCODE2260M;
                    rrDto.ZEK_Anz_ZEK_KMeld_m_ErCode_23_24_25_26 = (decimal?)deZek.ANZZEKKMELDMERCODE23242526;

                    rrDto.ZEK_Anz_ZEK_Synonyme = (decimal?)deZek.ANZZEKSYNONYME;
                    rrDto.ZEK_schlechtester_ZEK_AblCode = (decimal?)deZek.SCHLECHTESTERZEKABLCODE;
                    rrDto.ZEK_schlechtester_ZEK_Code = (decimal?)deZek.SCHLECHTESTERZEKBCODE;
                    rrDto.ZEK_Status = (decimal?)deZek.STATUS;
                    rrDto.ZEK_Datum_der_Auskunft = deZek.DATUMAUSKUNFT;
                    rrDto.ZEK_Kunde_Gesamtengagement = deZek.KUNDEGESAMTENGAGEMENT;
                    rrDto.ZEK_Anz_ZEK_Vertraege = (decimal?)deZek.ANZZEKVERTRAEGE;
                    rrDto.ZEK_Anz_ZEK_Gesuche = (decimal?)deZek.ANZZEKGESUCHE;

                    rrDto.ZEK_Anz_Eng_m_BCode_04_laufend = (decimal?)deZek.ANZBC04LFD;
                    rrDto.ZEK_Anz_Eng_m_BCode_04_saldiert = (decimal?)deZek.ANZBC04SAL;
                    rrDto.ZEK_Anz_Eng_m_BCode_03_laufend = (decimal?)deZek.ANZBC03LFD;
                    rrDto.ZEK_Anz_Eng_m_BCode_03_saldiert = (decimal?)deZek.ANZBC03SAL;
                    rrDto.ZEK_Anz_Ges_m_AblCode_04_07_09_99_BN = (decimal?)deZek.ANZZEKGESMABLCODE04070999E;
                    rrDto.ZEK_Anz_Ges_m_AblCode_04_07_09_99_noBN = (decimal?)deZek.ANZZEKGESMABLCODE04070999F;
                    rrDto.ZEK_Anz_Ges_m_AblCode_13_14_BN = (decimal?)deZek.ANZZEKGESMABLCODE1314E;
                    rrDto.ZEK_Anz_Ges_m_AblCode_13_14_noBN = (decimal?)deZek.ANZZEKGESMABLCODE1314F;
                    rrDto.ZEK_Anz_Ges_m_AblCode_10_BN = (decimal?)deZek.ANZZEKGESMABLCODE10E;
                    rrDto.ZEK_Anz_Ges_m_AblCode_10_noBN = (decimal?)deZek.ANZZEKGESMABLCODE10F;
                    rrDto.ZEK_Anz_KMeld_m_ErCode_21_mit_Positiv = (decimal?)deZek.ANZZEKKMELDMCODE21POS;
                    rrDto.ZEK_Anz_KMeld_m_ErCode_21_ohne_Positiv = (decimal?)deZek.ANZZEKKMELDMCODE21NEG;
                    rrDto.ZEK_Anz_KMeld_m_ErCode_22_mit_Positiv = (decimal?)deZek.ANZZEKKMELDMCODE22POS;
                    rrDto.ZEK_Anz_KMeld_m_ErCode_22_ohne_Positiv = (decimal?)deZek.ANZZEKKMELDMCODE22NEG;
                    //BNR13 TODO EDMX UND DB
                    ExtraZEKFelder extraZekFelder = new ExtraZEKFelder();
                    extraZekFelder = getExtraZekFelder(deZek.SYSDEZEK);
                    rrDto.ZEK_Positiveintraege = extraZekFelder.ANZZEKMELDPOS;
                    rrDto.ZEK_Negativeintraege = extraZekFelder.ANZZEKMELDNEG;
                    //*BNR16
                   rrDto.ZEK_Anz_ZEK_Eng_m_BCode_61 = (decimal?)deZek.ANZZEKENGBCODE61;
                   rrDto.ZEK_Anz_ZEK_Eng_m_BCode_61_l12M = (decimal?)deZek.ANZZEKENGBCODE61L12M;
                   rrDto.ZEK_Anz_ZEK_Eng_m_BCode_61_l24M = (decimal?)deZek.ANZZEKENGBCODE61L24M;
                   rrDto.ZEK_Anz_ZEK_Eng_m_BCode_61_l36M = (decimal?)deZek.ANZZEKENGBCODE61L36M;
                   rrDto.ZEK_Anz_Eng_m_BCode_61_laufend = (decimal?)deZek.ANZZEKENGBCODE61L12M;
                   rrDto.ZEK_Anz_Eng_m_BCode_61_saldiert = (decimal?)deZek.ANZZEKENGBCODE61SLD;
                   rrDto.ZEK_Anz_ZEK_Eng_m_BCode_71 = (decimal?)deZek.ANZZEKENGBCODE71;
                   rrDto.ZEK_Anz_ZEK_Eng_m_BCode_71_l12M = (decimal?)deZek.ANZZEKENGBCODE71L12M;
                   rrDto.ZEK_Anz_ZEK_Eng_m_BCode_71_l24M = (decimal?)deZek.ANZZEKENGBCODE71L24M;
                   rrDto.ZEK_Anz_ZEK_Eng_m_BCode_71_l36M = (decimal?)deZek.ANZZEKENGBCODE71L36M;
                   rrDto.ZEK_Anz_Eng_m_BCode_71_laufend = (decimal?)deZek.ANZZEKENGBCODE71LFD;
                   rrDto.ZEK_Anz_Eng_m_BCode_71_saldiert = (decimal?)deZek.ANZZEKENGBCODE71SLD;


                }
                // DEARBEIT
                if (deArbeit != null)
                {
                    rrDto.DV_AG_Decision_Maker = (decimal?)deArbeit.DECISIONMAKER;
                    rrDto.DV_AG_Firmenstatus = (decimal?)deArbeit.FIRMENSTATUS;
                    rrDto.DV_AG_Rechtsform = (decimal?)deArbeit.RECHTSFORM;
                    rrDto.DV_AG_Status = (decimal?)deArbeit.STATUS;
                    rrDto.DV_AG_Datum = deArbeit.DATUM;
                    rrDto.DV_AG_Gruendungsdatum = deArbeit.GRUENDUNGSDATUM;
                    rrDto.DV_AG_NOGA_Code = deArbeit.NOGACODE;
                    rrDto.DV_AG_Zeit = deArbeit.ZEIT;
                    rrDto.DV_AG_UID = deArbeit.UIDNUMMER;
                    rrDto.DV_AG_Kapital = deArbeit.KAPITAL;
                    
                    rrDto.DV_AG_ADDRESS_ID = getDEARBEITKUNDENID(deArbeit.SYSDEARBEIT);



                }

                // DEADRBONI
                if (deAdrBon != null)
                {
                    
                    rrDto.Netzwerkbeziehung = deAdrBon.NETZWERKBEZIEHUNG;
                    rrDto.Fluktuationsrate = deAdrBon.FLUKTUATIONSRATE;
                    rrDto.Fraudmngt = deAdrBon.FRAUDMNGT;

                    rrDto.DV_ADDRESS_ID = deAdrBon.KUNDENID.ToString();

                    rrDto.DV_Anz_BPM = (decimal?)deAdrBon.ANZBM;
                    rrDto.DV_Anz_BPM_l12m = (decimal?)deAdrBon.ANZBPM12M;
                    rrDto.DV_Anz_BPM_l24m = (decimal?)deAdrBon.ANZBPM24M;
                    rrDto.DV_Anz_BPM_l36m = (decimal?)deAdrBon.ANZBMP36M;
                    rrDto.DV_Anz_BPM_l48m = (decimal?)deAdrBon.ANZBMP48M;
                    rrDto.DV_Anz_BPM_l60m = (decimal?)deAdrBon.ANZBMP60M;

                    rrDto.DV_Anz_BPM_m_FStat_01 = (decimal?)deAdrBon.ANZBPMMFSTAT01;
                    rrDto.DV_Anz_BPM_m_FStat_02 = (decimal?)deAdrBon.ANZBPMMFSTAT02;
                    rrDto.DV_Anz_BPM_m_FStat_03 = (decimal?)deAdrBon.ANZBPMMFSTAT03;
                    rrDto.DV_Anz_BPM_m_FStat_04 = (decimal?)deAdrBon.ANZBPMMFSTAT04;

                    rrDto.DV_Anz_BPM_m_FStat_01_l12m = (decimal?)deAdrBon.ANZBPMMFSTAT0112M;
                    rrDto.DV_Anz_BPM_m_FStat_01_l24m = (decimal?)deAdrBon.ANZBPMMFSTAT0124M;
                    rrDto.DV_Anz_BPM_m_FStat_01_l36m = (decimal?)deAdrBon.ANZBPMMFSTAT0136M;
                    rrDto.DV_Anz_BPM_m_FStat_01_l48m = (decimal?)deAdrBon.ANZBPMMFSTAT0148M;
                    rrDto.DV_Anz_BPM_m_FStat_01_l60m = (decimal?)deAdrBon.ANZBPMMFSTAT0160M;

                    rrDto.DV_Anz_BPM_m_FStat_02_l12m = (decimal?)deAdrBon.ANZBPMMFSTAT0212M;
                    rrDto.DV_Anz_BPM_m_FStat_02_l24m = (decimal?)deAdrBon.ANZBPMMFSTAT0224M;
                    rrDto.DV_Anz_BPM_m_FStat_02_l36m = (decimal?)deAdrBon.ANZBPMMFSTAT0236M;
                    rrDto.DV_Anz_BPM_m_FStat_02_l48m = (decimal?)deAdrBon.ANZBPMMFSTAT0248M;
                    rrDto.DV_Anz_BPM_m_FStat_02_l60m = (decimal?)deAdrBon.ANZBPMMFSTAT0260M;

                    rrDto.DV_Anz_BPM_m_FStat_03_l12m = (decimal?)deAdrBon.ANZBPMMFSTAT0312M;
                    rrDto.DV_Anz_BPM_m_FStat_03_l24m = (decimal?)deAdrBon.ANZBPMMFSTAT0324M;
                    rrDto.DV_Anz_BPM_m_FStat_03_l36m = (decimal?)deAdrBon.ANZBPMMFSTAT0336M;
                    rrDto.DV_Anz_BPM_m_FStat_03_l48m = (decimal?)deAdrBon.ANZBPMMFSTAT0348M;
                    rrDto.DV_Anz_BPM_m_FStat_03_l60m = (decimal?)deAdrBon.ANZBPMMFSTAT0360M;

                    rrDto.DV_Anz_BPM_m_FStat_04_l12m = (decimal?)deAdrBon.ANZBPMMFSTAT0412M;
                    rrDto.DV_Anz_BPM_m_FStat_04_l24m = (decimal?)deAdrBon.ANZBPMMFSTAT0424M;
                    rrDto.DV_Anz_BPM_m_FStat_04_l36m = (decimal?)deAdrBon.ANZBPMMFSTAT0436M;
                    rrDto.DV_Anz_BPM_m_FStat_04_l48m = (decimal?)deAdrBon.ANZBPMMFSTAT0448M;
                    rrDto.DV_Anz_BPM_m_FStat_04_l60m = (decimal?)deAdrBon.ANZBPMMFSTAT0460M;

                    rrDto.DV_Firmenstatus = (decimal?)deAdrBon.FIRMENSTATUS;
                    rrDto.DV_Rechtform = (decimal?)deAdrBon.RECHTSFORM;

                    rrDto.DV_Schlechtester_FStat = (decimal?)deAdrBon.SCHLECHTESTERFSTAT;
                    rrDto.DV_Schlechtester_FStat_l12m = (decimal?)deAdrBon.SCHLECHTESTERFSTAT12M;
                    rrDto.DV_Schlechtester_FStat_l24m = (decimal?)deAdrBon.SCHLECHTESTERFSTAT24M;
                    rrDto.DV_Schlechtester_FStat_l36m = (decimal?)deAdrBon.SCHLECHTESTERFSTAT36M;
                    rrDto.DV_Schlechtester_FStat_l48m = (decimal?)deAdrBon.SCHLECHTESTERFSTAT48M;
                    rrDto.DV_Schlechtester_FStat_l60m = (decimal?)deAdrBon.SCHLECHTESTERFSTAT60M;

                    rrDto.DV_Status_Auskunft_Adressvalidierung = (decimal?)deAdrBon.STATUSAUSKUNFTADRESSVALID;
                    rrDto.DV_Anz_DV_Treffer_Adressvalidierung = (decimal?)deAdrBon.ANZDVTREFFERADRESSVALIDIERUNG;
                    rrDto.DV_Datum_an_der_aktuellen_Adresse_seit = deAdrBon.DATUMAKTUELLEADRESSESEIT;
                    rrDto.DV_Datum_der_Auskunft = deAdrBon.DATUMDERAUSKUNFT;
                    rrDto.DV_Datum_der_ersten_Meld = deAdrBon.DATUMERSTEMELDUNG;
                    rrDto.DV_Fraud_Feld = deAdrBon.FRAUDFELD;
                    rrDto.DV_Geburtsdatum = deAdrBon.GEBURTSDATUM;
                    rrDto.DV_Gruendungsdatum = deAdrBon.GRUENDUNGSDATUM;
                    rrDto.DV_Kapital = deAdrBon.KAPITAL;
                    rrDto.DV_Land = deAdrBon.LAND;
                    rrDto.DV_NOGA_Code_Branche = deAdrBon.NOGACODEBRANCHE;
                    rrDto.DV_PLZ = deAdrBon.PLZ;
                    rrDto.DV_Zeit_der_Auskunft = deAdrBon.ZEITDERAUSKUNFT;

                    rrDto.DV_UID = deAdrBon.UIDNUMMER;
                    rrDto.DV_Anz_DecisionMaker = deAdrBon.ANZDECISIONMAKER;

                    rrDto.DV_Datum_juengster_Eintrag = deAdrBon.DATUMJUENGSTEREINTRAG;
                    rrDto.DV_Kritischer_Glaeubiger = deAdrBon.KRITISCHERGLAEUBIGER;
                    rrDto.DV_Summe_offener_Betreibungen = deAdrBon.SUMOFFENERBETREIBUNGEN;
                    rrDto.DV_Anzahl_offene_Betreibungen = deAdrBon.ANZOFFENERBETREIBUNGEN;
                    rrDto.DV_Datum_juengste_Betreibung = deAdrBon.DATUMJUENGSTEBETREIBUNG;
                    rrDto.DV_Organisation_belastet = (decimal?)deAdrBon.DECVALUEORG;
                    rrDto.DV_Score = deAdrBon.DECVALUECCB;
                    rrDto.DV_Payment_Delay = deAdrBon.PAYMENTDELAYRATIO;
                    rrDto.DV_First_SHAB_Date = deAdrBon.FIRSTSHABDATE;
                    rrDto.DV_Risk_Alert = deAdrBon.DEVVALUERISK;




                    rrDto.DV_Uid_SameAsName = deAdrBon.UIDSAMEASNAME;

                    rrDto.DV_Anz_BM_m_FStat_00 = (decimal?)deAdrBon.ANZBPMMFSTAT00;
                }

                // DEAPPLICANT
                if (deApp != null)
                {
                    rrDto.A_Anz_der_Betreibungen = (decimal?)deApp.ANZAHLBETREIBUNGEN;
                    rrDto.A_Anz_Kinder_ueber_10_bis_12 = (decimal?)deApp.ANZKIND11BIS12;
                    rrDto.A_Anz_Kinder_bis_6 = (decimal?)deApp.ANZKINDBIS6;
                    rrDto.A_Anz_Kinder_ueber_6_bis_10 = (decimal?)deApp.ANZKINDER7BIS10;
                    rrDto.A_Anz_unterstuetzungsp_Kinder_ab_12 = (decimal?)deApp.ANZKINDERAB13;
                    rrDto.A_Anz_Mitarbeiter = (decimal?)deApp.ANZMITARBEITER;
                    rrDto.A_13_Montaslohn = (decimal?)deApp.FLAG13MONATSLOHN;
                    rrDto.A_marbeiter_Credit_Suisse_Group = (decimal?)deApp.FLAGMITARBEITERCS;
                    rrDto.A_In_Handelsregister_eingetragen = (decimal?)deApp.INHANDELSREGISTEREINGETRAGEN;
                    rrDto.A_KundenID = (decimal?)deApp.KUNDENID;
                    rrDto.A_Quellensteuerpflichtig = (decimal?)deApp.QUELLENSTEUERPFLICHTIG;
                    rrDto.A_Revisionsstelle_vorhanden = (decimal?)deApp.REVISIONSSTELLEVORHANDEN;
                    rrDto.A_Verlustscheine_Pfaendungen = (decimal?)deApp.VERLUSTSCHEINEPFAENDUNGEN;
                    rrDto.A_A_PID = deApp.APID != null ? deApp.APID.ToUpper() : deApp.APID;
                    rrDto.A_Arbeitgeber_beschaeftigt_bis = deApp.ARBEITGEBERBESCHAEFTIGTBIS;
                    rrDto.A_Arbeitgeber_seit_wann = deApp.ARBEITGEBERSEITWANN;
                    rrDto.A_Auslaenderausweis_Einreisedatum = deApp.AUSLAENDERAUSWEISEINREISEDATUM;
                    rrDto.A_Auslaenderausweis_Gueltigkeitsdatum = deApp.AUSLAENDERAUSWEISGUELTIGKEIT;
                    rrDto.A_Berufsauslagen_Betrag = deApp.BERUFSAUSLAGENBETRAG;
                    rrDto.A_Bestehende_Kreditrate = deApp.BESTEHENDEKREDITRATE;
                    rrDto.A_Bestehende_Leasingrate = deApp.BESTEHENDELEASINGRATE;
                    rrDto.A_Bilanzsumme = deApp.BILANZSUMME;
                    rrDto.A_Datum_letzter_Jahresabschluss = deApp.DATUMLETZTERJAHRESABSCHLUSS;
                    rrDto.A_Eigenkapital = deApp.EIGENKAPITAL;
                    rrDto.A_E_Mail = deApp.EMAIL;
                    rrDto.A_fluessige_mtel = deApp.FLUESSIGEMITTEL;
                    rrDto.A_Geburtsdatum = deApp.GEBURTSDATUM;
                    rrDto.A_Haupteinkommen_Betrag = deApp.HAUPTEINKOMMENBETRAG;
                    rrDto.A_hier_Wohnhaft_seit = deApp.HIWERWOHNHAFTSEIT;
                    rrDto.A_Hoehe_der_Betreibungen = deApp.HOEHEBETREIBUNGEN;
                    rrDto.A_Instradierung = deApp.INSTRADIERUNG != null ? deApp.INSTRADIERUNG.ToUpper() : deApp.INSTRADIERUNG;
                    rrDto.A_Jaehl_Gratifikation_Bonus = deApp.JAEHRLGRATIFIKATIONBONUS;
                    rrDto.A_Jahregewinn = deApp.JAHRESGEWINN;
                    rrDto.A_Jahresumsatz = deApp.JAHRESUMSATZ;
                    rrDto.A_Kanton = deApp.KANTON;
                    rrDto.A_Kurzfristige_Verbindlichkeiten = deApp.KURZFRISTIGEVERBINDLICHKEITEN;
                    rrDto.A_Land = deApp.LAND;
                    rrDto.A_Mobiltelefon = deApp.MOBILTELEFON;
                    rrDto.A_Nationalitaet = deApp.NATIONALITAET;
                    rrDto.A_Nebeneinkommen_Betrag = deApp.NEBENEINKOMMENBETRAG;
                    rrDto.A_PLZ = deApp.PLZ;
                    rrDto.A_Regelmaessige_Auslagen_Betrag = deApp.REGELMAESSIGEAUSLAGENBETRAG;
                    rrDto.A_Telefon_1 = deApp.TELEFON1;
                    rrDto.A_Telefon_2 = deApp.TELEFON2;
                    rrDto.A_Telefon_geschaeftlich = deApp.TELEFONGESCHAEFTLICH;
                    rrDto.A_Telefon_privat = deApp.TELEFONPRIVAT;
                    rrDto.A_Unterstuetzungsbeitraege_Betrag = deApp.UNTERSTUETZUNGSBEITRAEGEBETRAG;
                    rrDto.A_Wohnkosten_Miete = deApp.WOHNKOSTENMIETE;
                    rrDto.A_Zusatzeinkommen_Betrag = deApp.ZUSATZEINKOMMENBETRAG;
                    rrDto.A_Sprache = deApp.SPRACHE;
                    rrDto.A_Status = deApp.STATUS;
                    rrDto.A_Unterstuetzungsbeitraege = deApp.UNTERSTUETZUNGSBEITRAEGE;
                    rrDto.A_Wohnverhaeltnis = deApp.WOHNVERHAELTNIS;
                    rrDto.A_Zivilstand = deApp.ZIVILSTAND;
                    rrDto.A_Zusatzeinkommen = deApp.ZUSATZEINKOMMEN;
                    rrDto.A_Rueckzahlungsart = deApp.RUECKZAHLUNGSART;
                    rrDto.A_Regelmaessige_Auslagen = deApp.REGELMAESSIGEAUSLAGEN;
                    rrDto.A_Kundenart = deApp.KUNDENART;
                    rrDto.A_CS_Einheit = deApp.CSEINHEIT;
                    rrDto.A_Berufliche_Situation = deApp.BERUFLICHESITUATION;
                    rrDto.A_Auszahlungsart = deApp.AUSZAHLUNGSART;
                    rrDto.A_Auslaenderausweis = deApp.AUSLAENDERAUSWEIS;
                    rrDto.A_Einkommen_Art = deApp.EINKOMMENART;
                    rrDto.A_Berufsauslagen = deApp.BERUFSAUSLAGEN;
                    rrDto.A_MO_Counter = (decimal?)deApp.MOCOUNTER;
                    rrDto.A_EhePartnerFlag = (int?)deApp.EHEPARTNERFLAG;
                    rrDto.A_Weitere_Verpflichtungen_Betrag = (long?)deApp.WEITEREVERPFLICHTUNGENBETRAG;
                    rrDto.A_Weitere_Verpflichtungen = deApp.WEITEREVERPFLICHTUNGEN;
                    rrDto.A_UID = deApp.UIDNUMMER;
                    rrDto.A_AG_NAME = deApp.NAMEAG != null ? deApp.NAMEAG.ToUpper() : deApp.NAMEAG;
                    rrDto.A_Nebeneinkommen_seit_wann = deApp.NEBENEINKOMMENSEIT;
                    rrDto.A_Arbeitgeber_Beschaeftigt_bis2 = deApp.ARBEITGEBERBESCHAEFTIGTBIS2;
                    rrDto.A_Berufliche_Situation2 = deApp.BERUFLICHESITUATION2;
                    rrDto.A_Geschlecht = deApp.GESCHLECHT != null ? (decimal)deApp.GESCHLECHT : 0;
                    //BRNEUN
                    rrDto.A_AG_PLZ1 = deApp.PLZAG1;
                    rrDto.A_AG_PLZ2 = deApp.PLZAG2;
                    //BNR10
                    rrDto.A_AG_NAME2 = deApp.NAMEAG2;
                    rrDto.A_Arbeitgeber_seit_wann2 = deApp.ARBEITGEBERSEITWANN2;
                    //BNR11 
                    rrDto.A_ZULAGEKIND = deApp.ZULAGEKIND;
                    rrDto.A_ZULAGEAUSBILDUNG = deApp.ZULAGEAUSBILDUNG;
                    rrDto.A_ZULAGESONST = deApp.ZULAGESONST;

                }
                inDto.RecordRRDto[loop] = rrDto;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public string[] MygetDEFraudValues(string values)
        {
            if (values == null || values.Trim() == "")
                return null;
            string[] i_f_values = new string[101];


            XDocument doc = XDocument.Parse(values);
            var xelements = doc.Descendants().ToList();

            for (var i = 0; i < 101; i++)
                i_f_values[i] = xelements[i].Value;

            return i_f_values;

        }

        /// <summary>
        /// MyAddDERegel
        /// </summary>
        /// <param name="deDetail"></param>
        /// <param name="context"></param>
        /// <param name="area"></param>
        /// <param name="codeArray"></param>
        private void MyAddDERegel(DEDETAIL deDetail, DdDeExtended context, string area, string[] codeArray)
        {
            if (codeArray.Length > 0)
            {
                string[] distinctCodes = codeArray.Distinct().ToArray();
                foreach (string code in distinctCodes)
                {
                    // DB4250
                    // DEREGEL deRegel = new DEREGEL();
                    // deRegel.AREA = area;
                    // deRegel.CODE = code;
                    // deRegel.DEDETAIL = deDetail;
                    // context.DEREGEL.Add(deRegel);

                    var dedefrulQuery = from Dedefrul in context.DEDEFRUL
                                        where Dedefrul.EXTERNCODE == code
                                        select Dedefrul;


                    DEDEFRUL dedefrul = dedefrulQuery.FirstOrDefault();

                    if (dedefrul != null)
                    {
                        DERUL derul = new DERUL();
                        derul.DEDETAIL = deDetail;
                        derul.DEDEFRUL = dedefrul;
                        context.DERUL.Add(derul);
                    }
                    else
                    {
                        throw new ApplicationException("Exception Decision Engine AddDERegel, for code=" + code + " definition is not found in DEDEFRUL");
                    }
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        /// MyAddDECon
        /// </summary>
        /// <param name="deDetail"></param>
        /// <param name="context"></param>
        /// <param name="area"></param>
        /// <param name="codeArray"></param>
        private void MyAddDECon(DEDETAIL deDetail, DdDeExtended context, string area, string[] codeArray)
        {
            if (codeArray.Length > 0)
            {
                string[] distinctCodes = codeArray.Distinct().ToArray();
                foreach (string code in distinctCodes)
                {
                    var dedefconQuery = from Dedefcon in context.DEDEFCON
                                        where Dedefcon.EXTERNCODE == code
                                        select Dedefcon;

                    DEDEFCON dedefcon = dedefconQuery.FirstOrDefault();
                    if (dedefcon != null)
                    {
                        DECON decon = new DECON();
                        decon.DEDETAIL = deDetail;
                        decon.DEDEFCON = dedefcon;
                        context.DECON.Add(decon);
                    }
                    else
                    {
                        throw new ApplicationException("Exception in Decision Engine AddDECon, for code=" + code + " definition is not found in DEDEFCON ");
                    }
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        /// MyAddDERegelCon
        /// </summary>
        /// <param name="deDetail"></param>
        /// <param name="context"></param>
        private void MyAddDERegelCon(DEDETAIL deDetail, DdDeExtended context)
        {
            var deconQuery = from Decon in context.DECON
                             where Decon.DEDETAIL.SYSDEDETAIL == deDetail.SYSDEDETAIL
                             select Decon;
            List<DECON> deconList = deconQuery.ToList<DECON>();

            var derulQuery = from Derul in context.DERUL
                             where Derul.DEDETAIL.SYSDEDETAIL == deDetail.SYSDEDETAIL
                             select Derul;
            List<DERUL> derulList = derulQuery.ToList<DERUL>();

            if (deconList != null && derulList != null)
            {
                foreach (DECON decon in deconList)
                {
                    foreach (DERUL derul in derulList)
                    {
                        var dedefrulconQuery = from Dedefrulcon in context.DEDEFRULCON
                                               where Dedefrulcon.DEDEFRUL.SYSDEDEFRUL == derul.DEDEFRUL.SYSDEDEFRUL && Dedefrulcon.DEDEFCON.SYSDEDEFCON == decon.DEDEFCON.SYSDEDEFCON
                                               select Dedefrulcon;

                        if (dedefrulconQuery.FirstOrDefault() != null)
                        {
                            DERULCON derulcon = new DERULCON();
                            derulcon.DERUL = derul;
                            derulcon.DECON = decon;
                            context.DERULCON.Add(derulcon);
                        }
                    }
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        /// MyAddDEScore
        /// </summary>
        /// <param name="deDetail"></param>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <param name="berVar"></param>
        /// <param name="bez"></param>
        /// <param name="eingWert"></param>
        /// <param name="resWert"></param>
        private void MyAddDEScore(DEDETAIL deDetail, DdDeExtended context, decimal? id, string berVar, string bez, string eingWert, int? resWert)
        {
            if (id != null && id != 0)
            {
                DESCORE deScore = new DESCORE();
                deScore.RANK = (int?)id;
                deScore.BERECHNUNGSVARIABLEN = berVar;
                deScore.BEZEICHNUNG = bez;
                deScore.EINGABEWERT = eingWert;
                deScore.RESULTATWERT = resWert;
                deScore.DEDETAIL = deDetail;
                context.DESCORE.Add(deScore);
            }
        }

        /// <summary>
        /// MyUpdateDecisionEngineOutObjects
        /// </summary>
        /// <param name="deEnvOut"></param>
        /// <param name="deDecision"></param>
        /// <param name="deSonst"></param>
        /// <param name="outDto"></param>
        private void MyUpdateDecisionEngineOutObjects(DEENVOUT deEnvOut, DEDECISION deDecision, DESONST deSonst, DecisionEngineOutDto outDto)
        {
            deEnvOut.INQUIRYCODE = outDto.InquiryCode;
            deEnvOut.INQUIRYDATE = outDto.InquiryDate;
            deEnvOut.INQUIRYTIME = outDto.InquiryTime;
            deEnvOut.ORGANIZATIONCODE = outDto.OrganizationCode;
            deEnvOut.PROCESSCODE = outDto.ProcessCode;
            deEnvOut.PROCESSVERSION = outDto.ProcessVersion;
            deEnvOut.SYSTEMDECISION = outDto.System_Decision;
            deEnvOut.SYSTEMDECISIONGROUP = outDto.System_Decision_Group;

            deDecision.AMPEL = outDto.DEC_Ampel;
            deDecision.PROZESSSCHRITTID = (int?)outDto.DEC_ProzessschrittID;
            deDecision.REGELWERKCODE = outDto.DEC_Regelwerk_Code;
            deDecision.STATUS = outDto.DEC_Status;
            deDecision.STATUSID = (int?)outDto.DEC_StatusID;


            deSonst.DATE01 = outDto.ReserveDate1;
            deSonst.DATE02 = outDto.ReserveDate2;
            deSonst.DATE03 = outDto.ReserveDate3;
            deSonst.FEHLERCODE = (int?)outDto.Fehlercode;
            deSonst.NUMBER01 = outDto.ReserveNumber1;
            deSonst.NUMBER02 = outDto.ReserveNumber2;
            deSonst.NUMBER03 = outDto.ReserveNumber3;
            deSonst.RANDOMNUMBER = (int?)outDto.RandomNumber;
            deSonst.TEXT01 = outDto.ReserveText1;
            deSonst.TEXT02 = outDto.ReserveText2;
            deSonst.TEXT03 = outDto.ReserveText3;
        }

        private ExtraSalesFelder getExtraSalesFelder(long sysDESALES)
        {
            try
            {
                using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended context = new Cic.OpenOne.Common.Model.DdOw.DdOwExtended())
                {
                    String EXTRASALESQUERY = "select " +
                                               " SYSVARTABL," +
                                               " VTDAUERABLTOTAL," +
                                               " ANZABLVTPERVART," +
                                               " VTDAUERABLPERVART," +
                                               " VTDAUERABL " +
                                               " from DESALES where sysdesales = :psysdesales ";

                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysdesales", Value = sysDESALES });

                    ExtraSalesFelder extra = context.ExecuteStoreQuery<ExtraSalesFelder>(EXTRASALESQUERY, parameters.ToArray()).FirstOrDefault();

                    return extra;

                }
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim suche von getroffen Regeln. Error Message: " + ex.Message);
                throw ex;
            }

        }


        private ExtraDOLFelder getExtraDOLFelder(long sysDEOPENLEASE)
        {
            try
            {
                using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended context = new Cic.OpenOne.Common.Model.DdOw.DdOwExtended())
                {
                    String EXTRADOLQUERY = " select " +
                                           " DATUMERSTERANTRAG , " +
                                           " DATUMLETZTERANTRAG , " +
                                           " ANZMAHN1AVG6M , " +
                                           " ANZMAHN2AVG6M , " +
                                           " ANZMAHN3AVG6M , " +
                                           " ANZZAHLAVG12M , " +
                                           " RUECKSTANDAVG , " +
                                           " BUCHSALDOAVG  " +
                                           " from DEOPENLEASE where sysdeopenlease = :psysopenlease ";

                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysopenlease", Value = sysDEOPENLEASE });

                    ExtraDOLFelder extra = context.ExecuteStoreQuery<ExtraDOLFelder>(EXTRADOLQUERY, parameters.ToArray()).FirstOrDefault();

                    return extra;

                }
            }
            catch (Exception ex)
            {
                _log.Error("Fehler beim suche von getroffen Regeln. Error Message: " + ex.Message);
                throw ex;
            }

        }

        private decimal getRWGA(long sysDECONTRACT)
        {
            using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended context = new Cic.OpenOne.Common.Model.DdOw.DdOwExtended())
            {
                String RWGAQUERY = " select  SYSRWGA from DECONTRACT where sysdecontract = :psysdecontract ";

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysdecontract", Value = sysDECONTRACT });

                decimal extra = context.ExecuteStoreQuery<decimal>(RWGAQUERY, parameters.ToArray()).FirstOrDefault();

                return extra;

            }

        }

        public ExtraZEKFelder getExtraZekFelder(long sysDEZEK)
        {
            using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended context = new Cic.OpenOne.Common.Model.DdOw.DdOwExtended())
            {
                String ExtraZEKQUERY = "select ANZZEKMELDPOS, ANZZEKMELDNEG  from DEZEK where sysdezek = :psysdezek ";

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysdezek", Value = sysDEZEK });

                ExtraZEKFelder extra = context.ExecuteStoreQuery<ExtraZEKFelder>(ExtraZEKQUERY, parameters.ToArray()).FirstOrDefault();

                return extra;

            }

        }

        public string getDEARBEITKUNDENID(long sysDEARBEIT)
        {
            using (DdDeExtended context = new DdDeExtended())
            {
                String ExtraDEARBEITQUERY = "select kundenid  from DEARBEIT where sysdearbeit= :psysdearbeit ";

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysdearbeit", Value = sysDEARBEIT });

                decimal? extra = context.ExecuteStoreQuery<decimal?>(ExtraDEARBEITQUERY, parameters.ToArray()).FirstOrDefault();
                parameters.Clear();
                string kundenid = "";
                if (extra != null && extra != 0)
                    kundenid = extra.ToString();

                return kundenid;

            }

        }


        public void savedeArbeitInput(RecordRRDto[] recordRRDto, DEINPEXEC deInputExec)
        {

            foreach (RecordRRDto RRDto in recordRRDto)
            {
                using (DdDeExtended context = new DdDeExtended())
                {
                    DEARBEIT deArbeit = Mapper.Map<RecordRRDto, DEARBEIT>(RRDto);
                    deArbeit.DEINPEXEC = deInputExec;
                    context.DEARBEIT.Add(deArbeit);
                    context.SaveChanges();
                    decimal? kundenid = Convert.ToInt64(RRDto.DV_AG_ADDRESS_ID);
                    string UpdateDEARBEIT = "update DEARBEIT SET KUNDENID = :pKUNDENID where DEARBEIT.SYSDEARBEIT =:pSYSDEARBEIT";
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pKUNDENID", Value = kundenid });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pSYSDEARBEIT", Value = deArbeit.SYSDEARBEIT });
                    context.ExecuteStoreCommand(UpdateDEARBEIT, parameters.ToArray());
                    parameters.Clear();
                }

            }
        }

        #endregion Private Methods

    }
}