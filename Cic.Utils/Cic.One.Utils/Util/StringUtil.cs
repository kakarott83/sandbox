using System;
using System.Text;

namespace Cic.OpenOne.Common.Util
{
    /// <summary>
    /// StringUtil-Klasse
    /// </summary>
    [System.CLSCompliant(true)]
    public static class StringUtil
    {

        #region Methods
        /// <summary>
        /// Determines whether the specified value is trimed empty.
        /// </summary>
        /// <remarks>
        /// TESTEDBY IsTrimedEmptyTestFixture.Test1
        /// TESTEDBY IsTrimedEmptyTestFixture.Test2
        /// TESTEDBY IsTrimedEmptyTestFixture.Test3
        /// </remarks>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 	<c>true</c> if the specified value is trimed empty ; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsTrimedEmpty(string value)
        {
            return ((value != null) && (string.IsNullOrEmpty(value.Trim())));
        }

        /// <summary>
        /// Determines whether the specified value is trimed null or empty.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 	<c>true</c> if the specified value is trimed null or empty ; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// TESTEDBY IsTrimedNullOrEmptyTestFixture.NullString
        /// TESTEDBY IsTrimedNullOrEmptyTestFixture.EmptyString
        /// TESTEDBY IsTrimedNullOrEmptyTestFixture.SpaceString
        /// TESTEDBY IsTrimedNullOrEmptyTestFixture.SingleCharString
        /// TESTEDBY IsTrimedNullOrEmptyTestFixture.SpacesAndSingleCharString
        /// </remarks>
        public static bool IsTrimedNullOrEmpty(string value)
        {
            return ((string.IsNullOrEmpty(value)) || (string.IsNullOrEmpty(value.Trim())));
        }

        /// <summary>
        /// Trims the and get default if null or empty.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Default value if null or empty; otherwise trimed value.</returns>
        /// <remarks>
        /// TESTEDBY TrimAndGetDefaultIfNullOrEmptyTestFixture.NullStringDefaultNull
        /// TESTEDBY TrimAndGetDefaultIfNullOrEmptyTestFixture.EmptyStringDefaultNull
        /// TESTEDBY TrimAndGetDefaultIfNullOrEmptyTestFixture.SpaceStringDefaultNull
        /// TESTEDBY TrimAndGetDefaultIfNullOrEmptyTestFixture.NullStringDefaultNotNull
        /// TESTEDBY TrimAndGetDefaultIfNullOrEmptyTestFixture.EmptyStringDefaultNotNull
        /// TESTEDBY TrimAndGetDefaultIfNullOrEmptyTestFixture.SpaceStringDefaultNotNull
        /// TESTEDBY TrimAndGetDefaultIfNullOrEmptyTestFixture.SingleCharString
        /// TESTEDBY TrimAndGetDefaultIfNullOrEmptyTestFixture.SpacesAndSingleChar
        /// </remarks>
        public static string TrimAndGetDefaultIfNullOrEmpty(string value, string defaultValue)
        {
            // Check value
            if (IsTrimedNullOrEmpty(value))
            {
                // Set default value
                value = defaultValue;
            }
            else
            {
                // Trim
                value = value.Trim();
            }
            return value;
        }

        /// <summary>
        /// Gets the null start index substring.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="length">The length.</param>
        /// <returns>The substring.</returns>
        /// <remarks>
        /// TESTEDBY GetNullStartIndexSubstringTestFixture.NullStringZeroLength
        /// TESTEDBY GetNullStartIndexSubstringTestFixture.EmptyStringZeroLength
        /// TESTEDBY GetNullStartIndexSubstringTestFixture.SpaceAndSingleCharStringZeroLength
        /// TESTEDBY GetNullStartIndexSubstringTestFixture.NullStringBiggerZeroLength
        /// TESTEDBY GetNullStartIndexSubstringTestFixture.EmptyStringBiggerZeroLength
        /// TESTEDBY GetNullStartIndexSubstringTestFixture.SpaceAndSingleCharStringBiggerZeroLength
        /// </remarks>
        public static string GetNullStartIndexSubstring(string value, int length)
        {
            // Check value
            if ((!string.IsNullOrEmpty(value)) && (length > 0) && (value.Length > length))
            {
                // Change length
                value = value.Substring(0, length);
            }
            return value;
        }

