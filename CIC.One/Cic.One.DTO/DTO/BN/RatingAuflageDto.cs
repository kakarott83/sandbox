using Cic.OpenOne.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO.BN
{
    /// <summary>
    /// Ratingauflage für angebot/antrag detail
    /// </summary>
    public class RatingAuflageDto
    {
        public String auflagetext { get; set; }
        public int fullfilled { get; set; }
        public DateTime? fullfilleddate { get; set; }
        public String erledigtvon { get; set; }
        public String owner { get; set; }
        public DateTime? syschgdate { get; set; }
        public long syschgtime { get; set; }
        public long sysratingauflage { get; set; }
 
        /// <summary>
        /// Clarion converted DateTime
        /// </summary>
        public DateTime? sysChgDateGUI
        {
            get
            {
                int CnstClarionMinTime = 1;
                
               

                System.DateTime DateTime;

                // Set min date time
                DateTime = DateTimeHelper.DeliverMinDateForClarion();
                // Remove min value
                if (syschgdate != null)
                    // Return
                    return syschgdate + (DateTime.AddMilliseconds(((int)syschgtime - CnstClarionMinTime) * 10)).TimeOfDay;
                else
                    return syschgdate;
            }
            set
            {
                
            }
        }
    }
}
