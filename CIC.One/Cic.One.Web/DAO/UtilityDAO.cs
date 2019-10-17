using Cic.One.DTO;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.Web.DAO
{
    /// <summary>
    /// DAO for accessing various utility data
    /// </summary>
    public class UtilityDAO
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const String QUERYCFG = "select cfg.code||'/'||cfgsec.code||'/'||cfgvar.code path,wert value from cfg,cfgsec,cfgvar where cfg.syscfg=cfgsec.syscfg and cfgsec.syscfgsec=cfgvar.syscfgsec and cfg.activeflag=1 and cfgsec.activeflag=1";
        private const String QUERYUSRCFG = "select usrcfg.code||'/'||usrcfgsec.code||'/'||usrcfgvar.code path,wert value from cic.usrcfg,cic.usrcfgsec,cic.usrcfgvar where usrcfg.sysusrcfg=usrcfgsec.sysusrcfg and usrcfgsec.sysusrcfgsec=usrcfgvar.sysusrcfgsec and usrcfg.activeflag=1 and usrcfgsec.activeflag=1";

        /// <summary>
        /// Returns a list of all cfg variables, where usercfg overrides cfg
        /// </summary>
        /// <returns></returns>
        public List<CFGDto> getConfigList()
        {
            using(PrismaExtended ctx = new PrismaExtended())
            {
                List<CFGDto> baseValues = ctx.ExecuteStoreQuery<CFGDto>(QUERYCFG, null).ToList();
                List<CFGDto> userValues = ctx.ExecuteStoreQuery<CFGDto>(QUERYUSRCFG, null).ToList();
                Dictionary<String,CFGDto> tmpMap = new Dictionary<string,CFGDto>();
                foreach(CFGDto cfg in baseValues)
                {
                    tmpMap[cfg.path]= cfg;
                }
                //override with user values
                foreach (CFGDto cfg in userValues)
                {
                    if(!tmpMap.ContainsKey(cfg.path))//usercfg wird nur verwendet, wenn nicht in CFG vorhanden!
                        tmpMap[cfg.path]= cfg;
                }
                return tmpMap.Values.ToList();
            }
            
        }
    }
}
