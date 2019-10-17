using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.Utils.BO
{
    /// <summary>
    /// Facade for easy handling of the leasing calculation logic
    /// </summary>
    public class CalculatorFacade
    {
        public double zins { get; set; }
        public double rate { get; set; }
        /// <summary>
        /// Laufzeit, including letzteRate, if letzte Rate>0
        /// </summary>
        public int laufzeit { get; set; }
        /// <summary>
        /// vorschüssig=true
        /// </summary>
        public bool zahlmodus { get; set; }
        public double barwert { get; set; }

        public double letzteRate  { get; set; }
        public int zahlweise { get; set; }
        
        public String auszahlungsDatum;
        public String tilgungsbeginn;
        public int zinstage = Int32.MinValue;
        public DateTime valuta {get;set;}
        public DateTime beginn { get; set; }
        public CalcMethod methode {get;set;}
        /// <summary>
        /// list of payments, including a rw-payment!
        /// If rw-payment is included, parameter letzteRate has also to be set!
        /// if set, parameter rate will be ignored and calcRate will deliver the first payment!
        /// </summary>
        private List<double> raten;
        /// <summary>
        /// defines if startdate of datediff is also comletely used for interest calc
        /// </summary>
        public bool includeStart{get;set;}
        /// <summary>
        /// defines if enddate of datediff is also comletely used for interest calc
        /// </summary>
        public bool includeEnd{get;set;}
        
        public CalculatorFacade()
        {
            zahlmodus = Kalkulator.ZAHLMODUS_VORSCHUESSIG;
            zahlweise = StaffelKalkulator.ZAHLWEISE_MONATLICH;
            methode = CalcMethod.M_30_360;
            includeStart = false;
            includeEnd = true;
            letzteRate = 0;
            zins = 1;
            rate = 0;
            barwert = 0;
            laufzeit = 12;
            valuta = DateTime.Now;
            beginn = DateTime.Now;
        }

        /// <summary>
        /// calculates depending on a set rate or a set barwert the corresponding other value.
        // after this call all other methods will deliver correct values
        /// </summary>
        public void calculate()
        {
            if (raten != null)
                barwert = calcBarwertFromRaten();
            //Fall a) rate=0, bw vorgegeben, dann barwert setzen, rate=0, calcRate aufrufen und dann ci.rate zuweisen
            else if (rate == 0)
                rate = calcRate();
            //Fall b) barwert=0, rate vorgegeben, dann rate setzen, barwert=0, calcBarwertFromRaten aufrufen und dann ci.barwert zuweisen
            else if (barwert == 0)
                barwert = calcBarwertFromRaten();
        }

        /// <summary>
        /// Calulates the payment for a linear payment plan
        /// </summary>
        /// <returns></returns>
        public double calcRate()
        {
            StaffelKalkulator kalk = getKalkulator();

            kalk.calcRATEN(barwert);

            return kalk.raten[0];
        }

        /// <summary>
        /// validates the calculation
        /// </summary>
        /// <returns></returns>
        public bool checkBarwert()
        {
            StaffelKalkulator kalk = getKalkulator();

            double bw = kalk.calcBARWERT();//muss mit erwartetem Barwert übereinstimmen!
            double nbw = kalk.calcRATEN(barwert);//berechnet die raten neu, der barwert muss dann dem erwarteten entsprechen, die raten auch!
            
            
            double diff = Math.Abs(nbw - bw);
            if (diff > 0.10)
            {
                Console.WriteLine("---------------CALCULATION FAILURE!!!----------------------");
                if(kalk.raten!=null)
                Console.WriteLine(String.Join(";",kalk.raten) + " " + kalk.raten.Count);
                Console.WriteLine("expected BW: " + barwert + " -  barwert: " + nbw + " barwert calced: " + bw);
                if(kalk.raten!=null)
                Console.WriteLine("expected Rate: " + rate + " -  rate: " + kalk.raten[0]);
                else
                    Console.WriteLine("expected Rate: " + rate );
                Console.WriteLine("-----------------------------------------------------------");
                return false;
            }

            return true;
        }

        private StaffelKalkulator getKalkulator()
        {
            this.valuta = auszahlungsDatum != null ? getPerdatum() : this.valuta;
            this.beginn = tilgungsbeginn!=null?getTilgungsbeginn():this.beginn;
            StaffelKalkulator kalk = new StaffelKalkulator(this);
            //kalk.setParameters(barwert, rate, zins, zahlweise, laufzeit, 0, 0, zahlmodus, getRaten());


            if (zinstage != Int32.MinValue)
            {
                kalk.setZinstageFaktor(this.zinstage/360.0); //when set external
            }
            else
            {
                zinstage = CalcUtil.calcZinsTage(this.valuta, this.beginn, includeStart, includeEnd, methode);
                kalk.setZinstageFaktor(CalcUtil.calcZinsTageFaktor(this.valuta, this.beginn, includeStart, includeEnd, methode));
            }
            if (letzteRate > 0)
            {
                kalk.calcRange = "1-" + (laufzeit - 1);
            }
            else
            {
                kalk.calcRange = "1-" + (laufzeit);
            }
            return kalk;
        }

        /// <summary>
        /// returns the payments from the given barwert (and residual value if set)
        /// </summary>
        /// <returns></returns>
        public List<double> calcRatenFromBarwert()
        {

            StaffelKalkulator kalk = getKalkulator();
            kalk.calcRATEN(barwert);
            return kalk.raten;

        }

        /// <summary>
        /// returns the calculated Barwert from the given payments
        /// </summary>
        /// <returns></returns>
        public double calcBarwertFromRaten()
        {

            StaffelKalkulator kalk = getKalkulator();
            return kalk.calcBARWERT();

        }

     
        /// <summary>
        /// calulates the RW Barwert by setting all payments to zero except the RW-Payment
        /// </summary>
        /// <returns></returns>
        public double calcBarwertRestwertFromRaten()
        {

            StaffelKalkulator kalk = getKalkulator();
            List<double> ratenOrg = raten;
            double rateOrg = rate;
            rate = 0;
            raten = null;
            kalk.setParameters(barwert, rate, zins, zahlweise, laufzeit, 0, 0, zahlmodus, getRaten());
            double rval = kalk.calcBARWERT();
            raten = ratenOrg;
            rate = rateOrg;
            return rval;
        }

        

        /// <summary>
        /// returns the barwert for the payments, ignoring the residual value by setting residual value to zero and reducing the runtime
        /// </summary>
        /// <returns></returns>
        public double calcBarwertFKFromRaten()
        {

            StaffelKalkulator kalk = getKalkulator();
            List<double> ratenOrg = raten;
            double letzteRateOrg = letzteRate;
            int laufzeitOrg = laufzeit;
            if (letzteRate > 0)
                laufzeit--;
            letzteRate = 0;
            raten = null;
            kalk.setParameters(barwert, rate, zins, zahlweise, laufzeit, 0, 0, zahlmodus, getRaten());
            double rval = kalk.calcBARWERT();
            raten = ratenOrg;
            letzteRate = letzteRateOrg;
            laufzeit = laufzeitOrg;
            return rval;
        }



        /// <summary>
        /// returns the current ratenplan
        /// </summary>
        /// <returns></returns>
        public List<double> getRaten()
        {
            if (this.raten != null)
            {
                return raten;
            }

            List<double> rval = new List<double>();
            for (int i = 0; i < laufzeit; i++)
            {
                if (i % zahlweise == 0)
                {
                    if (i == (laufzeit - 1) && letzteRate > 0)
                    {
                        rval.Add(letzteRate);
                    }
                    else
                    {
                        rval.Add(rate);
                    }
                }
                else
                {
                    rval.Add(0);
                }
            }
            return rval;
        }

        /// <summary>
        /// returns the valuta date
        /// </summary>
        /// <returns></returns>
        public DateTime getPerdatum()
        {

            return DateTime.ParseExact(auszahlungsDatum, "yyyy-MM-dd",
                                   System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// returns the tilgungsbeginn
        /// </summary>
        /// <returns></returns>
        public DateTime getTilgungsbeginn()
        {

            return DateTime.ParseExact(tilgungsbeginn, "yyyy-MM-dd",
                                   System.Globalization.CultureInfo.InvariantCulture);
        }

       /// <summary>
       /// Sets the ratenplan
       /// </summary>
       /// <param name="raten"></param>
        public void setRaten(List<double> raten)
        {
            this.raten = raten;
        }

        /// <summary>
        ///  Tests calculation logic by using several known good values
        /// if one test fails, the calculator is not in a valid state
        /// </summary>
        public static void testCalculations()
        {
            CalculatorFacade ci = new CalculatorFacade();
            ci.auszahlungsDatum = "2012-09-01";
            ci.tilgungsbeginn = "2012-10-01";
            ci.zins = 2.83;
            ci.zinstage = 29;
            ci.laufzeit = 4;
            ci.letzteRate = 127475.18;
            ci.zahlweise = 1;
            ci.rate = 904.82;
            ci.barwert = 128991.54;
            ci.zahlmodus = Kalkulator.ZAHLMODUS_VORSCHUESSIG;

            List<double> raten = new List<double>();
            raten.Add(904.82);
            raten.Add(904.82);
            raten.Add(904.82);
            raten.Add(127475.18);
            ci.setRaten(raten);
            ci.checkBarwert();

            ci = new CalculatorFacade();
            ci.auszahlungsDatum = "2012-09-01";
            ci.tilgungsbeginn = "2012-09-30";
            ci.zins = 2.7;
            ci.zinstage = 29;
            ci.laufzeit = 48;
            ci.letzteRate = 2500;
            ci.zahlweise = 1;
            ci.rate = 338.07;
            ci.barwert = 17307.59;
            ci.zahlmodus = Kalkulator.ZAHLMODUS_VORSCHUESSIG;

            ci.checkBarwert();


            ci = new CalculatorFacade();
            ci.auszahlungsDatum = "2012-09-20";
            ci.tilgungsbeginn = "2012-10-01";
            ci.zins = 3.010;
            ci.zinstage = 11;
            ci.laufzeit = 55;
            ci.letzteRate = 0;
            ci.zahlweise = 3;
            ci.rate = 843.13;
            ci.barwert = 14973.04;
            ci.zahlmodus = Kalkulator.ZAHLMODUS_VORSCHUESSIG;

            //HP: 14986.90, Gillardon: 14973.04, Delta: 13.86
            //HP rate mit BW 14973,04: 842,35, Delta: 0,78 zu korrekter Rate
            //-> Das Delta von 13,86 muss als BW über die LZ abgezinst werden, gibt eine monatliche Rate von 0.78! diese muss dann unserer Rate aufgeschlagen werden!

            //algorithmus:
            //aus getipptem BW Rate bestimmen über die LZ
            //aus datetiff valuta zu tilgungsbeginn  den zinsanteil für valuta bis tilgungsbeginn berechnen
            //den zinsanteil über die lz als rate berechnen und jeder einzelrate aufschlagen

            ci.checkBarwert();


            //Beispiel Ratenankauf Restwert Darlehen:
            //Raten: 353286.299
            //RW 78565,004
            //AUSZAHLUNG 431851.304
            //Beide
            ci = new CalculatorFacade();
            ci.auszahlungsDatum = "2012-11-06";
            ci.tilgungsbeginn = "2012-11-30";
            ci.zins = 2.32;
            ci.laufzeit = 53;//finlaufzeit+1, weil Rw nach den lz raten=52 extra kommt
            ci.letzteRate = 87000;
            ci.zinstage = 24;
            ci.zahlweise = 1;
            ci.rate = 7145;
            ci.barwert = 431851.304;
            ci.zahlmodus = Kalkulator.ZAHLMODUS_VORSCHUESSIG;

            ci.checkBarwert();
           

            //Nur Raten:
            ci = new CalculatorFacade();
            ci.auszahlungsDatum = "2012-11-06";
            ci.tilgungsbeginn = "2012-11-30";
            ci.zins = 2.32;
            ci.laufzeit = 52;
            ci.zinstage = 24;
            ci.zahlweise = 1;
            ci.rate = 7145;
            ci.barwert = 353286.299;
            ci.zahlmodus = Kalkulator.ZAHLMODUS_VORSCHUESSIG;

            ci.checkBarwert();
           
            //Nur RW:
            ci = new CalculatorFacade();
            ci.auszahlungsDatum = "2012-11-06";
            ci.tilgungsbeginn = "2012-11-30";
            ci.zins = 2.32;
            ci.laufzeit = 53;//finlaufzeit+1
            ci.zahlweise = 1;
            ci.letzteRate = 87000;
            ci.zinstage = 24;
            ci.rate = 0;
            ci.barwert = 78565;
            ci.zahlmodus = Kalkulator.ZAHLMODUS_VORSCHUESSIG;

            ci.checkBarwert();



           
        }
        
        public static void main(String[] args)
        {

            CalculatorFacade.testCalculations();


            CalculatorFacade ci = new CalculatorFacade();

            //DER HIER MUSS ANGEBLICH GEHEN
            ci.auszahlungsDatum = "2013-10-30";
            ci.tilgungsbeginn = "2013-12-01";
            ci.zins = 3.13;
            ci.zahlweise = 1;
            ci.laufzeit = 43;//including letzte rate if letzte rate>0
            ci.letzteRate = 850.52;
            ci.rate = 850.52;
            ci.zinstage = 31;
            ci.barwert = 34550.43;//ERWARTET: 34553.52
            ci.zahlmodus = Kalkulator.ZAHLMODUS_VORSCHUESSIG;
            ci.checkBarwert();

            //System.out.println("Correct values for given rate,laufzeit,zins, auszahlungsDatum, tilgungsbeginn,letzteRate: ");
            Console.WriteLine("BW FK: " + ci.calcBarwertFKFromRaten());
            Console.WriteLine("BW RW: " + ci.calcBarwertRestwertFromRaten());
            Console.WriteLine("BW Gesamt: " + ci.calcBarwertFromRaten());
            //System.out.println("Raten: " + ci.calcRatenFromBarwert());
            Console.WriteLine("update einzelfinanzierung set rate_ankauf=" + ci.calcRate() + ", barwert_lr=" + ci.calcBarwertFKFromRaten() + ", barwert_rw=" + ci.calcBarwertRestwertFromRaten() + ", auszahlung=" + ci.calcBarwertFromRaten() + " where nr=");
        }
    }
}
