// HC 05.05.2010
#region Usings
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

#endregion

namespace Cic.OpenLease.Model.DdOiqueue
{
    [System.CLSCompliant(true)]
    public sealed class CfgHelper
    {
        #region Publics
        public readonly CFG cfgEntity;
        #endregion
        #region Constructors
        public CfgHelper()
        {
        }
        #endregion
        #region Methods

        public string GetCfgEntry(ref List<CFG> cfgList, string section, string entry, string defaultValue, string cfg)
        {
            CFG selectedCfg;
            CFGSEC selectedCfgSec;
            CFGVAR selectedCfgVar;

            selectedCfg = (from c in cfgList
                               where c.CODE == cfg.ToUpper()
                               select c).FirstOrDefault();

            //var selectedCfg = cfgList.Where(p => p.code == entry).FirstOrDefault()

            if (selectedCfg == null)
            {
                return defaultValue;
            }
            else
            {
                selectedCfgSec = (from s in selectedCfg.CFGSECList
                               where (s.CODE == section.ToUpper() && s.ACTIVEFLAG == 1)
                               select s).FirstOrDefault();
                if (selectedCfgSec == null)
                {
                    return defaultValue;
                }
                else
                {
                    selectedCfgVar = (from v in selectedCfgSec.CFGVARList
                                      where (v.CODE == entry.ToUpper())
                                      select v).FirstOrDefault();
                    if (selectedCfgVar == null)
                    {
                        return defaultValue;
                    }
                    else
                    {
                        if (selectedCfgVar.WERT != null && selectedCfgVar.WERT.Length > 1)
                        {
                            if (selectedCfgVar.WERT[0] == '!')
                            {
                                return Environment.GetEnvironmentVariable(selectedCfgVar.WERT.Substring(1));
                            }
                        }
                        return selectedCfgVar.WERT;
                    }
                }
            }
        }
        public List<CFG> LoadCFG()
        {
            using (OiqueueExtendedEntities Context = new OiqueueExtendedEntities())
            {
                List<CFG>  cfgList = (from c in Context.CFG where c.ACTIVEFLAG == 1 select c).ToList();
                foreach (var cfg in cfgList)
                {
                    cfg.CFGSECList.Load();
                    foreach (var cfgsec in cfg.CFGSECList)
                    {
                        cfgsec.CFGVARList.Load();
                    }
                }
                return cfgList;
            }
        }

        

        #endregion
        #region My methods

        #endregion
    }
}
