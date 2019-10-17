using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.One.DTO;
using Cic.One.Web.BO.Search;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.Web.BO
{
    /// <summary>
    /// Base Report BO for reporting
    /// works like searching, same input filters and same return structure based on ReportDto
    /// </summary>
    public class ReportBo : IReportBo
    {
        protected static Dictionary<String, String> reports = new Dictionary<string, string>();

        /// <summary>
        /// Returns a regvar configured report by its unique id or null if not found
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected String getReportById(String id)
        {
            RegVarDto result = BOFactoryFactory.getInstance().getAppSettingsBO().getAppSettingsItem(new igetAppSettingsItemsDto() { bezeichnung = RegVarPaths.getInstance().REPORTS + id,syswfuser=-1 });

           
            if (result != null && result.blobWert != null)
               return result.blobWert;
            return null;
        }

        /// <summary>
        /// Returns a list of results for the defined Report
        /// </summary>
        /// <param name="input"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        virtual
        public oSearchDto<ReportDto> getReportData(iSearchDto input, CredentialContext ctx)
        {
           
            throw new ArgumentException("Report Id not supported");

        }
    }
}
