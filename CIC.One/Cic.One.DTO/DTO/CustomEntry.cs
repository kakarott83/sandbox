using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    /// <summary>
    /// Defines all parameters for the view
    /// </summary>
    public class CustomEntry
    {
        /// <summary>
        /// wfv syscode for the gui detail-button
        /// </summary>
        public String detailsyscode { get; set; }
        /// <summary>
        /// wfv syscode for the list-gui new-button
        /// </summary>
        public String createsyscode { get; set; }
        /// <summary>
        /// wfv syscode to forward to after saving
        /// </summary>
        public String forwardsyscode { get; set; }
        /// <summary>
        /// title of the gui
        /// </summary>
        public String title { get; set; }
        /// <summary>
        /// icon of the gui
        /// </summary>
        public String icon { get; set; }
    /*}
    public class DetailEntry : CustomEntry
    {
        //...Detail spezifische dinge
    }
    public class ListEntry : CustomEntry
    {*/
        /// <summary>
        /// Filter-codes, comma-separated, to enable per default.
        /// Only Filter-Codes implemented in corresponding java-BO are supported
        ///      @Override public void addCustomFilters(Map<String, ListFilter> filters)  and there filter.put(CODE,listFilterInstance)
        /// Defaults are: OWNER, NONE
        /// </summary>
        public String filter { get; set; }

        /// <summary>
        /// GUI-Filter definitions for reducing list items 
        /// </summary>
        public List<Filter> filters { get; set; }


        public List<Filterpreset> filterpresets { get; set; }

        /// <summary>
        /// This sql will be appended to all list-querys as internal filter
        /// </summary>
        public String internalfilter { get; set; }

        /// <summary>
        /// comma separated fields from the searchQueryInfofactory-Query
        /// to search for user entered values
        /// </summary>
        public String filterfields { get; set; }


        /// <summary>
        /// initial sort fields
        /// </summary>
        public String sortfields { get; set; }

        /// <summary>
        /// initial sort order 
        /// </summary>
        public String sortorder { get; set; }

        /// <summary>
        /// SearchMode
        /// </summary>
        public SearchMode searchmode { get; set; }

        /// <summary>
        /// GUI layout description
        /// </summary>
        public ViewMeta viewmeta { get; set; }

        /// <summary>
        /// Filter-Panel Opened (Default is closed)
        /// </summary>
        public int filteropen { get; set; }

        /// <summary>
        /// ReadOnly View
        /// </summary>
        public int readOnly { get; set; }

        /// <summary>
        /// search instantly on view open
        /// </summary>
        public int instantSearch { get; set; }


    }
}