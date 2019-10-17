using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Abbstarct class for Offer and Application Operations
    /// </summary>
    public abstract class AbstractItBo : IItBo
    {
        /// <summary>
        /// Data Access Object for Offer/Application
        /// </summary>
        protected IItDao itDao;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="itDao"></param>
        public AbstractItBo(IItDao itDao)
        {
            this.itDao = itDao;
        }


        /// <summary>
        /// IT updaten
        /// </summary>
        /// <param name="ang"></param>
        /// <returns></returns>
        public KundeDto updateIt(KundeDto ang)
        {
            throw new NotImplementedException();
        }
    }
}
