using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Model.Prisma;

namespace Cic.OpenOne.Common.DAO.Versicherung
{
    /// <summary>
    /// Schnittstelle Versicherungs DAO
    /// </summary>
    public interface IInsuranceDao
    {
        /// <summary>
        /// returns the VSTYP
        /// </summary>
        /// <param name="sysvstyp"></param>
        /// <returns></returns>
        VSTYP getVSTYP(long sysvstyp);
    }
}
