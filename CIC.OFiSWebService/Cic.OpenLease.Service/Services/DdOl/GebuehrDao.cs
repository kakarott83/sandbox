using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Logging;
using System;
using System.Reflection;

namespace Cic.OpenLease.Service
{
    public class GebuehrInfo
    {
        public long sysgebuehr { get; set; }
        public String code { get; set; }
        public decimal gebuehr { get; set; }
    }
    public class GebuehrDao
    {
        private static CacheDictionary<String, GebuehrInfo> gebuehrCache = CacheFactory<String, GebuehrInfo>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        
        #region Private variables
        private const string CnstGebuhrenParam = "Bearbeitungsgebühr Vertrag";
        
        private DdOlExtended _context;
       // private PRParamDao prParamDao;
        
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        
        public GebuehrDao()
        {
        }
        public GebuehrDao(DdOlExtended context)
        {
            _context = context;
           // prParamDao = new PRParamDao(_context);
            rebuild();
            

        }
        private void rebuild()
        {
           /* if (!gebuehrCache.ContainsKey(PRParamDao.CnstBearbeitungsgebuehrCode))
            {
                List<GebuehrInfo> gebs = _context.ExecuteStoreQuery<GebuehrInfo>("select gebuehr,sysgebuehr,code from gebuehr", null).ToList();
                foreach (GebuehrInfo geb in gebs)
                {
                    gebuehrCache[geb.code] = geb;
                }

            }
            Percent = gebuehrCache[PRParamDao.CnstBearbeitungsgebuehrCode].gebuehr;*/
        }
        public GebuehrInfo getGebuehr(String code)
        {
            rebuild();
            return gebuehrCache[code];
        }
        public GebuhrenDto calcGebuehr(decimal finanzierungssummeBrutto, long sysPrProdukt, long sysObTyp, long sysObArt, decimal nachlass, long sysPerole, long sysBrand, bool noProvision)
        {
            GebuhrenDto gebuhrenDto = new GebuhrenDto();
            if (_context == null) return gebuhrenDto;


            //HCE no gebühr
            if(true) return gebuhrenDto;
/*
            decimal TaxRate = 0;
            PrismaDao pd = new CachedPrismaDao();

            long sysvart = pd.getVertragsart(sysPrProdukt).SYSVART;// PRPRODUCTHelper.DeliverSYSVART(_context, sysPrProdukt);

            TaxRate = LsAddHelper.GetTaxRate(_context, sysvart);
            _Log.Debug("Calc Gebühr for Produkt: " + sysPrProdukt + " / " + TaxRate);

            decimal finanzierungssummeNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(finanzierungssummeBrutto, TaxRate);

            PRPARAMDto param = prParamDao.DeliverPrParam(sysPrProdukt, sysObTyp, sysPerole, sysBrand, PRParamDao.CnstBearbeitungsgebuehrCode, sysObArt);
            decimal Gebuehr = CalculateBearbeitungsgebuehr(finanzierungssummeNetto, param);

            _Log.Debug("Calc Bearbeitungsgebühr with PRPARAM " + _Log.dumpObject(param) + " for Gebühr " + Gebuehr);
            Gebuehr = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(Gebuehr, TaxRate);
            

            gebuhrenDto.Gebuhren_Default =  Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Gebuehr);
            

            gebuhrenDto.Provision = 0;
            decimal sumnachlass = 0;
            

            //Subvention Gebühr--------------------
            Subvention sub = new Subvention(_context);

            long sysgebuehr = gebuehrCache[PRParamDao.CnstBearbeitungsgebuehrCode].sysgebuehr;
 * */
            /*var Query = from gebuehr in _context.GEBUEHRAlias
                        where gebuehr.CODE == KalkulationHelper.CnstBearbeitungsgebuehrCode
                        select gebuehr.SYSGEBUEHR;
            long sysgebuehr = Query.FirstOrDefault();*/
         /*   decimal subvention = Gebuehr - sub.deliverSubvention(Gebuehr, sysPrProdukt, Subvention.CnstAREA_CHARGE, sysgebuehr, 1, TaxRate);
            Gebuehr -= subvention;
            sumnachlass += subvention;
            _Log.Info("Subvention für Gebühr: " + subvention);
            //Ende Subvention-------------------------------------


            gebuhrenDto.GebuhrenBrutto = Gebuehr;
            if (nachlass > gebuhrenDto.GebuhrenBrutto)
                nachlass = gebuhrenDto.GebuhrenBrutto;

            PRPOUVOIR bearb_pouv = prParamDao.DeliverPrPouvoir(sysPrProdukt, sysObTyp, sysPerole, sysBrand, PRParamDao.CnstBearbGebNachlassFieldMeta, sysObArt);

            if (bearb_pouv != null)
            {
                if (bearb_pouv.TYP == 1)
                {
                    if (bearb_pouv.ADJMINP.HasValue)
                    {
                        decimal min = (Gebuehr * bearb_pouv.ADJMINP.Value / 100);
                        if (nachlass < min)
                            nachlass = min;

                    }
                    if (bearb_pouv.ADJMAXP.HasValue)
                    {
                        decimal max = (Gebuehr * bearb_pouv.ADJMAXP.Value / 100);
                        if (nachlass > max)
                            nachlass = max;
                    }

                }

            }
            else nachlass = 0;


            gebuhrenDto.GebuhrenBrutto -= nachlass;
            sumnachlass += nachlass;

            gebuhrenDto.GebuhrenBrutto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(gebuhrenDto.GebuhrenBrutto);
            gebuhrenDto.Gebuhren = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(gebuhrenDto.GebuhrenBrutto, TaxRate));


            gebuhrenDto.GebuhrenUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(gebuhrenDto.GebuhrenBrutto, TaxRate));
            gebuhrenDto.Subvention = subvention;
            gebuhrenDto.Provision = 0;

            if (sumnachlass == 0)
            {
                // Get the provision
                ProvisionDto Provision = new ProvisionDto();
                Provision.sysBrand = sysBrand;
                Provision.sysPerole = sysPerole;
                Provision.rank = (int)ProvisionTypeConstants.Bearbeitungsgebuehr;
                Provision.bearbeitungsgebuehr = gebuhrenDto.Gebuhren;
                Provision.finanzierungssumme = finanzierungssummeNetto;
                Provision.prparam = param;
               
                Provision.sysprproduct = sysPrProdukt;
                Provision.sysobtyp = sysObTyp;
                Provision.noProvision = noProvision;
                PROVDao dao = new PROVDao(_context);
                Provision = dao.DeliverProvision(Provision);
                gebuhrenDto.Provision = Provision.provision;
            }

            

           



            return gebuhrenDto;*/
        }

        /// <summary>
        /// DEPRECATED
        /// </summary>
        /// <param name="finanzierungssummeNetto"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public decimal CalculateBearbeitungsgebuehr(decimal finanzierungssummeNetto, PRPARAMDto param)
        {
            decimal Percent = 0;
            //Calculate result
            decimal Gebuehr = (Percent * finanzierungssummeNetto) / 100;

            if (param == null)
            {
                _Log.Error("No Prisma Parameter for " + PRParamDao.CnstBearbeitungsgebuehrCode + " found!");
                return Gebuehr;
            }

            //Change value depending on max value
            if (Gebuehr > param.MAXVALN)
            {
                Gebuehr = (decimal)param.MAXVALN;
            }

            //Change value depending on min value
            else if (Gebuehr < param.MINVALN)
            {
                Gebuehr = (decimal)param.MINVALN;
            }
            return Gebuehr;
        }
    }
}