using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
using Cic.One.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Returns all permitted users
    /// </summary>
    public class ogetPermittedUsersDto : oBaseDto
    {
        public WfuserDto[] users
        {
            get;
            set;
        }
    }
}