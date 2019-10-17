using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// Parameterklasse für BuchwertDto
    /// </summary>
    public class BuchwertDto
    {
        //Platzhalter für Buchwert Parameter
        // TB: in b2b_steuerung_1_1.docx nicht definiert.

        /// <summary>
        /// Vertragsnummer
        /// </summary>
        public String vertrag { get; set; }

        /// <summary>
        /// berechneter Buchwert
        /// </summary>
        public double buchwertBrutto { get; set; }

        /// <summary>
        /// PerDatum zum berechneten Buchwert
        /// </summary>
        public DateTime perDatum { get; set; }

        /// <summary>
        /// Buchwert-PDF
        /// </summary>
        public byte[] hfile { get; set; }

    }

    public class BuchwertInfoDto
    {
        /// <summary>
        /// perDatum Buchwert
        /// </summary>
        public DateTime activeOfferDate { get; set; }
        /// <summary>
        /// berechneter Buchwert
        /// </summary>
        public double activeOfferValue { get; set; }
        /// <summary>
        /// SysVtruek zum Buchwert
        /// </summary>
        public long activeOfferSysVtruek { get; set; }
        /// <summary>
        /// SysEaihfile zu PDF Dokument Buchwertberechnung
        /// </summary>
        public long activeOfferSysEaihfile { get; set; }
    }
}
