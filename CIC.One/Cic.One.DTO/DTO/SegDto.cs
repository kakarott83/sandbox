using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class SegDto : EntityDto
    {

        /*Primärschlüssel */
        public long sysSeg { get; set; }
        /*Verweis zu Wfuser, Segmentinhaber */
        public long sysOwner { get; set; }
        /*Privat, nur für Inhaber sichtbar (trotz Gesichtskreis) */
        public int privateFlag { get; set; }
        /*Bezeichnung */
        public String name { get; set; }
        /*Beschreibung */
        public String description { get; set; }
        /*Aktiv */
        public int activeFlag { get; set; }

        override public long getEntityId()
        {
            return sysSeg;
        }


    }
}