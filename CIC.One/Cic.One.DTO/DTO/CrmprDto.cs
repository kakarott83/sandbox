using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public class CrmprDto : EntityDto
    {
        /*Primärschlüssel */
        public long sysCrmPr { get; set; }
        /*Verweis zum Produkt */
        public long sysPrproduct { get; set; }
        /*Gebiet (zb Opportunity, Kampagne ...) */
        public String area { get; set; }
        /*Verweis zum Satz im Gebiet */
        public long sysId { get; set; }

        override public long getEntityId()
        {
            return sysCrmPr;
        }
    }
}