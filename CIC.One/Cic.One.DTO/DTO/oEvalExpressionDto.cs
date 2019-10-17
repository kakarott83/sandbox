using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class oEvalExpressionDto:oBaseDto
    {
        public string[] result;
        public WorkflowContext context;
    }
}
