namespace Cic.OpenLease.ServiceAccess.DdOl
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    #endregion

    public class BonitaetDto
    {
        #region Properties
        public DateTime? Erstelltam
        {
            get;
            set;
        }

        public int? Rang
        {
            get;
            set;
        }

        public string SbeArbeiter
        {
            get;
            set;
        }

        public string Bemerkung
        {
            get;
            set;
        }

        public int? BankScore
        {
            get;
            set;
        }

        public int? BilanzScore
        {
            get;
            set;
        }

        public int? BwaScore
        {
            get;
            set;
        }

        public int? GesamtScore
        {
            get;
            set;
        }

        public int? HandelsScore
        {
            get;
            set;
        }

        public int? KreditScore
        {
            get;
            set;
        }

        public int? ObjektScore
        {
            get;
            set;
        }

        public int? RatingScore
        {
            get;
            set;
        }

        public int? SchufaScore
        {
            get;
            set;
        }

        public int? ZahlScore
        {
            get;
            set;
        }

        public BoniPosDto[] BoniPos
        {
            get;
            set;
        }
        #endregion
    }
}
