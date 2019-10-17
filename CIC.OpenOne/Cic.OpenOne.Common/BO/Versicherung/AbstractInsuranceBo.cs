using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DTO.Versicherung;
using Cic.OpenOne.Common.DAO.Versicherung;
using Cic.OpenOne.Common.DAO;

namespace Cic.OpenOne.Common.BO.Versicherung
{
    /// <summary>
    /// Abstarkte Versicherungsklasse
    /// </summary>
    public abstract class AbstractInsuranceBo : IInsuranceBo
    {
        /// <summary>
        /// DAO
        /// </summary>
        protected IInsuranceDao dao;
        /// <summary>
        /// Quote DAO
        /// </summary>
        protected IQuoteDao QuoteDao;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="dao">DAO</param>
        /// <param name="quoteDao">Quote DAO</param>
        public AbstractInsuranceBo(IInsuranceDao dao, IQuoteDao quoteDao)
        {
            this.dao = dao;
            this.QuoteDao = quoteDao;
        }

        /// <summary>
        /// calculates an insurance
        /// </summary>
        /// <param name="vs"></param>
        /// <param name="inputValue"></param>
        /// <param name="perDate">Eingestelltes Datum</param>
        /// <returns></returns>
        public abstract oInsuranceDto calculateInsurance(AngAntVsDto vs, iInsuranceDto inputValue, DateTime perDate);
    }
}
