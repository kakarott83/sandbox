using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO
{
    public class StrasseDto : EntityDto
    {
        override public String getEntityBezeichnung()
        {
            return strasse;
        }

        /*Verweis zum Land */
        public long sysStrasse{ get; set; }

        /*Verweis zum Land */
        public long sysLand { get; set; }

        /* Verweis zum PLZ*/
        public long sysPlz { get; set; }

        /*Verweis zum Land Bezeichnung*/
        public String landBezeichnung { get; set; }

        /*Verweis zur Sprache */
        public long sysCtLang { get; set; }

        /*Verweis zur Sprache Bezeichnung*/
        public String ctLangBezeichnung { get; set; }

        /*Bezirk Bezeichnung*/
        public String bezirk { get; set; }

        /*Strasse */
        public String strasse { get; set; }

        /*Postleitzahl */
        public String plz { get; set; }

        /*Ort */
        public String ort { get; set; }

        //Land
        public String land { get; set; }

        override public long getEntityId()
        {
            return sysStrasse;
        }
    }
}