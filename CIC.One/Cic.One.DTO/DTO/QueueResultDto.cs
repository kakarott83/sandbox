using Cic.One.DTO.Mediator;
using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO
{
    public class QueueResultDto : EntityDto
    {
        public List<QueueDto> queues { get; set; }
        public override long getEntityId()
        {
            return 0;
        }
    }
}
