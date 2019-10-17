using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Erweitertes Xpro Dto, erweitert die alte DropList Dto um Zusatzdaten für ein schönes GUI-Panel mit Zeilen und Bild
    /// </summary>
    public class XproEntityDto : DropListDto
    {
        public XproEntityDto()
        {
        }
        public XproEntityDto(long sysId, String bezeichnung):this(sysId, bezeichnung, bezeichnung)
        {
            
        }
        public XproEntityDto(long sysId, String bezeichnung, String beschreibung)
        {
            this.sysID = sysId;
            this.bezeichnung = bezeichnung;
            this.beschreibung = beschreibung;
            this.title = this.bezeichnung;
        }
        public XproEntityDto(DropListDto dl)
        {
            //this.beschreibung = dl.beschreibung;
            //this.bezeichnung = dl.bezeichnung;
            this.code = dl.code;
            this.sysID = dl.sysID;

            this.title = dl.bezeichnung;
            this.desc1 = dl.beschreibung;
        }
        public String title { get; set; }
        public String desc1 { get; set; }
        public String desc2 { get; set; }
        public String desc3 { get; set; }
        public String indicator { get; set; }
    }
}
