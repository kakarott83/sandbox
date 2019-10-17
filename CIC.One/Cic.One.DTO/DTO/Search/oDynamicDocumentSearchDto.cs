using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Output für die Dynamic Document Search
    /// </summary>
    public class oDynamicDocumentSearchDto : oBaseDto
    {
        /// <summary>
        /// Trefferliste
        /// </summary>
        public HitlistDto Hitlist { get; set; }
    }
}