using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.InteropServices;
using System.Text;
using System.ServiceModel;

namespace IBANKernel
{
    
    public class IBANKernelAccess
    {
        [DllImport("IBANKernel64.dll")]
        public static extern int IT_IBANConvert(
            // Input
                                                [MarshalAs(UnmanagedType.LPStr)]
                                                String pszKonto,            // Kontonummer
                                                [MarshalAs(UnmanagedType.LPStr)]
                                               String pszBCPC,             // BC-Nummer (Bankenclearing-Nummer) oder Post-Code
            // Output
                                                [MarshalAs(UnmanagedType.LPStr)]
                                                StringBuilder pszIBAN,      // IBAN
                                                int nIBANLen,
                                                [MarshalAs(UnmanagedType.LPStr)]
                                                StringBuilder pszBC,        // BC-Nummer (Bankenclearing-Nummer)
                                                int nBCLen,
                                                [MarshalAs(UnmanagedType.LPStr)]
                                                StringBuilder pszPC,        // Post-Konto-Nummer
                                                int nPCLen,
                                                [MarshalAs(UnmanagedType.LPStr)]
                                                StringBuilder pszBIC,       // Bank Identifier Code (SWIFT)
                                                int nBICLen);

        [DllImport("IBANKernel64.dll")]
        public static extern int IT_IBANVersion(
            // Output
                                                ref UInt32 pdwMajor,
                                                ref UInt32 pdwminor,
                                                [MarshalAs(UnmanagedType.LPStr)]
                                                StringBuilder pszValidUntil,
                                                int nValidLen);

        // Die Länge der String-Parameter für die IT_IBANConvert-Methode aus der IBANKernel.dll
        private const int IBANKERNEL_STRINGLEN = 64;


        /// <summary>
        /// finds the bank-Id, BankName and IBAN for the given account number and bank code
        /// </summary>
        /// <param name="kontoNummer"></param>
        /// <param name="bcpcNummer"></param>
        /// <returns></returns>
        public static IBANInfo getIBANInfo(String kontoNummer, String bcpcNummer)
        {
           
            // output variables
            StringBuilder iban = new StringBuilder(IBANKERNEL_STRINGLEN);
            StringBuilder bcNr = new StringBuilder(IBANKERNEL_STRINGLEN);
            StringBuilder pcNr = new StringBuilder(IBANKERNEL_STRINGLEN);
            StringBuilder bicNr = new StringBuilder(IBANKERNEL_STRINGLEN);
            

            int returnValue = IT_IBANConvert(kontoNummer, bcpcNummer, iban, iban.Capacity, bcNr, bcNr.Capacity, pcNr, pcNr.Capacity, bicNr, bicNr.Capacity);

            IBANInfo rval = new IBANInfo();
            rval.iban = iban.ToString();
            rval.bankId = pcNr.ToString();
            rval.bankName = bicNr.ToString();

            return rval;
        }

        /// <summary>
        /// finds the major vesion, minor version and valid until date of the ibankernel.dll used
        /// </summary>
        /// <param name="kontoNummer"></param>
        /// <param name="bcpcNummer"></param>
        /// <returns></returns>
        public static IBANVersionInfo getIBANVersion()
        {

            // output variables
            UInt32 verMajor = 0;
            UInt32 verMinor = 0;
            StringBuilder validUntil = new StringBuilder(IBANKERNEL_STRINGLEN);


            int returnValue = IT_IBANVersion(ref verMajor, ref verMinor, validUntil, validUntil.Capacity);

            IBANVersionInfo rval = new IBANVersionInfo();
            rval.majorVersion = verMajor.ToString();
            rval.minorVersion = verMinor.ToString();
            rval.validUntil = validUntil.ToString();
            rval.status = returnValue;

            return rval;
        }

    }
}