using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    public class QueryInfoDataExt: QueryInfoData
    {
        public QueryInfoDataExt()
        {
            _queryStructure = new QueryStructure();
            _queryStructure.countQuery = "select count(*) from  {0} where {1}";//0=3, 1=4
            _queryStructure.completeQuery = "select  {0} from {1} where {2}";//0=2, 1=3, 2=4
            _queryStructure.partialQuery = "SELECT * FROM (SELECT rownum rnum, a.* FROM( select {0} from {1} where {2} ) a WHERE rownum <= {3}) WHERE rnum > {4}";//2=4, 3=5, 4=6
        }
        public QueryInfoDataExt(String resultFields_0, String resultTables_1, String idFields_2, String searchTables_3, String searchConditions_4)
            : this()
        {
              this.resultFields_0 = resultFields_0;
              this.resultTables_1 = resultTables_1;
              this.idFields_2 = idFields_2;
              this.searchTables_3 = searchTables_3;
              this.searchConditions_4 = searchConditions_4;
        }
        public QueryInfoDataExt(QueryInfoDataExt org)
            : base(org)
        {

        }

        override public object[] getCompleteParams()
        {
            return new object[] { resultFields_0, searchTables_3, searchConditions_4 };

        }
        override public object[] getCountParams()
        {
            return new object[] { resultTables_1, searchConditions_4 };

        }
        override public object[] getPartialParams()
        {
            return new object[] { resultFields_0, resultTables_1, searchConditions_4, pageSize_5, startRow_6 };

        }

        public override QueryInfoData clone()
        {
            return new QueryInfoDataExt(this);
        }
    }
}
