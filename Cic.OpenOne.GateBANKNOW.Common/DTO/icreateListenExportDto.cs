using System;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// InputParameter ListenExport Methode
    /// </summary>
    public class icreateListenExportDto
    {
        /// <summary>
        /// Dient zum Umschalten zwischen Vertragslisten und Eventualverbindlichkeiten
        /// Mögliche Werte : EvVb (Eventualverbindlichkeiten) und lfVT (laufende Verträge)
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// Bei einem Wert größer als 0 werden die Leasingverträge aufbereitet, deren Vertragslaufzeit innerhalb der 
        /// Monatsanzahl ab heute endet. Das Feld Datum wird dabei nicht berücksichtigt.
        /// </summary>
        public int anzahlMonate { get; set; }

        /// <summary>
        /// Bei Datum ungleich Null (Option „Ablauf per“) werden nur Verträge aufbereitet, welche exakt am eingegebenen Datum enden.
        /// </summary>
        public DateTime datum { get; set; }

        /// <summary>
        /// Bei True wird die Liste Details wie Kunden, Fahrzeug und Ver-käufer des Fahrzeugs beinhalten. 
        /// </summary>
        public bool mitDetails { get; set; }

        /// <summary>
        /// Workflow user
        /// </summary>
        public long sysWFUser { get; set; }

        /// <summary>
        /// PeRole
        /// </summary>
        public long sysPeRole { get; set; }
    }
}