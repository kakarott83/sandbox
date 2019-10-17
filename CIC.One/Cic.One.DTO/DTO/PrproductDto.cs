using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public class PrproductDto : EntityDto
    {
        /*Primärschlüssel */
        public long sysPrProduct { get; set; }
        /*Externer Name */
        public String name { get; set; }
        /*Interner Name */
        public String nameIntern { get; set; }
        /*Aktiv */
        public bool activeFlag { get; set; }
        /*Beginn */
        public DateTime? validFrom { get; set; }
        /*Ende */
        public DateTime? validUntil { get; set; }


        public long sysVart { get; set; }
        public long sysVartTab { get; set; }
        public long sysVttyp { get; set; }
        public long sysPrprodtype { get; set; }
        public long sysKalktyp { get; set; }
        public long sysInttype { get; set; }
        public long sysPrrap { get; set; }
        public String code { get; set; }
        public String description { get; set; }
        public String vartcode { get; set; }
        override public long getEntityId()
        {
            return sysPrProduct;
        }
    }
}