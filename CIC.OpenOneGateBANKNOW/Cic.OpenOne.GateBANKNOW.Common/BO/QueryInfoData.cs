using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Defines the Query Pattern for the three Search-Modes
    /// </summary>
    public class QueryStructure
    {
        public String countQuery { get; set; }
        public String partialQuery { get; set; }
        public String completeQuery { get; set; }
    }
    /// <summary>
    /// Base Class for Query-Information used in the QueryPattern
    /// </summary>
    public abstract class QueryInfoData
    {
        protected QueryStructure _queryStructure;
        public String resultFields_0 { get; set; }
        public String resultTables_1 { get; set; }
        public String idFields_2 { get; set; }
        public String searchTables_3 { get; set; }
        public String searchConditions_4 { get; set; }
        public String pageSize_5 { get; set; }
        public String startRow_6 { get; set; }
        public String resultConditions_7 { get; set; }
        public bool optimizeQuery { get; set; }
        public QueryStructure queryStruct
        {
            get { return _queryStructure; }
            set { _queryStructure = value; }

        }
        public QueryInfoData()
        {
            optimizeQuery = false;
     
        }
        public QueryInfoData(QueryInfoData org)
        {
            this._queryStructure = org._queryStructure;
            this.resultFields_0 = org.resultFields_0;
            this.resultTables_1 = org.resultTables_1;
            this.idFields_2 = org.idFields_2;
            this.searchTables_3 = org.searchTables_3;
            this.searchConditions_4 = org.searchConditions_4;
            this.pageSize_5 = org.pageSize_5;
            this.startRow_6 = org.startRow_6;
            this.resultConditions_7 = org.resultConditions_7;
            this.optimizeQuery = org.optimizeQuery;
           

        }
        public abstract QueryInfoData clone();
        /// <summary>
        /// Delivers the parameters for the count query 
        /// </summary>
        /// <returns></returns>
        public abstract object[] getCountParams();
        /// <summary>
        /// Delivers the parameters for the complete query
        /// </summary>
        /// <returns></returns>
        public abstract object[] getCompleteParams();
        /// <summary>
        /// Delivers the parameters for the partial query
        /// </summary>
        /// <returns></returns>
        public abstract object[] getPartialParams();
    }
}
