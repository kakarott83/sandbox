using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
        /// <summary>
        /// Indicator GUI Display Values
        /// </summary>
        public class ExpdefDto : ExpdispDto
        {
            public long areaid {get;set;}
            public string area { get; set; }
            public string output { get; set; }
        }
    
}
