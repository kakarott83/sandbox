using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class MemoDto : EntityDto
    {
        public long SYSWFMMEMO { get; set; }
        public long SYSWFMMKAT { get; set; }
        public long SYSWFMTABLE { get; set; }
        public long SYSLEASE { get; set; }
        public long SYSIDWFTA { get; set; }
        public String KURZBESCHREIBUNG { get; set; }
        public DateTime? EDITDATE { get; set; }
        public DateTime? EDITTIME { get; set; }
        public long EDITUSER { get; set; }
        public DateTime? CREATEDATE { get; set; }
        public DateTime? CREATETIME { get; set; }
        public long CREATEUSER { get; set; }
        public String STR01 { get; set; }
        public String STR02 { get; set; }
        public DateTime? DAT01 { get; set; }
        public DateTime? DAT02 { get; set; }
        public String NOTIZMEMO { get; set; }
        public String EDITUSERNAME { get; set; }
        public String CREATEUSERNAME { get; set; }

        public int INT01 { get; set; }
        public int INT02 { get; set; }

        public void setEDITTIMECLA(int edittime)
        {
            EDITTIME = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionTimeToDateTime((int)edittime);
        }
        public void setCREATETIMECLA(int createtime)
        {
            CREATETIME = Cic.OpenOne.Common.Util.DateTimeHelper.ClarionTimeToDateTime((int)createtime);
        }

        override public long getEntityId()
        {
            return SYSWFMMEMO;
        }

    }
}