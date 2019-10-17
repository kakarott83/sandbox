using Cic.OpenLease.Service.Services.DdOl;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxRef;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using System;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace Cic.OpenLease.Service
{
    class RwInfoDto
    {
        public long sysvgrw { get;set;}
        public String schwacke { get; set; }
        public String marke { get; set; }
    }
    /// <summary>
    /// Class to calculate the BMW specific residual value suggestion including the BMW austria CRV charge
    /// </summary>
    [System.CLSCompliant(true)]
    public class RVSuggest
    {
        
        
        private const string _CnstSA_KORR = "SA_KORR";
        private const string _CnstRW_KORR_FIN = "RW_KORR_FIN";
        private const string _CnstBALLON  = "BALLON";
        private const string _CnstVARTBALLON = "KREDIT_BALLON";
        

        #region Private variables
        private decimal _sfBase;
        private decimal _crv;
        private decimal _sfBaseprozent;
        private decimal _sfBaseprozentLP;
        private decimal _crvprozent;
        private decimal _saRVPercent;
        private decimal _crvprozentLP;
        private decimal calcBase=0;
        private decimal sonderminderung = 0;
        private int obarttyp = 0;
        private long sysobart = 0;
        private decimal _listenpreis;//laut neuer definition jetzt netto inkl. nova inkl. novaaufschlag
        private bool _noCRV = false;
        private String vartCode;
        private static CacheDictionary<String, decimal> rwpercentCache = CacheFactory<String, decimal>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static long fallbackvg = 0;
        private static long sysvgrw = 0;
        private static double rwDefaultPercent = 0;
        private DdOlExtended _context;
        #endregion

        #region Properties
        public decimal sfBase
        {
            get { return _sfBase; }
            set { _sfBase = value; }
        }
        public decimal crv
        {
            get { return _crv; }
            set { _crv = value; }
        }
        public decimal sfBaseprozent
        {
            get { return _sfBaseprozent; }
            set { _sfBaseprozent = value; }
        }
        public decimal sfBaseprozentLP
        {
            get { return _sfBaseprozentLP; }
            set { _sfBaseprozentLP = value; }
        }
        public decimal crvprozent
        {
            get { return _crvprozent; }
            set { _crvprozent = value; }
        }
        public decimal crvprozentLP
        {
            get { return _crvprozentLP; }
            set { _crvprozentLP = value; }
        }
        public decimal saRVPercent
        {
            get { return _saRVPercent; }
            set { _saRVPercent = value; }
        }
        public bool noCRV
        {
            get { return _noCRV; }
            set { _noCRV = value; }
        }
        private decimal bonusmalusexklzuschlag { get; set; }
        private decimal ust { get; set; }
        private decimal nova_percent { get; set; }
        private bool isOverflow = false;
        #endregion

        #region Constructors
        public RVSuggest(DdOlExtended context)
        {
            _context = context;
            if(fallbackvg==0)
                fallbackvg = context.ExecuteStoreQuery<long>("select sysvg from vg where name='Fallback'", null).FirstOrDefault();
            if (sysvgrw == 0)
                sysvgrw = context.ExecuteStoreQuery<long>("select sysvg from vg where name='Restwerte Gebrauchtwagen'", null).FirstOrDefault();
            if(rwDefaultPercent==0)
                rwDefaultPercent = (double)QUOTEDao.deliverQuotePercentValueByName("FALLBACK_RW_GEBRAUCHTWAGEN");
        }
        #endregion

        #region Methods

        public void initialize (decimal grundnettonetto, decimal sonzub, decimal sarvnetto, decimal bonusmalusinklzuschlag, decimal ust, 
			decimal nova_percent, decimal sonderminderung, String vartcode, int obarttyp, long sysobart, decimal rabattfaehigBetragExtern)
        {
            this.obarttyp = obarttyp;
            this.sysobart = sysobart;
            sonzub -= sarvnetto;
            grundnettonetto += sarvnetto;
            this.sonderminderung = sonderminderung;

            NovaType nt = new NovaType(NovaType.fetchNovaQuote(), 0, 0, 0);
            nt.setBruttoInklNova(bonusmalusinklzuschlag);
            
            this.bonusmalusexklzuschlag = nt.netto;
            this.nova_percent = nova_percent;
            this.ust = ust;
            this.vartCode = vartcode;
            _listenpreis = grundnettonetto + sonzub + bonusmalusexklzuschlag;

			if (sysobart == 13)		// Gebraucht immer Wertetabelle
			{
				decimal rabattfaehigBetragExternNetto = Cic.OpenLease.Service.MwStFacade.getInstance ().CalculateNetValue (rabattfaehigBetragExtern, ust);
				calcBase = rabattfaehigBetragExternNetto;
				_listenpreis = rabattfaehigBetragExternNetto;
			}
		}

		/// <summary>
		/// Fetches the EUrotax Forecast
		/// </summary>
		/// <param name="ubnahmekm"></param>
		/// <param name="ll"></param>
		/// <param name="lz"></param>
		/// <param name="age"></param>
		/// <param name="schwacke"></param>
		/// <param name="erstzul"></param>
		/// <param name="sonzub"></param>
		/// <param name="defValue"></param>
		/// <returns></returns>
		private decimal getRwFromEurotax(long ubnahmekm, long ll, long lz, int age, String schwacke, DateTime erstzul, decimal sonzub)
        {
           
                String cacheKey = ubnahmekm + "_" + ll + "_" + lz + "_" + age + "_" + schwacke + "_" + sonzub + "_" + erstzul;
                if (!rwpercentCache.ContainsKey(cacheKey))
                {
                    
                    IEurotaxBo etbo = AuskunftBoFactory.CreateDefaultEurotaxBo();
                    EurotaxInDto etin = new EurotaxInDto();
                    etin.CurrentMileageValue = (uint)ubnahmekm;
                    etin.EstimatedAnnualMileageValue = (uint)ll;

                    etin.ForecastPeriodFrom = "" + (lz);//+age
                    etin.ForecastPeriodUntil = etin.ForecastPeriodFrom;
                    etin.NationalVehicleCode = Convert.ToInt64(schwacke);
                    etin.RegistrationDate = erstzul;
                    etin.ISOCountryCode = ISOcountryType.DE;
                    etin.ISOCurrencyCode = ISOcurrencyType.EUR;
                    etin.ISOLanguageCode = ISOlanguageType.DE;
                    etin.TotalListPriceOfEquipment = (double)sonzub;
                    EurotaxOutDto etout = etbo.getEurotaxForecast(etin);
                    rwpercentCache[cacheKey] = (decimal)etout.RetailValueInPercentage;
                }
                return rwpercentCache[cacheKey];
           
        }
        
        private void wflog(String reason)
        {
            Cic.OpenOne.Common.Util.Logging.LogUtil.addWFLog("B2B-Restwert-Retail", reason,2);
        }

        private static CacheDictionary<long, RwInfoDto> rwInfoCache = CacheFactory<long, RwInfoDto>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);

        public void calculateRV(long sysKalkTyp, long sysObtyp, decimal grund, decimal sonzub, long ubnahmekm, decimal ll, long lz, DateTime erstzul, DateTime perDate, bool isSA3, decimal kpnetto, decimal sarvnetto, decimal rabattfaehigBetragExtern)
        {
            KORREKTURDao kh = new KORREKTURDao(_context);
            //bool isBallon = vartCode.Equals(_CnstVARTBALLON);
            decimal addBrandPercent = 0;
            decimal rwTabPercent = -1;
            string op = "+";
            decimal aufabschlag = (decimal)kh.Correct("RW_KORR_VART", 0, op, DateTime.Now, vartCode, "");
            //calculate age in months since erstzul till perDate
            int age;
            if (erstzul.Year < 1801) erstzul = perDate;//avoid invalid age

            for (age = 0; erstzul.AddMonths(age + 1).CompareTo(perDate) <= 0; age++) ;
           
            decimal rabattfaehigBetragExternNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(rabattfaehigBetragExtern, ust);
           
          
            try
            {
                sonzub -= sarvnetto;
                grund += sarvnetto;
                _listenpreis = grund + sonzub + bonusmalusexklzuschlag;

               /* if (isBallon)
                {
                    rwTabPercent = MydeliverBallon(lz, age, perDate, grund);
                    saRVPercent = 100;
                    calcBase = rabattfaehigBetragExternNetto;
                    _listenpreis = rabattfaehigBetragExternNetto;
                }
                else*/
                {

                    
                    if (!rwInfoCache.ContainsKey(sysObtyp))
                    {
                        rwInfoCache[sysObtyp] =  _context.ExecuteStoreQuery<RwInfoDto>("select schwacke, sysvgrw, (select max(etgmake.name) from etgmake,etgtype where etgmake.natcode=etgtype.makcd and etgtype.natcode=obtyp.schwacke) marke from obtyp where sysobtyp=" + sysObtyp, null).FirstOrDefault();

                        String dayStr = "to_date('" + perDate.Year + "-" + perDate.Month + "-" + perDate.Day + "', 'yyyy-mm-dd')";
                        String validStr = SQL.CheckDate(dayStr, "vgvalid");
                        rwInfoCache[sysObtyp].sysvgrw = _context.ExecuteStoreQuery<long>("select sysvgrw from obtyp where sysvgrw != 0 and exists(select vg.sysvg from vg,vgvalid where vg.sysvg = sysvgrw and vgvalid.sysvg=vg.sysvg and "+validStr+") start with sysobtyp = " + sysObtyp + " connect by prior sysobtypp = sysobtyp").FirstOrDefault();
                    }
                    RwInfoDto rwinfo = rwInfoCache[sysObtyp];



                    //RW-Tab Interpolation


                    isOverflow = false;
                    /*  12	0	Neuwagen
                        13	1	Gebrauchtwagen
                        14	2	Vorführwagen
                        15	2	Tageszulassung
                                             * */
                    long usedx = 0, usedy = 0;
                    if ( sysobart==13 )//Gebraucht immer Wertetabelle
                    {
                        calcBase = rabattfaehigBetragExternNetto;
                        _listenpreis = rabattfaehigBetragExternNetto;
                        //Das Alter soll hier keine Rolle spielen
                        rwTabPercent = (decimal)deliverUsedCarRV(perDate, (int)(lz), rabattfaehigBetragExtern);
                        /*try
                        {
                            rwTabPercent = getRwFromEurotax(ubnahmekm, (long)ll, lz, age, rwinfo.schwacke, erstzul, sonzub);
                        }
                        catch (Exception e)
                        {
                            wflog("Eurotax-Forecast was not possible for new/used car" + rwinfo.schwacke + " ll:" + ll + " lz:" + lz + " age: " + age + " ubnahme:" + ubnahmekm + " sonzub:" + sonzub + " ez:" + erstzul + " using " + rwTabPercent + " reason: " + e.Message);
                            if(fallbackvg>0)
                                rwTabPercent = MydeliverSFBase(fallbackvg, ubnahmekm, ll, lz, age, perDate, ref isOverflow, ref usedx, ref usedy);
                            if (isOverflow)
                            {
                                rwTabPercent = 10;
                                _Log.Error("RV - Table Forecast was not possible for new/used car " + rwinfo.schwacke + " ll:" + ll + " lz:" + lz + " age: " + age + " ubnahme:" + ubnahmekm + " sonzub:" + sonzub + " ez:" + erstzul + " using " + rwTabPercent, e);
                                wflog("RV-Table Forecast was not possible for new/used car " + rwinfo.schwacke + " ll:" + ll + " lz:" + lz + " age: " + age + " ubnahme:" + ubnahmekm + " sonzub:" + sonzub + " ez:" + erstzul + " using " + rwTabPercent + " reason: " + e.Message);
                            }

                        }*/

                    }
                    else if (rwinfo.sysvgrw > 0)//sonst immer rwtabelle (15=tageszulassung, 14=Vorführwagen, 12=Neuwagen)
                    {
                        rwTabPercent = MydeliverSFBase(rwinfo.sysvgrw, ubnahmekm, ll, lz, age, perDate, ref isOverflow, ref usedx, ref usedy);
                        if (isOverflow)
                        {
                            if(fallbackvg>0)
                                rwTabPercent = MydeliverSFBase(fallbackvg, ubnahmekm, ll, lz, age, perDate, ref isOverflow, ref usedx, ref usedy);
                            if (isOverflow)
                            {
                                rwTabPercent = 10;//TODO fallback rwgroup
                                wflog("RV-Table Forecast was not possible for " + rwinfo.schwacke + " ll:" + ll + " lz:" + lz + " age: " + age + " ubnahme:" + ubnahmekm + " sonzub:" + sonzub + " ez:" + erstzul + " using " + rwTabPercent + " reason: no value found in vg=" + rwinfo.sysvgrw + " for " + usedx + "/" + usedy);
                            }

                        }
                    }
                    else//wenn keine wertetabelle
                    {
                        if ("KIA".Equals(rwinfo.marke) || "HYUNDAI".Equals(rwinfo.marke))
                        {
                            if(fallbackvg>0)
                                rwTabPercent = MydeliverSFBase(fallbackvg, ubnahmekm, ll, lz, age, perDate, ref isOverflow, ref usedx, ref usedy);
                            if (isOverflow)
                            {
                                rwTabPercent = 10;//TODO fallback rwgroup
                                //bei kia/hyundai immer default, nie eurotax
                                wflog("RV-Table Forecast not possible for " + rwinfo.schwacke + " ll:" + ll + " lz:" + lz + " age: " + age + " ubnahme:" + ubnahmekm + " sonzub:" + sonzub + " ez:" + erstzul + " using " + rwTabPercent + " reason: no vg for obtyp registered and not tried Eurotax because of manufacturer " + rwinfo.marke);
                            }
                        }
                        else
                        {
                            //if no kia/hyunday, perform eurotax-query
                            try
                            {
                                rwTabPercent = getRwFromEurotax(ubnahmekm, (long)ll, lz, age, rwinfo.schwacke, erstzul, sonzub);
                            }
                            catch (Exception e)
                            {
                                wflog("Eurotax-Forecast was not possible for non KIA/Hyundai " + rwinfo.schwacke + " ll:" + ll + " lz:" + lz + " age: " + age + " ubnahme:" + ubnahmekm + " sonzub:" + sonzub + " ez:" + erstzul + " using " + rwTabPercent + " reason: " + e.Message);
                                if(fallbackvg>0)
                                    rwTabPercent = MydeliverSFBase(fallbackvg, ubnahmekm, ll, lz, age, perDate, ref isOverflow, ref usedx, ref usedy);
                                if (isOverflow)
                                {
                                    rwTabPercent = 10;
                                    _Log.Error("RV-Table Forecast not possible for " + rwinfo.schwacke + " ll:" + ll + " lz:" + lz + " age: " + age + " ubnahme:" + ubnahmekm + " sonzub:" + sonzub + " ez:" + erstzul + " using " + rwTabPercent, e);
                                    wflog("RV-Table Forecast not possible for non KIA/Hyundai " + rwinfo.schwacke + " ll:" + ll + " lz:" + lz + " age: " + age + " ubnahme:" + ubnahmekm + " sonzub:" + sonzub + " ez:" + erstzul + " using " + rwTabPercent + " reason: " + e.Message);
                                }

                            }
                        }
                    }
                    decimal sePercent = Math.Round((sonzub / (grund) * 100), 1, MidpointRounding.AwayFromZero);
                    saRVPercent = 100;
                    calcBase = (grund + (sonzub * saRVPercent / 100));
               
                }
                //Product-Correction
                decimal addProductPercent = aufabschlag;
                if (isOverflow)//Ticket #3468
                {
                    addProductPercent = 0;
                    addBrandPercent = 0;
                }
                //SF-BASE Percent
                decimal sfBasePercent = rwTabPercent + addBrandPercent + addProductPercent;
                if (sfBasePercent < 0) sfBasePercent = 0;


                //CRV
                decimal crvPercent = sfBasePercent;

                _Log.Debug("RV-Calculation for SYSOBTYP " + sysObtyp + " LP Netto: " + grund + " SZ: " + sonzub + " LL: " + ll + " LZ: " + lz + " Markenaufschlag: " + addBrandPercent + " RW-Tab: " + rwTabPercent + " Produktaufschlag: " + addProductPercent + " CRV: " + crvPercent + " sa3: " + isSA3 + " EZ: " + erstzul+" PER: "+perDate+" SARV: "+sarvnetto);
                if (grund == 0)
                {
                    _sfBase = 0;
                    _crv = 0;
                    return;
                }

                
                //Calculation of results
                _sfBaseprozent = sfBasePercent;
                _crvprozent = crvPercent; 
                _sfBase = calcBase * (sfBasePercent) / 100;
                _crv = calcBase * (crvPercent) / 100;
                _crvprozentLP = crvPercent;
                _sfBaseprozentLP = sfBasePercent;

                
                _crvprozentLP = Math.Round(_crv / (grund + sonzub) * 100, 2);
                _sfBaseprozentLP = Math.Round(_sfBase / (grund + sonzub) * 100, 2);
                
                //Round it:
                _crv = getRestwertNetto(_crvprozentLP).netto;

                _sfBase = getRestwertNetto(_sfBaseprozentLP).netto;

                _Log.Debug("SF-Base: " + _sfBase + " CRV: " + _crv + " SAProzent: " + saRVPercent);
            }
            catch (System.InvalidOperationException ioe)
            {
                _Log.Error("RV-Calculation", ioe);
                // TODO Exceptionhandling with Resources
                throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.GeneralSelect, "Can't calculate RV, invalid SYSOBTYP or perDate.");
            }


        }

        private double deliverUsedCarRV(DateTime perDate, int age, decimal wagenPreis)
        {
            double rwTabPercent;
            ValueGroupDao vgdao = new ValueGroupDao();
            //VGDao vg = new VGDao(_context);

            VGBoundaries vg = vgdao.getVGBoundaries(sysvgrw, perDate);
            long lzgw = age;
            
            if (lzgw < vg.xmin)
                lzgw = (long)vg.xmin;
            

            if (lzgw > vg.xmax)
            {
                rwTabPercent = rwDefaultPercent;
            }
            else
            {
                rwTabPercent = vgdao.getVGValue(sysvgrw, perDate, lzgw.ToString(), "0", VGInterpolationMode.VERTICAL);
            }

            return rwTabPercent;
        }

        private static long countRWSubv =-1;

        /// <summary>
        /// NOT USED FOR HCBE
        /// </summary>
        /// <param name="sysPrProduct"></param>
        /// <param name="ubnahmekm"></param>
        /// <param name="ll"></param>
        /// <param name="lz"></param>
        /// <param name="erstzul"></param>
        /// <param name="perDate"></param>
        /// <param name="isOverflow"></param>
        /// <returns></returns>
        public decimal calculateSubvention(long sysPrProduct, long ubnahmekm, decimal ll, long lz, DateTime erstzul, DateTime perDate,ref bool isOverflow)
        {
            

           
            try
            {
                long RWSYSVG = 0;

                if(countRWSubv<0)
                {
                    countRWSubv= _context.ExecuteStoreQuery<long>("select count(*) from vg,prproduct where sysprproduct>0 and vg.mappingext=prproduct.nameintern").FirstOrDefault();
                }
                if (countRWSubv == 0)
                    return 0;

                string query = "select vg.sysvg from vg,prproduct where sysprproduct="+sysPrProduct+" and vg.mappingext=prproduct.nameintern";
                RWSYSVG = _context.ExecuteStoreQuery<long>(query, null).FirstOrDefault();
                if (RWSYSVG == 0)
                {
                    _Log.Debug("Subvention for Product-RW not defined: " + sysPrProduct);
                    return 0;
                }

                //RW-Tab Interpolation
                //calculate age in months since erstzul till perDate
                int age;
                if (erstzul.Year < 1801) erstzul = perDate;//avoid invalid age

                for (age = 0; erstzul.AddMonths(age + 1).CompareTo(perDate) <= 0; age++) ;
                long usedx = 0, usedy = 0;
                decimal rwTabPercent = MydeliverSFBase(RWSYSVG, ubnahmekm, ll, lz, age, perDate, ref isOverflow,ref usedx, ref usedy);
                if (isOverflow)
                    rwTabPercent = 0;

                return rwTabPercent;
            }
            catch (Exception e )
            {
                _Log.Error("Subvention for Product-RW failed: " + sysPrProduct, e);
                return 0;
            }


        }

        
        /// <summary>
        /// parameter: ermittelter Prozentsatz aus RW-tabellen (mit Sa-Anpassung)
        /// Ergebnis: Restwert Netto inkl. Nova (inkl. Zuschlag)
        /// </summary>
        /// <param name="rwPercent"></param>
        /// <returns></returns>
        public NovaType getRestwertNetto(decimal rwPercent)
        {
            NovaType nt = new NovaType(ust, nova_percent, NovaType.fetchNovaQuote(), sonderminderung*(rwPercent/100));

            nt.setNetto( (_listenpreis - bonusmalusexklzuschlag)*rwPercent/100);
            //decimal rwnovabonusmalusexklzuschlag = bonusmalusexklzuschlag * rwPercent / 100;
            //nt.addBonusMalus(rwnovabonusmalusexklzuschlag);

            return nt;
          
        }

        public NovaType getValuesFromNetto(decimal nettonetto)
        {
          
            decimal rwPercent = getRestwertPercent(nettonetto);
            return getRestwertNetto(rwPercent);
           
        }

        public decimal getRestwertPercentFromBrutto(decimal rwbrutto)
        {

            NovaType nt = new NovaType(ust, nova_percent, NovaType.fetchNovaQuote(), sonderminderung );

            nt.setNetto(_listenpreis - bonusmalusexklzuschlag);
            decimal fahrzeugnettoinklnovainklbonusexklzuschlag = (_listenpreis + nt.nova);
            decimal fzbrutto = nt.ust+fahrzeugnettoinklnovainklbonusexklzuschlag + nt.novaZuschlag + bonusmalusexklzuschlag * nt.novaZuschlagPercent / 100;
            return rwbrutto / fzbrutto * 100;
        }

        public NovaType getRestwertFromBrutto(decimal rwbrutto)
        {
            return getRestwertNetto(getRestwertPercentFromBrutto(rwbrutto));
        }

        /// <summary>
        /// Ermittelt aus dem NettoNetto-RW den Prozentsatz
        /// </summary>
        /// <param name="rwNettoNetto"></param>
        /// <returns></returns>
        public decimal getRestwertPercent(decimal rwNettoNetto)
        {
            NovaType nt = new NovaType(ust, nova_percent, NovaType.fetchNovaQuote(), sonderminderung);
            nt.setNetto(_listenpreis - bonusmalusexklzuschlag);

            return rwNettoNetto * (1 + ust / 100) / nt.bruttoExklNova * 100;

      
        }
       
        #endregion

        #region My methods
        private decimal MydeliverCRV(long sysVg, DateTime perDatum)
        {

            DbParameter[] Parameters = 
                { 
                        new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysvg", Value = sysVg},
                        new Devart.Data.Oracle.OracleParameter{ ParameterName = "perDate", Value = perDatum}
                        };

            try
            {
                return _context.ExecuteStoreQuery<decimal>("select vgadj.value from vgadjvalid,vgadjtrg,vgavg,vgadj   where VgAdjTrg.Name = 'CRV' and  VgAdjValid.NAME = 'CRV Korrektur'  and vgavg.sysvg=:sysvg and   vgadjvalid.sysvgadjvalid=vgadjtrg.sysvgadjvalid and vgavg.sysvgadjvalid=vgadjvalid.sysvgadjvalid and vgadj.sysvgadjtrg=vgadjtrg.sysvgadjtrg and vgadj.sysvgavg=vgavg.sysvgavg and " 
                // + " (vgadjvalid.validfrom is null or vgadjvalid.validfrom<=:perDate) and  (vgadjvalid.validuntil is null or vgadjvalid.validuntil>=:perDate)"
                + SQL.CheckDate (" :perDate ", "vgadjvalid") 
                , Parameters).FirstOrDefault();

            }
            catch (Exception ex)
            {
                _Log.Error("CRV correction value detection failed - no data found for sysvg: " + sysVg + ": " + ex.Message);
            }
            return 0;

        }
        /// <summary>
        /// returns the configured residual value
        /// </summary>
        /// <param name="sysVg"></param>
        /// <param name="mileage">Kilometerstand</param>
        /// <param name="kilometer">=ll/monat</param>
        /// <param name="duration">laufzeit</param>
        /// <param name="age"></param>
        /// <param name="perDatum"></param>
        /// <param name="isOverflow"></param>
        /// <returns></returns>
        private decimal MydeliverSFBase(long sysVg, long mileage, decimal kilometer, long duration, long age, DateTime perDatum, ref bool isOverflow, ref long usedx, ref long usedy)
        {
            //StringBuilder debug = new StringBuilder();
            decimal sfBase;

            //set initial parameters


            long pLaufzeit = age + duration;
            decimal ym = 12M;
            long pKilometer = (long)((mileage + kilometer * duration / ym) / (pLaufzeit / ym));//(long)(mileage + kilometer / 12.0M * duration);
            //debug.AppendLine("Params: Kilometer: " + pKilometer + " Laufzeit: " + pLaufzeit);

            usedx = pLaufzeit;
            usedy = pKilometer;
            //get overflow default value
            decimal ofValue = QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_sysQuoteOb);
            isOverflow = false;

            //debug.AppendLine("KM-Overflow Default%: " + ofValue);
            VGDao vg = new VGDao(_context);
            vg.deliverVGBoundaries(sysVg, perDatum);
            if (pLaufzeit < vg.xmin)
                pLaufzeit = (long)vg.xmin;
            if (pKilometer < vg.ymin)
                pKilometer = (long)vg.ymin;

            if (pLaufzeit > vg.xmax || pKilometer>vg.ymax)
            {
                sfBase = ofValue;
                isOverflow = true;
            }
            else
            {
                //get interpolated value dependend on brand
                sfBase = VGDao.deliverVGValue(_context, sysVg, perDatum, pLaufzeit.ToString(), pKilometer.ToString(), 1);
                //debug.AppendLine("SF-Base: " + sfBase);
            }
            


            return sfBase;
        }

        /// <summary>
        /// Calculates the ballon-rate based on the discountable value of the vehicle and the age
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="age"></param>
        /// <param name="perDatum"></param>
        /// <param name="pBetrag"></param>
        /// <returns></returns>
        private decimal MydeliverBallon(long duration, long age, DateTime perDatum, decimal pBetrag)
        {
            decimal rval;
            long pLaufzeit = age + duration;
            
            
            //get overflow default value
            decimal ofValue = QUOTEDao.deliverQuotePercentValueByName(QUOTEDao.QUOTE_sysQuoteOb);
            isOverflow = false;

            
            VGDao vg = new VGDao(_context);
            long sysVg = _context.ExecuteStoreQuery<long>("select sysvg from vg where mappingext='"+_CnstBALLON+"'", null).FirstOrDefault();
            vg.deliverVGBoundaries(sysVg, perDatum);
            if (pLaufzeit < vg.xmin)
                pLaufzeit = (long)vg.xmin;
            if (pBetrag < vg.ymin)
                pBetrag = (long)vg.ymin;

            if (pLaufzeit > vg.xmax || pBetrag > vg.ymax)
            {
                rval = ofValue;
                isOverflow = true;
            }
            else
            {
                //get interpolated value dependend on brand
                rval = VGDao.deliverVGValue(_context, sysVg, perDatum, pLaufzeit.ToString(), ((long)pBetrag).ToString(), 1);
                
            }



            return rval;
        }
        #endregion







    }
}
