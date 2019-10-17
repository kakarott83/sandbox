using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO.Mediator
{
    public class QueueDto
    {
        public String name { get; set; }
        public QueueRecordDto[] records { get; set; }

        
    }
}