        /// <summary>
        /// Counts the alphanumeric chars.
        /// </summary>
        /// <remarks>
        /// TESTEDBY CountAlphanumericCharsTestFixture.Test1
        /// TESTEDBY CountAlphanumericCharsTestFixture.Test2
        /// TESTEDBY CountAlphanumericCharsTestFixture.Test3
        /// </remarks>
        /// <param name="value">The value.</param>
        /// <returns>The count.</returns>
        public static int CountAlphanumericChars(string value)
        {
            int CountOfAlphanumericChars = 0;

            // Check value
            if (!string.IsNullOrEmpty(value))
            {
                // Loop through array
                foreach (char LoopChar in value.ToCharArray())
                {
                    // Check alphanumeric
                    if (char.IsLetterOrDigit(LoopChar))
                    {
                        // Increase
                        CountOfAlphanumericChars = (CountOfAlphanumericChars + 1);
                    }
                }
            }
            return CountOfAlphanumericChars;
        }

        /// <summary>
        /// ToHexString
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToHexString(string value)
        {
            System.Text.StringBuilder StringBuilder;
            byte[] ValueBytes;
            string Result = null;

            // Check value
            if (value != null)
            {
                // Set default
                Result = string.Empty;

                // Check value
                if (!string.IsNullOrEmpty(value))
                {
                    // New string builder
                    StringBuilder = new System.Text.StringBuilder();
                    // Get byte array
                    ValueBytes = System.Text.Encoding.UTF8.GetBytes(value);
                    // Loop through byte array
                    foreach (byte b in ValueBytes)
                    {
                        // Append
                        StringBuilder.Append(string.Format("{0:x2}", b));
                    }
                    // Result
                    Result = StringBuilder.ToString();
                }
            }
            return Result;
        }

        /// <summary>
        /// Converts value from Hex to number
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FromHexString(string value)
        {
            System.Text.StringBuilder StringBuilder;
            string Result = null;

            // Check value
            if (value != null)
            {
                // Set default
                Result = string.Empty;

                // Check value
                if (!string.IsNullOrEmpty(value))
                {
                    // New string builder
                    StringBuilder = new System.Text.StringBuilder();
                    // Loop through string
                    for (int i = 0; i <= value.Length - 2; i += 2)
                    {
                        try
                        {
                            // Parse, convert and appen
                            StringBuilder.Append(System.Convert.ToString(System.Convert.ToChar(System.Int32.Parse(value.Substring(i, 2), System.Globalization.NumberStyles.HexNumber))));
                        }
                        catch
                        {
                            // Throw caught exception
                        }
                    }
                    // Set result
                    Result = StringBuilder.ToString();
                }
            }
            return Result;
        }

