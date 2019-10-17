using System;
using System.Collections.Generic;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DTO.Prisma;
using CIC.Database.OL.EF4.Model;

namespace Cic.OpenOne.Common.DAO.Prisma
{
    enum ProvisionDaoCacheIds
    {
        ProvisionAdjLinks,
        ProvisionAdjStep,
        ProvisionShares,
        ProvisionTypes,
        ProvisionPrFlds,
        ExtAblId
    }

    /// <summary>
    /// Cached Provision Data Access Object
    /// </summary>
    public class CachedProvisionDao : ProvisionDao
    {
        private static CacheDictionary<ProvisionDaoCacheIds, object> listCaches = CacheFactory<ProvisionDaoCacheIds, object>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static CacheDictionary<long, ABLTYP> ablCache = CacheFactory<long, ABLTYP>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static CacheDictionary<String, List<Cic.OpenOne.Common.Model.Prisma.PRPROVSTEP>> stepCache = CacheFactory<String, List<Cic.OpenOne.Common.Model.Prisma.PRPROVSTEP>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static CacheDictionary<String, bool> validProvCache = CacheFactory<String, bool>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static CacheDictionary<long, List<Cic.OpenOne.Common.Model.Prisma.PRPROVTYPE>> provTypCache = CacheFactory<long, List<Cic.OpenOne.Common.Model.Prisma.PRPROVTYPE>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static CacheDictionary<String, bool> provPrhGroupNecessary = CacheFactory<String, bool>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);

        /// <summary>
        /// Standarad Constuctor
        /// Database access Object for Prisma Products and Parameters
        /// </summary>
        public CachedProvisionDao()
        {
        }

        private object getCachedData(ProvisionDaoCacheIds cacheid)
        {
            if (!listCaches.ContainsKey(cacheid))
            {
                object val = null;
                switch (cacheid)
                {
                    case (ProvisionDaoCacheIds.ProvisionAdjLinks):
                        val = base.getProvisionAdjustLinks();
                        break;
                    case (ProvisionDaoCacheIds.ProvisionAdjStep):
                        val = base.getProvisionAdjustStep();
                        break;
                    case (ProvisionDaoCacheIds.ProvisionShares):
                        val = base.getProvisionShares();
                        break;
                    case (ProvisionDaoCacheIds.ProvisionTypes):
                        val = base.getProvisionTypes();
                        break;
                    case (ProvisionDaoCacheIds.ProvisionPrFlds):
                        val = base.getProvisionedPrFlds();
                        break;
                    case (ProvisionDaoCacheIds.ExtAblId):
                        val = base.getExternalABlID();
                        break;
                }
                listCaches[cacheid] = val;
            }
            return listCaches[cacheid];
        }

        /// <summary>
        /// returns all Product Parameter Sets linked to Products
        /// </summary>
        /// <returns>Parameter Condition Link List</returns>
        public override List<ProvisionConditionLink> getProvisionConditionLinks()
        {
            return CacheDelegator<CachedProvisionDao>.getInstance().getCached<List<ProvisionConditionLink>>("getProvisionConditionLinks", delegate()
            {
                return base.getProvisionConditionLinks();
            }
            );
        }

        /// <summary>
        /// Returns all configured Provsteps for Incentives (flagnokalk=1)
        /// </summary>
        /// <param name="sysPrHgroup"></param>
        /// <param name="sysPerole"></param>
        /// <param name="perDate"></param>
        /// <param name="sysProvType"></param>
        /// <returns></returns>
        public override List<Cic.OpenOne.Common.Model.Prisma.PRPROVSTEP> getProvstepsInc(long sysPrHgroup, long sysPerole, DateTime perDate, long sysProvType)
        {
            return CacheDelegator<CachedProvisionDao>.getInstance().getCached<List<Cic.OpenOne.Common.Model.Prisma.PRPROVSTEP>>("getProvstepsInc_" + sysPrHgroup + "_" + sysPerole + "_" + perDate.Year + "_" + perDate.Month + "_" + perDate.Day+"_"+sysProvType, delegate()
            {
                return base.getProvstepsInc(sysPrHgroup,sysPerole,perDate,sysProvType);
            }
           );
        }

        /// <summary>
        /// Returns all configured Provsteps as of Prisma concept 5.2.2.2.1
        /// </summary>
        /// <param name="sysPrHgroup"></param>
        /// <param name="sysPerole"></param>
        /// <param name="perDate"></param>
        /// <param name="sysProvType"></param>
        /// <param name="assignType"></param>
        /// <returns></returns>
        public override List<Cic.OpenOne.Common.Model.Prisma.PRPROVSTEP> getProvsteps(long sysPrHgroup, long sysPerole, DateTime perDate, long sysProvType, ProvGroupAssignType assignType)
        {
            String key = sysPrHgroup + "_" + sysPerole + "_" + perDate.Year + "_" + perDate.Month + "_" + perDate.Day + "_" + sysProvType + "_" + assignType;
            if (!stepCache.ContainsKey(key))
            {
                stepCache[key] = base.getProvsteps(sysPrHgroup, sysPerole, perDate, sysProvType, assignType);
            }
            return stepCache[key];
        }

