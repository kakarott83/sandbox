using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.OpenOne.Common.DAO.Prisma;

namespace Cic.OpenOne.Common.DAO.Versicherung
{
    /// <summary>
    /// Versicherungs DAO
    /// </summary>
    public class CachedInsuranceDao : IInsuranceDao
    {
        /// <summary>
        /// returns the VSTYP
        /// </summary>
        /// <param name="sysvstyp"></param>
        /// <returns></returns>
        public VSTYP getVSTYP(long sysvstyp)
        {
            List<VSTYP> vstypen = PrismaDaoFactory.getInstance().getPrismaServiceDao().getVSTYP();
            return (from v in vstypen
                    where v.SYSVSTYP == sysvstyp
                    select v).FirstOrDefault();
        }
    }
}
