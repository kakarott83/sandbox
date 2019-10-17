using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.One.Utils.Util.Exceptions;

namespace Cic.One.Utils.BO
{
    /// <summary>
    /// Leasing-Kalkulator
    /// Basis aller weiteren Kalkulatoren
    /// </summary>
    public class Kalkulator
    {
        private static double MAX_ERROR = 0.0000001;
        private static int MAX_STEPS = 10;
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static bool ZAHLMODUS_VORSCHUESSIG = true;
        public static bool ZAHLMODUS_NACHSCHUESSIG = false;

        /// <summary>
        /// Berechnet den Single Payment Present Value
        /// (einmalige Abzinsung einer Zahlung)
        /// </summary>
        /// <param name="zins">Periodischer Zinssatz, in Prozent ausgedrückt</param>
        /// <param name="perioden">Anzahl der Verzinsungsperioden </param>
        /// <returns>Single Payment Present Value</returns>
        public static double calcSPPV(double zins, double perioden)
        {
            return System.Math.Pow(1.0 + zins / 100.0, -perioden);
        }


        /// <summary>
        /// Berechnet den Single Payment Future Value
        /// (einmalige Aufzinsung einer Zahlung)
        /// </summary>
        /// <param name="zins">Periodischer Zinssatz, in Prozent ausgedrückt</param>
        /// <param name="perioden">Anzahl der Verzinsungsperioden </param>
        /// <returns>Single Payment Future Value</returns>
        public static double calcSPFV(double zins, double perioden)
        {
            return System.Math.Pow(1.0 + zins / 100.0, perioden);
        }

        /// <summary>
        /// Berechnet den Uniform Series Present Value
        /// (Barwert einer gleichbleibenden Rente)
        /// </summary>
        /// <param name="zins">Periodischer Zinssatz, in Prozent ausgedrückt</param>
        /// <param name="perioden"> Anzahl der Verzinsungsperioden </param>
        /// <returns>Uniform Series Present Value</returns>
        public static double calcUSPV(double zins, double perioden)
        {
            if (zins == 0)
            {
                return perioden;
            }
            double z = zins / 100.0;
            return (1 - System.Math.Pow(1.0 + z, -perioden)) / z;
        }

        /// <summary>
        /// Berechnet den Uniform Series Future Value
        /// (Zukunftswert einer gleichbleibenden Rente)
        /// </summary>
        /// <param name="zins">Periodischer Zinssatz, in Prozent ausgedrückt</param>
        /// <param name="perioden">perioden Anzahl der Verzinsungsperioden </param>
        /// <returns>Uniform Series Future Value</returns>
        public static double calcUSFV(double zins, double perioden)
        {
            double z = zins / 100.0;
            return (System.Math.Pow(1.0 + z, perioden) - 1) / z;
        }


        /// <summary>
        /// Annuitätenrechnung.
        /// Berechnet den Barwert in Abhängigkeit von 
        /// Rate, Zinssatz, Laufzeit, Schlusszahlung, vor- oder nachschüssig
        /// </summary>
        /// <param name="rate">Höhe der Zahlungen</param>
        /// <param name="zins">Periodischer Zinssatz, in Prozent ausgedrückt</param>
        /// <param name="perioden"> Anzahl der Verzinsungsperioden </param>
        /// <param name="endw">Schlusszahlung</param>
        /// <param name="zahlmodus">Zahlmodus (vorschüssig=true, nachschüssig=false)</param>
        /// <returns>Barwert der Annuitätenrechnung</returns>
        public static double calcBARW(double rate, double zins,
                                      double perioden, double endw, bool zahlmodus)
        {
            if (zahlmodus)
            { //vorschüssig
                return rate * calcUSPV(zins, perioden) * (1.0 + zins / 100.0) + endw * calcSPPV(zins, perioden);
            }
            else
            { //nachschüssig
                return rate * calcUSPV(zins, perioden) + endw * calcSPPV(zins, perioden);
            }
        }

