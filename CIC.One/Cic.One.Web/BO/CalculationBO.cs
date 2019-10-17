using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.One.DTO;
using Cic.One.Utils.BO;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DTO.Prisma;

namespace Cic.One.Web.BO
{
    public class CalculationBO : ICalculationBo
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static CacheDictionary<String, long> vgZinsCache = CacheFactory<String, long>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static CacheDictionary<String, long> vgMargeCache = CacheFactory<String, long>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);

        /// <summary>
        /// Solves the angebot/antrag kalkulation with the given settings
        /// </summary>
        /// <param name="input"></param>
        /// <param name="rval"></param>
        public void solveKalkulation(isolveKalkulationDto input, osolveKalkulationDto rval)
        {
            KalkbaseDto dto = null;
            if(input.angkalk != null)
                dto = input.angkalk;
            if(input.antkalk != null)
                dto = input.antkalk;
            
            double ahk = dto.bgextern - dto.rabatto;
            ahk = (1 + dto.ustzins / 100) * ahk;//brutto
            double finbetrag = ahk - dto.szbrutto + dto.provision;
            double bw = finbetrag - dto.subventiono + dto.gebuehrbrutto; //finbetrag - händlersubvention + gebühr
            bw += dto.marge;//marge has to be zero, than it will be used from marge vg below!


            if(dto.rwkalkbruttop<0 && input.sysobtyp>0)//autodetect rw percent
            {
                using (PrismaExtended ctx = new PrismaExtended())
                {
                    long sysvgrw = ctx.ExecuteStoreQuery<long>("select sysvgrw from obtyp where sysobtyp=" + input.sysobtyp, null).FirstOrDefault();
                    if (sysvgrw > 0)
                    {
                        long age = input.age;
                        double pLaufzeit = age + dto.lz.Value;
                        long kmstand = input.kmstand;
                        double ym = 12.0;
                        double pKilometer = (long)((kmstand + dto.ll.Value * dto.lz.Value / ym) / (pLaufzeit / ym));
                        dto.rwkalkbruttop = fetchVgValue(sysvgrw, ref pLaufzeit, ref pKilometer, dto.valutaa.Value);
                    }
                    else
                    {
                        dto.rwkalkbruttop = 10;//Default-Fallback
                    }
                    dto.rwkalkbrutto = ahk / 100.0 * dto.rwkalkbruttop;
                    dto.rwkalk = dto.rwkalkbrutto / (1 + dto.ustzins / 100);
                }
                
            }



            bool hasRapZins = false;
            //RAP-ZINS, falls vorhanden
            if (input.score != null && input.score.Length > 0 && dto.sysprproduct > 0)
            {
                try
                {
                    Convert.ToDouble(input.score);
                    IPrismaProductBo productBo = PrismaBoFactory.getInstance().createPrismaProductBo(PrismaProductBo.CONDITIONS_BANKNOW, input.isoLanguageCode);
                    PRPRODUCT prprod = productBo.getProduct(dto.sysprproduct);

                    if (prprod != null && prprod.SYSPRRAP != null && prprod.SYSPRRAP.HasValue)
                    {
                        IZinsBo zinsBo = Cic.OpenOne.Common.BO.CommonBOFactory.getInstance().createZinsBo(ZinsBo.CONDITIONS_BANKNOW, input.isoLanguageCode);
                        prKontextDto prCtx = new prKontextDto();
                        prCtx.perDate = DateTime.Now;
                        prCtx.sysprproduct = dto.sysprproduct;
                        double[] zinsen = zinsBo.getRAPZins(dto.sysprproduct, input.score, dto.lz.Value, bw, prCtx);
                        dto.zins = zinsen[0];
                        hasRapZins = true;
                    }
                }
                catch (Exception ex)
                {
                    //error with score - no number
                }
            }

            //bw = dto.bgextern - dto.rabatto - dto.szbrutto + dto.provision - dto.subventiono + dto.gebuehrbrutto+dto.marge
            if (!hasRapZins && (input.zinsVGCode != null || input.margeVGCode != null))
            {

                using (PrismaExtended ctx = new PrismaExtended())
                {
                    long sysvgzins = 0;
                    long sysvgmarge = 0;

                    if (input.zinsVGCode!=null && !vgZinsCache.ContainsKey(input.zinsVGCode))
                    {
                        List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "vgcode", Value = input.zinsVGCode });
                        sysvgzins =
                            ctx.ExecuteStoreQuery<long>("SELECT sysvg from vg where mappingext=:vgcode", pars.ToArray())
                                .FirstOrDefault();
                        vgZinsCache[input.zinsVGCode] = sysvgzins;
                    }
                    if(vgZinsCache.ContainsKey(input.zinsVGCode))
                        sysvgzins = vgZinsCache[input.zinsVGCode];
                    if (input.margeVGCode!=null && !vgZinsCache.ContainsKey(input.margeVGCode))
                    {
                        List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "vgcode", Value = input.margeVGCode });
                        sysvgmarge =
                            ctx.ExecuteStoreQuery<long>("SELECT sysvg from vg where mappingext=:vgcode", pars.ToArray())
                                .FirstOrDefault();
                        vgMargeCache[input.margeVGCode] = sysvgmarge;
                    }
                    if (vgMargeCache.ContainsKey(input.margeVGCode))
                        sysvgmarge = vgMargeCache[input.margeVGCode];

                    if (sysvgzins > 0 && dto.zins == 0)
                    {
                        double lz = dto.lz.Value;
                        dto.zins = fetchVgValue(sysvgzins, ref lz, ref ahk, dto.valutaa.Value);
                    }
                    if (sysvgmarge > 0 && dto.marge==0)//use automarge if given marge is zero
                    {
                        double lz = dto.lz.Value;
                        double marge = fetchVgValue(sysvgmarge, ref lz, ref ahk, dto.valutaa.Value);
                        dto.marge = marge / 100.0 * ahk;
                        bw += dto.marge;
                    }
                    
                }
            }

            //define the calculator parameter set - BRUTTO Calculation here!
            CalculatorFacade cf = new CalculatorFacade();
            cf.barwert = bw;
            cf.valuta = dto.valutaa.Value;
            cf.beginn = dto.beginn.Value;
            cf.zins = dto.zins;
            cf.laufzeit = dto.lz.Value;
            cf.zahlweise = 12 / dto.ppy.Value;
            cf.letzteRate = dto.rwkalkbrutto;
            if (cf.letzteRate > 0)
                cf.laufzeit++;

            cf.includeStart = false;//this has impact on the calculated zinsdays between valuta and begin
            cf.includeEnd = true;//this has impact on the calculated zinsdays between valuta and begin
            cf.zahlmodus = dto.modus == 0 ? Kalkulator.ZAHLMODUS_VORSCHUESSIG : Kalkulator.ZAHLMODUS_NACHSCHUESSIG;
            List<double> calcraten = new List<double>(input.raten);
            if (dto.rwkalkbrutto > 0)//raten-input array wont include rw!
            {
                calcraten.Add(dto.rwkalkbrutto);
            }
            cf.setRaten(calcraten);

            StaffelKalkulator kalk = new StaffelKalkulator(cf);
            kalk.roundType = RoundType.CHF;
            kalk.calcRange = input.range;
            

           

            KalkbaseDto rvalkalk = dto;
            rvalkalk.bgexternbrutto = cf.barwert;

            if(dto is AngkalkDto)
                rval.angkalk = (AngkalkDto)dto;
            if (dto is AntkalkDto)
                rval.antkalk = (AntkalkDto)dto;

            switch (input.command)
            {
                case (CalculationCommand.CALC_HAENDLERBET):
                    {
                        rvalkalk.bgexternbrutto = kalk.calcBARWERT();
                        rvalkalk.bgextern = rvalkalk.bgexternbrutto / (1 + dto.ustzins / 100);
                        cf.barwert = rvalkalk.bgexternbrutto;
                        rval.raten = kalk.raten;
                        //bw = dto.bgextern - dto.rabatto - dto.szbrutto + dto.provision - dto.subventiono + dto.gebuehrbrutto+dto.marge
                        dto.subventiono = ahk - dto.szbrutto + dto.provision + dto.gebuehrbrutto + dto.marge - rvalkalk.bgexternbrutto;
                        break;
                    }
                case (CalculationCommand.CALC_PROVISION):
                    {
                        rvalkalk.bgexternbrutto = kalk.calcBARWERT();
                        rvalkalk.bgextern = rvalkalk.bgexternbrutto / (1 + dto.ustzins / 100);
                        cf.barwert = rvalkalk.bgexternbrutto;
                        rval.raten = kalk.raten;
                        //bw = dto.bgextern - dto.rabatto - dto.szbrutto + dto.provision - dto.subventiono + dto.gebuehrbrutto+dto.marge
                        dto.provision = rvalkalk.bgexternbrutto - ahk + dto.szbrutto +dto.subventiono - dto.gebuehrbrutto-dto.marge;
                        break;
                    }
                case (CalculationCommand.CALC_RABATTOFFEN):
                    {
                        rvalkalk.bgexternbrutto = kalk.calcBARWERT();
                        rvalkalk.bgextern = rvalkalk.bgexternbrutto / (1 + dto.ustzins / 100);
                        cf.barwert = rvalkalk.bgexternbrutto;
                        rval.raten = kalk.raten;
                        //bw = dto.bgextern - dto.rabatto - dto.szbrutto + dto.provision - dto.subventiono + dto.gebuehrbrutto+dto.marge
                        dto.rabatto = dto.bgextern - dto.szbrutto + dto.provision - dto.subventiono + dto.gebuehrbrutto +dto.marge- rvalkalk.bgexternbrutto;
                        break;
                    }
                case (CalculationCommand.CALC_BWMARGE):
                    {
                        rvalkalk.bgexternbrutto = kalk.calcBARWERT();
                        rvalkalk.bgextern = rvalkalk.bgexternbrutto / (1 + dto.ustzins / 100);
                        cf.barwert = rvalkalk.bgexternbrutto;
                        rval.raten = kalk.raten;
                        //bw = dto.bgextern - dto.rabatto - dto.szbrutto + dto.provision - dto.subventiono + dto.gebuehrbrutto+dto.marge
                        dto.marge = rvalkalk.bgexternbrutto - ahk + dto.szbrutto - dto.provision + dto.subventiono - dto.gebuehrbrutto;
                        break;
                    }
                case (CalculationCommand.CALC_RATE):
                    {
                        kalk.calcRATEN(cf.barwert);
                        rval.raten = kalk.raten;
                        break;
                    }
                case (CalculationCommand.CALC_ZINS):
                    {
                        dto.zins = kalk.calcZINS(cf.barwert, 5);
                        rval.raten = kalk.raten;
                        break;
                    }
                case (CalculationCommand.CALC_BARWERT):
                    {
                        rvalkalk.bgexternbrutto = kalk.calcBARWERT();
                        rvalkalk.bgextern = rvalkalk.bgexternbrutto / (1 + dto.ustzins / 100);
                        cf.barwert = rvalkalk.bgexternbrutto;
                        rval.raten = kalk.raten;
                        break;
                    }
                case (CalculationCommand.CALC_RESTWERT)://raten are defined
                    {
                        
                        rvalkalk.rwkalkbrutto = kalk.calcENDWERT(cf.barwert);
                        rvalkalk.rwkalk = rvalkalk.rwkalkbrutto / (1 + dto.ustzins / 100);
                        rval.raten = kalk.raten;
                        break;
                    }
                case (CalculationCommand.CALC_SZ):
                    {
                        rvalkalk.bgexternbrutto = kalk.calcBARWERT();
                        dto.szbrutto = ahk - rvalkalk.bgexternbrutto + dto.provision - dto.subventiono + dto.gebuehrbrutto + dto.marge;
                        
                        rval.raten = kalk.raten;
                        break;
                    }
                case (CalculationCommand.CALC_SZMK)://calculates the szbrutto-part of szbrutto for MK 
                    {//(Die Ust./Vst wird sofort über die gesamte Summe fällig, so das die Mietzahlungen keine Ust. mehr auslösen. )
                        double diff = 10000;
                        int count = 0;
                        double lastUstSum = 0;
                        dto.szust = 0;
                        dto.szbrutto = dto.sz;//no ustzins for this
                        double szbruttoOrg = dto.szbrutto;
                        while (Math.Abs(diff) > 0.000001 && count < 10)
                        {
                            input.command = CalculationCommand.CALC_RATE;
                            solveKalkulation(input, rval);
                            double rsumbrutto = (from r in rval.raten
                                                 select r).Sum();

                            double rsumnetto = rsumbrutto / (1 + dto.ustzins / 100);
                            double ustsum = rsumbrutto - rsumnetto;
                            dto.szbrutto = szbruttoOrg - ustsum;
                            if (dto.szbrutto < 0)
                            {
                                dto.szbrutto = 0;
                            }
                            diff = ustsum - lastUstSum;
                            lastUstSum = ustsum;
                            count++;
                        }

                        dto.sz = dto.szbrutto;

                        break;
                    }
            }

            dto.sz = dto.szbrutto / (1 + dto.ustzins / 100);
            dto.gebuehr = dto.gebuehrbrutto / (1 + dto.ustzins / 100);
            rvalkalk.zinseff = CalculateEffectiveInterest(kalk.calcZINS(cf.barwert, 5), dto.ppy.Value);

        }

        /// <summary>
        /// From Antrag and Antkalk, create the solveKalkulation input
        /// </summary>
        /// <param name="antrag"></param>
        /// <param name="kalkulation"></param>
        /// <returns></returns>
        public isolveKalkulationDto createIsolveKalkulationFromAntrag(AntragDto antrag, AntkalkDto kalkulation)
        {
            isolveKalkulationDto ikalk = new isolveKalkulationDto();
            ikalk.antkalk = kalkulation;
            ikalk.command = CalculationCommand.CALC_RATE;
            ikalk.kmstand = antrag.antob.ubnahmekm;
            ikalk.sysobtyp = antrag.antob.sysObTyp;
            //ikalk.score = ;
            //ikalk.age = ...;
            //ikalk.zinsVGCode = ;
            //ikalk.margeVGCode=;
            ikalk.range = kalkulation.bezeichnung;

            ikalk.raten = (from z in kalkulation.zahlplan
                           select z.betrag).ToList();
            return ikalk;
        }

        /// <summary>
        /// Fetch the Value Table entry for the given value table at x/y for the given Date
        /// </summary>
        /// <param name="sysvg"></param>
        /// <param name="xval"></param>
        /// <param name="yval"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        private static double fetchVgValue(long sysvg, ref double xval, ref double yval, DateTime perDate)
        {
            Cic.OpenOne.Common.DAO.IVGDao vg = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getVGDao();
            //x=ahk, y=lz
            VGBoundaries bound = vg.getVGBoundaries(sysvg, perDate);
            if (xval < bound.xmin)
                xval = bound.xmin;
            if (xval > bound.xmax)
                xval = bound.xmax;
            if (yval < bound.ymin)
                yval = bound.ymin;
            if (yval > bound.ymax)
                yval = bound.ymax;
            return vg.getVGValue(sysvg, perDate, xval.ToString(), yval.ToString(), VGInterpolationMode.NONE);
        }
        /// <summary>
        /// get effective interest from nominal interest
        /// </summary>
        /// <param name="nominalInterest"></param>
        /// <param name="ppy"></param>
        /// <returns></returns>
        public static double CalculateEffectiveInterest(double nominalInterest, int ppy)
        {
            double Ppy = ppy;

            double d = System.Math.Pow((double)(1 + (nominalInterest / (100 * Ppy))), (double)Ppy);

            d = (d - 1) * 100;

            return d;
        }
    }
}
