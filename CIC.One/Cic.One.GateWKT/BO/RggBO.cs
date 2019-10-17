using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.One.Utils.DTO;
using Cic.OpenLease.Service;

namespace Cic.One.GateWKT.BO
{

    public static class RggBO
    {
        public const string CnstRGG = "Rg gebühr";

        public static double CalculateRgGeb(Cic.OpenLease.ServiceAccess.RgGebVersion rgGebVersion, int lz, double zins, double sz, bool kasko, double kaskoRate, bool hp, double hpRate, ref double rateNew, int ppy, double price, double rw, CalculationMode mode, double mwst, double finanzierungssumme, bool iscredit, double vsRate, double serviceRate)
        {


            if (iscredit)//gibt es nicht mehr!
                return 0;

            double rval = 0;
            double rggBase = CalculateRggBase(lz, rateNew, zins, sz, kasko, kaskoRate, hp, hpRate, mwst, vsRate, serviceRate);

            if (rgGebVersion == Cic.OpenLease.ServiceAccess.RgGebVersion.Einmalig)
            {
                rval = rggBase;
            }
            else if (rgGebVersion == Cic.OpenLease.ServiceAccess.RgGebVersion.Mitfinanziert)
            {
                double rateIter = rateNew;
                for (int i = 0; i < 3; i++)
                {
                    rateIter = CalculationBO.calculateRate(finanzierungssumme + rggBase, 0.0d, lz, 0.0d, rw, zins, 0.0d, 12, mode);
                    rggBase = CalculateRggBase(lz, rateIter, zins, sz, kasko, kaskoRate, hp, hpRate, mwst, vsRate, serviceRate);
                }
                rateNew = rateIter;
                rval = rggBase;
            }
            else
            {
                rval = 0;
            }

            return rval;
        }

        /// <summary>
        /// Calculates the RGG
        /// </summary>
        /// <param name="lz"></param>
        /// <param name="rate"></param>
        /// <param name="zins"></param>
        /// <param name="sz"></param>
        /// <param name="kasko"></param>
        /// <param name="kaskoRate"></param>
        /// <param name="hp"></param>
        /// <param name="hpRate"></param>
        /// <param name="mwst"></param>
        /// <param name="vsRate"></param>
        /// <param name="serviceRate"></param>
        /// <returns></returns>
        public static double CalculateRggBase(double lz, double rate, double zins, double sz, bool kasko, double kaskoRate, bool hp, double hpRate, double mwst, double vsRate, double serviceRate)
        {

            if (lz > 36)
            {
                lz = 36;
            }

            //brutto rate
            double result = (serviceRate + rate);
            result *= (1 + mwst / 100.0);//UST
            if (kasko || hp)
            {
                result += vsRate;
            }


            //multiply with lz
            result = lz * result;

            //add brutto sz
            result = result + sz * (1 + mwst / 100.0);

            //modify
            if (!kasko && !hp)
            {
                result = result * 0.0116;
            }
            else//wenn eine versicherung
            {
                result = result * 0.01;
            }

            return result;

        }
    }
}