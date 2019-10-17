using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO
{
    public class IndexedTableDto
    {
        public String id { get; set; }//will be used as the entity-Name for searching
        /// <summary>
        /// query for fetching the entries to index
        /// Must contain all Fields for Class IndexResult except fields named "entity" and "keyfield"
        /// especially field area for a index where areaField is defined
        /// especially field content for containing multiple searched fields
        /// </summary>
        public String query { get; set; }

        /// <summary>
        /// query for fetching the entries to update
        ///  has to contain a parameter {0} for the last timestamp
        /// and to_timestamp(TO_CHAR(cictlog.changedate,'yyyy-mm-dd HH24:MI:SS'),'YYYY-MM-DD HH24:MI:SS') >( to_timestamp(TO_CHAR(sysdate,'yyyy-mm-dd HH24:MI:SS'),'YYYY-MM-DD HH24:MI:SS') - interval '{0}'  MINUTE)
        /// and a parameter {1} when having a multiarea-query (areaField set)
        /// </summary>
        public String updatequery { get; set; }
        public String keyField { get; set; }//the column used for updating/deleting and for maxid-comparison
        public String areaField { get; set; }//the column used for different areas when keyField comes from different tables
        public String indexid { get; set; }//will be used for saving the last max-id in db as name
        public bool positiveId { get; set; }//for the maxcomparison, when ids are negative
        public bool peroleFilter { get; set; }//when true the peuni-query field will be compared to the given perole
        public String peuniFilter { get; set; }//when set the caller filters by peuni himself, should be a comma-separated list of areas there the peuni will be used

        public IndexedTableDto()
        {
            positiveId = true;
        }
       
    }
}
