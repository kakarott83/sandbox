using System;

namespace Cic.OpenOne.Common.DTO.Prisma
{
    /// <summary>
    /// Kontext Basisklasse für Prisma
    /// </summary>
    public class KontextDto
    {
        /// <summary>
        /// Person Role
        /// </summary>
        public long sysperole
        {
            get;
            set;
        }

        private DateTime PerDate;

        /// <summary>
        /// Person Date
        /// </summary>
        public DateTime perDate
        {
            get
            {
                return PerDate;
            }
            set
            {
                PerDate = Util.Config.CfgDate.verifyPerDate(value);
            }
        }
    }
}