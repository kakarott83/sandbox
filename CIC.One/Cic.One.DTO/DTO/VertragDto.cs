using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class VertragDto :EntityDto
    {
        override public long getEntityId()
        {
            return sysID;
        }
        /// <summary>
        /// Bezeichnung for searchresults/fav/recent list
        /// </summary>
        /// <returns></returns>
        override public String getEntityBezeichnung()
        {
            return vertrag + " " + produktName + " " + kundeName;
        }

        /// <summary>
        /// ID
        /// </summary>
        public long sysID {get;set;}

        public long sysls { get; set; }
        public long sysadm { get; set; }
        public long syswaehrung { get; set; }	

        /// <summary>
        /// Antragsid
        /// </summary>
        public long sysAntrag { get; set; }	

        /// <summary>
        /// Vertragsnr
        /// </summary>
        public string vertrag {get;set;}

        /// <summary>
        /// Mandant
        /// </summary>
        public LsaddDto ls {get;set;}

        /// <summary>
        /// Währung
        /// </summary>
        public WaehrungDto waehrung	{get;set;}

        /// <summary>
        /// Vertragsart
        /// </summary>
        public long sysVart	{get;set;}

        /// <summary>
        /// Vertragstyp
        /// </summary>
        public long sysVttyp { get; set; }

        /// <summary>
        /// Händler
        /// </summary>
        public long sysVpfil { get; set; }

        /// <summary>
        /// Vertragsart
        /// </summary>
        public string vart { get; set; }

        /// <summary>
        /// Vertragtyp
        /// </summary>
        public string produktName{ get; set; }

        /// <summary>
        /// Konstellation
        /// </summary>
        public string konstellation	{get;set;}

        /// <summary>
        /// Vertriebsweg
        /// </summary>
        public string vertriebsweg {get;set;}

        /// <summary>
        /// Finanzierung
        /// </summary>
        public string fform	{get;set;}

        /// <summary>
        /// Beginn
        /// </summary>
        public DateTime beginn {get;set;}

        /// <summary>
        /// Übernahme
        /// </summary>
        public DateTime uebernahme	{get;set;}

        /// <summary>
        /// BGexterne
        /// </summary>
        public double bgExtern {get;set;}
        
        /// <summary>
        /// Laufzeit
        /// </summary>
        public int lz {get;set;}

        /// <summary>
        /// Restlaufzeit
        /// </summary>
        public int rlz { get; set; }

        /// <summary>
        /// Aktivierung
        /// </summary>
        public DateTime datAktiv {get;set;}	

        /// <summary>
        /// Sonderzahlung
        /// </summary>
        public double sz {get;set;}	
        
        /// <summary>
        /// PPJ
        /// </summary>
        public int ppy {get;set;}	

        /// <summary>
        /// Ende
        /// </summary>
        public DateTime ende {get;set;}	

        /// <summary>
        /// Rückgabe
        /// </summary>
        public DateTime rueckgabe {get;set;}		

        /// <summary>
        /// Restwert
        /// </summary>
        public double rw {get;set;}

        /// <summary>
        /// Zustand
        /// </summary>
        public string zustand {get;set;}

        /// <summary>
        /// Attribut
        /// </summary>
        public string attribut { get; set; }

        /// <summary>
        /// geprüft
        /// </summary>
        public int ok {get;set;}

        /// <summary>
        /// aktiviert
        /// </summary>
        public int aktivKZ	{get;set;}

        /// <summary>
        /// Locked
        /// </summary>
        public int locked {get;set;}

        /// <summary>
        /// beendet
        /// </summary>
        public int endeKZ {get;set;}

        /// <summary>
        /// Ende am
        /// </summary>
        public DateTime endeAm	{get;set;}

        /// <summary>
        /// Rate
        /// </summary>
        public double rate { get; set; }

        /// <summary>
        /// depot
        /// </summary>
        public double depot { get; set; }

        /// <summary>
        /// OB Liste
        /// </summary>
        public List<ObDto> obList { get; set; }

        /// <summary>
        /// vtobslList
        /// </summary>
        public List<VtobslDto> vtobslList { get; set; }

        /// <summary>
        /// OP
        /// </summary>
        public double op { get; set; }

        /// <summary>
        /// activeFlag 
        /// </summary>
        public int activeFlag { get; set; }


        /// <summary>
        /// ADMADD/Außendienstmitarbeiter
        /// </summary>
        public AdmaddDto aussendienstmitarbeiter { get; set; }


        /// <summary>
        /// VPFILADD/vertriebspartner
        /// </summary>
        public VpfiladdDto vertriebspartner { get; set; }

        public String objektkz { get; set; }
        public String fabrikat { get; set; }

        /// <summary>
        /// Name des Kunden über syskd
        /// </summary>
        public String kundeName { get; set; }

        /// <summary>
        /// syskd
        /// </summary>
        public long sysPerson { get; set; }

        /// <summary>
        /// syskd
        /// </summary>
        public long sysKd { get; set; }

        public double gesamt { get; set; }

        public int rnv { get; set; }
        public String[] mitantragsteller { get; set; }


        public String marke { get; set; }
        public String modell { get; set; }

        /// <summary>
        /// Mahnstufe
        /// </summary>
        public int mstufe { get; set; }

        /// <summary>
        /// Mitantragsteller
        /// </summary>
        public long sysMa { get; set; }

        /// <summary>
        /// Haendler
        /// </summary>
        public long sysVk { get; set; }

        /// <summary>
        /// Vermittler-Person
        /// </summary>
        public long sysBerater { get; set; }

        /// <summary>
        /// Produktinformationen aus der Kalkulation
        /// </summary>
        public ProduktInfoDto produkt { get; set; }

        /// <summary>
        /// Produkt ID
        /// </summary>
        public long? sysprproduct { get; set; }

        /// <summary>
        /// Vorvertrag
        /// </summary>
        public long sysvt { get; set; }

        /// <summary>
        /// ESR
        /// </summary>
        public int esrflag { get; set; }

        /// <summary>
        /// LSV/DTA
        /// </summary>
        public int einzug { get; set; }

        /// <summary>
        /// KKG Pflicht
        /// </summary>
        public int kkgpflicht { get; set; }

        /// <summary>
        /// Zahlsperre
        /// </summary>
        public int zahlsperre { get; set; }

        /// <summary>
        /// Gesperrt (aus nkklinie)
        /// </summary>
        public int gesperrt { get; set; }

        /// <summary>
        /// PPI (Versicherungssumme)
        /// </summary>
        public double verssumme { get; set; }

        /// <summary>
        /// Kreditrahmen
        /// </summary>
        public double grund { get; set; }
        
        /// <summary>
        /// Auftrag zu Vertrag mit Typ 141
        /// </summary>
        public AuftragDto auftrag { get; set; }

        /// <summary>
        /// Standardzins effektiv
        /// </summary>
        public double exteff { get; set; }

        /// <summary>
        /// Mehrwertsteuer Prozent
        /// </summary>
        public double mwst { get; set; }

        /// <summary>
        /// Ursprüngliches Vertragsende
        /// </summary>
        public DateTime? ltratedat { get; set; }

        /// <summary>
        /// LR inkl.
        /// </summary>
        public double ltrate { get; set; }

        /// <summary>
        /// Unterschriftsdatum
        /// </summary>
        public DateTime? udatum { get; set; }

        /// <summary>
        /// Auszahlungsdatum
        /// </summary>
        public DateTime? ausdatum { get; set; }

        /// <summary>
        /// ZEK-Gesuch ID
        /// </summary>
        public string zekgesuchsid { get; set; }

        /// <summary>
        /// ZEK-Vertrags ID
        /// </summary>
        public string zekvertragsid { get; set; }

        /// <summary>
        /// Risikoklasse ID
        /// </summary>
        public long sysrisikokl { get; set; }

        /// <summary>
        /// Zinsreduktionscode
        /// </summary>
        public long sysintstrct { get; set; }

        /// <summary>
        /// card account id
        /// </summary>
        public string accountid { get; set; }

        /// <summary>
        /// ulon04 from vtoption
        /// </summary>
        public long aufschubfrist { get; set; }

        /// <summary>
        /// Version Vertrag
        /// </summary>
        public string version { get; set; }

        /// <summary>
        /// dat13 from vtoption
        /// </summary>
        public DateTime? zinsGarantBis { get; set; }

        /// <summary>
        /// option8+option7 from vtoption
        /// </summary>
        public string ratenpause { get; set; }

        /// <summary>
        /// Aufstockstop bis
        /// </summary>
        public DateTime? aufstockstopbis { get; set; }

        /// <summary>
        /// KLINIE: Beanspruchter Kredit Betrag
        /// </summary>
        public double beanspruchterKredit { get; set; }

        /// <summary>
        /// KLINIE: Beanspruchter Kredit Prozent
        /// </summary>
        public double beanspruchterKreditProz { get; set; }

        /// <summary>
        /// Anzahl Auszahlungen
        /// </summary>
        public long anzausz { get; set; }

        /// <summary>
        /// Auszahlungsgebühr
        /// </summary>
        public double gebuehr { get; set; }

        /// <summary>
        /// Saldoabsicherungsprämie
        /// </summary>
        public double ppifact { get; set; }

        /// <summary>
        /// Vertragszins
        /// </summary>
        public double zins { get; set; }

        /// <summary>
        /// Tabelle Zinssätze
        /// </summary>
        public List<IntkBandDto> intkband { get; set; }

        /// <summary>
        /// Kontakt Firma Daten (aus SYSKD-Person Feldern)
        /// </summary>
        public String kontfirmaname { get; set; }
        public String kontfirmavorname { get; set; }

        public DateTime? databschluss { get; set; }

        /// <summary>
        /// mwstbeginn
        /// </summary>
        public double mwstbeginn { get; set; }

        /// <summary>
        /// mwstende
        /// </summary>
        public double mwstende { get; set; }

        /// <summary>
        /// ursprRateExcl 
        /// </summary>
        public double ursprRateExcl { get; set; }


        /// <summary>
        /// ursprRateExcl 
        /// </summary>
        public double ursprRateIncl { get; set; }

        /// <summary>
        /// Benutzer
        /// </summary>
        public string benutzer { get; set; }

        /// <summary>
        /// Segment
        /// </summary>
        public string segment { get; set; }

		/// <summary>
		/// jahreskm Laufleistung (rh: 20180202)
		/// </summary>
		public long jahreskm { get; set; }

		/// <summary>
		/// kundeplz (rh: 20180202)
		/// ACHTUNG! MUSS als string geführt werden, 
		/// da im Feld PERSON::PLZ auch Buchstaben und (vor allem) Sterne drin sind
		/// </summary>
		public String kundeplz { get; set; }
		
		/// <summary>
		/// kundeort  (rh: 20180202)
		/// </summary>
		public String kundeort { get; set; }

		/// <summary>
		/// kundestrasse 
		/// </summary>
		public String kundestrasse { get; set; }

		/// <summary>
		/// kunden-Telefonnummer 
		/// </summary>
		public String kundetelnr { get; set; }

		/// <summary>
		/// kunde email 
		/// </summary>
		public String kundeemail { get; set; }
		/// <summary>
		///  0 for fix and 1 for variable interest
		///		SELECT inttype FROM inttype WHERE sysinttype=XY
		/// </summary>
		public int zinstyp { get; set; }

		/// <summary>
		/// "fix" for zinstyp 0 and "var" for zinstyp 1 (or other)  
		/// </summary>
		public String zinsart
		{
			get { if (zinstyp == 0) return "fix"; else return "var"; }
			set { }
		}


		/// <summary>
		/// Auflagenanzahl
		/// </summary>
		public int auflagenanzahl { get; set; }

		/// <summary>
		/// Auflagen JA / NEIN
		/// </summary>
		public String auflagen
		{
			get { if (auflagenanzahl > 0) return "JA"; else return "NEIN"; }
			set {  }
		}

	}
}