using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Cic.One.DTO
{
    /// <summary>
    /// Filter Klasee
    /// </summary>
    public class Filter
    {
        /// <summary>
        /// Filter Feldname
        /// </summary>
        public String fieldname { get; set; }
        /// <summary>
        /// Filterwert
        /// </summary>
        public String value { get; set; }
        /// <summary>
        /// Filterart
        /// </summary>
        public FilterType filterType { get; set; }

        
        /// <summary>
        /// all Filters with the same group will be concatenated into one bracket ( a=1 OR b=2 )
        /// in the GUI all Filters with same group will be combined into one Input-Element (preferred MULTISELECT)
        /// </summary>
        public String groupName { get; set; }


        /// <summary>
        /// flag indicating that groupName has to be used as or-Group
        /// </summary>
        public bool orFilter { get; set; }
        /// <summary>
        /// flag indicating that groupName has to be used as and-Group
        /// </summary>
        public bool andFilter { get; set; }

        /// <summary>
        /// Values for OR-/AND-Filtering can be used to group multiple values to compare with in one filter entity
        /// alternatively one can use another filter with same groupname and different value
        /// </summary>
        public String[] values { get; set; }

        /// <summary>
        /// Values for Fieldnames in or-Filter can be used to group multiple fieldnames to compare with in one filter entity
        /// alternatively one can use another filter with same groupname and different fieldname
        /// </summary>
        public String[] groupFields { get; set; }


        //Fields for representing the Filter in a GUI---------------------------------------------------------------------------------
        
        
        /// <summary>
        /// Code for pre-fill the filter selections by a xpro
        /// </summary>
        public String xprocode { get; set; }

        /// <summary>
        /// comma-separated list of name:value pairs for a selection
        /// </summary>
        public String optionvalues { get; set; }

        /// <summary>
        /// GUI Description
        /// </summary>
        public String description { get; set; }

        /// <summary>
        /// Type of the value for GUI converter
        /// </summary>
        public FilterValueType valueType { get; set; }

        /// <summary>
        /// Type of the GUI input Field
        /// </summary>
        public FilterFieldType fieldType { get; set; } 

        /// <summary>
        /// When true the filter is default enabled
        /// </summary>
        public bool enabled { get; set; }

        /// <summary>
        /// When true the filter may not be disabled
        /// </summary>
        public bool locked { get; set; }

        /// <summary>
        /// When true the items will be translated in frontend (used for non-translated dropdowns)
        /// </summary>
        public bool translate { get; set; }

        /// <summary>
        /// Causes another filter with the given xpro to refresh and using this filters values
        /// </summary>
        public String triggerxpro { get; set; }

        /// <summary>
        /// Filter will be visible only for the bperoles defined here
        /// eg SBFG,SBFK
        /// </summary>
        public String bperoles { get; set; }

        /// <summary>
        /// Holds the query used for Filtertype EXISTS like 
        /// "EXISTS(select 1 from xy where z = {0})"
        /// must contain a {0} that will hold the filter input value
        /// </summary>
        public String filterquery { get; set; }

        /// <summary>
        /// set to true when the filter uses fields like sysperole or other filters that have to change on every new gui-creation
        /// </summary>
        public bool nocache { get; set; }
    }
}
