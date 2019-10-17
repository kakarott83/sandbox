using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /**
     * Basis-objekt für OB, ANTOB, ANGOB
     */
    public class ObjektDto:EntityDto
    {
       
        /*	Primärschlüssel	*/
        public long sysOb { get; set; }
        /*	Verweis zur Objektart (Neu, gebraucht etc.)	*/
        public long sysObart { get; set; }
        /*	Verweis zur Objektkategorie (Ex AG …)	*/
        public long sysObkat { get; set; }

        public long sysObTyp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long sysVT { get; set; }
        public long sysAntrag { get; set; }

        public long sysLs { get; set; }

        public int aktivKZ { get; set; }

        /// <summary>
        /// Rang
        /// </summary>
        public int rang { get; set; }

        /// <summary>
        /// Zylinder Anzahl
        /// </summary>
        public int zylinder {get;set;}

        /// <summary>
        /// CCM
        /// </summary>
        public int ccm { get; set; }

        /// <summary>
        /// Normverbrauchsabgabe Flag
        /// </summary>
        public double nova { get; set; }
        /// <summary>
        /// Normverbrauchsabgabe in prozent (0-100)
        /// </summary>
        public double novap { get; set; }
        /// <summary>
        /// Objekt
        /// </summary
        public string objekt { get; set; }

        /// <summary>
        /// Bezeichnung Zusammengesetzt Modell&&E-Type&&Oka Code (zb 530d xDrive Touring F11 N57)
        /// </summary>
        public string bezeichnung { get; set; }

        /// <summary>
        /// Objektbezeichnung 
        /// </summary>
        public string objektVT { get; set; }

        /// <summary>
        /// activeFlag 
        /// </summary>
        public int activeFlag { get; set; }

        /// <summary>
        /// Vertrag 
        /// </summary>
        public string vertrag { get; set; }

        public String serie { get; set; }
        public String fgnr { get; set; }
        public String inventar { get; set; }
        
        public long ubnahmekm { get; set; }


        /*	Kaufpreis in Rechnungswährung	*/
        public double ahk { get; set; }

        /// <summary>
        /// Barkaufpreis Umsatzsteuer
        /// </summary>
        public double ahkUst { get; set; }
        /// <summary>
        /// Barkaufpreis Brutto
        /// </summary>
        public double ahkBrutto { get; set; }
        /// <summary>
        /// Anzahl Objekte
        /// </summary>
        public int anzahl { get; set; }

        /*	Anzahl Türen	*/
        public int anzahlSitze { get; set; }
        /*	Anzahl Sitze	*/
        public int anzahlTueren { get; set; }
        /*	Baujahr	*/
        public DateTime? baujahr { get; set; }
        /*	Baumonat	*/
        public String baumonat { get; set; }
        /*	Bestelldatum	*/
        public DateTime? bestellung { get; set; }

        /*	Erstzulassung	*/
        public DateTime? erstzul { get; set; }
        /*	Fabrikat*/
        public String fabrikat { get; set; }
        /*	Modell	*/
        public String modell { get; set; }
        /// <summary>
        /// Fahrer (Name, Vorname)
        /// </summary>
        public String fahrer { get; set; }
        /// <summary>
        /// Verweis zum Fahrertyp, bspw wie LN oder Dritter (Lookup und Übersetzung)
        /// </summary>
        public String fahrerCode { get; set; }
        /*	Farbe (aussen)	*/
        public String farbea { get; set; }
        /// <summary>
        /// Fahrzeugart (Boot, PW)
        /// </summary>
        public String fzart { get; set; }
        public String typfz { get; set; }
        /*	Listenpreis in Rechnungswährung	*/
        public double grund { get; set; }
        /// <summary>
        /// Neupreis Umsatzsteuer
        /// </summary>
        public double grundUst { get; set; }
        /// <summary>
        /// Neupreis Brutto
        /// </summary>
        public double grundBrutto { get; set; }
        /*	Marke	*/
        public String hersteller { get; set; }
        /// <summary>
        /// Jahreskilometer
        /// </summary>
        public long jahresKm { get; set; }
        /*	Schild/Kennzeichen/Kontrollschild	*/
        public String kennzeichen { get; set; }
        /*	Lieferdatum	*/
        public DateTime? lieferung { get; set; }
        
        /*	Rechnungsdatum	*/
        public DateTime? rechnung { get; set; }

        /// <summary>
        /// Betrag pro Mehrkilometer
        /// </summary>
        public double satzmehrKm { get; set; }

        /// <summary>
        /// Betrag pro Minderkilometer
        /// </summary>
        public double satzminderKm { get; set; }

        /// <summary>
        /// Eurotax/Schwacke Nummer (National Code)
        /// JATA-Nr in WKT
        /// </summary>
        public String schwacke { get; set; }

        /// <summary>
        /// Misused as Eurotax-Number in WKT
        /// </summary>
        public String fznr { get; set; }
       
        /*	Standort (via Lookup ddlkppos)	*/
        public String standort { get; set; }
       

        /*	Typ	*/
        public String typ { get; set; }

        /// <summary>
        /// Versicherung Name
        /// </summary>
        public String versicherungName { get; set; }
        /// <summary>
        /// Versicherung Ort
        /// </summary>
        public String versicherungOrt { get; set; }
        /// <summary>
        /// Versicherung Nummer
        /// </summary>
        public String versicherungNr { get; set; }

        /*	Händlerzubehör in Rechnungswährung	*/
        public double zubehoer { get; set; }
        /// <summary>
        /// Zubehör Umsatzsteuer
        /// </summary>
        public double zubehoerUst { get; set; }
        /// <summary>
        /// Zubehör Brutto
        /// </summary>
        public double zubehoerBrutto { get; set; }

        /*	Zustand	*/
        public String zustand { get; set; }

        /*	Zustand	*/
        public int importer { get; set; }

        /*	Kraftstoff	*/
        public int kraftstoff { get; set; }

        /*	Automatik	*/
        public int automatik { get; set; }

        /*	Wagentyp	*/
        public String wagentyp { get; set; }

        /*	Genehmigungs-Id	*/
        public String gehnid { get; set; }

        /*	Kostenstelle	*/
        public String bezdeutsch { get; set; }

        /// <summary>
        /// KW E-Motor
        /// </summary>
        public int kwe { get; set; }
        /// <summary>
        /// PS E-Motor
        /// </summary>
        public int pse { get; set; }
        public int kwgesamt { get; set; }
        public int psgesamt { get; set; }
        /// <summary>
        /// Energieeffizienzklasse
        /// </summary>
        public String eek { get; set; }
        public int reichweite { get; set; }
        /// <summary>
        /// Energieverbrauch
        /// </summary>
        public int kwh { get; set; }

        public int ps { get; set; }
        public int kw { get; set; }
        public long kmtoleranz { get; set; }

        public long sysfstypbsi { get; set; }
        public double mitfin { get; set; }

        public double lp { get; set; }

        public int bgn { get; set; }
        public double rw { get; set; }
        /// <summary>
        /// Object Equipment List
        /// </summary>
        public List<ObjektAustDto> ausstattungen { get; set; }
        public String ausstattung { get;set;}
       

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        override public long getEntityId()
        {
            return sysOb;
        }
    }
}