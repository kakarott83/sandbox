using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Cic.One.Utils.Util.Reflection;
using System.Reflection;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.DAO;
using System.Globalization;
using System.Web;
using System.Drawing.Imaging;
using System.IO;
using Cic.OpenOne.Common.DTO;
using System.Drawing;

/*
 * 
1. Each
<!--each{{$object.Array}}-->
<tr><td>{{$object.Array}}</td></tr>
<!--end-->

wird zu

<!--begineach Array-->
<tr><td>{{$object.Array[0]}}</td></tr>
<tr><td>{{$object.Array[1]}}</td></tr>
<tr><td>{{$object.Array[2]}}</td></tr>
…
<!—endeach Array-->

//Das funktioniert auch schon, wenn mehrere Schleifen ineinander sind.

2. If
<!--if{{$object.Abfrage}}-->
<tr><td>{{$object.Abfrage}}</td></tr>
<!--end-->

falls Abfrage == true:

<!--beginif Abfrage == true-->
<tr><td>{{$object.Abfrage}}</td></tr>
<!—endif Abfrage-->

falls Abfrage == false:

<!--beginif Abfrage == false-->

//Dabei muss die Eigenschaft ein bool-Wert sein
//Funktioniert auch geschachtelt und auch mit each in Verbindung

3. Normaler Wert
{{Standard$object.InfoPfad|Format}}

zB. {{nicht vorhanden$object.Adresse.PLZ|0:00000}} wobei das Objekt in dem Fall eine Person

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
namespace Cic.OpenOne.Common.BO
{
    /// <summary>
    /// realisiert die Erstellung eines HtmlReports
    /// 
    /// </summary>
    public class HtmlReportBo : AbstractHtmlReportBo
    {
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Enthält die ReflectionInfo, damit schneller auf Werte zugegriffen werden kann, da diese in einem Dictionary gecashed werden
        /// </summary>
        private ReflectionInfo reflectionInfo;
        /// <summary>
        /// Legt fest ob ein neues Reflectioninfo Objekt erstellt werden soll.
        /// </summary>
        private bool createNewReflectioninfo { get; set; }
        /// <summary>
        /// Soll eine Variable im String behalten werden, falls die Variable nicht gefunden wird
        /// </summary>
        private bool keepNotExisting { get; set; }
        /// <summary>
        /// Sollen die Variablen Html encodiert werden
        /// </summary>
        private bool useHtmlEncode { get; set; }

      
        /// <summary>
        /// Konstruktor des HtmlReportBo
        /// </summary>
        /// <param name="htmlTemplateDao"></param>
        public HtmlReportBo(IHtmlTemplateDao htmlTemplateDao)
            : base(htmlTemplateDao)
        {
            useHtmlEncode = true;
            keepNotExisting = false;
            createNewReflectioninfo = true;
        }

       

        /// <summary>
        /// Erzeugt einen Html Report
        /// </summary>
        /// <param name="data">Daten welche mit Reflection analysiert werden und dann in das Html Template eingefügt wird</param>
        /// <param name="keepNotExisting">Soll eine Variable im String behalten werden, falls die Variable nicht gefunden wird</param>
        /// <param name="useHtmlEncode">Sollen die Variablen Html encodiert werden</param>
        /// <returns>Fertige Html-Page mit eingefügtem Objekt</returns>
        /// <returns></returns>
        public override string CreateHtmlReport(object data, bool keepNotExisting, bool useHtmlEncode = true)
        {
            return CreateHtmlReport(data, true, keepNotExisting, useHtmlEncode);
        }

        /// <summary>
        /// Erzeugt ein neues Reflectioninfo-Objekt
        /// </summary>
        /// <param name="data">Daten</param>
        /// <returns>erzeugtes Objekt</returns>
        public ReflectionInfo CreateReflectionInfo(object data)
        {
            reflectionInfo = new ReflectionInfo(data);
            return reflectionInfo;
        }

        /// <summary>
        /// Kann verwendet werden um ein neues TemplateDao zu setzen
        /// </summary>
        /// <param name="newTemplateDao"></param>
        public void SetTemplateDao(IHtmlTemplateDao newTemplateDao)
        {
            htmlTemplateDao = newTemplateDao;
        }

        /// <summary>
        /// Erzeugt einen Html Report
        /// </summary>
        /// <param name="data">Daten welche mit Reflection analysiert werden und dann in das Html Template eingefügt wird</param>
        /// <param name="createNewReflectioninfo">Legt fest ob ein neues Reflectioninfo Objekt erstellt werden soll.</param>
        /// <param name="keepNotExisting">Soll eine Variable im String behalten werden, falls die Variable nicht gefunden wird</param>
        /// <param name="useHtmlEncode">Sollen die Variablen Html encodiert werden</param>
        /// <returns>Fertige Html-Page mit eingefügtem Objekt</returns>
        /// <returns></returns>
        public string CreateHtmlReport(object data, bool createNewReflectioninfo, bool keepNotExisting = false, bool useHtmlEncode = true)
        {
            DateTime time = DateTime.Now;
            string template = htmlTemplateDao.getHtmlTemplate();


            if (createNewReflectioninfo || reflectionInfo == null)
                reflectionInfo = new ReflectionInfo(data);

            template = new HtmlExpanderBo(reflectionInfo).ExpandHtml(template);
            string elapsedForExpanding = (DateTime.Now - time).TotalMilliseconds.ToString();

            template = ReplaceText(template, data, false, keepNotExisting, useHtmlEncode);

            return template;
        }
        /// <summary>
        /// Erzeugt einen Html Report
        /// </summary>
        /// <param name="data">Daten welche mit Reflection analysiert werden und dann in das Html Template eingefügt wird</param>
        /// <param name="templateid">Id für Datenquelle</param>
        /// <returns>Fertige Html-Page mit eingefügtem Objekt</returns>
        /// <returns></returns>
        override public string CreateHtmlReport(object data, int templateid)
        {
            DateTime time = DateTime.Now;
            string template = htmlTemplateDao.getHtmlTemplate(templateid);


            if (createNewReflectioninfo || reflectionInfo == null)
                reflectionInfo = new ReflectionInfo(data);

            template = new HtmlExpanderBo(reflectionInfo).ExpandHtml(template);
            string elapsedForExpanding = (DateTime.Now - time).TotalMilliseconds.ToString();

            template = ReplaceText(template, data, false, keepNotExisting, useHtmlEncode);

            return template;
        }

        /// <summary>
        /// Ersetzt alle Platzhalter in dem Template
        /// </summary>
        /// <param name="template">Text, welcher durchsucht werden soll</param>
        /// <param name="data">Daten welche mit Reflection analysiert werden und dann in das Html Template eingefügt wird</param>
        /// <param name="createNewReflectioninfo">Legt fest ob ein neues Reflectioninfo Objekt erstellt werden soll.</param>
        /// <param name="keepNotExisting">Soll eine Variable im String behalten werden, falls die Variable nicht gefunden wird</param>
        /// <param name="useHtmlEncode">Sollen die Variablen Html encodiert werden</param>
        /// <returns></returns>
        public string ReplaceText(string template, object data, bool createNewReflectioninfo, bool keepNotExisting = false, bool useHtmlEncode = true)
        {
            if (createNewReflectioninfo || reflectionInfo == null)
                reflectionInfo = new ReflectionInfo(data);

            DateTime time = DateTime.Now;

            StringBuilder sb = new StringBuilder();
            int lastIndex = 0;
            int ersetzungen = 0;
            //%7B sind in html { und %7d sind in html }
            foreach (Match m in Regex.Matches(template, "(?:\\%7B\\%7B|\\{\\{)(.*?)?\\$object\\.?(.*?)(?:\\|(.*?))?(?:\\%7D\\%7D|\\}\\})", RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.None))
            {
                sb.Append(template, lastIndex, m.Index - lastIndex);

                string def = m.Groups[1].Value;
                string infoPath = m.Groups[2].Value;
                string format = m.Groups[3].Value;

                string replacement = "";

                bool success;
                object foundObject = reflectionInfo.getValue(infoPath, out success);
                if (!success)
                {
                    if (keepNotExisting)
                    {
                        foundObject = m.Value;
                    }
                    else if (string.IsNullOrEmpty(def))
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
                if (CheckCustomConverter(foundObject, ref replacement))
                {
                }
                else if (string.IsNullOrEmpty(format))
                {
                    replacement = foundObject.ToString();
                }
                else
                    replacement = string.Format(CultureInfo.GetCultureInfo("de-DE"), "{" + format + "}", foundObject);

                if (useHtmlEncode)
                    sb.Append(HttpUtility.HtmlEncode(replacement));
                else
                    sb.Append(replacement);

                lastIndex = m.Index + m.Length;
                ersetzungen++;
            }

            sb.Append(template.Substring(lastIndex));
            template = sb.ToString();

            string elapsed = (DateTime.Now - time).TotalMilliseconds.ToString();

            return template;
        }

        /// <summary>
        /// CustomConverter, können erweitert werden um spezielle Objekte anders ersetzen zu lassen
        /// </summary>
        /// <param name="foundObject">Objekt, welches an dem InfoPfad gefunden wurde</param>
        /// <param name="replacement">Ergebnis, welches ersetzt werden soll</param>
        /// <returns>Ob ein CustomvConverter gefunden wurde</returns>
        private bool CheckCustomConverter(object foundObject, ref string replacement)
        {
            if (foundObject is ImageDescriptionDto)
            {
                ImageDescriptionDto imgDescription = foundObject as ImageDescriptionDto;

                if (string.IsNullOrEmpty(imgDescription.ReplacementData))
                {
                    if (imgDescription.Data != null)
                    {
                        Image img = byteArrayToImage(imgDescription.Data);
                        ImageCodecInfo codec = GetMimeType(img);
                        string mime = "image/unknown";
                        if (codec != null)
                        {
                            mime = codec.MimeType;
                        }
                        string imageBase64 = Convert.ToBase64String(imgDescription.Data);

                        replacement = "data:" + mime + ";base64," + imageBase64;
                        imgDescription.ReplacementData = replacement;
                    }
                }
                else
                {
                    replacement = imgDescription.ReplacementData;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gibt die ImageCodec Information zurück
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static ImageCodecInfo GetMimeType(Image image)
        {
            var imgguid = image.RawFormat.Guid;
            foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageDecoders())
            {
                if (codec.FormatID == imgguid)
                {
                    return codec;
                }
            }
            return null;
        }

        /// <summary>
        /// Wandelt einen Byte Array zu einem Bild um.
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            using (MemoryStream ms = new MemoryStream(byteArrayIn))
            {
                Image returnImage = Image.FromStream(ms);
                return returnImage;
            }
        }

    }
}
