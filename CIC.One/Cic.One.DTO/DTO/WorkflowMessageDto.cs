using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    public class WorkflowMessageDto
    {
        /// <summary>
        /// Defines if the item will be removed upon resuming workflow
        /// </summary>
        public int persist { get; set; }

        /// <summary>
        /// Message to display
        /// </summary>
        public String message { get; set; }
        public String title { get; set; }
        /// <summary>
        /// Type of Message, 0-3 corresponding to INFO, WARN, ERROR, FATAL
        /// </summary>
        public int type { get; set; }
    }
}