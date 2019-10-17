using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.One.DTO
{
    public class isendMessageDto
    {
        long syswfuser;
        /// <summary>
        /// Zu wem die Nachricht geschickt werden soll
        /// </summary>
        public long Syswfuser
        {
          get { return syswfuser; }
          set { syswfuser = value; }
        }

        string message;
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

    }
}
