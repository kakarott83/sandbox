using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    public class ogetDocumentDto : oBaseDto
    {
        public DmsDocDto document { get; set; }
        public DocumentTypeDto documentType { get; set; }
    }
}