using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenLease.Service.Services.DdOl.DAO
{
    /// <summary>
    /// Interface zum Laden eines HTML-Templates
    /// </summary>
    public interface IHtmlTemplateDao
    {
        /// <summary>
        /// Gibt das Template als string zurück
        /// </summary>
        /// <param name="templateId">Die Id des Templates, welches geladen werden soll</param>
        /// <returns>Gefundenes Template</returns>
        string getHtmlTemplate(int templateId);
        string getHtmlTemplateString(String templateId);
    }
}