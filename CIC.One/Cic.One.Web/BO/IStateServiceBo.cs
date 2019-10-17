using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.One.DTO;

namespace Cic.One.Web.BO
{
    public interface IStateServiceBo
    {
        /// <summary>
        /// Deliver Service Information
        /// </summary>
        /// <param name="Info">Daten</param>
        void getServiceInformation(ServiceInfoDto Info);

        /// <summary>
        /// Returns information about failed configuration checks
        /// </summary>
        /// <returns></returns>
        String performSanityChecks();
    }
}