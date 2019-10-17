using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.Util;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
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
