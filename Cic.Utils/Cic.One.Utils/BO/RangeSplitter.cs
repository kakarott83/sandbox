using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.Utils.BO
{

    /// <summary>
    /// Manages Splittings of payment ranges
    /// a linear payment plan has a range of 1-lz
    /// a special payment plan has a range of 1,2,3-12,13....,lz
    /// 
    /// the ranges can be split or assigned to a payment list to calculate/change the payments
    /// 
    /// this class does not manage the ranges! the different ranges should be created during the usage of this class by creating
    /// a string with x-y, comma separated for all different ranges of equal payments
    /// 
    /// Usage:
    /// rs = new RangeSplitter(block,range, zw, lz, vorschuessig)
    ///    where block are all currently available staffel x-y, comma-separated, e.g. 1-24,25-36
    ///    range is the staffel x-y to currently work on e.g. 25-36
    ///    then call rs.splitXY() returns the new range (a call to rs.getRange() will deliver a list of rate#s for this range)
    ///    in the StaffelKalkulator, set calcRange with the new range and calculate
    ///        e.g. sk.calcRATEN(double barwert) to calculate the raten for the new range 
    ///        or   set change the raten with sk.raten[all indizes of rs.getRange()] to the gui-values and then call sk.calcBARWERT() to get the new barwert
    /// </summary>
    public class RangeSplitter
    {

        // Split-Modes
        private static int CENTER = 0;
        private static int UPPER = -1;
        private static int LOWER = -2;
        private String nrange;
        private String calcRange = null;
        private int erate, lrate;
        private int zw, lz;
        private bool zahlmodus;

        /// <summary>
        /// Create a Splitter for the current ranges
        /// 
        /// </summary>
        /// <param name="ranges">all current ranges, comma-separated</param>
        /// <param name="range">the range to currently work on</param>
        /// <param name="zw">zahlweise</param>
        /// <param name="lz">laufzeit</param>
        /// <param name="zahlmodus">vor/nachschüssig</param>
        public RangeSplitter(String ranges, String range, int zw, int lz, bool zahlmodus)
        {
            this.zw = zw;
            this.zahlmodus = zahlmodus;
            this.lz = lz;

            nrange = ranges.Substring(0, ranges.IndexOf(range));
            if (ranges.Length > ranges.IndexOf(range) + range.Length + 1)
            {
                nrange += ranges.Substring(ranges.IndexOf(range) + range.Length + 1);
            }

            nrange = nrange.Replace(",,", ",");
            if (nrange.StartsWith(","))
            {
                nrange = nrange.Substring(1);
            }

            if (nrange.EndsWith(","))
            {
                nrange = nrange.Substring(0, nrange.Length - 1);
            }

            if (range.IndexOf("-") > -1)
            {
                erate = int.Parse(range.Substring(0, range.IndexOf("-")));
                lrate = int.Parse(range.Substring(range.IndexOf("-") + 1));
            }
            else
            {
                erate = int.Parse(range);
                lrate = erate;
            }
        }

        public RangeSplitter(String range, int zw, int lz, bool zahlmodus)
        {
            this.zw = zw;
            this.lz = lz;
            this.zahlmodus = zahlmodus;

            nrange = range;
            if (range.IndexOf("-") > -1)
            {
                erate = int.Parse(range.Substring(0, range.IndexOf("-")));
                lrate = int.Parse(range.Substring(range.IndexOf("-") + 1));
            }
            else
            {
                erate = int.Parse(range);
                lrate = erate;
            }
        }

        /// <summary>
        /// the first payment for this range (e.g. 5-36 returns 5)
        /// </summary>
        /// <returns></returns>
        public int getErsteRate()
        {
            return erate;
        }

        /// <summary>
        /// adds an amount to all payments of this range
        /// </summary>
        /// <param name="diff"></param>
        /// <param name="raten"></param>
        public void addDifference(double diff, List<Double> raten)
        {
            for (int i = erate; i <= lrate; i ++)
            {
                if (isValidRateIndex(i))
                {
                    double value = raten[i - 1];
                    raten[i - 1] = value + diff;
                }
            }
        }
        /// <summary>
        /// True if the 1-based offset is a valid rate in this range
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        private bool isValidRateIndex(int targetRateAbsolute)
        {
             
             for(int i=1; i<=lrate; i+=zw)
             {
                 if (i == targetRateAbsolute)
                     return true;
             }
             return false;
        }
        /// <summary>
        /// zeros all values of raten contained in this range
        /// </summary>
        /// <param name="raten"></param>
        public void zero(List<Double> raten)
        {
            for (int i = erate; i <= lrate; i++)
            {
                raten[i - 1] = 0;
            }
        }

        /// <summary>
        /// returns the number of payments for this range
        /// </summary>
        /// <returns></returns>
        public int getRatenCount()
        {
            return getRange().Count;
        }

        /// <summary>
        /// determines if the range is splittable
        /// </summary>
        /// <returns></returns>
        public bool isSplittable()
        {
            if (getRange().Count > 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// assigns the value to all payments of this range
        /// </summary>
        /// <param name="raten">all payments</param>
        /// <param name="val"></param>
        public void setValues(List<double> raten, Double val)
        {

            if (erate == lrate)
            {
                if (raten.Count > (erate - 1))
                {
                    raten[erate - 1] = val;
                }
                return;
            }
            int start = erate;
            if (!zahlmodus)
            {
                start += (zw - 1);
            }

            for (int i = start; i <= lrate; i += zw)
            {
                if (raten.Count > (i - 1))
                {
                    raten[i - 1] = val;
                }
            }
        }
        

        /// <summary>
        /// returns the range to recalc after a split
        /// </summary>
        /// <returns></returns>
        public String getCalcRange()
        {
            return calcRange;
        }

        /// <summary>
        /// splits the range according to all payment parameters at top, middle or bottom and returns the new range
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public String splitCenter(int mode)
        {
            //int middle = (int)Math.floor((lrate-erate)/2.0);
            List<int> valids = getRange();

            int middle = valids.Count / 2;
            //if(mode==CENTER)
            //  if(valids.size()%2!=0 && middle>1) middle++;
            if (mode == UPPER)
            {
                middle = 0;
            }
            else if (mode == LOWER)
            {
                middle = valids.Count - 1;
            }

            if (valids.Count == 0)
            {
                return "";
            }

            if (mode > 0)
            {
                for (int u = 0; u < valids.Count; u++)
                {
                    middle = u;
                    if (valids[u] > mode)
                        break;
                }
            }

            if (middle < 0) middle = 0;

            calcRange = "" + valids[middle];

            StringBuilder rval = new StringBuilder();



            if (middle > 0)
            {
                rval.Append(valids[0]);



                if (middle > 1)
                {
                    rval.Append("-");
                    rval.Append(valids[middle] - 1);
                }
            }
            else
            {
                rval.Append(valids[0]);
                middle++;
            }

            if (valids.Count > (middle))
            {
                if (mode == CENTER)
                {
                    calcRange = rval.ToString();
                }

                rval.Append(",");
                rval.Append(valids[middle]);

                if (valids.Count > (middle + 1))
                {
                    rval.Append("-");
                    rval.Append(valids[valids.Count - 1]);
                }
            }
            return nrange + ((nrange.Length > 0) ? "," : "") + rval.ToString();
        }

        public String splitCenter()
        {
            return splitCenter(CENTER);
        }

        public String splitUpper()
        {
            return splitCenter(UPPER);
        }

        public String splitLower()
        {
            return splitCenter(LOWER);
        }

        /// <summary>
        /// returns the range in form #ratenummer;#ratenummer
        /// </summary>
        /// <returns></returns>
        public String printRange()
        {
            if (zw == 1)
            {
                if (erate == lrate)
                {
                    return erate.ToString();
                }
                else
                {
                    return erate + "-" + lrate;
                }
            }

            List<int> raten = getRange();
            String blocks = "";
            foreach (int rate in raten)
            {
                if (blocks.Length > 0)
                {
                    blocks += ";";
                }
                blocks += rate;
            }

            return blocks.ToString();
        }

        /// <summary>
        /// returns the range as list containing the number of the payments, 1-based
        /// </summary>
        /// <returns></returns>
        public List<int> getRaten()
        {
            List<int> rval = new List<int>();

            if (zw == 1)
            {
                if (erate == lrate)
                {
                    rval.Add(erate);
                    return rval;
                }
            }


            List<int> raten = getRange();

            foreach (int rate in raten)
            {
                rval.Add((rate));
            }

            return rval;
        }

        /// <summary>
        /// returns the payments based on the given settings for this range
        /// </summary>
        /// <param name="erate"></param>
        /// <param name="lrate"></param>
        /// <param name="lz"></param>
        /// <param name="zw"></param>
        /// <param name="zahlmodus"></param>
        /// <returns></returns>
        public static List<int> getRaten(int erate, int lrate, int lz, int zw, bool zahlmodus)
        {
            List<int> rval = new List<int>();
            for (int i = 1; i <= lz; i++)
            {
                if (((i - 1) % zw == 0 && zahlmodus) || ((i - 1) % zw == (zw - 1) && !zahlmodus))
                {
                    if (i >= erate && i <= lrate)
                    {
                        rval.Add(i);
                    }
                }
            }
            return rval;
        }

        private List<int> getRange()
        {
            return getRaten(erate, lrate, lz, zw, zahlmodus);

        }

        public static void main(String[] args)
        {

            /*RangeSplitter rs = new RangeSplitter("13-36");
            List raten = new ArrayList();
            for(int i=0; i<36; i++)
            raten.add(new Double(0.0));
            rs.setValues(raten,null,12,false);
            Console.WriteLine(raten);
             */

            Console.WriteLine("A");
            RangeSplitter rs = new RangeSplitter("1-24", "1-24", 12, 24, true);
            Console.WriteLine(rs.splitCenter());
            Console.WriteLine(rs.getCalcRange());
            Console.WriteLine(rs.splitUpper());
            Console.WriteLine(rs.getCalcRange());
            Console.WriteLine(rs.printRange());

            Console.WriteLine(rs.splitLower());
            Console.WriteLine(rs.getCalcRange());

            Console.WriteLine("B");
            rs = new RangeSplitter("1-12,13-24", "1-12", 12, 24, true);
            Console.WriteLine(rs.splitCenter());
            Console.WriteLine(rs.getCalcRange());
            Console.WriteLine(rs.splitUpper());
            Console.WriteLine(rs.getCalcRange());
            Console.WriteLine(rs.splitLower());
            Console.WriteLine(rs.getCalcRange());

            Console.WriteLine("C");
            rs = new RangeSplitter("1-12,13-14,15-24", "13-14", 12, 24, true);
            Console.WriteLine(rs.splitCenter());
            Console.WriteLine(rs.getCalcRange());
            Console.WriteLine(rs.splitUpper());
            Console.WriteLine(rs.getCalcRange());
            Console.WriteLine(rs.splitLower());
            Console.WriteLine(rs.getCalcRange());

            Console.WriteLine("D");
            rs = new RangeSplitter("1-2,3-5,6-16", "3-5", 1, 16, true);
            Console.WriteLine(rs.splitCenter());
            Console.WriteLine(rs.getCalcRange());
            Console.WriteLine(rs.splitUpper());
            Console.WriteLine(rs.getCalcRange());
            Console.WriteLine(rs.splitLower());
            Console.WriteLine(rs.getCalcRange());
            Console.WriteLine(rs.getRaten());

            Console.WriteLine("E");
            Console.WriteLine(RangeSplitter.getRaten(1, 36, 36, 6, false));
            rs = new RangeSplitter("1,7-36", "7-36", 6, 36, false);
            Console.WriteLine(rs.splitCenter());
            Console.WriteLine(rs.getCalcRange());
            Console.WriteLine(rs.splitUpper());
            Console.WriteLine(rs.getCalcRange());
            Console.WriteLine(rs.splitLower());
            Console.WriteLine(rs.getCalcRange());
            Console.WriteLine(rs.splitCenter(24));
            Console.WriteLine(rs.getRaten());

            Console.WriteLine("F");
            rs = new RangeSplitter("19-31", 1, 36, false);
            Console.WriteLine(rs.printRange());
            Console.WriteLine(String.Join(";",rs.getRaten()));

        }
    }
}
