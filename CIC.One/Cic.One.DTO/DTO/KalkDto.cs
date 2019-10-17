using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util;
namespace Cic.One.DTO
{
    public class KalkDto : EntityDto
    {
        /// <summary>
        /// Sys ID
        /// </summary>
        public long syskalk { get; set; }

        override public long getEntityId()
        {
            return syskalk;
        }

        override public String getEntityBezeichnung()
        {
            return bezeichnung;
        }

        public int rang { get; set; }
        public string bezeichnung { get; set; }
        public bool inantrag { get; set; }
        public long sysprproduct { get; set; }
        public double bgExtern { get; set; }
        public double bgIntern { get; set; }
        public double rabatto { get; set; }
        public double rabattv { get; set; }
        public double subventiono { get; set; }
        public double subventionv { get; set; }
        public double provision { get; set; }
        public bool modus { get; set; }
        public int ppy { get; set; }
        public double faktor { get; set; }
        public int lz { get; set; }
        public long ll { get; set; }
        public double zins { get; set; }
        public double zinsEff { get; set; }
        public double refiZins1 { get; set; }
        public double sz { get; set; }
        public double anzahlung { get; set; }
        public double rw { get; set; }
        public double rate { get; set; }
        public double db { get; set; }
        public double depot { get; set; }
        public int zinstyp { get; set; }
        public int syskalktyp { get; set; }
    }
}
