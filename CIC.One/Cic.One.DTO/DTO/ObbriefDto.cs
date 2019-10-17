using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class ObbriefDto : EntityDto
    {
        override public long getEntityId()
        {
            return sysobbrief;
        }

        public long sysobbrief { get; set; }
        public String fart { get; set; }
        public long fkey { get; set; }
        public String aart { get; set; }
        public long akey { get; set; }
        public String hersteller { get; set; }
        public long hkey { get; set; }
        public String typ { get; set; }
        public long tkey { get; set; }
        public long auskey { get; set; }
        public String fident { get; set; }
        public long fidentkey { get; set; }
        public String antrieb { get; set; }
        public String antriebkey { get; set; }
        public long kmh { get; set; }
        public long kw { get; set; }
        public long ps { get; set; }
        public long hubraum { get; set; }
        public long last { get; set; }
        public long tank { get; set; }
        public long splatz { get; set; }
        public long sitze { get; set; }
        public long laenge { get; set; }
        public long breite { get; set; }
        public long hoehe { get; set; }
        public long leergew { get; set; }
        public long zulgew { get; set; }
        public long achsev { get; set; }
        public long achsem { get; set; }
        public long achseh { get; set; }
        public long raeder { get; set; }
        public long achsen { get; set; }
        public long angachsen { get; set; }
        public String reifv { get; set; }
        public String reifmuh { get; set; }
        public String alreifv { get; set; }
        public String alreifmuh { get; set; }
        public long ebremse { get; set; }
        public long zbremse { get; set; }
        public long kupform { get; set; }
        public long kuppruef { get; set; }
        public long lastamb { get; set; }
        public long lastaob { get; set; }
        public long stand { get; set; }
        public long fahr { get; set; }
        public DateTime? zulassung { get; set; }
        public long sstklasse { get; set; }
        public String zustand { get; set; }
        public DateTime? zustandam { get; set; }
        public long ok { get; set; }
        public long aktivkz { get; set; }
        public long sperrkz { get; set; }
        public long endekz { get; set; }
        public DateTime? endeam { get; set; }
        public String laufnummer { get; set; }
        public String impcode { get; set; }
        public String treibstoff { get; set; }
        public String getriebe { get; set; }
        public String motor { get; set; }
        public String energieeff { get; set; }
        public String felgv { get; set; }
        public String felgmuh { get; set; }
        public double verbrauchgesamt { get; set; }
        public double co2emi { get; set; }
        public double nox { get; set; }
        public double dpf { get; set; }
        public String motornummer { get; set; }
        public String ecodestatus { get; set; }
        public String ecodeid { get; set; }
        public String typengenehmigung { get; set; }
        public String mtyp { get; set; }
        public String masse { get; set; }
        public long partikel { get; set; }
        public long kat { get; set; }
        public long abs { get; set; }
        public long fairbag { get; set; }
        public long fbairbag { get; set; }
        public String beschreibung { get; set; }

    }
}
