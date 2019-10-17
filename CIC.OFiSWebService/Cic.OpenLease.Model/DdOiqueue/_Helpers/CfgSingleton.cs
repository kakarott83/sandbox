#region Usings
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;


#endregion

namespace Cic.OpenLease.Model.DdOiqueue
{
    [System.CLSCompliant(true)]
    public sealed class CfgSingleton
    {
        private static volatile CfgSingleton instance;
        public static CfgSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (LOCK)
                    {
                        if (instance == null)
                            instance = new CfgSingleton();
                    }
                }

                return instance;
            }
        }
        private static string LOCK = "LOCK";
        #region Private vars
        private CfgHelper cfgHelper = new CfgHelper();
        private List<CFG> cfgList;
        #endregion

        #region Constructors
        private CfgSingleton()
        {
            Init();
        }
        #endregion

        #region Methods
        public void Init()
        {
            cfgList = cfgHelper.LoadCFG();
        }

        public string GetEntry(string section, string entry, string defaultValue, string cfg)
        {
            return MyGetEntry(cfgHelper.GetCfgEntry(ref cfgList, section, entry, defaultValue, cfg));
        }

        /// <summary>
        /// Reloads a certain cfg branch
        /// </summary>
        /// <param name="code"></param>
        public void reloadCFG(String code)
        {
            using (OiqueueExtendedEntities Context = new OiqueueExtendedEntities())
            {
                CFG cfg = (from c in Context.CFG where c.ACTIVEFLAG == 1 && c.CODE.Equals(code) select c).FirstOrDefault();

                cfg.CFGSECList.Load();
                foreach (var cfgsec in cfg.CFGSECList)
                {
                    cfgsec.CFGVARList.Load();
                }
                foreach (CFG c in cfgList)
                {
                    if (c.CODE.Equals(code))
                    {
                        cfgList.Remove(c);
                        break;
                    }
                }
                cfgList.Add(cfg);

            }
        }
        #endregion

        #region My methods
        private string MyGetEntry(string entry)
        {
            // Resolve first char ! as environment var
            return entry;
        }


        public string getFilter(string section, string entry, string defaultValue, string cfg)
        {
            StringBuilder sb = new StringBuilder("");
            String filter = Instance.GetEntry(section, entry, defaultValue,cfg);
            if (filter != null && filter.Length > 0)
            {
                sb.Append(filter);
                if (!filter.Equals(defaultValue))
                {
                    for (int i = 2; i < 10; i++)
                    {
                        String centry = entry + i;
                        filter = Instance.GetEntry(section, centry, "DEFAULT", cfg);
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
    }
}
