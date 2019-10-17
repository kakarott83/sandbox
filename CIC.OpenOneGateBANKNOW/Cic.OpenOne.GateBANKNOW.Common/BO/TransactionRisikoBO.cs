using AutoMapper;
using AutoMapper.Mappers;
using Cic.One.Util.IO;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Serialization;
using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{


    /// <summary>
    /// Flags für Transaction Risiko
    /// </summary>
    public class Flags4TR
    {
        public int? fkp { get; set; }
        public int? pkp { get; set; }
        public int? umsp { get; set; }
        public int? bwgp { get; set; }
        public int? strap { get; set; }
        public int? fel1r { get; set; }
        public bool ums { get; set; }
        public bool stra { get; set; }
        public bool bwg { get; set; }
    }

    /// <summary>
    /// Transaction RisikoBO
    /// </summary>
    public class TransactionRisikoBO : AbstractTransactionRisikoBo
    {
        private const long codeTechExc = -2;
        private const long codeSerAufExc = -1;
        public static CacheDictionary<String, List<EurotaxOutDto>> FORECASTCache = CacheFactory<String, List<EurotaxOutDto>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);


        /// <summary>
        /// KDTYPID_PRIVAT
        /// </summary>
        public const int KDTYPID_PRIVAT = 1;
        private const String EL_BETRAG_MAX = "EL_BETRAG_MAX";
        private const String EL_BETRAG_MIN = "EL_BETRAG_MIN";
        private const String EL_PROZENT_MAX = "EL_PROZENT_MAX";
        private const String EL_PROZENT_MIN = "EL_PROZENT_MIN";
        private const String PROF_MAX = "PROF_MAX";
        private const String PROF_MIN = "PROF_MIN";



        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// TransactionRisikoBO
        /// </summary>
        /// <param name="angAntBo"></param>
        /// <param name="angAntDao"></param>
        /// <param name="vgDao"></param>
        /// <param name="eurotaxDBDao"></param>
        /// <param name="eurotaxBo"></param>
        /// <param name="mwStDao"></param>
        /// <param name="translateBo"></param>
        /// <param name="trDao"></param>
        public TransactionRisikoBO(IAngAntDao angAntDao, IVGDao vgDao, IEurotaxDBDao eurotaxDBDao, IEurotaxBo eurotaxBo, IMwStDao mwStDao, ITranslateBo translateBo, TransactionRisikoDao trDao, IEaihotDao eaihotDao)
            : base(angAntDao, vgDao, eurotaxDBDao, eurotaxBo, mwStDao, translateBo, trDao, eaihotDao)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <param name="laufzeit"></param>
        /// <param name="neupreis"></param>
        /// <param name="neupreisDefault"></param>
        /// <param name="kmStand"></param>
        /// <param name="zubehoer"></param>
        /// <param name="erstzulassung"></param>
        /// <param name="schwacke"></param>
        /// <param name="jahresKm"></param>
        /// <returns></returns>
        private List<EurotaxOutDto> getEurotaxOutList(long sysobtyp, int laufzeit, double neupreis, double neupreisDefault, double neupreisIW, double neupreisVGREF, double kmStand, double zubehoer, DateTime? erstzulassung, string schwacke, long jahresKm)
        {
            String erstzulassungDate = "";
            if (erstzulassung != null)
            {
                erstzulassungDate = String.Format("{0:MMddyyyy}", erstzulassung);
            }
            //BNRSIZE - 1145  LP wird neu immer aus der gleichen Quelle kommen wie die Restwertverläufe.
            string key = sysobtyp.ToString() + "_" + laufzeit.ToString() + "_" + neupreis.ToString()+"_"+neupreisDefault.ToString() + "_" + neupreisIW.ToString() + "_" + kmStand.ToString() + "_" + jahresKm + "_" + zubehoer + "_" + erstzulassungDate;
            
            double entryAge = FORECASTCache.getAge(key);
            if (entryAge > 1000 * 60 * 30)//Flush Data older 30Min
            {
                FORECASTCache.Remove(key);
            }

            if (!FORECASTCache.ContainsKey(key))
            {
                   FORECASTCache[key] =   eurotaxBo.getEurotaxOutList(sysobtyp, laufzeit, neupreis, neupreisDefault, neupreisIW, neupreisVGREF,kmStand, zubehoer, erstzulassung, schwacke, jahresKm);
            }

            return FORECASTCache[key];

           
        }



        /// <summary>
        /// processFinVorEinreichung
        /// </summary>
        /// <param name="input">input</param>
        /// <returns></returns>
        public override ofinVorEinreichungDto processFinVorEinreichung(ifinVorEinreichungDto input)
        {
            if (input.antrag == null)
            {
                throw new Exception("Parameter antrag was empty");
            }

            if (input.finanzierungsVariante == 0 && input.antrag.sysprchannel != 2)//nur FF
            {
                try
                {
                    EaihotDto eaiOutput = new EaihotDto();
                    eaiOutput = new EaihotDto()
                    {
                        CODE = "FINVORSCHLAG_B2B_RELEASE",
                        OLTABLE = "ANTRAG",
                        SYSOLTABLE = input.antrag.sysid,
                        SUBMITDATE = DateTimeHelper.DateTimeToClarionDate(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                        SUBMITTIME = DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                        EVE = 1,
                        PROZESSSTATUS = (int)EaiHotStatusConstants.Pending,
                        HOSTCOMPUTER = "*",
                        INPUTPARAMETER1 = input.antrag.sysid.ToString()

                    };
                    eaiOutput = eaihotDao.createEaihot(eaiOutput);

                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException("Could not AUTOCHECK. EAIHOT", ex);
                }



                if (input.antrag == null)
                {
                    throw new Exception("Parameter antrag was empty");
                }

                if (input.antrag.kalkulation == null)
                {
                    throw new Exception("Parameter kalkulation was empty");
                }

                return null;


            }



            if (input.antrag.kalkulation == null)
            {
                throw new Exception("Parameter kalkulation was empty");
            }

            //Werte von kalkulation aktualisieren 
            input.antrag.kalkulation.angAntKalkDto.syskalk = input.antrag.sysid;
            trDao.updateKalkMitVariantenDaten(input.antrag.kalkulation.angAntKalkDto);



            //BRNEUN CR45
            try
            {
                String code = "PRODUKTPRÜFUNG";
                String par2 = null;
                if(input.antrag.sysprchannel==2)//Für Kredit Code START
                {
                    code = "START";
                    par2= "PRODUKTPRÜFUNG";
                }
                EaihotDto eaiOutput = new EaihotDto();
                eaiOutput = new EaihotDto()
                {
                    CODE = "AUTOCHECK",
                    OLTABLE = "ANTRAG",
                    SYSOLTABLE = input.antrag.sysid,
                    SUBMITDATE = DateTimeHelper.DateTimeToClarionDate(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                    SUBMITTIME = DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                    EVE = 1,
                    PROZESSSTATUS = (int)EaiHotStatusConstants.Pending,
                    HOSTCOMPUTER = "*",
                    INPUTPARAMETER1 = code,
                    INPUTPARAMETER2 = par2

                };
                eaiOutput = eaihotDao.createEaihot(eaiOutput);

            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Could not AUTOCHECK. EAIHOT", ex);
            }



            if (input.antrag == null)
            {
                throw new Exception("Parameter antrag was empty");
            }

            if (input.antrag.kalkulation == null)
            {
                throw new Exception("Parameter kalkulation was empty");
            }

            return null;
        }


        /// <summary>
        /// checkTrRiskBySysid
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="sysPEROLE"></param>
        /// <param name="isoCode"></param>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        public override ocheckTrRiskDto checkTrRiskBySysid(long sysid, long sysPEROLE, string isoCode, long syswfuser)
        {
            ocheckTrRiskDto output = new ocheckTrRiskDto();
            AntragDto inputantrag = angAntDao.getAntrag(sysid, sysPEROLE);
            icheckTrRiskDto input = new icheckTrRiskDto();
            input.antrag = inputantrag;
            output = checkTrRisk(input, sysPEROLE, isoCode, true,syswfuser);

            return output;

        }



        /// <summary>
        /// checkTrRisk
        /// </summary>
        /// <param name="input">input</param>
        /// <param name="sysPEROLE">sysPEROLE</param>
        /// <param name="isoCode">isoCode</param>
        /// <param name="kalkVariante">kalkVariante  == false wenn B2B Variante3 prüft</param>
        /// <returns></returns>
        public override ocheckTrRiskDto checkTrRisk(icheckTrRiskDto input, long sysPEROLE, string isoCode, bool kalkVariante, long syswfuser)
        {
            // Kalkulation Kontext
            prKontextDto prodKontext = new prKontextDto();
            kalkKontext kkontext = new kalkKontext();
            prodKontext = MyCreateProductKontext(sysPEROLE, input.antrag);
            kkontext = MyCreateKalkKontext(input.antrag);
            //kalkulation ust
            IRounding round = RoundingFactory.createRounding();

            Boolean useRuleEngine = AppConfig.Instance.getBooleanEntry("RULEENGINE_TRANSAKTIONSRISIKO", "USE_RULEENGINE", false, "ANTRAGSASSISTENT");
            String ruleCode = AppConfig.Instance.GetCfgEntry("ANTRAGSASSISTENT", "RULEENGINE_TRANSAKTIONSRISIKO", "USE_RULESET_VAR", "");
            if(!kalkVariante)
                ruleCode = AppConfig.Instance.GetCfgEntry("ANTRAGSASSISTENT", "RULEENGINE_TRANSAKTIONSRISIKO", "USE_RULESET_B2B", "");

            if (useRuleEngine)
            {
                ocheckTrRiskDto rval = new ocheckTrRiskDto();
                rval.frontid = "CHECK_NOK";

                CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto[] vars = null;
                /*
                if (!kalkVariante)
                {
                    vars = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto[4];
                    vars[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto();
                    vars[0].LookupVariableName = "TR";
                    vars[0].VariableName = "SPRACHE";
                    vars[0].Value = isoCode;

                    vars[1] = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto();
                    vars[1].LookupVariableName = "TR";
                    vars[1].VariableName = "RESTWERT";
                    vars[1].Value = "" + (decimal?)round.RoundCHF(input.antrag.kalkulation.angAntKalkDto.rwBrutto);

                    vars[2] = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto();
                    vars[2].LookupVariableName = "TR";
                    vars[2].VariableName = "LAUFZEIT";
                    vars[2].Value = "" + input.antrag.kalkulation.angAntKalkDto.lz;

                    vars[3] = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto();
                    vars[3].LookupVariableName = "TR";
                    vars[3].VariableName = "ANZAHLUNG";
                    vars[3].Value = "" + (decimal?)round.RoundCHF(input.antrag.kalkulation.angAntKalkDto.sz);
                }

                */

                
                BPEQueue bpeQueue = new BPEQueue();
                bpeQueue.addQueue("qTR");

                if (!kalkVariante && input.antrag.kalkulation.angAntKalkVar3Dto!=null)
                {
                    bpeQueue.addQueueRecord("qTR").addQueueRecordValue("F01", "RESTWERT").addQueueRecordValue("F02", round.RoundCHF(input.antrag.kalkulation.angAntKalkVar3Dto.rwBrutto).ToString(CultureInfo.InvariantCulture));
                    bpeQueue.addQueueRecord("qTR").addQueueRecordValue("F01", "ANZAHLUNG").addQueueRecordValue("F02",  round.RoundCHF(input.antrag.kalkulation.angAntKalkVar3Dto.sz).ToString(CultureInfo.InvariantCulture));
                    bpeQueue.addQueueRecord("qTR").addQueueRecordValue("F01", "LAUFZEIT").addQueueRecordValue("F02",  input.antrag.kalkulation.angAntKalkVar3Dto.lz.ToString(CultureInfo.InvariantCulture));
                }


                List<CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto> queueResult = BPEBo.getQueueData(input.antrag.sysid, "ANTRAG", new String[] { "qTR" }, ruleCode, vars, syswfuser, bpeQueue.getQueues());
                CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto outQueue = (from f in queueResult
                                                                               where f.Name.Equals("qTR")
                                                                               select f).FirstOrDefault();
                //daten nun in antkalkvar rang=1 = 1.variante, rang=2 = 2.variante (gleiche rate)
               /* foreach (CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto qr in outQueue.Records)
                {
                    
                }*/

                rval.antrag = input.antrag;
                String status = "";
                if (!kalkVariante)
                {
                    rval.antrag.kalkulation.angAntKalkVar3Dto = getVariant(input.antrag.sysid, 3, out status);
                    rval.frontid = status;
                }
                else
                {
                    rval.antrag.kalkulation.angAntKalkVar1Dto = getVariant(input.antrag.sysid, 1,out status);
                    rval.frontid = status;
                    normalizeOutput(rval);

                    rval.antrag.kalkulation.angAntKalkVar2Dto = getVariant(input.antrag.sysid, 2,out status);
                    if(!"OK".Equals(rval.frontid))
                        rval.frontid = status;
                }
                

                return normalizeOutput(rval);
            }

            bool b2b = !kalkVariante;
            IPrismaDao prismaDao = PrismaDaoFactory.getInstance().getPrismaDao();
            IMwStBo mwstBo = new MwStBo(mwStDao);

            ocheckTrRiskByIdDto outputGUI = new ocheckTrRiskByIdDto();

            //sysvart, cctx.getUserLanguange()
            // bool FEL1 = FF && berechnen && (FF_LEASING || FF_TZK_x) && vg != null && v_clusterNames.Contains(vg.name);
            ocheckTrRiskDto output = new ocheckTrRiskDto();
            ocheckAntAngDto outDtoTR1 = new ocheckAntAngDto();
            ocheckAntAngDto outDtoTR2 = new ocheckAntAngDto();
            ocheckAntAngDto outDto = new ocheckAntAngDto();
            output.frontid = "CHECK_NOK";

            ELInDto eLIndto = new ELInDto();
            ELOutDto elOutDto = new ELOutDto();

           
           


            if (kalkVariante == true)
            {
                eLIndto = this.eurotaxDBDao.getELInDtoAusDB(input.antrag.sysid, input.antrag.kalkulation.angAntKalkDto.sysprproduct, sysPEROLE);
            }
            else
                // kalkVariante == false wenn B2B Variante3 prüft
                if (input.antrag.kalkulation.angAntKalkVar3Dto != null)
                {
                    eLIndto = this.eurotaxDBDao.getELInDtoAusAntrag(input.antrag.sysid, input.antrag.kalkulation.angAntKalkVar3Dto);
                }
                else
                {
                    throw new Exception("Transaction Risiko Prüfung nicht möglich / Variante 3 ist null");
                }

            double ust = 0;

            String vartCode = trDao.getCodeAusVart(eLIndto.sysvart);
            if (trDao.getCodeAusVart(eLIndto.sysvart) != "" && (vartCode.IndexOf("TZK") > -1))
            {
                ust = 0;
            }
            else
            {
                if (eLIndto.mwst == null || eLIndto.mwst == 0)
                {
                    ust = mwstBo.getMehrwertSteuer(1, eLIndto.sysvart, DateTime.Now);
                }
                else
                {
                    ust = (double)eLIndto.mwst;
                }
            }


            ELInDto eLIndtoOrg = new ELInDto();
            eLIndtoOrg = ObjectCloner.Clone(eLIndto);

            //Decision Engine INPUT
            bool simulationDE = true;
            DecisionEngineInDto inputDE = new DecisionEngineInDto();
            try
            {

                inputDE = getRequestDEFromAntrag(input.antrag);
            }
            catch (Exception)
            {
                _log.Info("Exception Decision Engine DEFRAUD  is not found");
                simulationDE = false;

            }
            // ANTRAG
            AngAntKalkDto uKalk = new AngAntKalkDto();
            uKalk = ObjectCloner.Clone(input.antrag.kalkulation.angAntKalkDto);
            AntragDto inputantrag = new AntragDto();
            inputantrag = input.antrag;




            //KUNDE
            long syskdTyp = 1;
            if (inputantrag.antrag != null && inputantrag.kunde != null)
            {
                syskdTyp = inputantrag.kunde.syskdtyp;
            }

            //a)	mit einer Anzahlung von = CHF 2‘000 wäre die TR-Prüfung erfolgreich
            double sz = eLIndto.anzahlung;


            //get Faktoren
            VgDto vg = new VgDto();
            FaktorenDto faktoren = getFaktoren(eLIndto.sysvg, eLIndto.scorebezeichnung, (long)eLIndto.ausfallwvg);
            outputGUI.Faktoren = faktoren;



            //Flags fkp kp, pkp umsp bwgp strap fel1r ums stra bwg 
            Flags4TR flags4TR = getFlags(eLIndto.sysid, eLIndto.scorebezeichnung, eLIndto.sysvg, eLIndto.sysVM, syskdTyp);
            if (eLIndto.sysvg > 0)
            {
                vg = vgDao.getVg(eLIndto.sysvg);

            }
            VClusterParamDto clusterParam = null;
            clusterParam = getClusterParam(vg, eLIndto.scorebezeichnung);
            outputGUI.clusterParam = clusterParam;

            Cic.OpenOne.GateBANKNOW.Common.DTO.VariantenParam variantenParam = getVarianteKalkParameter(eLIndto.sysvg, eLIndto.scorebezeichnung);
            outputGUI.VarianteTSParam = variantenParam;


            // Ausfallwahrscheinlichkeit = 1/(1+EXP(-(Scorekartenabhängiger_Kalibrierungswert+ Scorekartenabhängiger_Kalibrierungsfaktor*Scorewert)))//
            double ausfallwahrscheinlichkeitP = (1 / (1 + (double)Math.Pow(2.7182818285, (-(faktoren.kalibrierungswert + faktoren.kalibrierungsfaktor * eLIndto.scorewert))))) * 100;
            VClusterDto cluster = new VClusterDto();

            ELOutDto explos = new ELOutDto();
            explos = getEL(eLIndto, faktoren, ausfallwahrscheinlichkeitP);
            cluster.v_el_betrag = explos.TR;
            cluster.v_el_prozent = explos.TRP;
            cluster.v_prof = explos.profitabilitaetp;

            outputGUI.Ursprungskalkulation = new Cic.OpenOne.GateBANKNOW.Common.DTO.Ursprungskalkulation();
            outputGUI.Ursprungskalkulation.Anzahlung = eLIndto.anzahlung;
            outputGUI.Ursprungskalkulation.Laufzeit = eLIndto.laufzeit;
            outputGUI.Ursprungskalkulation.Restwert = eLIndto.restwert;
            outputGUI.Ursprungskalkulation.Rate = (double)eLIndto.rate;
            outputGUI.Ursprungskalkulation.Buchwert = explos.SUMME_BW;
            outputGUI.Ursprungskalkulation.TR_Risk = explos.TR;
            outputGUI.Ursprungskalkulation.eLOutDto = explos;

            if (kalkVariante == false)
            {
                transaktionsrisikoPrüfung(ref outDto, isoCode, true, false, cluster, clusterParam, flags4TR, prodKontext, inputantrag,eLIndto,b2b);
                if (outDto.status == ocheckAntAngDto.STATUS_GREEN && simulationDE)
                {

                    output.frontid = "CHECK_TR_OK";
                    irisikoSimDto inputV3 = new irisikoSimDto();
                    inputV3.antrag = input.antrag;
                    inputV3.eloutDto = explos;
                    inputV3.inputDE = inputDE;
                    inputV3.angAntKalkDto = input.antrag.kalkulation.angAntKalkVar3Dto;
                    orisikoSimDto outputRS3 = risikoSim(inputV3);
                    if (outputRS3.frontid.Equals("CHECK_DE_OK") || outputRS3.frontid.Equals("CHECK_DE_NOK"))
                    {
                        trDao.saveVariante(input.antrag.sysid, input.antrag.kalkulation.angAntKalkVar3Dto, (int)AntragVarianten.FreieKalkulation, outputRS3.simulationDERules);
                        output.frontid = outputRS3.frontid;

                        return normalizeOutput(output);
                    }
                    else
                    //output.frontid = outputRS3.frontid;
                    {
                        output.frontid = outputRS3.frontid;
                        return normalizeOutput(output);
                    }

                }
                output.frontid = "CHECK_TR_NOK";

                outputGUI.freivariante = new Cic.OpenOne.GateBANKNOW.Common.DTO.Freivariante();
                outputGUI.freivariante.eLOutDto = elOutDto;
                outputGUI.freivariante.eLOutDto = elOutDto;
                outputGUI.freivariante.Anzahlung = eLIndto.anzahlung;
                outputGUI.freivariante.Laufzeit = eLIndto.laufzeit;
                outputGUI.freivariante.Restwert = eLIndto.restwert;
                outputGUI.freivariante.Rate = (double)eLIndto.rate;
                outputGUI.freivariante.Buchwert = elOutDto.SUMME_BW;
                outputGUI.freivariante.TR_Risk = elOutDto.TR;


                return normalizeOutput(output);

            }


            int ALZ;
            if (eLIndto.laufzeit < variantenParam.LZ)
            {
                ALZ = variantenParam.ALZ1;
            }
            else
            {
                ALZ = variantenParam.ALZ2;
            }

            if (variantenParam.KOMBI == 0)
            {


                if (eLIndto.anzahlung <= variantenParam.ANZ)
                {

                    AngAntKalkDto angAntKalkDto = calculateMitNeuAnzahlung(variantenParam.ANZ, sysPEROLE, inputantrag.kalkulation, prodKontext, kkontext, ust, isoCode);
                    ELInDto eLIndtoNew = getELIndtoNachKalkulation(eLIndto, angAntKalkDto);
                    elOutDto = getEL(eLIndtoNew, faktoren, ausfallwahrscheinlichkeitP);



                    eLIndto.anzahlung = variantenParam.ANZ;
                    elOutDto = getEL(eLIndto, faktoren, ausfallwahrscheinlichkeitP);
                    cluster.v_el_betrag = elOutDto.TR;
                    cluster.v_el_prozent = elOutDto.TRP;
                    cluster.v_prof = elOutDto.profitabilitaetp;



                    transaktionsrisikoPrüfung(ref outDto, isoCode, true, false, cluster, clusterParam, flags4TR, prodKontext,inputantrag,eLIndto,b2b);

                    if (outDto.status == ocheckAntAngDto.STATUS_GREEN && simulationDE)
                    {

                        irisikoSimDto inputAUSW1 = new irisikoSimDto();
                        inputAUSW1.antrag = inputantrag;
                        inputAUSW1.eloutDto = elOutDto;
                        inputAUSW1.angAntKalkDto = input.antrag.kalkulation.angAntKalkDto;
                        inputAUSW1.angAntKalkDto.szBrutto = eLIndto.anzahlung;
                        inputAUSW1.inputDE = inputDE;
                        //orisikoSimDto outputRSAUSW1 = risikoSim(inputAUSW1);
                        //if (outputRSAUSW1.frontid.Equals("CHECK_OK"))
                        //{
                        //    output.frontid = "CHECK_AUSW_OK"; 
                        //}
                        //else
                        //    output.frontid = outputRSAUSW1.frontid;
                        output.frontid = "CHECK_AUSW_OK";

                    }

                    outputGUI.KOMBI0ANZ = new Cic.OpenOne.GateBANKNOW.Common.DTO.KOMBI0ANZ();
                    outputGUI.KOMBI0ANZ.eLOutDto = elOutDto;
                    outputGUI.KOMBI0ANZ.eLOutDto = elOutDto;
                    outputGUI.KOMBI0ANZ.Anzahlung = eLIndto.anzahlung;
                    outputGUI.KOMBI0ANZ.Laufzeit = eLIndto.laufzeit;
                    outputGUI.KOMBI0ANZ.Restwert = eLIndto.restwert;
                    outputGUI.KOMBI0ANZ.Rate = (double)eLIndto.rate;
                    outputGUI.KOMBI0ANZ.Buchwert = elOutDto.SUMME_BW;
                    outputGUI.KOMBI0ANZ.TR_Risk = elOutDto.TR;
                }

                if (output.frontid != "CHECK_OK")
                {
                    eLIndto.anzahlung = eLIndtoOrg.anzahlung;
                    eLIndto.laufzeit = eLIndto.laufzeit - ALZ;
                    elOutDto = getEL(eLIndto, faktoren, ausfallwahrscheinlichkeitP);
                    cluster.v_el_betrag = elOutDto.TR;
                    cluster.v_el_prozent = elOutDto.TRP;
                    cluster.v_prof = elOutDto.profitabilitaetp;



                    transaktionsrisikoPrüfung(ref outDto, isoCode, true, false, cluster, clusterParam, flags4TR,prodKontext,inputantrag,eLIndto,b2b);
                    if (outDto.status == ocheckAntAngDto.STATUS_GREEN && simulationDE)
                    {
                        irisikoSimDto inputAUSW2 = new irisikoSimDto();
                        inputAUSW2.antrag = inputantrag;
                        inputAUSW2.eloutDto = elOutDto;
                        inputAUSW2.angAntKalkDto = input.antrag.kalkulation.angAntKalkDto;
                        inputAUSW2.angAntKalkDto.szBrutto = eLIndto.anzahlung;
                        inputAUSW2.inputDE = inputDE;
                        //orisikoSimDto outputRSAUSW2 = risikoSim(inputAUSW2);
                        //if (outputRSAUSW2.frontid.Equals("CHECK_OK"))
                        //{
                        //    output.frontid = "CHECK_AUSW_OK"; ;
                        //}
                        //else
                        //    output.frontid = outputRSAUSW2.frontid;
                        output.frontid = "CHECK_AUSW_OK";

                    }
                    outputGUI.KOMBI0 = new Cic.OpenOne.GateBANKNOW.Common.DTO.KOMBI0();
                    outputGUI.KOMBI0.Anzahlung = eLIndto.anzahlung;
                    outputGUI.KOMBI0.Laufzeit = eLIndto.laufzeit;
                    outputGUI.KOMBI0.Restwert = eLIndto.restwert;
                    outputGUI.KOMBI0.Rate = (double)eLIndto.rate;
                    outputGUI.KOMBI0.Buchwert = elOutDto.SUMME_BW;
                    outputGUI.KOMBI0.TR_Risk = elOutDto.TR;
                    outputGUI.KOMBI0.eLOutDto = elOutDto;
                }

            }
            if (variantenParam.KOMBI == 1)
            {
                eLIndto = Mapper.Map<ELInDto, ELInDto>(eLIndtoOrg);
                if (eLIndto.anzahlung <= variantenParam.ANZ)
                {
                    eLIndto.anzahlung = variantenParam.ANZ;
                }

                double? evalR = null;
                if (faktoren.scoreTR_RWFormel != "")
                {
                    evalR = trDao.evalRestwert(faktoren.scoreTR_RWFormel, eLIndto.sysid, ALZ);
                }
                if (evalR == null)
                {
                    eLIndto.restwert = eLIndto.barkaufpreis - (((eLIndto.barkaufpreis - eLIndto.restwert) / eLIndto.laufzeit) * (eLIndto.laufzeit - ALZ));
                }
                else
                {
                    eLIndto.restwert = (double)evalR;
                }

                eLIndto.laufzeit = eLIndto.laufzeit - ALZ;
                elOutDto = getEL(eLIndto, faktoren, ausfallwahrscheinlichkeitP);
                cluster.v_el_betrag = elOutDto.TR;
                cluster.v_el_prozent = elOutDto.TRP;
                cluster.v_prof = elOutDto.profitabilitaetp;

                transaktionsrisikoPrüfung(ref outDto, isoCode, true, false, cluster, clusterParam, flags4TR, prodKontext,inputantrag,eLIndto,b2b);
                if (outDto.status == ocheckAntAngDto.STATUS_GREEN && simulationDE)
                {
                    irisikoSimDto inputAUSWK = new irisikoSimDto();
                    inputAUSWK.antrag = inputantrag;
                    inputAUSWK.eloutDto = elOutDto;
                    inputAUSWK.angAntKalkDto = input.antrag.kalkulation.angAntKalkDto;
                    inputAUSWK.angAntKalkDto.szBrutto = eLIndto.anzahlung;
                    inputAUSWK.inputDE = inputDE;
                    //orisikoSimDto outputRSAUSWK = risikoSim(inputAUSWK);
                    //if (outputRSAUSWK.frontid.Equals("CHECK_OK"))
                    //{
                    //    output.frontid = "CHECK_AUSW_OK"; ;
                    //}
                    //else
                    //    output.frontid = outputRSAUSWK.frontid;
                    output.frontid = "CHECK_AUSW_OK";
                }
                outputGUI.KOMBI1 = new Cic.OpenOne.GateBANKNOW.Common.DTO.KOMBI1();
                outputGUI.KOMBI1.Anzahlung = eLIndto.anzahlung;
                outputGUI.KOMBI1.Laufzeit = eLIndto.laufzeit;
                outputGUI.KOMBI1.Restwert = eLIndto.restwert;
                outputGUI.KOMBI1.Rate = (double)eLIndto.rate;
                outputGUI.KOMBI1.Buchwert = elOutDto.SUMME_BW;
                outputGUI.KOMBI1.TR_Risk = elOutDto.TR;
                outputGUI.KOMBI1.eLOutDto = elOutDto;

            }
            if (output.frontid == "CHECK_OK" || output.frontid == "CHECK_AUSW_OK")
            {
                output.frontid = output.frontid;
            }
            else
            {
                if (kalkVariante)
                {

                    //Finanzierungsvorschlag 1: Anpassung der min. erforderlichen Anzahlung
                    ELOutDto elOutDtoGUI1 = new ELOutDto();
                    AngAntKalkDto angAntKalkDtoV1 = getV1(eLIndtoOrg, inputantrag, faktoren, clusterParam, sysPEROLE, isoCode, ref elOutDtoGUI1, prodKontext, kkontext, ust, ref cluster);

                    if (angAntKalkDtoV1 != null)
                    {
                        _log.Info("Variante 1 vor Prüfung: " + XMLSerializer.SerializeUTF8WithoutNamespace(angAntKalkDtoV1));
                        ELInDto trprüfungV1 = new ELInDto();
                        trprüfungV1.anzahlung = angAntKalkDtoV1.szBrutto;
                        trprüfungV1.laufzeit = angAntKalkDtoV1.lz;
                        trprüfungV1.restwert = angAntKalkDtoV1.rwBrutto;
                        trprüfungV1.rate = angAntKalkDtoV1.rateBrutto;
                        ocheckAntAngDto outDtoPrüfungV1 = new ocheckAntAngDto();
                        transaktionsrisikoPrüfung(ref outDtoPrüfungV1, isoCode, true, false, cluster, clusterParam, flags4TR, prodKontext, inputantrag, trprüfungV1, b2b);
                        if (outDtoPrüfungV1.status != ocheckAntAngDto.STATUS_GREEN)
                        {
                            _log.Info("Variante 1 Prüfung  : "+ outDtoPrüfungV1.status  + XMLSerializer.SerializeUTF8WithoutNamespace(outDtoPrüfungV1));
                            angAntKalkDtoV1 = null;
                        }
                    }
                    outputGUI.variante1 = new Cic.OpenOne.GateBANKNOW.Common.DTO.Variante1();
                    if (angAntKalkDtoV1 != null)
                    {
                        _log.Info("Variante 1 nach Prüfung  : " + XMLSerializer.SerializeUTF8WithoutNamespace(angAntKalkDtoV1));
                        outputGUI.variante1.Anzahlung = angAntKalkDtoV1.szBrutto;
                        outputGUI.variante1.Laufzeit = angAntKalkDtoV1.lz;
                        outputGUI.variante1.Restwert = angAntKalkDtoV1.rwBrutto;
                        outputGUI.variante1.Rate = angAntKalkDtoV1.rateBrutto;
                        outputGUI.variante1.Buchwert = elOutDtoGUI1.SUMME_BW;
                        outputGUI.variante1.TR_Risk = elOutDtoGUI1.TR;
                        outputGUI.variante1.eLOutDto = elOutDtoGUI1;
                    }
                    //Finanzierungsvorschlag 2: Beibehaltung der erfassten Monatsrate
                    ELOutDto elOutDtoGUI2 = new ELOutDto();
                    AngAntKalkDto angAntKalkDtoV2 = getlzV2(eLIndtoOrg, inputantrag, faktoren, clusterParam, flags4TR, sysPEROLE, isoCode, ref elOutDtoGUI2, prodKontext, kkontext, ust, ref cluster);
                    if (angAntKalkDtoV2 != null)
                    {
                        _log.Info("Variante 2 vor Prüfung  : " + XMLSerializer.SerializeUTF8WithoutNamespace(angAntKalkDtoV2));
                        ELInDto trprüfungV2 = new ELInDto();
                        trprüfungV2.anzahlung = angAntKalkDtoV2.szBrutto;
                        trprüfungV2.laufzeit = angAntKalkDtoV2.lz;
                        trprüfungV2.restwert = angAntKalkDtoV2.rwBrutto;
                        trprüfungV2.rate = angAntKalkDtoV2.rateBrutto;
                        ocheckAntAngDto outDtoPrüfungV2 = new ocheckAntAngDto();
                        transaktionsrisikoPrüfung(ref outDtoPrüfungV2, isoCode, true, false, cluster, clusterParam, flags4TR, prodKontext, inputantrag, trprüfungV2, b2b);
                        if (outDtoPrüfungV2.status != ocheckAntAngDto.STATUS_GREEN)
                        {
                            _log.Info("Variante 2 Prüfung  : " + outDtoPrüfungV2.status + XMLSerializer.SerializeUTF8WithoutNamespace(outDtoPrüfungV2));
                            angAntKalkDtoV2 = null;
                        }
                    }
                    outputGUI.variante2 = new Cic.OpenOne.GateBANKNOW.Common.DTO.Variante2();
                    if (angAntKalkDtoV2 != null)
                    {
                        _log.Info("Variante 2 nach Prüfung  : " + XMLSerializer.SerializeUTF8WithoutNamespace(angAntKalkDtoV2));
                        outputGUI.variante2.Anzahlung = angAntKalkDtoV2.szBrutto;
                        outputGUI.variante2.Laufzeit = angAntKalkDtoV2.lz;
                        outputGUI.variante2.Restwert = angAntKalkDtoV2.rwBrutto;
                        outputGUI.variante2.Rate = angAntKalkDtoV2.rateBrutto;
                        outputGUI.variante2.Buchwert = elOutDtoGUI2.SUMME_BW;
                        outputGUI.variante2.TR_Risk = elOutDtoGUI2.TR;
                        outputGUI.variante2.eLOutDto = elOutDtoGUI2;
                    }

                    if (simulationDE)
                    {
                        if (angAntKalkDtoV1 != null)
                        {
                            irisikoSimDto inputV1 = new irisikoSimDto();
                            inputV1.antrag = inputantrag;
                            inputV1.eloutDto = elOutDtoGUI1;
                            inputV1.angAntKalkDto = angAntKalkDtoV1;
                            inputV1.inputDE = inputDE;
                            orisikoSimDto outputRS1 = risikoSim(inputV1);
                            if (outputRS1.frontid.Equals("CHECK_DE_OK") || outputRS1.frontid.Equals("CHECK_DE_NOK"))
                            {
                                trDao.saveVariante(inputantrag.sysid, uKalk, (int)AntragVarianten.Ursprungskalkulation, null);
                                if (angAntKalkDtoV1 != null)
                                {
                                    trDao.saveVariante(input.antrag.sysid, angAntKalkDtoV1, (int)AntragVarianten.Mindestanzahlung, outputRS1.simulationDERules);
                                    inputantrag.kalkulation.angAntKalkVar1Dto = angAntKalkDtoV1;
                                }
                                output.frontid = outputRS1.frontid;
                            }
                            else
                            {
                                output.frontid = outputRS1.frontid;
                            }
                        }
                        if (angAntKalkDtoV2 != null)
                        {
                            irisikoSimDto inputV2 = new irisikoSimDto();
                            inputV2.antrag = input.antrag;
                            inputV2.eloutDto = elOutDtoGUI2;
                            inputV2.angAntKalkDto = angAntKalkDtoV2;
                            inputV2.inputDE = inputDE;
                            orisikoSimDto outputRS2 = risikoSim(inputV2);
                            if (outputRS2.frontid.Equals("CHECK_DE_OK") || outputRS2.frontid.Equals("CHECK_DE_NOK"))
                            {

                                trDao.saveVariante(inputantrag.sysid, uKalk, (int)AntragVarianten.Ursprungskalkulation, null);
                                if (angAntKalkDtoV2 != null)
                                {
                                    trDao.saveVariante(input.antrag.sysid, angAntKalkDtoV2, (int)AntragVarianten.GleichbleibendeRate, outputRS2.simulationDERules);
                                    inputantrag.kalkulation.angAntKalkVar2Dto = angAntKalkDtoV2;
                                }
                                output.frontid = outputRS2.frontid;
                            }
                            else
                            {
                                output.frontid = outputRS2.frontid;
                            }
                        }

                    }
                    else
                    {
                        output.frontid = "CHECK_DE_NOK";
                    }
                }


            }

            output.antrag = inputantrag;
            String outputGUIString = XMLSerializer.SerializeUTF8WithoutNamespace(outputGUI);
            this.trDao.saveTROutput(input.antrag.sysid, outputGUIString);

            _log.Info("OUTPUTTR: " + XMLSerializer.SerializeUTF8WithoutNamespace(outputGUI));
            return normalizeOutput(output);
        }

        /// <summary>
        /// getELIndtoNachKalkulation
        /// </summary>
        /// <param name="eLIndto"></param>
        /// <param name="angAntKalkDto"></param>
        /// <returns></returns>
        private ELInDto getELIndtoNachKalkulation(ELInDto eLIndto, AngAntKalkDto angAntKalkDto)
        {
            ELInDto elInDtoNew = new ELInDto();
            elInDtoNew = Mapper.Map<ELInDto, ELInDto>(eLIndto);
            elInDtoNew.barkaufpreis = angAntKalkDto.bginternbrutto;
            elInDtoNew.anzahlung = angAntKalkDto.szBrutto;
            elInDtoNew.anz_bkp = System.Math.Round(eLIndto.anzahlung / eLIndto.barkaufpreis, 4);
            elInDtoNew.finanzierungsbetrag = eLIndto.barkaufpreis - angAntKalkDto.szBrutto;
            elInDtoNew.laufzeit = angAntKalkDto.lz;
            elInDtoNew.jahresKm = angAntKalkDto.ll;
            elInDtoNew.rate = angAntKalkDto.rateBrutto;
            elInDtoNew.restwert = angAntKalkDto.rwBrutto;
            elInDtoNew.zins = angAntKalkDto.zins;
            elInDtoNew.zinskosten = angAntKalkDto.calcZinskosten;
            return elInDtoNew;
        }


        public FaktorenDto getFaktoren(long sysvg, string scorebezeichnung, long ausfallwvg)
        {
            FaktorenDto faktorenDto = new FaktorenDto();
            faktorenDto.bkp_faktor = (double)vgDao.getVGValue(sysvg, DateTime.Now, scorebezeichnung, "BKP_FAKTOR", VGInterpolationMode.NONE, plSQLVersion.V1); //Faktor für den Barkaufpreis
            faktorenDto.pbkp_faktor = (double)vgDao.getVGValue(sysvg, DateTime.Now, scorebezeichnung, "P_BKP", VGInterpolationMode.NONE, plSQLVersion.V1); //Potenz zum Faktor Barkaufpreis
            faktorenDto.lz_faktor = (double)vgDao.getVGValue(sysvg, DateTime.Now, scorebezeichnung, "LZ_FAKTOR", VGInterpolationMode.NONE, plSQLVersion.V1); //Faktor für die Laufzeit
            faktorenDto.plz_faktor = (double)vgDao.getVGValue(sysvg, DateTime.Now, scorebezeichnung, "P_LZ", VGInterpolationMode.NONE, plSQLVersion.V1); //Potenz zum Faktor Laufzeit
            faktorenDto.OVERALLint = (double)vgDao.getVGValue(ausfallwvg, DateTime.Now, scorebezeichnung, "OVERALL_INT", VGInterpolationMode.NONE, plSQLVersion.V1);
            faktorenDto.OVERALLSlope = (double)vgDao.getVGValue(ausfallwvg, DateTime.Now, scorebezeichnung, "OVERALL_SLOPE", VGInterpolationMode.NONE, plSQLVersion.V1);
            faktorenDto.cost = (double)vgDao.getVGValue(sysvg, DateTime.Now, scorebezeichnung, "COST", VGInterpolationMode.NONE, plSQLVersion.V1); //Korrektur für die Profitabilität
            faktorenDto.kalibrierungswert = (double)vgDao.getVGValue(ausfallwvg, DateTime.Now, scorebezeichnung, "INT", VGInterpolationMode.NONE, plSQLVersion.V1);
            faktorenDto.kalibrierungsfaktor = (double)vgDao.getVGValue(ausfallwvg, DateTime.Now, scorebezeichnung, "SLOPE", VGInterpolationMode.NONE, plSQLVersion.V1);
            faktorenDto.alter_faktor = (double)vgDao.getVGValue(sysvg, DateTime.Now, scorebezeichnung, "ALTER_FAKTOR", VGInterpolationMode.NONE, plSQLVersion.V1); //Faktor für das Alter
            faktorenDto.palter_faktor = (double)vgDao.getVGValue(sysvg, DateTime.Now, scorebezeichnung, "P_ALT", VGInterpolationMode.NONE, plSQLVersion.V1); //Potenz zum Faktor Alter

            faktorenDto.scorekarte = scorebezeichnung;
            EaiparDao eaiParDao = new EaiparDao();
            faktorenDto.scoreKALTRFormel = eaiParDao.getEaiParFileByCode(scorebezeichnung + "_KALTR", "");
            faktorenDto.scoreTR_RWFormel = eaiParDao.getEaiParFileByCode(scorebezeichnung + "_TR_RW", "");

            return faktorenDto;

        }



        /// <summary>
        /// Variante 1 / Finanzierungsvorschlag 1: Anpassung der min. erforderlichen Anzahlung
        /// </summary>
        /// <param name="eLInDtoOrg"></param>
        /// <param name="antragDto"></param>
        /// <param name="faktoren"></param>
        /// <param name="clusterParam"></param>
        /// <param name="sysPEROLE"></param>
        /// <param name="isoCode"></param>
        /// <returns></returns>
        public AngAntKalkDto getV1(ELInDto eLInDtoOrg, AntragDto antragDto, FaktorenDto faktoren, VClusterParamDto clusterParam, long sysPEROLE, string isoCode, ref ELOutDto elOutDtoGUI, prKontextDto prodKontext, kalkKontext kkontext, double ust, ref VClusterDto cluster)
        {

            IRounding round = RoundingFactory.createRounding();

            ELInDto eLInDto = Mapper.Map<ELInDto, ELInDto>(eLInDtoOrg);

            double anzahlung = 0;

            try
            {
                anzahlung = getMinimalErfordelicheAnzahlung(eLInDto, faktoren, clusterParam, ref elOutDtoGUI, sysPEROLE, antragDto.kalkulation, prodKontext, kkontext, ust, isoCode);
            }

            catch (Exception e)
            {
                _log.Info(e.Message);
                return null;
            }


            AngAntKalkDto angAntKalkDtoV1 = new AngAntKalkDto();

            angAntKalkDtoV1.bginternbrutto = eLInDto.barkaufpreis;
            angAntKalkDtoV1.bgintern = round.getNetValue(eLInDto.barkaufpreis, ust);

            angAntKalkDtoV1.szBrutto = anzahlung;
            angAntKalkDtoV1.sz = round.getNetValue(angAntKalkDtoV1.szBrutto, ust);

            angAntKalkDtoV1.lz = (short)eLInDto.laufzeit;


            angAntKalkDtoV1.rwBrutto = eLInDto.restwert;
            angAntKalkDtoV1.rw = round.getNetValue(eLInDto.restwert, ust);

            angAntKalkDtoV1.rateBrutto = (double)eLInDto.rate;
            angAntKalkDtoV1.rate = round.getNetValue((double)eLInDto.rate, ust);

            angAntKalkDtoV1.ll = antragDto.kalkulation.angAntKalkDto.ll;
            angAntKalkDtoV1.auszahlung = antragDto.kalkulation.angAntKalkDto.auszahlung;


            angAntKalkDtoV1.sysprproduct = antragDto.kalkulation.angAntKalkDto.sysprproduct;




            KalkulationDto kalkinput = new KalkulationDto();
            IKalkulationBo bo = BOFactory.getInstance().createKalkulationBo(isoCode);
            Byte rateError = 0;
            kalkinput.angAntKalkDto = angAntKalkDtoV1;
            KalkulationDto kalk = bo.calculate(kalkinput, prodKontext, kkontext, isoCode, ref rateError);

            cluster.v_el_betrag = elOutDtoGUI.TR;
            cluster.v_el_prozent = elOutDtoGUI.TRP;
            cluster.v_prof = elOutDtoGUI.profitabilitaetp;



            return kalk.angAntKalkDto;
        }

        /// <summary>
        /// calculates the Variant
        /// </summary>
        /// <param name="antragDto"></param>
        /// <param name="prodKontext"></param>
        /// <param name="kkontext"></param>
        /// <param name="isoCode"></param>
        /// <returns></returns>
        public AngAntKalkDto getVariant(long sysid, int rang, out String status)//AntragDto antragDto, prKontextDto prodKontext, kalkKontext kkontext, string isoCode)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto rval = context.ExecuteStoreQuery<AngAntKalkDto>("select antkalkvar.* from antkalkvar,antkalk where antkalkvar.syskalk=antkalk.syskalk and antkalk.sysantrag=" + sysid + " and antkalkvar.rang=" + rang).FirstOrDefault();
                status = "";
                if (rval == null) return null;
                status = rval.status;
                return rval;
            }
            /*
            IRounding round = RoundingFactory.createRounding();

            AngAntKalkDto angAntKalkDtoV1 = new AngAntKalkDto();
            angAntKalkDtoV1.bginternbrutto = antragDto.kalkulation.angAntKalkDto.bginternbrutto;// eLInDto.barkaufpreis;
            angAntKalkDtoV1.bgintern = antragDto.kalkulation.angAntKalkDto.bgintern;// round.getNetValue(eLInDto.barkaufpreis, ust);
            angAntKalkDtoV1.szBrutto = antragDto.kalkulation.angAntKalkDto.szBrutto;// anzahlung;
            angAntKalkDtoV1.sz = antragDto.kalkulation.angAntKalkDto.sz;// round.getNetValue(angAntKalkDtoV1.szBrutto, ust);
            angAntKalkDtoV1.lz = antragDto.kalkulation.angAntKalkDto.lz;// (short)eLInDto.laufzeit;
            angAntKalkDtoV1.rwBrutto = antragDto.kalkulation.angAntKalkDto.rwBrutto;// eLInDto.restwert;
            angAntKalkDtoV1.rw = antragDto.kalkulation.angAntKalkDto.rw;// round.getNetValue(eLInDto.restwert, ust);
            angAntKalkDtoV1.rateBrutto = antragDto.kalkulation.angAntKalkDto.rateBrutto;// (double)eLInDto.rate;
            angAntKalkDtoV1.rate = antragDto.kalkulation.angAntKalkDto.rate;// round.getNetValue((double)eLInDto.rate, ust);
            angAntKalkDtoV1.ll = antragDto.kalkulation.angAntKalkDto.ll;
            angAntKalkDtoV1.auszahlung = antragDto.kalkulation.angAntKalkDto.auszahlung;
            angAntKalkDtoV1.sysprproduct = antragDto.kalkulation.angAntKalkDto.sysprproduct;

            KalkulationDto kalkinput = new KalkulationDto();
            IKalkulationBo bo = BOFactory.getInstance().createKalkulationBo(isoCode);
            Byte rateError = 0;
            kalkinput.angAntKalkDto = angAntKalkDtoV1;
            KalkulationDto kalk = bo.calculate(kalkinput, prodKontext, kkontext, isoCode, ref rateError);

            return kalk.angAntKalkDto;*/
        }


        /// <summary>
        /// Finanzierungsvorschlag 2: Beibehaltung der erfassten Monatsrate
        /// </summary>
        /// <param name="eLIndto"></param>
        /// <param name="antragDto"></param>
        /// <param name="isocode"></param>
        /// <param name="faktoren"></param>
        /// <param name="clusterParam"></param>
        /// <param name="flags4TR"></param>
        /// <param name="sysPEROLE"></param>
        /// <param name="isoCode"></param>
        /// <returns></returns>
        public AngAntKalkDto getlzV2(ELInDto eLIndto, AntragDto antragDto, FaktorenDto faktoren, VClusterParamDto clusterParam, Flags4TR flags4TR, long sysPEROLE, string isoCode, ref ELOutDto eLOutDtoGUI, prKontextDto prodKontext, kalkKontext kkontext, double ust, ref VClusterDto clusterext)
        {
            IPrismaDao prismaDao = PrismaDaoFactory.getInstance().getPrismaDao();
            IRounding round = RoundingFactory.createRounding();


            bool kondition = false;
            AngAntKalkDto angAntKalkDto = new AngAntKalkDto();
            VClusterDto cluster = new VClusterDto();
            ocheckAntAngDto outDtoV1 = new ocheckAntAngDto();

            angAntKalkDto = ObjectCloner.Clone(antragDto.kalkulation.angAntKalkDto);

            //nRW = uBKP - ((uBKP - uRW) / uLZ * nLZ) 
            //nANZ = Application.RoundUp((((nRW + RATE * (1 + (ZINS / 1200)) * (((1 + (ZINS / 1200)) ^ (nLZ - 1) - 1) / (ZINS / 1200))) / ((1 + (ZINS / 1200)) ^ (nLZ - 1)) / (1 + (ZINS / 1200)) - uBKP) * -1) / 500, 0) * 500 





            double anzahlung_Orig = eLIndto.anzahlung;



            // Ausfallwahrscheinlichkeit = 1/(1+EXP(-(Scorekartenabhängiger_Kalibrierungswert+ Scorekartenabhängiger_Kalibrierungsfaktor*Scorewert)))//
            double ausfallwahrscheinlichkeitP = (1 / (1 + (double)Math.Pow(2.7182818285, (-(faktoren.kalibrierungswert + faktoren.kalibrierungsfaktor * eLIndto.scorewert))))) * 100;

            double uBKP = eLIndto.barkaufpreis;

            double uRW = eLIndto.restwert;
            double nRW = 0;

            int uLZ = eLIndto.laufzeit;
            int nLZ = eLIndto.laufzeit;

            double nANZ = 0;


            double ZINS = eLIndto.zins;
            double? RATE = eLIndto.rate;


            while (!(kondition) && nLZ > 6)
            {
                nLZ = nLZ - 6;

                //nRW = uBKP - ((uBKP - uRW) / uLZ * nLZ) 
                nRW = uBKP - ((uBKP - uRW) / uLZ * nLZ);

                //nANZ = Application.RoundUp((((b22 + b23 * (1 + (b24 / 1200)) * (((1 + (b24 / 1200)) ^ (b25 - 1) - 1) / (b24 / 1200))) / ((1 + (b24 / 1200)) ^ (b25 - 1)) / (1 + (b24 / 1200)) - 17000) * -1) / 500, 0) * 500 
                double? temp = (((nRW + RATE * (1 + (ZINS / 1200)) * ((Math.Pow((1 + (ZINS / 1200)), (nLZ - 1)) - 1) / (ZINS / 1200))) / (Math.Pow(1 + (ZINS / 1200), (nLZ - 1))) / (1 + (ZINS / 1200)) - uBKP) * -1)/100;

                nANZ = Math.Round((double)temp, 0) * 100;

                //neue Kalkulation


                eLIndto.laufzeit = nLZ;
                eLIndto.restwert = nRW;
                eLIndto.anzahlung = nANZ;


                ELOutDto elOutDto = new ELOutDto();
                elOutDto = getEL(eLIndto, faktoren, ausfallwahrscheinlichkeitP);
                cluster.v_el_betrag = elOutDto.TR;
                cluster.v_el_prozent = elOutDto.TRP;
                cluster.v_prof = elOutDto.profitabilitaetp;

                eLOutDtoGUI = Mapper.Map<ELOutDto, ELOutDto>(elOutDto);

                transaktionsrisikoPrüfung(ref outDtoV1, isoCode, true, false, cluster, clusterParam, flags4TR, prodKontext,antragDto,eLIndto,false);

                if ((outDtoV1.status == ocheckAntAngDto.STATUS_RED) || (outDtoV1.status == ocheckAntAngDto.STATUS_YELLOW))
                {
                    nLZ = nLZ - 6;
                }
                if (outDtoV1.status == ocheckAntAngDto.STATUS_GREEN)
                {
                    kondition = true;
                    angAntKalkDto.lz = (short)nLZ;
                    angAntKalkDto.szBrutto = nANZ;
                    angAntKalkDto.rwBrutto = nRW;

                }
            }


            if (kondition)
            {

                KalkulationDto kalkinput = new KalkulationDto();
                kalkinput.angAntKalkDto = angAntKalkDto;
                angAntKalkDto.sz = round.getNetValue(angAntKalkDto.szBrutto, ust);
                angAntKalkDto.rw = round.getNetValue(angAntKalkDto.rwBrutto, ust);
               

                double anzahlung = 0;
                try
                {
                    anzahlung = getMinimalErfordelicheAnzahlung(eLIndto, faktoren, clusterParam, ref eLOutDtoGUI, sysPEROLE, kalkinput, prodKontext, kkontext, ust, isoCode);

                }
                catch (Exception e)
                {
                    _log.Info(e.Message);
                    return null;
                }
                angAntKalkDto.szBrutto = anzahlung;
                angAntKalkDto.sz = round.getNetValue(angAntKalkDto.szBrutto, ust);
                angAntKalkDto.sz = round.RoundCHF(angAntKalkDto.sz);

                IKalkulationBo bo = BOFactory.getInstance().createKalkulationBo(isoCode);
                Byte rateError = 0;
                kalkinput.angAntKalkDto = angAntKalkDto;
                KalkulationDto kalk = bo.calculate(kalkinput, prodKontext, kkontext, isoCode, ref rateError);
                clusterext = cluster;
                return kalk.angAntKalkDto;
            }

            return null;
        }


        public AngAntKalkDto getVariante3(AntragDto antragDto, ELInDto eLIndto, long sysPEROLE, string isoCode)
        {
            IPrismaDao prismaDao = PrismaDaoFactory.getInstance().getPrismaDao();
            IRounding round = RoundingFactory.createRounding();
            IMwStBo mwstBo = new MwStBo(mwStDao);
            prKontextDto prodKontext = new prKontextDto();
            kalkKontext kkontext = new kalkKontext();
            double ust = 0;
            String vartCode = trDao.getCodeAusVart(eLIndto.sysvart);
            if (trDao.getCodeAusVart(eLIndto.sysvart) != "" && (vartCode.IndexOf("TZK") > -1))
            {
                ust = 0;
            }
            else
            {
                if (eLIndto.mwst == null || eLIndto.mwst == 0)
                {
                    ust = mwstBo.getMehrwertSteuer(1, eLIndto.sysvart, DateTime.Now);
                }
                else
                {
                    ust = (double)eLIndto.mwst;
                }
            }


            prodKontext = MyCreateProductKontext(sysPEROLE, antragDto);
            kkontext = MyCreateKalkKontext(antragDto);
            AngAntKalkDto angAntKalkDto = new AngAntKalkDto();

            angAntKalkDto.bginternbrutto = eLIndto.barkaufpreis;
            angAntKalkDto.bgintern = round.getNetValue(eLIndto.barkaufpreis, ust);

            angAntKalkDto.sz = round.getNetValue(eLIndto.anzahlung, ust);

            angAntKalkDto.rw = round.getNetValue(eLIndto.restwert, ust);

            angAntKalkDto.rateBrutto = (double)eLIndto.rate;
            angAntKalkDto.rate = round.getNetValue((double)eLIndto.rate, ust);

            angAntKalkDto.ll = antragDto.kalkulation.angAntKalkDto.ll;

            angAntKalkDto.auszahlung = antragDto.kalkulation.angAntKalkDto.auszahlung;


            angAntKalkDto.sysprproduct = antragDto.kalkulation.angAntKalkDto.sysprproduct;


            KalkulationDto kalkinput = new KalkulationDto();
            IKalkulationBo bo = BOFactory.getInstance().createKalkulationBo(isoCode);
            Byte rateError = 0;
            kalkinput.angAntKalkDto = angAntKalkDto;
            KalkulationDto kalk = bo.calculate(kalkinput, prodKontext, kkontext, isoCode, ref rateError);
            return kalk.angAntKalkDto;

        }


        /// <summary>
        /// calculateMitNeuAnzahlung
        /// </summary>
        /// <param name="anzahlung"></param>
        /// <param name="sysPEROLE"></param>
        /// <param name="kalkulation"></param>
        /// <param name="prodKontext"></param>
        /// <param name="kkontext"></param>
        /// <param name="ust"></param>
        /// <param name="isoCode"></param>
        /// <returns></returns>
        public AngAntKalkDto calculateMitNeuAnzahlung(double anzahlung, long sysPEROLE, KalkulationDto kalkulation, prKontextDto prodKontext, kalkKontext kkontext, double ust, string isoCode)
        {
            IPrismaDao prismaDao = PrismaDaoFactory.getInstance().getPrismaDao();
            IMwStBo mwstBo = new MwStBo(mwStDao);
            IRounding round = RoundingFactory.createRounding();


            IMapper mapper = Mapper.Instance;/*.One.Utils.Util.Mapper.MapperConfig.getInstanceMapper("calculateMitNeuAnzahlung", delegate (MapperConfigurationExpression cfg) {
                cfg.CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto, Cic.OpenOne.GateBANKNOW.Common.DTO.AngAntKalkDto>();

                cfg.CreateMap<Cic.OpenOne.Common.DTO.AngAntProvDto, Cic.OpenOne.Common.DTO.AngAntProvDto>();
                cfg.CreateMap<Cic.OpenOne.Common.DTO.AngAntSubvDto, Cic.OpenOne.Common.DTO.AngAntSubvDto>();
                cfg.CreateMap<Cic.OpenOne.Common.DTO.AngAntVsDto, Cic.OpenOne.Common.DTO.AngAntVsDto>();
                cfg.CreateMap<Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto, Cic.OpenOne.GateBANKNOW.Common.DTO.KalkulationDto>();
            });*/

           
     
            //clone
            KalkulationDto kalkulationNew = new KalkulationDto();
            kalkulationNew = mapper.Map<KalkulationDto, KalkulationDto>(kalkulation);
            kalkulationNew.angAntKalkDto.sz = anzahlung;
            kalkulationNew.angAntKalkDto.szBrutto = anzahlung;
            kalkulationNew.angAntKalkDto.sz = round.getNetValue(kalkulationNew.angAntKalkDto.szBrutto, ust);

            IKalkulationBo bo = BOFactory.getInstance().createKalkulationBo(isoCode);
            Byte rateError = 0;
            KalkulationDto kalk = bo.calculate(kalkulationNew, prodKontext, kkontext, isoCode, ref rateError);
            return kalk.angAntKalkDto;


        }




        /// <summary>
        /// risikoSim
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override orisikoSimDto risikoSim(irisikoSimDto input)
        {
            orisikoSimDto output = new orisikoSimDto();
            AntragDto inDto = input.antrag;
            AngAntKalkDto angAntKalkDto = new AngAntKalkDto();
            angAntKalkDto = input.angAntKalkDto;

            if (angAntKalkDto == null)
            {
                output.frontid = "CHECK_DE_NOK";
                return output;
            }

            if (input.inputDE == null)
            {
                DecisionEngineInDto inDtoDE = new DecisionEngineInDto();
                inDtoDE = getRequestDEFromAntrag(input.antrag);

            }

            IRounding round = RoundingFactory.createRounding();
            input.inputDE.Finanzierungsbetrag = (decimal?)round.RoundCHF(angAntKalkDto.bginternbrutto);
            input.inputDE.Kaution = (decimal?)angAntKalkDto.depot;
            input.inputDE.Anzahlung_ErsteRate = (decimal?)round.RoundCHF(angAntKalkDto.sz);//wert kommt varianteninput
            input.inputDE.Laufzeit = angAntKalkDto.lz;//wert kommt varianteninput
            input.inputDE.Zinssatz = Math.Round((decimal)angAntKalkDto.zins, 6);//wert kommt varianteninput
            input.inputDE.Restwert = (decimal?)round.RoundCHF(angAntKalkDto.rwBrutto); //wert kommt varianteninput
            input.inputDE.Rate = (decimal?)round.RoundCHF((input.antrag.kalkulation.angAntKalkDto.rateBrutto + input.antrag.kalkulation.angAntKalkDto.calcRsvmonat));
            input.inputDE.Restwert_Eurotax = trDao.getRwbruttoFlag(input.antrag.angAntObDto.sysobtyp) == true ? (decimal?)round.RoundCHF(angAntKalkDto.rwBrutto) : 0;
            input.inputDE.Restwert_Banknow = trDao.getRwbruttoBankNowFlag(input.antrag.angAntObDto.sysobtyp) == true ? (decimal?)round.RoundCHF(angAntKalkDto.rwBrutto) : 0;

            input.inputDE.Expected_Loss = (decimal?)round.RoundCHF(input.eloutDto.TR);
            input.inputDE.Expected_Loss_Prozent = (decimal)round.Round(input.eloutDto.TRP, 2);
            input.inputDE.Expected_Loss_LGD = (decimal?)round.RoundCHF(input.eloutDto.LGD);
            input.inputDE.Profitabilitaet_Prozent = (decimal?)round.Round(input.eloutDto.profitabilitaetp, 2);



            IDecisionEngineBo decisionEngineBo = AuskunftBoFactory.CreateDefaultDecisionEngineBo();



            AuskunftDto auskunftOutput = decisionEngineBo.executeSimulation(input.inputDE);

            DecisionEngineOutDto outDto = new DecisionEngineOutDto();
            outDto = auskunftOutput.DecisionEngineOutDto;
            string rp;
            string fp;
            string rules = "";



            if (auskunftOutput.Fehlercode.Equals("0"))
            {
                for (int i = 0; i < outDto.RecordRRResponseDto.Length; i++)
                {
                    if (!string.IsNullOrEmpty(outDto.RecordRRResponseDto[i].DEC_Liste_getroffeneRegeln_RP))
                    {
                        rp = outDto.RecordRRResponseDto[i].DEC_Liste_getroffeneRegeln_RP;
                        // MyAddDERegel(deDetail, context, RP, rp);
                        rules = rules + rp + ";";

                    }

                    if (!string.IsNullOrEmpty(outDto.RecordRRResponseDto[i].DEC_Liste_getroffeneRegeln_FP))
                    {
                        fp = outDto.RecordRRResponseDto[i].DEC_Liste_getroffeneRegeln_FP;
                        // MyAddDERegel(deDetail, context, FP, fp);
                        rules = rules + fp + ";";

                    }
                }

                string[] simulationDERules = rules.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                output.simulationDERules = simulationDERules;

                if (rules.Length > 0)
                {
                    output.frontid = "CHECK_DE_NOK";
                }
                else
                {
                    output.frontid = "CHECK_DE_OK";

                }
            }
            else
            {
                output.frontid = "CHECK_DE_NOK";
            }
            output.antrag = input.antrag;
            return output;

        }

        /// <summary>
        /// Risikosimulation for Interop Call
        /// </summary>
        /// <param name="input"></param>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        public orisikoSimIODto risikoSim2(irisikoSimIODto input,long sysperole)
        {

            AntragDto inputantrag = angAntDao.getAntrag(input.sysid, sysperole);
            DecisionEngineInDto inDtoDE =  getRequestDEFromAntrag(inputantrag);

            inDtoDE.Finanzierungsbetrag = input.Finanzierungsbetrag;
            inDtoDE.Kaution = input.Kaution;
            inDtoDE.Anzahlung_ErsteRate = input.Anzahlung_ErsteRate;
            inDtoDE.Laufzeit = input.Laufzeit;
            inDtoDE.Zinssatz = input.Zinssatz;
            inDtoDE.Restwert = input.Restwert;
            inDtoDE.Rate = input.Rate;
            inDtoDE.Restwert_Eurotax = input.Restwert_Eurotax;
            inDtoDE.Restwert_Banknow = input.Restwert_Banknow;

            inDtoDE.Expected_Loss = input.Expected_Loss;
            inDtoDE.Expected_Loss_Prozent = input.Expected_Loss_Prozent;
            inDtoDE.Expected_Loss_LGD = input.Expected_Loss_LGD;
            inDtoDE.Profitabilitaet_Prozent = input.Profitabilitaet_Prozent;

            IDecisionEngineBo decisionEngineBo = AuskunftBoFactory.CreateDefaultDecisionEngineBo();
            AuskunftDto auskunftOutput = decisionEngineBo.executeSimulation(inDtoDE);

            DecisionEngineOutDto outDto = new DecisionEngineOutDto();
            outDto = auskunftOutput.DecisionEngineOutDto;
            string rp;
            string fp;
            string rules = "";


            orisikoSimIODto output = new orisikoSimIODto();
            if (auskunftOutput.Fehlercode.Equals("0"))
            {
                for (int i = 0; i < outDto.RecordRRResponseDto.Length; i++)
                {
                    if (!string.IsNullOrEmpty(outDto.RecordRRResponseDto[i].DEC_Liste_getroffeneRegeln_RP))
                    {
                        rp = outDto.RecordRRResponseDto[i].DEC_Liste_getroffeneRegeln_RP;
                        rules = rules + rp + ";";
                    }

                    if (!string.IsNullOrEmpty(outDto.RecordRRResponseDto[i].DEC_Liste_getroffeneRegeln_FP))
                    {
                        fp = outDto.RecordRRResponseDto[i].DEC_Liste_getroffeneRegeln_FP;
                        rules = rules + fp + ";";
                    }
                }

                string[] simulationDERules = rules.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                output.simulationDERules = simulationDERules;

           
            }
            return output;

        }

        /// <summary>
        /// solveKalkVarianten
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override osolveKalkVariantenDto solveKalkVarianten(isolveKalkVariantenDto input)
        {



            return null;
        }



        #region TESTGUI
        /// <summary>
        /// checkTrRiskBySysid
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="sysPEROLE"></param>
        /// <param name="isoCode"></param>
        /// <returns></returns>
        public override ocheckTrRiskByIdDto checkTrRiskBySysid(icheckTrRiskByIdDto inDto)
        {
            ocheckTrRiskByIdDto outputGUI = new ocheckTrRiskByIdDto();
            AntragDto inputantrag = angAntDao.getAntrag(inDto.sysid,inDto.sysPEROLE);
            icheckTrRiskDto input = new icheckTrRiskDto();
            input.antrag = inputantrag;
            ocheckTrRiskDto output = new ocheckTrRiskDto();

            output = checkTrRisk(input, inDto.sysPEROLE, inDto.isoCode, true, ref outputGUI);
            String outputString = XMLSerializer.SerializeUTF8WithoutNamespace(outputGUI);
            return outputGUI;

        }

        private ocheckTrRiskDto normalizeOutput(ocheckTrRiskDto output)
        {
            // Fix für Weboberfläche
            if (output.frontid.Contains("AUSW_OK"))
            {
                output.frontid = "CHECK_AUSW_OK";
            }
            else if (output.frontid.Contains("NOK"))
            {
                output.frontid = "CHECK_NOK";
            }
            else
            {
                output.frontid = "CHECK_OK";
            }
            return output;
        }

        /// <summary>
        /// checkTrRisk
        /// </summary>
        /// <param name="input">input</param>
        /// <param name="sysPEROLE">sysPEROLE</param>
        /// <param name="isoCode">isoCode</param>
        /// <param name="kalkVariante">kalkVariante</param>
        /// <returns></returns>
        public override ocheckTrRiskDto checkTrRisk(icheckTrRiskDto input, long sysPEROLE, string isoCode, bool kalkVariante, ref ocheckTrRiskByIdDto outputGUI)
        {
            IPrismaDao prismaDao = PrismaDaoFactory.getInstance().getPrismaDao();
            IMwStBo mwstBo = new MwStBo(mwStDao);
            bool b2b = !kalkVariante;
            //sysvart, cctx.getUserLanguange()
            // bool FEL1 = FF && berechnen && (FF_LEASING || FF_TZK_x) && vg != null && v_clusterNames.Contains(vg.name);
            ocheckTrRiskDto output = new ocheckTrRiskDto();
            ocheckAntAngDto outDtoTR1 = new ocheckAntAngDto();
            ocheckAntAngDto outDtoTR2 = new ocheckAntAngDto();
            ocheckAntAngDto outDto = new ocheckAntAngDto();
            output.frontid = "CHECK_NOK";

            ELInDto eLIndto = new ELInDto();
            ELOutDto elOutDto = new ELOutDto();

            // Kalkulation Kontext
            prKontextDto prodKontext = new prKontextDto();
            kalkKontext kkontext = new kalkKontext();
            prodKontext = MyCreateProductKontext(sysPEROLE, input.antrag);
            kkontext = MyCreateKalkKontext(input.antrag);
           
          


            if (kalkVariante == true)
            {
                eLIndto = this.eurotaxDBDao.getELInDtoAusDB(input.antrag.sysid, prodKontext.sysprproduct, sysPEROLE);
            }
            else
                // kalkVariante == false wenn B2B Variante3 prüft
                if (input.antrag.kalkulation.angAntKalkVar3Dto != null)
                {
                    eLIndto = this.eurotaxDBDao.getELInDtoAusAntrag(input.antrag.sysid, input.antrag.kalkulation.angAntKalkVar3Dto);
                }
                else
                {
                    throw new Exception("Transaction Risiko Prüfung nicht möglich / Variante 3 ist null");
                }

            //kalkulation ust
            double ust = 0;
            String vartCode = trDao.getCodeAusVart(eLIndto.sysvart);
            if (trDao.getCodeAusVart(eLIndto.sysvart) != "" && (vartCode.IndexOf("TZK") > -1))
            {
                ust = 0;
            }
            else
            {
                if (eLIndto.mwst == null || eLIndto.mwst == 0)
                {
                    ust = mwstBo.getMehrwertSteuer(1, eLIndto.sysvart, DateTime.Now);
                }
                else
                {
                    ust = (double)eLIndto.mwst;
                }
            }


            ELInDto eLIndtoOrg = new ELInDto();
            eLIndtoOrg = ObjectCloner.Clone(eLIndto);

            //Decision Engine INPUT
            bool simulationDE = true;
            DecisionEngineInDto inputDE = new DecisionEngineInDto();
            try
            {

                inputDE = getRequestDEFromAntrag(input.antrag);
            }
            catch (Exception)
            {
                _log.Info("Exception Decision Engine DEFRAUD  is not found");
                simulationDE = false;

            }
            // ANTRAG
            AngAntKalkDto uKalk = new AngAntKalkDto();
            uKalk = ObjectCloner.Clone(input.antrag.kalkulation.angAntKalkDto);
            AntragDto inputantrag = new AntragDto();
            inputantrag = input.antrag;




            //KUNDE
            long syskdTyp = 1;
            if (inputantrag.antrag != null && inputantrag.kunde != null)
            {
                syskdTyp = inputantrag.kunde.syskdtyp;
            }

            //a)	mit einer Anzahlung von = CHF 2‘000 wäre die TR-Prüfung erfolgreich
            double sz = eLIndto.anzahlung;


            //get Faktoren
            VgDto vg = new VgDto();
            FaktorenDto faktoren = getFaktoren(eLIndto.sysvg, eLIndto.scorebezeichnung, (long)eLIndto.ausfallwvg);
            outputGUI.Faktoren = faktoren;



            //Flags fkp kp, pkp umsp bwgp strap fel1r ums stra bwg 
            Flags4TR flags4TR = getFlags(eLIndto.sysid, eLIndto.scorebezeichnung, eLIndto.sysvg, eLIndto.sysVM, syskdTyp);
            if (eLIndto.sysvg > 0)
            {
                vg = vgDao.getVg(eLIndto.sysvg);

            }
            VClusterParamDto clusterParam = null;
            clusterParam = getClusterParam(vg, eLIndto.scorebezeichnung);
            outputGUI.clusterParam = clusterParam;

            Cic.OpenOne.GateBANKNOW.Common.DTO.VariantenParam variantenParam = getVarianteKalkParameter(eLIndto.sysvg, eLIndto.scorebezeichnung);
            outputGUI.VarianteTSParam = variantenParam;

            // Ausfallwahrscheinlichkeit = 1/(1+EXP(-(Scorekartenabhängiger_Kalibrierungswert+ Scorekartenabhängiger_Kalibrierungsfaktor*Scorewert)))//
            double ausfallwahrscheinlichkeitP = (1 / (1 + (double)Math.Pow(2.7182818285, (-(faktoren.kalibrierungswert + faktoren.kalibrierungsfaktor * eLIndto.scorewert))))) * 100;
            VClusterDto cluster = new VClusterDto();

            ELOutDto explos = new ELOutDto();
            explos = getEL(eLIndto, faktoren, ausfallwahrscheinlichkeitP);
            cluster.v_el_betrag = explos.TR;
            cluster.v_el_prozent = explos.TRP;
            cluster.v_prof = explos.profitabilitaetp;

            outputGUI.Ursprungskalkulation = new Cic.OpenOne.GateBANKNOW.Common.DTO.Ursprungskalkulation();
            outputGUI.Ursprungskalkulation.Anzahlung = eLIndto.anzahlung;
            outputGUI.Ursprungskalkulation.Laufzeit = eLIndto.laufzeit;
            outputGUI.Ursprungskalkulation.Restwert = eLIndto.restwert;
            outputGUI.Ursprungskalkulation.Rate = (double)eLIndto.rate;
            outputGUI.Ursprungskalkulation.Buchwert = explos.SUMME_BW;
            outputGUI.Ursprungskalkulation.TR_Risk = explos.TR;
            outputGUI.Ursprungskalkulation.eLOutDto = explos;


            if (kalkVariante == false)
            {
                transaktionsrisikoPrüfung(ref outDto, isoCode, true, false, cluster, clusterParam, flags4TR, prodKontext,inputantrag,eLIndto,b2b);
                if (outDto.status == ocheckAntAngDto.STATUS_GREEN && simulationDE)
                {

                    output.frontid = "CHECK_TR_OK";
                    irisikoSimDto inputV3 = new irisikoSimDto();
                    inputV3.antrag = input.antrag;
                    inputV3.eloutDto = explos;
                    inputV3.inputDE = inputDE;
                    inputV3.angAntKalkDto = input.antrag.kalkulation.angAntKalkVar3Dto;
                    orisikoSimDto outputRS3 = risikoSim(inputV3);
                    if (outputRS3.frontid.Equals("CHECK_DE_OK") || outputRS3.frontid.Equals("CHECK_DE_NOK"))
                    {
                        trDao.saveVariante(input.antrag.sysid, input.antrag.kalkulation.angAntKalkVar3Dto, (int)AntragVarianten.FreieKalkulation, outputRS3.simulationDERules);
                        output.frontid = "outputRS3.frontid";

                        return normalizeOutput(output);
                    }
                    else
                    //output.frontid = outputRS3.frontid;
                    {
                        output.frontid = outputRS3.frontid;
                        return normalizeOutput(output);
                    }

                }
                output.frontid = "CHECK_TR_NOK";

                outputGUI.freivariante = new Cic.OpenOne.GateBANKNOW.Common.DTO.Freivariante();
                outputGUI.freivariante.eLOutDto = elOutDto;
                outputGUI.freivariante.eLOutDto = elOutDto;
                outputGUI.freivariante.Anzahlung = eLIndto.anzahlung;
                outputGUI.freivariante.Laufzeit = eLIndto.laufzeit;
                outputGUI.freivariante.Restwert = eLIndto.restwert;
                outputGUI.freivariante.Rate = (double)eLIndto.rate;
                outputGUI.freivariante.Buchwert = elOutDto.SUMME_BW;
                outputGUI.freivariante.TR_Risk = elOutDto.TR;

                return normalizeOutput(output);

            }


            int ALZ;
            if (eLIndto.laufzeit < variantenParam.LZ)
            {
                ALZ = variantenParam.ALZ1;
            }
            else
            {
                ALZ = variantenParam.ALZ2;
            }

            if (variantenParam.KOMBI == 0)
            {


                if (eLIndto.anzahlung <= variantenParam.ANZ)
                {

                    AngAntKalkDto angAntKalkDto = calculateMitNeuAnzahlung(variantenParam.ANZ, sysPEROLE, inputantrag.kalkulation, prodKontext, kkontext, ust, isoCode);
                    ELInDto eLIndtoNew = getELIndtoNachKalkulation(eLIndto, angAntKalkDto);
                    elOutDto = getEL(eLIndtoNew, faktoren, ausfallwahrscheinlichkeitP);



                    eLIndto.anzahlung = variantenParam.ANZ;
                    elOutDto = getEL(eLIndto, faktoren, ausfallwahrscheinlichkeitP);
                    cluster.v_el_betrag = elOutDto.TR;
                    cluster.v_el_prozent = elOutDto.TRP;
                    cluster.v_prof = elOutDto.profitabilitaetp;



                    transaktionsrisikoPrüfung(ref outDto, isoCode, true, false, cluster, clusterParam, flags4TR, prodKontext,inputantrag, eLIndto, b2b);

                    if (outDto.status == ocheckAntAngDto.STATUS_GREEN && simulationDE)
                    {

                        irisikoSimDto inputAUSW1 = new irisikoSimDto();
                        inputAUSW1.antrag = inputantrag;
                        inputAUSW1.eloutDto = elOutDto;
                        inputAUSW1.angAntKalkDto = input.antrag.kalkulation.angAntKalkDto;
                        inputAUSW1.angAntKalkDto.szBrutto = eLIndto.anzahlung;
                        inputAUSW1.inputDE = inputDE;
                        //orisikoSimDto outputRSAUSW1 = risikoSim(inputAUSW1);
                        //if (outputRSAUSW1.frontid.Equals("CHECK_OK"))
                        //{
                        //    output.frontid = "CHECK_AUSW_OK"; ;
                        //}
                        //else
                        //    output.frontid = outputRSAUSW1.frontid;
                        output.frontid = "CHECK_AUSW_OK";

                    }
                    outputGUI.KOMBI0ANZ = new Cic.OpenOne.GateBANKNOW.Common.DTO.KOMBI0ANZ();
                    outputGUI.KOMBI0ANZ.eLOutDto = elOutDto;
                    outputGUI.KOMBI0ANZ.eLOutDto = elOutDto;
                    outputGUI.KOMBI0ANZ.Anzahlung = eLIndto.anzahlung;
                    outputGUI.KOMBI0ANZ.Laufzeit = eLIndto.laufzeit;
                    outputGUI.KOMBI0ANZ.Restwert = eLIndto.restwert;
                    outputGUI.KOMBI0ANZ.Rate = (double)eLIndto.rate;
                    outputGUI.KOMBI0ANZ.Buchwert = elOutDto.SUMME_BW;
                    outputGUI.KOMBI0ANZ.TR_Risk = elOutDto.TR;

                }

                if (output.frontid != "CHECK_OK")
                {
                    eLIndto.anzahlung = eLIndtoOrg.anzahlung;
                    eLIndto.laufzeit = eLIndto.laufzeit - ALZ;
                    elOutDto = getEL(eLIndto, faktoren, ausfallwahrscheinlichkeitP);
                    cluster.v_el_betrag = elOutDto.TR;
                    cluster.v_el_prozent = elOutDto.TRP;
                    cluster.v_prof = elOutDto.profitabilitaetp;



                    transaktionsrisikoPrüfung(ref outDto, isoCode, true, false, cluster, clusterParam, flags4TR, prodKontext,inputantrag,eLIndto,b2b);
                    if (outDto.status == ocheckAntAngDto.STATUS_GREEN && simulationDE)
                    {
                        irisikoSimDto inputAUSW2 = new irisikoSimDto();
                        inputAUSW2.antrag = inputantrag;
                        inputAUSW2.eloutDto = elOutDto;
                        inputAUSW2.angAntKalkDto = input.antrag.kalkulation.angAntKalkDto;
                        inputAUSW2.angAntKalkDto.szBrutto = eLIndto.anzahlung;
                        inputAUSW2.inputDE = inputDE;
                        //orisikoSimDto outputRSAUSW2 = risikoSim(inputAUSW2);
                        //if (outputRSAUSW2.frontid.Equals("CHECK_OK"))
                        //{
                        //    output.frontid = "CHECK_AUSW_OK"; ;
                        //}
                        //else
                        //    output.frontid = outputRSAUSW2.frontid;
                        output.frontid = "CHECK_AUSW_OK";
                    }

                    outputGUI.KOMBI0 = new Cic.OpenOne.GateBANKNOW.Common.DTO.KOMBI0();
                    outputGUI.KOMBI0.Anzahlung = eLIndto.anzahlung;
                    outputGUI.KOMBI0.Laufzeit = eLIndto.laufzeit;
                    outputGUI.KOMBI0.Restwert = eLIndto.restwert;
                    outputGUI.KOMBI0.Rate = (double)eLIndto.rate;
                    outputGUI.KOMBI0.Buchwert = elOutDto.SUMME_BW;
                    outputGUI.KOMBI0.TR_Risk = elOutDto.TR;
                    outputGUI.KOMBI0.eLOutDto = elOutDto;

                }

            }

            if (variantenParam.KOMBI == 1)
            {
                eLIndto = Mapper.Map<ELInDto, ELInDto>(eLIndtoOrg);
                if (eLIndto.anzahlung <= variantenParam.ANZ)
                {
                    eLIndto.anzahlung = variantenParam.ANZ;
                }

                double? evalR = null;
                if (faktoren.scoreTR_RWFormel != "")
                {
                    evalR = trDao.evalRestwert(faktoren.scoreTR_RWFormel, eLIndto.sysid, ALZ);
                }
                if (evalR == null)
                {
                    eLIndto.restwert = eLIndto.barkaufpreis - (((eLIndto.barkaufpreis - eLIndto.restwert) / eLIndto.laufzeit) * (eLIndto.laufzeit - ALZ));
                }
                else
                {
                    eLIndto.restwert = (double)evalR;
                }

                eLIndto.laufzeit = eLIndto.laufzeit - ALZ;
                elOutDto = getEL(eLIndto, faktoren, ausfallwahrscheinlichkeitP);
                cluster.v_el_betrag = elOutDto.TR;
                cluster.v_el_prozent = elOutDto.TRP;
                cluster.v_prof = elOutDto.profitabilitaetp;

                transaktionsrisikoPrüfung(ref outDto, isoCode, true, false, cluster, clusterParam, flags4TR, prodKontext,inputantrag,eLIndto,b2b);
                if (outDto.status == ocheckAntAngDto.STATUS_GREEN && simulationDE)
                {
                    irisikoSimDto inputAUSWK = new irisikoSimDto();
                    inputAUSWK.antrag = inputantrag;
                    inputAUSWK.eloutDto = elOutDto;
                    inputAUSWK.angAntKalkDto = input.antrag.kalkulation.angAntKalkDto;
                    inputAUSWK.angAntKalkDto.szBrutto = eLIndto.anzahlung;
                    inputAUSWK.inputDE = inputDE;
                    //orisikoSimDto outputRSAUSWK = risikoSim(inputAUSWK);
                    //if (outputRSAUSWK.frontid.Equals("CHECK_OK"))
                    //{
                    //   output.frontid = "CHECK_AUSW_OK"; ;
                    //}
                    //else
                    //    output.frontid = outputRSAUSWK.frontid;
                    output.frontid = "CHECK_AUSW_OK";
                }
                outputGUI.KOMBI1 = new Cic.OpenOne.GateBANKNOW.Common.DTO.KOMBI1();
                outputGUI.KOMBI1.Anzahlung = eLIndto.anzahlung;
                outputGUI.KOMBI1.Laufzeit = eLIndto.laufzeit;
                outputGUI.KOMBI1.Restwert = eLIndto.restwert;
                outputGUI.KOMBI1.Rate = (double)eLIndto.rate;
                outputGUI.KOMBI1.Buchwert = elOutDto.SUMME_BW;
                outputGUI.KOMBI1.TR_Risk = elOutDto.TR;
                outputGUI.KOMBI1.eLOutDto = elOutDto;


            }
            if (output.frontid == "CHECK_OK" || output.frontid == "CHECK_AUSW_OK")
            {
                output.frontid = output.frontid;
            }
            else
            {
                if (kalkVariante)
                {

                    //Finanzierungsvorschlag 1: Anpassung der min. erforderlichen Anzahlung
                    ELOutDto elOutDtoGUI1 = new ELOutDto();
                    AngAntKalkDto angAntKalkDtoV1 = getV1(eLIndtoOrg, inputantrag, faktoren, clusterParam, sysPEROLE, isoCode, ref elOutDtoGUI1, prodKontext, kkontext, ust, ref cluster);
                    if (angAntKalkDtoV1 != null)
                    {
                        _log.Info("Variante 1 vor Prüfung: " + XMLSerializer.SerializeUTF8WithoutNamespace(angAntKalkDtoV1));
                        ELInDto trprüfungV1 = new ELInDto();
                        trprüfungV1.anzahlung = angAntKalkDtoV1.szBrutto;
                        trprüfungV1.laufzeit = angAntKalkDtoV1.lz;
                        trprüfungV1.restwert = angAntKalkDtoV1.rwBrutto;
                        trprüfungV1.rate = angAntKalkDtoV1.rateBrutto;
                        ocheckAntAngDto outDtoPrüfungV1 = new ocheckAntAngDto();
                        transaktionsrisikoPrüfung(ref outDtoPrüfungV1, isoCode, true, false, cluster, clusterParam, flags4TR, prodKontext, inputantrag, trprüfungV1, b2b);
                        if (outDtoPrüfungV1.status != ocheckAntAngDto.STATUS_GREEN)
                        {
                            _log.Info("Variante 1 Prüfung  : " + outDtoPrüfungV1.status + XMLSerializer.SerializeUTF8WithoutNamespace(outDtoPrüfungV1));
                            angAntKalkDtoV1 = null;
                        }
                    }
                    if (angAntKalkDtoV1 != null)
                    {
                        _log.Info("Variante 1 nach Prüfung  : " + XMLSerializer.SerializeUTF8WithoutNamespace(angAntKalkDtoV1));
                        outputGUI.variante1 = new Cic.OpenOne.GateBANKNOW.Common.DTO.Variante1();
                        outputGUI.variante1.Anzahlung = angAntKalkDtoV1.sz;
                        outputGUI.variante1.Laufzeit = angAntKalkDtoV1.lz;
                        outputGUI.variante1.Restwert = angAntKalkDtoV1.rw;
                        outputGUI.variante1.Rate = angAntKalkDtoV1.rate;
                        outputGUI.variante1.Buchwert = elOutDtoGUI1.SUMME_BW;
                        outputGUI.variante1.TR_Risk = elOutDtoGUI1.TR;
                        outputGUI.variante1.eLOutDto = elOutDtoGUI1;
                    }


                    //Finanzierungsvorschlag 2: Beibehaltung der erfassten Monatsrate
                    ELOutDto elOutDtoGUI2 = new ELOutDto();
                    AngAntKalkDto angAntKalkDtoV2 = getlzV2(eLIndtoOrg, inputantrag, faktoren, clusterParam, flags4TR, sysPEROLE, isoCode, ref elOutDtoGUI2, prodKontext, kkontext, ust, ref cluster);
                    if (angAntKalkDtoV2 != null)
                    {
                        _log.Info("Variante 2 vor Prüfung: " + XMLSerializer.SerializeUTF8WithoutNamespace(angAntKalkDtoV2));
                        ELInDto trprüfungV2 = new ELInDto();
                        trprüfungV2.anzahlung = angAntKalkDtoV2.szBrutto;
                        trprüfungV2.laufzeit = angAntKalkDtoV2.lz;
                        trprüfungV2.restwert = angAntKalkDtoV2.rwBrutto;
                        trprüfungV2.rate = angAntKalkDtoV2.rateBrutto;
                        ocheckAntAngDto outDtoPrüfungV2 = new ocheckAntAngDto();
                        transaktionsrisikoPrüfung(ref outDtoPrüfungV2, isoCode, true, false, cluster, clusterParam, flags4TR, prodKontext, inputantrag, trprüfungV2, b2b);
                        if (outDtoPrüfungV2.status != ocheckAntAngDto.STATUS_GREEN)
                        {
                            _log.Info("Variante 2 Prüfung  : " + outDtoPrüfungV2.status + XMLSerializer.SerializeUTF8WithoutNamespace(outDtoPrüfungV2));
                            angAntKalkDtoV2 = null;
                        }
                    }
                    if (angAntKalkDtoV2 != null)
                    {
                        _log.Info("Variante 2 nach Prüfung  : " + XMLSerializer.SerializeUTF8WithoutNamespace(angAntKalkDtoV2));
                        outputGUI.variante2 = new Cic.OpenOne.GateBANKNOW.Common.DTO.Variante2();
                        outputGUI.variante2.Anzahlung = angAntKalkDtoV2.sz;
                        outputGUI.variante2.Laufzeit = angAntKalkDtoV2.lz;
                        outputGUI.variante2.Restwert = angAntKalkDtoV2.rw;
                        outputGUI.variante2.Rate = angAntKalkDtoV2.rate;
                        outputGUI.variante2.Buchwert = elOutDtoGUI2.SUMME_BW;
                        outputGUI.variante2.TR_Risk = elOutDtoGUI2.TR;
                        outputGUI.variante2.eLOutDto = elOutDtoGUI2; 
                    }

                    if (simulationDE)
                    {
                        if (angAntKalkDtoV1 != null)
                        {
                            irisikoSimDto inputV1 = new irisikoSimDto();
                            inputV1.antrag = inputantrag;
                            inputV1.eloutDto = elOutDtoGUI1;
                            inputV1.angAntKalkDto = angAntKalkDtoV1;
                            inputV1.inputDE = inputDE;
                            orisikoSimDto outputRS1 = risikoSim(inputV1);
                            if (outputRS1.frontid.Equals("CHECK_DE_OK") || outputRS1.frontid.Equals("CHECK_DE_NOK"))
                            {
                                trDao.saveVariante(inputantrag.sysid, uKalk, (int)AntragVarianten.Ursprungskalkulation, null);
                                trDao.saveVariante(input.antrag.sysid, angAntKalkDtoV1, (int)AntragVarianten.Mindestanzahlung, outputRS1.simulationDERules);
                                inputantrag.kalkulation.angAntKalkVar1Dto = angAntKalkDtoV1;
                                output.frontid = outputRS1.frontid;
                            }
                            else
                            {
                                output.frontid = outputRS1.frontid;
                            }
                        }

                        if (angAntKalkDtoV2 != null)
                        {
                            irisikoSimDto inputV2 = new irisikoSimDto();
                            inputV2.antrag = input.antrag;
                            inputV2.eloutDto = elOutDtoGUI2;
                            inputV2.angAntKalkDto = angAntKalkDtoV2;
                            inputV2.inputDE = inputDE;
                            orisikoSimDto outputRS2 = risikoSim(inputV2);
                            if (outputRS2.frontid.Equals("CHECK_DE_OK") || outputRS2.frontid.Equals("CHECK_DE_NOK"))
                            {
                                trDao.saveVariante(inputantrag.sysid, uKalk, (int)AntragVarianten.Ursprungskalkulation, null);
                                trDao.saveVariante(input.antrag.sysid, angAntKalkDtoV2, (int)AntragVarianten.GleichbleibendeRate, outputRS2.simulationDERules);
                                inputantrag.kalkulation.angAntKalkVar2Dto = angAntKalkDtoV2;
                                output.frontid = (outputRS2.frontid);
                            }
                            else
                            {
                                output.frontid = outputRS2.frontid;
                            }
                        }

                    }
                    else
                    {
                        output.frontid = "CHECK_DE_NOK";
                    }
                }


            }

            output.antrag = inputantrag;
            String outputGUIString = XMLSerializer.SerializeUTF8WithoutNamespace(outputGUI);
            _log.Info("OUTPUTTR: " + XMLSerializer.SerializeUTF8WithoutNamespace(outputGUI));
            this.trDao.saveTROutput(input.antrag.sysid, outputGUIString);

            return normalizeOutput(output);
        }


        #endregion TESTGUI

        #region private Methoden


        /// <summary>
        /// Anpassung der min. erforderlichen Anzahlung
        /// </summary>
        /// <param name="eLIndto"></param>
        /// <param name="faktoren"></param>
        /// <param name="clusterParam"></param>
        /// <returns></returns>
        private double getMinimalErfordelicheAnzahlung(ELInDto eLIndto, FaktorenDto faktoren, VClusterParamDto clusterParam, ref ELOutDto elOutDtoGUI, long sysPEROLE, KalkulationDto kalkulation, prKontextDto prodKontext, kalkKontext kkontext, double ust, string isoCode)
        {
            bool kondition = false;
            bool kondition1 = false;
            bool kondition2 = false;
            bool kondition3 = false;
            bool timeOut = false;
            const int TRTIMEOUT = 59;

            ELInDto eLIndtoNew = Mapper.Map<ELInDto, ELInDto>(eLIndto);


            // Ausfallwahrscheinlichkeit = 1/(1+EXP(-(Scorekartenabhängiger_Kalibrierungswert+ Scorekartenabhängiger_Kalibrierungsfaktor*Scorewert)))//
            double ausfallwahrscheinlichkeitP = (1 / (1 + (double)Math.Pow(2.7182818285, (-(faktoren.kalibrierungswert + faktoren.kalibrierungsfaktor * eLIndto.scorewert))))) * 100;
            double minanzahlung = 0;
            double initstart = DateTime.Now.TimeOfDay.TotalMilliseconds;
            int timeOutspan =MyGetTimeOutValue("TRTIMEOUT", TRTIMEOUT);

            while (!(kondition && kondition1 && kondition2 && kondition3) && eLIndtoNew.anzahlung < eLIndtoNew.barkaufpreis && !timeOut)
            {
                double start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                ELOutDto elOutDto = new ELOutDto();

                elOutDto = getEL(eLIndtoNew, faktoren, ausfallwahrscheinlichkeitP);
                kondition = elOutDto.TRP * 100 <= clusterParam.v_el_prozent.maxvaln;
                _log.Debug("TRP = "+elOutDto.TRP + "clusterParam.v_el_prozent.maxvaln" + clusterParam.v_el_prozent.maxvaln);
                //TR in % * (BKP – x)=clusterabhängiger EL_BETRAG  
                kondition1 = elOutDto.TR <= clusterParam.v_el_betrag.maxvaln;
                //COST Korrektur für die Profitabilität
                kondition2 = elOutDto.profitabilitaetp >= clusterParam.v_prof.minvaln;
                _log.Debug("profitabilitaetp= " + elOutDto.profitabilitaetp + "clusterParam.v_prof.minvaln" + clusterParam.v_prof.minvaln);
                kondition3 = (eLIndtoNew.anzahlung / eLIndtoNew.barkaufpreis >= 0) && (eLIndtoNew.anzahlung / eLIndtoNew.barkaufpreis <= 50);
                _log.Debug("eLIndtoNew.anzahlung= " + eLIndtoNew.anzahlung + "eLIndtoNew.barkaufpreisn" + eLIndtoNew.barkaufpreis);
                minanzahlung = eLIndtoNew.anzahlung;
                eLIndtoNew.anzahlung = minanzahlung + 500;
                AngAntKalkDto angAntKalkDto = calculateMitNeuAnzahlung(eLIndtoNew.anzahlung, sysPEROLE, kalkulation, prodKontext, kkontext, ust, isoCode);
                eLIndtoNew = getELIndtoNachKalkulation(eLIndto, angAntKalkDto);


                _log.Debug("Minimal erfordeliche Anzahlung = " + minanzahlung + "TR =" + elOutDto.TR + "clusterabhängiger EL_BETRAG   =  " + clusterParam.v_el_betrag.maxvaln);
                _log.Debug("Duration A: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                start = DateTime.Now.TimeOfDay.TotalMilliseconds;

                timeOut = ((start - initstart) / 1000 > timeOutspan);
                if (timeOut)
                {
                    throw new Exception("TimeOut bei getMinimalErfordelicheAnzahlung");
                }

                elOutDtoGUI = Mapper.Map<ELOutDto, ELOutDto>(elOutDto);
            }

            
            return minanzahlung;
        }





        /// <summary>
        /// getEL
        /// </summary>
        /// <param name="eLInDtoOrg"></param>
        /// <param name="faktoren"></param>
        /// <param name="ausfallwahrscheinlichkeitP"></param>
        /// <returns></returns>
        private ELOutDto getEL(ELInDto eLInDtoOrg, FaktorenDto faktoren, double ausfallwahrscheinlichkeitP)
        {


            ELInDto eLIndto = Mapper.Map<ELInDto, ELInDto>(eLInDtoOrg);

            ELOutDto eLOutDto = new ELOutDto();


            //BUCHWERTE
            BWInDto bwInDto = myMappingBWInDto(eLIndto);
            BWOutDto buchwerteOutDto = new BWOutDto();
            buchwerteOutDto = getBuchwerte(bwInDto);
            double buchwerte = buchwerteOutDto.buchwerte;

            double buchwerteadd = getBuchwerteadd(bwInDto);
            //MARKTWERTE
            double marktwerte = getMarktwerte(eLIndto);
            //zum Testen:
            //double buchwerte = 1338244.82;
            //1323793.6900000002
            //double marktwerte = 1128400.45; 

            double BKPFaktor = 0;
            double LZFaktor = 0;
            double ALTERFaktor = 0;

            if (eLIndto.sysvg == 0)
            {
                eLIndto.sysvg = 201;
            }

            // BKP Faktor 	(<<B>>/(Anzahlung/Barkaufspreis) 

            if (eLIndto.vgName == "STANDARD")

            {
                if (eLIndto.anzahlung == 0 || (eLIndto.anzahlung / eLIndto.barkaufpreis) <= 0.05)
                {
                    BKPFaktor = 2;
                }
                else
                {
                    BKPFaktor = Math.Pow((double)faktoren.bkp_faktor / (eLIndto.anzahlung / eLIndto.barkaufpreis), (double)faktoren.pbkp_faktor);
                }
            }
            else
                if (eLIndto.anzahlung == 0 || (eLIndto.anzahlung / eLIndto.barkaufpreis) <= 0.05)
                {
                    BKPFaktor = 3;
                }
                else
                {
                    BKPFaktor = Math.Pow((double)faktoren.bkp_faktor / (eLIndto.anzahlung / eLIndto.barkaufpreis), (double)faktoren.pbkp_faktor);
                }

            LZFaktor = Math.Pow((eLIndto.laufzeit / faktoren.lz_faktor), faktoren.plz_faktor);
            if (LZFaktor < 0.3)
            {
                LZFaktor = 0.3;
            }

            //  Alter Faktor	(Alter/<<C>>)

            if (faktoren.alter_faktor > 0)
            {
                ALTERFaktor = Math.Pow((eLIndto.alter_Fhz_Mt / faktoren.alter_faktor), faktoren.palter_faktor);
            }
            if (ALTERFaktor < 0.5)
            {
                ALTERFaktor = 1;
            }

            double Kalibrierungswert = Math.Round(LZFaktor, 2) * Math.Round(BKPFaktor, 2) * Math.Round(ALTERFaktor, 2);

            /*LGD in %:
            (((Summe aller Buchwerte * LZ Faktor * BKP Faktor * Alter Faktor* (1+MwSt))
            - (Summe aller Marktwerte * 0.9))
            / (Laufzeit-2))/ Barkaufpreis-Anzahlung */
            // double MW_Kons = 0.9;
            double lgd_CH = Math.Round(((buchwerte * Kalibrierungswert) - (marktwerte)) / (eLIndto.laufzeit - 2), 2);
            // LGD in %:
            double lgdP = (lgd_CH / (eLIndto.barkaufpreis - eLIndto.anzahlung)) * 100;


            // round((Antrags_Daten.Nom_Zins/12*EL_Grunddaten.BW_Summe_Profitabilität*1.08),2) Zinsertrag
            eLIndto.zinsertrag = Math.Round(eLIndto.zins / 100.0 / 12.0 * (buchwerte + buchwerteadd), 2);

            //Berechnung: Minimiere TR in %
            //TRP   EL_in_P,  
            double trp = ausfallwahrscheinlichkeitP * lgdP / 100;

            double kalitr = 0;
            double kalitrCH = 0;


            double? TR4Scorekarte = null;


            if (faktoren.scoreKALTRFormel != "")
            {
                //EvalTR4Scorekarte
                TR4Scorekarte = EvalTR4ScorekarteDB(faktoren.scorekarte, faktoren.scoreKALTRFormel, eLIndto.barkaufpreis, eLIndto.anzahlung, lgd_CH, faktoren.OVERALLint, faktoren.OVERALLSlope, 0, eLIndto.sysid);
            }



            if (TR4Scorekarte == null)
            {
                if (trp > 0)
                {
                    kalitr = faktoren.OVERALLint + faktoren.OVERALLSlope * Math.Sqrt(trp / 100);
                }

                if (kalitr < 0.002)
                {
                    kalitr = 0.002;
                }

                kalitrCH = kalitr * (eLIndto.barkaufpreis - eLIndto.anzahlung);
            }
            else
            {
                kalitrCH = (double)TR4Scorekarte;
                kalitr = kalitrCH / (eLIndto.barkaufpreis - eLIndto.anzahlung);

            }


            //PROFITABILITAET %
            double profitabilitaetP = ((eLIndto.zinsertrag - kalitrCH - faktoren.cost) / (eLIndto.barkaufpreis - eLIndto.anzahlung)) * 100;

            eLOutDto.LGDp = lgdP;
            eLOutDto.LGD = lgd_CH;
            eLOutDto.ausfallwahrscheinlichkeitP = ausfallwahrscheinlichkeitP;
            eLOutDto.profitabilitaetp = profitabilitaetP;
            eLOutDto.TR = kalitrCH;
            eLOutDto.TRP = kalitr;

            eLOutDto.SUMME_BW = buchwerte;
            eLOutDto.SUMME_MW = marktwerte;
            eLOutDto.LZ_FAKTOR = LZFaktor;
            eLOutDto.BKP_FAKTOR = BKPFaktor;
            eLOutDto.ALTER_FAKTOR = ALTERFaktor;

            eLOutDto.INTPUTBUCHWERTE = bwInDto;
            eLOutDto.OUTPUTBUCHWERTE = buchwerteOutDto;

            return eLOutDto;
        }


        /// <summary>
        /// Buchwerte Kalkulation
        /// </summary>
        /// <param name="bwInDto">bwInDto</param>
        /// <returns></returns>
        public BWOutDto getBuchwerteDeprecated(BWInDto bwInDto)
        {
            BWOutDto bwOutDto = new BWOutDto();

            IMwStBo mwstBo = new MwStBo(mwStDao);
            double ust = 0;
            if (bwInDto.mwst == null)
            {
                ust = mwstBo.getMehrwertSteuer(1, bwInDto.sysvart, DateTime.Now);
            }
            else
            {
                ust = (double)bwInDto.mwst;
            }

            double barkaufpreis = bwInDto.barkaufpreis;
            double anzahlung = bwInDto.anzahlung;

            double barwert = -(barkaufpreis - anzahlung);
            double restwert = bwInDto.restwert;
            double zins_nom = bwInDto.zins / 100;
            double zinsa;
            double tilgunga = anzahlung;
            double rate1 = anzahlung;

            double restschuld = barkaufpreis;
            double rate = (double)bwInDto.rate;
            restschuld -= rate1;
            zinsa = restschuld * zins_nom / 12;
            double tilgung1 = rate1 - zinsa;



            tilgunga = rate - zinsa;
            double tempTilgung = tilgunga;

            double[] bwarray = new double[bwInDto.laufzeit];
            double[] bwarrayround = new double[bwInDto.laufzeit];
            double[] tilgung = new double[bwInDto.laufzeit];
            double[] tilgunground = new double[bwInDto.laufzeit];
            tilgung[0] = tilgung1;

            double bwnet = (barkaufpreis - tilgung1) / (1 + ((double)bwInDto.mwst / 100));
            bwarray[0] = Math.Round(bwnet, 2);
            double sum = 0;

            sum = bwarray[0];

            for (int i = 0; i < bwInDto.laufzeit - 1; i++)
            {
                restschuld -= tilgunga;
                zinsa = restschuld * zins_nom * 30 / 360;
                tilgunga = rate - zinsa;
                tilgung[i + 1] = tilgunga;

                bwarray[i + 1] = Math.Round(bwarray[i] - (tilgung[i + 1] / (1 + ((double)bwInDto.mwst / 100))), 2);
                sum += Math.Round(bwarray[i + 1], 2);


            }

            bwOutDto.bwarray = bwarray;
            bwOutDto.BuchwerteListe = MyArraytoSting(bwarray);
            bwOutDto.tilgung = tilgung;
            bwOutDto.TilgungListe = MyArraytoSting(tilgung);
            bwOutDto.buchwerte = sum;
            return bwOutDto;

        }




        public double getBuchwerteadd(BWInDto bwInDtoBrutto)
        {
            double bw = 0;

            BWInDto bwInDto = new BWInDto();
            double MWST = 1.08;

            IMwStBo mwstBo = new MwStBo(mwStDao);
            if (bwInDtoBrutto.mwst == null)
            {
                MWST = 1 + ((double)bwInDtoBrutto.mwst / 100);
            }

            bwInDto.anzahlung = bwInDtoBrutto.anzahlung / MWST;
            bwInDto.barkaufpreis = bwInDtoBrutto.barkaufpreis / MWST;
            bwInDto.zins = bwInDtoBrutto.zins;
            bwInDto.laufzeit = bwInDtoBrutto.laufzeit;
            bwInDto.rate = bwInDtoBrutto.rate;

            double RATE = 0;
            bwInDto.restwert = bwInDtoBrutto.restwert / MWST;

            if (bwInDto.anzahlung > 0)
            {
                bw = -Kalkulator.calcPREFV(-bwInDto.barkaufpreis, 1, bwInDto.zins / 1200.0, bwInDto.anzahlung);
                bwInDto.barkaufpreis = bw;

                RATE = Math.Round(Kalkulator.calcpmt(-bwInDto.barkaufpreis, bwInDto.laufzeit - 1, bwInDto.zins / 1200, bwInDto.restwert, true), 5);
            }
            else
            {
                RATE = Math.Round(Kalkulator.calcpmt(-bwInDto.barkaufpreis, bwInDto.laufzeit, bwInDto.zins / 1200, bwInDto.restwert, true), 5);
                bw = -Kalkulator.calcPREFV(-bwInDto.barkaufpreis, 1, bwInDto.zins / 1200.0, RATE);

                bwInDto.barkaufpreis = bw;

            }
            bw += Kalkulator.calcBARW(RATE, bwInDto.zins / 12.0, bwInDto.laufzeit - 2, bwInDto.restwert, true);

            return bw * 1.08;
        }


        public BWOutDto getBuchwerte(BWInDto bwInDtoBrutto)
        {
            BWOutDto bwOutDto = new BWOutDto();
            BWInDto bwInDto = new BWInDto();
            double MWST = 1.08;

            IMwStBo mwstBo = new MwStBo(mwStDao);
            if (bwInDtoBrutto.mwst == null)
            {
                MWST = 1 + ((double)bwInDtoBrutto.mwst / 100);
            }

            bwInDto.anzahlung = bwInDtoBrutto.anzahlung / MWST;
            bwInDto.barkaufpreis = bwInDtoBrutto.barkaufpreis / MWST;
            bwInDto.zins = bwInDtoBrutto.zins;
            bwInDto.laufzeit = bwInDtoBrutto.laufzeit;
            bwInDto.rate = bwInDtoBrutto.rate;
            bwInDto.restwert = bwInDtoBrutto.restwert / MWST;



            double barkaufpreis = bwInDto.barkaufpreis;
            double[] bwarray = new double[bwInDto.laufzeit + 2];
            double[] bwarraynetto = new double[bwInDto.laufzeit + 2];
            double[] bwarraybrutto = new double[bwInDto.laufzeit + 2];
            double[] tilgung = new double[bwInDto.laufzeit + 2];
            double[] tilgunground = new double[bwInDto.laufzeit + 2];
            double ersterate = 0;
            double smbw = 0;
            double RATE = 0;
            

            if (bwInDto.anzahlung > 0)
            {
                ersterate = bwInDto.anzahlung;
            }
            else
            {
                ersterate = (double)bwInDto.rate;
            }

            if (bwInDto.anzahlung > 0)
            {

                //dblBarkaufpreisNeu = Round(FV(dblNomZins / 12, 1, dblAnzahlung, -(dblBarkaufpreis), 1), 2)
                //bwInDto.barkaufpreis = Math.Round(Kalkulator.calcENDW(-bwInDto.barkaufpreis, bwInDto.anzahlung, bwInDto.zins / 1200, 1, true), 2);
                bwInDto.barkaufpreis = -Kalkulator.calcPREFV(-bwInDto.barkaufpreis, 1, bwInDto.zins / 1200.0, bwInDto.anzahlung);



                //dblRate = Round(Pmt(dblNomZins / 12, GLZ - 1, -(dblBarkaufpreisNeu), dblRestwert, 1) * MwSt, 2)
                RATE = Math.Round(Kalkulator.calcpmt(-bwInDto.barkaufpreis, bwInDto.laufzeit - 1, bwInDto.zins / 1200, bwInDto.restwert, true), 2);

                //Sheet6.Cells(2, 2) = Round((Sheet3.Range("Barkaufpreis").Value / MwSt) - (dblBarkaufpreisNeu), 2)
                tilgung[2] = Math.Round(barkaufpreis - bwInDto.barkaufpreis, 2);

                // For i = 3 To GLZ
                for (int i = 3; i < bwInDto.laufzeit + 1; i++)
                {

                    //Sheet6.Cells(i, 2) = Round(PPmt(dblNomZins / 12, Sheet6.Cells(i, 1), GLZ - 1, -(dblBarkaufpreisNeu), dblRestwert, 1), 2)
                    tilgung[i] = Math.Round(Kalkulator.calcPPMT(bwInDto.zins / 1200, i, bwInDto.laufzeit - 1, bwInDto.barkaufpreis, bwInDto.restwert, true), 2);
                    bwarray[i] = Math.Round(Kalkulator.calcBARW(RATE, bwInDto.zins / 12, bwInDto.laufzeit - i, bwInDto.restwert, true), 2);
                    smbw += bwarray[i];

                }
                smbw = smbw - bwarray[0] - bwarray[1];
                bwOutDto.bwarray = bwarray;
                bwOutDto.BuchwerteListe = MyArraytoSting(bwOutDto.bwarray);
                bwOutDto.tilgung = tilgung;
                bwOutDto.TilgungListe = MyArraytoSting(tilgung);
                bwOutDto.buchwerte = smbw;
            }

            else
            {


                RATE = Math.Round(Kalkulator.calcpmt(-bwInDto.barkaufpreis, bwInDto.laufzeit, bwInDto.zins / 1200, bwInDto.restwert, true), 5);
                bwInDto.barkaufpreis = Math.Round(Kalkulator.calcENDW(-bwInDto.barkaufpreis, RATE, bwInDto.zins / 1200, 0, true), 5);
                tilgung[1] = 0;
                for (int i = 2; i <= bwInDto.laufzeit + 1; i++)
                {
                    tilgung[i] = Math.Round(Kalkulator.calcPPMT(bwInDto.zins / 1200, i + 1, bwInDto.laufzeit, bwInDto.barkaufpreis, bwInDto.restwert, true), 2);
                    bwarray[i] = Kalkulator.calcBARW(RATE, bwInDto.zins / 12, bwInDto.laufzeit - i, bwInDto.restwert, true);
                    bwarraynetto[i] = Math.Round(bwarray[i] - (tilgung[i]), 5);
                    bwarraybrutto[i] = bwarraynetto[i] * MWST;
                    smbw += bwarraynetto[i];

                }
                smbw = smbw - bwarraynetto[1] - bwarraynetto[0] - bwarraynetto[2] - bwarraynetto[3];
                bwOutDto.bwarray = bwarraynetto;
                bwOutDto.BuchwerteListe = MyArraytoSting(bwOutDto.bwarray);
                bwOutDto.tilgung = tilgung;
                bwOutDto.TilgungListe = MyArraytoSting(tilgung);
                bwOutDto.buchwerte = smbw;
            }

            if (bwInDtoBrutto.mwst != null)
            {
                MWST = 1 + ((double)bwInDtoBrutto.mwst / 100);
            }
            MWST = 1.08;
            bwOutDto.buchwerte = bwOutDto.buchwerte * MWST;
            return bwOutDto;
        }

        /// <summary>
        /// getMarktwerte
        /// </summary>
        /// <param name="eLIndto"></param>
        /// <returns></returns>
        public double getMarktwerte(ELInDto eLIndto) 
        {
            IPrismaDao prismaDao = PrismaDaoFactory.getInstance().getPrismaDao();
            IMwStBo mwstBo = new MwStBo(mwStDao);
            bool isDiffLeasing = false;
            double ust = 0;
            IRounding round = RoundingFactory.createRounding();

            List<EurotaxOutDto> outDtoList = new List<EurotaxOutDto>();

            List<EurotaxOutDto> outDtoListforEL = new List<EurotaxOutDto>();

            List<InsertParamDto> InsertParamListe = new List<InsertParamDto>();

            double barkaufpreis = eLIndto.barkaufpreis;
            double neupreis = eLIndto.barkaufpreis;
            int laufzeit = eLIndto.laufzeit;
            double RWBrutto = eLIndto.restwert;
            double RWDelta = 0;

            if (eLIndto.erstzulassung > DateTime.Today)
            {
                eLIndto.erstzulassung = DateTime.Today;
            }

            //BNRSIZE - 1145  neupreis4DoRemoDefault LP wird neu immer aus der gleichen Quelle kommen wie die Restwertverläufe.
            outDtoList = getEurotaxOutList(eLIndto.sysobtyp, eLIndto.laufzeit, eLIndto.neupreis4DoRemo,eLIndto.neupreis4DoRemoDefault,eLIndto.neupreis4DoRemoIW, eLIndto.neupreis4DoRemoVGREF, eLIndto.kmStand, eLIndto.zubehoer, eLIndto.erstzulassung, eLIndto.schwacke, eLIndto.jahresKm);

            double neupreis4DoRemo = 0;
            decimal lp_prozent = 0;
            double listenpreis = 0;
     

            EurotaxSource source = EurotaxSource.EurotaxForecast;
            EurotaxSource sourceEL;
            if (outDtoList.Count() > 0)
            {
                source = outDtoList[0].source;

            }
            if (outDtoListforEL.Count() > 0)
            {
                sourceEL = outDtoListforEL[0].source;
            }



            if (source == EurotaxSource.EurotaxForecast)
            {
                neupreis4DoRemo = eLIndto.neupreis4DoRemo;
                lp_prozent = eLIndto.lp_prozent;
                listenpreis = eLIndto.listenpreis4DoRemo;
            }

            if (source == EurotaxSource.InternalTableRW)
            {
                neupreis4DoRemo = eLIndto.neupreis4DoRemoIW;
                lp_prozent = eLIndto.lp_prozentIW;
                listenpreis = eLIndto.listenpreis4DoRemoIW;
            }

            if (source == EurotaxSource.InternalTableVGREF_RW)
            {
                neupreis4DoRemo = eLIndto.neupreis4DoRemoVGREF;
                lp_prozent = eLIndto.lp_prozentVGREF;
                listenpreis = eLIndto.listenpreis4DoRemoVGREF;
            }

            if (source == EurotaxSource.InternalTableRemo)
            {
                neupreis4DoRemo = eLIndto.neupreis4DoRemoDefault;
                lp_prozent = eLIndto.lp_prozent;
                listenpreis = eLIndto.listenpreis4DoRemoDefault;
            }

            if (neupreis4DoRemo > 0 && outDtoList.Count() > 0)
            {
                RWBrutto = eLIndto.restwert;
            }
            if (RWBrutto >= 0)
            {
                    RWDelta = RWBrutto - outDtoList[outDtoList.Count() - 1].RetailValueInPercentage / 100 * neupreis4DoRemo;
            }
            

            double rapzinssumme = outDtoList[outDtoList.Count() - 1].RetailValueInPercentage / 100 * neupreis4DoRemo;
            double marktwert = 0;
            double[] actuell_rw = new double[outDtoList.Count() + 2];
            double[] actuell_rw_orig = new double[outDtoList.Count() + 2];

            double[] pmt = new double[outDtoList.Count() + 2];
            double[] pmt1 = new double[outDtoList.Count() + 2];
            double[] tilgung = new double[outDtoList.Count() + 2];
            double[] fvliste = new double[outDtoList.Count() + 2];

            int rang = 1;
            int loop = 1;
            double fv = 0;
            double fvnetto = 0;

            double zins = eLIndto.zins;
            double zins_nom = eLIndto.zins / 100;
            double zinsmon = zins_nom / 12;
          
            
            double ersterate = 0;
            if (eLIndto.anzahlung > 0)
            {
                ersterate = eLIndto.anzahlung;
            }
            else
            {
                ersterate = (double)eLIndto.rate;
            }

            //BNR13 BNRDR-1981 
            //wurde in der Vorlage-Kalkulation (Excel-Kalkulator LEASE-now, Zelle C39) die Rundung des Buchwerts(Zukunftswert) explizit angefordert. 
            //Bitte das bei der REMO-Kalkulation analog gestalten.
            //[Bisherige DOREMO-Funktion hat offensichtlich an der gleichen Stelle das gleiches Problem gehabt, das bisher nicht entdeckt wurde]
            String vartCode = trDao.getCodeAusVart(eLIndto.sysvart);
            if (eLIndto.sysCreate == null)
            {
                ust = mwstBo.getMehrwertSteuer(1, eLIndto.sysvart, DateTime.Now);
            }
            else
            {
                ust = mwstBo.getMehrwertSteuer(1, eLIndto.sysvart, (DateTime)eLIndto.sysCreate);
            }

            if (eLIndto.sysprproduct > 0)
            {
                isDiffLeasing = prismaDao.isDiffLeasing(eLIndto.sysprproduct);
            }

            if (isDiffLeasing)
            {
                zins = eLIndto.zinscust;
                zins_nom = eLIndto.zinscust / 100;
                zinsmon = zins_nom / 12;
            }

            double ersteratenetto = round.getNetValue(ersterate, ust);
            double barkaufpreisnetto = round.getNetValue(barkaufpreis, ust);
            fvnetto = barkaufpreisnetto;
            if (ersteratenetto > 0)
            {
                // Buchwert nach 1.Monat
                if (isDiffLeasing)
                {
                    fvnetto = Kalkulator.calcENDW(barkaufpreisnetto, ersteratenetto, eLIndto.zinscust / 12, 1, true);
                }
                else
                {
                    fvnetto = System.Math.Round(Kalkulator.calcENDW(barkaufpreisnetto, ersteratenetto, eLIndto.zins / 12, 1, true) / 5, 2) * 5;
                }
            }
            double fvbruttoausNetto = round.getGrossValue(fvnetto, ust);
            fv = fvbruttoausNetto;

            IQuoteDao qd = OpenOne.Common.DAO.CommonDaoFactory.getInstance().getQuoteDao();
            // var für  Oracle Param 
            DateTime valuta = DateTime.Now;
            double zinsatz = 0;
            double betrag = 0;
            double betrag1 = 0;
            long rn = 0;
            long rnpos = 0;
            double faktor = 0;
            int FlagSR3 = 0;
            double betrag2 = 0;
            double Betrag3 = 0;
            double Betrag4 = 0;
            double Betrag5 = 0;
            double Betrag6 = 0;
            int FlagSr1 = 0;
            double Betrag7 = 0;
            int FlagSR2 = 0;
            double Betrag8 = 0;
            double Betrag9 = 0;
            double Betrag10 = 0,Betrag11=0,Betrag12=0;
            double zinsparam = 0;
            double betragINT = 0;
            double betragKORR = 0;
            double tilgungvirtual = 0;
            decimal zinsenvirtual = 0;
            double retailAmount = 0;
            double remoKurve = eLIndto.barkaufpreis;
            double diffchf = 0;
            double wertverlustproz = 0;
            
            double lastRwPercent = 1;//Neuwagen
            if (eLIndto.sysobart == 13)
                lastRwPercent = qd.getQuote("REMO_OCC_FAKTOR") * outDtoList[2].RetailValueInPercentage / 100.0;

            double totalWertverlust = lastRwPercent - outDtoList[outDtoList.Count() - 1].RetailValueInPercentage / 100;
            double anteilWertverlust = 0;
            double diffbkprwkalk = (eLIndto.barkaufpreis-eLIndto.restwert)/ eLIndto.barkaufpreis*100.0;
            double verteilungDifferenz = 0;

            foreach (EurotaxOutDto outDto in outDtoList)
            {
                


                //Alte Version fehlerhafter Verlauf
                actuell_rw_orig[loop] = outDto.RetailValueInPercentage / 100 * neupreis4DoRemo;
                actuell_rw[loop] = actuell_rw_orig[loop] + (double)rang / (double)laufzeit * RWDelta;

                //NEUER Removerlauf
                if (loop > 2)
                {
                    wertverlustproz = lastRwPercent - outDto.RetailValueInPercentage / 100.0;

                    lastRwPercent = outDto.RetailValueInPercentage / 100.0;
                    anteilWertverlust = wertverlustproz / totalWertverlust * 100.0;
                    verteilungDifferenz = diffbkprwkalk * anteilWertverlust / 100.0;
                    diffchf = eLIndto.barkaufpreis * verteilungDifferenz / 100.0;
                    remoKurve = remoKurve - diffchf;
                    actuell_rw[loop] = remoKurve;


                    if (actuell_rw[loop] / barkaufpreis * 100 > eLIndto.rwFaktor)
                    {
                        actuell_rw[loop] = eLIndto.rwFaktor * eLIndto.barkaufpreis / 100;
                    }

                    if (zins == 0)
                    {
                        pmt[loop] = round.RoundCHF((fv - actuell_rw[loop]) / (rang - 1));
                    }
                    else
                    {
                        //pmt[loop] = Kalkulator.calcPREPMT(-fv, rang - 1, zinsmon, actuell_rw[loop]);
                        pmt[loop] = Kalkulator.calcpmt(-fv, rang - 1, zinsmon, actuell_rw[loop], true);
                        //tilgung[loop] = Kalkulator.calcPPMT(zinsmon, 1, eLIndto.laufzeit, -actuell_rw[loop], fv, true);
                        pmt[loop] = round.RoundCHF(pmt[loop]);
                    }
                }


                if (eLIndto.saveMarktwerteInDb)
                {
                    if (loop > 2)
                    {
                        /* alte Version mit fehlerhaftem Verlauf
                        // var für  Oracle Param 
                        valuta = (eLIndto.erstzulassung != null) ? (DateTime)eLIndto.erstzulassung : DateTime.Now;
                        zinsatz = round.RoundCHF(actuell_rw[rang] / eLIndto.barkaufpreis * 100);

                        // EL Felder!!!

                        // EL Felder!!!
                        retailAmount = Math.Round(eLIndto.neupreis * (outDto.RetailValueInPercentage / 100), 2);
                        betrag = round.RoundCHF(retailAmount);
                        Betrag9 = round.RoundCHF(eLIndto.neupreis);
                        Betrag10 = round.RoundCHF(retailAmount * eLIndto.rwFaktor / 100.0);

                        // Remo Felder!!!
                        // BNRDR-1981

                        betrag1 = pmt[loop];
                        if (loop == outDtoList.Count())
                        {
                            betrag1 = (double)eLIndto.rate;
                        }
                        rn = eLIndto.sysid;
                        rnpos = eLIndto.sysob;
                        faktor = round.RoundCHF(pmt[loop] / eLIndto.ahkBrutto * 100);
                        FlagSR3 = loop;
                        betrag2 = outDto.RetailValueInPercentage;
                        Betrag3 = round.RoundCHF(RWDelta);
                        Betrag4 = round.RoundCHF(RWBrutto);
                        Betrag5 = barkaufpreis;
                        Betrag6 = neupreis4DoRemo;//BNRSIZE - 1145 
                        FlagSr1 = laufzeit;
                        Betrag7 = ersterate;
                        FlagSR2 = eLIndto.minLz;
                        Betrag8 = round.RoundCHF(actuell_rw[loop]);
                        betragKORR = listenpreis;//BNRSIZE - 1145 
                        tilgungvirtual = actuell_rw_orig[loop];
                        zinsenvirtual = lp_prozent; //BNRSIZE - 1145 

                        zinsparam = zins;
                        */
                        // var für Oracle Param
                        valuta = (eLIndto.erstzulassung != null) ? (DateTime)eLIndto.erstzulassung : DateTime.Now;
                        zinsatz = round.RoundCHF(actuell_rw[rang] / eLIndto.barkaufpreis * 100);

                        // EL Felder!!!

                        // EL Felder!!!
                        retailAmount = Math.Round(eLIndto.neupreis * (outDto.RetailValueInPercentage / 100), 2);
                        betrag = round.RoundCHF(retailAmount);
                        Betrag9 = round.RoundCHF(eLIndto.neupreis);
                        Betrag10 = round.RoundCHF(retailAmount * eLIndto.mwFaktor / 100.0);

                        // Remo Felder!!!
                        // BNRDR-1981

                        betrag1 = pmt[loop];
                        if (loop == outDtoList.Count())
                        { betrag1 = (double)eLIndto.rate; }

                        rn = eLIndto.sysid;
                        rnpos = eLIndto.sysob;
                        faktor = round.RoundCHF(pmt[loop] / eLIndto.ahkBrutto * 100);
                        FlagSR3 = loop;
                        betrag2 = outDto.RetailValueInPercentage;
                        Betrag3 = diffchf; // Differenz CHF. CR 479
                        Betrag4 = round.RoundCHF(RWBrutto);
                        Betrag5 = barkaufpreis;
                        Betrag6 = diffbkprwkalk;// Differenz BKP und RW Kalk %. CR 479
                        FlagSr1 = laufzeit;
                        Betrag7 = ersterate;
                        FlagSR2 = eLIndto.minLz;
                        Betrag8 = round.RoundCHF(actuell_rw[loop]);
                        betragKORR = wertverlustproz;// Wervertlust (Prozentpunkte). CR 479
                        tilgungvirtual = actuell_rw_orig[loop];
                        zinsenvirtual = (decimal) anteilWertverlust; // Anteil an Wertverlust. CR 479
                        Betrag11 = verteilungDifferenz; // Verteilung Differenz. CR 479
                        Betrag12 = totalWertverlust; // Total Wertverlust. CR 479

                        zinsparam = zins;
                    }
                    else
                    {

                        // var für  Oracle Param 
                        valuta = (eLIndto.erstzulassung != null) ? (DateTime)eLIndto.erstzulassung : DateTime.Now;
                        zinsatz = 0;
                        betrag = 0;
                        betrag1 = 0;
                        rn = eLIndto.sysid;
                        rnpos = eLIndto.sysob;
                        faktor = 0;
                        FlagSR3 = loop;
                        betrag2 = 0;
                        Betrag3 = 0;
                        Betrag4 = 0;
                        Betrag5 = barkaufpreis;
                        Betrag6 = neupreis4DoRemo;
                        FlagSr1 = laufzeit;
                        Betrag7 = 0;
                        FlagSR2 = eLIndto.minLz;
                        Betrag8 = 0;
                        Betrag10 = 0;
                        zinsparam = zins;
                        zinsenvirtual = 0;
                        Betrag11 = 0;
                        Betrag12 = 0;
                    }


                    // Insert Parameter für ANTOBSLPOS
                    InsertParamDto insert = new InsertParamDto();
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    insert.parameters = parameters;



                    //rang
                    insert.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "rang", Value = loop });

                    //anzahl
                    insert.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "anzahl", Value = 1 });

                    //DateTime Valuta = Erszulassung
                    insert.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "valuta", Value = valuta });

                    //_UPDATE('ANTOBSLPOS:Zinssatz',_QT('REMO','GET','F02',''))  
                    insert.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "zinssatz", Value = zinsatz });

                    //cicqt(l:qtName,'SET','F03',l:Neupreis * qListRestwert:RetailValueInPercentage / 100) !qListRestwert:RetailAmount
                    //_UPDATE('ANTOBSLPOS:Betrag',_QT('REMO','GET','F03',''),'0')
                    insert.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "betrag", Value = betrag });

                    //_UPDATE('ANTOBSLPOS:Betrag1',_QT('REMO','GET','F04',''),'0')
                    insert.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "betrag1", Value = betrag1 });

                    //_UPDATE('ANTOBSLPOS:RN',_QT('REMO','GET','F05',''),'0')
                    insert.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysrn", Value = rn });

                    //_UPDATE('ANTOBSLPOS:RNPOS',_QT('REMO','GET','F06',''),'0')
                    insert.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysrnpos", Value = rnpos });

                    //_UPDATE('ANTOBSLPOS:Faktor',_QT('REMO','GET','F04','')/ANTOB:AHKBRUTTO * 100)
                    insert.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "faktor", Value = faktor });

                    //_UPDATE('ANTOBSLPOS:FlagSR3',_QT('REMO','GET','F07',''),'0')
                    insert.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "flagSR3", Value = FlagSR3 });

                    //_UPDATE('ANTOBSLPOS:Betrag2',_QT('REMO','GET','F08',''),'0')
                    insert.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "betrag2", Value = betrag2 });

                    //_UPDATE('ANTOBSLPOS:Betrag3',_QT('REMO','GET','F09',''),'0')
                    insert.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "betrag3", Value = Betrag3 });

                    //_UPDATE('ANTOBSLPOS:Betrag4',_QT('REMO','GET','F10',''),'0')
                    insert.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "betrag4", Value = Betrag4 });

                    //_UPDATE('ANTOBSLPOS:Betrag5',_QT('REMO','GET','F11',''),'0')
                    insert.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "betrag5", Value = Betrag5 });

                    //_UPDATE('ANTOBSLPOS:Betrag6',_QT('REMO','GET','F12',''),'0')
                    insert.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "betrag6", Value = Betrag6 });

                    //_UPDATE('ANTOBSLPOS:Zins',_QT('REMO','GET','F13',''),'0')
                    insert.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "zins", Value = zinsparam });

                    //_UPDATE('ANTOBSLPOS:FlagSR1',_QT('REMO','GET','F14',''),'0')
                    insert.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "flagSR1", Value = FlagSr1 });

                    //_UPDATE('ANTOBSLPOS:FlagSR2',_QT('REMO','GET','F15',''),'0')
                    insert.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "flagSR2", Value = FlagSR2 });

                    //__UPDATE('ANTOBSLPOS:Betrag7',_QT('REMO','GET','F16',''),'0')
                    insert.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "betrag7", Value = Betrag7 });

                    //__UPDATE('ANTOBSLPOS:Betrag8',_QT('REMO','GET','F17',''),'0')
                    insert.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "betrag8", Value = Betrag8 });

                    //__UPDATE('ANTOBSLPOS:Betrag9',_QT('REMO','GET','F20',''),'0')    /*Neupreis original*/
                    insert.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "betrag9", Value = Betrag9 });

                    //__UPDATE('ANTOBSLPOS:Betrag10',_QT('REMO','GET','F03','')*_L('GET','EX','KORR_FAKTOR'),'1') /*Korrigierter Marktwertverlauf*/ 
                    insert.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "betrag10", Value = Betrag10 });


                    insert.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "BETRAGKORRP", Value = Betrag11 });
                    insert.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "ZINS10", Value = Betrag12 });




                    if (betrag1 != 0)
                    {
                        betragINT = (betrag1 - (double)eLIndto.rate) * (loop - 1);
                    }
                    else
                    {
                        betragINT = 0;
                    }
                    insert.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "betragINT", Value = betragINT });

                    insert.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "betragKORR", Value = betragKORR }); /* Listenpreis4doremo */

                    // RAPZINSSUMME Restwert per Vertragsende aus der RWDELTA-Berechnung 
                    insert.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "RAPZINSSUMME", Value = rapzinssumme });

                    // nicht korrigierten Restwert
                    insert.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "TILGUNGVIRTUAL", Value = tilgungvirtual });

                    //Eurotax-Quelle wegen enum das + 1
                    insert.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "FlagSR4", Value = (int)outDto.source + 1 });

                    //__UPDATE('ANTOBSLPOS:ZINSENVIRTUAL')*/ 
                    insert.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "ZINSENVIRTUAL", Value = zinsenvirtual});


                    InsertParamListe.Add(insert);
                }



                if (loop > 2)
                {
                    double outDtoELRetailAmount = Math.Round(eLIndto.neupreis * (outDto.RetailValueInPercentage / 100), 2);
                    marktwert += round.RoundCHF(outDtoELRetailAmount * eLIndto.mwFaktor / 100.0);
                }

                loop++;
                rang++;

            }

            if (eLIndto.saveMarktwerteInDb)
            {

                // Insert Parameter für ANTOBSL
                InsertParamDto insertANTOBSL = new InsertParamDto();
                List<Devart.Data.Oracle.OracleParameter> parameters2 = new List<Devart.Data.Oracle.OracleParameter>();
                insertANTOBSL.parameters = parameters2;

                //__UPDATE('ANTOBSL:SysVT',ANTRAG:SysID,'0')
                insertANTOBSL.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "SysVt", Value = eLIndto.sysid });
                //__UPDATE('ANTOBSL:SysSLTYP',957,'0')
                insertANTOBSL.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "SysSLTYP", Value = 957 });
                //__UPDATE('ANTOBSL:Bezeichnung','REMO Restwertstaffel','0')
                insertANTOBSL.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "Bezeichnung", Value = "REMO Restwertstaffel" });
                //__UPDATE('ANTOBSL:Faellig',ANTRAG:Beginn,'0')
                insertANTOBSL.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "Faellig", Value = eLIndto.beginn });
                //__UPDATE('ANTOBSL:LZ',_QT('REMO','COUNT','',''),'0')
                insertANTOBSL.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "LZ", Value = eLIndto.laufzeit });
                //__UPDATE('ANTOBSL:PPY',12,'0')
                insertANTOBSL.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "PPY", Value = 12 });
                //__UPDATE('ANTOBSL:ZINS',_L('GET','EX','AZINS',''),'1') 
                insertANTOBSL.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "ZINS", Value = zins });
                //__UPDATE('ANTOBSL:RANG',_L('GET','EX','RANG',''),'1') 
                insertANTOBSL.parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "RANG", Value = 957 });


                trDao.deleteStaffel(eLIndto.sysid);
                trDao.saveStaffel(InsertParamListe, insertANTOBSL, eLIndto.sysid);
            }

            if (marktwert > 0)
            {
                return marktwert;
            }

            return 0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="saveMarktwerteInDB"></param>
        /// <returns></returns>
        public override VClusterDto getV_cluster(long sysid, bool saveMarktwerteInDB, long sysprproduct, long sysperole)
        {
            ELInDto eLIndto = eurotaxDBDao.getELInDtoAusDB(sysid,sysprproduct,sysperole);
            eLIndto.saveMarktwerteInDb = saveMarktwerteInDB;

            //a)	mit einer Anzahlung von = CHF 2‘000 wäre die TR-Prüfung erfolgreich

            //get Faktoren
            FaktorenDto faktoren = getFaktoren(eLIndto.sysvg, eLIndto.scorebezeichnung, (long)eLIndto.ausfallwvg);

            double ausfallwahrscheinlichkeitP = (1 / (1 + (double)Math.Pow(2.7182818285, (-(faktoren.kalibrierungswert + faktoren.kalibrierungsfaktor * eLIndto.scorewert))))) * 100;
            VClusterDto cluster = new VClusterDto();

            ELOutDto explos = new ELOutDto();
            explos = getEL(eLIndto, faktoren, ausfallwahrscheinlichkeitP);
            cluster.v_el_betrag = explos.TR;
            cluster.v_el_prozent = explos.TRP * 100;
            cluster.v_prof = explos.profitabilitaetp;

            ocheckTrRiskByIdDto outputGUI = new ocheckTrRiskByIdDto();

            outputGUI.Ursprungskalkulation = new Cic.OpenOne.GateBANKNOW.Common.DTO.Ursprungskalkulation();
            outputGUI.Ursprungskalkulation.Anzahlung = eLIndto.anzahlung;
            outputGUI.Ursprungskalkulation.Laufzeit = eLIndto.laufzeit;
            outputGUI.Ursprungskalkulation.Restwert = eLIndto.restwert;
            outputGUI.Ursprungskalkulation.Rate = (double)eLIndto.rate;
            outputGUI.Ursprungskalkulation.Buchwert = explos.SUMME_BW;
            outputGUI.Ursprungskalkulation.TR_Risk = explos.TR;
            outputGUI.Ursprungskalkulation.eLOutDto = explos;


            String outputGUIString = XMLSerializer.SerializeUTF8WithoutNamespace(outputGUI);
            this.trDao.saveTROutput(sysid, outputGUIString);

            return cluster;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysid"></param>
        public override void remostaffelAnlegen(long sysid, long sysprproduct, long sysperole)
        {

            ELInDto eLIndto = eurotaxDBDao.getELInDtoAusDB(sysid,sysprproduct,sysperole);
            eLIndto.saveMarktwerteInDb = true;
            double marktwerte = getMarktwerte(eLIndto);

        }


        /// <summary>
        /// 
        /// </summary>
        public override bool getEL_KALKFlag(long sysid)
        {
            bool flag = false;
            EaiparDao eaiParDao = new EaiparDao();
            string query = eaiParDao.getEaiParFileByCode("EL_KALK", "");
            if (query == "")
            {
                return true;
            }
            else
            {
                int? rval = trDao.evalEL_KALKFLAG(sysid, query);
                flag = rval == 1 ? true : false;
            }

            return flag;

        }


        /// <summary>
        /// deleteVarianteUndDERulsByAntrag
        /// </summary>
        /// <param name="sysid"></param>
        public override void deleteVarianteUndDERulsByAntrag(long sysid)
        {

            trDao.deleteVarianteUndDERulsByAntrag(sysid);
        }

        /// <summary>
        /// verwendet die gespeicherte antkalkvar für die finanzierungsvorschlags-Variante, berechnet mit WS solve neu und liefert das AngAntKalkDto
        /// </summary>
        /// <param name="antrag"></param>
        /// <param name="rang"></param>
        /// <param name="sysPEROLE"></param>
        /// <param name="isoCode"></param>
        /// <param name="mitRisikoFilter"></param>
        /// <returns></returns>
        public override AngAntKalkDto getVariante(AntragDto antrag, long rang, long sysPEROLE, string isoCode, bool mitRisikoFilter)
        {
            prKontextDto prodKontext = new prKontextDto();
            kalkKontext kkontext = new kalkKontext();

            prodKontext = MyCreateProductKontext(sysPEROLE, antrag);
            kkontext = MyCreateKalkKontext(antrag);

            KalkulationDto kalkinput = new KalkulationDto();
            AngAntKalkDto variante = trDao.getVariante(antrag.sysid, rang, mitRisikoFilter);

            if (variante != null)
            {
                kalkinput.angAntKalkDto = antrag.kalkulation.angAntKalkDto;
                kalkinput.angAntKalkDto.lz = variante.lz;
                kalkinput.angAntKalkDto.rwBrutto = variante.rwBrutto;
                kalkinput.angAntKalkDto.szBrutto = variante.szBrutto;
                kalkinput.angAntKalkDto.rateBrutto = variante.rateBrutto;
                IKalkulationBo bo = BOFactory.getInstance().createKalkulationBo(isoCode);
                Byte rateError = 0;
                KalkulationDto kalk = bo.calculate(kalkinput, prodKontext, kkontext, isoCode, ref rateError);
                return kalk.angAntKalkDto;
            }
            return null;

        }

        /// <summary>
        /// getClusterParam
        /// </summary>
        /// <param name="vg"></param>
        /// <param name="scorebezeichnung"></param>
        /// <returns></returns>
        private VClusterParamDto getClusterParam(VgDto vg, String scorebezeichnung)
        {
            bool hasScore = scorebezeichnung != null && scorebezeichnung.Length > 0;
            if (vg != null && !vg.name.Equals(""))
            {
                VClusterParamDto clusterParam = new VClusterParamDto();
                clusterParam.v_cluster = vg.name;
                Cic.OpenOne.Common.DTO.Prisma.ParamDto betrag = new Cic.OpenOne.Common.DTO.Prisma.ParamDto();
                betrag.meta = "V_EL_BETRAG";
                if (hasScore)
                {
                    betrag.maxvaln = vgDao.getVGValue(vg.sysVgDto, DateTime.Now, scorebezeichnung, EL_BETRAG_MAX, 0);
                    betrag.minvaln = vgDao.getVGValue(vg.sysVgDto, DateTime.Now, scorebezeichnung, EL_BETRAG_MIN, 0);
                }
                clusterParam.v_el_betrag = betrag;
                Cic.OpenOne.Common.DTO.Prisma.ParamDto prozent = new Cic.OpenOne.Common.DTO.Prisma.ParamDto();
                prozent.meta = "V_EL_PROZENT";
                if (hasScore)
                {
                    prozent.maxvaln = vgDao.getVGValue(vg.sysVgDto, DateTime.Now, scorebezeichnung, EL_PROZENT_MAX, 0);
                    prozent.minvaln = vgDao.getVGValue(vg.sysVgDto, DateTime.Now, scorebezeichnung, EL_PROZENT_MIN, 0);
                }
                clusterParam.v_el_prozent = prozent;
                Cic.OpenOne.Common.DTO.Prisma.ParamDto prof = new Cic.OpenOne.Common.DTO.Prisma.ParamDto();
                prof.meta = "V_PROF";
                if (hasScore)
                {
                    prof.maxvaln = vgDao.getVGValue(vg.sysVgDto, DateTime.Now, scorebezeichnung, PROF_MAX, 0);
                    prof.minvaln = vgDao.getVGValue(vg.sysVgDto, DateTime.Now, scorebezeichnung, PROF_MIN, 0);
                }
                clusterParam.v_prof = prof;
                return clusterParam;
            }
            return null;
        }


        /// <summary>
        /// Parameter für Kalkulation Varianten
        /// </summary>
        /// <param name="sysvg"></param>
        /// <param name="scorebezeichnung"></param>
        /// <returns></returns>
        private Cic.OpenOne.GateBANKNOW.Common.DTO.VariantenParam getVarianteKalkParameter(long sysvg, string scorebezeichnung)
        {

            Cic.OpenOne.GateBANKNOW.Common.DTO.VariantenParam variantenParam = new Cic.OpenOne.GateBANKNOW.Common.DTO.VariantenParam();

            try
            {
                variantenParam.ISDEFAULTANZ = false;
                variantenParam.ANZ = (double)vgDao.getVGValue(sysvg, DateTime.Now, scorebezeichnung, "ANZ", VGInterpolationMode.NONE, plSQLVersion.V1); //Höhe der minimalen Anzahlung 
            }
            catch (Exception ex)
            {
                variantenParam.ANZ = 2000; //Höhe der minimalen Anzahlung 
                variantenParam.ISDEFAULTANZ = true;

                _log.Info("Gültige Varianten Parameter nicht gefunden " + ex); 
            }
            try
            {
                  variantenParam.ISDEFAULTLZ = false;
                  variantenParam.LZ = (int)vgDao.getVGValue(sysvg, DateTime.Now, scorebezeichnung, "LZ", VGInterpolationMode.NONE, plSQLVersion.V1);      //Laufzeitgrenzen für die Verwendung von 3 (ALZ1) oder 6 (ALZ2) Monaten
            }
            catch (Exception ex)
            {
                variantenParam.LZ = 12;      //Laufzeitgrenzen für die Verwendung von 3 (ALZ1) oder 6 (ALZ2) Monaten
                variantenParam.ISDEFAULTLZ = true;

                _log.Info("Gültige Varianten Parameter nicht gefunden " + ex);
            }
            try 
            {
                variantenParam.ISDEFAULTALZ1 = false;
                variantenParam.ALZ1 = (int)vgDao.getVGValue(sysvg, DateTime.Now, scorebezeichnung, "ALZ1", VGInterpolationMode.NONE, plSQLVersion.V1);  //Reduktion der Laufzeit bei kleiner LZ 
            }
            catch (Exception ex)
            {
                variantenParam.ALZ1 = 3;  //Reduktion der Laufzeit bei kleiner LZ 
                variantenParam.ISDEFAULTALZ1 = true;
                _log.Info("Gültige Varianten Parameter nicht gefunden " + ex);
            }
             try 
            {
                variantenParam.ISDEFAULTALZ2 = false;
                variantenParam.ALZ2 = (int)vgDao.getVGValue(sysvg, DateTime.Now, scorebezeichnung, "ALZ2", VGInterpolationMode.NONE, plSQLVersion.V1);  //Reduktion der Laufzeit bei größer/gleich LZ
            }
            catch (Exception ex)
            {
                variantenParam.ALZ2 = 6;  //Reduktion der Laufzeit bei größer/gleich LZ
                variantenParam.ISDEFAULTALZ2 = true;
                _log.Info("Gültige Varianten Parameter nicht gefunden " + ex);
            }
             try
            {
                variantenParam.ISDEFAULTKOMBI = false;
                variantenParam.KOMBI = (int)vgDao.getVGValue(sysvg, DateTime.Now, scorebezeichnung, "KOMBI", VGInterpolationMode.NONE, plSQLVersion.V1); // 1 = es müssen beiden Prüfungen OK sein 
            }
            catch (Exception ex)
            {
                variantenParam.KOMBI = 1; // 1 = es müssen beiden Prüfungen OK sein  
                variantenParam.ISDEFAULTKOMBI = true;
                _log.Info("Gültige Varianten Parameter nicht gefunden " + ex);

            }
            return variantenParam;
        }



        /// <summary>
        /// get Flags für Trasaction Risiko prüfung 
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="scorebezeichnung"></param>
        /// <param name="sysvg"></param>
        /// <param name="sysVM"></param>
        /// <param name="syskdTyp"></param>
        /// <returns></returns>
        private Flags4TR getFlags(long sysid, string scorebezeichnung, long sysvg, long sysVM, long syskdTyp)
        {
            const int KDTYPID_PRIVAT = 1;
            const int KDTYPID_FIRMA = 12;

            Flags4TR flags4TR = new Flags4TR();

            VgDto vg = new VgDto();

            if (sysvg > 0)
            {
                vg = vgDao.getVg((long)sysvg);

            }

            int straccount = 0;
            string fform = angAntDao.getFform(sysid);

            straccount = angAntDao.getStraccount(sysVM);

            bool ums = (fform != "") && fform != null;
            bool stra = (straccount == 1);
            bool bwg = (angAntDao.getFlagBwgarantie(sysid) == 1);

            try
            {


                flags4TR.fkp = (int?)vgDao.getVGValue(vg.sysVgDto, DateTime.Now, scorebezeichnung, "FK", 0);
                flags4TR.pkp = (int?)vgDao.getVGValue(vg.sysVgDto, DateTime.Now, scorebezeichnung, "PK", 0);
                flags4TR.umsp = (int?)vgDao.getVGValue(vg.sysVgDto, DateTime.Now, scorebezeichnung, "UMS", 0);
                flags4TR.bwgp = (int?)vgDao.getVGValue(vg.sysVgDto, DateTime.Now, scorebezeichnung, "BWG", 0);
                flags4TR.strap = (int?)vgDao.getVGValue(vg.sysVgDto, DateTime.Now, scorebezeichnung, "STRA", 0);
                flags4TR.fel1r = (int?)vgDao.getVGValue(vg.sysVgDto, DateTime.Now, scorebezeichnung, "FEL1R", 0);

            }
            catch (Exception ex)
            {
                _log.Info("Gültige Cluster Parameter nicht gefunden " + ex);
                ;
            }
            bool PK = (syskdTyp == KDTYPID_PRIVAT && flags4TR.pkp == 1) ? true : false;
            bool FK = (syskdTyp == KDTYPID_FIRMA && flags4TR.fkp == 1) ? true : false;


            flags4TR.ums = (ums && flags4TR.umsp == 1);
            flags4TR.bwg = (bwg && flags4TR.bwgp == 1);
            flags4TR.stra = (stra && flags4TR.strap == 1);

            return flags4TR;

        }


        /// <summary>
        /// Produktprüfregel für das Transaktionsrisiko FEL1 und FEL2
        /// </summary>
        /// <param name="outDto"></param>
        /// <param name="isoCode"></param>
        /// <param name="FEL1"></param>
        /// <param name="FEL2"></param>
        /// <param name="cluster"></param>
        /// <param name="clusterParam"></param>
        /// <param name="flags4TR"></param>
        private void transaktionsrisikoPrüfung(ref ocheckAntAngDto outDto, string isoCode, bool FEL1, bool FEL2, VClusterDto cluster, VClusterParamDto clusterParam, Flags4TR flags4TR,prKontextDto kontext, AntragDto antrag, ELInDto eLIndto, bool b2b)
        {
            outDto.status = ocheckAntAngDto.STATUS_GREEN;
            outDto.errortext = new List<string>();
            outDto.code = new List<string>();
           
            bool FF = antrag.sysprchannel == 1L;        // Fahrzeugfinanzierung
            VartDto vartDto  = null;
            if (antrag.sysvart == 0)
            {
                vartDto = this.angAntDao.getVart(eLIndto.sysvart);
            }
            else
            {
                vartDto = this.angAntDao.getVart(antrag.sysvart);
            }
            const int OBARTID_NEU = 12;
            const int OBARTID_OCCASION = 13;
            const int OBUSETYPEID_DemoLeasing = 4;


            bool FF_LEASING = vartDto.code.Equals("LEASING");
            bool FF_TZK_x = vartDto.code.Contains("TZK");  
            bool berechnen = true;

            bool FRW1 = FF &&  FF_LEASING;
            bool FRW2 = FF &&  FF_LEASING;
            bool FRW3 = FF && FF_TZK_x;
            bool RWA = FF && FF_LEASING;

            bool FR1 = FF && FF_LEASING;
            bool FR2 = FF && FF_TZK_x;
            bool FD1 = FF &&  FF_LEASING;
            bool FD2 = FF && (FF_TZK_x);
            bool FD3 = FF && FF_LEASING;
            bool FD4 = FF && FF_LEASING;
            bool FD5 = FF;

            IKundeDao kundeDao = Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getKundeDao();
            // KUNDE
            KundePlusDto kundePlusDto = new KundePlusDto();
            
            kundePlusDto = kundeDao.getItPlusbySysAntrag(antrag.sysid, antrag.sysit);
            
            if (kundePlusDto != null)
            {
                
                antrag.kunde = Mapper.Map<KundePlusDto, KundeDto>(kundePlusDto);
            }
            if (antrag.kunde != null)
            {
                kontext.syskdtyp = antrag.kunde.syskdtyp;
            }

            if (FEL1 && cluster != null && clusterParam != null)
            {
                if (cluster.v_el_prozent * 100 > clusterParam.v_el_prozent.maxvaln)
                {
                    if (flags4TR.ums || flags4TR.bwg)
                        //yellow
                    {
                        MyAddErrorMessage(outDto, "CHECKANTRAG_FEL1", isoCode, "Der berechneter Expected Loss in Prozent " + cluster.v_el_prozent * 100 + " darf nicht größer als " + clusterParam.v_el_prozent.maxvaln + "% sein", ocheckAntAngDto.STATUS_YELLOW);
                    }

                    else
                    if (flags4TR.fel1r != 1)
                    {
                        MyAddErrorMessage(outDto, "CHECKANTRAG_FEL1", isoCode, "Der berechneter Expected Loss in Prozent " + cluster.v_el_prozent * 100 + "  darf nicht größer als " + clusterParam.v_el_prozent.maxvaln + "% sein", ocheckAntAngDto.STATUS_YELLOW);
                    }
                    else
                    {
                        MyAddErrorMessage(outDto, "CHECKANTRAG_FEL1", isoCode, "Der berechnete berechneter Expected Loss in Prozent darf " + cluster.v_el_prozent * 100 + "  nicht größer als " + clusterParam.v_el_prozent.maxvaln + " sein", ocheckAntAngDto.STATUS_RED);
                    }
                }

                else
                    if (cluster.v_el_betrag > clusterParam.v_el_betrag.maxvaln)
                    {
                        if (flags4TR.ums || flags4TR.bwg)
                            //yellow
                        {
                            MyAddErrorMessage(outDto, "CHECKANTRAG_FEL1", isoCode, "Der berechnete Expected Loss " + cluster.v_el_betrag + "  darf nicht größer als CHF" + clusterParam.v_el_betrag.maxvaln + " sein", ocheckAntAngDto.STATUS_YELLOW);
                        }
                        else
                        if (flags4TR.fel1r != 1)
                        {
                            MyAddErrorMessage(outDto, "CHECKANTRAG_FEL1", isoCode, "Der berechnete Expected Loss "+ cluster.v_el_betrag +" darf nicht größer als CHF" + clusterParam.v_el_betrag.maxvaln + " sein", ocheckAntAngDto.STATUS_YELLOW);
                        }
                        else
                            //red
                        {
                            MyAddErrorMessage(outDto, "CHECKANTRAG_FEL1", isoCode, "Der berechnete Expected Loss " + cluster.v_el_betrag + "  darf nicht größer als CHF" + clusterParam.v_el_betrag.maxvaln + " sein", ocheckAntAngDto.STATUS_RED);
                        }
                    }

                    else
                        if (clusterParam.v_prof.minvaln > cluster.v_prof) // PROF. neg.
                        {
                            if (flags4TR.fel1r != 1)
                            {
                                if (flags4TR.stra || flags4TR.ums || flags4TR.bwg)
                                {
                                    outDto.status = ocheckAntAngDto.STATUS_GREEN;
                                }
                                else
                                {
                                    MyAddErrorMessage(outDto, "CHECKANTRAG_FEL1", isoCode, "Die Profitabilität darf nicht negativ sein "+cluster.v_prof, ocheckAntAngDto.STATUS_YELLOW);
                                }
                            }
                            else
                            if (flags4TR.stra || flags4TR.ums || flags4TR.bwg)
                            {
                                MyAddErrorMessage(outDto, "CHECKANTRAG_FEL1", isoCode, "Die Profitabilität darf nicht negativ sein " + cluster.v_prof, ocheckAntAngDto.STATUS_YELLOW);
                            }
                            else
                            {
                                MyAddErrorMessage(outDto, "CHECKANTRAG_FEL1", isoCode, "Die Profitabilität darf nicht negativ sein " + cluster.v_prof, ocheckAntAngDto.STATUS_RED);
                            }
                        }
            }

            #region PARAMETER PRÜFUNG


            double minimalBetrag = 0;




            DateTime aktuell = DateTime.Now;

            IPrismaParameterBo paramBo = PrismaBoFactory.getInstance().createPrismaParameterBo(PrismaParameterBo.CONDITIONS_BANKNOW);

            ParamDto KalkBorderEndalterKunde = paramBo.getParameter(kontext, EnumUtil.GetStringValue(PrismaParameters.KalkBorderEndalterKunde));
            ParamDto KalkBorderRate = paramBo.getParameter(kontext, EnumUtil.GetStringValue(PrismaParameters.KalkBorderRate));
            ParamDto kalkBorderRW = paramBo.getParameter(kontext, EnumUtil.GetStringValue(PrismaParameters.kalkBorderRW));
            ParamDto kalkBorderSonderZahlungProzent = paramBo.getParameter(kontext, EnumUtil.GetStringValue(PrismaParameters.kalkBorderSonderZahlungProzent));
            ParamDto KalkBorderBgIntern = paramBo.getParameter(kontext, EnumUtil.GetStringValue(PrismaParameters.KalkBorderBgIntern));




            //A = Alter -------------------------
            if (antrag.kunde != null && antrag.kunde.syskdtyp == KDTYPID_PRIVAT && antrag.kalkulation.angAntKalkDto != null)
            {
                double endAlterKunde = MyGetEndAlterKunde(aktuell.AddMonths(eLIndto.laufzeit), kundePlusDto.gebdatum);
                if (MyIsGreaterThanMaxVal(endAlterKunde, KalkBorderEndalterKunde.maxvaln))
                {
                    MyAddErrorMessage(outDto, "CHECKANTRAG_FA", isoCode, "Maximales Alter bei Vertragsende überschritten.", ocheckAntAngDto.STATUS_RED, "FA");
                }
            }



            // B = Betragsgrenzen -------------------------
            if (MyIsSmallerThanMinVal((double)eLIndto.rate, KalkBorderRate.minvaln))

            {
                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FB3", isoCode, "Mindesthöhe für Kreditrate wurde unterschritten. Mindesthöhe = {0}."), KalkBorderRate.minvaln), ocheckAntAngDto.STATUS_RED, "FB3");
            }

            else if (MyIsGreaterThanMaxVal((double)eLIndto.rate, KalkBorderRate.maxvaln))

            {
                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FB3_Max", isoCode, "Höhe der Kreditrate wurde überschritten. Maximale Höhe = {0}."), KalkBorderRate.maxvaln), ocheckAntAngDto.STATUS_RED, "FB3");
            }

            // R = Restwert -------------------------
            //FR1 FR3
            if (MyIsSmallerThanMinVal(eLIndto.restwert, MyCalcPercentToValue(antrag.kalkulation.angAntKalkDto.bginternbrutto, kalkBorderRW.minvalp)))

            {
                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FRW1", isoCode, "Minimaler Restwert von {0} % wurde unterschritten."), kalkBorderRW.minvalp), ocheckAntAngDto.STATUS_RED, "FRW1");
            }

            else if (MyIsGreaterThanMaxVal(eLIndto.restwert, MyCalcPercentToValue(antrag.kalkulation.angAntKalkDto.bginternbrutto, kalkBorderRW.maxvalp)))

            {
                MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FRW1_Max", isoCode, "Maximaler Restwert von {0} % wurde überschritten."), kalkBorderRW.maxvalp), ocheckAntAngDto.STATUS_RED, "FRW1");
            }

            // FR3
            if (FRW3 && eLIndto.restwert > 0)
            {
                double betrag = MyCalcBetragToCompare_FRW3(antrag.kalkulation.angAntKalkDto.bginternbrutto, berechnen);

                if (MyIsSmallerThanMinVal(eLIndto.restwert, MyCalcPercentToValue(betrag, kalkBorderRW.minvalp)))
                {
                    MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FRW3", isoCode, "Mindestrestrate von {0} % wurde unterschritten."), kalkBorderRW.minvalp), ocheckAntAngDto.STATUS_YELLOW, "FRW3");
                }
                else
                    if (MyIsGreaterThanMaxVal(eLIndto.restwert, MyCalcPercentToValue(betrag, kalkBorderRW.maxvalp)))
                    {
                        MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FRW3_Max", isoCode, "Maximale Restrate von {0} % wurde überschritten."), kalkBorderRW.maxvalp), ocheckAntAngDto.STATUS_YELLOW, "FRW3");
                    }
            }

            //RWA
            if (RWA)
            {
                //Wenn eine obligatorische RWA-Objektversicherung vorliegt, soll die Deckungssumme (Berechnungslogik siehe Kap.2.2.3.3 und 2.2.4) geprüft werden. Falls die Deckungssumme (ANTVS:DECKUNGSSUMME) >0 ist, gilt die Prüfung als bestanden. Sonst ist die Prüfung nicht bestanden.
                //: „Der eingegebene Restwert befindet sich ausserhalb der Vorgaben aus der abgesicherten Restwerttabelle. Bitte Restwert entsprechend anpassen oder eine andere Produktauswahl treffen.“. 
                AngAntVsDto rwavs = (from f in antrag.kalkulation.angAntVsDto
                                     where f.serviceType == ServiceType.RWA
                                     select f).FirstOrDefault();
                if (rwavs != null)
                {

                    int? optionalRWA = (from f in PrismaDaoFactory.getInstance().getPrismaServiceDao().getVSTYP()
                                        where f.SYSVSTYP == rwavs.sysvstyp
                                        select f.FLAGPAUSCHAL).FirstOrDefault();
                    if (optionalRWA.HasValue && optionalRWA.Value == 1 && rwavs.deckungssumme == 0)
                    {
                        MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FR4", isoCode, "Der eingegebene Restwert befindet sich ausserhalb der Vorgaben aus der abgesicherten Restwerttabelle. Bitte Restwert entsprechend anpassen oder eine andere Produktauswahl treffen.")), ocheckAntAngDto.STATUS_RED, "FR4");
                    }
                    //Wenn eine obligatorische RWA-Objektversicherung vorliegt  , soll die RWA-Indikation (Berechnungslogik siehe Kap.2.2.3.3 und 2.2.4) geprüft werden. Falls die RWA-Indikation (ANTOB:RWBASE) >0 ist, gilt die Prüfung als bestanden. Sonst ist die Prüfung nicht bestanden.
                    //: „Die Kalkulation enthält die Restwertabsicherung, aber dem selektierten Fahrzeug ist keine Restwertabsicherungs-Tabelle zugeordnet.“. 
                    if (optionalRWA.HasValue && optionalRWA.Value == 1 && antrag.kalkulation.angAntKalkDto.rwBase == 0)
                    {
                        MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FR5", isoCode, "Die Kalkulation enthält die Restwertabsicherung, aber dem selektierten Fahrzeug ist keine Restwertabsicherungs-Tabelle zugeordnet.")), ocheckAntAngDto.STATUS_RED, "FR5");
                    }
                }
            }











                // D = Diverse (Spezialregelungen) -------------------------

                // Höhe der ersten Rate > x% des Barkaufpreises
                if (FD1)
            {
                double maxVal = MyCalcPercentToValue(antrag.kalkulation.angAntKalkDto.bginternbrutto, kalkBorderSonderZahlungProzent.maxvalp);
                double minVal = MyCalcPercentToValue(antrag.kalkulation.angAntKalkDto.bginternbrutto, kalkBorderSonderZahlungProzent.minvalp);
                if (antrag.kalkulation.angAntKalkDto != null)
                {
                    if (MyIsGreaterThanMaxVal(eLIndto.anzahlung, maxVal))
                    {
                        MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FD1", isoCode, "Erste grosse Leasingrate darf maximal {0} % des Barkaufpreises betragen."), kalkBorderSonderZahlungProzent.maxvalp), ocheckAntAngDto.STATUS_RED, "FD1");
                    }
                    else
                        if (MyIsSmallerThanMinVal(eLIndto.anzahlung, minVal))
                        {
                            MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FD1_Min", isoCode, "Erste grosse Leasingrate muss minimal {0} % des Barkaufpreises betragen."), kalkBorderSonderZahlungProzent.minvalp), ocheckAntAngDto.STATUS_RED, "FD1");
                        }
                }
            }

            //B = Betragsgrenze -------------------------
            {
                // _log.Debug(String.Format("KALK_BORDER_BGINTERN: checkAntragByIdErweiterung(sysid = {0}, sysvart = {1}, isoCode = {2} ,b2b = {3}, nurallgemeine = {4})", sysid,  sysvart,  isoCode,  b2b,  nurallgemeine));

                if (antrag.kalkulation != null && antrag.kalkulation.angAntKalkDto != null)
                {
                    // überschritten
                    if (FF_TZK_x)
                    {
                        if (MyIsGreaterThanMaxVal(MyCalcBetragToCompare_FB12(antrag.kalkulation.angAntKalkDto.bginternbrutto,  eLIndto.anzahlung, berechnen), KalkBorderBgIntern.maxvaln))
                        {
                            MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FB1", isoCode, "Maximaler Kreditbetrag wurde überschritten. Maximaler Kreditbetrag = {0} (FB1)."), KalkBorderBgIntern.maxvaln), ocheckAntAngDto.STATUS_RED, "FB1");
                        }
                    }

                    if (FF_LEASING)
                    {
                        if (MyIsGreaterThanMaxVal(MyCalcBetragToCompare_FB4567(antrag.kalkulation.angAntKalkDto.bginternbrutto, eLIndto.anzahlung, (double)eLIndto.rate, berechnen), KalkBorderBgIntern.maxvaln))
                        {
                            MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FB7", isoCode, "Finanzierungsbetrag wurde überschritten. Maximaler Finanzierungsbetrag = {0}."), KalkBorderBgIntern.maxvaln), ocheckAntAngDto.STATUS_RED, "FB7");
                        }
                    }

                    // Unterschritten
                    minimalBetrag = KalkBorderBgIntern.minvaln;
                    if (FF_TZK_x)
                    {
                        // _log.Debug(String.Format("FB2:prParam.minvaln = {0}, bginternbrutto = {1}", prParam.minvaln, antrag.kalkulation.angAntKalkDto.bginternbrutto));

                        if (MyIsSmallerThanMinVal(MyCalcBetragToCompare_FB12(antrag.kalkulation.angAntKalkDto.bginternbrutto, eLIndto.anzahlung, berechnen), KalkBorderBgIntern.minvaln))
                        {
                            MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FB2", isoCode, "Minimaler Kreditbetrag wurde unterschritten. Mindestkreditbetrag = {0} (FB2)."), KalkBorderBgIntern.minvaln), ocheckAntAngDto.STATUS_RED, "FB2");
                        }
                    }

                    if (MyIsSmallerThanMinVal(MyCalcBetragToCompare_FB4567(antrag.kalkulation.angAntKalkDto.bginternbrutto, eLIndto.anzahlung, (double)eLIndto.rate, berechnen), KalkBorderBgIntern.minvaln))
                    {
                        // _log.Debug(String.Format("FB456: prParam.minvaln = {0}, bginternbrutto = {1}", prParam.minvaln, antrag.kalkulation.angAntKalkDto.bginternbrutto));
                        // _log.Debug(String.Format("FB456: MyCalcBetragToCompare_FB4567() = {0}", MyCalcBetragToCompare_FB4567(antrag.kalkulation.angAntKalkDto, berechnen)));

                        if (FF_LEASING && antrag.angAntObDto.sysobart == OBARTID_NEU)
                        {
                            MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FB4", isoCode, "Minimaler Finanzierungsbetrag wurde unterschritten. Minimaler Finanzierungsbetrag = {0} (FB4)."), KalkBorderBgIntern.minvaln), ocheckAntAngDto.STATUS_RED, "FB4");
                        }
                        if (FF_LEASING && antrag.angAntObDto.sysobart == OBARTID_OCCASION)
                        {
                            MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FB5", isoCode, "Minimaler Finanzierungsbetrag wurde unterschritten. Minimaler Finanzierungsbetrag = {0} (FB5)."), KalkBorderBgIntern.minvaln), ocheckAntAngDto.STATUS_RED, "FB5");
                        }
                        if (FF_LEASING && antrag.kalkulation.angAntKalkDto.sysobusetype == OBUSETYPEID_DemoLeasing)
                        {
                            MyAddErrorMessage2(outDto, String.Format(CultureInfo.CreateSpecificCulture("de-CH"), MyTranslate("CHECKANTRAG_FB6", isoCode, "Minimaler Finanzierungsbetrag wurde unterschritten. Minimaler Finanzierungsbetrag = {0} (FB6)."), KalkBorderBgIntern.minvaln), ocheckAntAngDto.STATUS_RED, "FB6");
                        }
                    }

                }





            }

            #endregion PARAMETERPRÜFUNG

            if (outDto.code.Count >0)
            {
                _log.Info("PRPrüfung in Transaktionsrisiko Codes: " + String.Join(",", outDto.code.ToArray()));
            }
            if (outDto.errortext.Count > 0)
            {
                _log.Info("PRPrüfung in Transaktionsrisiko Messages: " + String.Join(",", outDto.errortext.ToArray()));
            }
        }




        /// <summary>
        /// MyAddErrorMessage
        /// </summary>
        /// <param name="outDto"></param>
        /// <param name="messageCode"></param>
        /// <param name="isoCode"></param>
        /// <param name="defaultMessage"></param>
        /// <param name="newStatus"></param>
        private void MyAddErrorMessage(ocheckAntAngDto outDto, String messageCode, String isoCode, String defaultMessage, String newStatus)
        {

            outDto.errortext.Add(translateBo.translateMessage(messageCode, isoCode, defaultMessage));
            outDto.status = MySetStatus(outDto.status, newStatus);
        }

        // Die Methode vermeidet, dass der Status von rot auf gelb zurückgesetzt wird.
        private String MySetStatus(String oldstatus, String status)
        {
            if (oldstatus == ocheckAntAngDto.STATUS_RED)
            {
                return oldstatus;
            }
            else
            {
                return status;
            }
        }


        /// <summary>
        /// Buchwerte kalkulation Parameter Mapping
        /// </summary>
        /// <param name="eLIndto"></param>
        /// <returns></returns>
        private BWInDto myMappingBWInDto(ELInDto eLIndto)
        {
            BWInDto bwInDto = new BWInDto();
            bwInDto.mwst = eLIndto.mwst;
            bwInDto.barkaufpreis = eLIndto.barkaufpreis;
            bwInDto.zins = eLIndto.zins;
            bwInDto.restwert = eLIndto.restwert;
            bwInDto.laufzeit = eLIndto.laufzeit;
            bwInDto.rate = eLIndto.rate;
            bwInDto.anzahlung = eLIndto.anzahlung;


            return bwInDto;
        }


        /// <summary>
        /// MyCreateProductKontext
        /// </summary>
        /// <param name="sysPEROLE"></param>
        /// <param name="antrag"></param>
        /// <returns></returns>
        private  Cic.OpenOne.Common.DTO.Prisma.prKontextDto MyCreateProductKontext(long sysPEROLE, AntragDto antrag)
        {

            Cic.OpenOne.Common.DTO.Prisma.prKontextDto pKontext = new Cic.OpenOne.Common.DTO.Prisma.prKontextDto();

            if (antrag != null)
            {
                if (antrag.kalkulation == null)
                {
                    throw new ApplicationException("No active calculation.");
                }

                KalkulationDto activeKalk = antrag.kalkulation;
                // Kontext erstellen
                pKontext = new OpenOne.Common.DTO.Prisma.prKontextDto();

                pKontext.perDate = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                pKontext.sysperole = sysPEROLE;
                pKontext.sysprproduct = activeKalk.angAntKalkDto.sysprproduct;
                pKontext.sysbrand = 0;// angebot.sysbrand;
                if (antrag.kunde != null)
                {
                    pKontext.syskdtyp = antrag.kunde.syskdtyp;
                }
                if (antrag.angAntObDto != null)
                {
                    pKontext.sysobart = antrag.angAntObDto.sysobart;
                    pKontext.sysobtyp = antrag.angAntObDto.sysobtyp;
                }
                pKontext.sysprchannel = (long)antrag.sysprchannel;
                pKontext.sysprhgroup = (long)antrag.sysprhgroup;
                pKontext.sysprusetype = activeKalk.angAntKalkDto.sysobusetype;
                pKontext.sysbrand = antrag.sysbrand;
                pKontext.sysobart = antrag.angAntObDto.sysobart;
                pKontext.sysprhgroup = (long)antrag.sysprhgroup;
                pKontext.sysprjoker = antrag.sysprjoker;
                pKontext.sysprproduct = antrag.kalkulation.angAntKalkDto.sysprproduct;
                pKontext.sysvart = antrag.sysvart;
                if (antrag.sysvttyp!=null)
                {
                    pKontext.sysvttyp = (long)antrag.sysvttyp;
                }

                pKontext.sysprkgroup = angAntDao.getPrkgroupByAntragID(antrag.sysid);
               
     
               

                return pKontext;
            }
            throw new ApplicationException("No Antrag.");
        }


        /// <summary>
        /// MyCreateKalkKontext
        /// </summary>
        /// <param name="antrag"></param>
        /// <returns></returns>
        private static kalkKontext MyCreateKalkKontext(AntragDto antrag)
        {
            //B2B
            //this.rateBruttoInklAbsicherung = 0D;
            //this.ersteRateBruttoInklAbsicherung = 0D;
            kalkKontext kkontext = new kalkKontext();
            kkontext.grundBrutto = antrag.angAntObDto.grundBrutto;
            kkontext.ubnahmeKm = antrag.angAntObDto.ubnahmeKm;
            kkontext.erstzulassung = antrag.angAntObDto.erstzulassung;
            kkontext.zubehoerBrutto = antrag.angAntObDto.zubehoerBrutto;
            kkontext.zinsNominal = 0;
            return kkontext;
        }


        private string MyArraytoSting(double[] werteArray)
        {
            return String.Join(";", werteArray);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="antrag"></param>
        /// <returns></returns>
        private DecisionEngineInDto getRequestDEFromAntrag(AntragDto antrag)
        {
            // antrag.kalkulation.angAntKalkDto.sysobusetype
            const int Privat = 2;
            const int Gewerblich = 3;
            //const int DemoLeasing = 4;


            //Alle nicht für die Risikoprüfung relevanten Daten werden aus der Auskunft zur letzten Bonitätsprüfung geholt.

            DecisionEngineInDto inputDE = new DecisionEngineInDto();
            string kundenid = trDao.getKundenID(antrag.kunde.syskd);
            inputDE = trDao.getInputDatenDE(antrag.sysid);
            inputDE.Flagsimulation = 1;
            inputDE.FlagBonitaetspruefung = 0;
            inputDE.FlagRisikopruefung = 1;



            StrategyOneRequest S1Request = new StrategyOneRequest();


            //info: 1 11 6 2 4 8 7 3 0 decontract = kkgpflicht decimal / dto bool?
            inputDE.KKG_Pflicht = (antrag.kkgpflicht == null || antrag.kkgpflicht == false) ? 0 : 1;
            inputDE.Nutzungsart = antrag.kalkulation.angAntKalkDto.sysobusetype == Gewerblich ? "Gewerblich" : "";
            inputDE.Nutzungsart = antrag.kalkulation.angAntKalkDto.sysobusetype == Privat ? "Privat" : "";
            inputDE.Nutzungsart = antrag.kalkulation.angAntKalkDto.sysobusetype == Privat ? "Demo" : "";
            inputDE.sysVTTYP = antrag.sysvttyp;


            switch (antrag.erfassungsclient)//Clienttyp (10=B2B, 20=MA, 30=B2C, 50=ONE)
            {
                case AngAntDao.ERFASSUNGSCLIENT_B2B: inputDE.Erfassungskanal = "E_POS"; break;
                case AngAntDao.ERFASSUNGSCLIENT_MA: inputDE.Erfassungskanal = "BN_MA_AP"; break;
                case AngAntDao.ERFASSUNGSCLIENT_B2C: inputDE.Erfassungskanal = "I_B2C"; break;
                case AngAntDao.ERFASSUNGSCLIENT_ONE: inputDE.Erfassungskanal = "WEB_VLM"; break;
                case AngAntDao.ERFASSUNGSCLIENT_DMR: inputDE.Erfassungskanal = "DMS"; break;
                default: inputDE.Erfassungskanal = "E_POS"; break;
            }



            inputDE.Riskflag = (decimal?)trDao.getRiskFlag(antrag.sysid);

            if (antrag.sysvart > 0)
            {
                inputDE.Vertragsart = antrag.sysvart;
            }
            inputDE.PPI_Flag_Paket1 = (decimal?)trDao.getPPI_Flag_Paket1(antrag.sysid);
            inputDE.PPI_Flag_Paket2 = (decimal?)trDao.getPPI_Flag_Paket2(antrag.sysid);



            Budget4DESimDto budget4DESimDto = trDao.getBudget4DESim(antrag.sysid);

            inputDE.Budgetueberschuss_1 = budget4DESimDto.budget1;
            inputDE.Budgetueberschuss_2 = budget4DESimDto.saldo2;
            if (budget4DESimDto.saldo > 0)
            {
                inputDE.Budgetueberschuss_gesamt = budget4DESimDto.saldo;
            }
            else
            {
                inputDE.Budgetueberschuss_gesamt = inputDE.Budgetueberschuss_1;
            }
            inputDE.KremoCode = (decimal?)budget4DESimDto.fehlercode;


            // I_O - OBJEKT
            AngAntObDto objDaten = angAntDao.getObjektdaten(antrag.angAntObDto.sysobtyp);
            inputDE.Objektart = objDaten.bezeichnung; // v = enum ObjektartV
            inputDE.Modell = antrag.angAntObDto.bezeichnung.ToUpper();
            inputDE.Zustand = trDao.getZustandFromObject(antrag.angAntObDto.sysobart);
            inputDE.VIN_Nummer = antrag.angAntObDto.brief.fident;
            if (antrag.kalkulation != null && antrag.kalkulation.angAntVsDto != null)
            {
                AngAntVsDto rwavs = (from f in antrag.kalkulation.angAntVsDto
                                     where f.serviceType == ServiceType.RWA
                                     select f).FirstOrDefault();
                if (rwavs != null)
                {
                    inputDE.Restwertabsicherung = (decimal)rwavs.deckungssumme;
                }
            }

            
            //zum Test 0
            inputDE.KM_Stand = antrag.angAntObDto.ubnahmeKm;
            inputDE.KM_prJahr = antrag.angAntObDto.jahresKm;
            inputDE.Inverkehrssetzung = antrag.angAntObDto.erstzulassung;
            inputDE.Stammnummer = antrag.angAntObDto.brief.stammnummer;
            inputDE.Zubehoerpreis = (decimal?)antrag.angAntObDto.zubehoerBrutto;
            inputDE.Katalogpreis_Eurotax = (decimal?)antrag.angAntObDto.grundBrutto;
            inputDE.Fahrzeugpreis_Eurotax = (decimal?)antrag.angAntObDto.ahkBrutto; //???? TODO



            inputDE.Marktwert_Cluster = trDao.getMarktwert_Cluster(antrag.sysid);

            if (inputDE.RecordRRDto != null && inputDE.RecordRRDto[0] != null)
            {
                inputDE.RecordRRDto[0].BU_Budgetueberschuss = inputDE.Budgetueberschuss_1;
                inputDE.RecordRRDto[0].BU_Budgetueberschuss_gesamt = inputDE.Budgetueberschuss_gesamt;
            }

            return inputDE;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="score"></param>
        /// <param name="formel"></param>
        /// <param name="betrag"></param>
        /// <param name="sz"></param>
        /// <param name="trbetrag"></param>
        /// <param name="pint"></param>
        /// <param name="pslot"></param>
        /// <param name="sysWfuser"></param>
        /// <param name="sysID"></param>
        /// <returns></returns>
        private double? EvalTR4ScorekarteDB(string score, string formel, double betrag, double sz, double trbetrag, double pint, double pslot, long sysWfuser, long sysID)
        {
            double? rval = null;

            rval = trDao.evalTR4Scorekarte(formel, betrag, sz, trbetrag, pint, pslot, sysID);
            return rval;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="score"></param>
        /// <param name="formel"></param>
        /// <param name="betrag"></param>
        /// <param name="sz"></param>
        /// <param name="trbetrag"></param>
        /// <param name="pint"></param>
        /// <param name="pslot"></param>
        /// <param name="sysWfuser"></param>
        /// <param name="sysID"></param>
        /// <returns></returns>
        private double? EvalTR4ScorekarteBAS(string score, string formel, double betrag, double sz, double trbetrag, double pint, double pslot, long sysWfuser, long sysID)
        {
            double? rval = null;
            ICASBo bo = new CASBo();



            if (bo != null && formel != "")
            {
                iCASEvaluateDto ieval = new iCASEvaluateDto();
                ieval.area = "ANTRAG";

                formel = formel.Replace(":pBETRAG", betrag.ToString());
                formel = formel.Replace(":pSZ", sz.ToString());
                formel = formel.Replace(":pTRBETRAG", trbetrag.ToString());
                formel = formel.Replace(":pINT", pint.ToString());
                formel = formel.Replace(":pSLOP", pslot.ToString());


                ieval.expression = new String[] { formel };
                ieval.sysID = new long[] { sysID };
                CASEvaluateResult[] er = null;


                try
                {
                    er = bo.getEvaluation(ieval, sysWfuser);
                    if (er[0].clarionResult[0] != null && !"".Equals(er[0].clarionResult[0]))
                    {
                        rval = double.Parse(er[0].clarionResult[0], CultureInfo.InvariantCulture);
                    }
                }
                catch (Exception e)
                {
                    _log.Warn("CAS-Evaluation failed for EvalTR4Scorekarte", e);
                    rval = null;
                }

            }

            return rval;


        }

        private double MyGetEndAlterKunde(DateTime? endeAm, DateTime? gebDatum)
        {
            if (gebDatum != null && endeAm != null && gebDatum < endeAm)
            {
                return MyGetJahreDiff((DateTime)endeAm, (DateTime)gebDatum);
            }

            return 0;
        }

        private bool MyIsGreaterThanMaxVal(double valueToCompare, double maxValue)
        {
            // Wenn PRPARAM.MAXVALN gleich 0 ist, dann soll sie nicht berücksichtigt werden.
            if (maxValue > 0)
            {
                // Wenn PRPARAM.MAXVALN > 0 und valueToCompare > maxValue, dann wird ein Fehlertext ausgegeben.
                return (valueToCompare > maxValue);
            }
            // Kein Fehler
            return false;
        }


        /// <summary>
        /// MyAddErrorMessage
        /// </summary>
        /// <param name="outDto"></param>
        /// <param name="messageCode"></param>
        /// <param name="isoCode"></param>
        /// <param name="defaultMessage"></param>
        /// <param name="newStatus"></param>
        private void MyAddErrorMessage(ocheckAntAngDto outDto, String messageCode, String isoCode, String defaultMessage, String newStatus, String code)
        {
            outDto.errortext.Add(translateBo.translateMessage(messageCode, isoCode, defaultMessage));
            outDto.status = MySetStatus(outDto.status, newStatus);
            outDto.code.Add(code);
        }


        private bool MyIsSmallerThanMinVal(double valueToCompare, double minValue)
        {
            // Wenn PRPARAM.MINVALN gleich 0 ist, dann soll sie nicht berücksichtigt werden.
            if (minValue > 0)
            {
                // Wenn PRPARAM.MINVALN > 0 und valueToCompare <= minValue, dann wird ein Fehlertext ausgegeben.
                return (valueToCompare < minValue);
            }
            // Kein Fehler
            return false;
        }

        private double MyCalcPercentToValue(double? baseValueNullable, double percent)
        {
            return (baseValueNullable == null ? 0 : (double)baseValueNullable) * percent / 100;
        }

        /// <summary>
        /// MyAddErrorMessage2
        /// </summary>
        /// <param name="outDto"></param>
        /// <param name="message"></param>
        /// <param name="newStatus"></param>
        private void MyAddErrorMessage2(ocheckAntAngDto outDto, String message, String newStatus, String code)
        {
            outDto.errortext.Add(message);
            outDto.status = MySetStatus(outDto.status, newStatus);
            outDto.code.Add(code);
        }


        /// <summary>
        /// MyTranslate
        /// </summary>
        /// <param name="isoCode"></param>
        /// <param name="messageCode"></param>
        /// <param name="defaultMessage"></param>
        /// <returns></returns>
        private string MyTranslate(String messageCode, String isoCode, String defaultMessage)
        {
            return (translateBo.translateMessage(messageCode, isoCode, defaultMessage));
        }



        /// <summary>
        /// getJahreDiff
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        /// <returns></returns>
        private double MyGetJahreDiff(DateTime date1, DateTime date2)
        {
            return MyGetMonatDiff(date1, date2) / 12;
        }

        /// <summary>
        /// getMonatDiff
        /// </summary>
        /// <param name="ndate1"></param>
        /// <param name="ndate2"></param>
        /// <returns></returns>
        private double MyGetMonatDiff(DateTime? ndate1, DateTime? ndate2)
        {
            if (ndate1 == null && ndate2 == null)
            {
                return 0;
            }

            DateTime date1 = ndate1 == null ? Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null) : ndate1.Value;
            DateTime? dt2temp = DateTimeHelper.ClarionDateToDtoDate(ndate2);
            DateTime date2 = dt2temp == null ? Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null) : ndate2.Value;

            // Beide Daten in einer Liste speichern und sortieren 
            List<DateTime> period = new List<DateTime>() { date1, date2 };
            period.Sort(DateTime.Compare);

            // Ganze Monate zählen
            int monthsFull;
            for (monthsFull = 0; period[0].AddMonths(monthsFull + 1).CompareTo(period[1]) <= 0; monthsFull++)
            {
                ;
            }

            int dayDiff = 0;
            if (date2.Day != date1.Day)
            {
                DateTime startTime = period[0];
                DateTime endTime = period[1];
                dayDiff = endTime.Day - startTime.Day;
                if (dayDiff < 0)
                {
                    dayDiff += 31;
                }
            }

            // nur die Resttage durch 31 teilen
            return monthsFull + dayDiff / 31.0;
        }

      



        private double MyCalcBetragToCompare_FRW3(double bginternbrutto, bool berechnen)
        {
            double betrag = bginternbrutto;            // Aufruf
            if (berechnen)
            {
                betrag = bginternbrutto ;
            }
            return betrag;
        }


        private double MyCalcBetragToCompare_FB12(double bginternbrutto,double szBrutto, bool berechnen)
        {
            double betrag = bginternbrutto;            // Aufruf
            if (berechnen)
            {
                betrag = bginternbrutto - szBrutto;
            }
            return betrag;
        }

        private double MyCalcBetragToCompare_FB4567(double bginternbrutto,double szBrutto,double rateBrutto, bool berechnen)
        {
            double betrag = bginternbrutto;            // Aufruf
            if (berechnen)
            {
                if (szBrutto > 0)
                {
                    betrag -= szBrutto;
                }
                else
                {
                    betrag -= rateBrutto;
                }
            }
            return betrag;
        }

        /// <summary>
        /// Holt den Timeout-Parameter für den MinimaleAnzahlung TR aus der CFG
        /// </summary>
        /// <returns>Timeout-Wert</returns>
        private static int MyGetTimeOutValue(String timeoutKey, int defaultValue)
        {
            int retValue = 0;
            String cfgParam = Cic.OpenOne.Common.Util.Config.AppConfig.Instance.GetEntry("WEBSERVICES", timeoutKey, defaultValue.ToString(), "SETUP.NET");
            Int32.TryParse(cfgParam, out retValue);

            return retValue;
        }




       

        #endregion private Methoden




    }



}

