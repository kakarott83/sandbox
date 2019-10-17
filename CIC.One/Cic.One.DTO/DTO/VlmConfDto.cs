using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    /// <summary>
    /// VLM Configuration Entry
    /// </summary>
    public class VlmConfDto
    {
        public long sysvlmconf { get; set; }
        public long sysctlang { get; set; }
        public long sysls { get; set; }//default mandant id
        public int lschange { get; set; }//mandantwechsel erlaubt
        public String code { get; set; }
        public String coderfu { get; set; }
        public String codermo { get; set; }
        public String beschreibung { get; set; }
        public String initsequenz { get; set; }//start-dashboard vb eval
        public String finishsequenz { get; set; }
        public String checksequenz { get; set; }//vlm verfügbarkeitsprüfung (z.b. bos-db-version check)

        public String evalpicture { get; set; }//hintergrund-bild
        public String evallang { get; set; }
        public String designdatei { get; set; }//primefaces template id, dann templateselection in gui deaktiviert

        
        public int historyflag { get; set; }
        public int stickynotesflag { get; set; }
        public int chatflag { get; set; }
        public int volltextflag { get; set; }
        public int panelmanagerflag { get; set; }
        public int userconfigflag { get; set; }
        public int printersetupflag { get; set; }
        public int allowcontractsystoolbar { get; set; }
        public int flagpasswortreset { get; set; }
        public String backgroundpicture { get; set; }
        public String avatarpicture { get; set; }//logo oben links (Kunde)
        public String logopicture { get; set; }//logo unten links (CIC)

        /// <summary>
        /// contains default-mappings for areas 
        /// </summary>
        public List<VlmTableDto> tables { get; set; }

        public long syshauswaehrung { get; set; }//default currency
        public long sysmwst { get; set; }//default tax id
        /// <summary>
        /// available currencies
        /// </summary>
        public List<WaehrungDto> currencies { get; set; }
        /// <summary>
        /// available languages
        /// </summary>
        public List<LanguageDto> languages { get; set; }

        public String alertaction { get; set; }
        public String alerteval { get; set; }
        public long alertrefreshperiod  { get; set; }

        /// <summary>
        /// Global Security Settings
        /// </summary>
        public WfsysDto wfsys { get; set; }
        /// <summary>
        /// Global System settings
        /// </summary>
        public CicconfDto cicconf { get; set; }
    }
}
