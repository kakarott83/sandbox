using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenLease.Model.DdOl;
using Cic.OpenLease.Service;
using Cic.OpenOne.Common.DAO;
using Cic.One.GateWKT.DTO;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.OpenLease.ServiceAccess;
using Cic.One.Utils.DTO;
using Cic.OpenLease.Service.Versicherung;
using Cic.One.DTO;

namespace Cic.One.GateWKT.BO
{
    /// <summary>
    /// AlphaOne Calculation-BO for calculating, Rate, RW, Nova
    /// </summary>
    public class CalculationBO
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Berechnet Rate, NovaBM, RW for WKT
        /// Schritte:
        /// 1) novabonusmalus berechnen
        /// 2) bg = bgintern+provision+novabonusmalus
        /// 3) rw-p-berechnung mit sarvprozent-angabe
        /// 4) gesamt-rw-prozent = sap+rwzuabp, ausser crvp>, dann crvp
        /// 5) rate berechnen
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <param name="icalcRate"></param>
        /// <returns></returns>
        public void calculateRate(long syswfuser, icalcRateDto input, ocalcRateDto rval)
        {
            try
            {

                double crvcorrection = 0;
                double sfbaseP = 0;
                double saProzent = 0;
                double bg = 0;
                double rwbase = 0;
                int kalktypmodus = 0;
                bool iscredit = false;

                //RW-Tabelle und CRV-Korrektur:
                using (OlExtendedEntities dbctx = new OlExtendedEntities())
                {

                    //1) novabonusmalus berechnen
                    bool pkw = !HaftpflichtCalculator.isMotorrad(input.sysobtyp, dbctx);
                    if (pkw)
                    {
                        rval.novabonusmalus = calcNovaBonusMalus(input.krafststoff, input.co2, input.nox, input.particleCount, input.perDate, input.actuation);
                    }
                    else
                    {
                        rval.novabonusmalus = 0;
                    }
                    //Nova = (paksum + rabattsum + sonzubsum) * (getNovaProzentsatz() / 100.0) + getNovaSonderminderung();
                    rval.nova = input.bgnova * input.novap / 100.0;

                    rval.novasonderminderung = 0;
                    if (pkw)
                    {
                        rval.novasonderminderung = (double)NoVA.calculateSonderminderung(input.perDate, (FuelTypeConstants)input.krafststoff, input.actuation == 4);
                        rval.nova += rval.novasonderminderung;
                    }
                    if (rval.nova < 0)
                    {
                        rval.nova = 0;
                    }
                    //set bg (includes nova)
                    rval.novasteuervorteil = rval.nova / 1.2 * 0.2;
                    input.bgintern = input.bginternexklNova + rval.nova - rval.novasteuervorteil;

                    rwbase = getRwbase(input.grund, input.sarv, input.novap, rval.novabonusmalus);// r+val.novasonderminderung

                    //2) bg = bgintern+provision+novabonusmalus
                    bg = input.bgintern + input.provision + rval.novabonusmalus;




                    //get zins
                    if (input.zins == 0 && input.syszinstab > 0)
                    {
                        input.zins = dbctx.ExecuteStoreQuery<double>("select finzins from zinsdate where syszinstab=" + input.syszinstab + " and datum<=sysdate order by datum desc", null).FirstOrDefault();
                    }

                    //AIDA-Reprogrammed in this BO
                    sfbaseP = getSFBase(dbctx, input, ref saProzent, ref crvcorrection);
                    rval.sonderAustP = saProzent;

                    kalktypmodus = dbctx.ExecuteStoreQuery<int>("select modus from kalktyp where syskalktyp=" + input.syskalktyp, null).FirstOrDefault();
                    int rangsl = dbctx.ExecuteStoreQuery<int>("select rangsl from kalktyp where syskalktyp=" + input.syskalktyp, null).FirstOrDefault();
                    if (rangsl == 200)
                        iscredit = true;
                }



                //4) gesamt-rw-prozent
                double calcBase = rwbase;// (input.grund + (input.sarv * rval.sonderAustP / 100));


                rval.sfbaseP = sfbaseP;
                rval.sfbase = calcBase * (sfbaseP) / 100;
                rval.crvP = sfbaseP + crvcorrection;
                rval.crv = calcBase * (rval.crvP) / 100;


                rval.rwP = rval.crvP;



                if (input.rw >= 0)//rw as input reference
                {
                    if (input.rw > calcBase)
                        input.rw = calcBase;
                    rval.rwP = input.rw * 100.0 / calcBase;
                    rval.rw = input.rw;
                    
                }
                else if (input.rwp >= 0)//rwp as input reference
                {

                    if (input.rwp >= 100)
                        input.rwp = 100;
                    rval.rw = (double)Math.Round(input.rwp / 100.0 * calcBase, 2);
                    rval.rwP = rval.rw * 100.0 / calcBase;
                   

                }
                else
                {
                    if (rval.sfbaseP + input.rwfzaufabp > rval.rwP)
                        rval.rwP = rval.sfbaseP + input.rwfzaufabp;

                    if (rval.rwP >= 100)
                        rval.rwP = 100;
                    rval.rw = calcBase * rval.rwP / 100.0;//kalk. RW Gesamt für Ratenberechnung
                }
                if (rval.rwP > 100)
                    rval.rwP = 100;

                CalculationMode calcmode = CalculationMode.Begin;
                if (kalktypmodus == 1)
                    calcmode = CalculationMode.Begin;
                else
                    calcmode = CalculationMode.End;

                //5) rate berechnen
                double zins = input.zins + input.zinsaufab;
                if (zins > 12)
                    zins = 12;
                if (zins < -10)
                    zins = -10;

                double rate = calculateRate(bg, 0.0d, input.lz, 0.0d, rval.rw, zins, 0.0d, 12, calcmode);
                rval.rate = calculateRateWithVorteil(input.depot, rate, input.depotZins);

                double rateNew = (double)rval.rate;

                rval.rgg = (double)RggBO.CalculateRgGeb((RgGebVersion)input.rggtyp, input.lz, zins, input.sonder, input.kaskoRate > 0, input.kaskoRate, input.hpRate > 0, input.hpRate, ref rateNew, 12, (input.bgintern + input.sonder), rval.rw, calcmode, input.mwst, bg, iscredit, input.vsrate, input.servicerate);
                rval.rgg = Math.Round(rval.rgg, 2);
                rval.rate = (double)Math.Round(rateNew, 2);
                rval.zins = (double)Math.Round(zins, 6);
                rval.rwP = (double)Math.Round(rval.rwP, 5);
                rval.rw = (double)Math.Round(rval.rw, 2);
                rval.crv = (double)Math.Round(rval.crv, 2);
                rval.nova = (double)Math.Round(rval.nova, 2);

                rval.crvP = (double)Math.Round(rval.crvP, 6);
                rval.sfbaseP = (double)Math.Round(rval.sfbaseP, 6);
                rval.sfbase = (double)Math.Round(rval.sfbase, 2);
            }
            catch (Exception e)
            {
                _log.Error("Rate calculation failed", e);
            }
        }