        /// <summary>
        /// Annuitätenrechnung.
        /// Berechnet die Laufzeit in Abhängigkeit von Barwert, Rate, Zinssatz, Schlusszahlung, vor- oder nachschüssig
        /// </summary>
        /// <param name="barw">Barwert der Annuitätenrechnung</param>
        /// <param name="rate">Höhe der Zahlungen</param>
        /// <param name="zins">Periodischer Zinssatz, in Prozent ausgedrückt</param>
        /// <param name="endw">Schlusszahlung</param>
        /// <param name="zahlmodus">Zahlmodus (vorschüssig=true, nachschüssig=false)</param>
        /// <returns>Anzahl der Verzinsungsperioden </returns>
        public static double calcLZ(double barw, double rate, double zins,
                                    double endw, bool zahlmodus)
        {
            double perioden = 12;  // Startwert

            if (zahlmodus)
            { //vorschüssig
                perioden = calcLZiterationvorschuessig(barw, rate, zins, endw, MAX_STEPS, perioden);
            }
            else
            { //nachschüssig
                perioden = calcLZiterationnachschuessig(barw, rate, zins, endw, MAX_STEPS, perioden);
            }

            return perioden;
        }


        /// <summary>
        /// Hilfsfunktion für die Berechnung der Laufzeit nach dem Iterationsverfahren nach Newton.
        /// </summary>
        /// <param name="barw">Barwert der Annuitätenrechnung</param>
        /// <param name="rate">Höhe der Ratenzahlung</param>
        /// <param name="zins">Zins</param>
        /// <param name="endw">Schlusszahlung</param>
        /// <param name="step">reduced stepcount</param>
        /// <param name="perioden">Anzahl der Verzingsungsperioden</param>
        /// <returns>laufzeit</returns>
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


        /// <summary>
        /// Hilfsfunktion für die Berechnung der Laufzeit nach dem Iterationsverfahren nach Newton.
        /// </summary>
        /// <param name="barw">Barwert der Annuitätenrechnung</param>
        /// <param name="rate">Höhe der Ratenzahlung</param>
        /// <param name="zins">Zins</param>
        /// <param name="endw">Schlusszahlung</param>
        /// <param name="step">reduced stepcount</param>
        /// <param name="perioden">Anzahl der Verzingsungsperioden</param>
        /// <returns>laufzeit</returns>
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

        /// <summary>
        /// Annuitätenrechnung.
        /// Berechnet die Schlusszahlung in Abhängigkeit von 
        /// Barwert, Zinssatz, Laufzeit, Rate, vor- oder nachschüssig
        /// </summary>
        /// <param name="barw">Barwert der Annuitätenrechnung</param>
        /// <param name="rate">Höhe der Zahlungen</param>
        /// <param name="zins"> Periodischer Zinssatz, in Prozent ausgedrückt</param>
        /// <param name="perioden"> Anzahl der Verzinsungsperioden </param>
        /// <param name="zahlmodus">Zahlmodus (vorschüssig=true, nachschüssig=false)</param>
        /// <returns></returns>
        public static double calcENDW(double barw, double rate, double zins,
                                      double perioden, bool zahlmodus)
        {
            if (zahlmodus)
            { //vorschüssig
                return (barw - rate * calcUSPV(zins, perioden) * (1.0 + zins / 100.0)) / calcSPPV(zins, perioden);
            }
            else
            { //nachschüssig
                return (barw - rate * calcUSPV(zins, perioden)) / calcSPPV(zins, perioden);
            }
        }


