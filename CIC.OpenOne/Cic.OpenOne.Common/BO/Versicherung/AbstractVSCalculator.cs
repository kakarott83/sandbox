using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO.Versicherung;
using Cic.OpenOne.Common.DAO.Versicherung;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.DAO;

namespace Cic.OpenOne.Common.BO.Versicherung
{
    /// <summary>
    /// Abstract Base Class of all ensurance calculators
    /// </summary>
    [System.CLSCompliant(true)]
    public abstract class AbstractVSCalculator : IVSCalculator
    {
        /// <summary>
        /// Maximale Rate Konstante
        /// </summary>
        protected static String Cnst_MaxRate = "MAX_VSRATE";
        /// <summary>
        /// Versicherungs DAO
        /// </summary>
        protected IInsuranceDao dao;
        /// <summary>
        /// Quote DAO
        /// </summary>
        protected IQuoteDao QuoteDao;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="dao">Versicherungs DAO</param>
        /// <param name="quoteDao">Quote DAO</param>
        public AbstractVSCalculator(IInsuranceDao dao, IQuoteDao quoteDao)
        {
            this.dao = dao;
            this.QuoteDao = quoteDao;
        }

        /// <summary>
        /// Calculates the given Insurance
        /// </summary>
        /// <param name="param"></param>
        /// <param name="perDate">Eingestelltes Datum</param>
        /// <returns>Versicherungsdaten</returns>
        public abstract oInsuranceDto calculate(iInsuranceDto param, DateTime perDate);

        /// <summary>
        /// Versicherung auslesen
        /// </summary>
        /// <param name="sysvstyp">Versicherungstyp ID</param>
        /// <returns>VSTYP</returns>
        public VSTYP getInsurance(long sysvstyp)
        {
            return dao.getVSTYP(sysvstyp);
        }

       
    }
}
