using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    /// <summary>
    /// Describes defaultbehaviour for all entities (list, detail, indexing)
    /// 
    /// Lucene: descriptions are defined in luceneconfig.xml inside the query, the indexed fields are mapped to title, desc1,...
    /// Xpro: when entry for Xpro-Area found, uses this template to fetch title, desc1,..., else the programmed getdescription method is used
    /// Favorites-GUI: searches for correct entry and uses the configured detailsyscode
    /// Recent-GUI:  searches for correct entry and uses the configured detailsyscode
    /// Lucene-Search: searches for all fulltext-areas to display in the searchpage
    /// </summary>
    public class VlmTableDto
    {
        public long sysvlmtable {get;set;}
        public long sysvlm {get;set;}
        public long syswftable {get;set;}
        public String vlmcode { get; set; }
        /// <summary>
        /// Syscode aus WFTABLE
        /// </summary>
        public String syscode { get; set; }

        public String listsyscode {get;set;}
        public String detailsyscode {get;set;}
        /// <summary>
        /// for description in xpro dropdowns, replaces getBezeichnung()
        /// </summary>
        public String template_short {get;set;}
        /// <summary>
        /// Fields for building an infopanel, replaces xpro getBeschreibung()
        /// </summary>
        public String template_title {get;set;}
        public String template_description1 {get;set;}
        public String template_description2 {get;set;}
        public String template_description3 {get;set;}
        public String icon {get;set;}

        /// <summary>
        /// Fulltext Flag
        /// </summary>
        public int fulltext {get;set;}
        public int fulltextrang {get;set;}
    }
}
