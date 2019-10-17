using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    interface IBankNowCalculator
    {
        /// <summary>
        /// calculates the calculation
        /// </summary>
        /// <param name="kalkulation">Kalkulations DTO</param>
        /// <param name="prodCtx">Produktkontext</param>
        /// <param name="kalkCtx">Berechnungs-Kontext</param>
        /// <param name="rateError">Fehler bei Ratenberechnung</param>
        /// <returns></returns>
        KalkulationDto calculate(KalkulationDto kalkulation, prKontextDto prodCtx, kalkKontext kalkCtx, ref byte rateError);


        /// <summary>
        /// Calculates Provisions for Expected Loss Calculations
        /// uses a minimum required input interfaces
        /// </summary>
        /// <param name="prodCtx"></param>
        /// <param name="kundenScore"></param>
        /// <param name="finanzierungsbetrag"></param>
        /// <param name="zinsertrag"></param>
        /// <returns></returns>
        List<AngAntProvDto> calculateProvisionsDirect(prKontextDto prodCtx, String kundenScore, double finanzierungsbetrag, double zinsertrag);
    }
}
