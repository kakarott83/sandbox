using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.Web.BO.Search
{
    /// <summary>
    /// Query for user-defined query, no counting etc
    /// used for REPORTING
    /// </summary>
    public class QueryInfoDataType3 : QueryInfoData
    {
        private String query = "";
        public QueryInfoDataType3()
        {
//            select rownum sysreport, a.* from (select to_char(beginn, 'YYYY-MM') X,sum(bgextern) A1,sum(rw) A2, sum (sz) A3 from vt where beginn>='01.01.2014' and beginn<='01.12.2014' group by to_char(beginn, 'YYYY-MM') order by to_char(beginn, 'YYYY-MM')) a;
            _queryStructure = new QueryStructure();
            _queryStructure.countQuery = "select count(*) from ({0}) a";
            _queryStructure.completeQuery = "select rownum sysreport, a.* from ({0}) a";
            _queryStructure.partialQuery = "select rownum sysreport, a.* from ({0}) a";
            searchConditions = " 1=1 ";
        }

        public QueryInfoDataType3(String query) :this()
        {
            this.entityField = entityField;
            this.entityTable = entityTable;
            resultFields = entityTable + ".*";
            resultTables = entityTable;
            searchTables = entityTable;
            searchConditions = " 1=1 ";
            this.query = query;
            
        }
        public QueryInfoDataType3(QueryInfoDataType3 org):base(org)
        {
            this.query = org.query;
        }
        override public QueryInfoData clone()
        {
            return new QueryInfoDataType3(this);
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

    }
}