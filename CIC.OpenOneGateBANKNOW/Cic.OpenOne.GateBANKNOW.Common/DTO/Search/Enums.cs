using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Search
{
    /// <summary>
    /// Enum Sort Order
    /// </summary>
    [System.CLSCompliant(true)]
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
    /// Filtertyp
    /// </summary>
    [System.CLSCompliant(true)]
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
        /// NotLike
        /// </summary>
        NotLike = 18



    }

    /// <summary>
    /// Suchart
    /// </summary>
    [System.CLSCompliant(true)]
    public enum SearchType : int
    {
        /// <summary>
        /// Informativ
        /// </summary>
        Informative = 0, 
        /// <summary>
        /// Vollständig
        /// </summary>
        Complete = 1,
        /// <summary>
        /// Teilweise
        /// </summary>
        Partial = 2,
  
    }
}

