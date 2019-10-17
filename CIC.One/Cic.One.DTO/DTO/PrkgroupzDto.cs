using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public class PrkgroupzDto : EntityDto
    {
        /*Primärschlüssel */
        public long sysprkgroupz { get; set; }
        /*Verweis zur Kundengruppe */
        public long sysprkgroup { get; set; }
        /*Verweis zur Rubrik */
        public long sysddlkprub { get; set; }
        /*Verweis zur Collection */
        public long sysddlkpcol { get; set; }

        override public long getEntityId()
        {
            return sysprkgroupz;
        }


    }
}