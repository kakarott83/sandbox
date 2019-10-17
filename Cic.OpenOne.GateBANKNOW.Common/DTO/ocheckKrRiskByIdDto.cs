using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Attribute4UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    public class ocheckKrRiskByIdDto
    {
        [AttributeLabel("ANTRAGSDATEN")]
        public ELCalcVars antvars { get; set; }
        [AttributeLabel("DECISIONENGINEDATEN")]
        public DEValues4KR devalues {get;set;}
        [AttributeLabel("WERTEGRUPPE EL")]
        public Flags4KR flags {get;set;}
        [AttributeLabel("EXPECTED LOSS ERGEBNIS")]
        public ELResults elresults {get;set;}
        [AttributeLabel("ZINSERTRAG BERECHNET")]
        public double zinsertrag {get;set;}
        [AttributeLabel("PRODUKT ZFAKTOR")]
        public double? zfaktor { get; set; }

    }
}
