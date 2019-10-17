using System;
using System.Collections.Generic;
using System.Reflection;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.Common.Util.Exceptions;
using CIC.Database.PRISMA.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    public class IncentivierungBo : IIncentivierungBo
    {
        private static readonly ILog log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private IIncentivierungDao dao;
        /// <summary>
        /// expiring date of cummulating bonus levels
        /// </summary>
        public DateTime ValidUntil { get; set; }
      

     

        public IncentivierungBo(IIncentivierungDao dao)
        {
            this.dao = dao;
          
        }

   
        /// <summary>
        /// creates the incentive provisions for the given provision context 
        /// </summary>
        /// <param name="ctx"></param>
        public void createProvisions(provKontextDto ctx)
        {
            if(ctx.sysvt==0)
            {
                log.Warn("No Contract given in createProvisions' context");
                return;
            }
            IProvisionBo provBo = PrismaBoFactory.getInstance().createProvisionBo();
            //long sysprprovset = provBo.getProvisionPlan(ctx, 0);
            PRPROVSET provPlan = provBo.getProvisionsPlan(ctx.sysperole,ctx.perDate);
            if (provPlan==null ||provPlan.SYSPRPROVSET == 0)
            {

                log.Warn("There is no provision plan for the user " + ctx.sysperole +" for date "+ctx.perDate);
                return;
            }

            //get gesamtumsatz for VP
            double gesamtumsatz = dao.getProvPlanUmsatz(ctx.sysperole, provPlan.SYSPRPROVSET, ctx.sysvt);
            
            VTIncentivierungDataDto vtdata= dao.getContractData(ctx.sysvt);
            if (vtdata == null)
            {
                throw new NullReferenceException("Es sind keine Informationen zum Vertrag mit ID " + ctx.sysvt + " verfügbar.");
            }
            //Auf dem Verkäufer wird die bisherige Händler-Segmentierung ermittelt 
            String segment = dao.getSegmentation(ctx.sysperole);//gesamtumsatz, ctx.sysprkgroup, ctx.perDate);
            List<ProvKalkDto> tracing = new List<ProvKalkDto> ();
            List<AngAntProvDto> provs = new List<AngAntProvDto>();
            provs.AddRange(calcProvisions(ProvisionSourceField.VT_GESAMTUMSATZ, gesamtumsatz, provBo, ctx, gesamtumsatz.ToString(), segment, tracing));
            provs.AddRange(calcProvisions(ProvisionSourceField.VT_PPI_STK, 0, provBo, ctx, "" + vtdata.anzInsurance, segment, tracing));
            provs.AddRange(calcProvisions(ProvisionSourceField.VT_UMSATZ, vtdata.umsatz, provBo, ctx, vtdata.umsatz.ToString(), segment, tracing));

			foreach(ProvKalkDto pk in tracing)
            {
                if (pk.remark == null)
                {
                    pk.remark = "";
                }
                pk.remark += " " + segment;
            }
            //save provs
            dao.createIncentiveProvisions(provs, tracing);
        }

        /// <summary>
        /// Iterates over all provisiontypes and all provisioned prflds and creates provisions
        /// the calculation base values are fetched by the prfld-objectmeta-codes that are linked to the values by convention
        /// </summary>
        /// <param name="field">Feld</param>
        /// <param name="baseValue">Basiswert</param>
        /// <param name="provBo">Provisionen BO</param>
        /// <param name="provKontext">Provisionenkontext</param>
        /// <param name="vgXValue">VG lookup X-Value</param>
        /// <param name="vgYValue">VG lookup Y-Value</param>
        /// <param name="tracing">Tracing steps list</param>
        /// <returns></returns>
        private List<AngAntProvDto> calcProvisions(ProvisionSourceField field, double baseValue, IProvisionBo provBo, provKontextDto provKontext,  String vgXValue, String vgYValue, List<ProvKalkDto> tracing)
        {
            List<AngAntProvDto> rval = new List<AngAntProvDto>();

            //input: provisionSourceField
            //-> gets ParamDto -> prfld
            //if not in prflds return
            //optional sysvstyp/sysabltyp
            long FieldId = provBo.getPrfldId(field, null);
            if (FieldId == 0)
            {
                return rval;
            }

            //get all prprovtypes for the given prisma field (provisionSource)
            List<PRPROVTYPE> provtypes = provBo.getProvisionTypes(FieldId);
            iProvisionDto provParam = new iProvisionDto();
            provParam.interpolationMode = OpenOne.Common.DAO.VGInterpolationMode.NONE;
            provParam.vgQueryMode = OpenOne.Common.DAO.plSQLVersion.V2;
            provParam.vgXValue = vgXValue;
            provParam.vgYValue = vgYValue;

            foreach (PRPROVTYPE ptype in provtypes)
            {
                provParam.sysprprovtype = ptype.SYSPRPROVTYPE;
                provParam.sysprfld = FieldId;
                provParam.provisionInputValue = baseValue;
                provParam.provType = ProvGroupAssignType.AUTO;
                provParam.useProvisionOverwriteValue = false;
                provParam.provisionOverwriteValue = 0;
                //CR 196 - new field mapped
                provParam.defRoleType = (int)ptype.SYSDEFROLETYPE;
              
                List<AngAntProvDto> provs = provBo.calculateIncentiveProvision(provKontext, provParam, tracing);
                if (provs != null)
                {
                    rval.AddRange(provs);
                }
            }
            IRounding round = RoundingFactory.createRounding();
            foreach (AngAntProvDto dto in rval)
            {
                dto.provisionOrg = dto.provision;
                dto.provision = round.RoundCHF(dto.provision);
                dto.provisionBrutto = round.RoundCHF(dto.provisionBrutto);
                dto.defaultprovision = round.RoundCHF(dto.defaultprovision);
                dto.defaultprovisionbrutto = round.RoundCHF(dto.defaultprovisionbrutto);
            }
            return rval;
        }


      
        /// <summary>
        /// Get data displayed in the My Pocket panel for the seller
        /// </summary>
        /// <param name="sysperole">seller for whom the MyPocket shall be displayed</param>
        /// <returns>MyPocket data for seller</returns>
        public MyPocketDto GetPocket(long sysperole)
        {
            MyPocketDto rval = new MyPocketDto();
            IProvisionBo provBo = PrismaBoFactory.getInstance().createProvisionBo();

            rval.sys_seller = sysperole;
            PRPROVSET provPlan = provBo.getProvisionsPlan(sysperole, DateTime.Today);
            if (provPlan == null)
            {
                throw new ServiceBaseException("F_00003_ArgumentException","There is no provision plan for the user ",MessageType.Warn);// + sysperole);
            }
            if (provPlan.VALIDUNTIL.HasValue)
            {
                List<string> monatestring = new List<string> { "1", "3", "5", "7", "10", "12" };
                rval.rlz_monate = (provPlan.VALIDUNTIL.Value.Year * 12 + provPlan.VALIDUNTIL.Value.Month) - (DateTime.Today.Year * 12 + DateTime.Today.Month);
                rval.rlz_tage = (provPlan.VALIDUNTIL.Value - DateTime.Today.AddMonths(rval.rlz_monate)).Days;
                if (rval.rlz_tage < 0)
                {
                    rval.rlz_monate = rval.rlz_monate - 1;
                    if (monatestring.Contains(provPlan.VALIDUNTIL.Value.Month.ToString()))
                    {
                        rval.rlz_tage = 30 + rval.rlz_tage;
                    }
                    else
                    {
                        rval.rlz_tage = 31 + rval.rlz_tage;
                    }
                }
                rval.rlz_tage = rval.rlz_tage + 1;
            }
            
            //Sysprfld
            long sysprfld = provBo.getPrfldId(ProvisionSourceField.VT_GESAMTUMSATZ, null);
            long sysvg = dao.getSysvg(provPlan.SYSPRPROVSET, sysprfld);

            List<VGValuesDto> vgvalues = dao.getSegmentations(sysvg);


            //segmentname
            string segment = dao.getSegmentation(sysperole);
            string segmentNext = "";
            rval.hd_segment_aktuell = dao.getSegmentationName(segment, DateTime.Today);
            //gesamtumsatz (bisher mit vt umgesetzt, also x-achse
            double gesamtumsatz = dao.getProvPlanUmsatz(sysperole, provPlan.SYSPRPROVSET, -1);

            rval.next_target_kickback = 0;
            //get next provision
            bool foundCurrent = false;
            bool foundnextrange= false;
            String aktSegment = null;//Segment nach aktuellem Segment

            //LINESCALE: HDGR1_BASIS, HDGR1_BRONZE, sortiert nach linescale aufsteigend, darin x1/v1 absteigend
            //X1: Gesamtumsatz, pro linescale 1.5mio, 1Mio, 500t, 200t, 0
            //V1: Provision bei X1, 2000,1500,1000,300,0
            //X2:
            /*
             *  LINESCALE   COLSCALE V1     X1      Y1          V2      X2
             *  HDGR1_BASIS	0	    0	    0	    HDGR1_BASIS	300	    200000
                HDGR1_BASIS	200000	300	    200000	HDGR1_BASIS	1000	500000
                HDGR1_BASIS	500000	1000	500000	HDGR1_BASIS	1500	1000000
                HDGR1_BASIS	1000000	1500	1000000	HDGR1_BASIS	2000	1500000
                HDGR1_BASIS	1500000	2000	1500000	HDGR1_BASIS		    Double.MaxValue
             */
            foreach (VGValuesDto vgvalue in vgvalues)
            {
                //continue loop until we find the next linescale after our current segment
                if (SegmentSmallerThan(vgvalue.LineScale, segment))
                {
                    continue; //We need the next, not the previous
                }

                //get von-bis and provision for the current row of the linescale
                double von = Convert.ToDouble(vgvalue.X1);
                double bis = vgvalue.X2 == null || vgvalue.X2 == "" ? Double.MaxValue : Convert.ToDouble(vgvalue.X2);
                double prov = Convert.ToDouble(vgvalue.V1);

                //Erstes Mal, wenn AktSegment gefunden wurde und die Funktion auf dem anderen Segment steht, wird der NextSegment gesetzt und Funktion verlassen
                if (aktSegment != null && !aktSegment.Equals(vgvalue.LineScale))
                {
                    segmentNext = vgvalue.LineScale;
                    break;
                }

                if (!foundCurrent && (gesamtumsatz < bis))
                {                                   
                    //the provision in this range can be the latest provision already paid
                    foundCurrent = true;
                    aktSegment = vgvalue.LineScale;


                    continue;
                }

             

                //go on if there is no bonus. But only if this is not the last element in the list so we still have values
                //if (prov == 0 && vgvalues[vgvalues.Count - 1] != vgvalue)
                //    continue;

                //Fix mit Stas -04.02.2016
                //Erstes Mal nachdem der  Current gefunden wurde, werden die Variablen für den "Schritt Rechts in der Tabelle" befüllt
                if (foundCurrent && !foundnextrange)
                {
                    rval.next_payout_kickback = prov;
                    rval.next_target_kickback = von;
                    foundnextrange = true;
                }

                
            }
            rval.hd_segment_next = dao.getSegmentationName(segmentNext, DateTime.Today);
            if (rval.hd_segment_next == null)
            {
                rval.hd_segment_next = "";
            }

            rval.gap_value_kickback = rval.next_target_kickback - gesamtumsatz;
            
            if (rval.gap_value_kickback < 0)
            {
                rval.gap_value_kickback = 0;
            }

            rval.report_vk_incentives = dao.getDocumentId();
           
            rval.akt_payout_total = dao.getAktpayoutTotal(sysperole, provPlan.SYSPRPROVSET);

            rval.akt_volume_kickback = gesamtumsatz;

            /*
            if (!CurrentStatus.ContainsKey(sysperole))
                CurrentStatus.Add(sysperole, new IncentivierungStatusDto { AnzahlVertraege = 0, Finanzierungsvolumen = 0 });
            IncentivierungStatusDto currentStatus = CurrentStatus[sysperole];

            MyPocketDto rval = new MyPocketDto();
            if (ValidUntil != null)
            {
                rval.rlz_monate = (ValidUntil.Year * 12 + ValidUntil.Month) - (DateTime.Today.Year * 12 + DateTime.Today.Month);
                rval.rlz_tage = (ValidUntil - DateTime.Today.AddMonths(rval.rlz_monate)).Days;
            }

            rval.hd_segment_aktuell = GetLevel(currentStatus);
            rval.hd_segment_next = GetNextLevel(rval.hd_segment_aktuell);

            rval.next_target_kickback = Matrix.Matrix[rval.hd_segment_next].Finanzierungsvolumen;
            rval.gap_value_kickback = rval.next_target_kickback - currentStatus.Finanzierungsvolumen;

            rval.next_payout_kickback = Matrix.Matrix[rval.hd_segment_next].Provisionsbetrag;
            rval.akt_payout_total = currentStatus.BonusTotal;

            rval.sys_seller = sysperole;
            */
            return rval;
        }

        /// <summary>
        /// tells wheather the first segment is less valuable than the second segment
        /// </summary>
        /// <param name="seg1">first segment code</param>
        /// <param name="seg2">second segment code</param>
        /// <returns>the first segment is less valuable than the second segment</returns>
        private static bool SegmentSmallerThan(string seg1, string seg2)
        {
            //every segment code starts with "HDGR", followed by a digit.
            //The digit tells us what's more valuable. So we can just use default string comparison.
            return seg1.CompareTo(seg2) < 0;
        }
    }
}