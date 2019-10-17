using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    public class AbfragedatenDto : EntityDto
    {
        /// <summary>
        /// primary key
        /// </summary>
        public long sysauskunft { get; set; }
        /// <summary>
        /// request reason
        /// </summary>
        public long sysauskunfttyp { get; set; }
        /// <summary>
        /// request date
        /// </summary>
        public DateTime anfragedatum { get; set; }
        /// <summary>
        /// request time
        /// </summary>
        public long anfrageuhrzeit { get; set; }
        /// <summary>
        /// kontext area, defining the table sysid is referring to
        /// </summary>
        public string area { get; set; }
        /// <summary>
        /// kontext item foreign key pointing another table depending on kontext area
        /// </summary>
        public long sysid { get; set; }
        /// <summary>
        /// user committing the request
        /// </summary>
        public long syswfuser { get; set; }


        /// <summary>
        /// get ID
        /// </summary>
        /// <returns>primary key</returns>
        public override long getEntityId()
        {
            return sysauskunft;
        }
    }
}
