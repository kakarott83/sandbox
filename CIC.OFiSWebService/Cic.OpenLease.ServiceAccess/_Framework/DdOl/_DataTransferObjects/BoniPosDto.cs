namespace Cic.OpenLease.ServiceAccess.DdOl
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    #endregion

    public class BoniPosDto
    {
        #region Properties
        public int? Rang
        {
            get;
            set;
        }

        public string Lieferant
        {
            get;
            set;
        }

        public string AngefVon
        {
            get;
            set;
        }

        public DateTime? AngefAm
        {
            get;
            set;
        }

        public DateTime? Erham
        {
            get;
            set;
        }

        public string Bezeichnung
        {
            get;
            set;
        }

        public string Beschreibung
        {
            get;
            set;
        }

        public string Form
        {
            get;
            set;
        }

        public string ScoreExt
        {
            get;
            set;
        }

        public int? ScoreInt
        {
            get;
            set;
        }

        public decimal? Gewichtung
        {
            get;
            set;
        }

        public int? ScoreRel
        {
            get;
            set;
        }

        public decimal? Depot
        {
            get;
            set;
        }

        public long? SysWaehrung
        {
            get;
            set;
        }

        #endregion
    }
}
