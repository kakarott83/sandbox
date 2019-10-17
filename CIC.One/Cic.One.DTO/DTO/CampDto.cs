using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    public class CampDto :  TreeDto
    {
        /*Primärschlüssel */
        public long sysCamp { get; set; }

        /*Verweis zum Kampagnentyp */
        public long sysCampTp { get; set; }

        /* Kampagnentyp Bezeichnung */
        public String sysCampTpBezeichnung { get; set; }

        /*Verweis zur übergeordneten Kampagne (Rekursion) */
        public long sysCampParent { get; set; }

        /*Verweis zu Wfuser, Kampagneninhaber */
        public long sysOwner { get; set; }

        /*Privat, nur für Inhaber sichtbar (trotz Gesichtskreis) */
        public int privateFlag { get; set; }

        /*Bezeichnung */
        public String name { get; set; }

        /*Beschreibung */
        public String description { get; set; }

        /*Aktiv */
        public int activeFlag { get; set; }

        /*Beginn */
        public DateTime? validFrom { get; set; }

        /*Ende */
        public DateTime? validUntil { get; set; }

        /*(1=in Planung, 2=laufend, 3=abgeschlossen, 4=abgebrochen) */
        public int status { get; set; }

        /*Bei Mailing, Anzahl versendete Exemplare (bei Rekursion Anzwhl des Knotens) */
        public long numberSent { get; set; }

        /*Erwartete Antworten (bei Rekursion Anzahl des Knotens) */
        public long expResponse { get; set; }

        /*Bei Mailing, Anzahl Antworten (bei Rekursion Anzahl des Knotens) */
        public long numberResponse { get; set; }

        /*Erwarteter Umsatz (bei Rekursion die Umsätze des Knotens) */
        public double expRevenue { get; set; }

        /*Sollkosten, Budget (bei Rekursion das Budgets des Knotens) */
        public double budgetCosts { get; set; }

        /*Istkosten (bei Rekursion die Istkosten des Knotens) */
        public double actualCosts { get; set; }

        /*Flag für Händlerabgleich */
        public int hoflag { get; set; }

        override public long getEntityId()
        {
            return sysCamp;
        }

        public override string getEntityBezeichnung()
        {
            return name;
        }
    }
}