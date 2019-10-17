using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.DTO
{
    public class icreateOrUpdateFileattDto
    {
        public FileattDto fileatt { get; set; }

        public bool Send { get; set; }

        [NonSerialized]
        public Message error;
    }
}