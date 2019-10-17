using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    /// <summary>
    /// Enum Sort Order
    /// </summary>
    
    public enum SortOrder : int
    {
        /// <summary>
        /// Aufsteigend
        /// </summary>
        Asc = 0,
        /// <summary>
        /// Absteigend
        /// </summary>
        Desc = 1
    }

    /// <summary>
    /// Filter Value Type
    /// </summary>
    public enum FilterValueType : int
    {
        LONG = 0,
        STRING,
        DATE,
        DOUBLE
    }
    /// <summary>
    /// Filter Value Inputcontrol types
    /// </summary>
    public enum FilterFieldType : int
    {
        INPUT = 0,
        MULTISELECT,
        SELECT,
        CALENDAR,
        CHECKBOX,
        BETWEEN,
        BETWEENNUMBER,
        MULTISELECTCODE,
        MULTISELECTCODEAC,
        MULTISELECTAC,
        PLACEHOLDER,
        MULTISELECTBEZ,
        SELECTCODE,
        SELECTBEZ,
        SELECTAC,
        SELECTCODEAC,
        BUTTON,
        FAVFILTEREND
    }

    /// <summary>
    /// Filtertyp
    /// </summary>

    public enum FilterType : int
    {
        /// <summary>
        /// Wie
        /// </summary>
        Like = 0,
        /// <summary>
        /// Gleich
        /// </summary>
        Equal = 1,
        /// <summary>
        /// Startet mit
        /// </summary>
        Begins = 2,
        /// <summary>
        /// Ended mit
        /// </summary>
        Ends = 3,
        /// <summary>
        /// Größer als
        /// </summary>
        GT = 4,
        /// <summary>
        /// Kleiner als
        /// </summary>
        LT = 5,
        /// <summary>
        /// Regular Expression (Regulärer Ausdruck)
        /// </summary>
        RegEx = 6,
        /// <summary>
        /// SQL
        /// </summary>
        SQL = 7,
        /// <summary>
        /// Web
        /// </summary>
        WEB = 8,
        /// <summary>
        /// Null
        /// </summary>
        Null = 9,
        /// <summary>
        /// Not null
        /// </summary>
        NotNull = 10,
        /// <summary>
        ///  Größergleich
        /// </summary>
        GE = 11,
        /// <summary>
        /// Kleinergleich
        /// </summary>
        LE = 12,
        /// <summary>
        ///  Datum gleich
        /// </summary>
        DateEqual = 13,
        /// <summary>
        ///  Datum größer als
        /// </summary>
        DateGT = 14,
        /// <summary>
        ///  Datum kleiner als
        /// </summary>
        DateLT = 15,
        /// <summary>
        ///  Datum größergleich
        /// </summary>
        DateGE = 16,
        /// <summary>
        /// Datum kleinergleich
        /// </summary>
        DateLE = 17,
        /// <summary>
        /// Web2
        /// </summary>
        WEB2 = 18,
        /// <summary>
        /// PERMISSION
        /// </summary>
        OWNER = 19,

        /// <summary>
        /// Mit Oder Verbunden und gleich wie Equal sonst
        /// </summary>
        OrEqual = 20,

        /// <summary>
        /// Nicht gleich
        /// </summary>
        NotEqual = 21,

        /// <summary>
        /// Date between Filter
        /// </summary>
        BETWEEN = 22,

        /// <summary>
        /// Number between Filter
        /// </summary>
        BETWEENNUMBER = 23,
        
        /// <summary>
        /// Replaces {FILTER.FIELDNAME} inside complete sql-query with filter value
        /// eg for Filter.Fieldname TEST it will replace {TEST} with the entered value
        /// used in frontoffice to inject certain values like flags or user ids
        /// </summary>
        REPLACE = 24,

        /// <summary>
        /// Unlike Filtertype like where the query will be extended with
        /// AND filter.fieldname like filter.value
        /// this will extend the query with
        /// filter.filterquery where filterquery must contain a {0} that will be replaced with a query-parameter holding the filter.value (user-input)
        /// e.g.
        /// filter.type=EXISTS
        /// filter.filterquery=EXISTS(select 1 from xy where z = {0})
        /// </summary>
        EXISTS = 25,

        /// <summary>
        /// Wie, nur upper ohne auto-%,* wird durch % ersetzt
        /// </summary>
        Like2 = 26
    }

    /// <summary>
    /// Suchart
    /// </summary>
    
    public enum SearchType : int
    {
        /// <summary>
        /// Informativ
        /// </summary>
        Informative = 0, 
        /// <summary>
        /// Vollständig
        /// </summary>
        Complete,
        /// <summary>
        /// Teilweise
        /// </summary>
        Partial,
  
    }

    /// <summary>
    /// Possible Search Modes
    /// </summary>
    public enum SearchMode : int
    {
        Unset=0,
        Default,
        IdCache,
        //does no count query but counts the results itself and adds rownum<=100
        RowNum,
        //does no count query but counts the results itself
        NoCount

    }
}

