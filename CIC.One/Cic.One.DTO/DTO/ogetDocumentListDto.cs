﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class ogetDocumentListDto:oBaseDto
    {
        public List<PrintDocumentDto> documents { get; set; }
    }
}
