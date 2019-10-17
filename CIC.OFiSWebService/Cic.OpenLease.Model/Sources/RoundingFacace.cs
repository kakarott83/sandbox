using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenLease.Model
{
    public class RoundingFacade
    {
        private static RoundingFacade instance;

        private RoundingFacade()
        {

        }
        public static RoundingFacade getInstance()
        {
            if (instance == null)
                instance = new RoundingFacade();
            return instance;
        }
        public decimal RoundPrice(decimal value)
        {
            //Round to 2 places after coma
            return System.Math.Round(value, 2);
        }

        public decimal RoundInterest(decimal value)
        {
            //Round to 5 places after coma
            return System.Math.Round(value, 5);
        }

        public decimal RoundCosting(decimal value)
        {
            //Round to 3 places after coma
            return System.Math.Round(value, 3);
        }
    }
}
