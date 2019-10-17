using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Datastructure for Clarifications
    /// select level,wfmmemo.*,wfmmemo.createtime createtimecla from wfmmemo,wftable where wftable.syswftable=wfmmemo.syswfmtable start with syswfmmkat=174 and syslease=14094 and wftable.syscode='ANGEBOT' connect by prior syswfmmemo=syslease and syswfmtable=319
    ///order siblings by syswfmmemo asc;
    /// </summary>
    public class ClarificationDto:EntityDto
    {
        public int level { get; set; }
        public long syswfmmemo { get; set; }
        public long syswfmtable { get; set; }
        public long syslease { get; set; }
        public long syswfmmkat { get; set; }
        /// <summary>
        /// Create User
        /// </summary>
        public long createuser { get; set; }
        /// <summary>
        /// Targetuser
        /// </summary>
        public long edituser { get; set; }
        /// <summary>
        /// Process reference
        /// </summary>
        public long sysbpprocinst { get; set; }
        public String username { get; set; }
        public String kurzbeschreibung { get; set; }
        public String notizmemo { get; set; }
        public DateTime? createdate { get; set; }
        public DateTime? createtime { get; set; }

        public DateTime? reminderDate { get; set; }
        public String olarea { get; set; }

        /// <summary>
        /// Erledigt
        /// </summary>
        public int erledigt { get; set; }
        /// <summary>
        /// Zugewiesene Lane / Targetlane
        /// </summary>
        public long sysbprole { get; set; }

        public int isAnswer { get; set; }
        public int isForward { get; set; }

        private long _createtimecla;
        public long CREATETIMECLA
        {
            get { return _createtimecla; }
            set
            {
                _createtimecla = value;
                createtime = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionTimeToDateTime((int)value);
            }
        }
     

        public override long getEntityId()
        {
            return syswfmmemo;
        }
    }
}
