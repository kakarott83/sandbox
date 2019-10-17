using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
    /// <summary>
    /// KREMO Out Data Transfer Object
    /// </summary>
    public class KREMOOutDto
    {
        /// <summary>
        /// Kreditlimit
        /// </summary>
        public double saldoKorr36 { get; set; }

        /// <summary>
        /// Getter/Setter Base Ammount
        /// </summary>
        public double Grundbetrag { get; set; }

        /// <summary>
        /// Getter/Setter Calculate Health Insurance
        /// </summary>
        public double Berechkrankenkasse { get; set; }

        /// <summary>
        /// Getter/Setter Social expenditure 1
        /// </summary>
        public double Sozialausl1 { get; set; }

        /// <summary>
        /// Getter/Setter Social Expenditure 2
        /// </summary>
        public double Sozialausl2 { get; set; }

        /// <summary>
        /// Getter/Setter Taxes
        /// </summary>
        public double Steuern { get; set; }

        /// <summary>
        /// Getter/Setter Taxes 2
        /// </summary>
        public double Steuern2 { get; set; }

        /// <summary>
        /// Getter/Setter Return code
        /// </summary>
        public double ReturnCode { get; set; }

        /// <summary>
        /// Betrag Anzahl Kinder bis 6 Jahre in CHF
        /// </summary>
        public double Kind1 { get; set; }

        /// <summary>
        /// Betrag Anzahl Kinder bis 12 Jahre in CHF
        /// </summary>
        public double Kind2 { get; set; }

        /// <summary>
        /// Betrag Anzahl Kinder über 12 Jahre in CHF
        /// </summary>
        public double Kind3 { get; set; }

        /// <summary>
        /// Getter/Setter Version
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Getter/Setter Syskremo
        /// </summary>
        public long SysKremo { get; set; }

        /// <summary>
        /// Kreditlimit
        /// </summary>
        public double kreditLimit { get; set; }
    }
}
