namespace Cic.OpenLease.ServiceAccess.DdOl
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    #endregion

    public class AuflagenInfoDto
    {
        #region Properties
       

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

        public string Form
        {
            get;
            set;
        }

        public string Bezeichnung
        {
            get;
            set;
        }

        public bool isComment
        {
            get;
            set;
        }

        /// <summary>
        /// PRUEFER
        /// </summary>
        public string PRUEFER
        {
            get;
            set;
        }

        /// <summary>
        /// Erfasser
        /// </summary>
        public string ERFASSER
        {
            get;
            set;
        }

        /// <summary>
        /// Infotext
        /// </summary>
        public string Beschreibung
        {
            get;
            set;
        }

        /// <summary>
        /// Datum
        /// </summary>
        public DateTime Datum
        {
            get;
            set;
        }
        /// <summary>
        /// Status
        /// </summary>
        public String STATUS
        {
            get;
            set;
        }
        #endregion
    }
}
