using System.Collections.Generic;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    public class VertragExtDto : VertragDto
    {
        long sysantrag
        {
            get
            {
                if (angAntObDto == null)
                    return 0;
                return angAntObDto.sysantrag;
            }
            set
            {
                if (angAntObDto == null)
                    angAntObDto = new AngAntObDto();
                angAntObDto.sysantrag = value;
                
                if (kalkulation != null && kalkulation.angAntAblDto != null)
                {
                    foreach (AngAntAblDto abloese in kalkulation.angAntAblDto)
                        abloese.sysantrag = value;
                }
            }
        }

        new double rw
        {
            get
            {
                if (kalkulation == null || kalkulation.angAntKalkDto == null)
                    return 0;
                return kalkulation.angAntKalkDto.rw;
            }
            set
            {
                if (kalkulation == null)
                    kalkulation = new KalkulationDto();
                if (kalkulation.angAntKalkDto == null)
                    kalkulation.angAntKalkDto = new AngAntKalkDto();
                kalkulation.angAntKalkDto.rw = value;
            }
        }

        double aktuellerate
        {
            get
            {
                if (kalkulation == null || kalkulation.angAntAblDto == null || kalkulation.angAntAblDto.Count == 0)
                    return 0;
                return kalkulation.angAntAblDto[0].aktuelleRate;
            }
            set
            {
                if (kalkulation == null)
                    kalkulation = new KalkulationDto();
                if (kalkulation.angAntAblDto == null)
                    kalkulation.angAntAblDto = new List<AngAntAblDto>();
                if (kalkulation.angAntAblDto.Count == 0)
                {
                    AngAntAblDto abloese = new AngAntAblDto();
                    abloese.sysantrag = sysantrag;
                    kalkulation.angAntAblDto.Add(abloese);
                }
                kalkulation.angAntAblDto[0].aktuelleRate = value;
            }
        }
    }
}
