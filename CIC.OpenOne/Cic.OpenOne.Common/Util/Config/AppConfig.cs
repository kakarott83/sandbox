using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Model.DdOiqueue;

namespace Cic.OpenOne.Common.Util.Config
{
    class CfgEntry
    {
        public String cfg {get;set;}
        public String sec { get; set; }
        public String var { get; set; }
        public String wert { get; set; }
        public String getKey()
        {
            return cfg + "_" + sec + "_" + var;
        }
    }
    /// <summary>
    /// AppConfig-Klasse
    /// </summary>
    [System.CLSCompliant(true)]
    public sealed class AppConfig
    {
        private static readonly AppConfig instance = new AppConfig();

        private Dictionary<String, CfgEntry> cfgList = new Dictionary<String, CfgEntry>();

      

        #region Constructors
        /// <summary>
        /// AppConfig Constructor
        /// </summary>
        private AppConfig()
        {
            Init();
        }
        #endregion

        /// <summary>
        /// AppConfig Instance
        /// </summary>
        public static AppConfig Instance
        {
            get
            {
                return instance;
            }
        }

        #region Methods
        /// <summary>
        /// Init
        /// </summary>
        public void Init()
        {
            reloadCFG();
        }

        /// <summary>
        /// Returns the given config value from db
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="sec"></param>
        /// <param name="var"></param>
        /// <returns></returns>
        public String getValueFromDb(String cfg, String sec, String var)
        {
            using (DdOiQueueExtended ctx = new DdOiQueueExtended())
            {
                String rval = ctx.ExecuteStoreQuery<String>("select cfgvar.wert from cfg, cfgsec,cfgvar where cfgvar.syscfgsec=cfgsec.syscfgsec and cfgsec.syscfg=cfg.syscfg and cfg.code='" + cfg + "' and cfgsec.code='" + sec + "' and cfgvar.code='" + var + "'", null).FirstOrDefault();
                return rval;
            }
        }
        /// <summary>
        /// Returns the given config value from db and returns default if no value set
        /// </summary>
        /// <param name="cfg"></param>
        /// <param name="sec"></param>
        /// <param name="var"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public String getValueFromDb(String cfg, String sec, String var, String def )
        {
            using (DdOiQueueExtended ctx = new DdOiQueueExtended())
            {
                String rval = ctx.ExecuteStoreQuery<String>("select cfgvar.wert from cfg, cfgsec,cfgvar where cfgvar.syscfgsec=cfgsec.syscfgsec and cfgsec.syscfg=cfg.syscfg and cfg.code='" + cfg + "' and cfgsec.code='" + sec + "' and cfgvar.code='" + var + "'", null).FirstOrDefault();
                if (rval == null) return def;
                return rval;
            }
        }

