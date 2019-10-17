using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO
{
    /// <summary>
    /// Input for calling the rule-engine
    /// </summary>
    public class igetRuleSetDetailDto
    {
        public String ruleSetName { get; set; }
        public String[] queueName { get; set; }
        public String area { get; set; }
        public long sysid { get; set; }
        public ContextVariableDto[] variables { get; set; }
    }
}
