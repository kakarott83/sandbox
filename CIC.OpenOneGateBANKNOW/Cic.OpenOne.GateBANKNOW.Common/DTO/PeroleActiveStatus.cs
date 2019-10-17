using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Util;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
   
        public enum PeroleActiveStatus
        {
            /// <summary>
            /// All
            /// </summary>
            [StringValue("All")]
            ALL,

            /// <summary>
            /// Active
            /// </summary>
            [StringValue("Active")]
            ACTIVE,

            /// <summary>
            /// Inactive
            /// </summary>
            [StringValue("Inactive")]
            INACTIVE
        }

}