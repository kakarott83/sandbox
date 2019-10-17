using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public class CtlangDto : EntityDto
    {
        /*Primärschlüssel */
        public long sysCtLang { get; set; }
        /*Sprache */
        public String languageName { get; set; }

        override public long getEntityId()
        {
            return sysCtLang;
        }
    }
}