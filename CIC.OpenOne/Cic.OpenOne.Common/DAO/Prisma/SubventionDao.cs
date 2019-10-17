using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;

namespace Cic.OpenOne.Common.DAO.Prisma
{
    /// <summary>
    /// Type of Subvention
    /// </summary>
    public enum SubventionType
    {
        /// <summary>
        /// Explizit
        /// </summary>
        EXPLICIT = 1,
        /// <summary>
        /// Implizit
        /// </summary>
        IMPLICIT = 2

    }

    /// <summary>
    /// Defines the kind of subvention to calc
    /// </summary>
    public enum SubventionCalcMode
    {
        /// <summary>
        /// Explizit
        /// </summary>
        EXPLICIT,
        /// <summary>
        /// Implizit
        /// </summary>
        IMPLICIT,
        /// <summary>
        /// Explizit und Implizit
        /// </summary>
        BOTH
    }

    /// <summary>
    /// the area the subvention is assigned to
    /// </summary>
    public enum ExplicitSubventionArea
    {
        /// <summary>
        /// Interest
        /// </summary>
        INTEREST,
        /// <summary>
        /// Charge
        /// </summary>
        CHARGE,
        /// <summary>
        /// Service
        /// </summary>
        SERVICE,
        /// <summary>
        /// Insureance
        /// </summary>
        INSURANCE
    }

    /// <summary>
    /// Subvention Data Access Object
    /// </summary>
    public class SubventionDao : ISubventionDao
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const string QUERYGEBUEHR = "select prsubv.sysprsubv sysprsubv from prsubvtrggeb,gebuehr,prsubv where area=3 and prsubv.sysprsubv=prsubvtrggeb.sysprsubv and prsubvtrggeb.sysgebuehr=gebuehr.sysgebuehr and gebuehr.sysgebuehr=:areaid";
        private const string QUERYFSTYP = "select prsubv.sysprsubv sysprsubv from prsubvtrgfs,fstyp,prsubv where area=4 and  prsubv.sysprsubv=prsubvtrgfs.sysprsubv and prsubvtrgfs.sysfstyp=fstyp.sysfstyp and fstyp.sysfstyp=:areaid";
        private const string QUERYVSTYP = "select prsubv.sysprsubv sysprsubv from prsubvtrgvs,vstyp,prsubv where  area=5 and prsubv.sysprsubv=prsubvtrgvs.sysprsubv and prsubvtrgvs.sysvstyp=vstyp.sysvstyp and vstyp.sysvstyp=:areaid";
        private const string QUERYINT = "select prsubv.sysprsubv sysprsubv from prsubvtrgint,printstep,prsubv where  area=1 and prsubv.sysprsubv=prsubvtrgint.sysprsubv and prsubvtrgint.sysprintstep=printstep.sysprintstep and printstep.sysprintstep=:areaid";


        private const string QUERYTRIGGERINFO = "SELECT prproduct.sysprproduct, PRSUBV.trgtype,PRSUBV.SYSPRFLDTRG,PRSUBV.SYSPRSUBV      FROM prproduct,    prclprsubvset,    prsubvset,    prsubv,     prfld  WHERE prproduct.sysprproduct   =prclprsubvset.sysprproduct  AND prclprsubvset.sysprsubvset =prsubvset.sysprsubvset  AND prsubv.sysprsubvset        =prsubvset.sysprsubvset  AND prproduct.ActiveFlag       = 1  and prsubv.SYSPRFLDTRG=prfld.sysprfld   AND (prsubvset.validfrom      IS NULL  OR prsubvset.validfrom        <= :perDate or prsubvset.validfrom=to_date('01.01.0111' , 'dd.MM.yyyy'))  AND (prsubvset.validuntil     IS NULL  OR prsubvset.validuntil       >= :perDate  or prsubvset.validuntil=to_date('01.01.0111' , 'dd.MM.yyyy'))  AND prsubvset.aktivflag        =1  order by SYSPRPRODUCT,PRSUBV.RANK";

