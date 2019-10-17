using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// DMS Data Contract for DMS Updates OL->DMS
    /// </summary>
    public class DmsDataDto
    {
        /// <summary>
        /// Kundennummer, Pflichtfeld
        /// </summary>
        public long sysreferenz { get; set; }
        /// <summary>
        /// angebot/antrag/vt-Nummer
        /// Optional, wenn übergeben wird nur die Vertragsakte aktualisiert
        /// wenn nicht übergeben wird der Kundenstammdatensatz aktualisiert
        /// </summary>
        public String vertragsnummer { get; set; }
        /// <summary>
        /// DMR-Vorgangsnummer
        /// Optional
        /// Pflichtfeld wenn Vorgang im OL über DMR/DMS-Trigger angestossen wurde
        /// </summary>
        public String caseid { get; set; }
        /// <summary>
        /// (ANGEBOT,ANTRAG,VT)
        /// </summary>
        public String area { get; set; }
        /// <summary>
        /// techn. Schlüssel für area
        /// </summary>
        public long sysid { get; set; }
        /// <summary>
        /// techn. Schlüssel VT
        /// </summary>
        public long sysvt { get; set; }
        /// <summary>
        /// techn. Schlüssel Antrag
        /// </summary>
        public long sysantrag { get; set; }
        /// <summary>
        /// techn. Schlüssel Angebot
        /// </summary>
        public long sysangebot { get; set; }
        /// <summary>
        /// techn. Schlüssel IT
        /// </summary>
        public long sysit { get; set; }
        /// <summary>
        /// techn. Schlüssel KUNDE
        /// </summary>
        public long sysperson { get; set; }
        /// <summary>
        /// letzter Zustand aus Angebot,Antrag, VT
        /// </summary>
        public String attribut { get; set; }
        /// <summary>
        /// Kunde CODE
        /// </summary>
        public String personcode { get; set; }
        /// <summary>
        /// Kundenstammdaten
        /// </summary>
        public String name { get; set; }
        public String vorname { get; set; }
        public String strasse { get; set; }
        public String hsnr { get; set; }
        public String plz { get; set; }
        public String ort { get; set; }
        public DateTime gebdatum { get; set; }
        /// <summary>
        /// Flag BankNow Mitarbeiter
        /// </summary>
        public int isMA { get; set; }




    }
    public class DmsDataInDto
    {
        /// <summary>
        /// DMR-Vorgangsnummer
        /// Pflichtfeld
        /// </summary>
        public String caseid { get; set; }
        /// <summary>
        /// Dokument-Typ, Erforderlich
        /// </summary>
        public String documenttype { get; set; }
        /// <summary>
        /// DMS Dokumenten id
        /// Pflichtfeld
        /// </summary>
        public long documentId { get; set; }
        /// <summary>
        /// DMS Interface-Version String
        /// </summary>
        public String interfaceVersion { get; set; }

        /// <summary>
        /// Kundennummer
        /// </summary>
        public long sysreferenz { get; set; }
        /// <summary>
        /// angebot/antrag/vt-Nummer
        /// </summary>
        public String vertragsnummer { get; set; }


        //QR-Code-Daten (DMR-Fields) 
        //Ursprung: DMR
        //-------------------------------
        public long brand { get; set; }
        public long channel { get; set; }
        public long obart { get; set; }
        public long product { get; set; }
        public int syskdtyp { get; set; }
        public DateTime crtdate { get; set; }
        public String language { get; set; }
        public long pageIdx { get; set; }
        public long pages { get; set; }
        public int expectedReturnDok { get; set; }

        //DMR-Parameters 
        public String faxnr { get; set; }
        public String email { get; set; }
        /// <summary>
        /// Quelle wie FAX/EMAIL/WEB
        /// </summary>
        public int importsource { get; set; }
        public DateTime scantime { get; set; }
        //....ggf. weitere Metadaten
    }
}
