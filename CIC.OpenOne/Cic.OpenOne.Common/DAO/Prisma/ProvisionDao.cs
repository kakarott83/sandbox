using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DTO.Prisma;
using CIC.Database.OL.EF4.Model;

namespace Cic.OpenOne.Common.DAO.Prisma
{
    /// <summary>
    /// ProvGroupAssignType
    /// </summary>
    public enum ProvGroupAssignType
    {
        /// <summary>
        /// PEROLE / Vertriebspartner / Händler
        /// </summary>
        PEROLE = 0,

        /// <summary>
        /// HG / Handelsgruppe
        /// </summary>
        HG = 1,

        /// <summary>
        /// BOTH
        /// </summary>
        BOTH = 2
    }

    /// <summary>
    /// Provision Data Access Object
    /// </summary>
    public class ProvisionDao : IProvisionDao
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const string QUERYGUELTIGKEIT = "(prprovset.validfrom is null or prprovset.validfrom<=   :perDate  or prprovset.validfrom=to_date('01.01.0111' , 'dd.MM.yyyy')) and (prprovset.validuntil is null or prprovset.validuntil>=  :perDate  or prprovset.validuntil=to_date('01.01.0111' , 'dd.MM.yyyy'))" +
                                              " and (PRCLVPPROVSET.validfrom is null or PRCLVPPROVSET.validfrom<=   :perDate or PRCLVPPROVSET.validfrom=to_date('01.01.0111' , 'dd.MM.yyyy')) and (PRCLVPPROVSET.validuntil is null or PRCLVPPROVSET.validuntil>=  :perDate  or PRCLVPPROVSET.validuntil=to_date('01.01.0111' , 'dd.MM.yyyy')) ";
        private const string QUERYPROVSTEPVPINC = "select prprovstep.* from  prclvpprovset, prprovset, prprovtype,prprovstep  where prprovset.flagnokalk=1 and prprovstep.sysprprovtype=prprovtype.sysprprovtype  and prclvpprovset.sysprprovset=prprovset.sysprprovset and prprovstep.sysprprovset=prprovset.sysprprovset and prclvpprovset.sysperole=:sysperole and prclvpprovset.sysprhgroup is null and " + QUERYGUELTIGKEIT + " and prprovset.flagnokalk=1  and prprovtype.sysprprovtype=:sysprprovtype order by prprovstep.rank";
        private const string QUERYPROVSTEPHGINC = "select prprovstep.* from  prclvpprovset, prprovset, prprovtype,prprovstep  where prprovset.flagnokalk=1 and prprovstep.sysprprovtype=prprovtype.sysprprovtype  and prclvpprovset.sysprprovset=prprovset.sysprprovset and prprovstep.sysprprovset=prprovset.sysprprovset and prclvpprovset.sysperole is null and prclvpprovset.sysprhgroup=:sysprhgroup and " + QUERYGUELTIGKEIT + " and prprovset.flagnokalk=1 and prprovtype.sysprprovtype=:sysprprovtype order by prprovstep.rank";
        private const string QUERYPROVSTEPBOTHINC = "select prprovstep.* from  prclvpprovset, prprovset, prprovtype,prprovstep  where prprovset.flagnokalk=1 and prprovstep.sysprprovtype=prprovtype.sysprprovtype  and prclvpprovset.sysprprovset=prprovset.sysprprovset and prprovstep.sysprprovset=prprovset.sysprprovset and prclvpprovset.sysperole=:sysperole and prclvpprovset.sysprhgroup=:sysprhgroup and " + QUERYGUELTIGKEIT + " and prprovset.flagnokalk=1 and prprovtype.sysprprovtype=:sysprprovtype order by prprovstep.rank";


