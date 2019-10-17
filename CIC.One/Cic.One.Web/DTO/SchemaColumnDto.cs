using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    public class SchemaColumnDto
    {
        /// <summary>
        /// physical db col name
        /// </summary>
        public String baseColumnName { get; set; }
        /// <summary>
        /// alias column name from query
        /// </summary>
        public String columnName { get; set; }
        public int columnSize { get; set; }

        public SchemaColumnDto(DataRow row)
        {
            baseColumnName = (String)row["BaseColumnName"];
            columnName = (String)row["ColumnName"];
            columnSize = (int)row["ColumnSize"];
        }
    }
}