        /// <summary>
        /// Encodes a string with a escape char as meta char for encoded specialChars.
        /// </summary>
        /// <remarks>
        /// TESTEDBY CodeSpecialCharsTestFixture.EncodeWithNullEncoding
        /// TESTEDBY CodeSpecialCharsTestFixture.EncodeAndDecodeWithDifferentEncodings
        /// </remarks>
        /// <param name="value">The string to be encoded</param>
        /// <param name="escapeChar">The escape chat</param>
        /// <param name="specialChars">The special chars which will be encoded</param>
        /// <param name="encoding">The encoding used to encode the value</param>
        /// <returns>The encoded string</returns>
        public static string EncodeSpecialChars(string value, char escapeChar, char[] specialChars, System.Text.Encoding encoding)
        {
            try
            {
                // Return internal
                return MyEncodeSpecialChars(value, escapeChar, specialChars, encoding);
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }

        /// <summary>
        /// Encodes a string with a escape char as meta char for encoded specialChars.
        /// </summary>
        /// <remarks>
        /// TESTEDBY CodeSpecialCharsTestFixture.EncodeWithNullSpecialChars
        /// TESTEDBY CodeSpecialCharsTestFixture.EncodeSpecialCharsContainsEscapeChar
        /// TESTEDBY CodeSpecialCharsTestFixture.EncodeAndDecodeWithNullValue
        /// TESTEDBY CodeSpecialCharsTestFixture.EncodeAndDecodeWithEmptyValue
        /// TESTEDBY CodeSpecialCharsTestFixture.EncodeAndDecode
        /// TESTEDBY CodeSpecialCharsTestFixture.EncodeAndDecodeWithEmptySpecialChars
        /// TESTEDBY CodeSpecialCharsTestFixture.EncodeAndDecodeWithRepetition
        /// </remarks>
        /// <param name="value">The string to be encoded</param>
        /// <param name="escapeChar">The escape char</param>
        /// <param name="specialChars">The special chars which will be encoded</param>
        /// <returns>tThe encoded string</returns>
        public static string EncodeSpecialChars(string value, char escapeChar, char[] specialChars)
        {
            try
            {
                // Return internal
                return MyEncodeSpecialChars(value, escapeChar, specialChars, System.Text.Encoding.UTF8);
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }

        /// <summary>
        /// Decodes a string with a escape char as meta char.
        /// </summary>
        /// <remarks>
        /// TESTEDBY CodeSpecialCharsTestFixture.DecodeWithNullEncoding
        /// TESTEDBY CodeSpecialCharsTestFixture.EncodeAndDecodeWithDifferentEncodings
        /// </remarks>
        /// <param name="value">The encoded string</param>
        /// <param name="escapeChar">The escape char</param>
        /// <param name="encoding">the encoding used to decode the value</param>
        /// <returns>The decoded string</returns>
        public static string DecodeSpecialChars(string value, char escapeChar, System.Text.Encoding encoding)
        {
            try
            {
                // Return internal
                return MyDecodeSpecialChars(value, escapeChar, encoding);
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }

        /// <summary>
        /// Decodes a string with a escape char as meta char.
        /// </summary>
        /// <remarks>
        /// TESTEDBY CodeSpecialCharsTestFixture.EncodeAndDecodeWithNullValue
        /// TESTEDBY CodeSpecialCharsTestFixture.EncodeAndDecodeWithEmptyValue
        /// TESTEDBY CodeSpecialCharsTestFixture.EncodeAndDecode
        /// TESTEDBY CodeSpecialCharsTestFixture.EncodeAndDecodeWithEmptySpecialChars
        /// TESTEDBY CodeSpecialCharsTestFixture.EncodeAndDecodeWithRepetition
        /// </remarks>
        /// <param name="value">The encoded string</param>
        /// <param name="escapeChar">The escape char</param>
        /// <returns>The decoded string</returns>
        public static string DecodeSpecialChars(string value, char escapeChar)
        {
            try
            {
                // Return internal
                return MyDecodeSpecialChars(value, escapeChar, System.Text.Encoding.UTF8);
            }
            catch
            {
                // Throw caught exception
                throw;
            }
        }
        #endregion

        #region My methods
        /// <summary>
        /// Encodes a string with a escape char as meta char for encoded specialChars.
        /// </summary>
        /// <param name="value">The string to be encoded</param>
        /// <param name="escapeChar">The escape char</param>
        /// <param name="specialChars">The special Chars which will be encoded</param>
        /// <param name="encoding">The encoding used to encode the value</param>
        /// <returns>The encoded string.</returns>
        private static string MyEncodeSpecialChars(string value, char escapeChar, char[] specialChars, System.Text.Encoding encoding)
        {
            System.Text.StringBuilder StringBuilder;
            bool IsSpecial = false;
            string SpecialCharsString;
            string Result = null;
            string Append;
            byte[] EncodedCharAsByteArray;

            // Check value
            if (value != null)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    // Check special chars
                    if (specialChars == null)
                    {
                        throw new ArgumentNullException("specialChars");
                    }

                    // Set string
                    SpecialCharsString = new string(specialChars);

                    // Check containing
                    if (SpecialCharsString.Contains(escapeChar.ToString()))
                    {
                        throw new System.ApplicationException("The escapeChar char is member of the special chars.");
                    }

                    // Check encoding
                    if (encoding == null)
                    {
                        throw new ArgumentNullException("encoding");
                    }

                    // New string builder
                    StringBuilder = new System.Text.StringBuilder();

                    // Encoding
                    foreach (char LoopChar in value)
                    {
                        if (LoopChar == escapeChar)
                        {
                            // Append escapeChar twice
                            StringBuilder.Append(escapeChar, 2);
                        }
                        else
                        {
                            // Check containing
                            IsSpecial = SpecialCharsString.Contains(LoopChar.ToString());
                            // Check state
                            if (IsSpecial)
                            {
                                StringBuilder.Append(escapeChar);
                                // Convert LoopChar to a hexadecimal string
                                EncodedCharAsByteArray = encoding.GetBytes(new char[] { LoopChar });
                                foreach (byte LoopByte in EncodedCharAsByteArray)
                                {
                                    Append = System.Convert.ToString(LoopByte, 16);
                                    if (Append.Length < 2)
                                    {
                                        Append = "0" + Append;
                                    }
                                    StringBuilder.Append(Append);
                                }
                                StringBuilder.Append(escapeChar);
                            }
                            else
                            {
                                StringBuilder.Append(LoopChar);
                            }
                        }
                    }
                    // Set result
                    Result = StringBuilder.ToString();
                }
                else
                {
                    Result = string.Empty;
                }
            }
            return Result;
        }

        /// <summary>
        /// Decodes a string with a escape char as meta char.
        /// </summary>
        /// <param name="value">the string to be decoded</param>
        /// <param name="escapeChar">The escape char</param>
        /// <param name="encoding">The encoding used for decode</param>
        /// <returns>The decoded string</returns>
        private static string MyDecodeSpecialChars(string value, char escapeChar, System.Text.Encoding encoding)
        {
            System.Text.StringBuilder StringBuilder;
            string Result = null;
            int CharLength;
            char LoopChar;
            byte[] EncodedCharsAsByteArray;
            string Substring;

            // Check value
            if (value != null)
            {
                Result = string.Empty;

                if (!string.IsNullOrEmpty(value))
                {
                    // Check encoding
                    if (encoding == null)
                    {
                        throw new ArgumentNullException("encoding");
                    }

                    // New string builder
                    StringBuilder = new System.Text.StringBuilder();

                    // Decode
                    for (int Index = 0; Index < value.Length; Index++)
                    {
                        LoopChar = value[Index];
                        if (LoopChar == escapeChar)
                        {
                            if (value[Index + 1] == escapeChar)
                            {
                                StringBuilder.Append(escapeChar);
                                // Skip one char because its already read
                                Index++;
                            }
                            else
                            {
                                CharLength = 0;
                                do
                                {
                                    CharLength += 2;
                                }
                                while (value[Index + CharLength + 1] != escapeChar);

                                EncodedCharsAsByteArray = new byte[CharLength / 2];

                                for (int LoopCharLenght = 0; LoopCharLenght < CharLength; LoopCharLenght += 2)
                                {
                                    Substring = value.Substring(Index + LoopCharLenght + 1, 2);
                                    EncodedCharsAsByteArray[LoopCharLenght / 2] = System.Convert.ToByte(value.Substring(Index + LoopCharLenght + 1, 2), 16);
                                }

                                //Convert hexadecimal string to char
                                StringBuilder.Append(encoding.GetChars(EncodedCharsAsByteArray));
                                // Skip two chars because its already read
                                Index += CharLength + 1;
                            }
                        }
                        else
                        {
                            StringBuilder.Append(LoopChar);
                        }
                    }
                    Result = StringBuilder.ToString();
                }
            }
            return Result;
        }
        #endregion
    }
}