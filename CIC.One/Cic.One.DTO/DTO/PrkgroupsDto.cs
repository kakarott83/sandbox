using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public class PrkgroupsDto : EntityDto
    {
        /*Primärschlüssel */
        public long sysPrkgroups { get; set; }
        /*Verweis zur Kundengruppe */
        public long sysPrkgroup { get; set; }
        /*Verweis zum Segment */
        public long sysSeg { get; set; }

        override public long getEntityId()
        {
            return sysPrkgroups;
        }
    }
}