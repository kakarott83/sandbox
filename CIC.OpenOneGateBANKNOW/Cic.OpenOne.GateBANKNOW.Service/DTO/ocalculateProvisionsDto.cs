using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;


namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für
    /// <see cref="Cic.OpenOne.GateBANKNOW.Service.createOrUpdateAntragService.calculateProvisions"/> Methode
    /// </summary>
    public class ocalculateProvisionsDto : oBaseDto
    {
        public List<AngAntProvDto> provisions { get; set; }
    }
}
