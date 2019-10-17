using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    /// <summary>
    /// Structure for displaying a message box in the gui
    /// </summary>
    public class EntityIconDto
    {
        public string entity { get; set; }
        public long key { get; set; }
        public Boolean ischecked { get; set; }
        public Boolean isfav { get; set; }
        public string day { get; set; }
        public string month { get; set; }
        public byte[] iconimg { get; set; }
        //public boolean ischecked { get; set; }


    }
}