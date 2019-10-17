using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenLease.Service
{
    public class MwStFacade
    {
        private static MwStFacade instance;

        private MwStFacade()
        {
        
        }
        public static MwStFacade getInstance()
        {
            if (instance == null)
                instance = new MwStFacade();
            return instance;
        }

        /// <summary>
        /// Calculates tax value from gross value and tax rate.
        /// </summary>
        /// <param name="grossValue">Gross value.</param>
        /// <param name="taxRate">Tax rate.</param>
        /// <returns>Tax value.</returns>
        public decimal CalculateTaxValueFromGrossValue(decimal grossValue, decimal taxRate)
        {
            return (taxRate * grossValue) / (100M + taxRate);
        }

        /// <summary>
        /// Calculates tax value from net value and tax rate.
        /// </summary>
        /// <param name="netValue">Net value.</param>
        /// <param name="taxRate">Tax rate.</param>
        /// <returns>Tax value.</returns>
        public decimal CalculateTaxValueFromNetValue(decimal netValue, decimal taxRate)
        {
            return netValue * (taxRate / 100M);
        }

        /// <summary>
        /// Calculates net value from gross value and tax rate.
        /// </summary>
        /// <param name="grossValue">Gross value.</param>
        /// <param name="taxRate">Tax rate.</param>
        /// <returns>Net value</returns>
        public decimal CalculateNetValue(decimal grossValue, decimal taxRate)
        {
            return (grossValue * 100M) / (100M + taxRate);
        }

        /// <summary>
        /// Calculates gross value from net value and tax rate.
        /// </summary>
        /// <param name="netValue">Net value.</param>
        /// <param name="taxRate">Tax rate.</param>
        /// <returns>Gross value.</returns>
        public decimal CalculateGrossValue(decimal netValue, decimal taxRate)
        {
            return netValue + ((netValue * taxRate) / 100M);
        }
    }
}