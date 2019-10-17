using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.DTO
{
    public class ogetBonitaetDto : oBaseDto
    {
      //  public Cic.OpenLeaseAuskunftManagement.DTO.BonitaetOutDto output;
        public BonitaetDto bonitaet { get; set; }
        public byte[] reportData {get;set;}

    }
}