        /// <summary>
        /// Annuitätenrechnung.
        /// Berechnet die Rate in Abhängigkeit von 
        /// Barwert, Zinssatz, Laufzeit, Schlusszahlung, vor- oder nachschüssig
        /// </summary>
        /// <param name="barw"> Barwert der Annuitätenrechnung</param>
        /// <param name="zins">Zinssatz</param>
        /// <param name="perioden">Anzahl der Verzinsungsperioden</param>
        /// <param name="endw">Schlusszahlung</param>
        /// <param name="zahlmodus">Zahlmodus (vorschüssig=true, nachschüssig=false)</param>
        /// <returns>Höhe der Zahlungen</returns>
        public static double calcRATE(double barw, double zins,
                                      double perioden, double endw, bool zahlmodus)
        {
            if (zahlmodus)
            { //vorschüssig
                return (barw - endw * calcSPPV(zins, perioden)) / (calcUSPV(zins, perioden) * (1.0 + zins / 100.0));
            }
            else
            { //nachschüssig
                return (barw - endw * calcSPPV(zins, perioden)) / calcUSPV(zins, perioden);
            }
        }

        /// <summary>
        /// Berechnet den Zins in Abhängigkeit von Barwert,
        /// Rate, Zinssatz, Laufzeit, Schlusszahlung, vor- oder nachschüssig.
        /// Die Berechnung erfolgt numerisch nach dem Newton-Iterationsverfahren:
        /// z_(i+1) = z_i - f(z_i) / f'(z_i)
        /// </summary>
        /// <param name="barw"> Barwert der Annuitätenrechnung</param>
        /// <param name="rate">Höhe der Zahlungen</param>
        /// <param name="perioden">Anzahl der Verzinsungsperioden </param>
        /// <param name="endw">Schlusszahlung</param>
        /// <param name="zahlmodus"> Zahlmodus (vorschüssig=true, nachschüssig=false)</param>
        /// <returns>Zins</returns>
        public static double calcZINS(double barw, double rate,
                                      double perioden, double endw, bool zahlmodus)
        {
            double z = 5.0 / 1200;  // Startwert: 5% pro Jahr
            try
            {
                if (zahlmodus)
                { //vorschüssig
                    z = calcZINSiterationvorschuessig(barw, rate, perioden, endw, MAX_STEPS, z);
                }
                else
                { //nachschüssig
                    z = calcZINSiterationnachschuessig(barw, rate, perioden, endw, MAX_STEPS, z);
                }
            }
            catch (Exception ex)
            {
                _Log.Warn("Error in calcZins: " + ex.Message + " barw: " + barw + " rate: " + rate + " perioden: " + perioden + " endw: " + endw + " zahlmodus: " + zahlmodus);
                throw ex;
            }
            return z * 100;         // Zins pro Monat (=Zinsperiode)
        }



        /// <summary>
        /// Hilfsfunktion für die Berechnung des Zinssatzes nach dem Iterationsverfahren  nach Newton.
        /// </summary>
        /// <param name="barw">Barwert der Annuitätenrechnung</param>
        /// <param name="rate">Höhe der Ratenzahlung</param>
        /// <param name="perioden">Anzahl der Verzingsungsperioden</param>
        /// <param name="endw">Schlusszahlung</param>
        /// <param name="step">reduced stepcount</param>
        /// <param name="z">zins</param>
        /// <returns>Zins</returns>
        private static double calcZINSiterationvorschuessig(double barw, double rate,
                                                            double perioden, double endw, int step, double z)
        {
            if (step == 0)
            {
                return z;
            }
            if (z < 0) throw new InterestUnderflowException();
            if (z > 100) throw new InterestOverflowException();

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

        /// <summary>
        /// Hilfsfunktion für die Berechnung des Zinssatzes nach dem Iterationsverfahren  nach Newton.
        /// </summary>
        /// <param name="barw">Barwert der Annuitätenrechnung</param>
        /// <param name="rate">Höhe der Ratenzahlung</param>
        /// <param name="perioden">Anzahl der Verzingsungsperioden</param>
        /// <param name="endw">Schlusszahlung</param>
        /// <param name="step">reduced stepcount</param>
        /// <param name="z">zins</param>
        /// <returns>Zins</returns>
        private static double calcZINSiterationnachschuessig(double barw, double rate,
                                                             double perioden, double endw, int step, double z)
        {
            if (step == 0)
            {
                return z;
            }
            if (z < 0) throw new InterestUnderflowException();
            if (z > 100) throw new InterestOverflowException();

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
