using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Object Equipment Item
    /// </summary>
    public class MycalcaustDto : EntityDto
    {
        public long sysobaust { get; set; }
        public long sysob { get; set; }
        public long sysantob { get; set; }
        public long sysangob { get; set; }
        public String snr { get; set; }
        public String beschreibung { get; set; }
        public double betrag { get; set; }
        public long sysmycalc { get; set; }
        public int flagrwrel { get; set; }
        public int flagpacket { get; set; }


        public override long getEntityId()
        {
            return sysobaust;
        }
    }
}
