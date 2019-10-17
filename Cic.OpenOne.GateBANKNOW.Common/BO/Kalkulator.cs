using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Kalkulator
    /// </summary>
    public class Kalkulator
    {
        private static double MAX_ERROR = 0.0000001;
        private static int MAX_STEPS = 10;
        /// <summary>
        /// RATENART_LINEAR
        /// </summary>
        public static int RATENART_LINEAR = 0;

        /// <summary>
        /// RATENART_SONSTIGES
        /// </summary>
        public static int RATENART_SONSTIGES = 1;

        /// <summary>
        /// ZAHLMODUS_VORSCHUESSIG
        /// </summary>
        public static bool ZAHLMODUS_VORSCHUESSIG = true;

        /// <summary>
        /// ZAHLMODUS_NACHSCHUESSIG
        /// </summary>
        public static bool ZAHLMODUS_NACHSCHUESSIG = false;

        /// <summary>
        /// METHODE_30_360
        /// </summary>
        public static String METHODE_30_360 = "30/360";
        private String methode = METHODE_30_360;

        private int ratenart = RATENART_LINEAR;

        private bool zahlmodus = ZAHLMODUS_VORSCHUESSIG;


        /** Berechnet den Single Payment Present Value
         * (einmalige Abzinsung einer Zahlung)
         * @param zins Periodischer Zinssatz, in Prozent ausgedrÃ¼ckt
         * @param perioden Anzahl der Verzinsungsperioden 
         * @return Single Payment Present Value
         */
        public static double calcSPPV(double zins, double perioden)
        {
            return System.Math.Pow(1.0 + zins / 100.0, -perioden);
        }

        /** Berechnet den Single Payment Future Value
         * (einmalige Aufzinsung einer Zahlung)
         * @param zins Periodischer Zinssatz, in Prozent ausgedrÃ¼ckt
         * @param perioden Anzahl der Verzinsungsperioden 
         * @return Single Payment Future Value
         */
        public static double calcSPFV(double zins, double perioden)
        {
            return System.Math.Pow(1.0 + zins / 100.0, perioden);
        }

        /** Berechnet den Uniform Series Present Value
         * (Barwert einer gleichbleibenden Rente)
         * @param zins Periodischer Zinssatz, in Prozent ausgedrÃ¼ckt
         * @param perioden Anzahl der Verzinsungsperioden 
         * @return Uniform Series Present Value
         */
        public static double calcUSPV(double zins, double perioden)
        {
            if (zins == 0)
            {
                return perioden;
            }
            double z = zins / 100.0;
            return (1 - System.Math.Pow(1.0 + z, -perioden)) / z;
        }

        /** Berechnet den Uniform Series Future Value
         * (Zukunftswert einer gleichbleibenden Rente)
         * @param zins Periodischer Zinssatz, in Prozent ausgedrÃ¼ckt
         * @param perioden Anzahl der Verzinsungsperioden 
         * @return Uniform Series Future Value
         */
        public static double calcUSFV(double zins, double perioden)
        {
            double z = zins / 100.0;
            return (System.Math.Pow(1.0 + z, perioden) - 1) / z;
        }

        /** AnnuitÃ¤tenrechnung.
         * Berechnet den Barwert in AbhÃ¤ngigkeit von 
         * Rate, Zinssatz, Laufzeit, Schlusszahlung, vor- oder nachschÃ¼ssig
         * @param rate HÃ¶he der Zahlungen
         * @param zins Periodischer Zinssatz, in Prozent ausgedrÃ¼ckt
         * @param perioden Anzahl der Verzinsungsperioden 
         * @param endw Schlusszahlung
         * @param zahlmodus Zahlmodus (vorschÃ¼ssig=true, nachschÃ¼ssig=false)
         * @return Barwert der AnnuitÃ¤tenrechnung
         */
        public static double calcBARW(double rate, double zins,
                                      double perioden, double endw, bool zahlmodus)
        {
            if (zahlmodus)
            { //vorschÃ¼ssig
                return rate * calcUSPV(zins, perioden) * (1.0 + zins / 100.0) + endw * calcSPPV(zins, perioden);
            }
            else
            { //nachschÃ¼ssig
                return rate * calcUSPV(zins, perioden) + endw * calcSPPV(zins, perioden);
            }
        }

        /** AnnuitÃ¤tenrechnung.
         * Berechnet die Laufzeit in AbhÃ¤ngigkeit von 
         * Barwert, Rate, Zinssatz, Schlusszahlung, vor- oder nachschÃ¼ssig
         * @param barw Barwert der AnnuitÃ¤tenrechnung
         * @param rate HÃ¶he der Zahlungen
         * @param zins Periodischer Zinssatz, in Prozent ausgedrÃ¼ckt
         * @param endw Schlusszahlung
         * @param zahlmodus Zahlmodus (vorschÃ¼ssig=true, nachschÃ¼ssig=false)
         * @return Anzahl der Verzinsungsperioden 
         */
        public static double calcLZ(double barw, double rate, double zins,
                                    double endw, bool zahlmodus)
        {
            double perioden = 12;  // Startwert

            if (zahlmodus)
            { //vorschÃ¼ssig
                perioden = calcLZiterationvorschuessig(barw, rate, zins, endw, MAX_STEPS, perioden);
            }
            else
            { //nachschÃ¼ssig
                perioden = calcLZiterationnachschuessig(barw, rate, zins, endw, MAX_STEPS, perioden);
            }

            return perioden;
        }



        /// <summary>
        /// Kapitalrückzahlung einer Investition für die angegebene Periode Per (Buchwert für Rate X)
        /// </summary>
        /// <param name="intr">zins pro periode, z.b. 5.9/1200 bei monatlichen Zahlungen</param>
        /// <param name="Per">Periode welche zu berechnen ist, zwischen 1 und NPer</param>
        /// <param name="NPer">Laufzeit</param>
        /// <param name="PV">Barwert, negativ</param>
        /// <param name="FV">Restwert</param>
        /// <param name="Due">true wenn vorschüssig</param>
        /// <returns></returns>
        public static double calcPPMT(double intr, int Per, double NPer, double PV, double FV, bool Due)
        {
            double float1 = Per - (Due ? 2 : 1);
            double float2 = -FV * intr / ((Math.Pow(1 + intr, NPer) - 1) * (Due ? 1 + intr : 1)) + -PV / ((Due ? 1 : 0) + 1 / intr * (1 - 1 / Math.Pow(1 + intr, NPer - (Due ? 1 : 0))));
            return ((-FV * intr / ((Math.Pow(1 + intr, NPer) - 1) * (Due ? 1 + intr : 1)) + -PV / ((Due ? 1 : 0) + 1 / intr * (1 - 1 / Math.Pow(1 + intr, NPer - (Due ? 1 : 0)))) - -(PV * Math.Pow(1 + intr, float1) - (-0 * Math.Pow(1 + intr, float1) + -(1 / intr) * float2 * (Math.Pow(1 + intr, float1) - 1) * (Due ? 1 + intr : 1) - (Due ? float2 : 0))) * intr) * -1);
        }


        public static double calcPREFV(double PV, int NPer, double rate, double payment)
        {

            double fv = (PV * Math.Pow(1 + rate, NPer)) + (payment * (1 + rate) *( (Math.Pow(1 + rate, NPer) - 1) / rate));
            return fv;
        }


        public static double calcPV(double pmt, int NPer, double ip, double fv, bool vors)
        {
            double k = vors ? 1 + ip : 1 ;
            double temp1 = 1/Math.Pow(1+ip,NPer);
            double temp2 = pmt*k/ip;
            double PV = (temp2 - fv)*(temp2-temp1);
            return PV;
        }


        public static double calcPREPMT(double PV, int NPer, double rate, double FV)
        {
            double pow1 = Math.Pow(1 + rate, -NPer);
            double pmt = ( FV *pow1 - PV*(Math.Pow(1 + rate, 0) ) )/ ((1+rate)*(1-pow1)/rate);
            return pmt;

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="PV">Present Value</param>
        /// <param name="NPer">Number of Period</param>
        /// <param name="rate">Interest Rate per Period</param>
        /// <param name="FV">Future Value</param>
        /// <param name="vors">true wenn vorschüssig</param>
        /// <returns></returns>
        public static double calcpmt(double PV, int NPer, double rate, double FV, bool vors)
        {

            double k = vors ? 1 + rate : 1 ;
            double temp = Math.Pow(1 + rate, NPer) - 1;
            double pmt = (PV + ((PV + FV) / temp)) * -rate / k;
            return pmt;


        }

        /** Hilfsfunktion fÃ¼r die Berechnung der Laufzeit nach dem Iterationsverfahren 
         * nach Newton.
         */
        private static double calcLZiterationvorschuessig(double barw, double rate,
                                                          double zins, double endw, int step, double perioden)
        {
            if (step == 0)
            {
                return perioden;
            }

            double z = zins / 100.0;
            double s = System.Math.Pow(1.0 + z, -perioden);
            double u = (1 - s) / z;
            double r_z_rezip = rate / z;

            double zaehler = rate * u * (1.0 + z) + endw * s - barw;
            double nenner = ((1.0 + z) * r_z_rezip - endw) * s * System.Math.Log(1.0 + z);
            double perioden_new = perioden - zaehler / nenner;

            double rel_error = System.Math.Abs((perioden - perioden_new) / perioden_new);
            if (rel_error < MAX_ERROR)
            {
                return perioden_new;
            }

            return calcLZiterationvorschuessig(barw, rate, zins, endw, step - 1, perioden_new);
        }

        /** Hilfsfunktion fÃ¼r die Berechnung der Laufzeit nach dem Iterationsverfahren 
         * nach Newton.
         */
        private static double calcLZiterationnachschuessig(double barw, double rate,
                                                           double zins, double endw, int step, double perioden)
        {
            if (step == 0)
            {
                return perioden;
            }

            double z = zins / 100.0;
            double s = System.Math.Pow(1.0 + z, -perioden);
            double u = (1 - s) / z;
            double r_z_rezip = rate / z;

            double zaehler = rate * u + endw * s - barw;
            double nenner = (r_z_rezip - endw) * s * System.Math.Log(1.0 + z);
            double perioden_new = perioden - zaehler / nenner;

            double rel_error = System.Math.Abs((perioden - perioden_new) / perioden_new);
            if (rel_error < MAX_ERROR)
            {
                return perioden_new;
            }

            return calcLZiterationnachschuessig(barw, rate, zins, endw, step - 1, perioden_new);
        }

        /** AnnuitÃ¤tenrechnung.
         * Berechnet die Schlusszahlung in AbhÃ¤ngigkeit von 
         * Barwert, Rate, Zinssatz, Laufzeit, vor- oder nachschÃ¼ssig
         * @param barw Barwert der AnnuitÃ¤tenrechnung
         * @param rate HÃ¶he der Zahlungen
         * @param zins Periodischer Zinssatz, in Prozent ausgedrÃ¼ckt
         * @param perioden Anzahl der Verzinsungsperioden 
         * @param zahlmodus Zahlmodus (vorschÃ¼ssig=true, nachschÃ¼ssig=false)
         * @return Schlusszahlung
         */
        public static double calcENDW(double barw, double rate, double zins,
                                      double perioden, bool zahlmodus)
        {
            if (zahlmodus)
            { //vorschÃ¼ssig
                return (barw - rate * calcUSPV(zins, perioden) * (1.0 + zins / 100.0)) / calcSPPV(zins, perioden);
            }
            else
            { //nachschÃ¼ssig
                return (barw - rate * calcUSPV(zins, perioden)) / calcSPPV(zins, perioden);
            }
        }

        /** AnnuitÃ¤tenrechnung.
         * Berechnet die Rate in AbhÃ¤ngigkeit von 
         * Barwert, Zinssatz, Laufzeit, Schlusszahlung, vor- oder nachschÃ¼ssig
         * @param rate Barwert der AnnuitÃ¤tenrechnung
         * @param zins Periodischer Zinssatz, in Prozent ausgedrÃ¼ckt
         * @param perioden Anzahl der Verzinsungsperioden 
         * @param endw Schlusszahlung
         * @param zahlmodus Zahlmodus (vorschÃ¼ssig=true, nachschÃ¼ssig=false)
         * @return HÃ¶he der Zahlungen
         */
        public static double calcRATE(double barw, double zins,
                                      double perioden, double endw, bool zahlmodus)
        {
            if (zahlmodus)
            { //vorschÃ¼ssig
                return (barw - endw * calcSPPV(zins, perioden)) / (calcUSPV(zins, perioden) * (1.0 + zins / 100.0));
            }
            else
            { //nachschÃ¼ssig
                return (barw - endw * calcSPPV(zins, perioden)) / calcUSPV(zins, perioden);
            }
        }

        /** AnnuitÃ¤tenrechnung.
         * Berechnet den Zins in AbhÃ¤ngigkeit von Barwert,
         * Rate, Zinssatz, Laufzeit, Schlusszahlung, vor- oder nachschÃ¼ssig.
         * Die Berechnung erfolgt numerisch nach dem Newton-Iterationsverfahren:
         * z_(i+1) = z_i - f(z_i) / f'(z_i)
         * @param barw Barwert der AnnuitÃ¤tenrechnung
         * @param rate HÃ¶he der Zahlungen
         * @param perioden Anzahl der Verzinsungsperioden 
         * @param endw Schlusszahlung
         * @param zahlmodus Zahlmodus (vorschÃ¼ssig=true, nachschÃ¼ssig=false)
         * @return Periodischer Zinssatz, in Prozent ausgedrÃ¼ckt
         */
        public static double calcZINS(double barw, double rate,
                                      double perioden, double endw, bool zahlmodus)
        {
            double z = 5.0 / 1200;  // Startwert: 5% pro Jahr

            if (zahlmodus)
            { //vorschÃ¼ssig
                z = calcZINSiterationvorschuessig(barw, rate, perioden, endw, MAX_STEPS, z);
            }
            else
            { //nachschÃ¼ssig
                z = calcZINSiterationnachschuessig(barw, rate, perioden, endw, MAX_STEPS, z);
            }

            return z * 100;         // Zins pro Monat (=Zinsperiode)
        }

        /** Hilfsfunktion fÃ¼r die Berechnung des Zinssatzes nach dem Iterationsverfahren 
         * nach Newton.
         * @param z = zins
         * @param step = reduced stepcount
         * @param endw = Schlusszahlung
         * @param perioden = Anzahl der Verzingsungsperioden
         * @param rate = HÃ¶he der Ratenzahlung
         * @param barw = Barwert der AnnuitÃ¤tenrechnung
         */
        private static double calcZINSiterationvorschuessig(double barw, double rate,
                                                            double perioden, double endw, int step, double z)
        {
            if (step == 0)
            {
                return z;
            }

            double s = System.Math.Pow(1.0 + z, -perioden);
            double s_min = System.Math.Pow(1.0 + z, -perioden - 1);
            double u = (1 - s) / z;
            double z_rezip = 1.0 / z;

            double zaehler = rate * u * (1.0 + z) + endw * s - barw;
            double nenner = perioden * s_min * (rate * (z_rezip + 1) - endw) - rate * z_rezip * u;
            double z_new = z - zaehler / nenner;

            double rel_error = System.Math.Abs((z - z_new) / z_new);
            if (rel_error < MAX_ERROR)
            {
                return z_new;
            }

            return calcZINSiterationvorschuessig(barw, rate, perioden, endw, step - 1, z_new);
        }

        /** Hilfsfunktion fÃ¼r die Berechnung des Zinssatzes nach dem Iterationsverfahren 
         * nach Newton.
         */
        private static double calcZINSiterationnachschuessig(double barw, double rate,
                                                             double perioden, double endw, int step, double z)
        {
            if (step == 0)
            {
                return z;
            }

            double s = System.Math.Pow(1.0 + z, -perioden);
            double s_min = System.Math.Pow(1.0 + z, -perioden - 1);
            double u = (1 - s) / z;
            double r_z_rezip = rate / z;

            double zaehler = rate * u + endw * s - barw;
            double nenner = perioden * s_min * (r_z_rezip - endw) - r_z_rezip * u;
            double z_new = z - zaehler / nenner;

            double rel_error = System.Math.Abs((z - z_new) / z_new);
            if (rel_error < MAX_ERROR)
            {
                return z_new;
            }

            return calcZINSiterationnachschuessig(barw, rate, perioden, endw, step - 1, z_new);
        }


    }

}


