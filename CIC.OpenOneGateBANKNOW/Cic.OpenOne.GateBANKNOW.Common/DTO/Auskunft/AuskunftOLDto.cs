using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
   
    /// <summary>
    /// Interne Auskunft-FehlerCodes
    /// </summary>
    public enum AuskunftOLErrorCode
    {
        /// <summary>
        /// Erfolgreiche Verarbeitung
        /// </summary>
        NoError = 0,

        /// <summary>
        /// Externer Dienst nicht erreichbar (technischer Fehler ZEK)
        /// </summary>
        ErrorZEK = -1,

        /// <summary>
        /// Interner technischer Verarbeitungsfehler (technischer Fehler CIC)
        /// </summary>
        ErrorCIC = -2,

        /// <summary>
        /// Batch-Request an ZEK gesendet (bereit für batchStatus-Aufruf)
        /// </summary>
        BatchRequestSent = -5
    }

    /// <summary>
    /// Information Data Access Object
    /// </summary>
    [System.CLSCompliant(false)]
    public class AuskunftOLDto
    {
        /// <summary>
        /// Getter/Setter System Information
        /// </summary>
        public long sysAuskunft { get; set; }

        /// <summary>
        /// Getter/Setter System Information Type
        /// </summary>
        public long sysAuskunfttyp { get; set; }

        /// <summary>
        /// Getter/Setter State
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Getter/Setter Information request Date
        /// </summary>
        public DateTime Anfragedatum { get; set; }

        /// <summary>
        /// Getter/Setter Information request time
        /// </summary>
        public long Anfrageuhrzeit { get; set; }

        /// <summary>
        /// Getter/Setter Errorcode
        /// </summary>
        public String Fehlercode { get; set; }

        /// <summary>
        /// Getter/Setter Request Object
        /// </summary>
        public string requestXML { get; set; }

        /// <summary>
        /// Getter/Setter Response Object
        /// </summary>
        public string responseXML { get; set; }

        /// <summary>
        /// sysAreaid 
        /// </summary>
        public long sysAreaid { get; set; }

        /// <summary>
        /// Area
        /// </summary>
        public string area { get; set; }

        /// <summary>
        /// Bemerkung
        /// </summary>
        public string bemerkung { get; set; }

        /// <summary>
        /// Abfrage auf Zek extern / oder aus DB
        /// </summary>
        public bool externeAbrage { get; set; }

        /// <summary>
        /// "sprechende" Vertragnummer
        /// </summary>
        public string vertragnummer { get; set; }

        /// <summary>
        /// "sprechende" Antragnummer
        /// </summary>
        public string antragnummer { get; set; }

        /// <summary>
        /// Vertriebspartnernummer
        /// </summary>
        public string vpnummer { get; set; }

        /// <summary>
        /// WFUSER
        /// </summary>
        public long syswfuser { get; set; }

        /// <summary>
        /// Getter/Setter ZEK Input Data Structure
        /// </summary>
        public ZekOLInDto ZekOLInDto { get; set; }

        /// <summary>
        /// Getter/Setter ZEK Output Data Structure
        /// </summary>
        public ZekOLOutDto ZekOLOutDto { get; set; }

    }
}