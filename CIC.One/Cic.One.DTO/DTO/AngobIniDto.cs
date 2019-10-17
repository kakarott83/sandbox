using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// ANGOBAUST Entity, containing Offer Object Extended information
    /// </summary>
    public class AngobIniDto:EntityDto
    {
        public long sysobini { get; set; }
        public String typ_fz { get; set; }
        public DateTime? erstzul { get; set; }
        public String vorbesitzer { get; set; }
        public String vs_art { get; set; }
        public String kfzbrief { get; set; }
        public String depot { get; set; }
        public String farbe_i { get; set; }
        public String farbe_a { get; set; }
        public String aus_innen { get; set; }
        public int ps { get; set; }
        public int kw { get; set; }
        public int ccm { get; set; }
        public int kmstand { get; set; }
        public DateTime? tuev { get; set; }
        public DateTime? asu { get; set; }
        public String tk { get; set; }
        public String vk { get; set; }
        public double garantie { get; set; }
        public String garantie_text { get; set; }
        public String garantie_art { get; set; }
        public String antrag { get; set; }
        public String registrier { get; set; }
        public int nova { get; set; }
        public double nova_p { get; set; }
        public double nova_aw { get; set; }
        public String kfztyp { get; set; }
        public String motortyp { get; set; }
        public double verbrauch_s { get; set; }
        public double verbrauch_90 { get; set; }
        public long hubraum { get; set; }
        public String bemerkung { get; set; }
        public double kdzubehoer { get; set; }
        public String farbcode { get; set; }
        public int anztueren { get; set; }
        public String getrart { get; set; }
        public double verbrauch_d { get; set; }
        public long co2 { get; set; }
        public long actuation { get; set; }
        public double nox { get; set; }
        public double particles { get; set; }
        public String motorfuel { get; set; }
        public long co2def { get; set; }
        public double noxdef { get; set; }
        public double verbrauch_ddef { get; set; }
        public double particlesdef { get; set; }
        public int actuationdef { get; set; }
        public long sysprmart { get; set; }

        public int importer { get; set; }
        public int kraftstoff { get; set; }
        public int automatik { get; set; }
        public String wagentyp { get; set; }
        public String gehnid { get; set; }
        public String bezdeutsch { get; set; }
        /// <summary>
        /// primary key
        /// </summary>
        /// <returns></returns>
        override public long getEntityId()
        {
            return sysobini;
        }
    }
}
