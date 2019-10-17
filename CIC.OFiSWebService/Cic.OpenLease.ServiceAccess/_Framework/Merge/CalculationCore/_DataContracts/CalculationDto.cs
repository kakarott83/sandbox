// OWNER WB, 24-03-2010

using Cic.One.Utils.DTO;
namespace Cic.OpenLease.ServiceAccess.Merge.CalculationCore
{
    /// <summary>
    /// Kredit kalkulation objekt.
    /// MK
    /// </summary>
    [System.CLSCompliant(true)]
    public class CalculationDto
    {
        #region Constructors
        public CalculationDto()
        {
        }

        public CalculationDto(Cic.OpenLease.Model.DdOl.ICalculation calculation)
        {
            this.BG = calculation.BGEXTERN.GetValueOrDefault(0);
            this.ZINS = calculation.ZINS.GetValueOrDefault(0);
            this.ZINSEFF = calculation.ZINSEFF.GetValueOrDefault(0);
            this.PPY = calculation.PPY.GetValueOrDefault(0);
            this.LZ = calculation.LZ.GetValueOrDefault(0);
            this.RATE = calculation.RATE.GetValueOrDefault(0);
            this.RW = calculation.RW.GetValueOrDefault(0);
            this.RWP = calculation.RWP.GetValueOrDefault(0);
            this.SZ = calculation.SZ.GetValueOrDefault(0);
            this.SZP = calculation.SZP.GetValueOrDefault(0);
            this.CalculationTarget = (CalculationTargets)calculation.CALCTARGET.GetValueOrDefault((int)CalculationTargets.CalculateRate);
            int HoldValues = calculation.HOLDFIELDS.GetValueOrDefault(0);

            bool HoldRWTmp = false;
            bool HoldSZTmp = false;
            bool HoldZINSTmp = true;

            if (HoldValues != 0)
            {
                Convert(HoldValues, ref HoldSZTmp, ref HoldZINSTmp, ref HoldRWTmp);
            }

            this.HoldRW = HoldRWTmp;
            this.HoldSZ = HoldSZTmp;
            this.HoldZINS = HoldZINSTmp;
        }
        public static void Convert(int holdValues, ref bool holdSZ, ref bool holdZINS, ref bool holdRW)
        {
            System.Collections.BitArray HoldsBitArray;

            HoldsBitArray = new System.Collections.BitArray(System.BitConverter.GetBytes(holdValues));

            holdRW = HoldsBitArray[0];
            holdSZ = HoldsBitArray[1];
            holdZINS = HoldsBitArray[2];
        }
        #endregion

        /// <summary>
        /// Berechnungsgrundlage.
        /// MK
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal BG
        {
            get;
            set;
        }

        /// <summary>
        /// Anzahlung.
        /// MK
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal SZ
        {
            get;
            set;
        }

        /// <summary>
        /// Anzahlungprozent.
        /// MK
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal SZP
        {
            get;
            set;
        }

        /// <summary>
        /// Laufzeit.
        /// MK
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public int LZ
        {
            get;
            set;
        }

        /// <summary>
        /// Jahreskilometer.
        /// MK
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public int JAHRESKM
        {
            get;
            set;
        }

        /// <summary>
        /// Rate.
        /// MK
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal RATE
        {
            get;
            set;
        }

        /// <summary>
        /// Letzte Rate.
        /// MK
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal RW
        {
            get;
            set;
        }

        /// <summary>
        /// Letzte Rate Prozent.
        /// MK
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal RWP
        {
            get;
            set;
        }

        /// <summary>
        /// Zins.
        /// MK
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal ZINS
        {
            get;
            set;
        }

        /// <summary>
        /// Effektifzins.
        /// MK
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public decimal ZINSEFF
        {
            get;
            set;
        }

        /// <summary>
        /// Perioden pro Jahr.
        /// MK
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public int PPY
        {
            get;
            set;
        }

        /// <summary>
        /// Kalkulationsmodus bestimmt was zu kalkulieren ist.
        /// MK
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public CalculationTargets CalculationTarget
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember]
        public CalculationMode Mode
        {
            get;
            set;
        }

        /// <summary>
        /// SZ halten. Bestimmt welches wert Betrag oder Prozent gehalten sein soll.
        /// Wenn true gesetztz wird SZP berechnet. Wenn false wird SZ berechnet.
        /// MK
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public bool HoldSZ
        {
            get;
            set;
        }

        /// <summary>
        /// ZINS halten. Bestimmt welches wert Zins Nomilan (ZINS) oder Zins Effektiv (ZINSEFF) gehalten sein soll.
        /// Wenn true gesetztz wird ZINS berechnet. Wenn false wird ZINSEFF berechnet.
        /// MK
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public bool HoldZINS
        {
            get;
            set;
        }

        /// <summary>
        /// SZ halten. Bestimmt welches wert Betrag oder Prozent gehalten sein soll.
        /// Wenn true gesetztz wird SZP berechnet. Wenn false wird SZ berechnet.
        /// MK
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public bool HoldRW
        {
            get;
            set;
        }

        /// <summary>
        /// Minimale Laufzeit
        /// JJ
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public int? TermMin
        {
            get;
            set;
        }

        /// <summary>
        /// Maximale Lauzeit
        /// JJ
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public int? TermMax
        {
            get;
            set;
        }

        /// <summary>
        /// Schrittweite
        /// JJ
        /// </summary>
        [System.Runtime.Serialization.DataMember]
        public int? TermStep
        {
            get;
            set;
        }


    }
}
