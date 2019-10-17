using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Cic.OpenLease.Service.Services.DdOl.DTO;

namespace Cic.OpenLease.Service.Services.DdOl.Contract
{
    /// <summary>
    /// Das Interface für den Service, welcher HTML Reports anhand eines Dto's erzeugt.
    /// </summary>
    [ServiceContract(Name = "IHtmlReportService", Namespace = "http://cic-software.de/Cic.OpenLease.Service.DdOl")]
    public interface IHtmlReportService
    {

       
        
        /// <summary>
        /// Erzeugt einen HtmlReport
        /// </summary>
        /// <param name="input">icreateHtmlReportANGEBOTDto</param>
        /// <returns>ocreateHtmlReportDto</returns>
        [OperationContract]
        ocreateHtmlReportDto CreateHtmlReportANGEBOT(icreateHtmlReportANGEBOTDto input);
    }
}
