using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    public class icreateOrUpdateMailmsgDto
    {
        public MailmsgDto mailmsg { get; set; }

        public bool Send { get; set; }

        [NonSerialized]
        public Message error;
    }
}