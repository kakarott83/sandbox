using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    public class AngAntOptionDto
    {
        /// <summary>
        /// id of parent object (ang or ant)
        /// </summary>
        public long sysid { get; set; }

        /// <summary>
        /// short string option
        /// </summary>
        public String option7 { get; set; }


        /// <summary>
        /// integer option
        /// </summary>
        public long int02 { get; set; }

        /// <summary>
        /// Flag Lagerwagen
        /// </summary>
        public int flag10 { get; set; }

        //kostenstelle
        public String option1 { get; set; }

        //one of kostenträger/kfznr/kennzeichen
        public String option3 { get; set; }

        //value for option3
        public String option4 { get; set; }

        /// <summary>
        /// KNE Bestaetigung
        /// </summary>
        public String option2 { get; set; }

        /// <summary>
        /// Wenn 1 werden Kremodaten übernommen
        /// </summary>
        public String str06 { get; set; }


    }
}
