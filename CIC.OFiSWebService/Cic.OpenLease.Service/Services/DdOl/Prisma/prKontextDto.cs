using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenLease.Service
{
    public enum Prprodtype
    {
        /// <summary>
        /// Default Behaviour - B2B Products
        /// </summary>
       
        STANDARD = 0,
        /// <summary>
        /// RW Extension
        /// </summary>
        
        RWV = 1,
        /// <summary>
        /// Schnellkalk Products
        /// </summary>
       
        SCHNELLCALC = 2,
        /// <summary>
        /// B2C Products
        /// </summary>
       
        B2C = 3,
        /// <summary>
        /// B2C and B2B Products
        /// </summary>
      
        B2CANTRAG = 4,

    }
    /// <summary>
    /// Prisma Produkt Kontext
    /// </summary>
    [System.CLSCompliant(true)]
    public class prKontextDto : KontextDto
    {
        /// <summary>
        /// ID des Kanals
        /// </summary>
        public long sysprchannel
        {
            get;
            set;
        }

        /// <summary>
        /// ID der Brand
        /// </summary>
        public long sysbrand
        {
            get;
            set;
        }

        /// <summary>
        /// ID der Handelsgruppe
        /// </summary>
        public long sysprhgroup
        {
            get;
            set;
        }

        /// <summary>
        /// ID der Kundengruppe
        /// </summary>
        public long sysprkgroup
        {
            get;
            set;
        }

        /// <summary>
        /// ID des Kundentyps
        /// </summary>
        public long syskdtyp
        {
            get;
            set;
        }

        /// <summary>
        /// ID des Objekttyps
        /// </summary>
        public long sysobtyp
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
        /// ID des Zinsbindungstyps
        /// </summary>
        public long sysprinttype
        {
            get;
            set;
        }

        /// <summary>
        /// ID des Produktes
        /// </summary>
        public long sysprproduct
        {
            get;
            set;
        }

        /// <summary>
        /// ID der Vertragsart
        /// </summary>
        public long sysvart
        {
            get;
            set;
        }

        /// <summary>
        /// ID der Bildwelt
        /// </summary>
        public long sysprbildwelt
        {
            get;
            set;
        }

        /// <summary>
        /// VTTYP des Produktes und Antrag
        /// </summary>
        public long sysvttyp
        {
            get;
            set;
        }

        /// <summary>
        /// PRJOKER 
        /// </summary>
        public long sysprjoker
        {
            get;
            set;
        }

        /// <summary>
        /// PRODTYPE
        /// </summary>
        public Prprodtype prprodtype
        {
            get;
            set;
        }

        /// <summary>
        /// Treibstoffart
        /// </summary>
        public long sysprmart
        {
            get;
            set;
        }
        

    }
}
