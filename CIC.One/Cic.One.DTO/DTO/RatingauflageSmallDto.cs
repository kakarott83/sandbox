using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Ratingauflage
    /// </summary>
    public class RatingauflageSmallDto : EntityDto
    {
        public long sysratingauflage { get; set; }
        public int fullfilled { get; set; }
        public int status { get; set; }
        public DateTime? fullfilleddate { get; set; }
        public long syschguser { get; set; }
        public DateTime? syschgdate { get; set; }
        public String chguser { get; set; }
        /*public long sysrating { get; set; }
        public long sysperson { get; set; }
        public long sysdedefcon { get; set; }
        public long sysauskunft { get; set; }*/
        public override long getEntityId()
        {
            return sysratingauflage;
        }
    }
}
