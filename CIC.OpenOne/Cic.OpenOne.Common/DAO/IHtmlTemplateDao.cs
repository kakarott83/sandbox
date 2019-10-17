using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// Interface zum Laden eines HTML-Templates
    /// </summary>
    public interface IHtmlTemplateDao
    {
        /// <summary>
        /// Gibt das Template als string zurück
        /// </summary>
        /// <returns>Gefundenes Template</returns>
        string getHtmlTemplate();

        /// <summary>
        /// Gibt das Template als string zurück
        /// </summary>
        /// <param name="templateId">Die Id des Templates, welches geladen werden soll</param>
        /// <returns>Gefundenes Template</returns>
        string getHtmlTemplate(int templateId);
    }
}
