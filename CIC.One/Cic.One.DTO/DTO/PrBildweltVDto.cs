using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    public class PrBildweltVDto
    {
        public long sysprbildwelt { get; set; }
        public long sysprbildweltv { get; set; }
        public long sysvart { get; set; }
        public String bezeichnung { get; set; }
        public TranslationDto translation { get; set; }
    }
}
