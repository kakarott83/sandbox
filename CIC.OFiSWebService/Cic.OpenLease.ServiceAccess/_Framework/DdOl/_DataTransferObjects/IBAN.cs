using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenLease.ServiceAccess.DdOl
{
    /// <summary>
    /// IBAN Validation Error Types checked by the validator
    /// </summary>
    public enum IBANValidationErrorType
    {
        NoError = 0,
        InvalidCharacter = 1,
        InvalidStart = 2,
        InvalidChecksum = 3,
        InvalidControlCode = 4,
        InvalidCountry = 5,
        InvalidLength = 6,
        InvalidCountryFormat = 7
    }

    public class IBANValidationError
    {
        /// <summary>
        /// Type of the IBAN error
        /// </summary>
        public IBANValidationErrorType error { get; set; }
        /// <summary>
        /// validated IBAN or error-Detail
        /// </summary>
        public String detail { get; set; }

        /// <summary>
        /// zero based indizes of incorrect fields 
        /// </summary>
        public int[] errorFields { get; set; }

        /// <summary>
        /// BLZ from IBAN
        /// </summary>
        public String blz { get; set; }

        /// <summary>
        /// Name of Bank from BLZ
        /// </summary>
        public String bankname { get; set; }

        /// <summary>
        /// true, if bic does not correspond to BLZ of IBAN
        /// </summary>
        public bool bicwarning { get; set; }

        /// <summary>
        /// blz reference in database
        /// </summary>
        public long sysblz { get; set; }

        //new bic if given one was not found, but alternative
        public String newBIC { get; set; }
    }

    public class IBANInfoDto
    {
        public String countryCode { get; set; }
        public int inputFields { get; set; }
        public int length { get; set; }
    }

}
