using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class RecalcDto : EntityDto
    {
        public long sysrecalc { get; set; }
        public long sysowuser { get; set; }
        public long sysvt { get; set; }
        public long sysperson { get; set; }
        public String code { get; set; }
        public String zustand { get; set; }
        public DateTime? zustandab { get; set; }
        public DateTime? calcdate { get; set; }
        public int lza { get; set; }
        public int lzv { get; set; }
        public int lzn { get; set; }
        public long lla { get; set; }
        public long llv { get; set; }
        public long lln { get; set; }
        public long kmist { get; set; }
        public DateTime? kmistdate { get; set; }
        public String kmistsource { get; set; }
        public long kmhoch { get; set; }
        public long kmgesamt { get; set; }
        public int rlz { get; set; }
        public int alterende { get; set; }
        public double rwa { get; set; }
        public double rwv { get; set; }
        public double rwn { get; set; }
        public double rwpa { get; set; }
        public double rwpv { get; set; }
        public double rwpn { get; set; }
        public double zinsa { get; set; }
        public double zinsv { get; set; }
        public double zinsn { get; set; }
        public double ratea { get; set; }
        public double ratev { get; set; }
        public double raten { get; set; }
        public double deltav { get; set; }
        public double deltan { get; set; }
        public double ratera { get; set; }
        public double raterv { get; set; }
        public double ratern { get; set; }
        public double deltarv { get; set; }
        public double deltarn { get; set; }
        public int anzra { get; set; }
        public int anzrv { get; set; }
        public int anzrn { get; set; }
        public double ratesa { get; set; }
        public double ratesv { get; set; }
        public double ratesn { get; set; }
        public double deltasv { get; set; }
        public double deltasn { get; set; }
        public double ratepa { get; set; }
        public double ratepv { get; set; }
        public double ratepn { get; set; }
        public double deltapv { get; set; }
        public double deltapn { get; set; }
        public double rateanaba { get; set; }
        public double rateanabv { get; set; }
        public double rateanabn { get; set; }
        public double deltaanabv { get; set; }
        public double deltaanabn { get; set; }
        public double rateersaa { get; set; }
        public double rateersav { get; set; }
        public double rateersan { get; set; }
        public double deltaersav { get; set; }
        public double deltaersan { get; set; }
        public double ratesona { get; set; }
        public double ratesonv { get; set; }
        public double ratesonn { get; set; }
        public double deltasonv { get; set; }
        public double deltasonn { get; set; }
        public double mehrmindera { get; set; }
        public double ergebnisv { get; set; }
        public double ergebnisn { get; set; }
        public double rwbasea { get; set; }
        public double rwbasev { get; set; }
        public double rwbasen { get; set; }
        public double rwcrva { get; set; }
        public double rwcrvv { get; set; }
        public double rwcrvn { get; set; }
        //public String slserial { get; set; }

        
        
        /// <summary>
        /// Flag for Rücknahmevereinbarung /readonly
        /// </summary>
        public int rnvflag { get; set; }
        /// <summary>
        /// Service Flag/readonly
        /// </summary>
        public int svflag { get; set; }
        /// <summary>
        /// Reifen Flag/readonly
        /// </summary>
        public int tireflag { get; set; }
        /// <summary>
        /// FUelservice flag/readonly
        /// </summary>
        public int fuelflag { get; set; }
        /// <summary>
        /// Anabmeldeflag/readonly
        /// </summary>
        public int anabflag { get; set; }
        /// <summary>
        /// Ersatzwagenflag/readonly
        /// </summary>
        public int ewflag { get; set; }
        /// <summary>
        /// Sonstige Services Flag/readonly
        /// </summary>
        public int soflag { get; set; }
        
        /// <summary>
        /// Depot / readonly
        /// </summary>
        public double depot { get; set; }

        /// <summary>
        /// not saved in db, transmitted from client to save the entitybez
        /// </summary>
        public String vertrag { get; set; }

        /// <summary>
        /// Kalkulationsstaffel, Flag Variabel (Ausprägungen: fix = 0, variabel = 1)/ readonly
        /// </summary>
        public int zinsvariabel { get; set; }
        /// <summary>
        /// Sommerreifen-Staffel 5010, Flag Variabel (Ausprägungen: fix = 0, variabel = 1)/ readonly
        /// </summary>
        public int reifenvariabel { get; set; }
        /// <summary>
        /// Service-Staffel 5000, Flag Variabel (Ausprägungen: fix = 0, variabel = 1)/ readonly
        /// </summary>
        public int servicevariabel { get; set; }
        
        /// <summary>
        /// Rahmen aus Angebot
        /// </summary>
        public long sysrvt { get; set; }

        ///Rekalkulation Reifen Felder
        /*	Laufzeit Berechnet (Aktuelle Angebot)	*/
        public int lzc { get; set; }
        /*	Laufleistung Berechnet (Aktuelle Angebot)	*/
        public long llc { get; set; }
        /*	Restwert Berechnet (Aktuelle Angebot)	*/
        public double rwc { get; set; }
        /*	Restwert % Berechnet (Aktuelle Angebot)	*/
        public double rwpc { get; set; }
        /*	Zinssatz Berechnet (Aktuelle Angebot)	*/
        public double zinsc { get; set; }
        /*	Finanzrate Berechnet (Aktuelle Angebot)	*/
        public double ratec { get; set; }
       
        /*	Ergebnis Berechnet (Aktuelle Angebot)	*/
        public double ergebnisc { get; set; }
        /*	Rate Petrol Berechnet (Aktuelle Angebot)	*/
        public double ratepc { get; set; }
        /*	Rate An-/Abmeldekoste Berechnet (Aktuelle Angebot)	*/
        public double rateanabc { get; set; }
        /*	Rate Ersatzwagen Berechnet (Aktuelle Angebot)	*/
        public double rateersac { get; set; }
        /*	Rate Sonstige Berechnet (Aktuelle Angebot)	*/
        public double ratesonc { get; set; }
        /*	Anzahl Vorderreifen Aktuell	*/
        public int stirescounta { get; set; }
        /*	Anzahl Vorderreifen Berechnet	*/
        public int stirescountc { get; set; }
        /*	Anzahl Vorderreifen Vorschlag	*/
        public int stirescountv { get; set; }
        /*	Anzahl Vorderreifen Neu	*/
        public int stirescountn { get; set; }
        /*	Preis Vorderreifen Aktuell	*/
        public double stirespricea { get; set; }
        /*	Preis Vorderreifen Vorschlag	*/
        public double stirespricev { get; set; }
        /*	Preis Vorderreifen Neu	*/
        public double stirespricen { get; set; }
        /*	Anzahl Hinterreifen Aktuell	*/
        public int wtirescounta { get; set; }
        /*	Anzahl Hinterreifen Berechnet	*/
        public int wtirescountc { get; set; }
        /*	Anzahl Hinterreifen Vorschlag	*/
        public int wtirescountv { get; set; }
        /*	Anzahl Hinterreifen Neu	*/
        public int wtirescountn { get; set; }
        /*	Preis Hinterreifen Aktuell	*/
        public double wtirespricea { get; set; }
        /*	Preis Hinterreifen Vorschlag	*/
        public double wtirespricev { get; set; }
        /*	Preis Hinterreifen Neu	*/
        public double wtirespricen { get; set; }
        /*	Anzahl Felgen Aktuell	*/
        public int rimscounta { get; set; }
        /*	Anzahl Felgen Berechnet	*/
        public int rimscountc { get; set; }
        /*	Anzahl Felgen Vorschlag	*/
        public int rimscountv { get; set; }
        /*	Anzahl Felgen Neu	*/
        public int rimscountn { get; set; }
        /*	Preis Felgen Aktuell	*/
        public double rimspricea { get; set; }
        /*	Preis Felgen Vorschlag	*/
        public double rimspricev { get; set; }
        /*	Preis Felgen Neu	*/
        public double rimspricen { get; set; }
        /*	Nebenkosten Reifen Aktuell	*/
        public double ttirespricea { get; set; }
        /*	Nebenkosten Reifen Vorschlag	*/
        public double ttirespricev { get; set; }
        /*	Nebenkosten Reifen Neu	*/
        public double ttirespricen { get; set; }
        /*	Aufschlag Aktuell	*/
        public double tiresadditionpa { get; set; }
        /*	Aufschlag Vorschlag	*/
        public double tiresadditionpv { get; set; }
        /*	Aufschlag Neu	*/
        public double tiresadditionpn { get; set; }
        /*	Reifen Wechselintervall Aktuell	*/
        public long tireschangeintervala { get; set; }
        /*	Reifen Wechselintervall Berechnet	*/
        public long tireschangeintervalc { get; set; }
        /*	Reifen Wechselintervall Vorschlag	*/
        public long tireschangeintervalv { get; set; }
        /*	Reifen Wechselintervall Neu	*/
        public long tireschangeintervaln { get; set; }
        /*	Reifen Rate Berechnet	*/
        public double raterc { get; set; }
        /*	Service Rate Berechnet (Aktuelle Angebot)	*/
        public double ratesc { get; set; }
        /*	Service Rate Aktuell Standard	*/
        public double ratesadef { get; set; }
        /*	Service Rate Neu Standard	*/
        public double ratesndef { get; set; }

        public double rwaufab { get; set; }
        public double reifenaufabn { get; set; }
        public double serviceaufab { get; set; }
        public double sfbasea { get; set; }
        public double sfbasev { get; set; }
        public double sfbasen { get; set; }
        public int ttirescountc { get; set; }
        public int ttirescounta { get; set; }
        public int tiressetc { get; set; }
        public int tiresseta { get; set; }
        public double ttirescountv { get; set; }
        public double ttirescountn { get; set; }
        public int tiressetv { get; set; }
        public double reifenkapitaln { get; set; }
        public int tiressetn { get; set; }
        public double tiressetneededn { get; set; }
        
        public double reifengewinn { get; set; }
        public double reifengewinnn { get; set; }
        public double servicepa { get; set; }
        public double servicepn { get; set; }
        public double sfbasec { get; set; }


        /*	Ausgleichszahlung Berechnet (Aktuelle Angebot)	*/
        public double deltac { get; set; }
        public double deltarc { get; set; }
        public double deltasc { get; set; }
        public double deltapc { get; set; }
        public double deltaanabc { get; set; }
        public double deltaersac { get; set; }
        public double deltasonc { get; set; }
        
        


        public override long getEntityId()
        {
            return sysrecalc;
        }

        override public String getEntityBezeichnung()
        {
            return (vertrag!=null?vertrag:"")+" ["+code+"]";
        }
    }
}
