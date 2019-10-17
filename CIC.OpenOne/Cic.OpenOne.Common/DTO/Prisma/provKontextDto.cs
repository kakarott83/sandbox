using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.Common.DTO.Prisma
{
    /// <summary>
    /// Provisionsermittlungskontext
    /// </summary>
    public class provKontextDto : KontextDto
    {
        /// <summary>
        /// Brand-Zuordnung
        /// </summary>
        public long sysbrand
        {
            get;
            set;
        }
        /// <summary>
        /// Handelsgruppen-Zuordnung
        /// </summary>
        public long sysprhgroup
        {
            get;
            set;
        }
        /// <summary>
        /// Vertragsart-Zuordnung
        /// </summary>
        public long sysvart
        {
            get;
            set;
        }
        /// <summary>
        /// Vertragsunterart-Zuordnung
        /// </summary>
        public long sysvarttab
        {
            get;
            set;
        }
        /// <summary>
        /// Vertragstyp-Zuordnung
        /// </summary>
        public long sysvttyp
        {
            get;
            set;
        }
        /// <summary>
        /// Produkt-Zuordnung
        /// </summary>
        public long sysprproduct
        {
            get;
            set;
        }
        /// <summary>
        /// Versicherungstyp-Zuordnung
        /// </summary>
        public long sysvstyp
        {
            get;
            set;
        }
        /// <summary>
        /// Fullservicetyp-Zuordnung
        /// </summary>
        public long sysfstyp
        {
            get;
            set;
        }
        /// <summary>
        /// Ablösetyp-Zuordnung
        /// </summary>
        public long sysabltyp
        {
            get;
            set;
        }

        /// <summary>
        /// Objekt-Zuordnung
        /// </summary>
        public long sysobtyp
        {
            get;
            set;
        }

        /// <summary>
        /// PRKGROUP-Zuordnung
        /// </summary>
        public long sysprkgroup
        {
            get;
            set;
        }

        /// <summary>
        /// Vertrag syscode
        /// </summary>
        public long sysvt
        {
            get;
            set;
        }

        /// <summary>
        /// ID des Kanals
        /// </summary>
        public long sysprchannel
        {
            get;
            set;
        }

        /// <summary>
        /// ID der Objektart
        /// </summary>
        public long sysobart
        {
            get;
            set;
        }

        /// <summary>
        /// ID der Nutzungsart
        /// </summary>
        public long sysprusetype
        {
            get;
            set;
        }

        /// <summary>
        /// Händler-Segment
        /// </summary>
        public String segment
        {
            get;
            set;
        }
    }
}
