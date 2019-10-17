using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.DAO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Abstrakte Klasse Buchwert BO
    /// </summary>
   public abstract class AbstractBuchwertBo : IBuchwertBo
    {

        /// <summary>
        /// Data Access Object for Offer/Application
        /// </summary>
        protected IEaihotDao eaihotDao;
        protected IObTypDao obTypDao;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="eaihotDao"></param>
        public AbstractBuchwertBo(IEaihotDao eaihotDao, IObTypDao obTypDao)
        {
            this.eaihotDao = eaihotDao;
            this.obTypDao = obTypDao;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inputBw"></param>
        /// <returns></returns>
        public abstract ogetBuchwertDto getBuchwert(igetBuchwertDto inputBw);

        /// <summary>
        /// isBuchwertCalculationAllowed
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public abstract bool isBuchwertCalculationAllowed(long sysid);

    }
}