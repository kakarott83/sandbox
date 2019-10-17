using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO
{
    /// <summary>
    /// OL Global System Settings/Flags
    /// </summary>
    public class CicconfDto: EntityDto
    {
        public override long getEntityId()
        {
            return SYSCICCONF;
        }
        public long SYSCICCONF { get; set; }
        public int FLAGOP { get; set; }
        public String EVALOP { get; set; }
        public int FLAGSONDER { get; set; }
        public int FLAGEND { get; set; }
        public int FLAGKORR { get; set; }
        public int FLAGOPT1 { get; set; }
        public int FLAGOPT2 { get; set; }
        public int FLAGOPT3 { get; set; }
        public int FLAGOPT4 { get; set; }
        public int FLAGOPT5 { get; set; }
        public int FLAGOPT6 { get; set; }
        public int FLAGOPT7 { get; set; }
        public int FLAGOPT8 { get; set; }
        public int FLAGOPT9 { get; set; }
        public int FLAGOPT10 { get; set; }
        public String EVALOPT1 { get; set; }
        public String EVALOPT2 { get; set; }
        public String EVALOPT3 { get; set; }
        public String EVALOPT4 { get; set; }
        public String EVALOPT5 { get; set; }
        public String LKZ { get; set; }
        public long SYSLAND { get; set; }
        public int FLAGMANDANT { get; set; }
        public int PTYPSKONTOM { get; set; }
        public int PTYPMNKONTO { get; set; }
        public int PTYPCALCULATOR { get; set; }
        public int PTYPPUSER { get; set; }
        public int PTYPANGEBOT { get; set; }
        public int PSYNCANTRAG { get; set; }
        public int PSYNCANGEBOT { get; set; }
        public int FLAGLKPSELECTPRISMA { get; set; }
        public int NOTIERUNG { get; set; }
        public int VERTRIEBSBAUMART { get; set; }

    }
}