        /// <summary>
        /// Reloads a certain cfg branch
        /// </summary>
        /// <param name="code"></param>
        public void reloadCFG(String code)
        {
            using (DdOiQueueExtended Context = new DdOiQueueExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "code", Value = code });
                List<CfgEntry> entries = Context.ExecuteStoreQuery<CfgEntry>("select cfg.code cfg, cfgsec.code sec, cfgvar.code var,cfgvar.wert from cfg, cfgsec,cfgvar where cfgvar.syscfgsec=cfgsec.syscfgsec and cfgsec.syscfg=cfg.syscfg and cfg.activeflag=1 and cfg.code=:code", parameters.ToArray()).ToList();
                foreach (CfgEntry e in entries)
                {
                    cfgList[e.getKey()] = e;
                }
            }
        }

        /// <summary>
        /// Load all active CFG entries
        /// DEPRECATED!
        /// </summary>
        /// <returns></returns>
        public List<CFG> LoadCFG()
        {
            reloadCFG();
            return null;
        }
        public void reloadCFG()
        {
            using (DdOiQueueExtended Context = new DdOiQueueExtended())
            {
                List<CfgEntry> entries = Context.ExecuteStoreQuery<CfgEntry>("select cfg.code cfg, cfgsec.code sec, cfgvar.code var,cfgvar.wert from cfg, cfgsec,cfgvar where cfgvar.syscfgsec=cfgsec.syscfgsec and cfgsec.syscfg=cfg.syscfg and cfg.activeflag=1 ",null).ToList();
                foreach (CfgEntry e in entries)
                {
                    cfgList[e.getKey()] = e;
                }
            }
        }
        /// <summary>
        /// GetCfgEntry
        /// </summary>
        /// <param name="cfgListOuter">NOT USED</param>
        /// <param name="section">CFGSEC.Code</param>
        /// <param name="entry">CFGVAR.Code</param>
        /// <param name="defaultValue"></param>
        /// <param name="cfg">CFG.Code</param>
        /// <returns></returns>
        public String GetCfgEntry(List<CFG> cfgListOuter, string section, string entry, string defaultValue, string cfg)
        {
            return GetCfgEntry(cfg, section, entry, defaultValue);
        }

        public String GetCfgEntry(string cfg,string section, string entry, string defaultValue)
        {
            String key = cfg + "_" + section + "_" + entry;

            if (cfgList.ContainsKey(key))
            {
                String rval = cfgList[key].wert;
                if (rval == null)
                {
                    using (DdOiQueueExtended Context = new DdOiQueueExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "cfg", Value = cfg});
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "cfgsec", Value = section });
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "code", Value = entry });
                        rval = Context.ExecuteStoreQuery<String>("select usrcfgvar.wert from cic.usrcfg,cic.usrcfgsec,cic.usrcfgvar where usrcfg.sysusrcfg=usrcfgsec.sysusrcfg and usrcfgvar.sysusrcfgsec=usrcfgsec.sysusrcfgsec and usrcfg.code=:cfg and usrcfgsec.code=:cfgsec and usrcfgvar.code=:code", parameters.ToArray()).FirstOrDefault();
                        if(rval==null)
                            return defaultValue;
                        else
                        {
                            cfgList[key].wert = rval;
                        }
                    }
                }
                if (rval.StartsWith("!"))
                {
                    return Environment.GetEnvironmentVariable(rval.Substring(1));
                }
                return rval;
            }
            else
            {
                String rval = null;
                using (DdOiQueueExtended Context = new DdOiQueueExtended())
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "cfg", Value = cfg });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "cfgsec", Value = section });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "code", Value = entry });
                    rval = Context.ExecuteStoreQuery<String>("select usrcfgvar.wert from cic.usrcfg,cic.usrcfgsec,cic.usrcfgvar where usrcfg.sysusrcfg=usrcfgsec.sysusrcfg and usrcfgvar.sysusrcfgsec=usrcfgsec.sysusrcfgsec and usrcfg.code=:cfg and usrcfgsec.code=:cfgsec and usrcfgvar.code=:code", parameters.ToArray()).FirstOrDefault();
                    if (rval == null)
                        return defaultValue;
                    else
                    {
                        CfgEntry centry = new CfgEntry();
                        centry.wert = rval;

                        cfgList[key] = centry;
                    }
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// returns the cfgentry as long
        /// </summary>
        /// <param name="cfg">cfg</param>
        /// <param name="section">cfgsec</param>
        /// <param name="entry">cfgvar</param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public long GetCfgEntry(string cfg, string section, string entry, long defaultValue)
        {
            String key = cfg + "_" + section + "_" + entry;

            if (cfgList.ContainsKey(key))
            {
                String rval = cfgList[key].wert;
                if (rval == null) return defaultValue;
                try { 
                    return long.Parse(rval);
                }catch(Exception)
                {
                    return defaultValue;
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Returns the boolean value for the setting
        /// </summary>
        /// <param name="section"></param>
        /// <param name="entry"></param>
        /// <param name="defaultValue"></param>
        /// <param name="cfg"></param>
        /// <returns></returns>
        public bool getBooleanEntry(string section, string entry, bool defaultValue, string cfg)
        {
            String cfgvalue = GetCfgEntry(cfg,section, entry, defaultValue?"TRUE":"FALSE");
            
            if (cfgvalue == null) return false;
            
            if ("TRUE".Equals(cfgvalue.ToUpper().Trim()))
                return true;

            return false;
        }
        /// <summary>
        /// GetEntry
        /// </summary>
        /// <param name="section">CFGSEC.Code</param>
        /// <param name="entry">CFGVAR.Code</param>
        /// <param name="defaultValue"></param>
        /// <param name="cfg">CFG.Code</param>
        /// <returns></returns>
        public string GetEntry(string section, string entry, string defaultValue, string cfg)
        {
            return GetCfgEntry(cfg,section, entry, defaultValue);
        }

        /// <summary>
        /// getFilter
        /// </summary>
        /// <param name="section"></param>
        /// <param name="entry"></param>
        /// <param name="defaultValue"></param>
        /// <param name="cfg"></param>
        /// <returns></returns>
        public string getFilter(string section, string entry, string defaultValue, string cfg)
        {
            StringBuilder sb = new StringBuilder("");
            String filter = instance.GetEntry(section, entry, defaultValue, cfg);
            if (filter != null && filter.Length > 0)
            {
                sb.Append(filter);
                if (!filter.Equals(defaultValue))
                {
                    for (int i = 2; i < 10; i++)
                    {
                        String centry = entry + i;
                        filter = instance.GetEntry(section, centry, "DEFAULT", cfg);
                        if (filter != null && filter.Length > 0 && !"DEFAULT".Equals(filter))
                        {
                            sb.Append(" ");
                            sb.Append(filter);
                            sb.Append(" ");
                        }
                    }
                }
            }
            return sb.ToString();
        }
        #endregion

        #region My methods
        #endregion

    }
}