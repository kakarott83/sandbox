using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// ObIniDto Entity, containing VT Object Extended information
    /// </summary>
    public class ObIniDto:EntityDto
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
        public long ps { get; set; }
        public long kw { get; set; }
        public long ccm { get; set; }
        public long kmstand { get; set; }
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
        public int actuation { get; set; }
        public double nox { get; set; }
        public double particles { get; set; }
        public String motorfuel { get; set; }
        public int hybrid { get; set; }
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
