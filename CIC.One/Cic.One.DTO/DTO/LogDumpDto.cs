using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.Common.Util.Logging;

namespace Cic.One.DTO
{
    // rh: 20161108
    public class LogDumpDto : EntityDto
    {
        public long     SysLogDump { get; set; }
        public String   Description { get; set; }
        public int      InputFlag { get; set; }
        public DateTime DumpDate { get; set; }
        public long     DumpTime { get; set; }
        public String   Url { get; set; }
        public long     SysEaiHot { get; set; }
        public int      Art { get; set; }
        public long     SysCicLog { get; set; }
        public String   DumpValue { get; set; }
        public long     SysEaiJob { get; set; }
        public String   Area { get; set; }
        public long     SysID { get; set; }

        private static readonly ILog _log = Log.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        override public long getEntityId()
        {
            return SysLogDump;
        }

        public DateTime? DumpTimeGUI
        {
            get
            {
                System.DateTime DateTime;
                int CnstClarionMinTime = 1;
               
                // Set min date time
                DateTime = DateTimeHelper.DeliverMinDateForClarion();
                
                // Remove min value
                if (DumpDate != null)
                {
                    _log.Debug("Time: " + (DateTime.AddMilliseconds(((int)DumpTime - CnstClarionMinTime) * 10)));

                    // Return
                    return DumpDate + (DateTime.AddMilliseconds(((int)DumpTime - CnstClarionMinTime) * 10)).TimeOfDay;
                }
                else
                {
                    // return DumpDate;
                    return null;            // rh: is Null anyway!
                }
            }
            set
            {

            }
        }
    }

}
