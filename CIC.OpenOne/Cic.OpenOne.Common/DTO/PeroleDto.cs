using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.Common.DTO
{
    [System.CLSCompliant(true)]
    public class PeroleDto
    {
        public long SYSPEROLE { get; set; }
        public long SYSROLETYPE { get; set; }
        public long SYSPERSON { get; set; }
        public long SYSPARENT { get; set; }
        
        public String NAME { get; set; }
        public String ROLETYPENAME { get; set; }
        public String INAKTIVGRUND { get; set; }
        public String ZUSTAND { get; set; }
        public String ATTRIBUT { get; set; }
        public int ROLETYPETYP { get; set; }
        public int INACTIVEFLAG { get; set; }
        public int LOCKED { get; set; }
        public DateTime? VALIDFROM { get; set; }
        public DateTime? VALIDUNTIL { get; set; }
        
    }
}
