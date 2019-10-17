using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class NkontoDto : EntityDto
    {
        override public long getEntityId()
        {
            return sysnkonto;
        }

        /// <summary>
        /// sysnkonto
        /// </summary>
        public long sysnkonto { get; set; }

        /// <summary>
        /// konto
        /// </summary>
        public String konto { get; set; }

        /// <summary>
        /// bezeichnung
        /// </summary>
        public String bezeichnung { get; set; }

        /// <summary>
        /// sysskonto
        /// </summary>
        public long sysskonto { get; set; }

        /// <summary>
        /// syskontokl
        /// </summary>
        public long syskontokl { get; set; }

        /// <summary>
        /// syskostart
        /// </summary>
        public long syskostart { get; set; }

        /// <summary>
        /// syskosttrae
        /// </summary>
        public long syskosttrae { get; set; }

        /// <summary>
        /// syskoststel
        /// </summary>
        public long syskoststel { get; set; }

        /// <summary>
        /// syswaehrung
        /// </summary>
        public long syswaehrung { get; set; }

        /// <summary>
        /// opflag
        /// </summary>
        public long opflag { get; set; }

        /// <summary>
        /// referenz
        /// </summary>
        public String referenz { get; set; }

        /// <summary>
        /// status
        /// </summary>
        public long status { get; set; }

        /// <summary>
        /// direkt
        /// </summary>
        public long direkt { get; set; }

        /// <summary>
        /// uebergabe
        /// </summary>
        public long uebergabe { get; set; }

        /// <summary>
        /// inaktiv
        /// </summary>
        public long inaktiv { get; set; }

        /// <summary>
        /// flagreserve1
        /// </summary>
        public long flagreserve1 { get; set; }

        /// <summary>
        /// flagreserve2
        /// </summary>
        public long flagreserve2 { get; set; }

        /// <summary>
        /// flagreserve3
        /// </summary>
        public long flagreserve3 { get; set; }

        /// <summary>
        /// sysreserve1
        /// </summary>
        public long sysreserve1 { get; set; }

        /// <summary>
        /// sysreserve2
        /// </summary>
        public long sysreserve2 { get; set; }

        /// <summary>
        /// sysreserve3
        /// </summary>
        public long sysreserve3 { get; set; }

        /// <summary>
        /// typ
        /// </summary>
        public long typ { get; set; }

        /// <summary>
        /// beginn
        /// </summary>
        public DateTime? beginn { get; set; }

        /// <summary>
        /// ende
        /// </summary>
        public DateTime? ende { get; set; }

        /// <summary>
        /// flagsaldiert
        /// </summary>
        public long flagsaldiert { get; set; }

        /// <summary>
        /// ewbbetrag
        /// </summary>
        public double ewbbetrag { get; set; }

        /// <summary>
        /// ewbam
        /// </summary>
        public DateTime? ewbam { get; set; }

        /// <summary>
        /// bewdatum
        /// </summary>
        public DateTime? bewdatum { get; set; }

        /// <summary>
        /// bewkurs
        /// </summary>
        public double bewkurs { get; set; }

        /// <summary>
        /// syskas
        /// </summary>
        public long syskas { get; set; }

        /// <summary>
        /// sysakonto
        /// </summary>
        public long sysakonto { get; set; }

        /// <summary>
        /// flagnianla
        /// </summary>
        public long flagnianla { get; set; }

        /// <summary>
        /// methode
        /// </summary>
        public long methode { get; set; }
    }


}
