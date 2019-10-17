using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.BO.Versicherung;

using Cic.OpenOne.Common.BO.Versicherung;
using Cic.OpenOne.Common.DAO.Versicherung;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO.Versicherung;
using CIC.Database.PRISMA.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Versicherung
{
    class AUKCalculator : AbstractVSCalculator
    {
        public AUKCalculator(IInsuranceDao dao, IQuoteDao quoteDao)
            : base(dao, quoteDao)
        {
        }

        /// <summary>
        /// Berechnet AU-Versicherung BankNow
        ///  obergrenze für monatsrate über quote
        ///  ratebrutto ungerundet * prozentsatz = prämiebrutto monatlich ungerundet
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
                throw new Exception("RateBrutto not in input Parameters for AUK Calculation");
            }
            InsuranceCalcComponent laufzeit = param.additionalInputValues.Where(t => t.type == InsuranceCalcComponentType.Laufzeit).FirstOrDefault();
            if (laufzeit == null)
            {
                throw new Exception("Laufzeit not in input Parameters for AUK Calculation");
            }
            InsuranceCalcComponent aufschub = param.additionalInputValues.Where(t => t.type == InsuranceCalcComponentType.Aufschub).FirstOrDefault();
            if (aufschub == null)
            {
                throw new Exception("Aufschub not in input Parameters for AUK Calculation");
            }

            if (laufzeit.value <= 0)
            {
                throw new Exception("Laufzeit not in valid range. Check delay options!");
            }

            double inputValue = rate.value;

            if ( maxrate > 0 && inputValue > maxrate)
            {
                inputValue = maxrate;
            }

            InsuranceCalcComponent zins = param.additionalInputValues.Where(t => t.type == InsuranceCalcComponentType.Zins).FirstOrDefault();
            if (zins == null)
            {
                throw new Exception("Zins not in input Parameters for RSV Calculation");
            }

            IVGDao vg = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getVGDao();
           

            
            double xval = laufzeit.value;
            double yval = 0;
            vg.checkBoundaries(vstyp.SYSVG.Value, param.perDate,ref xval, ref yval);
            
            double factor = vg.getVGValue((long)vstyp.SYSVG.Value, perDate, xval.ToString(), "Prämiensatz", VGInterpolationMode.NONE);
            

            inputValue *= (laufzeit.value - aufschub.value) * (factor / 100);

            oInsuranceDto result = new oInsuranceDto();
            result.insuranceOutputValue = inputValue;
            result.insuranceOutputPercentValue = factor;
            result.additionalOutputValues = new List<InsuranceResultComponent>();
            
            result.additionalOutputValues.Add(new InsuranceResultComponent(InsuranceResultComponentType.RsvGesamt, inputValue));
            double rsvMonat = Kalkulator.calcRATE(inputValue, zins.value / 12, (laufzeit.value - aufschub.value), 0, false);
            result.additionalOutputValues.Add(new InsuranceResultComponent(InsuranceResultComponentType.RsvMonat, BankNowCalculator.RoundCHF(rsvMonat)));
            //double zinskostenVorRatenabsicherung = RoundCHF(calcZinsKosten(input.ersteRate > 0, input.laufzeit, result.rateBrutto, kalkulation.angAntKalkDto.szBrutto, kalkulation.angAntKalkDto.rwBrutto, kalkulation.angAntKalkDto.bgexternbrutto));
            result.additionalOutputValues.Add(new InsuranceResultComponent(InsuranceResultComponentType.RsvZins, BankNowCalculator.RoundCHF(BankNowCalculator.RoundCHF(rsvMonat) * (laufzeit.value - aufschub.value) - BankNowCalculator.RoundCHF(inputValue) )));
            result.additionalOutputValues.Add(new InsuranceResultComponent(InsuranceResultComponentType.RsvBarwertAddition, inputValue));
            return result;
        }
    }
}
