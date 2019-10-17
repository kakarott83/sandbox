using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO
{
    /// <summary>
    /// App User Security Settings
    /// </summary>
    public class WfsysDto:EntityDto
    {
        public long SYSWFSYS { get; set; }
        public int MINPASSWORD { get; set; }
        public int GRACELOGINS { get; set; }
        public int EXPIRATIONMONTHS { get; set; }
        public int DISABLED { get; set; }
        public int GRACEDAYS { get; set; }
        public int EXPIRATIONDAYS { get; set; }
        public int DIFFCOUNT { get; set; }
        public int WARNDAYS { get; set; }
        public int USEAGAIN { get; set; }
        public String INITIALPASSWORT { get; set; }
        public String BACKDOOR { get; set; }
        public int SECURITYMODEL { get; set; }
        public int DISABLEISMEMBER { get; set; }
        public String DISABLEDREASON { get; set; }
        public int SHOWBOTHSECMODELS { get; set; }
        public int UPPERCASECOUNT { get; set; }
        public int LOWERCASECOUNT { get; set; }
        public int SPECIALCHARCOUNT { get; set; }
        public int NUMBERCOUNT { get; set; }
        public int DISABLEB2B { get; set; }
        public String DISABLEREASONB2B { get; set; }
        public String CODERFU { get; set; }
        public String CODERMO { get; set; }
        public long SYSRFN { get; set; }


        public override long getEntityId()
        {
            return SYSWFSYS;
        }
    }
}