        /// <summary>
        /// returns all Provision Adjustment Links as of Prisma concept 5.2.2.2.3
        /// </summary>
        /// <returns>List of Adjustment links</returns>
        public override List<ProvisionAdjustConditionLink> getProvisionAdjustLinks()
        {
            return (List<ProvisionAdjustConditionLink>)getCachedData(ProvisionDaoCacheIds.ProvisionAdjLinks);
        }

        /// <summary>
        /// returns all Provision Adjustment Steps
        /// </summary>
        /// <returns>List of Adjustment Information</returns>
        public override List<Cic.OpenOne.Common.Model.Prisma.PRPROVADJSTEP> getProvisionAdjustStep()
        {
            return (List<Cic.OpenOne.Common.Model.Prisma.PRPROVADJSTEP>)getCachedData(ProvisionDaoCacheIds.ProvisionAdjStep);
        }

        /// <summary>
        /// returns all Provision Shares as of Prisma concept 5.2.2.2.4
        /// </summary>
        /// <returns>List of Share Information</returns>
        public override List<PROVSHAREDATA> getProvisionShares()
        {
            return (List<PROVSHAREDATA>)getCachedData(ProvisionDaoCacheIds.ProvisionShares);
        }

        /// <summary>
        /// returns all configured Provisiontypes
        /// </summary>
        /// <returns>Parameter Condition List</returns>
        public override List<Cic.OpenOne.Common.Model.Prisma.PRPROVTYPE> getProvisionTypes()
        {
            return (List<Cic.OpenOne.Common.Model.Prisma.PRPROVTYPE>)getCachedData(ProvisionDaoCacheIds.ProvisionTypes);
        }

        /// <summary>
        /// returns all configured Provisiontypes
        /// </summary>
        /// <returns>Parameter Condition List</returns>
        public override List<Cic.OpenOne.Common.Model.Prisma.PRPROVTYPE> getProvisionTypes(long sysprfld)
        {
            if (!provTypCache.ContainsKey(sysprfld))
            {
                provTypCache[sysprfld] = base.getProvisionTypes(sysprfld);
            }
            return provTypCache[sysprfld];
        }

        /// <summary>
        /// Returns a list of all prisma fields that have a provision configured
        /// </summary>
        /// <returns></returns>
        public override List<long> getProvisionedPrFlds()
        {
            return (List<long>)getCachedData(ProvisionDaoCacheIds.ProvisionPrFlds);
        }

        /// <summary>
        /// Returns the ABLTYP
        /// </summary>
        /// <param name="sysabltyp"></param>
        /// <returns></returns>
        public override ABLTYP getAblTyp(long sysabltyp)
        {
            if (!ablCache.ContainsKey(sysabltyp))
            {
                ablCache[sysabltyp] = base.getAblTyp(sysabltyp);
            }
            return ablCache[sysabltyp];
        }

        /// <summary>
        /// determines if the field and type are a valid pair
        /// </summary>
        /// <param name="sysprfld"></param>
        /// <param name="sysprprovtype"></param>
        /// <returns></returns>
        public override bool validProvision(long sysprfld, long sysprprovtype)
        {
            String key = sysprfld + "_" + sysprprovtype;
            if (!validProvCache.ContainsKey(key))
            {
                validProvCache[key] = base.validProvision(sysprfld, sysprprovtype);
            }
            return validProvCache[key];
        }

        /// <summary>
        /// ID der externen Abloese ermitteln
        /// </summary>
        /// <returns>ABLTYPID</returns>
        public override long getExternalABlID()
        {
            return (long)getCachedData(ProvisionDaoCacheIds.ExtAblId);
        }

        /// <summary>
        /// checkPrhGroup
        /// </summary>
        /// <param name="sysperole"></param>
        /// <param name="sysprhgroup"></param>
        /// <returns></returns>
        public override bool checkPrhGroup(long sysperole, long sysprhgroup)
        {
            String key = sysperole + "_" + sysprhgroup;
            if (!provPrhGroupNecessary.ContainsKey(key))
            {
                provPrhGroupNecessary[key] = base.checkPrhGroup(sysperole, sysprhgroup);
            }
            return provPrhGroupNecessary[key];
        }
    }
}