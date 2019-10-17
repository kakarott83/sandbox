using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.Common.BO.Versicherung;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DAO.Versicherung;
using Cic.OpenOne.Common.DTO.Versicherung;
using Cic.OpenOne.Common.DTO;
using CIC.Database.PRISMA.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Versicherung
{
    /// <summary>
    /// Versicherungs BO BANKNOW
    /// </summary>
    public class InsuranceBo : AbstractInsuranceBo
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="dao">DAO</param>
        /// <param name="quoteDao">QUote DAO</param>
        public InsuranceBo(IInsuranceDao dao, IQuoteDao quoteDao) : base(dao, quoteDao)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double RoundCHF(double value)
        {
            //Round to 2 places after coma
            return System.Math.Round(value * 20, 0) / 20;
        }

        /// <summary>
        /// calculates an insurance
        /// </summary>
        /// <param name="vs"></param>
        /// <param name="inputValue"></param>
        /// <param name="perDate">Eingestelltes Datum</param>
        /// <returns></returns>
        override public oInsuranceDto calculateInsurance(AngAntVsDto vs, iInsuranceDto inputValue, DateTime perDate)
        {
            VSTYP vstyp = dao.getVSTYP(inputValue.sysvstyp);
            if (vstyp == null)
            {
                return null;
            }

            IVSCalculator vsCalculator = VSCalcFactory.createCalculator(vstyp.CODEMETHOD, dao, QuoteDao);
            
            oInsuranceDto vsResult = vsCalculator.calculate(inputValue, perDate);
            if (vs != null)
            {
                vs.praemieOrg = vsResult.insuranceOutputValue;
                vs.praemie = vsResult.insuranceOutputValue;
                vs.praemie = RoundCHF(vs.praemie);
                vs.praemiep = vsResult.insuranceOutputPercentValue;
                vs.praemiep = System.Math.Round(vs.praemiep, 3);
            }
           

            return vsResult;
        }
    }
}
