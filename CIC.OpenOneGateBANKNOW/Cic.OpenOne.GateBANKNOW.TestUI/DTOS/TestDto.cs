using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Attribute4UI;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxRef;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.TestUI.DTOS
{
    class TestDto
    {
        public int? intnull { get; set; }
        public int int1{ get; set; }
        public string Fields { get; set; }
        public KontoDto Konto { get; set; }
        public KontoDto[] KontoArray { get; set; }
        public List<KontoDto> kontosliste { get; set; }
        public string aftstring { get; set; }
        public bool replace { get; set; }
        public string searchstring { get; set; }
    }
}