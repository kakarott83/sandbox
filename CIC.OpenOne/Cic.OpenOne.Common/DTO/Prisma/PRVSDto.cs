using System;

namespace Cic.OpenOne.Common.DTO.Prisma
{
    /// <summary>
    /// PR Versicherung DTO
    /// </summary>
    public class PRVSDto
    {
        /// <summary>
        /// SYSPRPRODUCT
        /// </summary>
        public long SYSPRPRODUCT { get; set; }

        /// <summary>
        /// SYSVSART
        /// </summary>
        public long? SYSVSART { get; set; }

        /// <summary>
        /// SYSPERSON
        /// </summary>
        public long? SYSPERSON { get; set; }

        /// <summary>
        /// SYSVSTYP
        /// </summary>
        public long SYSVSTYP { get; set; }

        /// <summary>
        /// Service Position Method
        /// </summary>
        public long METHOD { get; set; }

        /// <summary>
        /// Service Group Needed Flag
        /// </summary>
        public int NEEDEDGRP { get; set; }

        /// <summary>
        /// Service Group Disabled Flag
        /// </summary>
        public int DISABLEDGRP { get; set; }

        /// <summary>
        /// Service Position Needed Flag
        /// </summary>
        public int NEEDEDPOS { get; set; }

        /// <summary>
        /// Service Position Disabled Flag
        /// </summary>
        public int DISABLEDPOS { get; set; }

        /// <summary>
        /// MITFINFLAG
        /// </summary>
        public int MITFIN { get; set; }

        /// <summary>
        /// Service Group Id
        /// </summary>
        public int SETID { get; set; }

        /// <summary>
        /// Service Position Id
        /// </summary>
        public int POSID { get; set; }

        /// <summary>
        /// Service  Position Rank
        /// </summary>
        public int RANK { get; set; }

        /// <summary>
        /// FLAGDEFAULT
        /// </summary>
        public int FLAGDEFAULT { get; set; }

        /// <summary>
        /// VALIDFROM
        /// </summary>
        public DateTime? VALIDFROM { get; set; }

        /// <summary>
        /// VALIDUNTIL
        /// </summary>
        public DateTime? VALIDUNTIL { get; set; }

        /// <summary>
        /// VALIDFROMGRP
        /// </summary>
        public DateTime? VALIDFROMGRP { get; set; }

        /// <summary>
        /// VALIDUNTILGRP
        /// </summary>
        public DateTime? VALIDUNTILGRP { get; set; }

        /// <summary>
        /// EDITABLE
        /// </summary>
        public bool EDITABLE { get; set; }

        /// <summary>
        /// BESCHREIBUNG
        /// </summary>
        public string BESCHREIBUNG { get; set; }

        /// <summary>
        /// BEZEICHNUNG
        /// </summary>
        public string BEZEICHNUNG { get; set; }

        /// <summary>
        /// CODE
        /// </summary>
        public string CODE { get; set; }

        /// <summary>
        /// ISRSV - if sysvstyp is sysrsvtyp
        /// </summary>
        public short ISRSV { get; set; }

        /// <summary>
        /// Type of this service
        /// </summary>
        public ServiceType SERVICETYPE { get; set; }
    }
}
