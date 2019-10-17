using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;

namespace Cic.One.DTO
{
    /// <summary>
    /// Benutzer Kontextinformationen
    /// </summary>
    public class ogetValidateUserDto:oBaseDto
    {
        /// <summary>
        /// Benutzername
        /// </summary>
        public string username
        {
            get;
            set;
        }
        public int userType { get; set; }

        public WfuserDto userData { get; set; }

        /// <summary>
        /// Attributemap mit in der GUI verwendbaren Attributen
        /// </summary>
        public List<AttributeMapDto> attributmap
        {
            get;
            set;
        }

        /// <summary>
        /// Rolemap mit in der GUI verwendbaren Rollen
        /// </summary>
        public List<RoleMapDto> rolemap
        {
            get;
            set;
        }

        /// <summary>
        /// Rightsmap mit in der GUI verwendbaren Rechten
        /// </summary>
        public List<RightMapDto> rightsmap
        {
            get;
            set;
        }

        public prKontextDto kontext { get; set; }

    }
}