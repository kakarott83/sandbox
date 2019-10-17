using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO.Versicherung;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DAO.Versicherung;

namespace Cic.OpenOne.Common.BO.Versicherung
{
    /// <summary>
    /// Calculates a Default Insurance
    /// SYSQUOTE - Prämie
    /// OPTIONAL SYSKORRTYP1, param1: Laufzeit, param2: Finanzierungssumme, Startwert: praemie from SysQUOTE
    /// </summary>
    [System.CLSCompliant(true)]
    public class DefaultCalculator : AbstractVSCalculator
    {
        /// <summary>
        /// Standardkalkulator
        /// </summary>
        /// <param name="dao">Versicherungs DAO</param>
        /// <param name="quoteDao">Quote DAO</param>
        public DefaultCalculator(IInsuranceDao dao, IQuoteDao quoteDao)
            : base(dao, quoteDao)
        {
        }

         /// <summary>
        /// Calculates the given Insurance
        /// </summary>
        /// <param name="param">Versicherungsparameter</param>
        /// <param name="perDate">Eingestelltes Datum</param>
        /// <returns></returns>
        public override oInsuranceDto calculate(iInsuranceDto param, DateTime perDate)
        {
            oInsuranceDto rval = new oInsuranceDto();

            //Versicherungssteuer
            //decimal versicherungsSteuerProzent = MyDeliverSteuer(context);

            //decimal praemie = 0;
           /* if (vstyp.QUOTEAlias != null)
                praemie = (12 / param.Zahlmodus) * QUOTEDao.deliverQuotePercentValue(context, vstyp.QUOTEAlias.SYSQUOTE);
            */
            KorrekturBo korr = new KorrekturBo(new KorrekturDao());
            
            //string op = "+";

            //Bonuskasko - SYSKORRTYP2
            /*if (vstyp.SYSKORRTYP1 != null)
            {
                praemie = (decimal)korr.Correct((long)vstyp.SYSKORRTYP1, praemie, op, DateTime.Now, param.Laufzeit.ToString(), param.Finanzierungssumme.ToString());
            }*/
            //Default
           /* rval.Netto = praemie - (0 * (12 / param.Zahlmodus));
            rval.Motorsteuer = 0;
            rval.Versicherungssteuer = rval.Netto * versicherungsSteuerProzent / 100;
            rval.Praemie_Default = rval.Netto + rval.Versicherungssteuer;

            MySetSubventionNachlass(context, param, rval, versicherungsSteuerProzent);



            rval.Provision = 0;*/


            return rval;
        }

    }
}
