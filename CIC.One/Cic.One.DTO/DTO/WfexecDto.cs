using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO
{
    public class WfexecDto : EntityDto 
    {
        override public String getEntityBezeichnung()
        {
            return TABLESYSCODE;
        }
        override public long getEntityId()
        {
            return SYSWFEXEC;
        }
        public long SYSWFEXEC { get; set; }
        public long SYSWFJEXEC { get; set; }
        public long WFGSYSID { get; set; }
        public long WFTASYSID { get; set; }
        public long ALTERNATIVID { get; set; }
        public long LEASESYSID { get; set; }
        public String TABLESYSCODE { get; set; }
        public int JOB { get; set; }
        public DateTime? DATUM { get; set; }
        public long UHRZEIT { get; set; }
        public String SORTFIELD { get; set; }
        public String DUMMY { get; set; }
        public long USEREC { get; set; }

    }
}
