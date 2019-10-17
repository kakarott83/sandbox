using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.P000001.Common
{
    /// <summary>
    /// ALL SerchParams for a given Level must be met for a node to be displayed
    /// equals AND-Search
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
    [System.Serializable]
    public class SearchParam : Cic.P000001.Common.ISearchParam
    {
       
        public SearchParam()
        {
        }

        public SearchParam(Cic.P000001.Common.Level searchAtLevel, string searchPattern)
        {
            this.SearchAtLevel = searchAtLevel;
            this.Pattern = searchPattern;
        }
        

       
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
        public string Pattern
        {
            get;
            set;
        }
       
       
        [System.Runtime.Serialization.DataMember(IsRequired = true)]
        public Cic.P000001.Common.Level SearchAtLevel
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember(IsRequired = true)]
        public SearchType searchType
        {
            get;
            set;
        }
       
    }

    /// <summary>
    /// ALL SerchParams for a given Level must be met for a node to be displayed
    /// equals AND-Search
    /// </summary>
    [System.Runtime.Serialization.DataContractAttribute(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
    [System.Serializable]
    public class TypedSearchParam : Cic.P000001.Common.ISearchParam
    {
        public TypedSearchParam() { }
        public TypedSearchParam(TypedSearchParam p)
        {
            this.Pattern = p.Pattern;
            this.searchType = p.searchType;
        }

        [System.Runtime.Serialization.DataMember(IsRequired = true)]
        public string Pattern
        {
            get;
            set;
        }

        [System.Runtime.Serialization.DataMember(IsRequired = true)]
        public SearchType searchType
        {
            get;
            set;
        }

    }

    [System.Serializable]
    public enum SearchType
    {
        NONE = 0,
        ANTRIEBSART,
        CO2,
        TREIBSTOFF,
        GETRIEBEART,
        AUFBAU,
        ZEITRAUMVON,
        ZEITRAUMBIS,
        MODELL,
        MARKE,
        /// <summary>
        /// Typgenehmigung
        /// </summary>
        TYPENCODE,
        OBJEKTTYP,
        MARKEBEZEICHNUNG,
        SCHWACKE,
        FUZZY,
        ID,
        /// <summary>
        /// Vehicle Identification Number - FIN
        /// </summary>
        VIN,
        KWVON,
        KWBIS,
        /// <summary>
        /// Typschlüsselnr 2.2
        /// </summary>
        TSN,
        /// <summary>
        /// HerstellerSchlüsselNr 
        /// </summary>
        HSN,
        /// <summary>
        /// Kommisionsnummer
        /// </summary>
        KOMMNR,
        /// <summary>
        /// strategischer Partner
        /// </summary>
        PARTNER
    }
}
