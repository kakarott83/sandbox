using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO.Prisma;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// Parameterklasse für KalkulationDto
    /// </summary>
    public class KalkulationDto
    {
        /// <summary>
        /// 0..1 Finanzkalkulationsdaten
        /// </summary>
        public AngAntKalkDto angAntKalkDto { get; set; }

        /// <summary>
        /// 0..1 Finanzkalkulationsdaten Var1
        /// </summary>
        public AngAntKalkDto angAntKalkVar1Dto { get; set; }

        /// <summary>
        /// 0..1 Finanzkalkulationsdaten Var 2
        /// </summary>
        public AngAntKalkDto angAntKalkVar2Dto { get; set; }

        /// <summary>
        /// 0..1 Finanzkalkulationsdaten Var3
        /// </summary>
        public AngAntKalkDto angAntKalkVar3Dto { get; set; }

        /// <summary>
        /// 0..n Versicherungen
        /// </summary>
        public List<AngAntVsDto> angAntVsDto { get; set; }

        /// <summary>
        /// 0..n Provisionen
        /// </summary>
        public List<AngAntProvDto> angAntProvDto { get; set; }

        /// <summary>
        /// 0..n Ablösen
        /// </summary>
        public List<AngAntAblDto> angAntAblDto { get; set; }

        /// <summary>
        /// 0...n Provisionen Rap Min
        /// </summary>
        public List<AngAntProvDto> angAntProvDtoRapMin { get; set; }

        /// <summary>
        /// 0...n Provisionen Rap Max
        /// </summary>
        public List<AngAntProvDto> angAntProvDtoRapMax { get; set; }

        /// <summary>
        /// 0..n Subventionen
        /// </summary>
        public List<AngAntSubvDto> angAntSubvDto { get; set; }

    }
}