        private const string QUERYPROVSTEPVP = "select prprovstep.* from  prclvpprovset, prprovset, prprovtype,prprovstep  where prprovstep.sysprprovtype=prprovtype.sysprprovtype  and prclvpprovset.sysprprovset=prprovset.sysprprovset and prprovstep.sysprprovset=prprovset.sysprprovset and prclvpprovset.sysperole=:sysperole and prclvpprovset.sysprhgroup is null and "+ QUERYGUELTIGKEIT + " and prprovset.flagnokalk=0 and prprovtype.sysprprovtype=:sysprprovtype order by prprovstep.rank";
        private const string QUERYPROVSTEPHG = "select prprovstep.* from  prclvpprovset, prprovset, prprovtype,prprovstep  where prprovstep.sysprprovtype=prprovtype.sysprprovtype  and prclvpprovset.sysprprovset=prprovset.sysprprovset and prprovstep.sysprprovset=prprovset.sysprprovset and prclvpprovset.sysperole is null and prclvpprovset.sysprhgroup=:sysprhgroup and " + QUERYGUELTIGKEIT + "  and prprovset.flagnokalk=0 and prprovtype.sysprprovtype=:sysprprovtype order by prprovstep.rank";
        private const string QUERYPROVSTEPBOTH = "select prprovstep.* from  prclvpprovset, prprovset, prprovtype,prprovstep  where prprovstep.sysprprovtype=prprovtype.sysprprovtype  and prclvpprovset.sysprprovset=prprovset.sysprprovset and prprovstep.sysprprovset=prprovset.sysprprovset and prclvpprovset.sysperole=:sysperole and prclvpprovset.sysprhgroup=:sysprhgroup and " + QUERYGUELTIGKEIT + "  and prprovset.flagnokalk=0 and prprovtype.sysprprovtype=:sysprprovtype order by prprovstep.rank";

        private const string QUERYPROVTARIF = "select provtarif.*,2 typ,sysprovtarif sysid from provstrct,provtarif,provdate where provdate.sysprovdate=provtarif.sysprovdate and provstrct.sysprovstrct=provdate.sysprovstrct and  provdate.validfrom = (select max(provdate.validfrom) from provdate where provdate.sysprovdate=provtarif.sysprovdate and   (provdate.validfrom is null or provdate.validfrom<= trunc(:perDate)  or provdate.validfrom=to_date('01.01.0111' , 'dd.MM.yyyy')  )) and provstrct.sysprovstrct=:sysprovstrct";
        private const string QUERYPROVRATE = "select provrate.*,0 typ,sysprovrate sysid from provstrct,provrate,provdate where provdate.sysprovdate=provrate.sysprovdate and provstrct.sysprovstrct=provdate.sysprovstrct and  provdate.validfrom = (select max(provdate.validfrom) from provdate where provdate.sysprovdate=provrate.sysprovdate and   (provdate.validfrom is null or provdate.validfrom<= trunc(:perDate)  or provdate.validfrom=to_date('01.01.0111' , 'dd.MM.yyyy')   )) and provstrct.sysprovstrct=:sysprovstrct";
        private const string QUERYPROVPLAN = "select provplan.*,1 typ,sysprovplan sysid from provstrct,provplan,provdate where provdate.sysprovdate=provplan.sysprovdate and provstrct.sysprovstrct=provdate.sysprovstrct and  provdate.validfrom = (select max(provdate.validfrom) from provdate where provdate.sysprovdate=provplan.sysprovdate and   (provdate.validfrom is null or provdate.validfrom<= trunc(:perDate)  or provdate.validfrom=to_date('01.01.0111' , 'dd.MM.yyyy')   )) and provstrct.sysprovstrct=:sysprovstrct order by lowerb, lowerbp";

        private const string QUERYPROVISIONADJLINKS = "select prprovadjstep.*,validfrom,validuntil,sysprproduct  from prclprprovadj,prprovadj,prprovadjstep where activeflag=1 and prclprprovadj.sysprprovadj=prprovadj.sysprprovadj and prprovadjstep.sysprprovadj=prprovadj.sysprprovadj order by rank";
        private const string QUERYPRPROVADJSTEP = "select * from prprovadjstep";
        private const string QUERYPROVSHARE = "select prprovshr.sysprprovshr, prclpeprovshr.sysperole sysvprole,prprovshr.sysperole sysvkrole, method,shrrate,shrval,sysprprovtype,sysprhgroup,prclpeprovshr.validfrom,prclpeprovshr.validuntil,perole.sysparent peroleparent from prclpeprovshr,prprovshr,perole where perole.sysperole=prprovshr.sysperole and prprovshr.sysprclpeprovshr=prclpeprovshr.sysprclpeprovshr  and activflag=1";