        /// <summary>
        /// CRV Aufschlag berechnen
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sysVg"></param>
        /// <param name="perDatum"></param>
        /// <returns></returns>
        private double getCRV(OlExtendedEntities context, long sysVg, DateTime perDatum)
        {
            List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvg", Value = sysVg });
            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "perDate", Value = perDatum });
            return context.ExecuteStoreQuery<double>("select vgadj.value from vgadjvalid,vgadjtrg,vgavg,vgadj   where VgAdjTrg.Name = 'CRV' and  VgAdjValid.NAME = 'CRV Korrektur'  and vgavg.sysvg=:sysvg and   vgadjvalid.sysvgadjvalid=vgadjtrg.sysvgadjvalid and vgavg.sysvgadjvalid=vgadjvalid.sysvgadjvalid and vgadj.sysvgadjtrg=vgadjtrg.sysvgadjtrg and vgadj.sysvgavg=vgavg.sysvgavg and (vgadjvalid.validfrom is null or vgadjvalid.validfrom<=:perDate) and  (vgadjvalid.validuntil is null or vgadjvalid.validuntil>=:perDate)", parameters.ToArray()).FirstOrDefault();
        }

        /// <summary>
        /// Delivers the interpolated SF Base residual value percentage
        /// </summary>
        /// <param name="sysVg"></param>
        /// <param name="mileage">KmStand</param>
        /// <param name="kilometer">KM/Jahr</param>
        /// <param name="duration">Laufzeit</param>
        /// <param name="age"></param>
        /// <param name="perDatum"></param>
        /// <param name="isOverflow"></param>
        /// <returns></returns>
        private double getSFBaseFromRwTab(long sysVg, long mileage, decimal kilometer, long duration, long age, DateTime perDatum, ref bool isOverflow)
        {
            //StringBuilder debug = new StringBuilder();
            double sfBase;

            //set initial parameters


            long pLaufzeit = age + duration;
            long pKilometer = (long)((mileage + kilometer * duration / 12.0M) / (pLaufzeit / 12.0M));
            //debug.AppendLine("Params: Kilometer: " + pKilometer + " Laufzeit: " + pLaufzeit);

            CachedQuoteDao qd = new CachedQuoteDao();
            //get overflow default value
            double ofValue = qd.getQuote(QUOTEDao.QUOTE_RWDEFAULTALPHABET);
            isOverflow = false;

            //debug.AppendLine("KM-Overflow Default%: " + ofValue);
            CachedVGDao vgd = new CachedVGDao();
            try
            {
                VGBoundaries vg = vgd.getVGBoundaries(sysVg, perDatum);
                if (pLaufzeit < vg.xmin)
                    pLaufzeit = (long)vg.xmin;
                if (pKilometer < vg.ymin)
                    pKilometer = (long)vg.ymin;

                if (pLaufzeit > vg.xmax || pKilometer > vg.ymax)
                {
                    sfBase = ofValue;
                    isOverflow = true;
                }
                else
                {
                    //get interpolated value dependend on brand
                    sfBase = vgd.getVGValue(sysVg, perDatum, pLaufzeit.ToString(), pKilometer.ToString(), VGInterpolationMode.LINEAR);
                    //debug.AppendLine("SF-Base: " + sfBase);
                }

            }
            catch (Exception)
            {
                sfBase = ofValue;
                isOverflow = true;
                _log.Error("No SF-Base Value found for sysVg: " + sysVg);
            }
            return sfBase;
        }

        /// <summary>
        /// SF-Base % Ermittlung
        /// </summary>
        /// <param name="context"></param>
        /// <param name="input"></param>
        /// <param name="saProzent"></param>
        /// <param name="crvcorrection"></param>
        /// <returns></returns>
        private double getSFBase(OlExtendedEntities context, icalcRateDto input, ref double saProzent, ref double crvcorrection)
        {

            /*
             Rechenvorgang sollte wie folgt sein: #2793

                Restwertvorschlag für das ausgewählte Fahrzeug ist 47,22% (Eurotax Code 160562)
                -> es wird die anrechenbare Sonderausstattung berechnet
                -> SA-Anteil im Verhältnis zum Listenpreis sind im Beispiel 15%
                -> lt. SA_KORR (Korrekturtabelle Sonderausstattung) werden bei 15% SA 50% angerechnet
                -> Berechnung des Restwertes der Sonderausstattung (47,22% von der angerechneten SA)
                -> Restwert Fahrzeug und Restwert Sonderausstattung werden addiert
                -> Restwert in EUR vorhanden
                -> Gesamtrestwert im Verhältnis zum Listenpreis inkl. Sonderausstattung ist dann der richtige Restwertvorschlag in %
            */
            //get correction value, model (eg middle class) specific, also get value group id
            try
            {
                if (input.sysobtyp == 0) return 0;
                CachedKorrekturDao kdao = new CachedKorrekturDao();
                Cic.OpenOne.Common.BO.KorrekturBo kh = new OpenOne.Common.BO.KorrekturBo(kdao);
                long ubnahmekm = input.kmstand;
                string op = "+";
                ADJDto vgParam = VGADJDao.deliverAIDAVGAdjValue(context, input.sysobtyp, input.perDate);
                /*
                 * 
                    select sysobtyp, sysvgrw from obtyp where schwacke='177894;
                    SELECT sysbrand into pSysBrand FROM (select sysbrand,importtable from obtyp   connect by PRIOR sysobtypp = sysobtyp start with sysobtyp=:OBTYP order by level desc ) WHERE importtable='ETGMAKE' ;
                    select vgadj.value,vgavg.sysvg into pValue, pSysVg from vgadjvalid,vgadjtrg,vgavg,vgadj,obtyp   where obtyp.sysobtyp=:pSysObtyp and vgavg.sysvg=obtyp.sysvgrw and  vgadjtrg.sysbrand = :pSysBrand and   vgadjvalid.sysvgadjvalid=vgadjtrg.sysvgadjvalid and vgavg.sysvgadjvalid=vgadjvalid.sysvgadjvalid and vgadj.sysvgadjtrg=vgadjtrg.sysvgadjtrg and vgadj.sysvgavg=vgavg.sysvgavg and (vgadjvalid.validfrom is null or vgadjvalid.validfrom<=pPerDatum) and  (vgadjvalid.validuntil is null or vgadjvalid.validuntil>=pPerDatum);
                 */

                double addBrandPercent = (double)vgParam.adjvalue;
                DateTime erstzul = input.erstZul;
                //RW-Tab Interpolation
                //calculate age in months since erstzul till perDate
                int age;
                if (erstzul.Year < 1801) erstzul = input.perDate;//avoid invalid age

                for (age = 0; erstzul.AddMonths(age + 1).CompareTo(input.perDate) <= 0; age++) ;

                bool isOverflow = false;
                double rwTabPercent = getSFBaseFromRwTab(vgParam.sysvg, ubnahmekm, input.ll, input.lz, age, input.perDate, ref isOverflow);

                //Product-Correction
                double addProductPercent = kh.Correct("RW_KORR_FIN", 0, op, input.perDate, input.syskalktyp.ToString(), "");
                if (isOverflow)//Ticket #3468
                {
                    addProductPercent = 0;
                    addBrandPercent = 0;
                }
                //SF-BASE Percent
                double sfBasePercent = rwTabPercent + addBrandPercent + addProductPercent;
                if (sfBasePercent < 0) sfBasePercent = 0;


                //CRV
                double crvPercent = sfBasePercent;

                crvcorrection = getCRV(context, vgParam.sysvg, input.perDate);
                crvPercent += crvcorrection;

                //SA-Correction
                //% SA zu Listenpreis Fahrzeug
                double sePercent = Math.Round((input.sarv / (input.grund) * 100), 1, MidpointRounding.AwayFromZero);

                saProzent = 100;

                //Anrechenbare SA für SA-Anteil
                saProzent = kh.Correct("SA_KORR", 0, op, input.perDate, (sePercent).ToString(), "");

                //-> Gesamtrestwert im Verhältnis zum Listenpreis inkl. Sonderausstattung ist dann der richtige Restwertvorschlag in %

                //Calculation of results
                return sfBasePercent;
                /*_crvprozent = crvPercent;

                calcBase = (grund + (sonzub * saRVPercent / 100));
                //http://80.146.230.161/otrs/index.pl?Action=AgentTicketZoom;TicketID=687#5556 # 16
                if (isOverflow)
                {
                    calcBase = kpnetto;
                    sonzub = 0;
                    grund = calcBase;

                    _listenpreis = kpnetto;
                    bonusmalusexklzuschlag = 0;
                    nova = 0;
                }

                _sfBase = calcBase * (sfBasePercent) / 100;
                _crv = calcBase * (crvPercent) / 100;

                //Gesamtrestwert im Verhältnis zum Listenpreis inkl. Sonderausstattung ist dann der richtige Restwertvorschlag in %
                _crvprozentLP = Math.Round(_crv / (grund + sonzub) * 100, 2);

                _sfBaseprozentLP = Math.Round(_sfBase / (grund + sonzub) * 100, 2);

                //Round it:
                _crv = getRestwertNetto(_crvprozentLP).netto;

                _sfBase = getRestwertNetto(_sfBaseprozentLP).netto;

                _Log.Debug("SF-Base: " + _sfBase + " CRV: " + _crv + " SAProzent: " + saRVPercent);*/
            }
            catch (System.InvalidOperationException ioe)
            {
                _log.Error("RV-Calculation", ioe);
                // TODO Exceptionhandling with Resources
                throw new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.GeneralSelect, "Can't calculate RV, invalid SYSOBTYP or perDate.");
            }


        }

        /// <summary>
        /// Calculates the residual value base
        /// </summary>
        /// <param name="grund"></param>
        /// <param name="sarv"></param>
        /// <param name="novap"></param>
        /// <returns></returns>
        private static double getRwbase(double grund, double sarv, double novap, double novaminderung)
        {
            double novabetrag = (grund + sarv) * novap / 100;
            novabetrag += novaminderung;
            if (novabetrag < 0) novabetrag = 0;
            return grund + sarv + novabetrag;
        }

        /// <summary>
        /// Calculates the interest rate
        /// </summary>
        /// <param name="aquisitionCost"></param>
        /// <param name="firstPayment"></param>
        /// <param name="term"></param>
        /// <param name="rate"></param>
        /// <param name="residualValue"></param>
        /// <param name="interest"></param>
        /// <param name="zinsEff"></param>
        /// <param name="ppy"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static double calculateRate(double aquisitionCost, double firstPayment, int term, double rate, double residualValue, double interest, double zinsEff, int ppy, Cic.One.Utils.DTO.CalculationMode mode)
        {
            if (interest == 0)
            {
                if (aquisitionCost - residualValue == 0)
                {
                    rate = 0;
                    return 0;
                }
                rate = (aquisitionCost - residualValue) / term;
                return rate;
            }


            bool zahlmodus = true;
            if (mode == Cic.One.Utils.DTO.CalculationMode.End)
                zahlmodus = false;
            rate = Cic.One.Utils.BO.Kalkulator.calcRATE((aquisitionCost - firstPayment), (double)interest / 12.0, term, residualValue, zahlmodus);
            return rate;
        }

        /// <summary>
        /// Calculates the interest rate, taking depot into account
        /// </summary>
        /// <param name="depot"></param>
        /// <param name="rateBrutto"></param>
        /// <param name="depotZins"></param>
        /// <returns></returns>
        private static double calculateRateWithVorteil(double depot, double rateBrutto, double depotZins)
        {
            double JahreZins;
            double Vorteil = 0;

            if (depotZins != 0)
            {
                JahreZins = (depotZins / 100) / 12;
                Vorteil = (depot * JahreZins);
            }

            return rateBrutto - Vorteil;
        }

        /// <summary>
        /// calculates the nova bonus/malus
        /// </summary>
        /// <param name="krafststoff"></param>
        /// <param name="co2"></param>
        /// <param name="nox"></param>
        /// <param name="particleCount"></param>
        /// <param name="perDate"></param>
        /// <param name="actuation"></param>
        /// <returns></returns>
        private static double calcNovaBonusMalus(int krafststoff, double co2, double nox, double particleCount, DateTime perDate, int actuation)
        {
            using (OlExtendedEntities ctx = new OlExtendedEntities())
            {
                NoVA nv = new NoVA(ctx);
                String kraftstoffcode = NoVA.getKraftstoff((FuelTypeConstants)krafststoff, actuation == 4);
                return (double)nv.calculateNovaBonusMalus(kraftstoffcode, (decimal)co2, (decimal)nox, (decimal)particleCount, perDate);
            }
        }
    }
}