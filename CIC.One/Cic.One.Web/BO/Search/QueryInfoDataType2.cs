using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.Web.BO.Search
{
    /// <summary>
    /// Query Patterns for optimizing the oracle-search-behaviour in PTA-Environment
    /// </summary>
    public class QueryInfoDataType2 : QueryInfoData
    {
        public QueryInfoDataType2()
        {
            _queryStructure = new QueryStructure();
            _queryStructure.countQuery = "select count(*) from  {0} where {1} {2} {3}";
            _queryStructure.completeQuery = "select  {0} from {1} where {2} {3} {4}";
            _queryStructure.partialQuery = "select {0} FROM {1},  (SELECT * FROM (SELECT rownum rnum, a.* from (  select  {2} from {3} where {4} {8} {9}) a   ) WHERE rownum <= {5} and rnum > {6}) b   where  {7}";
        }
        public QueryInfoDataType2(QueryInfoDataType2 org)
            : base(org)
        {

        }
        override public QueryInfoData clone()
        {
            return new QueryInfoDataType2(this);
        }
        override protected object[] getCompleteParams()
        {
            return new object[] { resultFields, searchTables + getAdditionalTables(), getSearchConditions(),getGroupBy(),getOrderBy() };

        }
        override protected object[] getCountParams()
        {
            return new object[] { searchTables + getAdditionalTables(), getSearchConditions(), getGroupBy(), getOrderBy() };

        }
        override protected object[] getPartialParams()
        {
            return new object[] { resultFields, resultTables + getAdditionalTables(), idFields, searchTables + getAdditionalTables(), getSearchConditions(), pageSize, startRow, resultConditions, getGroupBy(), getOrderBy() };

        }

    }

}