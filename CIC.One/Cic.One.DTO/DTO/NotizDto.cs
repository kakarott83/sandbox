using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util;

namespace Cic.One.DTO
{
    public class NotizDto : EntityDto
    {
       
        override public long getEntityId()
        {
            return SYSNOTIZ;
        }

        public DateTime? DATUM { get; set; }

        public string GEBIET { get; set; }

        public string NOTIZ { get; set; }

        public long SYSGEBIET { get; set; }

        public long SYSNOTIZ { get; set; }

        public long SYSPERSON { get; set; }

        public long SYSVT { get; set; }

        public long SYSWFUSER { get; set; }

        public long ZEIT { get; set; }

        /// <summary>
        /// Clarion converted DateTime
        /// </summary>
        public DateTime? ZEITGUI
        {
            get
            {
                return DateTimeHelper.ClarionTimeToDateTimeNoException((int)ZEIT);
            }
            set
            {
                int? val = DateTimeHelper.DateTimeToClarionTime(value);
                if (val.HasValue)
                    ZEIT = (long)val.Value;

                else
                    ZEIT = 0;
            }
        }


        /// <summary>
        /// Name des Accounts
        /// </summary>
        public String PERSONNAME { get; set; }
    }
}
