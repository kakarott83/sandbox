using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.Model.DdOl;
using AutoMapper;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;


namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Offer/Application Relevant Functions
    /// </summary>
    public class ItBo : AbstractItBo
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pDao"></param>
        public ItBo(ItDao pDao)
            : base(pDao)
        {
        }

        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    }
}
