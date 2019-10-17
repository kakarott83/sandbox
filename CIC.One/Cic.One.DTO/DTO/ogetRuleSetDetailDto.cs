using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.One.DTO.Mediator;

namespace Cic.One.DTO
{
    /// <summary>
    /// Returns the detail of a ruleengine ruleset-request
    /// derives from oBaseDto to include Error and runtime information
    /// </summary>
    public class ogetRuleSetDetailDto : oBaseDto
    {
        public QueueResultDto data { get; set; }
        
    }
}