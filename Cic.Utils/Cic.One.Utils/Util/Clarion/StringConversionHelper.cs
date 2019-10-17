using System;
using System.Collections.Generic;
using System.Text;

namespace Cic.OpenOne.Common.Util
{
    /// <summary>
    /// Handles Clarion Strings
    /// </summary>
    public static class StringConversionHelper
    {
        #region

        /// <summary>
        /// StringToByte
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static byte[] StringToByte(string text)
        {
            if (text == null)
            {
                return null;
            }

            System.Text.ASCIIEncoding ASCIIEncoding = new System.Text.ASCIIEncoding();
            return ASCIIEncoding.GetBytes(text);
        }

        /// <summary>
        /// ByteToString
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ByteToString(byte[] text)
        {
            if (text == null)
            {
                return null;
            }
            System.Text.ASCIIEncoding ASCIIEncoding = new System.Text.ASCIIEncoding();
            return ASCIIEncoding.GetString(text);
        }

        /// <summary>
        /// Converts a Clarion DB-Field fetched from DB-Query (which is assumed to be ISO-8859-1 to a correct UTF-8-Encoding (e.g. ASCII 180 is then ceb4 instead of b4)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String convertClarionISOToUTF8(String text)
        {
            if (text == null) return null;
            return System.Text.Encoding.Default.GetString(Encoding.Convert(Encoding.GetEncoding("ISO-8859-1"), Encoding.UTF8, Encoding.Default.GetBytes(text)));
        }

        /// <summary>
        /// StringToClarionByte
        /// </summary>
        /// <param name="utf8text"></param>
        /// <returns></returns>
        public static byte[] StringToClarionByte(string utf8text)
        {
            if (utf8text == null)
            {
                return null;
            }

            //ugly hack for clarion :(
            int appendlength = 2000 - utf8text.Length;

            StringBuilder sb = new StringBuilder();
            if (appendlength > 0)
                for (int i = 0; i < appendlength; i++)
                    sb.Append(" ");
            utf8text = utf8text + sb.ToString();
            if (utf8text.Length > 2000)
                utf8text = utf8text.Substring(0, 2000);

            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            Encoding utf8 = Encoding.UTF8;

            byte[] utfBytes = utf8.GetBytes(utf8text);
            byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
            List<byte> rval = new List<byte>();
            //rval.AddRange(TwoBytesFromShort((short)(isoBytes.Length+2)));
            rval.AddRange(isoBytes);

            return rval.ToArray();
        }

        /// <summary>
        /// StringToClarionByte
        /// </summary>
        /// <param name="utf8text"></param>
        /// <param name="maxByteCount"></param>
        /// <returns></returns>
        public static byte[] StringToClarionByte(string utf8text, int maxByteCount)
        {
            if (utf8text == null)
            {
                return null;
            }

            //ugly hack for clarion :(
            int appendlength = maxByteCount - utf8text.Length;

            StringBuilder sb = new StringBuilder();
            if (appendlength > 0)
                for (int i = 0; i < appendlength; i++)
                    sb.Append(" ");
            utf8text = utf8text + sb.ToString();
            if (utf8text.Length > maxByteCount)
                utf8text = utf8text.Substring(0, maxByteCount);

            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            Encoding utf8 = Encoding.UTF8;

            byte[] utfBytes = utf8.GetBytes(utf8text);
            byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
            List<byte> rval = new List<byte>();

            rval.AddRange(isoBytes);

            return rval.ToArray();
        }

        /// <summary>
        /// ClarionByteToString
        /// </summary>
        /// <param name="isodata"></param>
        /// <returns></returns>
        public static string ClarionByteToString(byte[] isodata)
        {
            if (isodata == null)
            {
                return null;
            }
            List<byte> rval = new List<byte>();
            rval.AddRange(isodata);
            //List<byte> length = rval.GetRange(0, 2);
            //rval.RemoveRange(0, 2);

            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            Encoding utf8 = Encoding.UTF8;

            byte[] utf8bytes = Encoding.Convert(iso, utf8, rval.ToArray());
            string val = utf8.GetString(utf8bytes);
            if (val != null)
                return val.Trim();
            return null;
        }

        private static byte[] TwoBytesFromShort(short s)
        {
            byte[] b = BitConverter.GetBytes(s);
            Array.Reverse(b);
            return b;
        }

        #endregion
    }
}