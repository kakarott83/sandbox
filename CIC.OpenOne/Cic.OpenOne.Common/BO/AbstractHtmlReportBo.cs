using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DAO;
using Cic.One.Utils.Util;

namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// Abstrakte HtmlReport BO Klasse
    /// </summary>
    public abstract class AbstractHtmlReportBo : IHtmlReportBo
    {
        /// <summary>
        /// HtmlTemplate DAO
        /// </summary>
        protected IHtmlTemplateDao htmlTemplateDao;
        public AbstractHtmlReportBo(IHtmlTemplateDao htmlTemplateDao)
        {
            this.htmlTemplateDao = htmlTemplateDao;
        }

        /// <summary>
        /// Erzeugt einen Html Report
        /// </summary>
        /// <param name="data">Daten welche mit Reflection analysiert werden und dann in das Html Template eingefügt wird</param>
        /// <param name="keepNotExisting">Soll eine Variable im String behalten werden, falls die Variable nicht gefunden wird</param>
        /// <param name="useHtmlEncode">Sollen die Variablen Html encodiert werden</param>
        /// <returns>Fertige Html-Page mit eingefügtem Objekt</returns>
        public abstract string CreateHtmlReport(object data, bool keepNotExisting = false, bool useHtmlEncode = true);

        /// <summary>
        /// Erzeugt einen Html Report
        /// </summary>
        /// <param name="data">Daten welche mit Reflection analysiert werden und dann in das Html Template eingefügt wird</param>
        /// <param name="templateid">Id für Datenquelle</param>
        /// <returns>Fertige Html-Page mit eingefügtem Objekt</returns>
        /// <returns></returns>
        public abstract string CreateHtmlReport(object data, int templateid);

        /// <summary>
        /// Erzeugt einen Html Report
        /// </summary>
        /// <param name="xmlData">Daten welche mit Reflection analysiert werden und dann in das Html Template eingefügt wird (in Form von XML)</param>
        /// <param name="keepNotExisting">Soll eine Variable im String behalten werden, falls die Variable nicht gefunden wird</param>
        /// <param name="useHtmlEncode">Sollen die Variablen Html encodiert werden</param>
        /// <returns>Fertige Html-Page mit eingefügtem Objekt</returns>
        public virtual string CreateHtmlReport(string xmlData, bool keepNotExisting = false, bool useHtmlEncode = true)
        {
            object data;
            data = ExpandoUtil.GetExpandoFromXml(xmlData);
            return CreateHtmlReport(data, keepNotExisting, useHtmlEncode);
        }

        /// <summary>
        /// Erzeugt einen HtmlMail Report
        /// </summary>
        /// <param name="variables">Variablen, welche eingesetzt werden sollen</param>
        /// <param name="snippets">Snippets, welche vor den Variablen eingesetzt werden sollen</param>
        /// <returns>Fertige Html-Page mit eingefügtem Objekt</returns>
        public string CreateHtmlMailReport(Dictionary<string,string> variables, Dictionary<string,string> snippets)
        {
            if (snippets != null)
                htmlTemplateDao = new DAO.StringHtmlTemplateDao(CreateHtmlReport(ExpandoUtil.GetExpando(snippets), true, false));

            return CreateHtmlReport(ExpandoUtil.GetExpando(variables), false, true);
        }

        
    }
}
