using System.Collections.Generic;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// Kunde Plus DTO
    /// </summary>
    public class KundePlusDto : KundeDto
    {
        /// <summary>
        /// Nationalitaet des Landes
        /// </summary>
        public LandDto landNationalitaet { get; set; }
        /// <summary>
        /// Wohnsotz im Land
        /// </summary>
        public LandDto landWohnsitz { get; set; }
        /// <summary>
        /// KPZ im Antrag
        /// </summary>
        public PkzDto pkzantrag { get; set; }
        /// <summary>
        /// Risikoklasse
        /// </summary>
        public RisikoklDto risikokl { get; set; }
        /// <summary>
        /// PKZ
        /// </summary>
        public PkzDto pkz { get; set; }
        /// <summary>
        /// Anzahl Vertraege
        /// </summary>
        public int anzV { get; set; }
        /// <summary>
        /// Anzahl Vertraege Express
        /// </summary>
        public int anzVExpress { get; set; }
        /// <summary>
        /// Anzahl Vertraege Dispo
        /// </summary>
        public int anzVDispo { get; set; }

        /// <summary>
        /// Budget 
        /// </summary>
        public double? budget { get; set; }

        /// <summary>
        /// Kredit innerhalb KKG (KreditKonsumGesetz)
        /// Normalerweise wird Budgetüberschuss auf 80'000 beschränkt, kann also nicht höher sein
        /// </summary>
        public double? kredinkkg { get; set; }

        /// <summary>
        /// Max Kredit innerhalb KKG (KreditKonsumGesetz)
        /// Budgetüberschuss multipliziert mit einem faktor
        /// kredinkkgMax kann höher als 80'000 sein
        /// </summary>
        public double? kredinkkgMax { get; set; }

        /// <summary>
        /// Kredit ausserhalb KKG (KreditKonsumGesetz)
        /// </summary>
        public double? kredoutkkg { get; set; }

        /// <summary>
        /// Vertraege Dispo
        /// </summary>
        public List<long> vDispo { get; set; }

        /// <summary>
        /// Fiktive Rate 36 Monate
        /// </summary>
        public double? rateBerechNeu36 { get; set; }

        /// <summary>
        /// Fiktive Rate 
        /// </summary>
        public double? rateBerechNeu { get; set; }

        /// <summary>
        /// Korrespondenz-Adresse Land
        /// </summary>
        public LandDto korrAdresse { get; set; }
    }
}