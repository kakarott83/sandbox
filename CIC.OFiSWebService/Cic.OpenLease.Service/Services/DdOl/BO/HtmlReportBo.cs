using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenLease.Service.Services.DdOl.DAO;
using System.Text;
using System.Text.RegularExpressions;

using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using System.Globalization;

/*
 * 
1. Each
<!--each{$object.Array}-->
<tr><td>{$object.Array}</td></tr>
<!--end-->

wird zu

<!--begineach Array-->
<tr><td>{$object.Array[0]}</td></tr>
<tr><td>{$object.Array[1]}</td></tr>
<tr><td>{$object.Array[2]}</td></tr>
…
<!—endeach Array-->

//Das funktioniert auch schon, wenn mehrere Schleifen ineinander sind.

2. If
<!--if{$object.Abfrage}-->
<tr><td>{$object.Abfrage}</td></tr>
<!--end-->

falls Abfrage == true:

<!--beginif Abfrage == true-->
<tr><td>{$object.Abfrage}</td></tr>
<!—endif Abfrage-->

falls Abfrage == false:

<!--beginif Abfrage == false-->

//Dabei muss die Eigenschaft ein bool-Wert sein (werd ich noch erweitern)
//Funktioniert auch geschachtelt und auch mit each in Verbindung

3. Normaler Wert
{Standard$object.InfoPfad|Format}

zB. {nicht vorhanden$object.Adresse.PLZ|0:00000} wobei das Objekt in dem Fall eine Person

//als erstes wird versucht den Pfad aufzulösen, falls er nichts findet oder der Wert null ist, wird der Standard verwendet.
//Am Ende wird das Ergebnis noch durch ein string.Format geschickt.
 Formate-Beispiele:
{{$object.angebot.ANGOBGRUNDEXTERNBRUTTO|0:n}}
{{$object.angebot.ANGOBGRUNDEXTERNBRUTTO|0:0,0.00}}
{{$object.angebot.ANGOBGRUNDEXTERNBRUTTO|0:0,0.000}}
{{$object.angebot.ANGOBGRUNDEXTERNBRUTTO|0:0,0}}
{{$object.angebot.ANGOBGRUNDEXTERNBRUTTO|0:0.00}}
{{$object.angebot.ANGOBGRUNDEXTERNBRUTTO|0:0.000}}
{{$object.angebot.ANGOBGRUNDEXTERNBRUTTO|0:0}}
ergibt:
61.460,00
61.460,00
61.460,000
61.460
61460,00
61460,000
61460


*/
namespace Cic.OpenLease.Service.Services.DdOl.BO
{
    /// <summary>
    /// realisiert die Erstellung eines HtmlReports
    /// 
    /// </summary>
    public class HtmlReportBo : AbstractHtmlReportBo
    {
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Konstruktor des HtmlReportBo
        /// </summary>
        /// <param name="htmlTemplateDao"></param>
        public HtmlReportBo(IHtmlTemplateDao htmlTemplateDao)
            : base(htmlTemplateDao)
        {

        }

        /// <summary>
        /// Erzeugt einen Html Report
        /// </summary>
        /// <param name="data">Daten welche mit Reflection analysiert werden und dann in das Html Template eingefügt wird</param>
        /// <param name="templateId">Id von dem Template welches geladen werden soll</param>
        /// <returns>Fertige Html-Page mit eingefügtem Objekt</returns>
        public override string CreateHtmlReport(object data, String templateId)
        {
            DateTime time = DateTime.Now;
            string template = htmlTemplateDao.getHtmlTemplateString(templateId);

      
            
            ReflectionInfoBo rbo = new ReflectionInfoBo(data);
            template = new HtmlExpanderBo(rbo).ExpandHtml(template);
            string elapsedForExpanding = (DateTime.Now - time).TotalMilliseconds.ToString();

            StringBuilder sb = new StringBuilder();
            int lastIndex = 0;

            int ersetzungen = 0;
            foreach (Match m in Regex.Matches(template, "\\{\\{(.*?)?\\$object\\.?(.*?)(?:\\|(.*?))?\\}\\}", RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.None))
            {
                sb.Append(template, lastIndex, m.Index - lastIndex);

                string def = m.Groups[1].Value;
                string infoPath = m.Groups[2].Value;
                string format = m.Groups[3].Value;

                string replacement = "";

                bool success;
                object foundObject = rbo.getValue(infoPath, out success);
                if (!success)
                {
                    if (string.IsNullOrEmpty(def))
                    {
                        _Log.Warn("Could not parse " + def + " " + infoPath);
                        foundObject = "";
                    }
                    else
                    {
                        decimal deci;
                        if (decimal.TryParse(def, out deci))
                        {
                            foundObject = deci;
                        }
                        else
                        {
                            foundObject = def;
                        }
                    }
                }

                if (string.IsNullOrEmpty(format))
                {
                    replacement = foundObject.ToString();
                }
                else
                    replacement = string.Format(CultureInfo.GetCultureInfo("de-DE"), "{" + format + "}", foundObject);

                sb.Append(HttpUtility.HtmlEncode(replacement));
                lastIndex = m.Index + m.Length;
                ersetzungen++;
            }

            sb.Append(template.Substring(lastIndex));
            template = sb.ToString();

            string elapsed = (DateTime.Now - time).TotalMilliseconds.ToString();

            return template;
        }

       
    }
}