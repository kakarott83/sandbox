using Cic.OpenLease.Service.Provision;
using Cic.OpenLease.Service.Versicherung;
using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Logging;
using CIC.Database.OL.EF6.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Cic.OpenLease.Service
{
    /// <summary>
    /// Class to calculate and distribute subventions
    /// </summary>
    [System.CLSCompliant(true)]
    public class Subvention
    {
        #region Constants
        private const int _CnstCALCMETHOD_PERCENT = 1;
        private const int _CnstCALCMETHOD_PERCENTAGE = 0;
        private const int _CnstCALCMETHOD_VALUE = 2;

        private const int _CnstSHAREMETHOD_PERCENT = 0;
        private const int _CnstSHAREMETHOD_VALUE = 1;

        public const int CnstAREA_CHARGE = 3;
        public const int CnstAREA_INSURANCE = 5;
        public const int CnstAREA_INTEREST_OR_RESIDUALVALUE = 1;
        public const int CnstAREA_SERVICE = 4;

        //currently the subvention is assigned to a prparam
        //the subvention can now be triggered by the prparam (with all its assignments to obart, obtyp, prproduct, brand,...)
        //or be triggered by the prparams PRFLD (the common unique field, independent on params like obart, obtyp, product)
        //to use PRPARAM -resolution instead of the common PRFLD, set this parameter to true
        private static bool USE_PRPARAM_NOT_PRFLD = false;

        //private static string CnstAllgemeinPrFldArt = "Allgemein";
        //private static string CnstSubventionsmappingPrFldArt = "Subventionsmapping";
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Private variables
        private DdOlExtended _context;
        #endregion

        #region Properties
        #endregion


        #region Constructors
        public Subvention(DdOlExtended context)
        {
            _context = context;
        }
        #endregion

        #region Methods

        /// <summary>
        /// returns the sum of all subventions
        /// </summary>
        /// <param name="subventionen"></param>
        /// <returns></returns>
        public decimal getSubventionTotal(List<ANGSUBV> subventionen)
        {
            decimal sum = 0;

            foreach (ANGSUBV sub in subventionen)
            {
                sum += (decimal)sub.BETRAGBRUTTO;
            }
            return sum;
        }

        /// <summary>
        /// attaches subvention positions to angebot and returns the value of the subvention
        /// </summary>
        /// <param name="subventionen"></param>
        /// <param name="angebot"></param>
        /// <param name="code">id of the subvention</param>
        /// <returns></returns>
        public decimal saveSubventionen(List<ANGSUBV> subventionen, ANGEBOT angebot, SubventionTypeConstants code)
        {
            decimal sum = 0;
            foreach (ANGSUBV sub in subventionen)
            {
                sub.ANGEBOT = angebot;
                sub.CODE = ((int)code).ToString();
                sum += (decimal)sub.BETRAGBRUTTO;
                if (angebot != null)
                    _context.ANGSUBV.Add(sub);
            }
            return sum;
        }
        
        private static CacheDictionary<String, List<PRSUBV>> subvCache = CacheFactory<String, List<PRSUBV>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);

        /// <summary>
        /// generates the ANGSUBV Entries to save for distribution of explicit subventions
        /// </summary>
        /// <param name="defaultValue">the calculated value</param>
        /// <param name="currentValue">the current value to check for subvention</param>
        /// <param name="sysprproduct">the finance product</param>
        /// <param name="area">the explicit subvention area type</param>
        /// <param name="areaid">the explicit subvention area id</param>
        /// <param name="beginn">start of contract</param>
        /// <param name="term">term of contract</param>
        /// <param name="subinfo">Info about distributed subventions</param>
        /// <param name="name">Name of Subvention</param>
        /// <param name="syspersonhd">Vendor ID</param>
        /// <param name="Ust">Ust</param>
        /// <param name="DefaultValue">DefaultValue</param>
        /// <returns>a list of ANGSUBV Entities to save</returns>
        public List<ANGSUBV> generateExplicitSubvention(decimal defaultValue, decimal currentValue, long sysprproduct, int area, long areaid, DateTime? beginn, int term, List<SubventionDto> subinfo, string name, long? syspersonhd, decimal Ust, decimal DefaultValue)
        {
            //subvention granted:
            decimal subventionValue = defaultValue - currentValue;

            SubventionDto info = new SubventionDto();
            info.NAME = name;
            info.CURRENTVALUE = subventionValue;
            info.VALUE = 0;
            info.SUBVENTIONSGEBER = new SubventionPosDto[0];
            info.DEFAULTVALUE = defaultValue;
            subinfo.Add(info);

            if (subventionValue == 0)
                return new List<ANGSUBV>();

            String key = area + "_" + areaid + "_" + sysprproduct;

            if (!subvCache.ContainsKey(key))
            {
                System.Data.Common.DbParameter[] Parameters = 
                        { 
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "Area", Value = area}, 
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "AreaId", Value = areaid},
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysprproduct", Value = sysprproduct}
                        };


                string query = MyDeliverQuery(area);
                subvCache[key] = _context.ExecuteStoreQuery<PRSUBV>(query, Parameters).ToList();


            }
            List<PRSUBV> subventionList = subvCache[key];
            if (subventionList.Count() == 0)
            {
                _Log.Debug("No Excplicit Subvention found for Area " + area + " AreaId " + areaid + " SysPrProduct " + sysprproduct);
                return new List<ANGSUBV>();
            }
            return MyGenerateSubvention(subventionValue, subventionList, beginn, term, info, name, syspersonhd, false, Ust, DefaultValue);
        }

        public static SubventionDto mergeSubventionDto(List<SubventionDto> tmpsubinfos)
        {
            if (tmpsubinfos.Count == 0) return null;
            List<SubventionPosDto> geber = new List<SubventionPosDto>();

            if (tmpsubinfos.Count == 1)
            {
                if (tmpsubinfos[0].SUBVENTIONSGEBER == null)
                    tmpsubinfos[0].SUBVENTIONSGEBER = geber.ToArray();
                return tmpsubinfos[0];
            }
            SubventionDto a = tmpsubinfos[0];
            SubventionDto b = tmpsubinfos[1];

            decimal tmpValue = a.CURRENTVALUE.GetValueOrDefault();
            a.CURRENTVALUE = tmpValue + b.CURRENTVALUE.GetValueOrDefault();

            tmpValue = a.VALUE.GetValueOrDefault();
            a.VALUE = tmpValue + b.VALUE.GetValueOrDefault();


            if (a.SUBVENTIONSGEBER != null) geber.AddRange(a.SUBVENTIONSGEBER);
            if (b.SUBVENTIONSGEBER != null) geber.AddRange(b.SUBVENTIONSGEBER);
            a.SUBVENTIONSGEBER = geber.ToArray();
            return a;
        }
        private static CacheDictionary<String, List<PRSUBV>> implSubvCache = CacheFactory<String, List<PRSUBV>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        /// <summary>
        /// generates the ANGSUBV Entries to save for distribution of implicit subventions
        /// </summary>
        /// <param name="defaultValue">the calculated value</param>
        /// <param name="currentValue">the current value to check for subvention</param>
        /// <param name="sysprproduct">the finance product</param>
        /// <param name="sysPrFld">the implicit subvention field id</param>
        /// <param name="beginn">start of contract</param>
        /// <param name="term">term of contract</param>
        /// <param name="subinfo">Info about distributed subventions</param>
        /// <param name="name">Name of Subvention</param>
        /// <param name="syspersonhd">Vendor ID</param>
        /// <param name="Ust">Ust</param>
        /// <param name="DefaultValue">DefaultValue</param>
        /// <returns>a list of ANGSUBV Entities to save</returns>
        public List<ANGSUBV> generateImplicitSubvention(decimal defaultValue, decimal currentValue, long sysprproduct, long sysPrFld, DateTime? beginn, int term, List<SubventionDto> subinfo, string name, long? syspersonhd, decimal Ust, decimal DefaultValue)
        {
            List<long> prflds = new List<long>();
            prflds.Add(sysPrFld);
            return generateImplicitSubvention(defaultValue, currentValue, sysprproduct, prflds, beginn, term, subinfo, name, syspersonhd, Ust, defaultValue);
        }

        private List<ANGSUBV> generateImplicitSubvention(decimal defaultValue, decimal currentValue, long sysprproduct, List<long> sysPrFlds, DateTime? beginn, int term, List<SubventionDto> subinfo, string name, long? syspersonhd, decimal Ust, decimal DefaultValue)
        {
            //subvention granted:
            decimal subventionValue = defaultValue - currentValue;

            SubventionDto info = new SubventionDto();
            info.NAME = name;
            info.SUBVENTIONSGEBER = new SubventionPosDto[0];
            info.CURRENTVALUE = subventionValue;
            info.DEFAULTVALUE = defaultValue;
            subinfo.Add(info);


            if (subventionValue == 0)
                return new List<ANGSUBV>();
            String prflds = String.Join(",", sysPrFlds.ToArray());
            String key = prflds + "_" + sysprproduct;
            if (!implSubvCache.ContainsKey(key))
            {
                System.Data.Common.DbParameter[] Parameters = 
                        { 
                            //new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysPrFld", Value = sysPrFld},
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysprproduct", Value = sysprproduct}
                        };

                //this query uses the general prfld, independent of a certain prisma parameter
                //query for using a prfield - also replace at position XPRFIELDX in this file
                string query = "select PRSUBV.* from VN_PRSUBV, PRSUBV,prparam  where VN_PRSUBV.SYSPRPRODUCT=:sysprproduct and  PRSUBV.SYSPRSUBV = VN_PRSUBV.SYSPRSUBV and PRSUBV.trgtype=2 and  "+
                                " PRSUBV.SYSPRFLDTRG=prparam.sysprparam and prparam.sysprfld in (" + prflds + ")   order by PRSUBV.RANK";

                //this query uses a special prparam
                if (USE_PRPARAM_NOT_PRFLD)
                    query = "select PRSUBV.* from VN_PRSUBV, PRSUBV  where VN_PRSUBV.SYSPRPRODUCT=:sysprproduct and  PRSUBV.SYSPRSUBV = VN_PRSUBV.SYSPRSUBV and PRSUBV.trgtype=2 and  PRSUBV.SYSPRFLDTRG in (" + prflds + ")  order by PRSUBV.RANK";

                implSubvCache[key] = _context.ExecuteStoreQuery<PRSUBV>(query, Parameters).ToList();
            }
            List<PRSUBV> subventionList = implSubvCache[key];
            if (subventionList.Count() == 0)
            {
                _Log.Debug("No Implicit Subvention found for sysprprod " + sysprproduct + " sysprfld " + prflds);
                return new List<ANGSUBV>();
            }

            return MyGenerateSubvention(subventionValue, subventionList, beginn, term, info, name, syspersonhd, true, Ust, DefaultValue);
        }



        /// <summary>
        /// calculates the sum of the explicit subvention
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <param name="sysprproduct"></param>
        /// <param name="area">Trigger Type</param>
        /// <param name="areaid"></param>
        /// <param name="term"></param>
        /// <param name="Ust">Ust</param>
        /// <returns>the defaultValue reduced by the subvention or defaultValue when no subvention is configured</returns>
        public decimal deliverSubvention(decimal defaultValue, long sysprproduct, int area, long areaid, long term, decimal Ust)
        {


            List<SubventionDto> subinfos = new List<SubventionDto>();
            List<ANGSUBV> subventionen = generateExplicitSubvention(defaultValue, (decimal)0, sysprproduct, area, areaid, null, (int)term, subinfos, "Dummy", 0, Ust, defaultValue);
            decimal subvention = getSubventionTotal(subventionen);

            return defaultValue - subvention;

            //Disabled: The max subvention is defined by subvpos of subv
            /*
            decimal subventionValue = 0;

            System.Data.Common.DbParameter[] Parameters = 
                        { 
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "Area", Value = area}, 
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "AreaId", Value = areaid},
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysprproduct", Value = sysprproduct}
                        };

            List<PRSUBV> subventionList;
            

            string query = MyDeliverQuery(area);
            
            subventionList = _context.ExecuteStoreQuery<PRSUBV>(query, Parameters).ToList();

            foreach (PRSUBV subvention in subventionList)
            {
                subventionValue += MyCalcSubvention(defaultValue,subvention, term);
            }

            return defaultValue-subventionValue;*/
        }

        public static List<SubventionDto> DeliverSubventions(ANGEBOTDto angebotDto, DdOlExtended context, long? syspersonhd, long sysperson, long sysperole)
        {
            return SetSubventions(angebotDto, null, context, syspersonhd, sysperson, sysperole);
        }

        public static List<SubventionDto> SetSubventions(ANGEBOTDto angebotDto, ANGEBOT angebot, DdOlExtended context, long? syspersonhd, long sysperson, long sysperole)
        {
            if (!syspersonhd.HasValue)
                syspersonhd = sysperson;

            List<SubventionDto> subinfos = new List<SubventionDto>();
            try
            {
                PrismaDao pd = new PrismaDao();
                long sysvart = pd.getVertragsart((long)angebotDto.SYSPRPRODUCT).SYSVART;//long sysvart = Cic.OpenLease.Model.DdOl.PRPRODUCTHelper.DeliverSYSVART(context, (long)angebotDto.SYSPRPRODUCT);
                decimal Ust = LsAddHelper.GetTaxRate(context, sysvart);

                //delete old subvention entries, if available
                if (angebot != null)
                {
                    try
                    {
                        var t = from a in context.ANGSUBV
                                where a.ANGEBOT.SYSID == angebot.SYSID
                                select a;

                        foreach (ANGSUBV a in t)
                            context.DeleteObject(a);
                    }
                    catch (Exception e)
                    {
                        _Log.Error("Löschen der alten Subventionen nicht erfolgt: " + e.Message, e);
                    }
                }

                Subvention Subvention = new Subvention(context);
                decimal DefaultValue, CurrentValue;
                long SysPrProduct = angebotDto.SYSPRPRODUCT.GetValueOrDefault();
                long sysObTyp = angebotDto.SYSOBTYP.GetValueOrDefault();
                long sysObArt = angebotDto.SYSOBART.GetValueOrDefault();
                long sysBrand = angebotDto.SYSBRAND.GetValueOrDefault();

                long SysPrFld = 0;
                if (angebotDto.ANGOBLIEFERUNG == null)
                {
                    angebotDto.ANGOBLIEFERUNG = DateTime.Now;
                    _Log.Warn("No Lieferdatum set, using today for Subvention Calculation");
                }
                _Log.Debug("Subvention for Date " + angebotDto.ANGOBLIEFERUNG + " / Term" + angebotDto.ANGKALKLZ);
                DateTime beginn = (DateTime)angebotDto.ANGOBLIEFERUNG;
                int laufzeit = (int)angebotDto.ANGKALKLZ;
                List<ANGSUBV> subventionen = null;
                GebuehrDao geb = new GebuehrDao(context);
                //EXPLIZITE (AUTO) SUBVENTIONEN---------------------------------------------------------------------------
                // Calculate gebuehr subvention
                if(geb.getGebuehr(PRParamDao.CnstBearbeitungsgebuehrCode)!=null)
                try
                {


                    List<SubventionDto> tmpsubinfos = new List<SubventionDto>();
                    DefaultValue = (decimal)angebotDto.ANGKALKGEBUEHR_DEFAULT.GetValueOrDefault();
                    //CurrentValue = (decimal)angebotDto.ANGKALKGEBUEHR_SUBVENTION.GetValueOrDefault();
                    SysPrProduct = angebotDto.SYSPRPRODUCT.GetValueOrDefault();

                    subventionen = Subvention.generateExplicitSubvention(DefaultValue, 0, SysPrProduct, Subvention.CnstAREA_CHARGE, geb.getGebuehr(PRParamDao.CnstBearbeitungsgebuehrCode).sysgebuehr, beginn, laufzeit, tmpsubinfos, "Bearbeitungsgebühr", syspersonhd, Ust, DefaultValue);
                    //angebotDto.ANGKALKGEBUEHR_SUBVENTION = 
                    tmpsubinfos.Last().CURRENTVALUE = Subvention.saveSubventionen(subventionen, angebot, SubventionTypeConstants.Gebuehr);

                    decimal nachlass = (decimal)angebotDto.ANGKALKGEBUEHR_DEFAULT.GetValueOrDefault() - (decimal)angebotDto.ANGKALKGEBUEHRBRUTTO.GetValueOrDefault() - (decimal)angebotDto.ANGKALKGEBUEHR_SUBVENTION.GetValueOrDefault();
                    if (nachlass > 0)
                    {

                        //implicit
                        SysPrFld = MyGetImplSysPrParam(SubventionTypeConstants.Gebuehr, context, SysPrProduct, sysObTyp, sysperole, sysBrand, sysObArt);
                        subventionen = Subvention.generateImplicitSubvention(nachlass, 0, SysPrProduct, SysPrFld, beginn, laufzeit, tmpsubinfos, "Bearbeitungsgebühr", syspersonhd, Ust, DefaultValue);
                        //angebotDto.ANGKALKGEBUEHR_SUBVENTION += 
                        Subvention.saveSubventionen(subventionen, angebot, SubventionTypeConstants.Gebuehr);
                    }
                    SubventionDto infoDto = Subvention.mergeSubventionDto(tmpsubinfos);
                    if (infoDto != null)
                    {
                        infoDto.DEFAULTVALUE = DefaultValue;
                        subinfos.Add(infoDto);
                    }
                }
                catch (Exception se)
                {
                    _Log.Error("Subvention for Bearbeitungsgebühr failed", se);
                }


                try
                {
                    // Calculate rggebuehr subvention
                    DefaultValue = angebotDto.ANGKALKRGGEBUEHR_DEFAULT.GetValueOrDefault();
                    CurrentValue = (decimal)angebotDto.ANGKALKRGGEBUEHR.GetValueOrDefault();
                    long sysgebuehr = 0;
                    GebuehrInfo gi = geb.getGebuehr(RgGebHelper.CnstRGG);
                    if (gi != null)
                    {
                        sysgebuehr = gi.sysgebuehr;
                        subventionen = Subvention.generateExplicitSubvention(DefaultValue, 0, SysPrProduct, Subvention.CnstAREA_CHARGE, sysgebuehr, beginn, laufzeit, subinfos, "Rechtsgeschäftsgebühr", syspersonhd, Ust, DefaultValue);
                        angebotDto.ANGKALKRGGEBUEHR_SUBVENTION = Subvention.saveSubventionen(subventionen, angebot, SubventionTypeConstants.RgGebuehr);
                        subinfos.Last().CURRENTVALUE = angebotDto.ANGKALKRGGEBUEHR_SUBVENTION;
                        subinfos[subinfos.Count - 1].DEFAULTVALUE = DefaultValue;
                    }
                    else
                        _Log.Warn("Subvention für RGGebühr nicht möglich, Gebühr mit Code " + RgGebHelper.CnstRGG + " nicht definiert!");
                }
                catch (Exception se)
                {
                    _Log.Error("Subvention for RGG failed", se);
                }

                try
                {
                    /* Boolean iscredit = false;
                     Cic.OpenLease.Service.Services.DdOl.CalculationDao calcDao = new Cic.OpenLease.Service.Services.DdOl.CalculationDao(context);
                     //Get Ust
                     VART va = calcDao.getVART(angebotDto.SYSPRPRODUCT.Value);
                     if (va.BEZEICHNUNG.Equals("KREDIT"))
                         iscredit = true;
                     */

                    // Calculate rate subvention
                    // a special case, although explicit it is identified only by prfld and prproduct, because intsteps are not known in the framework
                    CurrentValue = angebotDto.ANGKALKRATE_SUBVENTION.GetValueOrDefault();//calculated by bmwcalculate-service
                    DefaultValue = angebotDto.ANGKALKRATE_DEFAULT.GetValueOrDefault();
                    if (DefaultValue == 0)
                        DefaultValue = angebotDto.ANGKALKRATEBRUTTO.GetValueOrDefault(); //for support of old offers without default-value
                    //SysPrFld = MyGetSysPrFld( SubventionTypeConstants.Rate, context);

                    SysPrFld = 0;
                    List<SubventionDto> tmpsubinfos = null;
                    List<long> prflds = new List<long>();
                    try//first try finding a field for nominalzins
                    {
                        tmpsubinfos = new List<SubventionDto>();
                        SysPrFld = MyGetImplSysPrParam(SubventionTypeConstants.Rate, context, SysPrProduct, sysObTyp, sysperole, sysBrand, sysObArt);
                        prflds.Add(SysPrFld);
                        subventionen = Subvention.generateExplicitSubvention(CurrentValue, 0, SysPrProduct, Subvention.CnstAREA_INTEREST_OR_RESIDUALVALUE, SysPrFld, beginn, laufzeit, tmpsubinfos, "Zins- und Ratenstützungen", syspersonhd, Ust, DefaultValue);
                    }
                    catch (Exception ez)
                    {
                        _Log.Warn("No Expl.Subvention for Typ LEAKALK_ZINSNOMINAL_NACHLASS", ez);
                    }

                    //if not found there must be one for effzins or noone at all
                    if (subventionen == null || subventionen.Count == 0)
                    {
                        try
                        {
                            tmpsubinfos = new List<SubventionDto>();
                            SysPrFld = MyGetImplSysPrParam(SubventionTypeConstants.RateKredit, context, SysPrProduct, sysObTyp, sysperole, sysBrand, sysObArt);
                            prflds.Add(SysPrFld);
                            subventionen = Subvention.generateExplicitSubvention(CurrentValue, 0, SysPrProduct, Subvention.CnstAREA_INTEREST_OR_RESIDUALVALUE, SysPrFld, beginn, laufzeit, tmpsubinfos, "Zins- und Ratenstützungen", syspersonhd, Ust, DefaultValue);
                        }
                        catch (Exception ez)
                        {
                            _Log.Warn("No Expl.Subvention for Typ LEAKALK_ZINSEFF_NACHLASS", ez);
                        }
                    }

                    tmpsubinfos.Last().CURRENTVALUE = Subvention.saveSubventionen(subventionen, angebot, SubventionTypeConstants.Rate);
                    //decimal nachlass = laufzeit * ((decimal)angebotDto.ANGKALKRATE_DEFAULT.GetValueOrDefault() - (decimal)angebotDto.ANGKALKRATEBRUTTO.GetValueOrDefault() - (decimal)(angebotDto.ANGKALKRATE_SUBVENTION.GetValueOrDefault() / laufzeit));

                    decimal nachlass = angebotDto.ANGKALKRATE_SUBVENTION2.GetValueOrDefault();
                    if (nachlass > 0 && SysPrFld > 0)
                    {
                        _Log.Info("Nachlass Zins/Rate: " + nachlass + " Feld: " + SysPrFld);
                        subventionen = Subvention.generateImplicitSubvention(nachlass, 0, SysPrProduct, prflds, beginn, laufzeit, tmpsubinfos, "Zins- und Ratenstützungen", syspersonhd, Ust, DefaultValue);
                        //angebotDto.ANGKALKRATE_SUBVENTION += 
                        Subvention.saveSubventionen(subventionen, angebot, SubventionTypeConstants.Rate);

                    }
                    SubventionDto infoDto = Subvention.mergeSubventionDto(tmpsubinfos);
                    if (infoDto != null)
                    {
                        infoDto.DEFAULTVALUE = DefaultValue;
                        subinfos.Add(infoDto);
                    }

                }
                catch (Exception se)
                {
                    _Log.Error("Subvention for Zins/Rate failed", se);
                }

                try
                {
                    // Calculate restwert subvention
                    //this is a special case, although it is 'explicit' it has not trigger-table and is only identified by the prfld and prproduct and not the area or trgtype
                    CurrentValue = angebotDto.ANGKALKRWKALKBRUTTO_SUBVENTION.GetValueOrDefault();//calculated by bmwcalculate-service
                    DefaultValue = angebotDto.ANGKALKRWKALKBRUTTO_DEFAULT.GetValueOrDefault();


                    List<SubventionDto> tmpsubinfos = new List<SubventionDto>();
                    SysPrFld = MyGetImplSysPrParam(SubventionTypeConstants.Restwert, context, SysPrProduct, sysObTyp, sysperole, sysBrand, sysObArt);

                    subventionen = Subvention.generateExplicitSubvention(CurrentValue * laufzeit, 0, SysPrProduct, Subvention.CnstAREA_INTEREST_OR_RESIDUALVALUE, SysPrFld, beginn, laufzeit, tmpsubinfos, "Restwertstützungen", syspersonhd, Ust, DefaultValue);
                    tmpsubinfos.Last().CURRENTVALUE = Subvention.saveSubventionen(subventionen, angebot, SubventionTypeConstants.Restwert);


                    decimal nachlass = (decimal)angebotDto.ANGKALKRWKALKBRUTTO.GetValueOrDefault() - (decimal)angebotDto.ANGKALKRWKALKBRUTTO_DEFAULT.GetValueOrDefault();

                    if (nachlass > 0)
                    {

                        subventionen = Subvention.generateImplicitSubvention(nachlass, 0, SysPrProduct, SysPrFld, beginn, laufzeit, tmpsubinfos, "Restwertstützungen", syspersonhd, Ust, DefaultValue);
                        Subvention.saveSubventionen(subventionen, angebot, SubventionTypeConstants.Restwert);
                    }
                    SubventionDto infoDto = Subvention.mergeSubventionDto(tmpsubinfos);
                    if (infoDto != null)
                    {
                        infoDto.DEFAULTVALUE = DefaultValue;
                        subinfos.Add(infoDto);
                    }


                }
                catch (Exception se)
                {
                    _Log.Error("Subvention for Restwert failed", se);
                }


                try
                {
                    // Calculate MITFIN subvention---------------------------------------------------------------------------------------------------------------
                    DefaultValue = angebotDto.ANGKALKMITFIN_DEFAULT.GetValueOrDefault();
                    CurrentValue = (decimal)angebotDto.ANGKALKMITFINBRUTTO.GetValueOrDefault();
                    long syskdtyp = 1;
                    if (angebotDto.SYSIT.HasValue && angebotDto.SYSIT.Value > 0)
                    {
                        IT it = (from c in context.IT
                                 where c.SYSIT == angebotDto.SYSIT
                                 select c).FirstOrDefault();//context.SelectById<IT>((long)angebotDto.SYSIT);

                        if (it != null && it.SYSKDTYP.HasValue)
                            syskdtyp = (long)it.SYSKDTYP;
                    }
                    List<SubventionDto> subinfosfs = new List<SubventionDto>();

                    //the Subvention Info with all donors has to be the sum of all mitfin items

                    angebotDto.ANGKALKMITFIN_SUBVENTION = 0;

                    List<SubventionDto> tmpsubinfos = new List<SubventionDto>();
                    MitfinanzierteBestandteileDto[] Result = FsPreisHelper.GetMitfinanzierteBestandteileProduct(SysPrProduct, angebotDto.SYSOBTYP.HasValue ? (long)angebotDto.SYSOBTYP : 0, (angebotDto.SYSPRKGROUP.HasValue) ? (long)angebotDto.SYSPRKGROUP : 1, syskdtyp, angebotDto.SYSOBART.HasValue ? (long)angebotDto.SYSOBART : 0, laufzeit, angebotDto.ANGOBJAHRESKM.HasValue ? (long)angebotDto.ANGOBJAHRESKM : 0, true);
                    decimal subvsum = 0;
                    foreach (MitfinanzierteBestandteileDto dto in Result)
                    {
                        if (dto.isSubvention) continue;
                        if (dto.Subvention == 0) continue;
                        subvsum += dto.Subvention;
                        subventionen = Subvention.generateExplicitSubvention(dto.FsPreis, 0, SysPrProduct, Subvention.CnstAREA_SERVICE, (long)dto.SysFsTyp, beginn, laufzeit, tmpsubinfos, "Mitfinanzierte Bestandteile", syspersonhd, Ust, DefaultValue);
                        tmpsubinfos.Last().CURRENTVALUE = Subvention.saveSubventionen(subventionen, angebot, SubventionTypeConstants.MitFin);
                        angebotDto.ANGKALKMITFIN_SUBVENTION += tmpsubinfos.Last().CURRENTVALUE;
                    }
                    DefaultValue = CurrentValue + subvsum;
                    SubventionDto infoDto = Subvention.mergeSubventionDto(tmpsubinfos);
                    if (infoDto != null)
                    {
                        infoDto.DEFAULTVALUE = DefaultValue;
                        subinfos.Add(infoDto);
                    }

                }
                catch (Exception se)
                {
                    _Log.Error("Subvention for Mitfin. Bestandteile failed", se);
                }


                
                try
                {
                    //ZUGANG - only if negative!
                    decimal zugangSubValue = angebotDto.ZUGANGSPROVISION.GetValueOrDefault();
                    if (zugangSubValue < 0)
                    {
                        zugangSubValue *= -1;
                        
                        subventionen = new List<ANGSUBV>();
                        ANGSUBV subv = new ANGSUBV();
                        subv.BEGINN = beginn;
                        subv.LZ = laufzeit;
                        subv.BETRAGBRUTTO = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(zugangSubValue);
                        subv.BETRAG = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(zugangSubValue, Ust));
                        subv.BETRAGUST = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(zugangSubValue, Ust));
                        subv.BETRAGDEF = 0;//keine subvention als default
                        subv.SYSSUBVG = syspersonhd;
                        
                        //unknown:
                        //subv.SYSPRSUBV=0;
                        //subv.SUBVTYP;
                        subventionen.Add(subv);
                        Subvention.saveSubventionen(subventionen, angebot, SubventionTypeConstants.RateKredit);//adds angebot+code to angsubv

                    }
                }
                catch (Exception se)
                {
                    _Log.Error("Subvention for Zugangsprovision/ZinsEff failed", se);
                }

                VSTYPDao vsdao = new VSTYPDao(context);
                //Insurance Subventions -----------------------------------------------------------------------------
                if (angebotDto.ANGVSPARAM != null)
                {
                    try
                    {
                        //GAP - only if negative!
                        decimal gapSubventionValue = ANGEBOTAssembler.getProvisionFromInsurance(context, angebotDto, "GAP");
                        if(gapSubventionValue<0 && gapSubventionValue!= decimal.MinValue)
                        {
                            gapSubventionValue *= -1;
                            
                            subventionen = new List<ANGSUBV>();
                            ANGSUBV subv = new ANGSUBV();
                            subv.BEGINN = beginn;
                            subv.LZ = laufzeit;
                            subv.BETRAGBRUTTO = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(gapSubventionValue);
                            subv.BETRAG = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(gapSubventionValue, Ust));
                            subv.BETRAGUST = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(gapSubventionValue, Ust));
                            subv.BETRAGDEF = 0;//keine subvention als default
                            subv.SYSSUBVG = syspersonhd;
                            //unknown:
                            //subv.SYSPRSUBV=0;
                            //subv.SUBVTYP;
                            subventionen.Add(subv);
                            Subvention.saveSubventionen(subventionen, angebot, SubventionTypeConstants.GAP);//adds angebot+code to angsubv
                        
                        }
                    }
                    catch (Exception se)
                    {
                        _Log.Error("Subvention for GAP failed", se);
                    }



                    try
                    {
                        foreach (var LoopInsurance in angebotDto.ANGVSPARAM)
                        {

                            decimal vsProzent = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(LoopInsurance.InsuranceResult.Versicherungssteuer * 100 / LoopInsurance.InsuranceResult.Netto);
                            decimal praemieDefaultInklSteuer = LoopInsurance.InsuranceResult.Praemie_Default;
                            decimal nettoOrg = (praemieDefaultInklSteuer - LoopInsurance.InsuranceResult.Motorsteuer) / (1 + vsProzent / 100);
                            decimal nettoNeu = (LoopInsurance.InsuranceResult.Praemie - LoopInsurance.InsuranceResult.Motorsteuer - LoopInsurance.InsuranceResult.Versicherungssteuer);


                            decimal vssubvention = LoopInsurance.InsuranceResult.Praemie_Default;

                            VSTYP vsdata = vsdao.getVsTyp(LoopInsurance.InsuranceParameter.SysVSTYP);

                            string vsname = "Stützungen Versicherung";
                            if (vsdata == null)
                            {
                                _Log.Error("Insurance " + LoopInsurance.InsuranceParameter.SysVSTYP + " not found for calculation of Subvention");
                                continue;
                            }
                            SubventionTypeConstants fldid = SubventionTypeConstants.Versicherung;

                            //only checked insurances to calculate
                            if (vsdata.VSART.CODE.Equals(VSCalcFactory.Cnst_CALC_HAFTPFLICHT) && angebotDto.ANGKALKFSHPFLAG != 1)
                                continue;
                            if (vsdata.VSART.CODE.Equals(VSCalcFactory.Cnst_CALC_INSASSEN) && angebotDto.ANGKALKFSINSASSENFLAG != 1)
                                continue;
                            if (vsdata.VSART.CODE.Equals(VSCalcFactory.Cnst_CALC_GAP))// && angebotDto.ANGKALKFSGAPFLAG != 1)
                                continue;
                            if (vsdata.VSART.CODE.Equals(VSCalcFactory.Cnst_CALC_KASKO) && angebotDto.ANGKALKFSVKFLAG != 1)
                                continue;
                            if (vsdata.VSART.CODE.Equals(VSCalcFactory.Cnst_CALC_RECHTSSCHUTZ) && angebotDto.ANGKALKFSRECHTSCHUTZFLAG != 1)
                                continue;
                            if (vsdata.VSART.CODE.Equals(VSCalcFactory.Cnst_CALC_RESTSCHULD) && angebotDto.ANGKALKFSRSVFLAG != 1)
                                continue;
                            if (vsdata.VSART.CODE.Equals(VSCalcFactory.Cnst_CALC_HAFTPFLICHT))
                                fldid = SubventionTypeConstants.Haftpflicht;
                            else if (vsdata.VSART.CODE.Equals(VSCalcFactory.Cnst_CALC_INSASSEN))
                                fldid = SubventionTypeConstants.IUV;
                            else if (vsdata.VSART.CODE.Equals(VSCalcFactory.Cnst_CALC_KASKO))
                                fldid = SubventionTypeConstants.Kasko;
                            else if (vsdata.VSART.CODE.Equals(VSCalcFactory.Cnst_CALC_RECHTSSCHUTZ))
                                fldid = SubventionTypeConstants.Rechtsschutz;
                            else if (vsdata.VSART.CODE.Equals(VSCalcFactory.Cnst_CALC_RESTSCHULD))
                                fldid = SubventionTypeConstants.Restschuld;
                            else continue;//this vsart is not supported
                            decimal vssteuer = 1 + vsProzent / 100;

                            vsname = "Stützungen " + vsdata.VSART.BESCHREIBUNG;
                            try
                            {
                                decimal defpraemie = praemieDefaultInklSteuer * laufzeit;
                                List<SubventionDto> tmpsubinfos = new List<SubventionDto>();
                                subventionen = Subvention.generateExplicitSubvention(vssubvention * laufzeit, 0, SysPrProduct, Subvention.CnstAREA_INSURANCE, vsdata.SYSVSTYP, beginn, laufzeit, tmpsubinfos, vsname, syspersonhd, vsProzent, defpraemie);
                                LoopInsurance.InsuranceResult.Praemie_Subvention = Subvention.saveSubventionen(subventionen, angebot, fldid);
                                tmpsubinfos.Last().CURRENTVALUE = LoopInsurance.InsuranceResult.Praemie_Subvention;

                                //decimal nachlass = (praemieDefaultInklSteuer * laufzeit - LoopInsurance.InsuranceResult.Praemie_Subvention - (LoopInsurance.InsuranceResult.Praemie - LoopInsurance.InsuranceResult.Motorsteuer) * laufzeit) / laufzeit / (1 + vsProzent / 100);

                                decimal nachlass = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(((praemieDefaultInklSteuer * laufzeit - LoopInsurance.InsuranceResult.Praemie_Subvention)) / laufzeit / (1 + vsProzent / 100));
                                decimal tmpVal = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(LoopInsurance.InsuranceResult.Praemie / (1 + vsProzent / 100));


                                //decimal nachlass2 = (((praemieDefaultInklSteuer * laufzeit - LoopInsurance.InsuranceResult.Praemie_Subvention)) - (LoopInsurance.InsuranceResult.Praemie * laufzeit)) / laufzeit / (1 + vsProzent / 100);

                                nachlass = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nachlass - tmpVal);

                                if (nachlass > 0.01M)
                                {
                                    SysPrFld = MyGetImplSysPrParam(fldid, context, SysPrProduct, sysObTyp, sysperole, sysBrand, sysObArt);
                                    subventionen = Subvention.generateImplicitSubvention(nachlass * laufzeit * vssteuer, 0, SysPrProduct, SysPrFld, beginn, laufzeit, tmpsubinfos, vsname, syspersonhd, vsProzent, defpraemie);
                                    LoopInsurance.InsuranceResult.Praemie_Subvention += Subvention.saveSubventionen(subventionen, angebot, fldid);
                                }
                                SubventionDto infoDto = Subvention.mergeSubventionDto(tmpsubinfos);
                                if (infoDto != null)
                                {
                                    infoDto.DEFAULTVALUE = defpraemie;
                                    subinfos.Add(infoDto);
                                }
                            }
                            catch (Exception se1)
                            {
                                _Log.Error("Subvention not caluclated for " + vsname, se1);
                            }
                        }
                    }
                    catch (Exception se)
                    {
                        _Log.Error("Subvention for Insurances failed", se);
                    }
                }
                if (angebotDto.ANGKALKFSMAINTENANCEFLAG > 0)
                {
                    try
                    {
                        //IMPLICIT Subventionen (NUR über Nachlass-Felder)--------------------------------------------------------------------------
                        // Calculate maintenance subvention
                        DefaultValue = laufzeit * angebotDto.ANGKALKFSMAINTENANCE_DEFAULT.GetValueOrDefault();
                        CurrentValue = laufzeit * (decimal)angebotDto.ANGKALKFSMAINTENANCEBRUTTO.GetValueOrDefault();
                        DefaultValue = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(DefaultValue);
                        CurrentValue = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(CurrentValue);

                        SysPrFld = MyGetImplSysPrParam(SubventionTypeConstants.Maintenance, context, SysPrProduct, sysObTyp, sysperole, sysBrand, sysObArt);
                        subventionen = Subvention.generateImplicitSubvention(DefaultValue, CurrentValue, SysPrProduct, SysPrFld, beginn, laufzeit, subinfos, "Stützungen Wartung & Reparatur", syspersonhd, Ust, DefaultValue);
                        angebotDto.ANGKALKFSMAINTENANCE_SUBVENTION = Subvention.saveSubventionen(subventionen, angebot, SubventionTypeConstants.Maintenance);
                        subinfos[subinfos.Count - 1].DEFAULTVALUE = DefaultValue;
                    }
                    catch (Exception se)
                    {
                        _Log.Error("Subvention for Maintenance failed", se);
                    }
                }
                if (angebotDto.ANGKALKFSFUELFLAG > 0)
                {
                    try
                    {
                        // Calculate fuel price subvention
                        DefaultValue = laufzeit * angebotDto.ANGKALKFSFUELPRICE_DEFAULT.GetValueOrDefault();
                        CurrentValue = laufzeit * (decimal)angebotDto.ANGKALKFSFUELBRUTTO.GetValueOrDefault();
                        DefaultValue = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(DefaultValue);
                        CurrentValue = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(CurrentValue);

                        SysPrFld = MyGetImplSysPrParam(SubventionTypeConstants.FuelPrice, context, SysPrProduct, sysObTyp, sysperole, sysBrand, sysObArt);
                        subventionen = Subvention.generateImplicitSubvention(DefaultValue, CurrentValue, SysPrProduct, SysPrFld, beginn, laufzeit, subinfos, "Stützungen Petrol", syspersonhd, Ust, DefaultValue);
                        angebotDto.ANGKALKFSFUELPRICE_SUBVENTION = Subvention.saveSubventionen(subventionen, angebot, SubventionTypeConstants.FuelPrice);
                        subinfos[subinfos.Count - 1].DEFAULTVALUE = DefaultValue;
                    }
                    catch (Exception se)
                    {
                        _Log.Error("Subvention for Fuel Price failed", se);
                    }
                }
                if (angebotDto.ANGKALKFSANABMLDFLAG > 0)
                {
                    try
                    {
                        // Calculate anabmeldung subvention
                        DefaultValue = laufzeit * angebotDto.ANGKALKFSANABMELDUNG_DEFAULT.GetValueOrDefault();
                        CurrentValue = laufzeit * (decimal)angebotDto.ANGKALKFSANABBRUTTO.GetValueOrDefault();
                        DefaultValue = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(DefaultValue);
                        CurrentValue = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(CurrentValue);
                        SysPrFld = MyGetImplSysPrParam(SubventionTypeConstants.Anabmeldung, context, SysPrProduct, sysObTyp, sysperole, sysBrand, sysObArt);
                        subventionen = Subvention.generateImplicitSubvention(DefaultValue, CurrentValue, SysPrProduct, SysPrFld, beginn, laufzeit, subinfos, "Stützungen An- und Abmeldekosten", syspersonhd, Ust, DefaultValue);
                        angebotDto.ANGKALKFSANABMELDUNG_SUBVENTION = Subvention.saveSubventionen(subventionen, angebot, SubventionTypeConstants.Anabmeldung);
                        subinfos[subinfos.Count - 1].DEFAULTVALUE = DefaultValue;
                    }
                    catch (Exception se)
                    {
                        _Log.Error("Subvention for Anabmeldung failed", se);
                    }
                }
                if (angebotDto.ANGKALKFSREPCARFLAG > 0)
                {
                    try
                    {
                        // Calculate repcarrate subvention
                        DefaultValue = laufzeit * angebotDto.ANGKALKFSREPCARRATE_DEFAULT.GetValueOrDefault();
                        CurrentValue = laufzeit * (decimal)angebotDto.ANGKALKFSREPCARRATEBRUTTO.GetValueOrDefault();
                        DefaultValue = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(DefaultValue);
                        CurrentValue = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(CurrentValue);
                        SysPrFld = MyGetImplSysPrParam(SubventionTypeConstants.RepCarRate, context, SysPrProduct, sysObTyp, sysperole, sysBrand, sysObArt);
                        subventionen = Subvention.generateImplicitSubvention(DefaultValue, CurrentValue, SysPrProduct, SysPrFld, beginn, laufzeit, subinfos, "Stützungen Ersatzfahrzeug", syspersonhd, Ust, DefaultValue);
                        angebotDto.ANGKALKFSREPCARRATE_SUBVENTION = Subvention.saveSubventionen(subventionen, angebot, SubventionTypeConstants.RepCarRate);
                        subinfos[subinfos.Count - 1].DEFAULTVALUE = DefaultValue;
                    }
                    catch (Exception se)
                    {
                        _Log.Error("Subvention for Reparatur failed", se);
                    }
                }
                if (angebotDto.ANGKALKFSTIRESINCLFLAG > 0)
                {
                    try
                    {
                        // Calculate tires price subvention
                        DefaultValue = laufzeit * angebotDto.ANGKALKFSSTIRESPRICE_DEFAULT.GetValueOrDefault();
                        CurrentValue = laufzeit * (decimal)angebotDto.ANGKALKFSSTIRESPRICEBRUTTO.GetValueOrDefault();
                        DefaultValue = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(DefaultValue);
                        CurrentValue = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(CurrentValue);
                        SysPrFld = MyGetImplSysPrParam(SubventionTypeConstants.TiresPrice, context, SysPrProduct, sysObTyp, sysperole, sysBrand, sysObArt);
                        subventionen = Subvention.generateImplicitSubvention(DefaultValue, CurrentValue, SysPrProduct, SysPrFld, beginn, laufzeit, subinfos, "Stützungen Reifen", syspersonhd, Ust, DefaultValue);
                        angebotDto.ANGKALKFSSTIRESPRICE_SUBVENTION = Subvention.saveSubventionen(subventionen, angebot, SubventionTypeConstants.TiresPrice);
                        subinfos[subinfos.Count - 1].DEFAULTVALUE = DefaultValue;
                    }
                    catch (Exception se)
                    {
                        _Log.Error("Subvention for TiresPrice failed", se);
                    }
                }

            }
            catch (Exception e)
            {
                _Log.Error("Subventionsberechnung nicht erfolgt: " + e.Message, e);
            }



            return subinfos;
        }

        #endregion

        #region My methods

        /// <summary>
        /// Distributes and returns all Subvention Entries to save for the given SubventionList
        /// </summary>
        /// <param name="subventionValue">current subvention</param>
        /// <param name="subventionList">list of subventions</param>
        /// <param name="beginn">start of contract</param>
        /// <param name="term">term of contract</param>
        /// <param name="info">Info about distributed subventions</param>
        /// <param name="name">Name of Subvention</param>
        /// <param name="syspersonhd">Vendor ID</param>
        /// <param name="limit">Limit maximum value of granted subvention to subventionvalue</param>
        /// <param name="Ust">Ust</param>
        /// <param name="defaultVal">defaultValue</param>
        /// <returns>ANGSUBV entries for save</returns>
        private List<ANGSUBV> MyGenerateSubvention(decimal subventionValue, List<PRSUBV> subventionList, DateTime? beginn, int term, SubventionDto info, string name, long? syspersonhd, bool limit, decimal Ust, decimal defaultVal)
        {
            List<ANGSUBV> rval = new List<ANGSUBV>();

            List<SubventionPosDto> subpos = new List<SubventionPosDto>();

            foreach (PRSUBV subvention in subventionList)
            {
                decimal zuSubvention = MyCalcSubvention(subventionValue, subvention, term);
                if (limit)
                {
                    if (zuSubvention > subventionValue)
                    {
                        zuSubvention = subventionValue;
                    }
                }

                subventionValue -= zuSubvention;//pro subvention kann max. remaining ausgeschöpft werden, der rest kann auf weitere subventionen verteilt werden
                //subventionValue ist jetzt der Rest der noch subventioniert werden kann
                //zuSubvention enthält den gerade subventionierten Betrag der nun zugewiesen wird

                var slist = from subv in _context.PRSUBV
                            where subv.SYSPRSUBV == subvention.SYSPRSUBV
                            select subv.PRSUBVPOSList;

                ICollection<PRSUBVPOS> poslist = slist.FirstOrDefault();

                var vt = from subv in _context.PRSUBV
                         where subv.SYSPRSUBV == subvention.SYSPRSUBV
                         select subv.SUBVTYP;

                SUBVTYP vtyp = vt.FirstOrDefault();
                decimal sum = 0;
                decimal valForSubvention = zuSubvention;
                foreach (PRSUBVPOS pos in poslist)
                {
                    decimal facAbs = MyDistributeSubvention(valForSubvention, ref zuSubvention, pos, subvention);

                    //create and save the SUBVENTION to database
                    ANGSUBV s = new ANGSUBV();
                    sum += facAbs;
                    s.BEGINN = beginn;
                    s.BETRAGBRUTTO = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(facAbs);
                    s.BETRAG = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(facAbs, Ust));
                    s.BETRAGUST = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(facAbs, Ust));
                    s.BETRAGDEF = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(defaultVal);
                    s.SUBVTYP = vtyp;
                    s.SYSPRSUBV = subvention.SYSPRSUBV;

                    s.LZ = subvention.TERM != null && subvention.TERM > 0 ? (int)subvention.TERM : term;

                    SubventionPosDto sinfo = new SubventionPosDto();
                    sinfo.SYSPRSUBVPOS = pos.SYSPRSUBVPOS;
                    sinfo.VALUE = s.BETRAGBRUTTO;
                    sinfo.VALUE_NETTO = s.BETRAG;
                    sinfo.VALUE_UST = s.BETRAGUST;

                    //wenn keine person hinterlegt, auf händler 
                    long? sysp = pos.SYSPERSON.HasValue && pos.SYSPERSON > 0 ? (long)pos.SYSPERSON : syspersonhd;
                    if (sysp.HasValue && sysp > 0)
                    {
                        sinfo.SYSPERSON = (long)sysp;
                        PERSON pers = PERSONHelper.SelectBySysPERSONWithoutException(_context, (long)sysp);
                        sinfo.VORNAME = pers.VORNAME;
                        if (sinfo.VORNAME == null || sinfo.VORNAME.Trim().Length == 0)
                            sinfo.VORNAME = "";
                        sinfo.NACHNAME = pers.NAME;
                        if (sinfo.NACHNAME == null || sinfo.NACHNAME.Trim().Length == 0)
                            sinfo.NACHNAME = "";
                    }
                    subpos.Add(sinfo);
                    s.SYSSUBVG = (long)sysp;
                    rval.Add(s);
                    if (zuSubvention <= 0) break;
                }
                info.VALUE = sum;
                info.SUBVENTIONSGEBER = subpos.ToArray();

                if (zuSubvention > 0)
                    subventionValue += zuSubvention; //wurde von aktueller Stützung nicht verwendet, für weitere wieder verfügbar machen

                if (subventionValue <= 0) break;
            }
            return rval;

        }


        /// <summary>
        /// Distributes subventionValue to the Subvention-Donor
        /// If the remaining subventionValue would be below zero, the return value will be
        /// reduced accordingly and subventionValue will be zero
        /// </summary>
        /// <param name="subventionValue">current remaining subvention value</param>
        /// <param name="pos">Subvention Donor Information object</param>
        /// <param name="sub">Subvention Information object</param>
        /// <param name="subventionSum">Subvention sum as base for percentage distribution</param>
        /// <returns>the value the donor has to pay</returns>
        private decimal MyDistributeSubvention(decimal subventionSum, ref decimal subventionValue, PRSUBVPOS pos, PRSUBV sub)
        {
            decimal rval = 0;
            if (sub.SHAREMETHOD == _CnstSHAREMETHOD_VALUE)
            {
                rval = (decimal)pos.PARTVAL;
            }
            else if (sub.SHAREMETHOD == _CnstSHAREMETHOD_PERCENT)
            {
                rval = (decimal)pos.PARTRATE / 100 * subventionSum;

            }
            if (subventionValue - rval < 0)
            {
                rval = subventionValue;
            }
            subventionValue -= rval;
            return rval;
        }

        /// <summary>
        /// Calculates the subvention for the given value and subvention-entity
        /// Only percent-Methods take the current item value into account
        /// </summary>
        /// <param name="value">the value to calculate the subvention for</param>
        /// <param name="subvention">the configuration entity object for the subvention</param>
        /// <param name="term">Duration</param>
        /// <returns></returns>
        private decimal MyCalcSubvention(decimal value, PRSUBV subvention, long term)
        {
            bool subventionLz = subvention.TERM.HasValue && subvention.TERM.Value > 0;
            decimal lz = subventionLz ? (decimal)subvention.TERM : (decimal)term;
            if (term != 0)//if a different term is configured, calculate the percentage in relation to the whole term
                lz /= ((int)term * 1.0M);

            switch (subvention.CALCMETHOD)
            {
                case (_CnstCALCMETHOD_PERCENT):
                    return value * (decimal)subvention.SUBVRATE * lz;

                case (_CnstCALCMETHOD_PERCENTAGE):
                    return value * (decimal)subvention.SUBVRATE / 100 * lz;

                case (_CnstCALCMETHOD_VALUE):
                    return (decimal)subvention.SUBVVAL * lz;

            }
            throw new ArgumentException("Subvention enthält keine Berechnungsmethode");
        }

        /// <summary>
        /// creates the correct trigger subvention query
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        private static string MyDeliverQuery(long area)
        {
            string query = "select PRSUBV.* from VN_PRSUBV, PRSUBV  where VN_PRSUBV.SYSPRPRODUCT=:sysprproduct and PRSUBV.SYSPRSUBV = VN_PRSUBV.SYSPRSUBV and " + 
                            " PRSUBV.trgtype=1 and PRSUBV.AREA=:Area and PRSUBV.SYSPRFLDTGT=:AreaId  order by PRSUBV.RANK";

            switch (area)
            {
                case (CnstAREA_INTEREST_OR_RESIDUALVALUE):
                    //query for using a prparam
                   /* if (USE_PRPARAM_NOT_PRFLD)
                        query = "select PRSUBV.* from VN_PRSUBV, PRSUBV  where VN_PRSUBV.SYSPRPRODUCT=:sysprproduct and PRSUBV.SYSPRSUBV = VN_PRSUBV.SYSPRSUBV  " +
                            " and (PRSUBV.trgtype=1 or PRSUBV.trgtype=0) and (PRSUBV.AREA=:Area or PRSUBV.AREA>=0) and PRSUBV.SYSPRFLDTRG=:AreaId  order by PRSUBV.RANK";
                    else
                        //query for using a prfield - also replace at position XPRFIELDX in this file
                        query = "select PRSUBV.* from VN_PRSUBV, PRSUBV,prparam  where VN_PRSUBV.SYSPRPRODUCT=:sysprproduct and PRSUBV.SYSPRSUBV = VN_PRSUBV.SYSPRSUBV  " +
                            " and (PRSUBV.trgtype=1 or PRSUBV.trgtype=0) and (PRSUBV.AREA=:Area or PRSUBV.AREA>=0) and  PRSUBV.SYSPRFLDTRG=prparam.sysprparam and prparam.sysprfld=:AreaId  order by PRSUBV.RANK";
                    * */
                    break;
                case (CnstAREA_CHARGE):     //Gebühr
                    query = "select PRSUBV.* from VN_PRSUBV, PRSUBV , PRSUBVTRGGEB, GEBUEHR where VN_PRSUBV.SYSPRPRODUCT=:sysprproduct and " + 
                            " GEBUEHR.SYSGEBUEHR=PRSUBVTRGGEB.SYSGEBUEHR and PRSUBVTRGGEB.SYSPRSUBV=PRSUBV.SYSPRSUBV and PRSUBV.SYSPRSUBV = VN_PRSUBV.SYSPRSUBV and PRSUBV.trgtype=1 and PRSUBV.AREA=:Area and prsubvtrggeb.sysgebuehr=:AreaId  order by PRSUBV.RANK";
                    break;
                case (CnstAREA_SERVICE):    //Service
                    query = "select PRSUBV.* from VN_PRSUBV, PRSUBV, PRSUBVTRGFS, FSTYP where VN_PRSUBV.SYSPRPRODUCT=:sysprproduct and FSTYP.SYSFSTYP=PRSUBVTRGFS.SYSFSTYP and "+
                            " PRSUBVTRGFS.SYSPRSUBV=PRSUBV.SYSPRSUBV and PRSUBV.SYSPRSUBV = VN_PRSUBV.SYSPRSUBV and PRSUBV.trgtype=1 and PRSUBV.AREA=:Area and PRSUBVTRGFS.SYSFSTYP=:AreaId  order by PRSUBV.RANK";
                    break;
                case (CnstAREA_INSURANCE):  //Versicherung
                    query = "select PRSUBV.* from VN_PRSUBV, PRSUBV  , PRSUBVTRGVS, VSTYP where VN_PRSUBV.SYSPRPRODUCT=:sysprproduct and  VSTYP.SYSVSTYP=PRSUBVTRGVS.SYSVSTYP " + 
                            " and PRSUBVTRGVS.SYSPRSUBV=PRSUBV.SYSPRSUBV and PRSUBV.SYSPRSUBV = VN_PRSUBV.SYSPRSUBV and PRSUBV.trgtype=1 and PRSUBV.AREA=:Area and PRSUBVTRGVS.SYSVSTYP=:AreaId  order by PRSUBV.RANK";
                    break;

                default:
                    break;
                    // throw new ArgumentException("AREA-Typ für Subventionsberechnung nicht definiert");
            }
            return query;
        }
        /*
        private static long MyGetSysPrFld(SubventionTypeConstants type, OlExtendedEntities context)
        {
            // Get the field info
            FieldInfo Field = typeof(SubventionTypeConstants).GetField(type.ToString());

            // Get the attributes
            DescriptionAttribute Description = (DescriptionAttribute)Attribute.GetCustomAttribute(Field, typeof(DescriptionAttribute));

            // Check if the attributes array is not empty
            if (Description == null)
            {
                // Throw an exception
                throw new Exception("The description attribute of \"" + type + "\" constant could not be found.");
            }

            PrismaDao pd = new PrismaDao();
            
            // Get PrFld
            var CurrentPrFld = (from PrFld in context.PRFLD
                                where PrFld.NAME == Description.Description
                                && PrFld.PRFLDART.NAME == CnstSubventionsmappingPrFldArt && PrFld.PRFLDART.ACTIVEFLAG == 1
                                orderby PrFld.SYSPRFLD descending
                                select PrFld).FirstOrDefault();

            // Check if nothing was found
            if (CurrentPrFld == null)
            {
                // Throw an exception
                throw new Exception("PrFld for Subvention \"" + type + "\" with name "+Description.Description+" for FieldArt "+CnstSubventionsmappingPrFldArt+" could not be found.");
            }

            // Return the id
            return CurrentPrFld.SYSPRFLD;
        }
        */
        private static long MyGetImplSysPrParam(SubventionTypeConstants type, DdOlExtended context, long sysPrProduct, long sysObTyp, long sysPEROLE, long sysBRAND, long sysObArt)
        {
            // Get the field info
            FieldInfo Field = typeof(SubventionTypeConstants).GetField(type.ToString());

            // Get the attributes
            DescriptionAttribute Description = (DescriptionAttribute)Attribute.GetCustomAttribute(Field, typeof(DescriptionAttribute));

            // Check if the attributes array is not empty
            if (Description == null)
            {
                // Throw an exception
                throw new Exception("The description attribute of \"" + type + "\" constant could not be found.");
            }

            if (USE_PRPARAM_NOT_PRFLD)
            {
                //Use specific parameter (allows different subventions for eg PKW and Motorrad)
                //for using a prparam - also replace at position XPRFIELDX in this file
                PRParamDao prparamDao = new PRParamDao(context);
                PRPARAMDto par = prparamDao.DeliverPrParam(sysPrProduct, sysObTyp, sysPEROLE, sysBRAND, Description.Description, sysObArt, false);
                if (par != null)
                    return par.SYSPRPARAM;

                throw new Exception("PrFld for Subvention \"" + type + "\" with name " + Description.Description + " could not be found.");
            }
            else
            {
                PrismaDao pd = new CachedPrismaDao();
                //query for using a prfield - also replace at position XPRFIELDX in this file
                //use general parameter (independent of assigned prisma parameter)
                // Get PrFld
                var CurrentPrFld = (from param in pd.getParams()
                                    where param.meta == Description.Description
                                    select param).FirstOrDefault();

                // Check if nothing was found
                if (CurrentPrFld == null)
                {
                    // Throw an exception
                    throw new Exception("PrFld for Subvention \"" + type + "\" with name " + Description.Description + " could not be found.");
                }

                // Return the id
                return CurrentPrFld.sysprfld;
            }
        }

        #endregion
    }
}