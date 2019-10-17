using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class ojoinChatDto : oBaseDto
    {
        List<WfuserDto> users;
        public List<WfuserDto> Users
        {
            get { return users; }
            set { users = value; }
        }
    }
}
