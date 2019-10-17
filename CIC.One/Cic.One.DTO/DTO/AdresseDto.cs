using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public class AdresseDto : AdrDto 
    {
      

        /*Primärschlüssel */
        public long sysAdresse { get; set; }

        /*Verweis zum Adresstyp */
        public long sysAdrTp { get; set; }

        
        /*Verweis zur Person */
        public long sysPerson { get; set; }
   
      

        override public long getEntityId()
        {
            return sysAdresse;
        }
    }
}