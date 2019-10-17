using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    /// <summary>
    /// Holds a key value pair for user defined workflow variables
    /// </summary>
    public class ContextVariableDto
    {
        public String key { get; set; }
        public String value { get; set; }
        public String group { get; set; }
    }
}