using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    public class iEvalExpressionDto
    {
        public string area { get; set; }
        public long sysID { get; set; }
        public string[] expression { get; set; }
        public WorkflowContext context;
        
    }
}
