using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public class BrancheDto : EntityDto
    {
        /*Primärschlüssel */
        public long sysBranche { get; set; }
        /*Bezeichnung */
        public String bezeichnung { get; set; }

        override public long getEntityId()
        {
            return sysBranche;
        }
    }
}