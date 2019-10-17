using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenLease.Service.Services.DdOl.DAO
{
    /// <summary>
    /// Dies ist eine Testklasse, welche immer das Selbe HtmlTemplate zurück gibt.
    /// </summary>
    public class TestHtmlTemplateDao : IHtmlTemplateDao
    {
        /// <summary>
        /// Gibt ein Testtemplate zurück
        /// </summary>
        /// <param name="templateId">Die Id des Templates, welches geladen werden soll</param>
        /// <returns>Gefundenes Template</returns>
        public string getHtmlTemplate(int templateId)
        {
            if (templateId == -3)
            {
                return @"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01//EN""
                   ""http://www.w3.org/TR/html4/strict.dtd"">
                    <html>
                    <head>
                    <title>Aufbau einer Tabelle</title>
                    </head>
                    <body>

                    <h1>Tabelle</h1>

                    <table border=""0"">
                      <!--each{$object.PLZ}-->
                      <tr>
                        <td>{$object.PLZ}</td>
                      </tr>
                      <!--end-->
                    </table>
                </body>
                </html>
				 ";
            }
            if (templateId == -2)
            {
                return @"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01//EN""
                   ""http://www.w3.org/TR/html4/strict.dtd"">
                    <html>
                    <head>
                    <title>Aufbau einer Tabelle</title>
                    </head>
                    <body>

                    <h1>Tabelle</h1>

                    <table border=""0"">
                      <!--each{$object}-->
                      <tr>
                        <td>{$object}</td>
                      </tr>
                      <!--end-->
                    </table>
                </body>
                </html>
				 ";
            }
            if (templateId == -1)
            {
                return @"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01//EN""
                   ""http://www.w3.org/TR/html4/strict.dtd"">
                    <html>
                    <head>
                    <title>Aufbau einer Tabelle</title>
                    </head>
                    <body>

                    <h1>Tabelle</h1>

                    <table border=""0"">
                      <!--each{$object.BigArray}-->
                      <tr>
                        <td></td>
                        <td align=""right"">{$object.BigArray}</td>
                        <!--each{$object.Adressen}-->
                            <!--if{$object.Adressen.Show}-->
                                <td>{$object.Adressen.Names[0]}</td>
                                <!--if{$object.Adressen.Show2}-->
                                    <!--each{$object.Adressen.Names}-->
                                        <td>{$object.Adressen.Names}</td>
                                    <!--end-->
                                <!--end-->
                            <!--end-->
                        <!--end-->
                      </tr>
                      <!--end-->
                    </table>
                </body>
                </html>
				 ";
            }
            else
            return @"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.01//EN""
                   ""http://www.w3.org/TR/html4/strict.dtd"">
                    <meta http-equiv=""content-type"" content=""text/html; charset=utf-8"" />
                    <html>
                    <head>
                    <title>Aufbau einer Tabelle</title>
                    </head>
                    <body>

                    <h1>BMW Financial Services</h1>
                    <table border=""0"" width=""100%"">
                      <tr>
                        <tr colspan=""3"">Unverbindliche Kundeninformation zu {$object.ANGOBTYP} (vllt. noch falsch) Nr. {$object.ANGEBOT1} (vllt. noch falsch)</tr>
                      </tr>
                      <tr>
                        <td>Fahrzeug:</td>
                        <td>{$object.ANGOBHERSTELLER} {$object.ANGOBFABRIKAT} (vllt. noch falsch)</td>
                      </tr>
                      <tr>
                        <td>Fahrzeugart:</td>
                        <td>{$object.ANGOBFZART}</td>
                      </tr>
                      <tr>
                        <td>Nova-Faktor:</td>
                        <td>{$object.ANGOBNOVAP|0:0.00%}</td>
                      </tr>
                      <tr>
                        <td>Erstzulassung:</td>
                        <td>{-$object.ANGOBINIERSTZUL}</td>
                      </tr>
                      <tr>
                        <td>Kilometerstand:</td>
                        <td>{0$object.ANGOBINIKMSTAND} km</td>
                      </tr>
                    </table>

                    <hr size=""1"" width=""100%"">
                    <table border=""0"" width=""100%"">
                      <tr>
                        <td></td>
                        <td align=""right"">EUR Brutto</td>
                      </tr>
                      <tr>
                        <td>Listenpreis (inkl. NoVA):</td>
                        <td align=""right"">{$object.ANGOBGRUNDBRUTTO|0:n}</td>
                      </tr>
                      <tr>
                        <td>Sonderausstattung (inkl. NoVA):</td>
                        <td align=""right"">{$object.ANGOBSONZUBBRUTTO|0:n}</td>
                      </tr>
                      <tr>
                        <td>Pakete:</td>
                        <td align=""right"">(fehlt noch)</td>
                      </tr>
                      <tr>
                        <td>Händlerzubehör:</td>
                        <td align=""right"">(fehlt noch)</td>
                      </tr>
                      <tr>
                        <td>Herstellerzubehör:</td>
                        <td align=""right"">(fehlt noch)</td>
                      </tr>
                      <tr>
                        <td>Anschaffungswert (Barzahlungspreis):</td>
                        <td align=""right"">{0$object.ANGKALKBGEXTERNBRUTTO|0:n}</td>
                      </tr>
                      <tr>
                        <td>- NoVA-Aufschlag:</td>
                        <td align=""right"">(fehlt noch)</td>
                      </tr>
                      <tr>
                        <td></td>
                        <td align=""right""></td>
                      </tr>
                      <tr>
                        <td>Erfassungsdatum</td>
                        <td align=""right"">{$object.ERFASSUNG|0:D}</td>
                      </tr>
                    </table>
                    <hr size=""1"" width=""100%"">
                    <table border=""0"" width=""100%"">
                      <tr>
                        <td colspan=""5"">Anschaffungswert inkl. NoVA und USt</td>
                        <td align=""right"">(fehlt noch)</td>
                      </tr>
                      <tr>
                        <td></td>
                        <td></td>
                        <td align=""right"">EUR Netto</td>
                        <td align=""right"">USt-%</td>
                        <td align=""right"">EUR USt</td>
                        <td align=""right"">EUR Brutto</td>
                      </tr>
                      <tr>
                        <td>Mietvorauszahlung:</td>
                        <td align=""right"">{$object.ANGKALKSZBRUTTOP|0:0.00%}</td>
                        <td align=""right"">{$object.ANGKALKSZ|0:n}</td>
                        <td align=""right"">20%</td>
                        <td align=""right"">{$object.ANGKALKSZUST|0:n}</td>
                        <td align=""right"">{$object.ANGKALKSZBRUTTO|0:n}</td>
                      </tr>
                      <tr>
                        <td>Mitfinanzierter Bestandteil:</td>
                        <td align=""right""></td>
                        <td align=""right"">{$object.ANGKALKMITFIN|0:n}</td>
                        <td align=""right"">20%</td>
                        <td align=""right"">{$object.ANGKALKMITFINUST|0:n}</td>
                        <td align=""right"">{$object.ANGKALKMITFINBRUTTO|0:n}</td>
                      </tr>
                      <tr>
                        <td>Depot:</td>
                        <td align=""right"">{$object.ANGKALKDEPOTP|0:0.00%}</td>
                        <td align=""right""></td>
                        <td align=""right""></td>
                        <td align=""right""></td>
                        <td align=""right"">{$object.ANGKALKDEPOT|0:n}</td>
                      </tr>
                      <tr>
                        <td>Fahrleistung p.a:</td>
                        <td colspan=""3"">{$object.ANGOBJAHRESKM|0:n} km</td>
                        <td>Mehr-KM-Satz:</td>
                        <td align=""right"">{$object.ANGOBSATZMEHRKMBRUTTO|0:0.###}</td>
                      </tr>
                      <tr>
                        <td>Laufzeit:</td>
                        <td colspan=""3"">{$object.ANGKALKLZ} Monate (Vertragsbeginn = Monat der Fahrzeugübernahme)</td>
                        <td>Minder-KM-Satz:</td>
                        <td align=""right"">{$object.ANGOBSATZMINDERKMBRUTTO|0:0.###}</td>
                      </tr>
                      <tr>
                        <td>Verzinsungsart:</td>
                        <td colspan=""3"">{$object.HIST_ANGKALKZINSTYP} (Anpassung quartalsmäßig, Basis 3-Monats-Euribor) (vllt. noch falsch)</td>
                        <td>Toleranzgrenze:</td>
                        <td align=""right"">{$object.ANGOBKMTOLERANZ}</td>
                      </tr>
                      <tr>
                        <td colspan=""6""><hr size=""1"" width=""100%""></td>
                      </tr>
                      <tr>
                        <td colspan=""2""><b>Laufende monatliche Zahlungen</b></td>
                        <td align=""right"">EUR Netto</td>
                        <td align=""right"">USt-%</td>
                        <td align=""right"">EUR Ust.</td>
                        <td align=""right"">EUR Brutto</td>
                      </tr>
                      <tr>
                        <td colspan=""2"">Leasingentgelt</td>
                        <td align=""right"">{$object.ANGKALKRATE|0:n}</td>
                        <td align=""right"">20%</td>
                        <td align=""right"">{$object.ANGKALKRATEUST|0:n}</td>
                        <td align=""right"">{$object.ANGKALKRATEBRUTTO|0:n}</td>
                      </tr>
                      <tr>
                        <td colspan=""6""><b>gewählte Zusatzprodukte</b></td>
                      </tr>
                      <tr>
                        <td colspan=""6"">(fehlt noch)</td>
                      </tr>
                      <tr>
                        <td colspan=""6""><hr size=""1"" width=""100%""></td>
                      </tr>
                      <tr>
                        <td colspan=""5""><b>Gesamtbetrag</b></td>
                        <td align=""right"">{$object.ANGKALKRATEBRUTTO|0:n}</td>
                      </tr>
                      <tr>
                        <td colspan=""6""><b>Gebühren einmalig</b></td>
                      </tr>
                      <tr>
                        <td colspan=""5"">Rechtsgeschäftsgebühr mit erster Rate</td>
                        <td align=""right"">{$object.ANGKALKRGGEBUEHR|0:n}</td>
                      </tr>
                      <tr>
                        <td colspan=""2"">Bearbeitungsgebühr</td>
                        <td align=""right"">{$object.ANGKALKGEBUEHR|0:n}</td>
                        <td align=""right"">20%</td>
                        <td align=""right"">{$object.ANGKALKGEBUEHRUST|0:n}</td>
                        <td align=""right"">{$object.ANGKALKGEBUEHRBRUTTO|0:n}</td>
                      </tr>
                      <tr>
                        <td colspan=""6""><hr size=""1"" width=""100%""></td>
                      </tr>
                    </table>
                </body>
                </html>
				 ";
        }


        public string getHtmlTemplateString(string templateId)
        {
            throw new NotImplementedException();
        }
    }
}