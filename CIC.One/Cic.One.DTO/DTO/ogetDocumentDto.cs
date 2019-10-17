using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class ogetDocumentDto : oBaseDto
    {
        public EaihotDto document;
        public byte[] data;
    }
}
