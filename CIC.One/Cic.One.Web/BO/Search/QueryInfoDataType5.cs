using Cic.One.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.Web.BO.Search
{
    /// <summary>
    /// Query for user-defined query including union etc, no counting etc
    /// used when query.query is set in wfvconfig
    /// 
    /// requirements to the query: the where condition must be present e.g. where 1=1
    /// 
    /// </summary>
    public class QueryInfoDataType5 : QueryInfoData
    {
        private String query = "";
		private ViewMeta queryConfig;

        public QueryInfoDataType5()
        {
//            select rownum sysreport, a.* from (select to_char(beginn, 'YYYY-MM') X,sum(bgextern) A1,sum(rw) A2, sum (sz) A3 from vt where beginn>='01.01.2014' and beginn<='01.12.2014' group by to_char(beginn, 'YYYY-MM') order by to_char(beginn, 'YYYY-MM')) a;
            _queryStructure = new QueryStructure();
            _queryStructure.countQuery = "select count(*) from ({0}) a where 1=1 {1}";
            _queryStructure.completeQuery = "select rownum sysrownum, tab.* from ({0}) tab where 1=1 {1} {2}";
            _queryStructure.partialQuery = "select rownum sysrownum, tab.* from ({0}) tab where 1=1 {1} {2}";
            searchConditions = " ";
        }


         /// <summary>
        /// Creates a search-Query from the query configuration using the given primary-key fetching prefix
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pkeyPrefix"></param>
        public QueryInfoDataType5(ViewMeta meta, String pkeyPrefix, SearchMode sm)
            : this()
        {
           /* this.entityField = query.pkey;
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
            searchConditions = query.where;*/
            this.searchMode = sm;
            permissionCondition = meta.query.permissioncondition;
            this.query = meta.query.query;            
            optimized = false;
			this.queryConfig = meta;
        }

        public QueryInfoDataType5(String query) :this()
        {
            this.entityField = entityField;
            this.entityTable = entityTable;
            resultFields = entityTable + ".*";
            resultTables = entityTable;
            searchTables = entityTable;
            searchConditions = " ";
            this.query = query;
			
            
        }
        public QueryInfoDataType5(QueryInfoDataType5 org):base(org)
        {
            this.query = org.query;
			this.queryConfig = org.queryConfig;
        }
        override public QueryInfoData clone()
        {
            return new QueryInfoDataType5(this);
        }
        /*public virtual String getCompleteQuery()
        {
            String query = String.Format(queryStruct.completeQuery, getCompleteParams());

            foreach (KeyValuePair<string, string> replace in replaceStrings)
            {
                query = query.Replace("{" + replace.Key + "}", replace.Value);
            }
            return query;
        }



        override public String getCompleteQuery()
        {
            return String.Format(String.Format(queryStruct.completeQuery, getCompleteParams()), getSearchConditions());
        }
        override protected object[] getCompleteParams()
        {
            return new object[] { query};

        }
        override protected object[] getCountParams()
        {
            return new object[] { query};

        }
        override protected object[] getPartialParams()
        {
            return new object[] { query };

        }
        */





        override protected object[] getCompleteParams()
        {
            //if the query contains this pattern, dont replace the searchconditions
            String sc = "";
            if (query.IndexOf("{FILTERCONDITIONS}") < 0)
                sc = getSearchConditions();
            return new object[] { query, sc, getOrderBy() };

        }
        override protected object[] getCountParams()
        {
            String sc = "";
            if (query.IndexOf("{FILTERCONDITIONS}") < 0)
                sc = getSearchConditions();
            return new object[] { query, sc };

        }
        override protected object[] getPartialParams()
        {
            String sc = "";
            if (query.IndexOf("{FILTERCONDITIONS}") < 0)
                sc = getSearchConditions();
            return new object[] { query,sc, getOrderBy() };

        }

		public ViewMeta getQueryConfig()
		{
			return queryConfig;
		}




    }
   


}