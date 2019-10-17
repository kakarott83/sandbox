using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO.Prisma
{
    /// <summary>
    /// Contains all information needed for finding subventions for a product, subvention type (explicit/implicit) and trigger-prparm
    /// </summary>
    public class PrSubvTriggerDto
    {
        /// <summary>
        /// Id of the product
        /// </summary>
        public long sysprproduct { get; set; }

        /// <summary>
        /// Type of subvention (explicit=1/implicit=2)
        /// </summary>
        public int trgtype { get; set; }

        /// <summary>
        /// sysprparam-id triggering this subvention
        /// </summary>
        public long sysprfldtrg { get; set; }

        /// <summary>
        /// Id of Subvention
        /// </summary>
        public long sysprsubv { get; set; }
    }
}
