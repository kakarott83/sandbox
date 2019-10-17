using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenLease.Service.Services.DdOl.BO
{ 
    /// <summary>
    /// Schnittstelle für das Exportieren von Html Reports
    /// </summary>
    public interface IHtmlReportBo
    {
        /// <summary>
        /// Erzeugt einen Html Report
        /// </summary>
        /// <param name="data">Daten welche mit Reflection analysiert werden und dann in das Html Template eingefügt wird</param>
        /// <param name="templateId">Id von dem Template welches geladen werden soll</param>
        /// <returns>Fertige Html-Page mit eingefügtem Objekt</returns>
        string CreateHtmlReport(object data, String templateId);

    }
}