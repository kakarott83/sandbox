using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    /// <summary>
    /// Account (PERSON) Zusatzdaten, werden meist erst noch ermittelt/berechnet
    /// </summary>
    public class AccountExtDataDto
    {
        /// <summary>
        /// Budgetüberschuss aus kremo
        /// </summary>
        public double budget1 { get; set; }
        /// <summary>
        /// Budgetüberschuss aus kremo, wenn 2 Antragsteller
        /// </summary>
        public double saldo { get; set; }

        /// <summary>
        /// Einkommengesamt aus kremo
        /// </summary>
        public double einknettoberech { get; set; }

        /// <summary>
        /// Miete aus PKZ
        /// </summary>
        public double miete { get; set; }

        /// <summary>
        /// Engagement _xql:fetch('ENGAGEMENTKUNDE', 'CICSQL5:F01')
        /// </summary>
        public double engagement { get; set; }
        /// <summary>
        /// Summe offene Rechnungen _OP:XTD('PERSON',PERSON:SysPERSON,'',0)
        /// </summary>
        public double opos { get; set; }
        /// <summary>
        /// Höchste Mahnstufe vtmahn
        /// </summary>
        public int mstufe { get; set; }
        /// <summary>
        /// Total 1. Mahnstufe vtmahn
        /// </summary>
        public int mzaehler1 { get; set; }
        /// <summary>
        /// Total 2. Mahnstufe vtmahn
        /// </summary>
        public int mzaehler2 { get; set; }
        /// <summary>
        /// Total 3. Mahnstufe vtmahn
        /// </summary>
        public int mzaehler3 { get; set; }
        /// <summary>
        /// Letzes Mahndatum Stufe 1
        /// </summary>
        public DateTime? mdatum1 { get; set; }
        /// <summary>
        /// Letzes Mahndatum Stufe 2
        /// </summary>
        public DateTime? mdatum2 { get; set; }
        /// <summary>
        /// Letzes Mahndatum Stufe 3
        /// </summary>
        public DateTime? mdatum3 { get; set; }
        /// <summary>
        /// Aufstockstop bis antrag.aufstockstopbis (agg über alle Verträge)
        /// </summary>
        public DateTime? aufstockstopbis { get; set; }

        public String kam { get; set; }
        public String abwicklungsort { get; set; }
        public String risikoklasse { get; set; }
        public double restwertlimite { get; set; }
        public double restwertengagement { get; set; }
        public double restwertevtlengagement { get; set; }

    }
}
