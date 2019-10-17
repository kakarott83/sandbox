using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    /// <summary>
    /// OutputParameter für <see cref="Cic.OpenOne.GateBANKNOW.Service.AuskunftService.EurotaxGetForecast"/> Methode
    /// </summary>
    public class oEurotaxGetForecastDto : oBaseDto
    {
        /// <summary>
        /// List of EurotaxForecastDto
        /// </summary>
        public List<EurotaxGetForecastDto> EurotaxForecastList { get; set; }
        
        /// <summary>
        /// true when data comes from internal rv tables
        /// </summary>
        public Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.EurotaxSource source { get; set; }
    }
}
