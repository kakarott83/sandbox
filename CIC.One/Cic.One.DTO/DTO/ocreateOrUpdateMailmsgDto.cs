using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    /// <summary>
    /// Delivers newly updated or created E-Mails
    /// </summary>
    public class ocreateOrUpdateMailmsgDto : oBaseDto
    {
        public MailmsgDto mailmsg { get; set; }

    }
}