        private const string QUERYPRPROVTYPES = "select * from prprovtype where sysprprovtype in (select distinct prprovtype.sysprprovtype from prprovstep,prprovtype,prfld where prprovstep.sysprfld=prfld.sysprfld and prprovtype.sysprprovtype=prprovstep.sysprprovtype and prfld.sysprfld=:sysprfld)";

        private const string QUERYPRPROVTYPE = "select * from prprovtype where activeflag=1 and code is not null";
        private const string QUERYPRFLD = "select distinct sysprfld from prprovstep where sysprfld is not null";
        private const string QUERYABLTYP = "select * from abltyp where sysabltyp=:sysabltyp";

        private const string VALIDPROV = "select count(*) anz from prprovstep,prprovtype,prfld where prprovstep.sysprfld=prfld.sysprfld and prprovtype.sysprprovtype=prprovstep.sysprprovtype and prfld.sysprfld=:sysprfld and prprovtype.sysprprovtype=:sysprprovtype";
        private const string GETABLTYP = "select sysabltyp from abltyp where code = 'EXTERN'";
        private const string GETPRHGROUPANDSYSPEROLE = "SELECT 1 FROM prclvpprovset WHERE prclvpprovset.sysperole =:sysperole AND prclvpprovset.sysprhgroup =:sysprhgroup";

        private const string EIGENABLQUERY = "select verssumme,vart.code VARTCODE from vt,vart where vart.sysvart=vt.sysvart and sysid=:sysvorvt";

        private const string VKPARENT = "select sysparent from perole where sysperole = :sysperole";
        private const string ROLEBYTYPE = "SELECT sysperole FROM perole where sysroletype=:sysroletype CONNECT BY prior perole.sysparent = perole.sysperole start with perole.sysperole=:sysperole";
        //all productparameter links
        private const string QUERYPROVISIONLINKS = "SELECT prclprovset.*,prprovset.validfrom,prprovset.validuntil FROM prclprovset, prprovset WHERE prclprovset.sysprprovset = prprovset.sysprprovset AND prprovset.activeflag = 1  order by case prclprovset.area when 99 then 1 when 0 then 1 when 1 then 2 when 3 then 3 when 2 then 4 else 5 end";
        private const string QUERYPRPROVSET = "select * from prprovset where sysprprovset=:sysprprovset";

        /// <summary>
        /// Standarad Constuctor
        /// Database access Object for Prisma Products and Parameters
        /// </summary>
        public ProvisionDao()
        {
        }

