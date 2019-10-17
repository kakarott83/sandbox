using Cic.OpenLease.Service.Provision;
using Cic.OpenLease.Service.Services.DdOl;
using Cic.OpenLease.ServiceAccess;
using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Collection;
using CIC.Database.OL.EF6.Model;
using System;
using System.Linq;

namespace Cic.OpenLease.Service.Versicherung
{
    /// <summary>
    /// Calculates the BMW AIDA Haftpflicht Insurance
    /// SYSKORRTYP1 - Prämienstufe, param1: Prämienstufe
    /// SYSKORRTYP2 - Motorsteuer, param1: Zahlmodus, param2: Fahrzeugart
    /// SYSVG - Prämie über Deckungssumme/KW
    /// </summary>
    [System.CLSCompliant(true)]
    public class HaftpflichtCalculator : AbstractVSCalculator
    {
        #region Constants
        private static String MOTORRAD = "40";
        #endregion
        
        private static CacheDictionary<long, bool> motorradCache = CacheFactory<long, bool>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);

        /// <summary>
        /// Returns true if the vehicle is purely electric
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool isElectric(long sysobtyp, DdOlExtended context)
        {
             NoVA nv = new NoVA(context);
             FuelTypeConstants ft = nv.getAntriebsart(sysobtyp);
             return ft == FuelTypeConstants.Electricity;
        }

        /// <summary>
        /// returns true if the vehicle is a motorcycle
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool isMotorrad(long sysobtyp, DdOlExtended context)
        {
            if (!motorradCache.ContainsKey(sysobtyp))
            {
                //Fahrzeugart
                bool bike = false;

                string query = "select aklasse from OBTYP where sysobtyp=" + sysobtyp;
                String aKlasse = context.ExecuteStoreQuery<String>(query).FirstOrDefault();

                //var q = from o in context.OBTYP where o.SYSOBTYP == param.sysObTyp select o;
                //OBTYP ob = q.FirstOrDefault();
                if (MOTORRAD.Equals(aKlasse))
                    bike = true;

                motorradCache[sysobtyp] = bike;
            }
            return motorradCache[sysobtyp];
        }

        override
        public ServiceAccess.DdOl.InsuranceResultDto calculate(DdOlExtended context, long sysPerole, long sysBrand, VSTYP vstyp, InsuranceParameterDto param)
        {
            bool pkw = !isMotorrad(param.sysObTyp, context);


            string fahrzeugart = "PKW";
            if (!pkw) fahrzeugart = "BIKE";

            //Prämienstufe - SYSKORRTYP1
            if (vstyp.SYSKORRTYP1 == null && pkw)
            {
                _Log.Error("Haftpflicht-Input: "+_Log.dumpObject(param));
                throw new NullReferenceException("Prämienstufe (SYSKORRTYP1) in Haftpflicht-VSTYP " + vstyp.SYSVSTYP + "/OBTYP:" + param.sysObTyp + " nicht konfiguriert.");
            }
            //Motorsteuer - SYSKORRTYP2
            if (vstyp.SYSKORRTYP2 == null)
            {
                _Log.Error("Haftpflicht-Input: " + _Log.dumpObject(param));
                
                throw new NullReferenceException("Motorsteuer (SYSKORRTYP2) in Haftpflicht-VSTYP " + vstyp.SYSVSTYP + "/OBTYP:" + param.sysObTyp + " nicht konfiguriert.");
            }

            InsuranceResultDto rval = new InsuranceResultDto();
            int tabIdx = (int)param.Zahlmodus - 1;
            if (tabIdx > 3) tabIdx = 3;

            KORREKTURDao korr = new KORREKTURDao(context);
            string op = "+";



            //Prämie über Deckungssumme/KW
            long sysvg = vstyp.SYSVG.GetValueOrDefault();
            if (sysvg == 0)
                throw new NullReferenceException("Wertegruppe (SYSVG) in Haftpflicht-VSTYP " + vstyp.SYSVSTYP + " nicht konfiguriert.");

            VGDao vg = new VGDao(context);

            vg.deliverVGBoundaries(sysvg, DateTime.Now);
            decimal p1 = param.KW;
            int polmode = VGDao.CnstINTERPOLATION_MAXIMUM;
            if (!pkw)
                p1 = param.Hubraum;
            if (p1 < vg.xmin)
            {
                p1 = vg.xmin;
                polmode = VGDao.CnstINTERPOLATION_MINIMUM;
            }
            if (p1 > vg.xmax)
                p1 = vg.xmax;

            decimal p2 = (decimal)vstyp.MAXVSL;
            if (p2 < vg.ymin)
                p2 = vg.ymin;
            if (p2 > vg.ymax)
                p2 = vg.ymax;

            DateTime perDatum = DateTime.Now;
            if (param.lieferdatum != null && param.lieferdatum.HasValue)
                perDatum = param.lieferdatum.Value;
            if (perDatum.CompareTo(new DateTime(2014, 3, 1)) < 0)
                perDatum = new DateTime(2014, 3, 1);

            decimal praemieMonatlich = 0;
            try
            {
                praemieMonatlich = vg.deliverVGValue(sysvg, perDatum, decimal.Floor(p1).ToString(), decimal.Floor(p2).ToString(), polmode);
            }
            catch (Exception ie)
            {
                throw new InvalidOperationException("Prämientabelle in Haftpflicht-VSTYP " + vstyp.SYSVSTYP + " nicht vollständig konfiguriert", ie);
            }


            decimal grundstufeProzent = 100;
            if (pkw)
                grundstufeProzent = korr.Correct((long)vstyp.SYSKORRTYP1, 0, op, perDatum, param.Praemienstufe.ToString(), "");

            //Versicherungssteuer
            decimal versicherungsSteuerProzent = MyDeliverSteuer(context);
            rval.Motorsteuer = 0;
            if (pkw && !isElectric(param.sysObTyp,context))
            {
                rval.Motorsteuer = korr.Correct((long)vstyp.SYSKORRTYP2, 0, op, perDatum, param.KW.ToString(), fahrzeugart);
            }
            else if(!pkw)
            {
                rval.Motorsteuer = korr.Correct((long)vstyp.SYSKORRTYP2, 0, op, perDatum, param.Hubraum.ToString(), fahrzeugart);
            }

            //Default
            rval.Netto = praemieMonatlich * grundstufeProzent / 100 - (0 * (12 / param.Zahlmodus));
            rval.Versicherungssteuer = rval.Netto * versicherungsSteuerProzent / 100;
            rval.Praemie_Default = rval.Netto + rval.Versicherungssteuer + rval.Motorsteuer;

            MySetSubventionNachlass(context, param, rval, versicherungsSteuerProzent);


            // Provisionsberechnung
            try
            {
                rval.Provision = 0;

                rval.Provision = MyDeliverProvision(context, (int)ProvisionTypeConstants.Haftpflicht, sysBrand, sysPerole, rval.Netto * param.Zahlmodus, rval.Praemie * param.Zahlmodus, param.sysPrProduct, param.sysObTyp, param.calcProvision, vstyp.SYSVSTYP);
            }
            catch (Exception ie)
            {
                throw new InvalidOperationException("Haftpflicht-Provision (" + ProvisionTypeConstants.Haftpflicht + ") nicht konfiguriert", ie);
            }


            return rval;
        }
    }
}