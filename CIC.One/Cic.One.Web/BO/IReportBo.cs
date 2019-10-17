using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.One.DTO;
using Cic.OpenOne.Common.Util.Security;

namespace Cic.One.Web.BO
{
    /// <summary>
    /// Interface for Reporting
    /// </summary>
    public interface IReportBo
    {
        /// <summary>
        /// Returns a list of results for the defined Report
        /// </summary>
        /// <param name="input"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        oSearchDto<ReportDto> getReportData(iSearchDto input, CredentialContext ctx);
    }
}
