namespace Cic.OpenLease.ServiceAccess.Merge.OlClient
{
    #region Using
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    #endregion

    public class DocumentShortDto
    {
        #region Constructors
        public DocumentShortDto()
        {
        }

        public DocumentShortDto(string name, string description, AreaConstants area)
        {
            this.Name = name;
            this.Description = description;
            this.Area = area;
        }
        #endregion

        #region Properties
        public string Name
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public AreaConstants Area
        {
            get;
            set;
        }
        /// <summary>
        /// Druck
        /// </summary>
        public int Druck { get; set; }

        /// <summary>
        /// Abschnitt
        /// </summary>
        public String Section { get; set; }
        #endregion
    }
}
