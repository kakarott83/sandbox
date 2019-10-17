using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class SegcDto :  EntityDto
    {
        /*Primärschlüssel */
        public long sysSegc { get; set; }
        /* Verweis zum Segment */
        public long sysSeg { get; set; }
        /* Verweis zur Kampagne */
        public long sysCamp { get; set; }


        override public long getEntityId()
        {
            return sysSegc;
        }
    }
}