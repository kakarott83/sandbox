using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO
{
    public class icreateOrUpdateMemoDto
    {
        public MemoDto memo { get; set; }

        /// <summary>
        /// (optional) name of table referenced by memo.syswfmtable
        /// </summary>
        public String refTable { get; set; }
    }
}
