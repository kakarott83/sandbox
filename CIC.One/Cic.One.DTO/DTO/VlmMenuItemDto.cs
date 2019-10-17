using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    public class VlmMenuItemDto
    {
        public long sysvlmmitem { get; set; }
        public long sysvlmmparent { get; set; }
        public long header { get; set; }

        public String webart { get; set; }
        public int art { get; set; }
        public String text { get; set; }
        public String tip { get; set; }
        public String icon { get; set; }
        public String action { get; set; }
        public String checkExpression { get; set; }
        public int checkEffect { get; set; }
        public List<VlmMenuItemDto> items { get; set; }
        public String coderfu { get; set; }
        public String codermo { get; set; }
    }
}
