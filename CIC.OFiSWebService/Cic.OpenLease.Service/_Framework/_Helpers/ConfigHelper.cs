namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenOne.Common.Model.DdOiqueue;
    using CIC.Database.OIQUEUE.EF6.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    #endregion

    public static class ConfigHelper
    {
        #region Methods
        public static List<CFGVAR> GetConfigEntries(string cfgCode, string cfgSecCode)
        {
            try
            {
                // Create the entities
                using (DdOiQueueExtended Entities = new DdOiQueueExtended())
                {
                    // Query CFG
                    var Config = (from CfgEntry in Entities.CFG
                                  where CfgEntry.CODE.ToUpper() == cfgCode.ToUpper()
                                  orderby CfgEntry.SYSCFG descending
                                  select CfgEntry).FirstOrDefault();

                    // Check if nothing was found
                    if (Config == null)
                    {
                        // Throw an exception
                        throw new ApplicationException("Code " + cfgCode + " could not be found in th table CFG.");
                    }

                    // Query CFGSEC
                    var ConfigSec = (from CfgEntrySec in Entities.CFGSEC
                                     where CfgEntrySec.CFG.SYSCFG == Config.SYSCFG
                                     && CfgEntrySec.CODE.ToUpper() == cfgSecCode.ToUpper()
                                     orderby CfgEntrySec.SYSCFGSEC
                                     select CfgEntrySec).FirstOrDefault();

                    // Check if nothing was found
                    if (ConfigSec == null)
                    {
                        // Throw an exception
                        throw new ApplicationException("Code " + cfgSecCode + " could not be found in the table CFGSEC.");
                    }

                    // Query CFGVAR
                    var ConfigVars = from CfgEntryVar in Entities.CFGVAR
                                     where CfgEntryVar.CFGSEC.SYSCFGSEC == ConfigSec.SYSCFGSEC
                                     select CfgEntryVar;

                    // Return the list
                    return ConfigVars.ToList();
                }
            }
            catch (Exception e)
            {
                // Throw an exception
                throw new ApplicationException("Could not get the configuration entries list.", e);
            }
        }
        #endregion
    }
}