        /// <summary>
        /// returns all Product Parameter Sets linked to Products
        /// </summary>
        /// <returns>Parameter Condition Link List</returns>
        public virtual List<ProvisionConditionLink> getProvisionConditionLinks()
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                return ctx.ExecuteStoreQuery<ProvisionConditionLink>(QUERYPROVISIONLINKS, null).ToList();
            }
        }

        /// <summary>
        /// Returns all configured Provsteps for Incentives (flagnokalk=1)
        /// </summary>
        /// <param name="sysPrHgroup"></param>
        /// <param name="sysPerole"></param>
        /// <param name="perDate"></param>
        /// <param name="sysProvType"></param>
        /// <returns></returns>
        public virtual List<Cic.OpenOne.Common.Model.Prisma.PRPROVSTEP> getProvstepsInc(long sysPrHgroup, long sysPerole, DateTime perDate, long sysProvType)
        {
            long haendlerperole = PeRoleUtil.FindRootPEROLEByRoleType(new DdOlExtended(), sysPerole, (int)RoleTypeTyp.HAENDLER);
            using (PrismaExtended ctx = new PrismaExtended())
            {
                String query = null;

                //Parameters for query
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "perDate", Value = perDate });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprprovtype", Value = sysProvType });

                if (sysPrHgroup == 0 && sysPerole > 0)
                {
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = haendlerperole });
                    query = QUERYPROVSTEPVPINC;
                }
                else if (sysPrHgroup > 0 && sysPerole == 0)
                {
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprhgroup", Value = sysPrHgroup });
                    query = QUERYPROVSTEPHGINC;
                }
                else
                {
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = haendlerperole });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprhgroup", Value = sysPrHgroup });
                    query = QUERYPROVSTEPBOTHINC;
                }
                return ctx.ExecuteStoreQuery<Cic.OpenOne.Common.Model.Prisma.PRPROVSTEP>(query, parameters.ToArray()).ToList();
            }
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
        public virtual List<Cic.OpenOne.Common.Model.Prisma.PRPROVSTEP> getProvsteps(long sysPrHgroup, long sysPerole, DateTime perDate, long sysProvType, ProvGroupAssignType assignType)
        {
            long haendlerperole = PeRoleUtil.FindRootPEROLEByRoleType(new DdOlExtended(), sysPerole, (int)RoleTypeTyp.HAENDLER);
            using (PrismaExtended ctx = new PrismaExtended())
            {
                String query = null;

                //Parameters for query
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "perDate", Value = perDate });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprprovtype", Value = sysProvType });

                if (sysPrHgroup == 0 && sysPerole > 0)
                {
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = haendlerperole });
                    query = QUERYPROVSTEPVP;
                }
                else if (sysPrHgroup > 0 && sysPerole == 0)
                {
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprhgroup", Value = sysPrHgroup });
                    query = QUERYPROVSTEPHG;
                }
                else
                {
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = haendlerperole });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprhgroup", Value = sysPrHgroup });
                    query = QUERYPROVSTEPBOTH;
                }
                return ctx.ExecuteStoreQuery<Cic.OpenOne.Common.Model.Prisma.PRPROVSTEP>(query, parameters.ToArray()).ToList();
            }
        }

        /// <summary>
        /// get all provision structure data entries (either rate, tarif, plan)
        /// as of Prisma concept 5.2.2.2.2
        /// </summary>
        /// <param name="sysprovstrct"></param>
        /// <param name="perDate"></param>
        /// <returns></returns>
        public List<PROVSTRCTDATA> getStrctData(long sysprovstrct, DateTime perDate)
        {
            List<PROVSTRCTDATA> rval = new List<PROVSTRCTDATA>();

            using (PrismaExtended ctx = new PrismaExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "perDate", Value = perDate });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprovstrct", Value = sysprovstrct });

                rval.AddRange(ctx.ExecuteStoreQuery<PROVSTRCTDATA>(QUERYPROVTARIF, parameters.ToArray()).ToList());

                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "perDate", Value = perDate });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprovstrct", Value = sysprovstrct });

                rval.AddRange(ctx.ExecuteStoreQuery<PROVSTRCTDATA>(QUERYPROVRATE, parameters.ToArray()).ToList());

                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "perDate", Value = perDate });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprovstrct", Value = sysprovstrct });

                rval.AddRange(ctx.ExecuteStoreQuery<PROVSTRCTDATA>(QUERYPROVPLAN, parameters.ToArray()).ToList());
            }
            return rval;
        }

        /// <summary>
        /// returns all Provision Adjustment Links as of Prisma concept 5.2.2.2.3
        /// </summary>
        /// <returns>List of Adjustment links</returns>
        public virtual List<ProvisionAdjustConditionLink> getProvisionAdjustLinks()
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                return ctx.ExecuteStoreQuery<ProvisionAdjustConditionLink>(QUERYPROVISIONADJLINKS, null).ToList();
            }
        }

        /// <summary>
        /// returns all Provision Adjustment Steps
        /// </summary>
        /// <returns>List of Adjustment Information</returns>
        public virtual List<Cic.OpenOne.Common.Model.Prisma.PRPROVADJSTEP> getProvisionAdjustStep()
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                return ctx.ExecuteStoreQuery<Cic.OpenOne.Common.Model.Prisma.PRPROVADJSTEP>(QUERYPRPROVADJSTEP, null).ToList();
            }
        }

        /// <summary>
        /// returns all Provision Shares as of Prisma concept 5.2.2.2.4
        /// </summary>
        /// <returns>List of Share Information</returns>
        public virtual List<PROVSHAREDATA> getProvisionShares()
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                return ctx.ExecuteStoreQuery<PROVSHAREDATA>(QUERYPROVSHARE, null).ToList();
            }
        }

        /// <summary>
        /// returns all configured Provisiontypes
        /// </summary>
        /// <returns>Parameter Condition List</returns>
        public virtual List<Cic.OpenOne.Common.Model.Prisma.PRPROVTYPE> getProvisionTypes()
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                return ctx.ExecuteStoreQuery<Cic.OpenOne.Common.Model.Prisma.PRPROVTYPE>(QUERYPRPROVTYPE, null).ToList();
            }
        }

        /// <summary>
        /// returns all configured Provisiontypes
        /// </summary>
        /// <returns>Parameter Condition List</returns>
        public virtual List<Cic.OpenOne.Common.Model.Prisma.PRPROVTYPE> getProvisionTypes(long sysprfld)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprfld", Value = sysprfld });

                return ctx.ExecuteStoreQuery<Cic.OpenOne.Common.Model.Prisma.PRPROVTYPE>(QUERYPRPROVTYPES, parameters.ToArray()).ToList();
            }
        }

        /// <summary>
        /// Returns a list of all prisma fields that have a provision configured
        /// </summary>
        /// <returns></returns>
        public virtual List<long> getProvisionedPrFlds()
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                return ctx.ExecuteStoreQuery<long>(QUERYPRFLD, null).ToList();
            }
        }

        /// <summary>
        /// Returns the ABLTYP
        /// </summary>
        /// <param name="sysabltyp"></param>
        /// <returns></returns>
        public virtual CIC.Database.OL.EF4.Model.ABLTYP getAblTyp(long sysabltyp)
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysabltyp", Value = sysabltyp });

                return ctx.ExecuteStoreQuery<ABLTYP>(QUERYABLTYP, parameters.ToArray()).FirstOrDefault();
            }
        }

        /// <summary>
        /// determines if the field and type are a valid pair
        /// </summary>
        /// <param name="sysprfld"></param>
        /// <param name="sysprprovtype"></param>
        /// <returns></returns>
        public virtual bool validProvision(long sysprfld, long sysprprovtype)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprfld", Value = sysprfld });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprprovtype", Value = sysprprovtype });

                long cnt = ctx.ExecuteStoreQuery<long>(VALIDPROV, parameters.ToArray()).FirstOrDefault();
                return cnt > 0;
            }
        }

        /// <summary>
        /// ID der externen Abloese ermitteln
        /// </summary>
        /// <returns>ABLTYPID</returns>
        public virtual long getExternalABlID()
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                return ctx.ExecuteStoreQuery<long>(GETABLTYP, null).FirstOrDefault();
            }
        }

        /// <summary>
        /// checkPrhGroup
        /// </summary>
        /// <param name="sysperole"></param>
        /// <param name="sysprhgroup"></param>
        /// <returns></returns>
        public virtual bool checkPrhGroup(long sysperole, long sysprhgroup)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = sysperole });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprhgroup", Value = sysprhgroup });

                var query = ctx.ExecuteStoreQuery<long>(GETPRHGROUPANDSYSPEROLE, parameters.ToArray()).ToList();
                if (query.Count == 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// get the Eigenabloeseinformation
        /// </summary>
        /// <param name="sysvorvt"></param>
        /// <returns></returns>
        public virtual EigenAblInfo getEigenabloeseInfo(long sysvorvt)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvorvt", Value = sysvorvt });

                return ctx.ExecuteStoreQuery<EigenAblInfo>(EIGENABLQUERY, parameters.ToArray()).FirstOrDefault();
            }
        }

        public virtual long getVkparent(long sysperole)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = sysperole });

                return ctx.ExecuteStoreQuery<long>(VKPARENT, parameters.ToArray()).FirstOrDefault();
            }
        }

        /// <summary>
        /// gets the sysperole of given type starting from the leaf sysperole
        /// </summary>
        /// <param name="sysperole"></param>
        /// <param name="sysroletype"></param>
        /// <returns></returns>
        public virtual long getRoleByType(long sysperole, long sysroletype)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = sysperole });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysroletype", Value = sysroletype });

                return ctx.ExecuteStoreQuery<long>(ROLEBYTYPE, parameters.ToArray()).FirstOrDefault();
            }

        }

        /// <summary>
        /// returns the provisionplan (PRPROVSET)
        /// </summary>
        /// <param name="sysprprovset"></param>
        /// <returns></returns>
        public virtual PRPROVSET getPrprovset(long sysprprovset)
        {
           
            using (PrismaExtended ctx = new PrismaExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprprovset", Value = sysprprovset });

                return ctx.ExecuteStoreQuery<PRPROVSET>(QUERYPRPROVSET, parameters.ToArray()).FirstOrDefault();
            }
        }
    }
}