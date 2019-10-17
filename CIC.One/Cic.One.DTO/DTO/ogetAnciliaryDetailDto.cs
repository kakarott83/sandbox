using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO
{
    public class ogetAnciliaryDetailDto : oBaseDto
    {

       public long sysberater { get; set; }
       public long sysabwicklung { get; set; }
        /// <summary>
        /// The angebot or antrag nummer
        /// </summary>
       public String nummer { get; set; }
       public String smstext { get; set; }
       public int locked{ get; set; }
    }
}
