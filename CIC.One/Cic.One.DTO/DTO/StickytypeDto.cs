using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public class StickytypeDto : EntityDto
    {
        /*Primärschlüssel */
        public long sysStickytype { get; set; }
        public String bezeichnung { get; set; }
        /*Inhalt */
        public String codeStickytype { get; set; }
        /*sysCrtDate */
        public String sysCrtDate { get; set; }
        /*sysCrtTime */
        public String sysCrtTime { get; set; }
        /*sysCrtUser */
        public String sysCrtUser { get; set; }
        /*sysChgDate */
        public String sysChgDate { get; set; }
        /*sysChgTime */
        public String sysChgTime { get; set; }
        /*sysChgUser */
        public String sysChgUser { get; set; }
        /*coderfu */
        public String coderfu { get; set; }
        /*codermo*/
        public String codermo { get; set; }
        /*headercolor */
        public long headercolor { get; set; }
        /*bodycolor */
        public long bodycolor { get; set; }
        /*defaultflag */
        public int defaultflag { get; set; }
        /*fontname */
        public string fontname { get; set; }
        /*fontsize */
        public long fontsize { get; set; }
        /*dfontstyle */
        public long fontstyle { get; set; }
        /*fontcolor */
        public long fontcolor { get; set; }
        /*fontcharset */
        public long fontcharset { get; set; }
        /*csscontent */
        public string csscontent { get; set; }
        /*defaultshowflag */
        public int defaultshowflag { get; set; }

        override public long getEntityId()
        {
            return sysStickytype;
        }
    }
}
