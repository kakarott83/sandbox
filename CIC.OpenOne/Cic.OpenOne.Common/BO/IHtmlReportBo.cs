using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.BO
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
        /// <param name="keepNotExisting">Soll eine Variable im String behalten werden, falls die Variable nicht gefunden wird</param>
        /// <param name="useHtmlEncode">Sollen die Variablen Html encodiert werden</param>
        /// <returns>Fertige Html-Page mit eingefügtem Objekt</returns>
        string CreateHtmlReport(object data, bool keepNotExisting = false, bool useHtmlEncode = true);

        /// <summary>
        /// Erzeugt einen Html Report
        /// </summary>
        /// <param name="xmlData">Daten welche mit Reflection analysiert werden und dann in das Html Template eingefügt wird (in Form von XML)</param>
        /// <param name="keepNotExisting">Soll eine Variable im String behalten werden, falls die Variable nicht gefunden wird</param>
        /// <param name="useHtmlEncode">Sollen die Variablen Html encodiert werden</param>
        /// <returns>Fertige Html-Page mit eingefügtem Objekt</returns>
        string CreateHtmlReport(string xmlData, bool keepNotExisting = false, bool useHtmlEncode = true);

        /// <summary>
        /// Erzeugt einen HtmlMail Report
        /// </summary>
        /// <param name="variables">Variablen, welche eingesetzt werden sollen</param>
        /// <param name="snippets">Snippets, welche vor den Variablen eingesetzt werden sollen</param>
        /// <returns>Fertige Html-Page mit eingefügtem Objekt</returns>
        string CreateHtmlMailReport(Dictionary<string, string> variables, Dictionary<string, string> snippets);


        /// <summary>
        /// Erzeugt einen Html Report
        /// </summary>
        /// <param name="data">Daten welche mit Reflection analysiert werden und dann in das Html Template eingefügt wird</param>
        /// <param name="templateid">Id für Datenquelle</param>
        /// <returns>Fertige Html-Page mit eingefügtem Objekt</returns>
        /// <returns></returns>
        string CreateHtmlReport(object data, int templateid);
    }
}
