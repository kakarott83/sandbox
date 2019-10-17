namespace Cic.OpenLease.ServiceAccess.DdOl
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    #endregion

    public class ApprovalDto
    {
        #region Properties
        public string Votum
        {
            get;
            set;
        }

        public string VotumRem
        {
            get;
            set;
        }

        public string VotumKomp
        {
            get;
            set;
        }

        public DateTime? VotumDatum
        {
            get;
            set;
        }

        public string Genehmigung
        {
            get;
            set;
        }

        public string GenehmRem
        {
            get;
            set;
        }

        public string AuflagenRem
        {
            get;
            set;
        }

        public string AuflagenKomp
        {
            get;
            set;
        }

        public DateTime? AuflagenDatum
        {
            get;
            set;
        }

        public int? SicherHeiten
        {
            get;
            set;
        }

        public int? Kalkulation
        {
            get;
            set;
        }
        #endregion
    }
}
