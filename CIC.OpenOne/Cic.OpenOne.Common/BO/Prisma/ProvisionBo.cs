using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.BO.Prisma
{
    /// <summary>
    /// Bo for Provisions
    /// </summary>
    [System.CLSCompliant(true)]
    public class ProvisionBo : AbstractProvisionBo
    {
        #region Private variables
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private Dictionary<ConditionLinkType, string> mapConditionLinkToParameter = new Dictionary<ConditionLinkType, string>();//Maps ConditionLinkType to the given parameter name
        private Dictionary<ConditionLinkType, long> mapConditionLinkToProvisionTrigger = new Dictionary<ConditionLinkType, long>();//Maps ConditionLinkType to the trigger id


        private ConditionLinkType[] supportedLinks = { ConditionLinkType.COMMON, ConditionLinkType.BRAND, ConditionLinkType.OBTYP, ConditionLinkType.PRHGROUP, ConditionLinkType.PRKGROUP, ConditionLinkType.PEROLE };
        private ConditionLinkType[] supportedLinksProvPlan = { ConditionLinkType.COMMON, ConditionLinkType.BRAND, ConditionLinkType.OBTYP, ConditionLinkType.OBART, ConditionLinkType.USETYPE, ConditionLinkType.VART, ConditionLinkType.BCHNL };
        private DateTime nullDate = new DateTime(1800, 1, 1);
        private List<long> possiblePrflds;
        private Dictionary<ConditionLinkType, long> mapConditionLinkToPRPROVSETArea = new Dictionary<ConditionLinkType, long>();//Maps ConditionLinkType to the area id

        #endregion

        #region Constructors
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="dao">Provisionen DAO</param>
        /// <param name="obDao">Objekttyp DAO</param>
        /// <param name="prismaParameterBo">Prisma Parameter BO</param>
        /// <param name="vgDao">Wertegruppen DAO</param>
        public ProvisionBo(IProvisionDao dao, IObTypDao obDao, IPrismaParameterBo prismaParameterBo, IVGDao vgDao)
            : base(dao, obDao, prismaParameterBo, vgDao)
        {

            mapConditionLinkToProvisionTrigger[ConditionLinkType.COMMON] = 99;
            //map the conditionlinkType to the parameter-Name of the ProvisionContext
            mapConditionLinkToParameter[ConditionLinkType.BRAND] = "sysbrand";
            mapConditionLinkToProvisionTrigger[ConditionLinkType.BRAND] = 0;

            mapConditionLinkToParameter[ConditionLinkType.PRHGROUP] = "sysprhgroup";
            mapConditionLinkToProvisionTrigger[ConditionLinkType.PRHGROUP] = 2;

            mapConditionLinkToParameter[ConditionLinkType.OBTYP] = "sysobtyp";
            mapConditionLinkToProvisionTrigger[ConditionLinkType.OBTYP] = 1;

            mapConditionLinkToParameter[ConditionLinkType.PRKGROUP] = "sysprkgroup";
            mapConditionLinkToProvisionTrigger[ConditionLinkType.PRKGROUP] = 3;

            mapConditionLinkToParameter[ConditionLinkType.PEROLE] = "sysperole";
            mapConditionLinkToProvisionTrigger[ConditionLinkType.PEROLE] = 4;

            mapConditionLinkToParameter[ConditionLinkType.PEROLE] = "sysperole";
            mapConditionLinkToProvisionTrigger[ConditionLinkType.PEROLE] = 4;

            mapConditionLinkToParameter[ConditionLinkType.VART] = "sysvart";
            mapConditionLinkToParameter[ConditionLinkType.VARTTAB] = "sysvarttab";
            mapConditionLinkToParameter[ConditionLinkType.VTTYP] = "sysvttyp";
            mapConditionLinkToParameter[ConditionLinkType.PRODUCT] = "sysprproduct";
            mapConditionLinkToParameter[ConditionLinkType.VSTYP] = "sysvstyp";
            mapConditionLinkToParameter[ConditionLinkType.FSTYP] = "sysfstyp";
            mapConditionLinkToParameter[ConditionLinkType.BCHNL] = "sysprchannel";
            mapConditionLinkToParameter[ConditionLinkType.OBART] = "sysobart";
            mapConditionLinkToParameter[ConditionLinkType.USETYPE] = "sysprusetype";
            

            possiblePrflds = dao.getProvisionedPrFlds();

            //Maps the ConditionLink-Type to the db-value
            mapConditionLinkToPRPROVSETArea[ConditionLinkType.BRAND] = 2;
            mapConditionLinkToPRPROVSETArea[ConditionLinkType.BCHNL] = 3;
            mapConditionLinkToPRPROVSETArea[ConditionLinkType.PRHGROUP] = 20;
            mapConditionLinkToPRPROVSETArea[ConditionLinkType.PRKGROUP] = 40;
            mapConditionLinkToPRPROVSETArea[ConditionLinkType.NONE] = 99999;
            mapConditionLinkToPRPROVSETArea[ConditionLinkType.OBTYP] = 31;
            mapConditionLinkToPRPROVSETArea[ConditionLinkType.OBART] = 30;
            mapConditionLinkToPRPROVSETArea[ConditionLinkType.USETYPE] = 70;
            mapConditionLinkToPRPROVSETArea[ConditionLinkType.LS] = 1;
            mapConditionLinkToPRPROVSETArea[ConditionLinkType.VART] = 10;
            mapConditionLinkToPRPROVSETArea[ConditionLinkType.VARTTAB] = 11;
            mapConditionLinkToPRPROVSETArea[ConditionLinkType.VTTYP] = 12;
            mapConditionLinkToPRPROVSETArea[ConditionLinkType.KALKTYP] = 13;
            mapConditionLinkToPRPROVSETArea[ConditionLinkType.PRODUCT] = 14;
            mapConditionLinkToPRPROVSETArea[ConditionLinkType.PEROLE] = 21;
            mapConditionLinkToPRPROVSETArea[ConditionLinkType.KDTYP] = 41;
            mapConditionLinkToPRPROVSETArea[ConditionLinkType.FSTYP] = 50;
            mapConditionLinkToPRPROVSETArea[ConditionLinkType.VSTYP] = 60;
            mapConditionLinkToPRPROVSETArea[ConditionLinkType.INTTYPE] = 9999;//not yet used
            mapConditionLinkToPRPROVSETArea[ConditionLinkType.COMMON] = 99999;

        }
        #endregion

        /// <summary>
        /// returns all configured Provisiontypes
        /// </summary>
        /// <returns>Parameter Condition List</returns>
        override public List<PRPROVTYPE> getProvisionTypes()
        {
            return dao.getProvisionTypes();
        }

        /// <summary>
        /// returns all configured Provisiontypes
        /// </summary>
        /// <returns>Parameter Condition List</returns>
        override public List<PRPROVTYPE> getProvisionTypes(long sysprfld)
        {
            return dao.getProvisionTypes(sysprfld);
        }
        

        /// <summary>
        /// Returns a list of all prisma fields that have a provision configured
        /// </summary>
        /// <returns></returns>
        override public List<long> getPrFlds()
        {
            return possiblePrflds;
        }

        /// <summary>
        /// Calculates the Provisions by the given rank
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        override public List<AngAntProvDto> calculateProvision(provKontextDto ctx, iProvisionDto param)
        {
            //avoid computing time when iterating over many provisioned fields that are not configured
            if (!possiblePrflds.Contains(param.sysprfld))
                return null;
            if (!dao.validProvision(param.sysprfld, param.sysprprovtype))
                return null;

            double provisionvalue = param.provisionOverwriteValue;

            //1. Provisionsschritte für kontext ermitteln-----------------------------------------------------------------------------------------
            List<PRPROVSTEP> steps = dao.getProvsteps(ctx.sysprhgroup, ctx.sysperole, ctx.perDate, param.sysprprovtype, param.provType);
            //hier holen wir uns auch den Provisionsplan für den händler/verkäufer
            long? prprovset = (from x in steps
                         select x.SYSPRPROVSET).Distinct().FirstOrDefault();

          

            //Verfügbarkeit Provisionsplan ermitteln (PRPROVSET + PRCLPROVSET)
            long sysprprovset = prprovset.HasValue?prprovset.Value:0;

            //Filter for given field
            steps = (from s in steps
                     where s.SYSPRFLD == param.sysprfld
                     select s).ToList();

            if (steps.Count > 0)
            {
                List<long> obtypen = obDao.getObTypAscendants(ctx.sysobtyp);

                steps = (from s in steps
                         where
                                (s.SYSVART == ctx.sysvart || s.SYSVART == null)
                                && (s.SYSVARTTAB == ctx.sysvarttab || s.SYSVARTTAB == null)
                                && (s.SYSVTTYP == ctx.sysvttyp || s.SYSVTTYP == null)
                                && (s.SYSVSTYP == ctx.sysvstyp || s.SYSVSTYP == null)
                                && (s.SYSPRPRODUCT == ctx.sysprproduct || s.SYSPRPRODUCT == null)
                                && (s.SYSBRAND == ctx.sysbrand || s.SYSBRAND == null)
                                && (s.SYSPRHGROUP == ctx.sysprhgroup || s.SYSPRHGROUP == null)
                                && ((s.SYSOBTYP.HasValue && obtypen.Contains(s.SYSOBTYP.Value)) || s.SYSOBTYP == null)
                         select s).ToList();
            }

            _log.Debug("Calc Provision for SYSPRFLD " + param.sysprfld + " steps: " + steps.Count);
            //Provisionsbetrag ermittlen
            ProvisionValuePair pvalPair = null;
            if (steps.Count > 0)
                pvalPair = getProvisionValue(ctx, param, steps,null);

            double defaultProvision = steps.Count > 0 ? pvalPair.value: 0;
            double defaultProvisionP = steps.Count > 0 ? pvalPair.percentage : 0;
            if (!param.useProvisionOverwriteValue)
            {
                provisionvalue = defaultProvision;
            }

            List<AngAntProvDto> rval = new List<AngAntProvDto>();
            if (provisionvalue == 0 && defaultProvisionP == 0) return null;

            long haendler = obDao.getHaendlerByEmployee(ctx.sysperole);
            bool bfail = distributeProvisions(ctx, param, provisionvalue, rval, haendler, defaultProvision, pvalPair, null,sysprprovset);

            if (bfail)//aufteilung nicht möglich
                return null;
            else
                return rval;
        }

      

        /// <summary>
        /// Calculates the Provisions for incentives
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="param"></param>
        /// <param name="tracing">traces all steps</param>
        /// <returns></returns>
        override public List<AngAntProvDto> calculateIncentiveProvision(provKontextDto ctx, iProvisionDto param, List<ProvKalkDto> tracing)
        {
            //avoid computing time when iterating over many provisioned fields that are not configured
            if (!possiblePrflds.Contains(param.sysprfld))
                return null;
            if (!dao.validProvision(param.sysprfld, param.sysprprovtype))
                return null;

            double provisionvalue = param.provisionOverwriteValue;
            List<ProvKalkDto> mytracing = new List<ProvKalkDto>();

            //incentive Provisionsschritte für kontext ermitteln-----------------------------------------------------------------------------------------
            //Auf dem Händler wird der aktuell gültige Provisionsplan ermittelt (PRCLVPPROVSET+PRPROVSET+PRPROVSTEP+PRPROVTYPE)
            List<PRPROVSTEP> steps = dao.getProvstepsInc(ctx.sysprhgroup,ctx.sysperole, ctx.perDate, param.sysprprovtype);
            //hier holen wir uns auch den Provisionsplan für den händler/verkäufer
            long? prprovset = (from x in steps
                         select x.SYSPRPROVSET).Distinct().FirstOrDefault();

            if (!prprovset.HasValue) return null;

            //Verfügbarkeit Provisionsplan ermitteln (PRPROVSET + PRCLPROVSET)
            long sysprprovset = prprovset.Value;
            sysprprovset = getProvisionPlan(ctx, sysprprovset);
            if (sysprprovset == 0) return null;

            //Filter for given field and provisionplan
            steps = (from s in steps
                     where s.SYSPRFLD == param.sysprfld
                     && s.SYSPRPROVSET==sysprprovset
                     select s).ToList();

            if (steps.Count > 0)
            {
                List<long> obtypen = obDao.getObTypAscendants(ctx.sysobtyp);

                steps = (from s in steps
                         where
                                (s.SYSVART == ctx.sysvart || s.SYSVART == null)
                                && (s.SYSVARTTAB == ctx.sysvarttab || s.SYSVARTTAB == null)
                                && (s.SYSVTTYP == ctx.sysvttyp || s.SYSVTTYP == null)
                                && (s.SYSVSTYP == ctx.sysvstyp || s.SYSVSTYP == null)
                                && (s.SYSPRPRODUCT == ctx.sysprproduct || s.SYSPRPRODUCT == null)
                                && (s.SYSBRAND == ctx.sysbrand || s.SYSBRAND == null)
                                && (s.SYSPRHGROUP == ctx.sysprhgroup || s.SYSPRHGROUP == null)
                                && ((s.SYSOBTYP.HasValue && obtypen.Contains(s.SYSOBTYP.Value)) || s.SYSOBTYP == null)
                         select s).ToList();
            }

            _log.Debug("Calc Provision for SYSPRFLD "+param.sysprfld+" steps: "+steps.Count);
            //Provisionsbetrag ermittlen
            ProvisionValuePair pvalPair = null;
            long oldbrand = ctx.sysbrand;
            ctx.sysbrand = 0;
            if (steps.Count > 0)
                pvalPair = getProvisionValue(ctx, param, steps, mytracing);
            ctx.sysbrand = oldbrand;

            double defaultProvision = steps.Count > 0 ? pvalPair.value : 0;
            double defaultProvisionP = steps.Count > 0 ? pvalPair.percentage : 0;
            if (!param.useProvisionOverwriteValue)
            {
                provisionvalue = defaultProvision;
            }

            List<AngAntProvDto> rval = new List<AngAntProvDto>();
            if (provisionvalue == 0 && defaultProvisionP == 0) return null;

            long haendler = obDao.getHaendlerByEmployee(ctx.sysperole);
            bool bfail = distributeProvisions(ctx, param, provisionvalue, rval, haendler, defaultProvision, pvalPair, mytracing, sysprprovset);

            if (tracing != null)//update rang
            {
                int rang = 0;
                foreach (ProvKalkDto k in mytracing)
                {
                    k.rang = rang;
                    rang += 10;
                    if(k.prov==null && rval.Count>0)
                    {
                        k.prov = rval[0];
                    }
                }
                tracing.AddRange(mytracing);
            }

            if (bfail)//aufteilung nicht möglich
                return null;
            else
                return rval;
        }

        /// <summary>
        /// Returns the current provision plan (prprovset) id for the given provision context
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="sysprprovset"></param>
        /// <returns></returns>
        override
            public long getProvisionPlan(provKontextDto ctx, long sysprprovset)
        {
            //provision plans for context--------------------------------------------------------------------------------------
            //get all provsets, valid 
            //for all provsets check conditions in this order:
            /*area 
            99		none
            0	sysbrand
            1	sysobtyp
            3	sysprkgroup
            2	sysprhgroup*/
            List<ProvisionConditionLink> allparamLinks = dao.getProvisionConditionLinks();//prprovset
            IEnumerable<long> prProvSets = new List<long>();
            allparamLinks = (from a in allparamLinks
                             where 
                             a.sysprprovset == sysprprovset && 
                             (a.VALIDFROM == null || a.VALIDFROM <= ctx.perDate || a.VALIDFROM <= nullDate)
                              && (a.VALIDUNTIL == null || a.VALIDUNTIL >= ctx.perDate || a.VALIDUNTIL <= nullDate)
                             select a).ToList();

            //enumerate conditions from context, every of supportedLinks is a must-have parameter
            bool isValid;
            bool isGepflegt;

            foreach (ConditionLinkType ctype in supportedLinksProvPlan)
            {
                isValid = false;
                isGepflegt = false;
                long area = mapConditionLinkToPRPROVSETArea[ctype];
                try
                {
                    List<long> ContextList = ConditionLink.getParameter(ctx, ctype, mapConditionLinkToParameter, obDao, ctx.perDate);
                    prProvSets = getAvailabilityByArea(prProvSets, allparamLinks, ContextList, ctx.perDate, area, ref isValid, ref isGepflegt);
                    //BNRVZ-649
                    if (isGepflegt && !isValid) return 0;
                }

                catch (MethodAccessException ex)
                {
                    //explicitely not handled, we ignore the parameter
                    _log.Debug("Explicitly unhandled Exception: " + ex.Message);
                }
            }



            //prProvSets now contains the sysprprovset of all valid plans
            //provision plans for context END--------------------------------------------------------------------------------------
            if (prProvSets.Count() == 0) return 0;

            return prProvSets.First();
        }

        /// <summary>
        /// Takes all ConditionLinks for the given area, that correspond to the given context-value (=conditionKeys-List) and adds them to resultLinks
        /// 
        /// Returns the items left of all items in allTargetItems
        /// after checking allconditionlinks against conditionKeys
        /// </summary>
        /// <param name="resultLinks"></param>
        /// <param name="allConditionLinks"></param>
        /// <param name="conditionKeys"></param>
        /// <param name="perDate"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        private IEnumerable<long> getAvailability(IEnumerable<long> resultLinks, List<ProvisionConditionLink> allConditionLinks, List<long> conditionKeys, DateTime perDate, long area)
        {
            List<long> assignedConditionIds = (from a in allConditionLinks
                                               where conditionKeys.Contains(a.CONDITIONID) && a.area == area

                                               select (long)a.TARGETID).ToList();
            //if (debug)                _log.Debug("Assigned Product Paramsets: " + assignedConditionIds.Count);
            
            return resultLinks.Union(assignedConditionIds).Distinct();
        }


        /// <summary>
        /// Takes all ConditionLinks for the given area, that correspond to the given context-value (=conditionKeys-List) and adds them to resultLinks
        ///  Returns the items left of all items in allTargetItems
        ///  after checking allconditionlinks against conditionKeys 
        /// </summary>
        /// <param name="resultLinks"></param>
        /// <param name="allConditionLinks"></param>
        /// <param name="conditionKeys"></param>
        /// <param name="perDate"></param>
        /// <param name="area"></param>
        /// <param name="istValid"></param>
        /// <param name="istGepflegt"></param>
        /// <returns></returns>
        private IEnumerable<long> getAvailabilityByArea(IEnumerable<long> resultLinks, List<ProvisionConditionLink> allConditionLinks, List<long> conditionKeys, DateTime perDate, long area, ref bool istValid, ref bool istGepflegt) 
        {
             List<ProvisionConditionLink> assignedConditions= (from a in allConditionLinks
                                               where  a.area == area
                                               select a).ToList();
             if (assignedConditions.Count() > 0)
                {
                    istGepflegt = true;
                    List<long> assignedConditionIdsWithArea = (from a in assignedConditions
                                                       where conditionKeys.Contains(a.CONDITIONID)
                                                       select (long)a.TARGETID).ToList();

                    if (assignedConditionIdsWithArea.Count() > 0)
                    {
                        istValid= true;
                        return resultLinks.Union(assignedConditionIdsWithArea).Distinct();
                    }
                       
                }
              return resultLinks;

             
        }

        /// <summary>
        /// Distributes the provisions based on the provshares, steps, adjustments
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="param"></param>
        /// <param name="provisionvalue"></param>
        /// <param name="rval"></param>
        /// <param name="haendler"></param>
        /// <param name="defaultProvision"></param>
        /// <param name="provalInfo"></param>
        /// <param name="tracing"></param>
        /// <param name="sysprprovset">provisionsplan-id</param>
        /// <returns></returns>
        private bool distributeProvisions(provKontextDto ctx, iProvisionDto param, double provisionvalue, List<AngAntProvDto> rval, long haendler, double defaultProvision, ProvisionValuePair provalInfo, List<ProvKalkDto> tracing, long sysprprovset)
        {
            bool bfail = false;
            double provisionvalueOrg = provisionvalue;
            double defaultProvisionOrg = defaultProvision;

            //4. Aufteilung anwenden-----------------------------------------------------------------------------------------
            List<PROVSHAREDATA> shares = dao.getProvisionShares();
            //Aufteilungspositionen für diesen Händler
            shares = (from s in shares
                      where (s.VALIDFROM == null || s.VALIDFROM <= ctx.perDate || s.VALIDFROM <= nullDate)
                                          && (s.VALIDUNTIL == null || s.VALIDUNTIL >= ctx.perDate || s.VALIDUNTIL <= nullDate)
                                          && (s.sysprhgroup == ctx.sysprhgroup || s.sysprhgroup==0)
                                          && (s.sysprprovtype == 0 || s.sysprprovtype == param.sysprprovtype)
                                          && s.sysvprole == haendler
                      select s).ToList();
            

            long vprole = haendler;
            if (param.defRoleType>0)//Override, alles an den Verkäufer oder den im Rollenzweig mit gegebenem Rollentyp
            {
                long vkrole = dao.getRoleByType(ctx.sysperole, param.defRoleType);
                long vkperson = obDao.getPersonIDByPEROLE(vkrole);
                int shrcountnoValue = 0;
                if(shares.Count >0)
                    shrcountnoValue= (from s in shares where s.sysvkrole == vkrole && s.method == 2 select s).Count();

                if (vkperson > 0 && shrcountnoValue==0)
                {
                    AngAntProvDto dto = new AngAntProvDto();
                    dto.sysprprovtype = param.sysprprovtype;
                    dto.syspartner = vkperson;
                    dto.provision = provisionvalue;
                    dto.abrechnung = ctx.perDate;
                    
                    dto.provisionBrutto = dto.provision;
                    dto.provisionUst = 0;
                    if (param.provisionInputValue > 0)
                        dto.provisionPro = Math.Round((dto.provision / param.provisionInputValue) * 10000) / 100;

                    if (provalInfo != null)
                        dto.provisionPro = provalInfo.percentage;

                    dto.defaultprovision = defaultProvision;
                    
                    dto.defaultprovisionbrutto = dto.defaultprovision;
                    dto.defaultprovisionust = 0;

                    if (provalInfo != null)
                        dto.defaultprovisionp = provalInfo.percentage;

                    if (param.useProvisionOverwriteValue)
                        dto.flaglocked = 1;

                    dto.sysvt = ctx.sysvt;
                    dto.basis = param.provisionInputValue;
                    dto.auszahlung = dto.defaultprovision;
                    dto.defauszahlung = dto.auszahlung;
                    dto.auszahlungsart = 0;
                    dto.abrechnung = ctx.perDate;
                    dto.sysprprovtype = param.sysprprovtype;
                    dto.area = "PRPROVSET";
                    dto.syslease = sysprprovset;

                    rval.Add(dto);

                    if (tracing != null)
                    {
                        ProvKalkDto pk = new ProvKalkDto();
                        pk.area = "PRPROVSHR";
                        pk.syslease = -1;
                        pk.remark = "Alles an Rollentyp "+param.defRoleType+", keine Aufteilung";
                        pk.sysprprovtype = param.sysprprovtype;
                        pk.provrate = dto.defaultprovisionp;
                        pk.provval = defaultProvision;
                        pk.provision = defaultProvision;
                        
                        pk.kalkbasis = param.provisionInputValue;
                        pk.prov = dto;
                        tracing.Add(pk);
                    }
                }
                else
                {
                    _log.Warn("DEFROLETYPE was set (all to vendor), but for user"+ctx.sysperole+" the role "+param.defRoleType+" was not found");
                }
            }
            else if (shares.Count == 0)//Wenn keine Aufteilung geht alles an den Händler
            {
                long hdperson = obDao.getPersonIDByPEROLE(vprole);
                if (hdperson>0)
                {
                    AngAntProvDto dto = new AngAntProvDto();
                    dto.sysprprovtype = param.sysprprovtype;
                    dto.syspartner = hdperson;
                    dto.provision = provisionvalue;
                   
                    dto.provisionBrutto = dto.provision;
                    dto.provisionUst = 0;
                    if (param.provisionInputValue > 0)
                        dto.provisionPro = Math.Round((dto.provision / param.provisionInputValue) * 10000) / 100;

                    if (provalInfo != null)
                        dto.provisionPro = provalInfo.percentage;
                 
                    dto.defaultprovision = defaultProvision;
                   
                    dto.defaultprovisionbrutto = dto.defaultprovision;
                    dto.defaultprovisionust = 0;

                    if (provalInfo != null)
                        dto.defaultprovisionp = provalInfo.percentage;

                    if (param.useProvisionOverwriteValue)
                        dto.flaglocked = 1;

                    dto.sysvt = ctx.sysvt;
                    dto.basis = param.provisionInputValue;
                    dto.auszahlung = dto.defaultprovision;
                    dto.defauszahlung = dto.auszahlung;
                    dto.auszahlungsart = 0;
                    dto.abrechnung = ctx.perDate;
                    dto.sysprprovtype = param.sysprprovtype;
                    dto.abrechnung = ctx.perDate;
                    dto.area = "PRPROVSET";
                    dto.syslease = sysprprovset;

                    rval.Add(dto);
                    if (tracing != null)
                    {
                        ProvKalkDto pk = new ProvKalkDto();
                        pk.area = "PRPROVSHR";
                        pk.syslease = -1;
                        pk.remark = "Alles an Händler, keine Aufteilung";
                        pk.sysprprovtype = param.sysprprovtype;
                        pk.provrate = dto.defaultprovisionp;
                        pk.provval = defaultProvision;
                        pk.provision = defaultProvision;
                       
                        pk.kalkbasis = param.provisionInputValue;
                        pk.prov = dto;
                        tracing.Add(pk);
                    }
                }
                else
                {
                    _log.Warn("No Shares, all to vendor, but vendor was not found for VK Role " + vprole);
                }
            }
            else
            {
               // BNOWBASIS-689
               /* long vkparentrole = (from s in shares
                                     where s.sysvkrole == ctx.sysperole
                                     select s.peroleparent).FirstOrDefault();*/
                long vkparentrole = dao.getVkparent(ctx.sysperole);
                //Ticket#2011102010000145 — Provisionsaufteilung darf nur für einen Verkäufer greifen  (20.10.2011)
                //SOLL: Die Provision wird für den aktuellen Verkäufer und alle in PRPROVSHR-aufgeführte übergeordneten Rollen aufgeteilt (Rest geht an den Händler).
                shares = (from s in shares
                          where (s.sysvkrole == ctx.sysperole) || (vkparentrole != 0 && s.sysvkrole != ctx.sysperole && s.peroleparent != vkparentrole)
                          select s).ToList();

                double provisionssatzdefault = 0;
                double provisionsbasisbetragdefault = 0;
                double provisionssatzhaendlerdefault = 0;
                if (provalInfo != null)
                {
                    provisionssatzdefault = provalInfo.percentage;
                    provisionssatzhaendlerdefault = provalInfo.percentage;
                    provisionsbasisbetragdefault = provalInfo.value;
                }

                foreach (PROVSHAREDATA share in shares)
                {
                    if (provisionvalue <= 0 && provisionssatzdefault<=0)
                        bfail = true;

                    double shareVal = 0;
                    double sharePercentDefault = 0;
                    double shareValDefault = 0;
                    if (share.method == 0)//Prozentual
                    {
                        shareVal = provisionvalue * share.shrrate / 100;
                        sharePercentDefault = provisionssatzdefault * share.shrrate / 100;
                        provisionvalue -= shareVal;

                        shareValDefault = defaultProvision * share.shrrate / 100;
                        defaultProvision -= shareValDefault;
                        //shareVal for share.sysvkrole
                    }
                    else if (share.method == 1)//Betragswertig
                    {
                        shareVal = share.shrval;
                        if (shareVal > provisionvalue) shareVal = provisionvalue;

                        provisionvalue -= shareVal;

                        sharePercentDefault = shareVal / provisionsbasisbetragdefault;

                        shareValDefault = share.shrval;
                        if (shareValDefault > defaultProvision) shareValDefault = defaultProvision;

                        defaultProvision -= shareValDefault;
                        //shareVal for share.sysvkrole
                    }
                    else if (share.method == 2)//nicht verteilen
                        continue;

                    //Aufteilungen gehen an den Verkäufer
                    long sharePersonId = obDao.getPersonIDByPEROLE(share.sysvkrole);

                    if (sharePersonId>0)
                    {
                        AngAntProvDto dto = new AngAntProvDto();
                        dto.sysprprovtype = param.sysprprovtype;
                        dto.syspartner = sharePersonId;
                        dto.provision = shareVal;
                        dto.provisionBrutto = dto.provision;
                        dto.provisionUst = 0;
                        dto.provisionPro = 0;                      
                        provisionssatzhaendlerdefault -= sharePercentDefault;
                        if (param.provisionInputValue > 0)
                            dto.provisionPro = Math.Round((dto.provision / param.provisionInputValue) * 10000) / 100;

                        dto.defaultprovision = shareValDefault;
                        dto.defaultprovisionp = sharePercentDefault;                       
                        dto.defaultprovisionbrutto = dto.defaultprovision;
                        dto.defaultprovisionust = 0;

                        if (param.useProvisionOverwriteValue)
                            dto.flaglocked = 1;

                        dto.sysvt = ctx.sysvt;
                        dto.basis = param.provisionInputValue;
                        dto.auszahlung = dto.defaultprovision;
                        dto.defauszahlung = dto.auszahlung;
                        dto.auszahlungsart = 0;
                        dto.abrechnung = ctx.perDate;
                        dto.sysprprovtype = param.sysprprovtype;
                        dto.abrechnung = ctx.perDate;
                        dto.area = "PRPROVSET";
                        dto.syslease = sysprprovset;

                        rval.Add(dto);

                        vprole = share.sysvprole;

                        if (tracing != null)
                        {
                            ProvKalkDto pk = new ProvKalkDto();
                            pk.area = "PRPROVSHR";
                            pk.syslease = share.sysprprovshr;
                            pk.sysprprovtype = param.sysprprovtype;
                            pk.method = share.method;
                            pk.provrate = sharePercentDefault;
                            pk.provval = shareValDefault;
                            pk.provision = shareValDefault;
                            
                            pk.kalkbasis = param.provisionInputValue;
                            pk.prov = dto;
                            tracing.Add(pk);
                        }
                    }
                }
                long shareHaendlerId = obDao.getPersonIDByPEROLE(vprole);
                
                if (provisionssatzhaendlerdefault < 0)
                    provisionssatzhaendlerdefault = 0;
                if (provisionvalue < 0)
                    provisionvalue = 0;

                //Der Rest geht an den Händler
                if (shareHaendlerId>0 && (provisionssatzhaendlerdefault > 0 || provisionvalue > 0))
                {
                    AngAntProvDto dtoHaendler = new AngAntProvDto();
                    dtoHaendler.sysprprovtype = param.sysprprovtype;
                    dtoHaendler.syspartner = shareHaendlerId;

                    dtoHaendler.provision = provisionvalue;
                    dtoHaendler.provisionBrutto = dtoHaendler.provision;
                    dtoHaendler.provisionUst = 0;
                    dtoHaendler.provisionPro = 0;
                    if (param.provisionInputValue > 0)
                        dtoHaendler.provisionPro = Math.Round((dtoHaendler.provision / param.provisionInputValue) * 10000) / 100;

                    dtoHaendler.defaultprovision = defaultProvision;
                    dtoHaendler.defaultprovisionbrutto = dtoHaendler.defaultprovision;
                    dtoHaendler.defaultprovisionust = 0;
                    dtoHaendler.defaultprovisionp = provisionssatzhaendlerdefault;

                    if (param.useProvisionOverwriteValue)
                        dtoHaendler.flaglocked = 1;

                    dtoHaendler.sysvt = ctx.sysvt;
                    dtoHaendler.basis = param.provisionInputValue;
                    dtoHaendler.auszahlung = dtoHaendler.defaultprovision;
                    dtoHaendler.defauszahlung = dtoHaendler.auszahlung;
                    dtoHaendler.auszahlungsart = 0;
                    dtoHaendler.abrechnung = ctx.perDate;
                    dtoHaendler.sysprprovtype = param.sysprprovtype;
                    dtoHaendler.area = "PRPROVSET";
                    dtoHaendler.syslease = sysprprovset;

                    rval.Add(dtoHaendler);

                    if (tracing != null)
                    {
                        ProvKalkDto pk = new ProvKalkDto();
                        pk.area = "PRPROVTYPE";
                        pk.syslease = -1;
                        pk.remark = "Rest an Händler";
                        pk.sysprprovtype = param.sysprprovtype;
                        pk.provrate = dtoHaendler.provisionPro;
                        pk.provval = dtoHaendler.defaultprovision;
                        pk.provision = defaultProvision;
                        
                        pk.kalkbasis = param.provisionInputValue;
                        pk.prov = dtoHaendler;
                        tracing.Add(pk);
                    }
                }
            }

            return bfail;
        }

        /// <summary>
        /// calculate the provision amount
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="param"></param>
        /// <param name="steps"></param>
        /// <param name="tracing"></param>
        /// <returns></returns>
        private ProvisionValuePair getProvisionValue(provKontextDto ctx, iProvisionDto param, List<PRPROVSTEP> steps, List<ProvKalkDto> tracing)
        {
            //Anpassungen ermitteln----------------------------------------------------------------------------
            IEnumerable<long> adjustments = new List<long>();
            List<ProvisionAdjustConditionLink> allAdjLinks = dao.getProvisionAdjustLinks();
            allAdjLinks = (from a in allAdjLinks
                           where a.sysprproduct == ctx.sysprproduct &&
                           (a.VALIDFROM == null || a.VALIDFROM <= ctx.perDate || a.VALIDFROM <= nullDate)
                            && (a.VALIDUNTIL == null || a.VALIDUNTIL >= ctx.perDate || a.VALIDUNTIL <= nullDate)
                           select a).ToList();

            //enumerate conditions from context, every of supportedLinks is a must-have parameter
            foreach (ConditionLinkType ctype in supportedLinks)
            {
                long trigger = mapConditionLinkToProvisionTrigger[ctype];
                adjustments = getAvailability(adjustments, allAdjLinks, ConditionLink.getParameter(ctx, ctype, mapConditionLinkToParameter, obDao, ctx.perDate), ctx.perDate, trigger);
            }
            List<PRPROVADJSTEP> adjSteps = dao.getProvisionAdjustStep();
            adjSteps = (from s in adjSteps
                        join a in adjustments on s.SYSPRPROVADJSTEP equals a
                        where s.SYSPRPROVTYPE == param.sysprprovtype
                        select s).ToList();

            //Anpassungen ermitteln ende -------------------------------------------------------------------------


            //the inputvalue to calc a provision for
            double inputvalue = param.provisionInputValue;
            ProvisionValuePair provValues = new ProvisionValuePair();

            
            foreach (PRPROVSTEP step in steps)
            {
                _log.Debug("PROVSEP "+step.SYSPRPROVSTEP);
                double useprate = 0, usepval = 0;//the provision percentage/value to use for calculation

                if (step.SOURCEBASIS == 0 && step.SYSPROVSTRCT.HasValue) //STRUKTUR
                {
                    double tmpval = getProvStructRate(step.SYSPROVSTRCT.Value, ctx.perDate, param.sysProvTarif, inputvalue, false);
                    if (step.METHOD < 3)
                        useprate = tmpval;
                    else
                        usepval = tmpval;
                }
                else if (step.SOURCEBASIS == 1 && step.SYSVG.HasValue)//Wertegruppe
                {
                    double tmpval = vgDao.getVGValue(step.SYSVG.Value, ctx.perDate, param.vgXValue, param.vgYValue, param.interpolationMode, param.vgQueryMode);

                    if (step.METHOD < 3)
                        useprate = tmpval;
                    else
                        usepval = tmpval;
                }
                else if (step.SOURCEBASIS == 2 && (step.PROVRATE.HasValue || step.PROVVAL.HasValue)) //Direkt
                {

                    if (step.METHOD < 3)
                        useprate = step.PROVRATE.Value;
                    else
                        usepval = step.PROVVAL.Value;

                }

                //calculate the provision:
                provValues = getProvisionValue(step.METHOD.Value, inputvalue, useprate, usepval, provValues, false);
                _log.Debug("Value rate: " + useprate + " p: " + usepval + " basis: " + inputvalue + " output: " + provValues.value + " output p: " + provValues.percentage);
                if(tracing!=null)
                {
                    ProvKalkDto pk = new ProvKalkDto();
                    pk.area = "PRPROVSTEP";
                    pk.syslease = step.SYSPRPROVSTEP;
                    pk.sysprprovtype = param.sysprprovtype;
                    pk.method = step.METHOD.Value;
                    pk.provrate = useprate;
                    pk.provval = usepval;
                    pk.provision = provValues.value;
                    pk.basis = inputvalue;
                    
                    tracing.Add(pk);
                }

            }
            //2. Anpassungen--------------------------------------------------------------------------------------

            foreach (PRPROVADJSTEP adjstep in adjSteps)
            {

                double adjrate = 0, adjval = 0;//adjustment of rate

                if (adjstep.SOURCEBASIS == 0 && adjstep.SYSPROVSTRCT.HasValue) //STRUKTUR
                {

                    double tmpval = getProvStructRate(adjstep.SYSPROVSTRCT.Value, ctx.perDate, param.sysProvTarif, inputvalue, false);
                    if (adjstep.METHOD < 3)
                        adjrate = tmpval;
                    else
                        adjval = tmpval;
                }
                else if (adjstep.SOURCEBASIS == 1 && adjstep.SYSVG.HasValue)//Wertegruppe
                {

                    //TODO - what kind of x/y-values to use here?
                    double tmpval = vgDao.getVGValue(adjstep.SYSVG.Value, ctx.perDate, "1", inputvalue.ToString(), VGInterpolationMode.LINEAR, plSQLVersion.V1);
                    if (adjstep.METHOD < 3)
                        adjrate = tmpval;
                    else
                        adjval = tmpval;
                }
                else if (adjstep.SOURCEBASIS == 2 && (adjstep.ADJRATE.HasValue || adjstep.ADJVAL.HasValue)) //Direkt
                {
                    if (adjstep.METHOD < 3)
                        adjrate = adjstep.ADJRATE.Value;
                    else
                        adjval = adjstep.ADJVAL.Value;

                }

                //adjust the provision rate
                //calculate the provision:
                provValues = getProvisionValue(adjstep.METHOD.Value, inputvalue, adjrate, adjval, provValues, true);

                if (tracing != null)
                {
                    ProvKalkDto pk = new ProvKalkDto();
                    pk.area = "PRPROVADJSTEP";
                    pk.syslease = adjstep.SYSPRPROVADJSTEP;
                    pk.sysprprovtype = param.sysprprovtype;
                    pk.method = adjstep.METHOD.Value;
                    pk.provrate = adjrate;
                    pk.provval = adjval;
                    pk.provision = provValues.value;
                    pk.basis = inputvalue;
                    
                    tracing.Add(pk);
                }

            }
            return provValues;
        }

        /// <summary>
        /// Gets the percentage (or value) of the given provstrct-id
        /// </summary>
        /// <param name="sysprovstrct"></param>
        /// <param name="perDate"></param>
        /// <param name="sysProvTarif">optional, needed as tarif identifier (else default is used)</param>
        /// <param name="inputValue">the value the provision is calced for - just used for plan-values</param>
        /// <param name="forValue">true to return the value instead of percentage</param>
        /// <returns></returns>
        private double getProvStructRate(long sysprovstrct, DateTime perDate, long sysProvTarif, double inputValue, bool forValue)
        {
            //2. Strukturen für berechnung ermitteln
            List<PROVSTRCTDATA> strctdata = dao.getStrctData(sysprovstrct, perDate);
            double prate = 0, pval = 0;
            double defrate = 0;
            bool leave = false;
            foreach (PROVSTRCTDATA pdata in strctdata)
            {
                if (leave) break;
                switch (pdata.typ)
                {
                    case 0://rate
                        prate = pdata.provrate;
                        pval = pdata.provval;
                        break;
                    case 1://plan
                        if (pdata.lowerbp >= inputValue)
                        {
                            prate = pdata.provrate;
                            leave = true;
                        }
                        if (pdata.lowerb >= inputValue)
                        {
                            pval = pdata.provval;
                            leave = true;
                        }
                        break;
                    case 2://tarif
                        if (sysProvTarif > 0 && pdata.sysid == sysProvTarif)
                            prate = pdata.provrate;
                        if (pdata.standardflag > 0)
                            defrate = pdata.provrate;
                        break;
                }
            }
            if (defrate > 0 && sysProvTarif == 0)
                prate = defrate;
            if (forValue) return pval;

            return prate;
        }

        /// <summary>
        /// Returns the items left of all items in allTargetItems
        /// after checking allconditionlinks against conditionKeys
        /// </summary>
        /// <param name="resultLinks"></param>
        /// <param name="allConditionLinks"></param>
        /// <param name="conditionKeys"></param>
        /// <param name="perDate"></param>
        /// <param name="trigger"></param>
        /// <returns></returns>
        private IEnumerable<long> getAvailability(IEnumerable<long> resultLinks, List<ProvisionAdjustConditionLink> allConditionLinks, List<long> conditionKeys, DateTime perDate, long trigger)
        {
            List<long> assignedConditionIds = (from a in allConditionLinks
                                               where conditionKeys.Contains(a.CONDITIONID) && a.adjustmenttrigger == trigger

                                               select (long)a.TARGETID).ToList();
            return resultLinks.Union(assignedConditionIds).Distinct();
        }



        /// <summary>
        /// calculates the step value based on the method and input rate/value
        /// </summary>
        /// <param name="method">method of calculation/input-usage</param>
        /// <param name="basis">source provision value</param>
        /// <param name="provrate">percentage adjustment</param>
        /// <param name="provvalue">value adjustment</param>
        /// <param name="curProv">Derzeitiger Provisionswert</param>
        /// <param name="isAdj">Anpassungsschritt-Flag</param>
        /// <returns></returns>
        private ProvisionValuePair getProvisionValue(int method, double basis, double provrate, double provvalue, ProvisionValuePair curProv, bool isAdj)
        {
            /* Sample:
             * 1000 ausgangsbasis für einen provisionstyp

                schritt: prozent 10 				10    100
                schritt: prozentpunkte 5            15    150
                schritt: prozent überschreiben 8    8	   80
                schritt: betrag 55                  13.5  135
                schritt: betrag überschreiben 34    3.4    34
             * */

            //method 0=prozent, 1=prozentpunkte, 2=überschreiben, 3=Betrag, 4=Überschreiben Betrag
            switch (method)
            {
                //Prozent
                case 0:
                    if (curProv.value == 0 && !isAdj)
                    {
                        curProv.value = basis * provrate / 100;
                        curProv.percentage = provrate;
                    }
                    else
                    {
                        curProv.percentage += curProv.percentage / 100 * provrate;
                        curProv.value = basis * curProv.percentage / 100;
                    }
                    break;
                // Prozentpunkte
                case 1:
                    curProv.percentage += provrate;
                    curProv.value = basis * curProv.percentage / 100;
                    break;


                // Prozent überschreiben
                case 2:
                    curProv.percentage = provrate;
                    curProv.value = basis * curProv.percentage / 100;
                    break;
                // Betrag
                case 3:
                    curProv.value += provvalue;
                    if (basis == 0) curProv.percentage = 0;
                    else curProv.percentage = curProv.value / basis * 100;
                    if (curProv.percentage > 100) curProv.percentage = 100;
                    break;
                // Betrag überschreiben
                case 4:
                    curProv.value = provvalue;
                    if (basis == 0) curProv.percentage = 0;
                    else curProv.percentage = curProv.value / basis * 100;
                    if (curProv.percentage > 100) curProv.percentage = 100;
                    break;
            }
            return curProv;
        }


        /// <summary>
        /// checks if the abloese is of the given type
        /// </summary>
        /// <param name="sysabltyp"></param>
        /// <param name="typ"></param>
        /// <returns></returns>
        public override bool isAbloesetyp(long sysabltyp, Abloesetyp typ)
        {
            CIC.Database.OL.EF4.Model.ABLTYP t = dao.getAblTyp(sysabltyp);
            if (t == null)
                throw new Exception("ABLTYP " + sysabltyp + " not found. Probably wrong input sysabltyp for method call.");
            return t.CODE.Equals(EnumUtil.GetStringValue(typ));
        }

        /// <summary>
        /// checks if prhgroup must be 0
        /// </summary>
        /// <param name="sysperole"></param>
        /// <param name="sysprhgroud"></param>
        /// <returns></returns>
        public override bool isPrhGroup(long sysperole, long sysprhgroud)
        {
            return dao.checkPrhGroup(sysperole, sysprhgroud);
        }

        /// <summary>
        /// get the Eigenabloeseinformation
        /// </summary>
        /// <param name="sysvorvt"></param>
        /// <returns></returns>
        public override EigenAblInfo getEigenabloeseInfo(long sysvorvt)
        {
            return dao.getEigenabloeseInfo(sysvorvt);
        }

        /// <summary>
        /// Returns the current provision plan for Kickback for the given role and prhgroup
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        public override PRPROVSET getProvisionsPlan(long sysperole, DateTime perDate)
        {
            long FieldId = getPrfldId(ProvisionSourceField.VT_GESAMTUMSATZ, null);
            if (FieldId == 0) return null;
            //get all prprovtypes for the given prisma field (provisionSource)
            List<PRPROVTYPE> provtypes = getProvisionTypes(FieldId);
            if (provtypes == null || provtypes.Count==0) return null;
            PRPROVTYPE provtype = provtypes[0];

            long sysprhgroup = 0;
            //TODO
            //sysprhgroup = ObtypDao.getPrhGroups(key, sysobtyp, perDate); -> firstorDefault();

            //incentive Provisionsschritte für kontext ermitteln-----------------------------------------------------------------------------------------
            //Auf dem Händler wird der aktuell gültige Provisionsplan ermittelt (PRCLVPPROVSET+PRPROVSET+PRPROVSTEP+PRPROVTYPE)
            List<PRPROVSTEP> steps = dao.getProvstepsInc(sysprhgroup, sysperole, perDate, provtype.SYSPRPROVTYPE);
            //hier holen wir uns auch den Provisionsplan für den händler/verkäufer
            long? prprovset = (from x in steps
                               select x.SYSPRPROVSET).Distinct().FirstOrDefault();
            if (!prprovset.HasValue)
                return null;

            return dao.getPrprovset(prprovset.Value);
        }
    }

    /// <summary>
    /// Temporary Provision value holder for value and percentage
    /// </summary>
    class ProvisionValuePair
    {
        public double percentage { get; set; }
        public double value { get; set; }
    }

}

