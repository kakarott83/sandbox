using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;

namespace Cic.One.Web.DAO
{
    enum DictionaryCacheIds
    {
        FremdBanken,
        Insurances,
        Land,
        Nationalities,
        Staat,
        Branche,
        CTLANG,
        CTLANG_PRINT,
        PrioHaendler,
    }

    public class CachedDictionaryListsDao : IDictionaryListsDao// CRMDictionaryListsDao
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static CacheDictionary<DictionaryCacheIds, object> listCaches = CacheFactory<DictionaryCacheIds, object>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);
        private static bool cached = false;
        private IDictionaryListsDao baseDao;

        public CachedDictionaryListsDao(IDictionaryListsDao baseDao)
        {
            this.baseDao = baseDao;
            if (!cached)//cache-warmup
            {
                this.deliverBranche();
                this.deliverCTLANG();
                this.deliverCTLANG_PRINT();
                this.deliverInsurance();
                this.deliverLAND();
                this.deliverNATIONALITIES();
                this.deliverSTAAT();
                this.listFremdBanken();

                foreach (DDLKPPOSType code in Enum.GetValues(typeof(DDLKPPOSType)))
                {
                    if(code==DDLKPPOSType.FOLLOW_TYPE)
                        this.findByDDLKPPOSCode(code, null,null,true);
                    else
                        this.findByDDLKPPOSCode(code, null);
                }
                cached = true;
            }
        }

        private object getCachedData(DictionaryCacheIds cacheid)
        {
            if (!listCaches.ContainsKey(cacheid))
            {
                object val = null;
                switch (cacheid)
                {
                    case (DictionaryCacheIds.FremdBanken):
                        val = baseDao.listFremdBanken();
                        break;
                    case (DictionaryCacheIds.Insurances):
                        val = baseDao.deliverInsurance();
                        break;
                    case (DictionaryCacheIds.Land):
                        val = baseDao.deliverLAND();
                        break;
                    case (DictionaryCacheIds.Nationalities):
                        val = baseDao.deliverNATIONALITIES();
                        break;
                    case (DictionaryCacheIds.Staat):
                        val = baseDao.deliverSTAAT();
                        break;
                    case (DictionaryCacheIds.Branche):
                        val = baseDao.deliverBranche();
                        break;
                    case (DictionaryCacheIds.CTLANG):
                        val = baseDao.deliverCTLANG();
                        break;
                    case (DictionaryCacheIds.CTLANG_PRINT):
                        val = baseDao.deliverCTLANG_PRINT();
                        break;
                }
                listCaches[cacheid] = val;
            }
            return listCaches[cacheid];
        }

        public DropListDto[] deliverAufbau(string isoCode)
        {
            return CacheDelegator<CachedDictionaryListsDao>.getInstance().getCachedCloned<DropListDto[]>("Aufbau"+isoCode, delegate()
                {
                    return baseDao.deliverAufbau(isoCode);
                }
            );
        }

        public DropListDto[] deliverBRAND(string isoCode, int obart)
        {
            return CacheDelegator<CachedDictionaryListsDao>.getInstance().getCachedCloned<DropListDto[]>("Brand" + isoCode+"_"+obart, delegate()
            {
                return baseDao.deliverBRAND(isoCode, obart);
            }
            );
        }
        public DropListDto[] deliverGetriebeart(string isoCode)
        {
            return CacheDelegator<CachedDictionaryListsDao>.getInstance().getCachedCloned<DropListDto[]>("Getriebeart" + isoCode, delegate()
            {
                return baseDao.deliverGetriebeart(isoCode);
            }
            );
        }
		public DropListDto[] deliverSlaPause (string isoCode)
		{
			return CacheDelegator<CachedDictionaryListsDao>.getInstance ().getCachedCloned<DropListDto[]> ("SlaPause" + isoCode, delegate ()
			{
				return baseDao.deliverSlaPause (isoCode);
			}
			);
		}
		public DropListDto[] deliverTreibstoffart (string isoCode)
        {
            return CacheDelegator<CachedDictionaryListsDao>.getInstance().getCachedCloned<DropListDto[]>("Treibstoffart" + isoCode, delegate()
            {
                return baseDao.deliverTreibstoffart(isoCode);
            }
            );
        }
        public DropListDto[] deliverISO3LAND()
        {
            return CacheDelegator<CachedDictionaryListsDao>.getInstance().getCachedCloned<DropListDto[]>("ISOLand" , delegate()
            {
                return baseDao.deliverISO3LAND();
            }
            );
        }

        public DropListDto[] findByDDLKPPOSCode(DDLKPPOSType code, string isoCode, string domainId)
        {
            return CacheDelegator<CachedDictionaryListsDao>.getInstance().getCachedCloned<DropListDto[]>("DDLKPPOSCode" + code + "_" + isoCode + "_" + domainId + "_" + false, delegate()
            {
                return baseDao.findByDDLKPPOSCode(code, isoCode, domainId);
            }
            );
        }
        public DropListDto[] findByDDLKPPOSCode(DDLKPPOSType code, string isoCode)
        {
            return CacheDelegator<CachedDictionaryListsDao>.getInstance().getCachedCloned<DropListDto[]>("DDLKPPOSCode" + code + "_" + isoCode + "_" + null + "_" + false, delegate()
            {
                return baseDao.findByDDLKPPOSCode(code, isoCode);
            }
            );
        }
        public DropListDto[] findByDDLKPPOSCode(DDLKPPOSType code, string isoCode, string domainId, bool stringid)
        {
            return CacheDelegator<CachedDictionaryListsDao>.getInstance().getCachedCloned<DropListDto[]>("DDLKPPOSCode" + code+"_"+isoCode+"_"+domainId+"_"+stringid, delegate()
            {
                return baseDao.findByDDLKPPOSCode(code,isoCode,domainId,stringid);
            }
            );
        }
        public DropListDto[] findByDDLKPPOSCode(String code, string isoCode, string domainId, bool stringid)
        {
            return CacheDelegator<CachedDictionaryListsDao>.getInstance().getCachedCloned<DropListDto[]>("DDLKPPOSCode" + code + "_" + isoCode + "_" + domainId + "_" + stringid, delegate()
            {
                return baseDao.findByDDLKPPOSCode(code, isoCode, domainId, stringid);
            }
            );
        }

        public FremdbankDto[] listFremdBanken()
        {
            return (FremdbankDto[]) ((FremdbankDto[])getCachedData(DictionaryCacheIds.FremdBanken)).Clone();
        }
        public InsuranceDto[] deliverInsurance()
        {
            return (InsuranceDto[])((InsuranceDto[])getCachedData(DictionaryCacheIds.Insurances)).Clone();
        }
        public DropListDto[] deliverLAND()
        {
            return (DropListDto[])((DropListDto[])getCachedData(DictionaryCacheIds.Land)).Clone();
        }
        public DropListDto[] deliverNATIONALITIES()
        {
            return (DropListDto[])((DropListDto[])getCachedData(DictionaryCacheIds.Nationalities)).Clone();
        }
        public DropListDto[] deliverSTAAT()
        {
            return (DropListDto[])((DropListDto[])getCachedData(DictionaryCacheIds.Staat)).Clone();
        }
        public DropListDto[] deliverBranche()
        {
            return (DropListDto[])((DropListDto[])getCachedData(DictionaryCacheIds.Branche)).Clone();
        }
        public DropListDto[] deliverCTLANG()
        {
            return (DropListDto[])((DropListDto[])getCachedData(DictionaryCacheIds.CTLANG)).Clone();
        }
        public DropListDto[] deliverCTLANG_PRINT()
        {
            return (DropListDto[])((DropListDto[])getCachedData(DictionaryCacheIds.CTLANG_PRINT)).Clone();
        }

        public DdlkpsposDto[] createOrUpdateDdlkpspos(DdlkpsposDto[] ddlkpspos)
        {
            throw new NotImplementedException();
        }

        public DdlkpcolDto getDdlkpcolDetails(long sysddlkpcol)
        {
            throw new NotImplementedException();
        }

        public List<DdlkpcolDto> getDdlkpcols(long sysddlkprub)
        {
            throw new NotImplementedException();
        }

        public List<DdlkpposDto> getDdlkppos(long sysddlkpcol)
        {
            throw new NotImplementedException();
        }

        public DdlkpposDto getDdlkpposDetails(long sysDdlkppos)
        {
            throw new NotImplementedException();
        }

        public DdlkprubDto getDdlkprubDetails(long sysDdlkprub)
        {
            throw new NotImplementedException();
        }

        public List<DdlkprubDto> getDdlkprubs(string area)
        {
            throw new NotImplementedException();
        }

        public List<DdlkpsposDto> getDdlkpspos(string area, long areaid)
        {
            throw new NotImplementedException();
        }

        public DdlkpsposDto getDdlkpsposDetails(long sysddlkpspos)
        {
            throw new NotImplementedException();
        }

        public int getPrioHaendler(long syshaendler)
        {
            return baseDao.getPrioHaendler(syshaendler);
        }

    }
}