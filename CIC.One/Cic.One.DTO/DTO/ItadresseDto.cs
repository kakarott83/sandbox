using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public class ItadresseDto : AdrDto 
    {
     

        /*Primärschlüssel */
        public long sysItadresse { get; set; }
        public long sysIt { get; set; }

        override public long getEntityId()
        {
            return sysItadresse;
        }
    }
}