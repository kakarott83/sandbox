using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    public class WfvConfigEntry
    {
        public String syscode { get; set; }
        public String entrytype { get; set; }
        public String befehlszeile { get; set; }
        public String einrichtung { get;set;}
        public String beschreibung { get; set; }
    }
}
