using System.Collections.Generic;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    /// <summary>
    /// OutputParameter für extendedValidateUser Methode
    /// </summary>
    public class oExtendedUserDto : oBaseDto
    {
        /// <summary>
        /// Benutzername
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// Attributemap mit in der GUI verwendbaren Attributen
        /// </summary>
        public List<AttributeMap> attributmap { get; set; }

        /// <summary>
        /// Rolemap mit in der GUI verwendbaren Rollen
        /// </summary>
        public List<RoleMap> rolemap { get; set; }

        /// <summary>
        /// Rightsmap mit in der GUI verwendbaren Rechten
        /// </summary>
        public List<RightsMap> rightsmap { get; set; }
    }
}