        /// <summary>
        /// Standarad Constuctor
        /// Database access Object for Subventions
        /// </summary>
        public SubventionDao()
        {

        }


        /// <summary>
        /// Delivers all subvention trigger infos as of Prisma Concept 5.6.2.1
        /// </summary>
        /// <param name="perDate"></param>
        /// <returns></returns>
        virtual public List<PrSubvTriggerDto> getSubventionTriggers(DateTime perDate)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "perDate", Value = perDate });
                return ctx.ExecuteStoreQuery<PrSubvTriggerDto>(QUERYTRIGGERINFO, parameters.ToArray()).ToList();

            }
        }

        /// <summary>
        /// returns all Subventions
        /// </summary>
        /// <returns></returns>
        virtual public List<PRSUBV> getSubventions()
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                List<PRSUBV> rval = ctx.PRSUBV.ToList();
                foreach (PRSUBV sp in rval)
                {
                    ctx.Detach(sp);
                }
                return rval;
            }
        }

        /// <summary>
        /// returns all Subvention Positions
        /// </summary>
        /// <returns></returns>
        virtual public List<PRSUBVPOS> getSubventionPositions(long sysprsubv)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                List<PRSUBVPOS> poslist = ctx.PRSUBVPOS.Where(t => t.SYSPRSUBV == sysprsubv).ToList();
                var poslistquery = from p in poslist
                                   group p by p.SYSPERSON into g
                                   from p2 in g
                                   where p2.FROMSUBVVAL == g.Max(p3 => p3.FROMSUBVVAL)
                                   select p2;
                List<PRSUBVPOS> poslistfilterd = poslistquery.ToList<PRSUBVPOS>();
                foreach (PRSUBVPOS sp in poslistfilterd)
                {
                    ctx.Detach(sp);
                }

                return poslistfilterd;
            }
        }

        /// <summary>
        /// returns all Subvention Positions
        /// </summary>
        /// <returns></returns>
        virtual public List<PRSUBVPOS> getSubventionPositions(long sysprsubv, double betrag)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {

                List<PRSUBVPOS> poslist = ctx.PRSUBVPOS.Where(t => t.SYSPRSUBV == sysprsubv).Where(t => t.FROMSUBVVAL <= betrag).ToList();
                var poslistquery = from p in poslist
                                   group p by p.SYSPERSON into g
                                   from p2 in g
                                   where p2.FROMSUBVVAL == g.Max(p3 => p3.FROMSUBVVAL)
                                   select p2;
                List<PRSUBVPOS> poslistfilterd = poslistquery.ToList<PRSUBVPOS>();
                foreach (PRSUBVPOS sp in poslistfilterd)
                {
                    ctx.Detach(sp);
                }
                return poslistfilterd;
            }
        }

        /// <summary>
        /// returns all excplicit subventions ids for the given area and id  as of Prisma Concept 5.6.2.2
        /// </summary>
        /// <param name="area"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        virtual public List<long> getExplicitSubventionIds(ExplicitSubventionArea area, long areaId)
        {
            List<long> ids = null;
            object[] pars = { new Devart.Data.Oracle.OracleParameter { ParameterName = "areaid", Value = areaId } };

            using (PrismaExtended ctx = new PrismaExtended())
            {
                switch (area)
                {
                    case ExplicitSubventionArea.CHARGE:
                        ids = ctx.ExecuteStoreQuery<long>(QUERYGEBUEHR, pars).ToList();
                        break;
                    case ExplicitSubventionArea.INSURANCE:
                        ids = ctx.ExecuteStoreQuery<long>(QUERYVSTYP, pars).ToList();
                        break;
                    case ExplicitSubventionArea.INTEREST:
                        ids = ctx.ExecuteStoreQuery<long>(QUERYINT, pars).ToList();
                        break;
                    case ExplicitSubventionArea.SERVICE:
                        ids = ctx.ExecuteStoreQuery<long>(QUERYFSTYP, pars).ToList();
                        break;
                }
            }
            return ids;

        }

    }
}
