using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public class AntragzustandDto : EntityDto
    {
        override public long getEntityId()
        {
            return sysid;
        }

        override public String getEntityBezeichnung()
        {
            return zustand;
        }

        public string antrag { get; set; }


        public string benutzer { get; set; }

        
        public DateTime? adatum { get; set; }


        public DateTime? aenderung { get; set; }


        public int aktivkz { get; set; }


        public string angebot { get; set; }


        public string attribut { get; set; }


        public string attributam { get; set; }


        public int aufschub { get; set; }


        public DateTime? beginn { get; set; }


        public double bgextern { get; set; }


        public double ahk { get; set; }


        public DateTime? checkedon { get; set; }


        public DateTime? databschluss { get; set; }


        public DateTime? dataktiv { get; set; }


        public DateTime? datangebot { get; set; }


        public DateTime? dateinreichung { get; set; }


        public long dateinreichungzeit { get; set; }


        public DateTime? druck { get; set; }


        public int drucksperre { get; set; }


        public DateTime? edatum { get; set; }


        public DateTime? ende { get; set; }


        public DateTime? endeam { get; set; }


        public int endekz { get; set; }


        public DateTime? erfassung { get; set; }


        public int erfassungsclient { get; set; }


        public long erfassungzeit { get; set; }


        public string extreferenz { get; set; }


        public int flagbwgarantie { get; set; }


        public int flagfreigabeausz { get; set; }


        public int flagfreigabeform { get; set; }


        public DateTime? freigabeausz { get; set; }


        public DateTime? freigabeform { get; set; }


        public double grund { get; set; }


        public DateTime? gueltigbis { get; set; }


        public string gutschriftreferenz { get; set; }


        public string kartennummer { get; set; }


        public int kkgpflicht { get; set; }


        public string konstellation { get; set; }


        public int locked { get; set; }


        public DateTime? lockedon { get; set; }


        public int lz { get; set; }


        public int notstopflag { get; set; }


        public string objektvt { get; set; }


        public int ok { get; set; }


        public int ppy { get; set; }


        public int rangki { get; set; }


        public double rate { get; set; }


        public double rgg { get; set; }


        public double rw { get; set; }


        public long sysabwicklung { get; set; }


        public long sysberater { get; set; }


        public long syshantrag { get; set; }


        public long sysid { get; set; }


        public long sysit { get; set; }


        public long sysls { get; set; }


        public long sysmarktab { get; set; }


        public long sysmwst { get; set; }


        public long sysoppo { get; set; }


        public long sysprchannel { get; set; }


        public long sysprhgroup { get; set; }


        public long sysprproduct { get; set; }


        public long sysrwga { get; set; }


        public long sysvart { get; set; }


        public long sysvk { get; set; }


        public long sysvm { get; set; }


        public long sysvttyp { get; set; }


        public long syswaehrung { get; set; }


        public long syswfuser { get; set; }


        public long syswfuserausz { get; set; }


        public long syswfuserchange { get; set; }


        public long syswfuserform { get; set; }


        public long syswfuserprint { get; set; }


        public double sz { get; set; }


        public int testflag { get; set; }


        public string vart { get; set; }


        public string vertriebsweg { get; set; }


        public string verwendungszweckcode { get; set; }


        public string zustand { get; set; }


        public DateTime? zustandam { get; set; }
    }
}