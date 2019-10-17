using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO.Prisma
{
    /// <summary>
    /// Parameterklasse von srvKontextDto
    /// </summary>
    public class srvKontextDto : KontextDto
    {
        /// <summary>
        /// ID des Produktes
        /// </summary>
        public long sysprprodukt
        {
            get;
            set;
        }

        /// <summary>
        /// ID der Kundengruppe
        /// </summary>
        public long sysprkgroup
        {
            get;
            set;
        }

        /// <summary>
        /// ID des Kundentyps
        /// </summary>
        public long syskdtyp
        {
            get;
            set;
        }

        /// <summary>
        /// ID des Objekttyps
        /// </summary>
        public long sysobtyp
        {
            get;
            set;
        }

        /// <summary>
        /// ID der Objektart
        /// </summary>
        public long sysobart
        {
            get;
            set;
        }
    }
}
