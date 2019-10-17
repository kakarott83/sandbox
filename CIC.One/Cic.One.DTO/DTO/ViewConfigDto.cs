using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    /// <summary>
    /// defines a view currently available in the frontend
    /// by name, type and a deprecation flag
    /// 
    /// </summary>
    public class ViewConfigDto
    {
        public String befehlszeile {get;set;}
        public int entryType {get;set;}
        public int deprecated { get; set; }
    }
}
