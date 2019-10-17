using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public class RecurrDto : EntityDto
    {
        /*Primärschlüssel */
        public long sysRecurr { get; set; }
        /*Verweis zum Termin */
        public long sysApptmt { get; set; }
        /*Verweis zur Aufgabe */
        public long sysPtask { get; set; }
        /*Serienbeginn */
        public DateTime? startDate { get; set; }
        /*Serienende */
        public DateTime? endDate { get; set; }
        /*Anzahl Wiederholungen */
        public int? numberRecurr { get; set; }
        /*Periodizität (1=Täglich, 2=Wöchentlich, 3=Monatlich, 4=Jährlich) */
        public int period { get; set; }
        /*Intervall, alle x Tage, Wochen, Monate, Jahre - bezogen auf Periodizität */
        public int? intervall { get; set; }
        /*Tag im Monat (Werte von 1-31) */
        public int? dayIndex { get; set; }
        /*Monat im Jahr (Werte von 1-12) */
        public int? monthIndex { get; set; }
        /*Entwpricht dayOfTheWeekIndex Exchange (1=Erster,2=Zweiter,3=Dritter,4=Vierter,5=Letzter) */
        public int? dayWeekIndex { get; set; }
        /*An Montagen */
        public int onMondays { get; set; }
        /*An Dienstagen */
        public int onTuesdays { get; set; }
        /*An Mittwochen */
        public int onWednesdays { get; set; }
        /*An Donnerstagen */
        public int onThursdays { get; set; }
        /*An Freitagen */
        public int onFridays { get; set; }
        /*An Samstagen */
        public int onSaturdays { get; set; }
        /*An Sonntagen */
        public int onSundays { get; set; }
        /*Nur an Werktagen (abhängig von Feiertagskalender) */
        public int onWorkdays { get; set; }

        override public long getEntityId()
        {
            return sysRecurr;
        }
    }
}