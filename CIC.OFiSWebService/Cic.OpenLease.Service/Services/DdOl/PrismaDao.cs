using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.OpenLease.Model.DdOl;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DTO.Prisma;


namespace Cic.OpenLease.Service.Services.DdOl
{
    public class PrismaDao
    {
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private long sysperole, sysbrand;

        public PrismaDao(long sysperole, long sysbrand)
        {
            this.sysperole = sysperole;
            this.sysbrand = sysbrand;
        }
        /// <summary>
        /// Verfügbarkeit von Produkten ermitteln.
        /// </summary>
        /// <returns>Liste von <see cref="Cic.OpenLease.ServiceAccess.DdOl.PRPRODUCTDto"/></returns>
        public Cic.OpenLease.ServiceAccess.DdOl.PRPRODUCTDto[] DeliverAvailablePrProductsFiltered(long sysObTyp, long sysObArt, PrParamFilter[] filter, DateTime lieferdatum, long? sysprproduct, long[] conditiontypes, long sysmart, long syskdtyp, long sysperole)
        {
            
            System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.DdOl.PRPRODUCTDto> PrProductDtoList = null;
            System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.DdOl.PRPRODUCTDto> rval = null;
            System.Collections.Generic.List<Cic.OpenLease.Model.DdOl.PRPRODUCT> PrProductList = new List<PRPRODUCT>();



            using (Cic.OpenLease.Model.DdOl.OlExtendedEntities context = new Cic.OpenLease.Model.DdOl.OlExtendedEntities())
            {

                try
                {
                    // Get VP PeRole
                    long sysVpPeRole = PeroleHelper.FindRootPEROLEByRoleType(context, sysperole, PEROLEHelper.CnstVPRoleTypeNumber);

                    // Get PrHBroup - in BMW there can be only one - thats why FirstOrDefault
                    PRHGROUP PrHGroup = PeroleHelper.DeliverVPPrHGroupList(context, sysbrand, sysVpPeRole).FirstOrDefault<PRHGROUP>();



                    if (PrHGroup != null && sysVpPeRole != 0)
                    {

                        //Parameters for query
                       /* object[] Parameters = 
                        { 
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "SysBRAND", Value =  sysbrand}, 
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "SysPrHGroup", Value = PrHGroup.SYSPRHGROUP},
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "SysPEROLE", Value = sysperole}, 
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "VpPeRole", Value = sysVpPeRole}, 
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysObTyp", Value = sysObTyp},
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysObArt", Value = sysObArt}
                        };*/
                        Cic.OpenLease.Service.PrismaDao pdao = new Cic.OpenLease.Service.PrismaDao();
                        ObTypDao obDao = new ObTypDao();

                        PrismaProductBo ppb = new PrismaProductBo(pdao, obDao);
                        PrismaParameterBo parbo = new PrismaParameterBo(pdao, obDao);

                        prKontextDto kontext = new prKontextDto();
                        kontext.perDate = lieferdatum;
                        //kontext.sysbrand = sysbrand;
                        //kontext.sysprhgroup = PrHGroup.SYSPRHGROUP;
                        //kontext.sysobart = sysObArt;
                        //kontext.sysobtyp = sysObTyp;
                        //kontext.sysvpperole = sysVpPeRole;
                        //kontext.sysprchannel = 2; //AIDA
                        //kontext.sysprkgroup = 0;

                        
                        kontext.sysobtyp = sysObTyp;
                        kontext.sysobart = sysObArt;
                        kontext.sysprchannel = 1;//immer 1
                        kontext.sysperole = sysperole;
                        kontext.syskdtyp = syskdtyp;
                        
                        
                        // kontext.sysprmart = sysmart; 
                        
                        _Log.Debug("Retrieving Products for sysBrand: " + sysbrand + " sysprhgroup: " + PrHGroup.SYSPRHGROUP + " sysperole: " + sysperole + " vpperole: " + sysVpPeRole + " sysobtyp: " + sysObTyp + " sysobart: " + sysObArt+" sysmart: "+sysmart);
                        // Execute Query
                        //string channel = "AIDA";
                        //string Query = "SELECT PRPRODUCT.NAME,PRPRODUCT.SYSINTTYPE, PRPRODUCT.DESCRIPTION,PRPRODUCT.SYSPRPRODUCT,RANGSL, PRPRODUCT.SYSVART FROM BCHANNEL,PRCLPRBCHNL,PRPRODUCT,PRPRODTYPE,TABLE(CIC.CIC_PRISMA_UTILS.DeliverAvailPrProd(:SysBRAND, :SysPrHGroup,  :SysPEROLE , :VpPeRole, :sysObTyp, :sysObArt)) WHERE PRPRODUCT.SYSPRPRODUCT = SYSID AND  PRPRODTYPE.sysprprodtype = PRPRODUCT.sysprprodtype and BCHANNEL.NAME='" + channel + "' and PRCLPRBCHNL.SYSBCHANNEL=BCHANNEL.SYSBCHANNEL and PRCLPRBCHNL.SYSPRPRODUCT=PRPRODUCT.SYSPRPRODUCT ORDER BY PRPRODUCT.SYSKALKTYP,PRPRODTYPE.CONDITIONTYPE,PRPRODUCT.NAME";
                        double starttime = DateTime.Now.TimeOfDay.TotalMilliseconds;
                        //PrProductDtoList = context.ExecuteStoreQuery<PRPRODUCTDto>(Query, Parameters).ToList();
                        PrProductDtoList = ppb.listAvailableProducts(kontext);
                        PrProductDtoList = ppb.filterAvailableProducts(PrProductDtoList, conditiontypes);
                        _Log.Debug("Avail Products Query Time: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - starttime));
                        rval = PrProductDtoList;
                        PRParamDao dao = new PRParamDao(context);
                        starttime = DateTime.Now.TimeOfDay.TotalMilliseconds;
                        List<long> obpath = obDao.getObTypAscendants(sysObTyp);//context.ExecuteStoreQuery<long>("select sysobtyp from obtyp start with sysobtyp=" + sysObTyp + " connect by prior sysobtypp=sysobtyp", null).ToList();
                        _Log.Debug("OBTyp Query Time: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - starttime));
                        if (filter != null)
                        {
                            rval = new List<PRPRODUCTDto>();
                            foreach (PRPRODUCTDto product in PrProductDtoList)
                            {
                                if (!checkValidFrom(product.VALIDFROM, product.VALIDUNTIL, lieferdatum)) continue;
                                starttime = DateTime.Now.TimeOfDay.TotalMilliseconds;
                                if (sysprproduct.HasValue && sysprproduct.Value != product.SYSPRPRODUCT) continue;

                                kontext.sysprproduct = product.SYSPRPRODUCT;
                                List<PRPARAMDto> prparams = //dao.DeliverPrParams(product.SYSPRPRODUCT, sysObTyp, sysperole, sysbrand, sysObArt, sysVpPeRole, PrHGroup.SYSPRHGROUP, 0, obpath,false," prfld.OBJECTMETA IN (''VERFUEGBAR_KAUFPREIS'',''VERFUEGBAR_KILOMETER'',''VERFUEGBAR_ALTERMONAT'') ");
                                    parbo.listAvailableParameter(kontext);
                                _Log.Debug("PRPARAM Query Time: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - starttime) + " prproduct: " + product.SYSPRPRODUCT+" sysobtyp: "+sysObTyp+" sysperole: "+sysperole+" sysbrand: "+sysbrand+" sysobart: "+ sysObArt+" vprole: "+ sysVpPeRole+" sysprhgroup: "+PrHGroup.SYSPRHGROUP);

                                bool use = true;
                               /* foreach (PrParamFilter cfilter in filter)//all filtercriterias have to be met
                                {
                                    var cparam = from pa in prparams
                                                 where pa.PRFLDOBJECTMETA == cfilter.PRFLDMETA
                                                 select pa;
                                    PRPARAMDto checkParam = cparam.FirstOrDefault();
                                    if (checkParam == null) continue;
                                    if (!checkFilter(checkParam, cfilter))
                                    {
                                        use = false;
                                        break;
                                    }
                                }*/
                                if (use)
                                {
                                    rval.Add(product);
                                }
                            }
                        }

                    }
                }
                catch
                {
                    throw;
                }
            }

            return rval.ToArray();
        }

        private bool checkFilter(PRPARAMDto param, PrParamFilter filter)
        {

            switch (filter.TYP)
            {
                case (0)://number
                    if (filter.VALUEN == null) return true; //no data, assume correct
                    if (param.MINVALN <= filter.VALUEN && param.MAXVALN >= filter.VALUEN)
                        return true;
                    break;
                case (1)://percent
                    if (filter.VALUEP == null) return true; //no data, assume correct
                    if (param.MINVALP <= filter.VALUEP && param.MAXVALP >= filter.VALUEP)
                        return true;
                    break;
                case (2)://date
                    if (filter.VALUED == null) return true; //no data, assume correct
                    if ((((DateTime)param.MINVALD).ToFileTimeUtc()) <= (((DateTime)filter.VALUED).ToFileTimeUtc()) && (((DateTime)param.MAXVALD).ToFileTimeUtc()) >= (((DateTime)filter.VALUED).ToFileTimeUtc()))
                        return true;
                    break;
            }
            return false;
        }

        /// <summary>
        /// Verfügbarkeit von Aktion ermitteln.
        /// MK
        /// </summary>
        /// <param name="sysObTyp">The sys ob typ.</param>
        /// <param name="sysObArt">The sys ob art.</param>
        /// <param name="sysKalkTyp">The KALKTYP:SYSKALKTYP.</param>
        /// <param name="lieferdatum">The delivery date.</param>
        /// <param name="sysprproduct">The sysprproduct to filter</param>
        /// <param name="filter">Paramfilters</param>
        /// <returns>
        /// Liste von <see cref="Cic.OpenLease.ServiceAccess.DdOl.PRPRODUCTDto"/>
        /// </returns>
        public Cic.OpenLease.ServiceAccess.DdOl.PRPRODUCTDto[] OFFDeliverAvailablePrProductActionsFiltered(long sysObTyp, long sysObArt, long sysKalkTyp, PrParamFilter[] filter, DateTime lieferdatum, long? sysprproduct)
        {

            System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.DdOl.PRPRODUCTDto> PrProductDtoList = null;
            System.Collections.Generic.List<Cic.OpenLease.ServiceAccess.DdOl.PRPRODUCTDto> rval = null;
            System.Collections.Generic.List<Cic.OpenLease.Model.DdOl.PRPRODUCT> PrProductList = new List<PRPRODUCT>();

            List<long> RankslList = new List<long>();



            using (Cic.OpenLease.Model.DdOl.OlExtendedEntities context = new Cic.OpenLease.Model.DdOl.OlExtendedEntities())
            {

                try
                {
                    // Get VP PeRole
                    long sysVpPeRole = PeroleHelper.FindRootPEROLEByRoleType(context, sysperole, PEROLEHelper.CnstVPRoleTypeNumber);

                    // Get PrHBroup - in BMW there can be only one - thats why FirstOrDefault
                    PRHGROUP PrHGroup = PeroleHelper.DeliverVPPrHGroupList(context, sysbrand, sysVpPeRole).FirstOrDefault<PRHGROUP>();

                    //Parameters for query
                    object[] Parameters = 
                    { 
                         new Devart.Data.Oracle.OracleParameter{ ParameterName = "SysBRAND", Value =  sysbrand}, 
                         new Devart.Data.Oracle.OracleParameter{ ParameterName = "SysPrHGroup", Value = PrHGroup.SYSPRHGROUP},
                         new Devart.Data.Oracle.OracleParameter{ ParameterName = "SysPEROLE", Value = sysperole}, 
                         new Devart.Data.Oracle.OracleParameter{ ParameterName = "VpPeRole", Value = sysVpPeRole}, 
                         new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysObTyp", Value = sysObTyp},
                         new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysObArt", Value = sysObArt},
                         new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysKalkTyp", Value = sysKalkTyp}
                    };

                    _Log.Debug("Retrieving Product Actions for sysBrand: " + sysbrand + " sysprhgroup: " + PrHGroup.SYSPRHGROUP + " sysperole: " + sysperole + " vpperole: " + sysVpPeRole + " sysobtyp: " + sysObTyp + " sysobart: " + sysObArt + " syskalktyp: " + sysKalkTyp);

                    // Execute Query
                    string channel = "AIDA";
                    string Query = "SELECT PRPRODUCT.NAME,PRPRODUCT.SYSINTTYPE, PRPRODUCT.DESCRIPTION,PRPRODUCT.SYSPRPRODUCT,RANGSL,PRPRODUCT.SYSVART,PRPRODUCT.VALIDFROM,PRPRODUCT.VALIDUNTIL FROM BCHANNEL,PRCLPRBCHNL,PRPRODUCT,PRPRODTYPE,TABLE(CIC.CIC_PRISMA_UTILS.DeliverAvailPrProdAct(:SysBRAND, :SysPrHGroup, :SysPEROLE , :VpPeRole, :sysObTyp, :sysObArt, :sysKalkTyp)) WHERE PRPRODUCT.SYSPRPRODUCT = SYSID AND  PRPRODTYPE.sysprprodtype = PRPRODUCT.sysprprodtype  and BCHANNEL.NAME='" + channel + "' and PRCLPRBCHNL.SYSBCHANNEL=BCHANNEL.SYSBCHANNEL and PRCLPRBCHNL.SYSPRPRODUCT=PRPRODUCT.SYSPRPRODUCT ORDER BY PRPRODUCT.SYSKALKTYP,PRPRODTYPE.CONDITIONTYPE,PRPRODUCT.NAME";
                    PrProductDtoList = context.ExecuteStoreQuery<PRPRODUCTDto>(Query, Parameters).ToList();
                    rval = PrProductDtoList;

                    List<long> obpath = context.ExecuteStoreQuery<long>("select sysobtyp from obtyp start with sysobtyp=" + sysObTyp + " connect by prior sysobtypp=sysobtyp", null).ToList();

                    PRParamDao dao = new PRParamDao(context);

                    if (filter != null)
                    {
                        rval = new List<PRPRODUCTDto>();
                        foreach (PRPRODUCTDto product in PrProductDtoList)
                        {

                            if (!checkValidFrom(product.VALIDFROM, product.VALIDUNTIL, lieferdatum)) continue;
                            if (sysprproduct.HasValue && sysprproduct.Value != product.SYSPRPRODUCT) continue;

                            PRPARAMDto[] prparams = dao.DeliverPrParams(product.SYSPRPRODUCT, sysObTyp, sysperole, sysbrand, sysObArt, sysVpPeRole, PrHGroup.SYSPRHGROUP, 0, obpath,false," prfld.OBJECTMETA IN (''VERFUEGBAR_KAUFPREIS'',''VERFUEGBAR_KILOMETER'',''VERFUEGBAR_ALTERMONAT'')");

                            bool use = true;
                           /* foreach (PrParamFilter cfilter in filter)//all filtercriterias have to be met
                            {
                                var cparam = from pa in prparams
                                             where pa.PRFLDOBJECTMETA == cfilter.PRFLDMETA
                                             select pa;
                                PRPARAMDto checkParam = cparam.FirstOrDefault();
                                if (checkParam == null) continue;
                                if (!checkFilter(checkParam, cfilter))
                                {
                                    use = false;
                                    break;
                                }
                            }*/
                            if (use)
                            {
                                rval.Add(product);
                            }
                        }
                    }


                }
                catch
                {
                    throw;
                }
            }


            return rval.ToArray();
        }

        private bool checkValidFrom(DateTime validFrom, DateTime validUntil, DateTime check)
        {
            // Optimistic
            bool IsValid = true;
            if (check == null) return IsValid;
            if (check.Year < 2000) return IsValid;
            
            // Check Valid from
            if (validFrom != null && validFrom.Year > 1900)
            {
                IsValid = IsValid && validFrom.Date <= check.Date;
            }

            // Check Valid until
            if (validUntil != null && validUntil.Year > 1900)
            {
                IsValid = IsValid && validUntil.Date >= check.Date;
            }

            return IsValid;
        }
    }
}