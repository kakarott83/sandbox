using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;

namespace Cic.OpenOne.Common.DAO.Versicherung
{
    /// <summary>
    /// Versicherungs DAO
    /// </summary>
    public class InsuranceDao : IInsuranceDao
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// returns the VSTYP
        /// </summary>
        /// <param name="sysvstyp"></param>
        /// <returns></returns>
        virtual public VSTYP getVSTYP(long sysvstyp)
        {
            VSTYP retval;
            using (PrismaExtended ctx = new PrismaExtended())
            {
                retval = ctx.VSTYP.Where(t => t.SYSVSTYP == sysvstyp).FirstOrDefault();
                return retval;
            }
        }
    }
}
