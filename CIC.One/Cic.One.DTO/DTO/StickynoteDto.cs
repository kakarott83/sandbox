using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util;
namespace Cic.One.DTO
{
    public class StickynoteDto : EntityDto
    {
        /*Primärschlüssel */
        public long sysStickynote { get; set; }
        /*Gebiet */
        public String area { get; set; }
        /*Sysid */
        public long sysId { get; set; }
        /*Bezeichnung */
        public String bezeichnung { get; set; }
        /*Inhalt */
        public String inhalt { get; set; }
        /*Stickyflag */
        public int stickyflag { get; set; }
        /*Privatflag */
        public int privatflag { get; set; }
        /*Codestickytype */
        public string codeStickytype { get; set; }
        /*sysCrtDate */
        public DateTime? sysCrtDate { get; set; }
        /*sysCrtTime */
        public long sysCrtTime { get; set; }
        /*sysCrtUser */
        public long sysCrtUser { get; set; }
        /*Owner name */
        public String wfuserName { get; set; }
        /*sysChgDate */
        public DateTime? sysChgDate { get; set; }
        /*sysChgTime */
        public long sysChgTime { get; set; }
        /// <summary>
        /// Clarion converted DateTime
        /// </summary>
        public DateTime? sysChgTimeGUI
        {
            get
            {
                return DateTimeHelper.ClarionTimeToDateTimeNoException((int)sysChgTime);
            }
            set
            {
                int? val = DateTimeHelper.DateTimeToClarionTime(value);
                if (val.HasValue)
                    sysChgTime = (long)val.Value;

                else
                    sysChgTime = 0;
            }
        }
        /*sysChgUser */
        public long sysChgUser { get; set; }
        /*showFlag */
        public int showFlag { get; set; }
        /*activeFlag */
        public int activeFlag { get; set; }
        /*deleteFlag */
        public int deleteFlag { get; set; }

        override public long getEntityId()
        {
            return sysStickynote;
        }

        public DateTime? DATUM { get; set; }

        public string GEBIET { get; set; }

        public string NOTIZ1 { get; set; }

        public long SYSGEBIET { get; set; }

        public long SYSNOTIZ { get; set; }

        public long SYSPERSON { get; set; }

        public long SYSVT { get; set; }

        public long SYSWFUSER { get; set; }

        public long ZEIT { get; set; }
    }
}
