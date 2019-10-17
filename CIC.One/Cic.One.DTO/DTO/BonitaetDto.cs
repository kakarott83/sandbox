using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;
namespace Cic.One.DTO
{
    public class BonitaetDto : EntityDto
    {
        
        override public long getEntityId()
        {
            return 1;
        }
        public Cic.OpenLeaseAuskunftManagement.CrefoService.Tsearchhit[] hit { get; set; }
        public Cic.OpenLeaseAuskunftManagement.CrefoService.Tcompanyidentification companyidentification { get; set; }
        public Cic.OpenLeaseAuskunftManagement.CrefoService.Tsolvencyindex solvencyindex { get; set; }
       
    }
}