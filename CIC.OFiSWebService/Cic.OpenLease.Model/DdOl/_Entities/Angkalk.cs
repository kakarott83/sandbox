// OWNER BK, 18-03-2009
namespace Cic.OpenLease.Model.DdOl
{
	public partial class ANGKALK : Cic.OpenLease.Model.DdOl.ICalculation
    {
        #region Methods
        public void SetBgIntern()
        {
            this.BGINTERN = this.GESAMT.GetValueOrDefault(0) - this.RABATTO.GetValueOrDefault(0) - this.RABATTV.GetValueOrDefault(0) + this.PROVISION.GetValueOrDefault(0);
        }
        #endregion

        #region IVehicleLeasingCalculation properties
        public long ExtId
		{
			// TEST BK 0 BK, Not tested
			get
			{
				return this.SYSKALK;
			}
		}

		// TODO MK 5 BK, Remove, if EF property is available.
        public double? ProvisionSeller
        {
            get;
            set;
        }

		// TODO MK 5 BK, Remove, if EF property is available.
        public double? ProvisionSellerInPercent
        {
            get;
            set;
        }

		// TODO MK 5 BK, Remove, if EF property is available.
		public double? CaseOfDeathInsurance
		{
            get;
            set;
        }

		// TODO MK 5 BK, Remove, if EF property is available.
		public double? UnemploymentInsurance
		{
            get;
            set;

		}

		// TODO MK 5 BK, Remove, if EF property is available.
		public double? RateInPercent
		{
            get;
            set;
        }

        ///// <summary>
        ///// Kalkulationsmodus bestimmt was zu kalkulieren ist.
        ///// MK
        ///// </summary>
        //[System.Runtime.Serialization.DataMember]
        //public Cic.Basic.OpenLease.CalculationCore.CalculationTargets CalculationTarget
        //{
        //    get;
        //    set;
        //}

        ///// <summary>
        ///// SZ halten. Bestimmt welches wert Betrag oder Prozent gehalten sein soll.
        ///// Wenn true gesetztz wird SZP berechnet. Wenn false wird SZ berechnet.
        ///// MK
        ///// </summary>
        //[System.Runtime.Serialization.DataMember]
        //public bool HoldSZ
        //{
        //    get
        //    {
        //        int HoldValues = this.HOLDFIELDS.GetValueOrDefault(0);

        //        bool HoldRWTmp = false;
        //        bool HoldSZTmp = false;
        //        bool HoldZINSTmp = true;

        //        if (HoldValues != 0)
        //        {
        //            Cic.Basic.OpenLease.CalculationHoldValuesHelper.Convert(HoldValues, ref HoldSZTmp, ref HoldZINSTmp, ref HoldRWTmp);
        //        }

        //        return HoldSZTmp;
        //    }
        //    set
        //    {
        //        HoldSZ = value;

        //        int HoldValues = 0;

        //        Cic.Basic.OpenLease.CalculationHoldValuesHelper.Convert(HoldSZ, HoldZINS, HoldRW, ref HoldValues);

        //        this.HOLDFIELDS = HoldValues;
        //    }
        //}

        ///// <summary>
        ///// ZINS halten. Bestimmt welches wert Zins Nomilan (ZINS) oder Zins Effektiv (ZINSEFF) gehalten sein soll.
        ///// Wenn true gesetztz wird ZINS berechnet. Wenn false wird ZINSEFF berechnet.
        ///// MK
        ///// </summary>
        //[System.Runtime.Serialization.DataMember]
        //public bool HoldZINS
        //{
        //    get
        //    {
        //        int HoldValues = this.HOLDFIELDS.GetValueOrDefault(0);

        //        bool HoldRWTmp = false;
        //        bool HoldSZTmp = false;
        //        bool HoldZINSTmp = true;

        //        if (HoldValues != 0)
        //        {
        //            Cic.Basic.OpenLease.CalculationHoldValuesHelper.Convert(HoldValues, ref HoldSZTmp, ref HoldZINSTmp, ref HoldRWTmp);
        //        }

        //        return HoldZINSTmp;
        //    }
        //    set
        //    {
        //        HoldZINS = value;

        //        int HoldValues = 0;

        //        Cic.Basic.OpenLease.CalculationHoldValuesHelper.Convert(HoldSZ, HoldZINS, HoldRW, ref HoldValues);

        //        this.HOLDFIELDS = HoldValues;
        //    }
        //}

        ///// <summary>
        ///// SZ halten. Bestimmt welches wert Betrag oder Prozent gehalten sein soll.
        ///// Wenn true gesetztz wird SZP berechnet. Wenn false wird SZ berechnet.
        ///// MK
        ///// </summary>
        //[System.Runtime.Serialization.DataMember]
        //public bool HoldRW
        //{
        //    get
        //    {
        //        int HoldValues = this.HOLDFIELDS.GetValueOrDefault(0);

        //        bool HoldRWTmp = false;
        //        bool HoldSZTmp = false;
        //        bool HoldZINSTmp = true;

        //        if (HoldValues != 0)
        //        {
        //            Cic.Basic.OpenLease.CalculationHoldValuesHelper.Convert(HoldValues, ref HoldSZTmp, ref HoldZINSTmp, ref HoldRWTmp);
        //        }

        //        return HoldRWTmp;
        //    }
        //    set
        //    {
        //        HoldRW = value;

        //        int HoldValues = 0;
                
        //        Cic.Basic.OpenLease.CalculationHoldValuesHelper.Convert(HoldSZ, HoldZINS, HoldRW, ref HoldValues);

        //        this.HOLDFIELDS = HoldValues;
        //    }
        //}

		#endregion

        #region Formatting properties
        public System.Collections.Generic.List<string> CurrencyProperties
        {
            get
            {
                return new System.Collections.Generic.List<string>()
                {
                    FieldNames.GESAMT,
                    FieldNames.SZ,
                    FieldNames.RW,
                    FieldNames.RATE,
                    FieldNames.BGEXTERN,
                    FieldNames.BGINTERN,
                    FieldNames.DB,
                    FieldNames.GRUND,
                    FieldNames.RABATTO,
                    FieldNames.RABATTV,
                    FieldNames.MARGE,
                    FieldNames.PROVISION
                };
            }
        }
        #endregion
	}
}