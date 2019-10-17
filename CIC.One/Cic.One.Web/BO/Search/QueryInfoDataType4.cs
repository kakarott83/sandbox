using Cic.One.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.Web.BO.Search
{
    /// <summary>
    /// Query for user-defined query, no counting etc
    /// used for GVIEW with I_UKEY pkey as select-list id
    /// used for reporting
    /// </summary>
    public class QueryInfoDataType4 : QueryInfoDataType1
    {
        public QueryInfoDataType4()
        {
            _queryStructure = new QueryStructure();
            _queryStructure.countQuery = "select count(*) from  {0} where {1} {2} {3}";
            _queryStructure.completeQuery = "select  {0} from {1} where {2} {3} {4}";
            _queryStructure.partialQuery = partialQueryDefault;
            if (overridePartialQuery != null)
                _queryStructure.partialQuery = overridePartialQuery;
            searchConditions = " 1=1 ";
        }

        public QueryInfoDataType4(String entityTable, String entityField)
            : this()
        {
            this.entityField = entityField;
            this.entityTable = entityTable;
            resultFields = entityTable + ".*";
            resultTables = entityTable;
            searchTables = entityTable;
            searchConditions = " 1=1 ";
        }
        /// <summary>
        /// Creates a search-Query from the query configuration using the given primary-key fetching prefix
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pkeyPrefix"></param>
        public QueryInfoDataType4(Query query, String pkeyPrefix, SearchMode sm)
            : this()
        {
            this.entityField = query.pkey;
            this.entityTable = query.table;
            this.searchMode = sm;
            resultFields = "";
            if(String.IsNullOrEmpty(query.group))
                resultFields = this.entityField + " I_UKEY, ";
            
            if(pkeyPrefix!=null && pkeyPrefix.Length>0)
                resultFields += pkeyPrefix + ", ";
            
            resultFields+=query.fields;
            groupby = query.group;
            resultTables = query.tables;
            searchTables = query.tables;
            searchConditions = query.where;
            permissionCondition = query.permissioncondition;
            optimized = false;
        }
        public QueryInfoDataType4(QueryInfoDataType4 org)
            : base(org)
        {

        }
        override public QueryInfoData clone()
        {
            return new QueryInfoDataType4(this);
        }
    }
}