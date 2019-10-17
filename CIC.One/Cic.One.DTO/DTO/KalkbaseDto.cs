using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class KalkbaseDto : EntityDto 
    {
        override public String getEntityBezeichnung()
        {
            return bezeichnung;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        override public long getEntityId()
        {
            return syskalk;
        }

        public String bezeichnung { get; set; }


        public double ahk { get; set; }


        public double ahknk { get; set; }


        public double ahknkbrutto { get; set; }


        public double ahknkust { get; set; }


        public double anzahlung { get; set; }


        public double anzahlungp { get; set; }


        public double auszahlung { get; set; }


        public int? auszahlungtyp { get; set; }


        public double basiszins { get; set; }


        public DateTime? beginn { get; set; }
        public DateTime? ersterate { get; set; }
        public DateTime? letzterate { get; set; }
        public DateTime? valutag { get; set; }
        public DateTime? valutaa { get; set; }
        public DateTime? ende { get; set; }
        

        public double bgextern { get; set; }


        public double bgexternbrutto { get; set; }


        public double bgexternexkln { get; set; }


        public double bgexternnova { get; set; }


        public double bgexternust { get; set; }


        public double bgintern { get; set; }


        public double bginternbrutto { get; set; }


        public double bginternust { get; set; }


        public int? calctarget { get; set; }


        public double db { get; set; }


        public double dbp { get; set; }


        public double depot { get; set; }


        public double depotp { get; set; }


        public double disagio { get; set; }


        public int? flagsoko { get; set; }


        public double fzuabschlag2 { get; set; }


        public double fzuabschlag3 { get; set; }


        public double fzuabschlag4 { get; set; }


        public double gebuehr { get; set; }


        public double gebuehrbrutto { get; set; }


        public double gebuehrinternbrutto { get; set; }


        public double gebuehrnachlass { get; set; }


        public double gebuehrust { get; set; }


        public double gesamt { get; set; }


        public double gesamtbrutto { get; set; }


        public double gesamtkosten { get; set; }


        public double gesamtkostenbrutto { get; set; }


        public double gesamtkostenust { get; set; }


        public double gesamtkredit { get; set; }


        public double gesamtnetto { get; set; }


        public double gesamtust { get; set; }


        public double grund { get; set; }


        public int? holdfields { get; set; }


        public long? ll { get; set; }


        public int? lz { get; set; }


        public double marge { get; set; }


        public double margep { get; set; }


        public double mitfin { get; set; }


        public double mitfinb { get; set; }


        public double mitfinbrutto { get; set; }


        public double mitfinust { get; set; }


        public int? modus { get; set; }


        public double pakete { get; set; }


        public double pakrabo { get; set; }


        public double pakrabop { get; set; }


        public double pakrabv { get; set; }


        public double pakrabvp { get; set; }


        public int? ppy { get; set; }


        public double provision { get; set; }


        public double provisionp { get; set; }


        public double rabatto { get; set; }


        public double rabattop { get; set; }


        public double rabattv { get; set; }


        public double rabattvp { get; set; }


        public double rapratebruttomax { get; set; }


        public double rapratebruttomin { get; set; }


        public double raprsvmonatmax { get; set; }


        public double raprsvmonatmin { get; set; }


        public double rapzinseffmax { get; set; }


        public double rapzinseffmin { get; set; }


        public double rapzinskostenmax { get; set; }


        public double rapzinskostenmin { get; set; }


        public double rate { get; set; }


        public double ratebrutto { get; set; }


        public double rategesamt { get; set; }


        public double rategesamtbrutto { get; set; }


        public double rategesamtust { get; set; }


        public double rateust { get; set; }


        public double refizins1 { get; set; }


        public double restkaufpreis { get; set; }


        public double rggebuehr { get; set; }


        public int? rggfrei { get; set; }


        public int? rggverr { get; set; }


        public double rsvgesamt { get; set; }


        public double rsvmonat { get; set; }


        public double rsvzins { get; set; }


        public int? rueckzahlungtyp { get; set; }


        public double rw { get; set; }


        public double rwbase { get; set; }


        public double rwbasebrutto { get; set; }


        public double rwbasebruttop { get; set; }


        public double rwbaseust { get; set; }


        public double rwbrutto { get; set; }


        public double rwcrv { get; set; }


        public double rwcrvbrutto { get; set; }


        public double rwcrvbruttop { get; set; }


        public double rwcrvust { get; set; }


        public double rwkalk { get; set; }


        public double rwkalkbrutto { get; set; }


        public double rwkalkbruttodef { get; set; }


        public double rwkalkbruttoorg { get; set; }


        public double rwkalkbruttop { get; set; }


        public double rwkalkbruttopdef { get; set; }


        public double rwkalkbruttoporg { get; set; }


        public double rwkalkdef { get; set; }


        public double rwkalkust { get; set; }


        public double rwkalkustdef { get; set; }


        public double rwp { get; set; }


        public double rwust { get; set; }


        public string schwacke { get; set; }


        public double? servicefee { get; set; }


        public double sonzub { get; set; }


        public double sonzubrabo { get; set; }


        public double sonzubrabop { get; set; }


        public double sonzubrabv { get; set; }


        public double sonzubrabvp { get; set; }


        public double subventiono { get; set; }


        public double subventionv { get; set; }


        


        public DateTime? syschange { get; set; }


        public DateTime? syscreate { get; set; }


        public long? sysintstrct { get; set; }


        public long? sysinttype { get; set; }


        public long syskalk { get; set; }


        public long? syskalktyp { get; set; }
        public long sysprproduct { get; set; }

        public long? sysvt { get; set; }


        public long? syswaehrung { get; set; }


        public double sz { get; set; }


        public double szbrutto { get; set; }


        public double szbruttop { get; set; }


        public double szp { get; set; }


        public double szust { get; set; }


        public double ustzins { get; set; }


        public double verrechnung { get; set; }


        public int? verrechnungflag { get; set; }


        public double zins { get; set; }


        public double zins1 { get; set; }


        public double zins2 { get; set; }


        public double zins3 { get; set; }


        public double zins4 { get; set; }


        public double zins5 { get; set; }


        public double zinscust { get; set; }


        public double zinsdef { get; set; }


        public double zinseff { get; set; }


        public double zinseffdef { get; set; }


        public double zinskosten { get; set; }


        public double zinsrap { get; set; }


        public int? zinstyp { get; set; }


        public double zollgebuehr { get; set; }


        public double zollgebuehrp { get; set; }


        public double zuabschlag1 { get; set; }


        public double zuabschlag2 { get; set; }


        public double zuabschlag4 { get; set; }


        public double zubehoer { get; set; }


        public double zubehoerbrutto { get; set; }


        public double zubehoernetto { get; set; }


        public double zubehoeror { get; set; }


        public double zubehoerorp { get; set; }


        public double zubehoerp { get; set; }


        public string zustand { get; set; }


        public DateTime? zustandam { get; set; }

        public long sysfstypbsi { get; set; }

        

        /// <summary>
        /// Zahlplan / Staffelpositionen
        /// </summary>
        public List<SlDto> zahlplan { get; set; }

    }
}
