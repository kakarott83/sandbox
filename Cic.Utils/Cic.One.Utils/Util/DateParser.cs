using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Cic.One.Utils.Util
{
    /// <summary>
    /// DateParser which allows to parse Strings to a valid Date
    /// based on http://archive.msdn.microsoft.com/Project/Download/FileDownload.aspx?ProjectName=clrsamples&DownloadId=14861
    /// </summary>
    public class DateParser
    {


        private string decimalSeparator;
        private string amDesignator, pmDesignator, aDesignator, pDesignator;
        private string pattern;

      

        private string[] numberFormats = { "C", "D", "E", "e", "F", "G", "N", "P", "R", "X", "x" };
        private const int DEFAULTSELECTION = 5;
        private string[] dateFormats = { "g", "d", "D", "f", "F", "g", "G", "M", "O", "R", "s", 
                                       "t", "T", "u", "U", "Y" };


        public DateParser()
        {
            init();
        }
        private void init()
        {


            // Get decimal separator.
            decimalSeparator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;

            // Get am, pm designators.
            amDesignator = DateTimeFormatInfo.CurrentInfo.AMDesignator;
            if (amDesignator.Length >= 1)
                aDesignator = amDesignator.Substring(0, 1);
            else
                aDesignator = String.Empty;

            pmDesignator = DateTimeFormatInfo.CurrentInfo.PMDesignator;
            if (pmDesignator.Length >= 1)
                pDesignator = pmDesignator.Substring(0, 1);
            else
                pDesignator = String.Empty;

            // For regex pattern for date and time components.
            pattern = @"^\s*\S+\s+\S+\s+\S+(\s+\S+)?(?<!" + amDesignator + "|" +
                      aDesignator + "|" + pmDesignator + "|" + pDesignator + @")\s*$";


        }


        /// <summary>
        /// parses the given String as DateTime
        /// </summary>
        /// <param name="value"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static DateTime? parseDate(String value, CultureInfo culture)
        {

            if (culture == null)
                culture = CultureInfo.InvariantCulture;
            DateParser p = new DateParser();


            //culture = CultureInfo.CreateSpecificCulture("de-DE");

            // Parse string as date

            DateTime dat = DateTime.MinValue;
            DateTimeOffset dto = DateTimeOffset.MinValue;
            long ticks;


            // Is the date a number expressed in ticks?
            if (Int64.TryParse(value, out ticks))
            {
                dat = new DateTime(ticks);
                return dat;
            }
            else
            {
                // Does the date have three components (date, time offset), or fewer than 3?
                if (Regex.IsMatch(value, p.pattern, RegexOptions.IgnoreCase))
                {
                    if (DateTimeOffset.TryParse(value, out dto))
                    {
                        return dto.DateTime;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    // The string is to be interpeted as a DateTime, not a DateTimeOffset.
                    if (DateTime.TryParse(value, out dat))
                    {
                        return dat;
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            /*  else {
                 // Handle formatting of a number.
                 long intToFormat;
                 BigInteger bigintToFormat = BigInteger.Zero;
                 double floatToFormat;

                 // Format a floating point value.
                 if (Value.Text.Contains(decimalSeparator)) {
                       try {
                          if (! Double.TryParse(Value.Text, out floatToFormat))
                             label.Text = rm.GetString("MSG_InvalidFloat");
                          else
                             this.Result.Text = floatToFormat.ToString(this.FormatStrings.Text, culture);
                       }
                       catch (FormatException) {
                          label.Text = rm.GetString("MSG_InvalidFormat");
                          this.formatInfo = true;
                       }
                 }
                 else {
                    // Handle formatting an integer.
                    //
                    // Determine whether value is out of range of an Int64
                    if (! BigInteger.TryParse(Value.Text, out bigintToFormat)) {
                       label.Text = rm.GetString("MSG_InvalidInteger");
                    }
                    else {
                       // Format an Int64
                       if (bigintToFormat >= Int64.MinValue && bigintToFormat <= Int64.MaxValue) { 
                          intToFormat = (long) bigintToFormat;
                          try {
                             this.Result.Text = intToFormat.ToString(this.FormatStrings.Text, culture);
                          }
                          catch (FormatException) {
                             label.Text = rm.GetString("MSG_InvalidFormat");
                             this.formatInfo = true;
                          }
                       }
                       else {
                          // Format a BigInteger
                          try {
                             this.Result.Text = bigintToFormat.ToString(this.FormatStrings.Text, culture);
                          }
                          catch (FormatException) {
                             label.Text = rm.GetString("MSG_InvalidFormat");
                             this.formatInfo = true;
                          }
                       }
                    }
                 }
              }*/
        }


    }
}
