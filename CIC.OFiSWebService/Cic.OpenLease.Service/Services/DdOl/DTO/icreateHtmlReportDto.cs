using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenLease.ServiceAccess.DdOl;

namespace Cic.OpenLease.Service.Services.DdOl.DTO
{
    /// <summary>
    /// Das Input Dto für createHtmlReport
    /// </summary>
    public class icreateHtmlReportDto<T>
    {
        /// <summary>
        /// Enthält das Objekt, welches über Reflection in das Template eingefügt wird
        /// </summary>
        //public Dictionary<string,string> Data { get; set; }
        public T Data { get; set; }

        /// <summary>
        /// Enthält die Id des Templates welches benutzt werden soll
        /// </summary>
        public int HtmlTemplateid { get; set; }
    }

    /// <summary>
    /// Input Parameter für den HtmlReporter vom Typ TestDTO
    /// </summary>
    public class icreateHtmlReportTestDto : icreateHtmlReportDto<TestDTO>
    {
    }

    /// <summary>
    /// Input Parameter für den HtmlReporter vom Typ ANGEBOTDto
    /// </summary>
    public class icreateHtmlReportANGEBOTDto : icreateHtmlReportDto<ANGEBOTDto>{

    }
}