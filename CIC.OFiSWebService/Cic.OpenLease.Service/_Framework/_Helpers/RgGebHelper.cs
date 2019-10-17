namespace Cic.OpenLease.Service
{

    #region Using


    using Cic.One.Utils.DTO;
    #endregion

    public static class RgGebHelper
    {
        public const string CnstRGG = "Rg gebühr";
        public static decimal CalculateRgGeb(Cic.OpenLease.ServiceAccess.RgGebVersion rgGebVersion, decimal lz, decimal zins, decimal sz, bool kasko, decimal kaskoRate, bool hp, decimal hpRate, ref decimal rateNew, int ppy, decimal price, decimal rw, CalculationMode mode, decimal mwst, decimal finanzierungssumme,bool iscredit)
        {
            decimal FirstPayment = 0;
            decimal Rate = rateNew;
            decimal RggBase;
            decimal RgGeb;
            decimal RggPrice = price - sz;

            if (iscredit)
                return 0;

            if (iscredit)
                RggBase = CalculateRggBaseCredit(finanzierungssumme);
            else
                RggBase = CalculateRggBase(lz, Rate, zins, sz, kasko, kaskoRate, hp, hpRate, mwst);

            if (rgGebVersion == ServiceAccess.RgGebVersion.Einmalig)
            {
                RgGeb = RggBase;
            }
            else if (rgGebVersion == ServiceAccess.RgGebVersion.Mitfinanziert)
            {
                decimal zinsEff = 0;
                Rate = KalkulationHelper.CalculateRate(ref RggPrice, ref FirstPayment, ref lz, ref rateNew, ref rw, ref zins, ref zinsEff, ppy, CalculationTargets.CalculateRate, mode);
                // BmwCalculateHelper.MyCalculateRate(ref RggPrice, ref FirstPayment, ref lz, ref rateNew, ref rw, ref mwst, ppy, CalculationTargets.CalculateRate, mode, zins);

                decimal PriceAndBase = price - sz + RggBase;
                rateNew = KalkulationHelper.CalculateRate(ref PriceAndBase, ref FirstPayment, ref lz, ref rateNew, ref rw, ref zins, ref zinsEff, ppy, CalculationTargets.CalculateRate, mode);
                    //BmwCalculateHelper.MyCalculateRate(ref PriceAndBase, ref FirstPayment, ref lz, ref rateNew, ref rw, ref mwst, ppy, CalculationTargets.CalculateRate, mode, zins);
                if (iscredit)
                    RggBase = CalculateRggBaseCredit(PriceAndBase);
                else
                    RggBase = CalculateRggBase(lz, Rate, zins, sz, kasko, kaskoRate, hp, hpRate, mwst);

                PriceAndBase = price - sz + RggBase;
                rateNew = KalkulationHelper.CalculateRate(ref PriceAndBase, ref FirstPayment, ref lz, ref rateNew, ref rw, ref zins, ref zinsEff, ppy, CalculationTargets.CalculateRate, mode);
                    //BmwCalculateHelper.MyCalculateRate(ref PriceAndBase, ref FirstPayment, ref lz, ref rateNew, ref rw, ref mwst, ppy, CalculationTargets.CalculateRate, mode, zins);
                if (iscredit)
                    RggBase = CalculateRggBaseCredit(PriceAndBase);
                else
                    RggBase = CalculateRggBase(lz, Rate, zins, sz, kasko, kaskoRate, hp, hpRate, mwst);
                PriceAndBase = price - sz + RggBase;

                rateNew = KalkulationHelper.CalculateRate(ref PriceAndBase, ref FirstPayment, ref lz, ref rateNew, ref rw, ref zins, ref zinsEff, ppy, CalculationTargets.CalculateRate, mode);
                    //BmwCalculateHelper.MyCalculateRate(ref PriceAndBase, ref FirstPayment, ref lz, ref rateNew, ref rw, ref mwst, ppy, CalculationTargets.CalculateRate, mode, zins);
                if (iscredit)
                    RggBase = CalculateRggBaseCredit(PriceAndBase);
                else
                    RggBase = CalculateRggBase(lz, Rate, zins, sz, kasko, kaskoRate, hp, hpRate, mwst);
                RgGeb = RggBase;
            }
            else
            {
                RgGeb = 0;
            }

            return RgGeb;
        }

        public static decimal CalculateRggBaseCredit(decimal finanzierungssumme)
        {
            return 0; // finanzierungssumme * (decimal)(0);
        }
        public static decimal CalculateRggBase(decimal lz, decimal rate, decimal zins, decimal sz, bool kasko, decimal kaskoRate, bool hp, decimal hpRate, decimal mwst)
        {
            decimal Result;

            if (lz > 36)
            {
                lz = 36;
            }

            //brutto rate
            Result = rate;

            //add insurance rates
            if (kasko)
            {
                Result += kaskoRate;
            }

            if (hp)
            {
                Result += hpRate;
            }

            //multiply with lz
            Result = lz * Result;

            //add brutto sz
            Result = Result + sz;

            //modify
            if (!kasko && !hp)
            {
                Result = Result * 0.0116M;
            }
            else if (kasko && !hp)
            {
                Result = Result * 0.0106M;
            }
            else if (!kasko && hp)
            {
                Result = Result * 0.011M;
            }
            else
            {
                Result = Result * 0.01M;
            }

            return Result;

        }
    }
}