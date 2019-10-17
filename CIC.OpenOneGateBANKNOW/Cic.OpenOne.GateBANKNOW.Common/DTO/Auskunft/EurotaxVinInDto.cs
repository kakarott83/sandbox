using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft
{
   public class EurotaxVinInDto
    {
        public DAO.Auskunft.EurotaxVinRef.ISOcountryType ISOCountryCode { get; set; }

        public DAO.Auskunft.EurotaxVinRef.ISOlanguageType ISOLanguageCode { get; set; }
 
        public string vinCode { get; set; }

        public string typeETGCode { get; set; }

        public bool extendedOutput { get; set; }

        public int serviceCode { get; set; }
        
        public string serviceId { get; set; }

        public long syswfuser { get; set; }

        public string area { get; set; } //BNRZW-1724

        public long sysid { get; set; } //BNRZW-1724
       
    }

}
