using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.KREMORef;
using Cic.OpenOne.Common.Model.DdIc;
using Cic.OpenOne.Common.Util.Logging;
using AutoMapper;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.Util.Config;
using System.Globalization;
using System.Reflection;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.Util;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    /// <summary>
    /// KREMO Business Object
    /// </summary>
    public class KREMOBo : AbstractKREMOBo
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const double Default = 0.0;
        private const long codeTechExc = -2;
        private const long codeSerAufExc = -1;

        /// <summary>
        /// KREMO Business Object
        /// </summary>
        /// <param name="dao"></param>
        /// <param name="dbdao"></param>
        /// <param name="auskunftdao"></param>
        public KREMOBo(IKREMOWSDao dao, IKREMODBDao dbdao, IAuskunftDao auskunftdao)
            : base(dao, dbdao, auskunftdao)
        {


        }



        /// <summary>
        /// maps Zivilstand OL to Kremo
        /// 
        /// Kremo:
        /// 0=ledig, 1=verheiratet, 2=geschieden, 3=gerichtlich getrennt, 4=verwitwet, 5=eingetragene Partnerschaft, 
        /// 6=gerichtlich aufgelöste Partnerschaft (wird wie geschieden behandelt)
        /// 
        /// 
        /// 
        /// </summary>

        private static double mapFamstand(double famstand)
        {
            if (famstand == 1)
                return 0;
            if (famstand == 2)
                return 1;
            if (famstand == 3)
                return 4;
            if (famstand == 4)
                return 2;
            if (famstand == 5)
                return 3;

            return 0;
        }



        /// <summary>
        /// Calculates the Budgetüberschuss for Budgetcalculator 
        /// considers AS1 and AS2 
        /// uses Ruleenine "ANTRAGSASSISTENT", "RULEENGINE_BUDGET", "USE_RULESET_B2B"
        /// calls Kremo-Interface before calling ruleengine
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <param name="budget1"></param>
        /// <param name="budget2"></param>
        /// <returns></returns>
        override public ogetKremoBudget getKremoBudget(long syswfuser, Cic.OpenOne.GateBANKNOW.Common.DTO.KremoBudgetDto budget1, Cic.OpenOne.GateBANKNOW.Common.DTO.KremoBudgetDto budget2, long sysprproduct)
        {
            ogetKremoBudget rval = new ogetKremoBudget();
            String param = Cic.OpenOne.Common.Util.Config.AppConfig.Instance.GetEntry("GETBUDGET", "EINKOMMEN_MAX", "1000000", "KREMO");
            double mValue = Double.Parse(param);
          
            KREMOInDto kin = Mapper.Map<PkzDto, KREMOInDto>(budget1.pkz);
            
            kin.Plz = budget1.plz;
            

            if (budget1 != null)
            {
                if (budget1.pkz.einknetto > mValue)
                    budget1.pkz.einknetto = mValue;
                if (budget1.pkz.jbonusnetto > mValue)
                    budget1.pkz.jbonusnetto = mValue;
                if (budget1.pkz.jbonusbrutto > mValue)
                    budget1.pkz.jbonusbrutto = mValue;
                if (budget1.pkz.nebeinknetto > mValue)
                    budget1.pkz.nebeinknetto = mValue;

                kin.Einkbrutto = budget1.pkz.einkbrutto.GetValueOrDefault();
                kin.Einknetto = budget1.pkz.einknetto.GetValueOrDefault();
                kin.Famstandcode = mapFamstand(budget1.pkz.familienstand);
                
                kin.GebDatum = budget1.GebDatum.Value.Year * 10000 + budget1.GebDatum.Value.Month * 100 + budget1.GebDatum.Value.Day;
                kin.Kantoncode = budget1.getKantoncodeInternal();
                kin.Grundcode = 0;
                try
                {
                    kin.Grundcode = double.Parse(budget1.pkz.wohnverhCode);
                }
                catch (Exception exce) { }

                kin.Nebeneinkbrutto = budget1.pkz.nebeinkbrutto.GetValueOrDefault();
                kin.Nebeneinknetto = budget1.pkz.nebeinknetto.GetValueOrDefault();
                kin.Qstflag = budget1.pkz.quellensteuerFlag ? 1 : 0;
                kin.Unterhalt = budget1.pkz.unterhalt.GetValueOrDefault();
                if (budget1.pkz.jbonusnetto.HasValue)
                    kin.Einknetto += budget1.pkz.jbonusnetto.Value / 12.0;
                if (budget1.pkz.jbonusbrutto.HasValue)
                    kin.Einkbrutto += budget1.pkz.jbonusbrutto.Value / 12.0;
                rval.pkz1 = budget1.pkz;
            }
            if (budget2 != null)
            {
                if (budget2.pkz.einknetto > mValue)
                    budget2.pkz.einknetto = mValue;
                if (budget2.pkz.jbonusnetto > mValue)
                    budget2.pkz.jbonusnetto = mValue;
                if (budget2.pkz.jbonusbrutto > mValue)
                    budget2.pkz.jbonusbrutto = mValue;
                if (budget2.pkz.nebeinknetto > mValue)
                    budget2.pkz.nebeinknetto = mValue;

                kin.Einkbrutto2 = budget2.pkz.einkbrutto.GetValueOrDefault();
                kin.Einknetto2 = budget2.pkz.einknetto.GetValueOrDefault();
                kin.Famstandcode2 = mapFamstand(budget2.pkz.familienstand);
                kin.GebDatum2 = budget2.GebDatum.Value.Year * 10000 + budget2.GebDatum.Value.Month * 100 + budget2.GebDatum.Value.Day;
                kin.Kantoncode2 = budget2.getKantoncodeInternal();
                
                kin.Nebeneinkbrutto2 = budget2.pkz.nebeinkbrutto.GetValueOrDefault();
                kin.Nebeneinknetto2 = budget2.pkz.nebeinknetto.GetValueOrDefault();
                kin.Qstflag2 = budget2.pkz.quellensteuerFlag ? 1 : 0;
                kin.Unterhalt2 = budget2.pkz.unterhalt.GetValueOrDefault();
                if (budget2.pkz.jbonusnetto.HasValue)
                    kin.Einknetto2 += budget2.pkz.jbonusnetto.Value / 12.0;
                if (budget2.pkz.jbonusbrutto.HasValue)
                    kin.Einkbrutto2 += budget2.pkz.jbonusbrutto.Value / 12.0;
                rval.pkz2 = budget2.pkz;

                if (budget2.pkz.einknettoFlag)
                    kin.Einkbrutto2 = 0;
                else kin.Einknetto2 = 0;


            }
            if (budget1.pkz.einknettoFlag)//den jeweils anderen Wert entfernen!
                kin.Einkbrutto = 0;
            else kin.Einknetto = 0;

            
            
            
            

            //Kremoberechnung
            KREMOOutDto kvaluesAS1 = getKremoValues(kin);

            //Krankenkassenberechnung
            if (!budget1.pkz.krankenkasse.HasValue || budget1.pkz.krankenkasse == 0)
                budget1.pkz.krankenkasse = kvaluesAS1.Berechkrankenkasse;
            rval.pkz1 = budget1.pkz;


            ConditionLinkType[] CONDITIONS_BANKNOW_KR = { ConditionLinkType.COMMON, ConditionLinkType.BRAND, ConditionLinkType.OBTYP, ConditionLinkType.PRHGROUP, ConditionLinkType.PRKGROUP, ConditionLinkType.PRODUCT, ConditionLinkType.BCHNL, ConditionLinkType.OBART, ConditionLinkType.KDTYP, ConditionLinkType.USETYPE, ConditionLinkType.VART, ConditionLinkType.VTTYP, ConditionLinkType.PEROLE };
            IPrismaParameterBo paramBo = PrismaBoFactory.getInstance().createPrismaParameterBo(CONDITIONS_BANKNOW_KR);

            IPrismaDao prismaDao = PrismaDaoFactory.getInstance().getPrismaDao();
            CIC.Database.PRISMA.EF6.Model.VART vart = prismaDao.getVertragsart(sysprproduct);
            CIC.Database.PRISMA.EF6.Model.VTTYP vtTYP = prismaDao.getVttyp(sysprproduct);
            prKontextDto kontext = new prKontextDto();
            if(vart!=null)
                kontext.sysvart = vart.SYSVART;  
            if(vtTYP!=null)
                kontext.sysvttyp = vtTYP.SYSVTTYP;
            kontext.sysprproduct = sysprproduct;
            
            List<Cic.OpenOne.Common.DTO.Prisma.ParamDto> prodParams = paramBo.listAvailableParameter(kontext);

            Cic.OpenOne.Common.DTO.Prisma.ParamDto budgetzins = (from zf in prodParams
                                                           where zf.meta.Equals("BUDGETZINS")
                                                           select zf).FirstOrDefault();


            Boolean useRuleEngine = true;
            String ruleCode = AppConfig.Instance.GetCfgEntry("ANTRAGSASSISTENT", "RULEENGINE_BUDGET", "USE_RULESET_B2B", "");
            if (useRuleEngine)
            {
                CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto[] vars = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto[2];
                vars[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto();
                vars[0].LookupVariableName = "SYSPRPRODUCT"; 
                vars[0].VariableName = "BUDGET";
                vars[0].Value = ""+sysprproduct;
                vars[1] = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto();
                vars[1].LookupVariableName = "BUDGETZINS";
                vars[1].VariableName = "BUDGET";
                vars[1].Value = "0";
                if(budgetzins!=null)
                    vars[1].Value = "" + budgetzins.defvalp;

                BPEQueue bpeQueue = new BPEQueue();
                bpeQueue.addQueue("qBUDGETIN");
                bpeQueue.addQueue("qBUDGETOUT");
                bpeQueue.addQueue("qFAKTOR");
                if (budget1.pkz.einknettoFlag)
                {
                    bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS1_EINKOMMEN").addQueueRecordValue("F02", budget1.pkz.einknetto.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                    bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS1_NEBENEINKOMMEN").addQueueRecordValue("F02", budget1.pkz.nebeinknetto.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                    bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS1_ZUSATZEINKOMMEN").addQueueRecordValue("F02", budget1.pkz.zeinknetto.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                    bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS1_MONATLICHEGRATIFIKATION").addQueueRecordValue("F02", (budget1.pkz.jbonusnetto.GetValueOrDefault() / 12.0).ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS1_EINKOMMEN").addQueueRecordValue("F02", budget1.pkz.einkbrutto.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                    bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS1_NEBENEINKOMMEN").addQueueRecordValue("F02", budget1.pkz.nebeinkbrutto.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                    bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS1_ZUSATZEINKOMMEN").addQueueRecordValue("F02", budget1.pkz.zeinkbrutto.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                    bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS1_MONATLICHEGRATIFIKATION").addQueueRecordValue("F02", (budget1.pkz.jbonusbrutto.GetValueOrDefault() / 12.0).ToString(CultureInfo.InvariantCulture));
                }

                bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS1_ENTHALTENEZULAGEN").addQueueRecordValue("F02",  (budget1.pkz.zulagesonst.GetValueOrDefault()+ budget1.pkz.zulagekind.GetValueOrDefault()+ budget1.pkz.zulageausbildung.GetValueOrDefault()).ToString(CultureInfo.InvariantCulture));
                bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS1_ZULAGESONST").addQueueRecordValue("F02", budget1.pkz.zulagesonst.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS1_ZULAGEKIND").addQueueRecordValue("F02", budget1.pkz.zulagekind.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS1_ZULAGEAUSBILDUNG").addQueueRecordValue("F02", budget1.pkz.zulageausbildung.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));

                bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS1_13MONATSLOHN").addQueueRecordValue("F02",  budget1.pkz.monatslohnXtdFlag ? "1" : "0");
                
                
                bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS1_WOHNKOSTENMIETE").addQueueRecordValue("F02",  budget1.pkz.miete.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS1_REGAUSLAGEN").addQueueRecordValue("F02",  budget1.pkz.auslagen.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS1_UNTERSTUETZUNGSBEITRAEGE").addQueueRecordValue("F02",  budget1.pkz.unterhalt.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS1_BERUFSAUSLAGEN").addQueueRecordValue("F02",  budget1.pkz.berufsauslagen.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS1_WEITEREVERPFLICHTUNGEN").addQueueRecordValue("F02",  budget1.pkz.weitereauslagen.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS1_KRANKENKASSE").addQueueRecordValue("F02",  budget1.pkz.krankenkasse.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS1_ARBEITSWEGPAUSCHALE").addQueueRecordValue("F02",  budget1.pkz.arbeitswegpauschale1.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS1_EXTERNEBETREUUNGSKOSTEN").addQueueRecordValue("F02",  budget1.pkz.betreuungskosten.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS1_ZEKVERPFLICHTUNGEN").addQueueRecordValue("F02",  (budget1.pkz.kredtrate + budget1.pkz.leasingrate).GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS1_GRUNDBEDARF").addQueueRecordValue("F02",  kvaluesAS1.Grundbetrag.ToString(CultureInfo.InvariantCulture));
                bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS1_GRUNDBEDARFKIND").addQueueRecordValue("F02",  (kvaluesAS1.Kind1 + kvaluesAS1.Kind2 + kvaluesAS1.Kind3).ToString(CultureInfo.InvariantCulture));
                bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS1_SOZIALAUSLAGEN").addQueueRecordValue("F02",  (kvaluesAS1.Sozialausl1 + kvaluesAS1.Sozialausl2).ToString(CultureInfo.InvariantCulture));
                bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS1_STEUERN").addQueueRecordValue("F02",  kvaluesAS1.Steuern.ToString(CultureInfo.InvariantCulture));

                if (budget2 != null)
                {
                    if (budget2.pkz.einknettoFlag)
                    {
                        bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS2_EINKOMMEN").addQueueRecordValue("F02", budget2.pkz.einknetto.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                        bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS2_NEBENEINKOMMEN").addQueueRecordValue("F02", budget2.pkz.nebeinknetto.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                        bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS2_ZUSATZEINKOMMEN").addQueueRecordValue("F02", budget2.pkz.zeinknetto.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                        bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS2_MONATLICHEGRATIFIKATION").addQueueRecordValue("F02", (budget2.pkz.jbonusnetto.GetValueOrDefault() / 12.0).ToString(CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS2_EINKOMMEN").addQueueRecordValue("F02", budget2.pkz.einkbrutto.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                        bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS2_NEBENEINKOMMEN").addQueueRecordValue("F02", budget2.pkz.nebeinkbrutto.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                        bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS2_ZUSATZEINKOMMEN").addQueueRecordValue("F02", budget2.pkz.zeinkbrutto.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                        bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS2_MONATLICHEGRATIFIKATION").addQueueRecordValue("F02", (budget2.pkz.jbonusbrutto.GetValueOrDefault() / 12.0).ToString(CultureInfo.InvariantCulture));
                    }

                    bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS2_ENTHALTENEZULAGEN").addQueueRecordValue("F02",  (budget2.pkz.zulagesonst.GetValueOrDefault()+ budget2.pkz.zulagekind.GetValueOrDefault()+ budget2.pkz.zulageausbildung.GetValueOrDefault()).ToString(CultureInfo.InvariantCulture));
                    bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS2_ZULAGESONST").addQueueRecordValue("F02", budget2.pkz.zulagesonst.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                    bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS2_ZULAGEKIND").addQueueRecordValue("F02", budget2.pkz.zulagekind.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                    bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS2_ZULAGEAUSBILDUNG").addQueueRecordValue("F02", budget2.pkz.zulageausbildung.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));

                    bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS2_13MONATSLOHN").addQueueRecordValue("F02",  budget2.pkz.monatslohnXtdFlag?"1":"0");
                    
                    bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS2_REGAUSLAGEN").addQueueRecordValue("F02",  budget2.pkz.auslagen.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                    bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS2_UNTERSTUETZUNGSBEITRAEGE").addQueueRecordValue("F02",  budget2.pkz.unterhalt.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                    bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS2_BERUFSAUSLAGEN").addQueueRecordValue("F02",  budget2.pkz.berufsauslagen.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                    bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS2_WEITEREVERPFLICHTUNGEN").addQueueRecordValue("F02",  budget2.pkz.weitereauslagen.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                    bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS2_KRANKENKASSE").addQueueRecordValue("F02",  budget2.pkz.krankenkasse.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                    bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS2_ARBEITSWEGPAUSCHALE").addQueueRecordValue("F02",  budget2.pkz.arbeitswegpauschale1.GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                    bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS2_ZEKVERPFLICHTUNGEN").addQueueRecordValue("F02",  (budget2.pkz.kredtrate + budget2.pkz.leasingrate).GetValueOrDefault().ToString(CultureInfo.InvariantCulture));
                    bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS2_ISMA").addQueueRecordValue("F02", "1");
                    bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS2_STEUERN").addQueueRecordValue("F02", kvaluesAS1.Steuern2.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    bpeQueue.addQueueRecord("qBUDGETIN").addQueueRecordValue("F01", "AS2_ISMA").addQueueRecordValue("F02", "0");
                }

                List<CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto> queueResult = BPEBo.getQueueData(1, "SYSTEM", new String[] { "qBUDGETIN" }, ruleCode, vars,syswfuser, bpeQueue.getQueues());
                if (queueResult != null && queueResult.Count > 0)
                {
                    CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto outQueue = (from f in queueResult
                                                                                   where f.Name.Equals("qBUDGETOUT")
                                                                                   select f).FirstOrDefault();
                    foreach (CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto qr in outQueue.Records)
                    {
                        try
                        {
                            String fieldname = BPEBo.getQueueRecordValue(qr, "F01");
                            String fieldvalue = BPEBo.getQueueRecordValue(qr, "F02");

                            if ("AS1_BUDGETUEBERSCHUSS".Equals(fieldname))
                            {
                                rval.budget = double.Parse(fieldvalue, CultureInfo.InvariantCulture);
                            }
                           /* if (input.budget2 != null)
                            {
                                if ("AS2_BUDGETUEBERSCHUSS".Equals(fieldname))
                                {
                                    rval.budget2 = double.Parse(fieldvalue, CultureInfo.InvariantCulture);
                                }
                            }*/

                        }
                        catch (Exception re)
                        {
                            _log.Debug("Problem processing qBUDGETOUT-QueueRecord: " + re.Message);
                        }
                    }
                    rval.faktoren = new List<KremoLaufzeitFaktorDto>();

                    outQueue = (from f in queueResult
                                                                                   where f.Name.Equals("qFAKTOR")
                                                                                   select f).FirstOrDefault();
                    foreach (CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto qr in outQueue.Records)
                    {
                        try
                        {
                            String lz = BPEBo.getQueueRecordValue(qr, "F01");
                            String fak = BPEBo.getQueueRecordValue(qr, "F02");
                            KremoLaufzeitFaktorDto lzf = new KremoLaufzeitFaktorDto();
                            lzf.laufzeit = int.Parse(lz);
                            lzf.faktor = double.Parse(fak,CultureInfo.InvariantCulture);
                            rval.faktoren.Add(lzf);

                        }
                        catch (Exception re)
                        {
                            _log.Debug("Problem processing qFAKTOR-QueueRecord: " + re.Message);
                        }
                    }
 

                }
            }
            return rval;
        }

        /// <summary>
        /// gets the available budget for the given data
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        override
        public double getBudget(KREMOInDto inDto)
        {
            KREMOOutDto rval = getKremoValues(inDto);
            return getBudgetValue(rval);
        }

        /// <summary>
        /// Gets the budget-Value from the KREMO result
        /// </summary>
        /// <param name="outDto"></param>
        /// <returns></returns>
        override
        public double getBudgetValue(KREMOOutDto outDto)
        {
          
            double reduce = 10;
            String reduceParam = Cic.OpenOne.Common.Util.Config.AppConfig.Instance.GetEntry("GETBUDGET", "LIMITREDUCTION", reduce.ToString(), "KREMO");
            reduce = Double.Parse(reduceParam);

            double rval = outDto.kreditLimit - outDto.kreditLimit / 100 * reduce;

            String param = Cic.OpenOne.Common.Util.Config.AppConfig.Instance.GetEntry("GETBUDGET", "LIMIT_MAX", "1000000", "KREMO");
            double mValue = Double.Parse(param);
            if (rval > mValue)
            {
                rval = mValue;
            }

            return rval;
        }


        /// <summary>
        /// gets the Kremo values for the input 
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        override
        public KREMOOutDto getKremoValues(KREMOInDto inDto)
        {
            ArrayOfDouble in_Value = new ArrayOfDouble();
            ArrayOfDouble out_Value = new ArrayOfDouble();
            for (int i = 0; i < 208; i++)
            {
                out_Value.Add(0.0);
            }

            // Fülle in_Value Array mit Werten aus InDto
            MyFillInValue(ref in_Value, inDto);
            // KREMO WS Call (in_Value and out_Value will be filled)
            kremoWSDao.CallKremoByValues(ref in_Value, ref out_Value);

            KREMOOutDto outDto = new KREMOOutDto();
            MyFillOutDto(out_Value, outDto);

            return outDto;
        }


        /// <summary>
        /// Fills in_Value array with values from InDto and calls KREMOWSDao method CallKremoByValues()
        /// AUSKUNFT, KREMOINP, KREMO, KREMOOUT will be created
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns>KREMODto, filled with values returned by KREMO Webservice</returns>
        public override AuskunftDto callByValues(KREMOInDto inDto)
        {

            long returnCode;
            long code = codeTechExc;

            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.KREMOCallByValues);
            try
            {
                KREMOOutDto outDto;
                ArrayOfDouble in_Value = new ArrayOfDouble();
                ArrayOfDouble out_Value = new ArrayOfDouble();
                for (int i = 0; i < 200; i++)
                {
                    out_Value.Add(0.0);
                }

                // Fülle in_Value Array mit Werten aus InDto
                MyFillInValue(ref in_Value, inDto);

                // Save KREMO
                long sysKremo = kremoDBDao.SaveKREMOInDto(inDto);

                // Save KREMOInp
                kremoDBDao.SaveKREMOInp(sysAuskunft, sysKremo);
                code = codeSerAufExc;

                //For report
                kremoWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

                // KREMO WS Call (in_Value and out_Value will be filled)
                returnCode = kremoWSDao.CallKremoByValues(ref in_Value, ref out_Value);
                code = codeTechExc;
                outDto = new KREMOOutDto();
                outDto.ReturnCode = returnCode;
                MyFillOutDto(out_Value, outDto);

                // Save KREMOOut
                kremoDBDao.SaveKREMOOutDto(outDto, sysAuskunft, sysKremo);
                outDto.SysKremo = sysKremo;

                code = (long)outDto.ReturnCode;

                // Update Auskunft
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);
                auskunftdto.KremoOutDto = outDto;
                auskunftdto.requestXML = kremoWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = kremoWSDao.getSoapXMLDto().responseXML;
                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                throw new ApplicationException("Unexpected Exception in KREMO Webservice!", e);
            }

        }

        /// <summary>
        /// Fills in_Value array with values from InDto and calls KREMOWSDao method CallKremoByValues()
        /// AUSKUNFT, KREMOINP, KREMO already exist, only a Auskunft and Kremo will be updated
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>KREMODto, filled with values returned by KREMO Webservice</returns>
        public override AuskunftDto callByValues(long sysAuskunft)
        {
            long returnCode;
            long code = codeTechExc;

            // Get AuskunftDto
            AuskunftDto auskunftdto = auskunftDao.FindBySysId(sysAuskunft);

            try
            {
                KREMOOutDto outDto;
                KREMOInDto kremoindto = new KREMOInDto();
                ArrayOfDouble in_Value = new ArrayOfDouble();
                ArrayOfDouble out_Value = new ArrayOfDouble();
                for (int i = 0; i < 200; i++)
                {
                    out_Value.Add(0.0);
                }

                // Get KremoInDto
                kremoindto = kremoDBDao.FindBySysId(sysAuskunft);
                auskunftdto.KremoInDto = kremoindto;

                // Fill in_Value Array with values from kremoInDto
                MyFillInValue(ref in_Value, kremoindto);
                code = codeSerAufExc;

                // For Report
                kremoWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));

                // KREMO WS Call (in_Value and out_Value will be filled)
                returnCode = kremoWSDao.CallKremoByValues(ref in_Value, ref out_Value);
                code = codeTechExc;

                outDto = new KREMOOutDto();
                outDto.ReturnCode = returnCode;
                MyFillOutDto(out_Value, outDto);
                auskunftdto.KremoOutDto = outDto;
                // Save KREMOOut
                kremoDBDao.SaveKREMOOutDto(outDto, sysAuskunft, (long)kremoindto.SysKremo);

                code = (long)outDto.ReturnCode;

                // Update Auskunft
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftdto, code);
                auskunftdto.requestXML = kremoWSDao.getSoapXMLDto().requestXML;
                auskunftdto.responseXML = kremoWSDao.getSoapXMLDto().responseXML;
                return auskunftdto;
            }
            catch (Exception e)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                throw new ApplicationException("Unexpected Exception in KREMO Webservice!", e);
            }
        }

        /// <summary>
        /// Calls KREMOWSDao method getVersion()
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns>KREMODto filled with Version number returned by KREMO Webservice</returns>
        public override KREMOOutDto getVersion(KREMOInDto inDto)
        {
            KREMOOutDto kremoOutDto = new KREMOOutDto();
            kremoOutDto.Version = kremoWSDao.CallKremoGetVersion();
            return kremoOutDto;
        }

        /// <summary>
        /// Methode nicht implementiert, da nur Versionsnummer zurückgegeben wird. 
        /// Es lohnt sich nicht dafür AUSKUNFT, KREMOINP, KREMO, KREMOOUT in DB anzulegen
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public override AuskunftDto getVersion(long sysAuskunft)
        {
            throw new NotImplementedException();
        }

        #region Private methods
        /// <summary>
        /// Fills in_Value Array 
        /// </summary>
        /// <param name="in_Value"></param>
        /// <param name="inDto"></param>
        private void MyFillInValue(ref ArrayOfDouble in_Value, KREMOInDto inDto)
        {
            in_Value.Add(inDto.GebDatum); //0
            in_Value.Add(inDto.Anredecode); //1
            in_Value.Add(inDto.Kalkcode); //2
            in_Value.Add(inDto.Glz); //3
            in_Value.Add(inDto.Kreditsumme); //4
            in_Value.Add(inDto.Rw); //5
            in_Value.Add(inDto.Zins); //6
            in_Value.Add(inDto.Zinsnomflag); //7
            in_Value.Add(inDto.Einkbrutto); //8
            in_Value.Add(inDto.Einknetto); //9
            in_Value.Add(Default); //10
            in_Value.Add(inDto.Famstandcode); //11
            in_Value.Add(inDto.Grundcode); //12
            in_Value.Add(inDto.Kantoncode); //13
            in_Value.Add(inDto.Anzkind1); //14
            in_Value.Add(inDto.Anzkind2); //15
            in_Value.Add(inDto.Anzkind3); //16
            for (int i = 17; i < 31; i++) //17 - 30
            {
                in_Value.Add(Default);
            }
            in_Value.Add(inDto.Miete); //31
            in_Value.Add(inDto.Nebeneinknetto);  //32
            in_Value.Add(inDto.Plz); //33
            in_Value.Add(Default); //34
            in_Value.Add(inDto.Unterhalt); //35
            for (int i = 36; i < 41; i++)
            {
                in_Value.Add(Default);
            }
            in_Value.Add(inDto.Qstflag); //41
            in_Value.Add(inDto.Nebeneinkbrutto); //42
            for (int i = 43; i < 49; i++) //43-48
            {
                in_Value.Add(Default);
            }
            in_Value.Add(inDto.Anzkind4); //49
            in_Value.Add(inDto.GebDatum2); //50
            in_Value.Add(inDto.Anredecode2); //51
            in_Value.Add(inDto.Einkbrutto2); //52
            in_Value.Add(inDto.Einknetto2); //53
            in_Value.Add(inDto.Famstandcode2); //54
            in_Value.Add(inDto.Kantoncode2); //55
            for (int i = 56; i < 70; i++) //56-69
            {
                in_Value.Add(Default);
            }
            in_Value.Add(inDto.Nebeneinknetto2); //70
            in_Value.Add(inDto.Plz2); //71
            in_Value.Add(Default); //72
            in_Value.Add(inDto.Unterhalt2);//73
            in_Value.Add(inDto.Qstflag2); //74
            in_Value.Add(Default); //75
            in_Value.Add(Default); //76
            in_Value.Add(Default); //77
            in_Value.Add(Default); //78
            in_Value.Add(inDto.Nebeneinkbrutto2); //79
            for (int i = 80; i < 84; i++)
            {
                in_Value.Add(Default);
            }
            in_Value.Add(inDto.Fininstcode); // 84
            for (int i = 85; i < 100; i++)
            {
                in_Value.Add(Default);
            }
            /*in_Value[0] = inDto.GebDatum;
              in_Value[1] = inDto.Anredecode;
              in_Value[2] = inDto.Kalkcode;
              in_Value[3] = inDto.Glz;
              in_Value[4] = inDto.Kreditsumme;
              in_Value[5] = inDto.Rw;
              in_Value[6] = inDto.Zins;
              in_Value[7] = inDto.Zinsnomflag;
              in_Value[8] = inDto.Einkbrutto;
              in_Value[9] = inDto.Einknetto;
              in_Value[10] = inDto.Ersatz;
              in_Value[11] = inDto.Famstandcode;
              in_Value[12] = inDto.Grundcode;
              in_Value[13] = inDto.Kantoncode;
              in_Value[14] = inDto.Anzkind1;
              in_Value[15] = inDto.Anzkind2;
              in_Value[16] = inDto.Anzkind3;
              in_Value[31] = inDto.Miete;
              in_Value[33] = inDto.Plz;
              in_Value[35] = inDto.Unterhalt;
              in_Value[41] = inDto.Qstflag;
              in_Value[49] = inDto.Anzkind4;
              in_Value[50] = inDto.GebDatum2;
              in_Value[51] = inDto.Anredecode2;
              in_Value[52] = inDto.Einkbrutto2;
              in_Value[54] = inDto.Famstandcode2;
              in_Value[55] = inDto.Kantoncode2;
              in_Value[71] = inDto.Plz2;
              in_Value[73] = inDto.Unterhalt2;*/
        }

        /// <summary>
        /// Fills out_value Array
        /// </summary>
        /// <param name="out_Value"></param>
        /// <param name="outDto"></param>
        private void MyFillOutDto(ArrayOfDouble out_Value, KREMOOutDto outDto)
        {
            outDto.Grundbetrag = out_Value[107];
            outDto.Kind1 = out_Value[108];
            outDto.Kind2 = out_Value[109];
            outDto.Kind3 = out_Value[110];
            outDto.Berechkrankenkasse = out_Value[142];
            outDto.Sozialausl1 = out_Value[129];
            outDto.Steuern = out_Value[131];
            outDto.Sozialausl2 = out_Value[163];
            outDto.Steuern2 = out_Value[165];
            outDto.saldoKorr36 = out_Value[120];
            outDto.kreditLimit = out_Value[206];
        }
        #endregion
    }

}
