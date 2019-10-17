using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class AngAntOptionDto:EntityDto
    {
        public long sysid { get; set; }

        //kostenstelle
        public String option1 { get; set; }

        //one of kostenträger/kfznr/kennzeichen
        public String option3 { get; set; }

        //value for option3
        public String option4 { get; set; }

        public String option7 { get; set; }

        public String str01 { get; set; }
        public String str02 { get; set; }
        public String str03 { get; set; }

        /// <summary>
        /// Flag Lagerwagen
        /// </summary>
        public int? flag10 { get; set; }

        /// <summary>
        /// nonpersistent field containing the amount of calculations
        /// </summary>
        public long mycalccount { get; set; }

        public override long getEntityId()
        {
            return sysid;
        }
    }
}
