using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    /// <summary>
    /// Contains input data to fill a html template 
    /// </summary>
    public class iprintFromTemplateDto
    {
        public EntityContainer data {get;set;}
        public String template { get; set; }
    }
}
