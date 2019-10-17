using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Text;

namespace Cic.One.Web.BO.Search
{
    /// <summary>
    /// Noch lange nicht fertig
    /// Kann gelöscht werden
    /// </summary>
    public class QueryInfoDataTypeUnion : QueryInfoData
    {

        private List<QueryInfoData> queriesForUnion = new List<QueryInfoData>();
        private bool unionAll = true;

        public QueryInfoDataTypeUnion()
        {
        }

        public QueryInfoDataTypeUnion(QueryInfoDataTypeUnion org)
            : base(org)
        {
            foreach (QueryInfoData qid in org.queriesForUnion)
            {
                Add(qid.clone());
            }
        }

        override public QueryInfoData clone()
        {
            return new QueryInfoDataTypeUnion(this);
        }

        /// <summary>
        /// Add another query to the union
        /// as all have to use the same pageSize/startRow/optimization and resultFields, these settings are also 
        /// written to the unionDataType
        /// </summary>
        /// <param name="data"></param>
        public void Add(QueryInfoData data)
        {
            queriesForUnion.Add(data);
            this.resultFields = data.resultFields;
            this.pageSize = data.pageSize;
            this.startRow = data.startRow;
            this.optimized = data.optimized;

            this.entityField = data.entityField;
            this.resultFields = data.resultFields;
            this.resultTables = data.resultTables;
            this.searchTables = data.searchTables;
            this.searchConditions = data.searchConditions;
            this.entityTable = data.entityTable;
            
        }

        public void Remove(QueryInfoData data)
        {
            queriesForUnion.Remove(data);
          
        }

       
        public bool UnionAll
        {
            get { return unionAll; }
            set { unionAll = value; }
        }

        private String getUnion()
        {
            if (unionAll) return " UNION ALL ";
            return " UNION ";
        }

        /// <summary>
        /// As search-bo alters some values this writes the changes to all union subqueries
        /// </summary>
        /// <param name="qid"></param>
        private void updateQuery(QueryInfoData qid)
        {
            qid.resultFields = this.resultFields;
            qid.pageSize = this.pageSize;
            qid.startRow = this.startRow;
            qid.optimized = this.optimized;
        }
        override public String getCountQuery()
        {
            StringBuilder sb = new StringBuilder();
            String union = getUnion();

            foreach(QueryInfoData query in queriesForUnion)
            {
                if (sb.Length > 0)
                {
                    sb.Append(union);
                }
                updateQuery(query);
                query.clearSortConditions();
                sb.Append(query.getCountQuery());
            }
            return sb.ToString();
        }

        override public String getCompleteQuery()
        {
            StringBuilder sb = new StringBuilder();
            String union = getUnion();

            foreach (QueryInfoData query in queriesForUnion)
            {
                if (sb.Length > 0)
                {
                    sb.Append(union);
                }
                updateQuery(query);
                query.clearSortConditions();
                sb.Append(query.getCompleteQuery());
            }
            return sb.ToString();
            
        }

        override public String getPartialQuery()
        {
            StringBuilder sb = new StringBuilder();
            String union = getUnion();

            foreach (QueryInfoData query in queriesForUnion)
            {
                if (sb.Length > 0)
                {
                    sb.Append(union);
                }
                updateQuery(query);
                query.clearSortConditions();
                sb.Append(query.getPartialQuery());
            }
            return sb.ToString();
            
        }


        override protected object[] getCompleteParams()
        {
            return new object[] {};

        }
        override protected object[] getCountParams()
        {
            return new object[] {  };

        }
        override protected object[] getPartialParams()
        {
            return new object[] {  };

        }

        public override void clearAdditionalSearchConditions()
        {
            foreach (QueryInfoData query in queriesForUnion)
            {
                query.clearAdditionalSearchConditions();
            }
        }

        public override void addAdditionalSearchConditions(String cond)
        {
            foreach (QueryInfoData query in queriesForUnion)
            {
                query.addAdditionalSearchConditions(cond);
            }
        }
    }
}