using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    /// <summary>
    /// Defines the current primary key value of a certain table
    /// </summary>
    public class Pkey
    {
        public long value { get; set; }
        public String area { get; set; }
        public String table { get; set; }
        public Pkey()
        {

        }
        public Pkey(long value, String table)
        {
            this.value = value;
            this.table = table;
        }
    }
}
