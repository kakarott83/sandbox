using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.Utils.BO
{
    /// <summary>
    /// Types to round
    /// </summary>
    public enum RoundType
    {
        //GERMAN Kaufmännisch
        KFM2,
        //Schweizer 5-Rappen Rundung
        CHF
    }

    /// <summary>
    /// Kalkulationsmethoden
    /// </summary>
    public enum CalcMethod
    {
        //Tagesgenau
        M_ACT_ACT,
        //30 Tage pro Monat
        M_30_360,
        //Eurozins
        M_ACT_360,
        //english Method
        M_ACT_365
    }

    /// <summary>
    /// Hilfsmethoden für den Staffelkalkulator
    /// </summary>
    public class CalcUtil
    {
        public static double calcPerioden(DateTime vonDatum, DateTime bisDatum, int zahlweise)
        {

            int diff = 0;

            if (vonDatum != null && bisDatum != null)
            {


                int x = vonDatum.Day;
                int faellig_alt = 12 * vonDatum.Year + vonDatum.Month;


                int y = bisDatum.Day;
                if (x > y)
                {
                    faellig_alt++;
                }
                int faellig_neu = 12 * bisDatum.Year + bisDatum.Month;

                diff = faellig_neu - faellig_alt;
            }

            return Math.Floor((double)(diff / zahlweise));

        }

        /// <summary>
        /// Calculates the zins factor for the given date difference and method
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="includeStart"></param>
        /// <param name="includeEnd"></param>
        /// <param name="methode"></param>
        /// <returns></returns>
        public static double calcZinsTageFaktor(DateTime startDate, DateTime endDate, bool includeStart, bool includeEnd, CalcMethod methode)
        {
            if (startDate == null || endDate == null) throw new Exception("calcZinsTage called without two dates");

            double rval = 0;

            if (methode == CalcMethod.M_30_360)// ein Monat hat 30 Tage
            {

                int faellig_alt = 12 * startDate.Year + startDate.Month;
                int startmonatTage = startDate.Day;

                if (startmonatTage == DateTime.DaysInMonth(startDate.Year, startDate.Month))
                {
                    startmonatTage = 30;  // 30/360 Regel
                }
                int faellig_neu = 12 * endDate.Year + endDate.Month;
                int endmonatTage = endDate.Day;
                
                if (endmonatTage == DateTime.DaysInMonth(endDate.Year, endDate.Month))
                {
                    if (endDate.Month == 2)//endet die Finanzierung im Februar, so nur die Tage des Februars verwenden
                        endmonatTage = DateTime.DaysInMonth(endDate.Year, endDate.Month);
                    else endmonatTage = 30;  // 30/360 Regel
                }
                rval = 30 * (faellig_neu - faellig_alt) + endmonatTage - startmonatTage;
                if (includeStart) rval++;
                if (!includeEnd) rval--;

               /* if (rval == 30 && ((includeEnd && !includeStart) || (!includeEnd && includeStart)))
                {
                    rval = 29;//as the 30-days/Month rule has only 30 days when start or end is not included we have to subtract one
                }*/

                return rval / 360.0;
            }
            else 
            {
                int stdDays = 0;
                int leapDays = 0;
                
                if (startDate.Year != endDate.Year)
                {
                    int startYearDays = 0;
                    int endYearDays = 0;

                    startYearDays = -1* (int)(startDate.Date - new DateTime(startDate.Year, 12, 31).Date).TotalDays;
                    if (includeStart) startYearDays++;
                    if (DateTime.IsLeapYear(startDate.Year))
                        leapDays += startYearDays;
                    else
                        stdDays += startYearDays;


                    endYearDays = (int)(endDate.Date - new DateTime(endDate.Year, 01, 01).Date).TotalDays + 1;
                    if (!includeEnd) endYearDays--;
                    if (DateTime.IsLeapYear(endDate.Year))
                        leapDays += endYearDays;
                    else
                        stdDays += endYearDays;
                    
                    for (int i = 0; i < ((endDate.Year) - (startDate.Year + 1)); i++)
                    {
                        int testYear = i + startDate.Year + 1;
                        if (DateTime.IsLeapYear(testYear))
                            leapDays += 366;
                        else stdDays += 365;
                    }

                   
                }
                else //start and end in same year;
                {
                   
                    int days = -1 * (int)(startDate.Date - endDate.Date).TotalDays;
                    if (DateTime.IsLeapYear(startDate.Year))
                        leapDays = days;
                    else stdDays = days;
                }
                //Console.WriteLine(stdDays + " / " + leapDays + "=" + (stdDays + leapDays));
                if (methode == CalcMethod.M_ACT_360)
                    return (stdDays + leapDays) / 360.0;
                else if (methode == CalcMethod.M_ACT_365)
                    return (stdDays + leapDays) / 365.0;
                else
                    return (stdDays / 365.0) + (leapDays / 366.0);
                
            }
            

        }

        /// <summary>
        /// Calculates the Zins Days
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="includeStart"></param>
        /// <param name="includeEnd"></param>
        /// <param name="methode"></param>
        /// <returns></returns>
        public static int calcZinsTage(DateTime startDate, DateTime endDate, bool includeStart, bool includeEnd, CalcMethod methode)
        {
            if (startDate == null || endDate == null) throw new Exception("calcZinsTage called without two dates");
            
            int rval = 0;

            if (methode == CalcMethod.M_30_360)// ein Monat hat 30 Tage
            {   

                int faellig_alt = 12 * startDate.Year + startDate.Month;
                int startmonatTage = startDate.Day;

                if (startmonatTage == DateTime.DaysInMonth(startDate.Year, startDate.Month))
                {
                    startmonatTage = 30;  // 30/360 Regel
                }
                int faellig_neu = 12 * endDate.Year + endDate.Month;
                int endmonatTage = endDate.Day;
                if (endmonatTage == DateTime.DaysInMonth(endDate.Year, endDate.Month))
                {
                    if (endDate.Month == 2)//endet die Finanzierung im Februar, so nur die Tage des Februars verwenden
                        endmonatTage = DateTime.DaysInMonth(endDate.Year, endDate.Month);
                    else endmonatTage = 30;  // 30/360 Regel
                }
                rval = 30 * (faellig_neu - faellig_alt) + endmonatTage - startmonatTage;
                /*if (rval == 30 && ((includeEnd && !includeStart) || (!includeEnd && includeStart)))
                {
                    rval = 29;//as the 30-days/Month rule has only 30 days when start or end is not included we have to subtract one
                }*/
            }
            else if (methode == CalcMethod.M_ACT_ACT || methode == CalcMethod.M_ACT_360 || methode == CalcMethod.M_ACT_365)
            {  
                rval = -1*(int)(startDate.Date - endDate.Date).TotalDays;
            }
            if (includeStart) rval++;
            if (!includeEnd) rval--;
            

            return rval;
        }

      


        public static double round(double d, RoundType roundtype, long fractiondigits)
        {

            if (roundtype == RoundType.KFM2)
            {
                return roundNachkomma(d, fractiondigits);
            }
            else if (roundtype == RoundType.CHF)
            {
                double dabs = Math.Abs(d);
                double dsgn = 1.0;
                if (d < 0)
                {
                    dsgn = -1.0;
                }
                long x = ((int)Math.Round((dabs * 100.0)) % 10);
                if (x >= 0 && x <= 2)
                {
                    dabs = roundNachkomma(dabs - 0.01 * x, fractiondigits);
                }
                else if (x == 8 || x == 9)
                {
                    dabs = roundNachkomma(dabs + 0.1 - 0.01 * x, fractiondigits);
                }
                else
                {
                    dabs = roundNachkomma(dabs + 0.05 - 0.01 * x, fractiondigits);
                }
                return dsgn * dabs;
            }
            return d;
        }
        public static double roundNachkomma(double d, long stellen)
        {
            double basis = Math.Pow(10.0, stellen);
            long x = (long)(Math.Abs(d) * basis + 0.5);
            double result = ((double)x) / basis;
            if (result == 0)
            {
                return 0.0;
            }

            if (d < 0)
            {
                return -1 * result;
            }
            return result;
        }

    }
}
