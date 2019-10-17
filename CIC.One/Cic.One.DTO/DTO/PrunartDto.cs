using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO
{
    /// <summary>
    /// Checklist Validity
    /// </summary>
    public class PrunartDto : EntityDto
    {
        public long sysptype { get; set; }
        public long sysprunart { get; set; }


        public DateTime? validUntil { get; set; }
        public DateTime? validFrom { get; set; }
        public int activeflag { get; set; }
        public String art { get; set; }

        public String artNew { get; set; }
        public DateTime? validUntilNew { get; set; }

        public override long getEntityId()
        {
            return sysprunart;
        }
     
    }
}
