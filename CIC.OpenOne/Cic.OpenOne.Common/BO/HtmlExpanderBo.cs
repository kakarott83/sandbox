using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using Cic.One.Utils.Util.Reflection;

namespace Cic.OpenOne.Common.BO
{/// <summary>
    /// Expandiert ein HtmlTemplate
    /// </summary>
    public class HtmlExpanderBo
    {
        private ReflectionInfo reflectionInfo;
        /// <summary>
        /// Konstruktor von dem Expandierer
        /// </summary>
        /// <param name="rbo">Enthält alle Informationen, welche der Expandierer über Reflection benötigt</param>
        public HtmlExpanderBo(ReflectionInfo rbo)
        {
            this.reflectionInfo = rbo;
        }

        /// <summary>
        /// Expandiert ein HtmlTemplate
        /// </summary>
        /// <param name="template">Template welches expandiert werden soll</param>
        /// <returns>expandiertes Template</returns>
        internal string ExpandHtml(string template)
        {
            template = calculateDeepness(template);
            template = ExpandEach(template, 1);
            return template;
        }

        /// <summary>
        /// Berechnet die jeweilige Tiefe der <!--(end)--> Fußzeilen, damit Sie richtig zugeordnet werden können
        /// </summary>
        /// <param name="template">Template, wessen <!--(end)--> Fußzeilen berechnet werden sollen</param>
        /// <returns>Fertig expandiertes Template (wurde noch nicht mit Daten gefüllt)</returns>
        private string calculateDeepness(string template)
        {

            string pattern = "(?:<!--(if|each|ifnot)\\{\\{\\$object\\.?(?:.*?)\\}\\}-->)|(?:<!--(end)-->)";

            int currentIndex = 0;
            StringBuilder sb = new StringBuilder();
            int deepness = 0;
            foreach (Match m in Regex.Matches(template, pattern, RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.None))
            {
                sb.Append(template, currentIndex, m.Index - currentIndex);
                currentIndex = m.Index + m.Length;
                if (m.Groups[2].Value == "")
                {
                    sb.Append(m.Value);
                    deepness++;
                }
                else
                {
                    sb.Append("<!--end" + deepness + "-->");
                    deepness--;
                }
            }
            sb.Append(template.Substring(currentIndex));
            return sb.ToString();
        }

        /// <summary>
        /// Expandiert ein Template wobei auch alle ifs schon berechnet werden
        /// </summary>
        /// <param name="template">Template welches benutzt werden soll</param>
        /// <param name="deepness">Tiefenstufe, welche aufgelöst werden soll</param>
        /// <returns>Fertig expandiertes Template (wurde noch nicht mit Daten gefüllt)</returns>
        private string ExpandEach(string template, int deepness)
        {
            StringBuilder result = new StringBuilder();
            int templateIndex = 0;
            string pattern = "<!--(each|if|ifnot)\\{\\{\\$object\\.?(.*?)\\}\\}-->(.*?)<!--end" + deepness + "-->";
            bool foundMatches = false;

            //Als erstes wird nach allen Schleifen/If-Köpfe gesucht
            foreach (Match m in Regex.Matches(template, pattern, RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.None))
            {
                //Er fügt alles von dem letzten Treffer bis zu diesem Treffer hinzu
                result.Append(template, templateIndex, m.Index - templateIndex);
                //Und setzt den Index für das Ende diesen Treffers
                templateIndex = m.Index + m.Length;
                //Legt fest, dass er noch eine Stufe Tiefer schauen muss, da noch Schleifen innerhalb der Vorlage existieren könnten
                foundMatches = true;
                //Der InfoPfad legt fest wo sich die Daten im Objekt befinden also zB. "Groups[2].Value" wäre ein InfoPfad von m (nächste Zeile)
                string infoPath = m.Groups[2].Value;
                //Sucht das Objekt, welcher sich an dem InfoPfad befindet
                bool success;
                object os = reflectionInfo.getValue(infoPath, out success);

                //Falls ein If gefunden wurde wird es aufgelöst, falls das Objekt der If Abfrage auch wirklich ein bool-Wert ist
                if (m.Groups[1].Value == "if" || m.Groups[1].Value == "ifnot")
                {
                    if (os is bool)
                    {
                        bool show = ((m.Groups[1].Value == "if") && (bool)os) || ((m.Groups[1].Value == "ifnot") && !(bool)os);
                        if (show)
                        {
                            result.Append("<!--begin" + m.Groups[1].Value + " " + infoPath + " == true-->");
                            result.Append(m.Groups[3].Value);
                            result.Append("<!--end" + m.Groups[1].Value + "-->");
                        }
                        else
                        {
                            result.Append("<!--begin" + m.Groups[1].Value + " " + infoPath + " == false-->");
                        }
                    }
                    else
                    {
                        result.Append("<!--Couldn't Parse If " + infoPath + "-->");
                    }
                }
                else
                {
                    //Ansonsten muss es eine Schleife sein
                    if (os is IEnumerable)
                    {
                        int index = 0;
                        string innerLoop = m.Groups[3].Value;

                        MatchCollection InnerLoopMatches = Regex.Matches(innerLoop, "\\{\\{(?:(.*?)\\|)?\\$object\\.?(.*?)(?:\\|(.*?))?\\}\\}", RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.None);
                        result.Append("<!--begineach " + infoPath + "-->");

                        IEnumerable list = os as IEnumerable;
                        //und baut für jedes Objekt der Liste den Rumpf zusammen
                        foreach (object o in list)
                        {
                            string newInfoPath = infoPath + "[" + index + "]";
                            reflectionInfo.setValue(newInfoPath, o);

                            int loopStringIndex = 0;
                            foreach (Match InnerLoopMatch in InnerLoopMatches)
                            {
                                result.Append(innerLoop, loopStringIndex, InnerLoopMatch.Index - loopStringIndex);

                                string addedString = InnerLoopMatch.Value;
                                if (infoPath == "")
                                {
                                    addedString = addedString.Replace("$object" + infoPath, "$object" + newInfoPath);
                                }
                                else
                                    addedString = addedString.Replace("$object." + infoPath, "$object." + newInfoPath);

                                //if (InnerLoopMatch.Value.StartsWith("{$object." + infoPath))
                                //{
                                //    addedString = "{$object." + newInfoPath + InnerLoopMatch.Value.Substring(("{$object." + infoPath).Length);
                                //}
                                //else
                                //    addedString = InnerLoopMatch.Value;

                                result.Append(addedString);
                                loopStringIndex = InnerLoopMatch.Index + InnerLoopMatch.Length;
                            }
                            result.Append(innerLoop.Substring(loopStringIndex));
                            index++;
                        }
                        result.Append("<!--endeach " + infoPath + "-->");
                    }
                    else
                    {
                        result.Append("<!--Couldn't Parse Loop " + infoPath + "-->");
                    }
                }
            }
            if (foundMatches)
            {
                result.Append(template.Substring(templateIndex));
                return ExpandEach(result.ToString(), deepness + 1);
            }
            else
                return template;
        }
    }
}
