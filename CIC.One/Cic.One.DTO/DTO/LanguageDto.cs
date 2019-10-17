using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO
{
    /// <summary>
    /// Holds available language infos
    /// </summary>
    public class LanguageDto
    {
        public long sysctlang { get; set; }
        public long syscttlang { get; set; }
        public String languagename { get; set; }
        public String isocode { get; set; }
    }
}
