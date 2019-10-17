using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    /// <summary>
    /// Input for finding a wfuser,
    /// either syswfuser is given
    /// or sysperson|sysperole
    /// and
    /// sysroletype|roletypetyp
    /// </summary>
    public class igetWfuserDto
    {

        public long syswfuser;

        public long sysperson;
        public long sysperole;


        public long sysroletype;
        public long roletypetyp;
        
    }
}
