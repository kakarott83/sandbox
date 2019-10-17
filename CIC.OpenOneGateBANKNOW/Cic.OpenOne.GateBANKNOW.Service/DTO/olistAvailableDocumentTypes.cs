using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Contains the resulting document types from service call listAvailableDocumentTypes
    /// </summary>
    public class olistAvailableDocumentTypes : oBaseDto
    {
        public List<DocumentTypeDto> types { get;set;}
    }
}