using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    public class ExtidentDto
    {
        public String codeextidenttyp { get; set; }
        public String extidentvalue { get; set; }
        public String area { get; set; }
        public long sysarea { get; set; }
        public long sysextident { get; set; }
        public String source { get; set; }
    }
}
