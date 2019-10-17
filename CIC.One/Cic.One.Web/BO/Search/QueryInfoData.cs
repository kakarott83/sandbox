using Cic.One.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.Web.BO.Search
{
    /// <summary>
    /// Base Class for Query-Information used in the QueryPattern
    /// SPECIAL replacementfilter for query: {FILTERCONDITIONS} will be replaced with all filterconditions
    /// </summary>
    public abstract class QueryInfoData
    {
        protected QueryStructure _queryStructure;
        
        public String resultTables { get; set; }
        public String searchTables { get; set; }
        /// <summary>
        /// the where-part of the query
        /// </summary>
        public String searchConditions { get; set; }
        public String groupby { get; set; }
        public String orderby { get; set; }

        public String entityField { get; set; }
        public String entityTable { get; set; }
        public String permissionCondition { get; set; }
        public String permissionTables { get; set; }
        

        public String resultFields { get; set; }
        public String pageSize { get; set; }
        public String startRow { get; set; }
        public bool optimized { get; set; }
        public SearchMode searchMode { get; set; }

        protected String resultConditions { get; set; }
        protected String idFields { get; set; }

        private String additionalTables { get; set; }
        private String additionalSearchConditions { get; set; }
        private String sortConditions { get; set; }

        public Dictionary<String, String> replaceStrings { get; set; }

        public QueryInfoData()
        {
            optimized = false;
            additionalSearchConditions = "";
            sortConditions = "";
            additionalTables = "";
            replaceStrings = new Dictionary<string, string>();
            searchMode = SearchMode.Default;
        }

       
        public QueryInfoData(QueryInfoData org)
        {
            this._queryStructure = org._queryStructure;
            this.resultFields = org.resultFields;
            this.resultTables = org.resultTables;
            this.idFields = org.idFields;
            this.searchTables = org.searchTables;
            this.searchConditions = org.searchConditions;
            this.pageSize = org.pageSize;
            this.groupby = org.groupby;
            this.startRow = org.startRow;
            this.resultConditions = org.resultConditions;
            this.optimized = org.optimized;
            this.entityField = org.entityField;
            this.entityTable = org.entityTable;
            this.permissionCondition = org.permissionCondition;
            this.permissionTables = org.permissionTables;
            this.searchMode = org.searchMode;

            this.replaceStrings = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> pair in org.replaceStrings)
            {
                this.replaceStrings.Add(pair.Key, pair.Value);
            }
            
        }
        
        public QueryStructure queryStruct
        {
            get { return _queryStructure; }
            set { _queryStructure = value; }

        }
        public abstract QueryInfoData clone();

        public String getAdditionalTables() { return additionalTables; }
        /// <summary>
        /// Delivers the parameters for the count query 
        /// </summary>
        /// <returns></returns>
        protected abstract object[] getCountParams();
        /// <summary>
        /// Delivers the parameters for the complete query
        /// </summary>
        /// <returns></returns>
        protected abstract object[] getCompleteParams();
        /// <summary>
        /// Delivers the parameters for the partial query
        /// </summary>
        /// <returns></returns>
        protected abstract object[] getPartialParams();
        private long permissionId;
        /// <summary>
        /// Returns the permission filter subquery, appended to the where-condition
        /// </summary>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        public String getPermissionCondition(long permissionId, String filter)
        {
            this.permissionId = permissionId;
            if (permissionCondition == null || permissionCondition.Length == 0 || permissionId==0) return "";
            return String.Format(permissionCondition,new object[]{permissionId, filter!=null?filter:"1=1"});
        }

        public virtual String getCountQuery()
        {
            String query = String.Format(queryStruct.countQuery, getCountParams());

            foreach (KeyValuePair<string, string> replace in replaceStrings)
            {
                query = query.Replace("{" + replace.Key + "}", replace.Value);
            }
            query = setFilterConditions(query, getSearchConditions());
            return query;
        }

        public virtual String getCompleteQuery()
        {
            String query = String.Format(queryStruct.completeQuery, getCompleteParams());

            foreach (KeyValuePair<string, string> replace in replaceStrings)
            {
                query = query.Replace("{" + replace.Key + "}", replace.Value);
            }
            query = setFilterConditions(query, getSearchConditions());
            return query;
        }
        /// <summary>
        /// Replaces all replace-filters in the query conditions and replaces {FILTERCONDITIONS} inside the query with them
        /// </summary>
        /// <param name="query"></param>
        /// <param name="conditions"></param>
        /// <returns></returns>
        private String setFilterConditions(String query, String conditions)
        {
            foreach (KeyValuePair<string, string> replace in replaceStrings)
            {
                conditions = conditions.Replace("{" + replace.Key + "}", replace.Value);
            }
            String rval=query.Replace("{FILTERCONDITIONS}", conditions);
            rval = rval.Replace("{SYSPEROLE}", ""+permissionId);
            return rval;
        }
        public virtual String getPartialQuery()
        {
            String query = String.Format(queryStruct.partialQuery, getPartialParams());

            foreach (KeyValuePair<string, string> replace in replaceStrings)
            {
                query = query.Replace("{" + replace.Key + "}", replace.Value);
            }
            query = setFilterConditions(query, getSearchConditions());
            return query;
        }

        public virtual String getSearchConditions()
        {
            return searchConditions + " " + additionalSearchConditions+" "+sortConditions;
        }
        public virtual String getGroupBy()
        {
            if (String.IsNullOrEmpty(groupby))
                return "";
            return " group by " + groupby;
        }
        public virtual String getOrderBy()
        {
            if (String.IsNullOrEmpty(orderby))
                return "";
            return " ORDER by " + orderby;
        }

        public virtual void clearAdditionalSearchConditions()
        {
            additionalSearchConditions = "";
        }
        public virtual void addAdditionalSearchConditions(String cond)
        {
            additionalSearchConditions += cond;
        }
        public virtual void addSortConditions(String cond)
        {
            sortConditions += cond;
        }
        public virtual void clearSortConditions()
        {
            sortConditions = "";
        }
        public virtual void addAdditionalTables(String tables)
        {
            additionalTables += tables;
        }
    }
}