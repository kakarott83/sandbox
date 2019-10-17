using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für Provisions Zusatzinformationen
    /// </summary>
    public class ZusatzinformationDto
    {
        /// <summary>
        /// Type of additional Information
        /// </summary>
        public ZusatzinformationType type
        {
            get;
            set;
        }

        /// <summary>
        /// Grundprovision Vertrag (P-Code)
        /// </summary>
        public double provisionGrund
        {
            get;
            set;
        }

        /// <summary>
        /// Grundprovision Neugeld (P-Code Neugeld)
        /// </summary>
        public double provisionNeugeld
        {
            get;
            set;
        }

        /// <summary>
        /// Grundprovision Ablösen (P-Code Ablösen)
        /// </summary>
        public double provisionAbloesen
        {
            get;
            set;
        }

        /// <summary>
        /// Provision Ratenabsicherung (V-Code)
        /// </summary>
        public double provisionRsv
        {
            get;
            set;
        }

        /// <summary>
        /// Zusatzprovision (S-Code)
        /// </summary>
        public double provisionZusatz
        {
            get;
            set;
        }

        /// <summary>
        /// Provision Total (Summe)
        /// </summary>
        public double provisionTotal
        {
            get;
            set;
        }
    }
}
