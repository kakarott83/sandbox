using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenLease.Service.Services.DdOl.DAO;

namespace Cic.OpenLease.Service.Services.DdOl.BO
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
        /// <param name="templateId">Id von dem Template welches geladen werden soll</param>
        /// <returns>Fertige Html-Page mit eingefügtem Objekt</returns>
        public abstract string CreateHtmlReport(object data, String templateId);


    }
}