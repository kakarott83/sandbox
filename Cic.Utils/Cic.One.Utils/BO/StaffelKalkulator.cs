using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.Utils.BO
{
   
    /// <summary>
    /// Staffelkalkulator,
    /// ermöglicht die Berechung von nicht-linearen Zahlplänen mit Ratenplan/Staffeln
    /// </summary>
    public class StaffelKalkulator : Kalkulator
    {
        public static int ZAHLWEISE_MONATLICH = 1;
        public static int ZAHLWEISE_VIERTELJAHR = 3;
        public static int ZAHLWEISE_HALBJAHR = 6;
        public static int ZAHLWEISE_JAHR = 12;
        

        private bool norounding = false;
        private long precision = 3;
        private double barwert, rate, zins, perioden, endwert, laufzeit;
        private bool zahlmodus;
        private int zahlweise;
        private CalcMethod method;

        public List<double> raten { get; set; }
        public DateTime perdate { get; set; }
        public DateTime tilgungsbeginn { get; set; }
        public RoundType roundType { get; set; }
        /// <summary>
        /// defines the range of raten to calc, only used for calcRATEN
        /// </summary>
        public String calcRange { get; set; }

        private double difftageFaktorExtern = 0;

        /// <summary>
        /// defines if startdate of datediff is also comletely used for interest calc
        /// </summary>
        public bool includeStart { get; set; }
        /// <summary>
        /// defines if enddate of datediff is also comletely used for interest calc
        /// </summary>
        public bool includeEnd { get; set; }

        public StaffelKalkulator()
        {
            includeStart = false;
            includeEnd = true;
            roundType = RoundType.KFM2;
            method = CalcMethod.M_30_360;
            perdate = new DateTime();
            tilgungsbeginn = new DateTime();
        }

        public StaffelKalkulator(CalculatorFacade cf)
        {
            this.barwert = cf.barwert;
            this.rate = cf.rate;
            this.zins = cf.zins;
            this.raten = cf.getRaten();
            this.zahlweise = cf.zahlweise;
            this.zahlmodus = cf.zahlmodus;
            this.laufzeit = cf.laufzeit;
            this.method = cf.methode;
            this.endwert = cf.letzteRate;
            this.includeStart = cf.includeStart;
            this.includeEnd = cf.includeEnd;
            if (cf.getRaten() != null && cf.getRaten().Count > 0)
            {
                this.laufzeit = cf.getRaten().Count;
                this.endwert = 0;
            }
            this.perdate = cf.valuta;
            this.tilgungsbeginn = cf.beginn;
        }

        public void setZinstageFaktor(double fac)
        {
            difftageFaktorExtern = fac;
        }

       /// <summary>
       /// Setzt die für die Kalkulation benötigten Daten
       /// </summary>
       /// <param name="barwert"></param>
       /// <param name="rate"></param>
       /// <param name="zins"></param>
       /// <param name="zahlweise"></param>
       /// <param name="laufzeit"></param>
       /// <param name="perioden"></param>
       /// <param name="endwert"></param>
       /// <param name="zahlmodus"></param>
       /// <param name="raten"></param>
        public void setParameters(double barwert, double rate, double zins, int zahlweise, double laufzeit, double perioden, double endwert, bool zahlmodus, List<double> raten)
        {
            this.barwert = barwert;
            this.rate = rate;
            this.zins = zins;
            this.perioden = perioden;
            this.endwert = endwert;
            this.zahlmodus = zahlmodus;
            this.zahlweise = zahlweise;
            this.laufzeit = laufzeit;
            this.raten = raten;
        }
      

        private double round(double value)
        {
            if (norounding)
            {
                return value;
            }
            return CalcUtil.round(value, roundType, precision);
        }
        private double round(double value, long precision)
        {
            if (norounding)
            {
                return value;
            }
            return CalcUtil.round(value, roundType, precision);
        }
        private double roundValue(double value, long precision)
        {
            return CalcUtil.round(value, roundType, precision);
        }


        public double calcENDWPeriode(double barw, double rate, double zins, long zahlweise, double laufzeit, bool zahlmodus)
        {
            return calcENDW(barw, rate, zins * zahlweise / 12.0, laufzeit / zahlweise, zahlmodus);
        }

        public double calcENDWPeriode(double barw)
        {
            return calcENDW(barw, rate, zins * zahlweise / 12.0, laufzeit / zahlweise, zahlmodus);
        }

        public double calcENDWERT(double barw)
        {


            if (raten == null || raten.Count == 0)
            {
                return calcENDWPeriode(barw);
            }

            norounding = true;
            int iter = 99;
            double barwert_neu = 0;
            double diff = 0;
            endwert = 0;
            raten[raten.Count - 1] = endwert;
            while (iter > 0 && roundValue((barwert_neu - barw), precision + 2) != 0)
            {
                barwert_neu = calcBARWERT();

                double barwert_diff = barwert_neu - barw;
                diff = Math.Abs(barwert_diff / 2);

                if (barwert_diff < 0)
                {
                    endwert += diff;
                }
                else
                {
                    endwert -= diff;
                }
                raten[raten.Count - 1] = endwert;
                iter--;
            }
            norounding = false;
            return roundValue(endwert, precision);
        }
       
        public double calcRATEPeriode(double barw, double zins, long zahlweise, double laufzeit, double endw, bool zahlmodus)
        {
            return calcRATE(barw, zins * zahlweise / 12.0, laufzeit / zahlweise, endw, zahlmodus);
        }

        public double calcRATEPeriode(double barw)
        {
            return calcRATE(barw, zins * zahlweise / 12.0, laufzeit / zahlweise, endwert, zahlmodus);
        }

        public double calcRATEN(double barwert)
        {
            if (raten == null || raten.Count== 0 || calcRange == null)
            {
                return calcRATEPeriode(barwert);
            }

            norounding = true;
            int ratecount = 0;
            String[] ranges = calcRange.Split(',');
            List<RangeSplitter> splitters = new List<RangeSplitter>();
            foreach (String range in ranges)
            {
                RangeSplitter rs = new RangeSplitter(range, (int)zahlweise, (int)laufzeit, zahlmodus);
                ratecount += rs.getRatenCount();
                rs.zero(raten);
                splitters.Add(rs);
            }
            int iter = 200;
            double barwert_neu = 0;

            
            
            while (iter > 0 && roundValue((barwert_neu - barwert), precision + 2) != 0)
            {
                barwert_neu = calcBARWERT();
                double barwert_diff = barwert_neu - barwert;

                double rateneu = calcENDW(barwert_diff, 0, zins, 0, true);
                double ratediff = -1 * rateneu / ratecount;
            
                foreach(RangeSplitter rs in splitters)
                {
                    rs.addDifference(ratediff, raten);
                }
                iter--;
            }

            for (int i = 0; i < laufzeit; i++)
            {
                raten[i]= roundValue(raten[i], 2);
            }
            norounding = false;

            return roundValue(barwert_neu, precision);
        }

        public double calcBARWPeriode(double rate, double zins, long zahlweise, double laufzeit, double endw, bool zahlmodus)
        {
            return calcBARW(rate, zins * zahlweise / 12.0, laufzeit / zahlweise, endw, zahlmodus);
        }

        public double calcBARWPeriode()
        {
            return calcBARW(rate, zins * zahlweise / 12.0, laufzeit / zahlweise, endwert, zahlmodus);
        }

        public double calcBARWERT()
        {
            if (raten == null || raten.Count == 0)
            {
                return calcBARWPeriode();
            }

            bool onr = norounding;
            norounding = true;
            barwert = 0.0;

            for (int i = 0; i < laufzeit; i += zahlweise)
            {
                int pos = 0;
                if (zahlmodus)
                {
                    pos = i;
                }
                else
                {
                    pos = i + (int)zahlweise;
                }
               
                double perioden = pos / zahlweise;
               
                double zwischenErgebnis = calcBARW(0, zins * zahlweise / 12.0, perioden, raten[i], false);
               
                barwert += zwischenErgebnis;
                
            }

            
            // Schweinchenrate
            double difftageFaktor = difftageFaktorExtern;
            //if not set externally calculate by default settings
            if(difftageFaktorExtern==0)
                difftageFaktor = CalcUtil.calcZinsTageFaktor(perdate, tilgungsbeginn, includeStart, includeEnd, method);
            
            double bwdebug = barwert;
            barwert = calcBARW(0, zins * difftageFaktor, 1, barwert, false);
            norounding = onr;

            return round(barwert);
        }
        
        public double calcLZPeriode(double barw, double rate, double zins, double zahlweise, double endw, bool zahlmodus)
        {
            return round(calcLZ(barw, rate, zins * zahlweise / 12.0, endw, zahlmodus) * zahlweise);
        }

        public long calcLZPeriode(double barw)
        {
            return (long)round(calcLZ(barw, rate, zins * zahlweise / 12.0, endwert, zahlmodus) * zahlweise);
        }
        
        public long calcLZ(double barw)
        {
            if (raten == null || raten.Count== 0)
            {
                return calcLZPeriode(barw);
            }

            return raten.Count;
        }
       
        public double calcZINSPeriode(double barw, double rate, long zahlweise, double laufzeit, double endw, bool zahlmodus)
        {
            return round(calcZINS(barw, rate, laufzeit / zahlweise, endw, zahlmodus) * (12.0 / zahlweise), precision);
        }

        public double calcZINSPeriode(double barw, long precision)
        {

            return round(calcZINS(barw, rate, laufzeit / zahlweise, endwert, zahlmodus) * (12.0 / zahlweise),precision);
        }

        public double calcZINS(double barw, long precision)
        {
            if (raten == null || raten.Count == 0)
            {
                return calcZINSPeriode(barw, precision);
            }

            norounding = true;

            int iter = 200;
            double barwert_neu = 0;
            double diff = 20;

            while (iter > 0 && round((barwert_neu - barw), precision + 2) != 0)
            {
                barwert_neu = calcBARWERT();
                double barwert_diff = barwert_neu - barw;
                diff /= 2.0;

                if (barwert_diff > 0)
                {
                    zins += diff;
                }
                else
                {
                    zins -= diff;
                }

                iter--;
            }
            norounding = false;

            return round(zins, precision);
        }

    }
}
