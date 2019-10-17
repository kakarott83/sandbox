using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenLease.Service.Services.DdOl.DTO
{
    /// <summary>
    /// OutputParameter für die createHtmlReport Methode
    /// </summary>
    public class ocreateHtmlReportDto : Cic.OpenOne.Common.DTO.oBaseDto
    {
        /// <summary>
        /// Enthält die erstellte HTML-Seite als byte[]
        /// </summary>
        public byte[] CreatedHtmlReport { get; set; }

        /// <summary>
        /// Enthält die erstellte HTML-Seite als string
        /// </summary>
        public string CreatedHtmlReportString { get; set; }
    }
}