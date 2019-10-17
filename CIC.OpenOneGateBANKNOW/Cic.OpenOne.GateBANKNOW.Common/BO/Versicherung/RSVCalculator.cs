using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.DTO.Versicherung;
using Cic.OpenOne.Common.DAO.Versicherung;
using Cic.OpenOne.Common.BO.Versicherung;
using Cic.OpenOne.Common.DAO;
using CIC.Database.PRISMA.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Versicherung
{
    /// <summary>
    /// Versicherungsrechner BO
    /// </summary>
    public class RSVCalculator : AbstractVSCalculator
    {
        /// <summary>
        /// RS Versicherungskalkulator
        /// </summary>
        /// <param name="dao">Versicherungs DAO</param>
        /// <param name="quoteDao">Quote DAO</param>
        public RSVCalculator(IInsuranceDao dao, IQuoteDao quoteDao)
            : base(dao, quoteDao)
        {
        }

        /// <summary>
        /// Berechnet Restschuldversicherung BankNow
        ///  obergrenze für monatsrate über quote
        ///  ratebrutto ungerundet*laufzeit* prozentsatz über laufzeit-lookup = prämiebrutto gesamt
        /// </summary>
        /// <param name="param"></param>
        /// <param name="perDate">Eingestelltes Datum</param>
        /// <returns></returns>
        public override oInsuranceDto calculate(iInsuranceDto param, DateTime perDate)
        {
            double maxrate = QuoteDao.getQuote(Cnst_MaxRate);
            VSTYP vstyp = this.getInsurance(param.sysvstyp);

            if (vstyp == null)
            {
                throw new Exception("VSTYP not defined: " + param.sysvstyp);
            }

            if (!vstyp.SYSVG.HasValue)
            {
                throw new Exception("RSVCalculator - VSTYP " + param.sysvstyp + " has no VG");
            }

            InsuranceCalcComponent rate = param.additionalInputValues.Where(t => t.type == InsuranceCalcComponentType.RateBruttoUnrounded).FirstOrDefault();
            if (rate == null)
            {
                throw new Exception("RateBrutto not in input Parameters for RSV Calculation");
            }

            double inputValue = rate.value;
            if (inputValue > maxrate)
            {
                inputValue = maxrate;
            }

            InsuranceCalcComponent laufzeit = param.additionalInputValues.Where(t => t.type == InsuranceCalcComponentType.Laufzeit).FirstOrDefault();
            if (laufzeit == null)
            {
                throw new Exception("Laufzeit not in input Parameters for RSV Calculation");
            }

            InsuranceCalcComponent zins = param.additionalInputValues.Where(t => t.type == InsuranceCalcComponentType.Zins).FirstOrDefault();
            if (zins == null)
            {
                throw new Exception("Zins not in input Parameters for RSV Calculation");
            }

            IVGDao vg = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getVGDao();


            
            double xval = laufzeit.value;
            double yval = 0;
            vg.checkBoundaries(vstyp.SYSVG.Value, param.perDate, ref xval, ref yval);
            
            double factor = vg.getVGValue((long)vstyp.SYSVG.Value, perDate, xval.ToString(), "Prämiensatz", VGInterpolationMode.NONE);
            
            inputValue *= laufzeit.value * (factor / 100);

            oInsuranceDto result = new oInsuranceDto();
            result.insuranceOutputPercentValue = factor;


            result.insuranceOutputValue = inputValue;
            result.additionalOutputValues = new List<InsuranceResultComponent>();


            result.additionalOutputValues.Add(new InsuranceResultComponent(InsuranceResultComponentType.RsvGesamt, inputValue));
            double rsvMonat = Kalkulator.calcRATE(inputValue, zins.value / 12, laufzeit.value, 0, false);
            result.additionalOutputValues.Add(new InsuranceResultComponent(InsuranceResultComponentType.RsvMonat, BankNowCalculator.RoundCHF(rsvMonat)));
            //double zinskostenVorRatenabsicherung = RoundCHF(calcZinsKosten(input.ersteRate > 0, input.laufzeit, result.rateBrutto, kalkulation.angAntKalkDto.szBrutto, kalkulation.angAntKalkDto.rwBrutto, kalkulation.angAntKalkDto.bgexternbrutto));
            result.additionalOutputValues.Add(new InsuranceResultComponent(InsuranceResultComponentType.RsvZins, BankNowCalculator.RoundCHF(rsvMonat) - BankNowCalculator.RoundCHF(rate.value)));
            result.additionalOutputValues.Add(new InsuranceResultComponent(InsuranceResultComponentType.RsvBarwertAddition, inputValue));

            return result;
        }
    }
}
