using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.DAO.Versicherung;
using Cic.OpenOne.Common.BO.Versicherung;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO.Versicherung;
using CIC.Database.PRISMA.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Versicherung
{
    /// <summary>
    /// Todesfallrechner BO
    /// </summary>
    public class TodesfallCalculator : AbstractVSCalculator
    {
        /// <summary>
        /// Todesfall Kalkulator Contstructor
        /// </summary>
        /// <param name="dao"></param>
        /// <param name="quoteDao">Quote DAO</param>
        public TodesfallCalculator(IInsuranceDao dao, IQuoteDao quoteDao)
            : base(dao, quoteDao)
        {
        }

        /// <summary>
        /// Berechnet TodesfallVersicherung BankNow
        ///  obergrenze für monatsrate über quote
        ///  ratebrutto ungerundet * prozentsatz = prämiebrutto monatlich ungerundet
        /// </summary>
        /// <param name="param"></param>
        /// <param name="perDate">Eingestelltes Datum</param>
        /// <returns></returns>
        public override oInsuranceDto calculate(iInsuranceDto param, DateTime perDate)
        {
            double maxrate = QuoteDao.getQuote(Cnst_MaxRate);
            VSTYP vs = this.getInsurance(param.sysvstyp);
            if (vs == null)
            {
                throw new Exception("VSTYP not defined: " + param.sysvstyp);
            }
            if (!vs.SYSQUOTE.HasValue)
            {
                throw new Exception("VSTYP " + param.sysvstyp + " has no SYSQUOTE");
            }
            double factor = QuoteDao.getQuote(vs.SYSQUOTE.Value);

            InsuranceCalcComponent rate = param.additionalInputValues.Where(t => t.type == InsuranceCalcComponentType.RateBruttoUnrounded).FirstOrDefault();
            if (rate == null)
            {
                throw new Exception("RateBrutto not in input Parameters for Todesfall Calculation");
            }
            InsuranceCalcComponent laufzeit = param.additionalInputValues.Where(t => t.type == InsuranceCalcComponentType.Laufzeit).FirstOrDefault();
            if (laufzeit == null)
            {
                throw new Exception("Laufzeit not in input Parameters for Todesfall Calculation");
            }

            double inputValue = rate.value;
            if (inputValue > maxrate)
            {
                inputValue = maxrate;
            }

            inputValue *= (factor / 100);

            oInsuranceDto result = new oInsuranceDto();
            result.insuranceOutputValue = inputValue;
            result.insuranceOutputPercentValue = factor;
            result.additionalOutputValues = new List<InsuranceResultComponent>();
            result.additionalOutputValues.Add(new InsuranceResultComponent(InsuranceResultComponentType.RsvMonat, BankNowCalculator.RoundCHF(inputValue)));
            result.additionalOutputValues.Add(new InsuranceResultComponent(InsuranceResultComponentType.RsvGesamt, inputValue * laufzeit.value));

            return result;
        }
    }
}
