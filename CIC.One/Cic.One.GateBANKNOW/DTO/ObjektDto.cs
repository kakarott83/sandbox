using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.GateBANKNOW.DTO
{
    public class ObjektDto
    {
        /*	Eindeutiger Identifier In Mapping als Feld Schwacke übernommen Leer bei Fremdmarken	*/
        public String modellcode { get; set; }
        /*	Chassisnummer/Fahrgestellnummer/VIN Gefüllt bei Gebrauchtfahrzeugen	*/
        public String fident { get; set; }
        /*	Marke	*/
        public String hersteller { get; set; }
        /*	Modell 	*/
        public String bezeichnung { get; set; }
        /*	Verweis zur Objektart (Neu, gebraucht etc.) Mapping Excel OBJEKTART	*/
        public long sysobart { get; set; }
        /*	Erstzulassung 	*/
        public DateTime? erstzulassung { get; set; }
        /*	Zubehör Brutto 	*/
        public double zubehoerBrutto { get; set; }
        /*	Barkaufpreis Brutto 	*/
        public double ahkBrutto { get; set; }
        /*	Übernahmekilometer 	*/
        public long ubnahmeKm { get; set; }
        /*	Stammnummer (xxx.xxx.xxx) 	*/
        public String stammnummer { get; set; }
        /*	Lieferdatum (voraussichtlich) 	*/
        public DateTime? liefer { get; set; }
        /*	Schild/Kennzeichen 	*/
        public String kennzeichen { get; set; }
        /*	Farbe (aussen) 	*/
        public String farbeA { get; set; }
        /*	PW,LW	*/
        public String aklasse { get; set; }
        /*	Fabrikat bzw Modell aus Objekttyp 	*/
        public String fabrikat { get; set; }
        /*	Baujahr 	*/
        public DateTime? baujahr { get; set; }
        /*	Baumonat 	*/
        public String baumonat { get; set; }
        /*	Neupreis Brutto 	*/
        public double grundBrutto { get; set; }
        /*	Co2-Emissionen 	*/
        public double co2emi { get; set; }
        /*	Getriebe (Schaltung, Automatik) 	*/
        public String getriebe { get; set; }
        /*	Aufbau	*/
        public String aart { get; set; }
        /*	Fahrer (Name, Vorname) 	*/
        public String fahrer { get; set; }
        /*	Verweis zum Fahrertyp, bspw wie LN oder Dritter aus Fahrertypentabelle 	*/
        public String fahrerCode { get; set; }
        /*	Typengenehmigung 	*/
        public String gehnid { get; set; }
        /*	Versicherung Name 	*/
        public String versicherungName { get; set; }
        /*	Importeurcode 	*/
        public String impcode { get; set; }
        /*	Laufnummer	*/
        public String laufnummer { get; set; }


    }
}