using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.One.Web.DAO;
using Cic.One.DTO;
using Cic.OpenOne.Common.DTO;
using Cic.One.Web.BO.Search;
using System.Reflection;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.DTO.Prisma;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.BO.Prisma;
using System.Text;
using Cic.OpenOne.Common.Util;


namespace Cic.One.GateBANKNOW.BO.Search
{
    /// <summary>
    /// Lookup-List Datasource for BN
    /// </summary>
    public class XproInfoFactory : Cic.One.Web.BO.Search.XproInfoFactory
    {
        private static volatile XproInfoFactory _self;
        private static readonly object InstanceLocker = new Object();
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static CacheDictionary<long, Dictionary<int, List<long>>> obtypSearchFilter = CacheFactory<long, Dictionary<int, List<long>>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);

        new public static XproInfoFactory getInstance()
        {
            if (_self == null)
            {
                lock (InstanceLocker)
                {
                    if (_self == null)
                        _self = new XproInfoFactory();
                }
            }
            return _self;
        }


        protected override DictionaryListsBo CreateDictionaryListsBo(String isoCode)
        {
            return new DictionaryListsBo(DAOFactoryFactory.getInstance().getDictionaryListsDao(), new CachedTranslateDao(), isoCode);
        }

        /// <summary>
        /// registers all needed lookup lists
        /// </summary>
        protected override void registerProviders()
        {
            //register defaults, can be overriden later
            base.registerProviders();

            dictionaryCode.Add("ABWICKLUNGSORT", new XproInfoDao<XproEntityDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select perole.sysperole sysid,perole.name code,perole.name title, perole.name desc1 from perole where sysperole=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();

                        //par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = input.sysperole });
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value = "%"+input.Filter+"%" });
                        //return ctx.ExecuteStoreQuery<XproEntityDto>("select perole.sysperole sysid,perole.name code,perole.name title, perole.name desc1 from perole,roletype  where UPPER(perole.name) like UPPER(:filter) and perole.sysroletype=roletype.sysroletype and roletype.typ=6 and perole.sysperole in (select syschild from perolecache where sysparent=:sysperole) connect by prior perole.sysperole=sysparent order by code", par.ToArray()).ToArray();
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select perole.sysperole sysid,perole.name code,perole.name title, perole.name desc1 from perole,roletype  where UPPER(perole.name) like UPPER(:filter) and perole.sysroletype=roletype.sysroletype and roletype.typ=2 order by code", par.ToArray()).ToArray();
                    }
                }
            });
            //Abwicklungsort änderung, filialliste mit gültigkeit aktuell
            dictionaryCode.Add("ABWICKLUNGSORTCHG", new XproInfoDao<XproEntityDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select perole.sysperole sysid,perole.name code,perole.name title, perole.name desc1 from perole where sysperole=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();

                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value = "%" + input.Filter + "%" });
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select perole.sysperole sysid,perole.name code,perole.name title, perole.name desc1 from perole,roletype  where UPPER(perole.name) like UPPER(:filter) and perole.sysroletype=roletype.sysroletype and roletype.typ=2 AND perole.INACTIVEFLAG = 0 AND (perole.VALIDFROM is null OR trunc(perole.VALIDFROM) <= trunc(sysdate)) AND (perole.VALIDUNTIL is null OR trunc(perole.VALIDUNTIL) >= trunc(sysdate)) order by code", par.ToArray()).ToArray();
                    }
                }
            });
            //Abwicklungsort änderung, mitarbeiterliste filiale, übergabe p1 = sysperole filiale
            dictionaryCode.Add("ABWICKLUNGSORTMA", new XproInfoDao<XproEntityDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = id });
                        return ctx.ExecuteStoreQuery<XproEntityDto>("SELECT wfuser.syswfuser sysid,perole.name code,perole.name title, perole.name desc1 FROM wfuser,perole where WFUSER.SYSPERSON=PEROLE.SYSPERSON and wfuser.syswfuser=:p1", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        /*List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        if (input.filters.Length > 0 && input.filters[0].value.Length > 0)
                        {
                            par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = input.filters[0].value });
                        }
                        else
                        {
                            par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = "0" });
                        }
                        */
                        
                        //return ctx.ExecuteStoreQuery<XproEntityDto>("SELECT perole.sysperole sysid,perole.name code,perole.name title, perole.name desc1 FROM PERELATE,PEROLE,WFUSER WHERE PERELATE.SYSPEROLE2 = PEROLE.SYSPEROLE AND PERELATE.SYSPEROLE1 = :p1 AND PEROLE.SYSROLETYPE = 8 AND PERELATE.FLAGDEFAULT = 1 AND WFUSER.SYSPERSON = PEROLE.SYSPERSON AND (PERELATE.RELBEGINNDATE is null OR trunc(PERELATE.RELBEGINNDATE) <= trunc(sysdate)) AND (PERELATE.RELENDDATE is null OR trunc(PERELATE.RELENDDATE) >= trunc(sysdate)) order by perole.name", par.ToArray()).ToArray();
                        String query = @"SELECT wfuser.syswfuser sysid,perole.name code,perole.name title, perole.name desc1
                                    FROM CIC.PEROLE PEROLE, CIC.ROLETYPE ROLETYPE, WFUSER
                                   WHERE WFUSER.SYSPERSON=PEROLE.SYSPERSON and PEROLE.SYSROLETYPE = ROLETYPE.SYSROLETYPE (+)
                                     AND sysperole IN(SELECT perole.sysperole
                                           FROM cfg, cfgsec, cfgvar, roletype, perole
                                          WHERE perole.sysroletype = roletype.sysroletype AND cfg.syscfg = cfgsec.syscfg
                                     AND cfgsec.syscfgsec = cfgvar.syscfgsec AND cfgvar.code = roletype.code
                                     AND DECODE(cfgvar.wert , 'INTERN', 1
                                                    , 0) = NVL(roletype.flagintern, 0)
                                     AND cfg.code = 'VERTRIEBSBAUM INSERT LOGIK' AND cfgsec.code = 'FILIALE')
                                     AND perole.inactiveflag = 0
                                ORDER BY PEROLE.NAME";
                        return ctx.ExecuteStoreQuery<XproEntityDto>(query, null).ToArray();
                    }
                }
            });

            //WFUSER
            dictionaryCode.Add("BERATER", new XproInfoDao<XproEntityDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select wfuser.syswfuser sysid, wfuser.syswfuser code, wfuser.vorname||' '||wfuser.name title from  wfuser where wfuser.syswfuser=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();

                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = input.sysperole });
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value = "%" + input.Filter + "%" });
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select wfuser.syswfuser sysid, wfuser.syswfuser code, wfuser.vorname||' '||wfuser.name title from perole,wfuser where UPPER(wfuser.name) like UPPER(:filter) and perole.sysperson=wfuser.sysperson and perole.sysparent=(select sysparent from perole where sysperole=:sysperole) order by wfuser.name", par.ToArray()).ToArray();
                    }
                }
            });
            //Wie berater nur sysperson
            dictionaryCode.Add("BEARBEITER", new XproInfoDao<XproEntityDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select person.sysperson sysid, person.sysperson code, person.vorname||' '||person.name title from person where person.sysperson=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();

                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = input.sysperole });
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value = "%" + input.Filter + "%" });
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select person.sysperson sysid, person.sysperson code, person.vorname||' '||person.name title from perole,person where UPPER(person.name) like UPPER(:filter) and perole.sysperson=person.sysperson and perole.sysparent=(select sysparent from perole where sysperole=:sysperole) order by person.name", par.ToArray()).ToArray();
                    }
                }
            });
            dictionaryCode.Add("BERATERBYPERSON", new XproInfoDao<XproEntityDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select wfuser.syswfuser sysid, wfuser.syswfuser code, wfuser.vorname||' '||wfuser.name title from  wfuser where wfuser.sysperson=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();

                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = input.sysperole });
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value = "%" + input.Filter + "%" });
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select wfuser.syswfuser sysid, wfuser.syswfuser code, wfuser.vorname||' '||wfuser.name title from perole,wfuser where UPPER(wfuser.name) like UPPER(:filter) and perole.sysperson=wfuser.sysperson and perole.sysparent=(select sysparent from perole where sysperole=:sysperole) order by wfuser.name", par.ToArray()).ToArray();
                    }
                }
            });
            dictionaryCode.Add("NUTZUNGSART", new XproInfoDao<DropListDto>()
            {
                QueryItemFunction2 = (input) =>
                {


                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        DropListDto[] dl = new Cic.OpenOne.GateBANKNOW.Common.BO.RoleContextListsBo(new Cic.OpenOne.GateBANKNOW.Common.DAO.RoleContextListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao()).listAvailableNutzungsarten(input.isoCode);
                        return (from d in dl
                                where d.sysID == input.EntityId
                                select d).FirstOrDefault();
                    }
                },

                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended olCtx = new Cic.OpenOne.Common.Model.DdOl.DdOlExtended())
                    {
                        return new Cic.OpenOne.GateBANKNOW.Common.BO.RoleContextListsBo(new Cic.OpenOne.GateBANKNOW.Common.DAO.RoleContextListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao()).listAvailableNutzungsarten(input.isoCode);
                    }
                }
            });

            dictionary.Remove(XproEntityType.ATTRIBUT);
            dictionary.Add(XproEntityType.ATTRIBUT, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction2 = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = input.isoCode });
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = input.EntityId });
                        String query = @"select sysattributdef sysid,code, attribut bezeichnung from
                                            (
                                              select distinct attributdef.sysattributdef, attributdef.attribut, attributdef.beschreibung, attributdef.attribut  code
                                              from attribut, attributdef, state,ctlang   
                                              where state.sysstate = attribut.sysstate and attribut.sysattributdef = attributdef.sysattributdef and state.syswftable = 5 and state.flagaktiv = 1  
                                               and ctlang.isocode=:isocode 
                                              and attributdef.sysattributdef not in (select sysattributdef from cttattributdef where sysctlang = ctlang.sysctlang)  
                                              union  
                                              select cttattributdef.SYSATTRIBUTDEF, cttattributdef.replaceattribut, cttattributdef.replacebeschreibung, attributdef.attribut code
                                              from cttattributdef, attributdef, ctlang
                                              where cttattributdef.sysattributdef = attributdef.sysattributdef  
                                              and attributdef.sysattributdef in (select attribut.sysattributdef from attribut, state where state.sysstate = attribut.sysstate and state.syswftable = 5 and state.flagaktiv = 1) 
                                               and ctlang.isocode=:isocode
                                              and cttattributdef.sysctlang = ctlang.sysctlang
                                            ) 
                                            where sysid=:id";
                        return ctx.ExecuteStoreQuery<DropListDto>(query, par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = input.isoCode });
                        String query = @"select sysattributdef sysid,code, attribut bezeichnung from
                                            (
                                              select distinct attributdef.sysattributdef, attributdef.attribut, attributdef.beschreibung, attributdef.attribut  code
                                              from attribut, attributdef, state,ctlang   
                                              where state.sysstate = attribut.sysstate and attribut.sysattributdef = attributdef.sysattributdef and state.syswftable = 5 and state.flagaktiv = 1  
                                               and ctlang.isocode=:isocode 
                                              and attributdef.sysattributdef not in (select sysattributdef from cttattributdef where sysctlang = ctlang.sysctlang)  
                                              union  
                                              select cttattributdef.SYSATTRIBUTDEF, cttattributdef.replaceattribut, cttattributdef.replacebeschreibung, attributdef.attribut code
                                              from cttattributdef, attributdef, ctlang
                                              where cttattributdef.sysattributdef = attributdef.sysattributdef  
                                              and attributdef.sysattributdef in (select attribut.sysattributdef from attribut, state where state.sysstate = attribut.sysstate and state.syswftable = 5 and state.flagaktiv = 1) 
                                               and ctlang.isocode=:isocode
                                              and cttattributdef.sysctlang = ctlang.sysctlang
                                            ) 
                                            order by upper(bezeichnung)";
                        return ctx.ExecuteStoreQuery<DropListDto>(query, par.ToArray()).ToArray();
                    }
                }
            });

            dictionary.Add(XproEntityType.ANGANTATTRIBUT, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction2 = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = input.isoCode });
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = input.EntityId });
                        String query = @"select sysattributdef sysid,code, attribut bezeichnung from
                                            (
                                              select distinct attributdef.sysattributdef, attributdef.attribut, attributdef.beschreibung, attributdef.attribut  code
                                              from attribut, attributdef, state,ctlang   
                                              where state.sysstate = attribut.sysstate and attribut.sysattributdef = attributdef.sysattributdef and state.syswftable in (117,122) and state.flagaktiv = 1  
                                               and ctlang.isocode=:isocode 
                                               AND attributdef.attribut != 'Auszahlungspendenz/Payments'
                                              and attributdef.sysattributdef not in (select sysattributdef from cttattributdef where sysctlang = ctlang.sysctlang)  
                                              union  
                                              select cttattributdef.SYSATTRIBUTDEF, cttattributdef.replaceattribut, cttattributdef.replacebeschreibung, attributdef.attribut code
                                              from cttattributdef, attributdef, ctlang
                                              where cttattributdef.sysattributdef = attributdef.sysattributdef  
                                              AND attributdef.attribut != 'Auszahlungspendenz/Payments'
                                              and attributdef.sysattributdef in (select attribut.sysattributdef from attribut, state where state.sysstate = attribut.sysstate and state.syswftable in (117,122) and state.flagaktiv = 1) 
                                               and ctlang.isocode=:isocode
                                              and cttattributdef.sysctlang = ctlang.sysctlang
                                            ) 
                                            where sysid=:id";
                        return ctx.ExecuteStoreQuery<DropListDto>(query, par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = input.isoCode });
                        String query = @"select sysattributdef sysid,code, attribut bezeichnung from
                                            (
                                              select distinct attributdef.sysattributdef, attributdef.attribut, attributdef.beschreibung, attributdef.attribut  code
                                              from attribut, attributdef, state,ctlang   
                                              where state.sysstate = attribut.sysstate and attribut.sysattributdef = attributdef.sysattributdef and state.syswftable in (117,122) and state.flagaktiv = 1  
                                               and ctlang.isocode=:isocode 
                                               AND attributdef.attribut != 'Auszahlungspendenz/Payments'
                                              and attributdef.sysattributdef not in (select sysattributdef from cttattributdef where sysctlang = ctlang.sysctlang)  
                                              union  
                                              select cttattributdef.SYSATTRIBUTDEF, cttattributdef.replaceattribut, cttattributdef.replacebeschreibung, attributdef.attribut code
                                              from cttattributdef, attributdef, ctlang
                                              where cttattributdef.sysattributdef = attributdef.sysattributdef  
                                              AND attributdef.attribut != 'Auszahlungspendenz/Payments'
                                              and attributdef.sysattributdef in (select attribut.sysattributdef from attribut, state where state.sysstate = attribut.sysstate and state.syswftable in (117,122) and state.flagaktiv = 1) 
                                               and ctlang.isocode=:isocode
                                              and cttattributdef.sysctlang = ctlang.sysctlang
                                            ) 
                                            order by upper(bezeichnung)";
                        return ctx.ExecuteStoreQuery<DropListDto>(query, par.ToArray()).ToArray();
                    }
                }
            });

            dictionary.Remove(XproEntityType.VTZUSTAND);
            dictionary.Add(XproEntityType.VTZUSTAND, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction2 = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = input.isoCode });
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = input.EntityId });
                        String query = @" select sysstatedef sysid,code, zustand bezeichnung from
                                            (
                                              select distinct statedef.sysstatedef, statedef.zustand, statedef.beschreibung, statedef.zustand code
                                              from state, statedef where state.sysstatedef = statedef.sysstatedef and state.syswftable = 5 and state.flagaktiv = 1 
  
                                              and statedef.sysstatedef not in (select sysstatedef from cttstatedef,ctlang where cttstatedef.sysctlang = ctlang.sysctlang and ctlang.isocode=:isocode) 
                                              union 
                                              select cttstatedef.SYSSTATEDEF, cttstatedef.replacezustand, cttstatedef.replacebeschreibung, statedef.zustand code
                                              from cttstatedef, statedef,ctlang where cttstatedef.sysctlang = ctlang.sysctlang and cttstatedef.sysstatedef = statedef.sysstatedef and statedef.sysstatedef in (select state.sysstatedef from state where state.syswftable = 5 and state.flagaktiv = 1) and cttstatedef.sysctlang = ctlang.sysctlang and ctlang.isocode=:isocode 
                                            ) 
                                            where sysstatedef=:id";
                        return ctx.ExecuteStoreQuery<DropListDto>(query, par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = input.isoCode });
                        String query = @"select sysstatedef sysid,code, zustand bezeichnung from
                                            (
                                              select distinct statedef.sysstatedef, statedef.zustand, statedef.beschreibung, statedef.zustand code
                                              from state, statedef where state.sysstatedef = statedef.sysstatedef and state.syswftable = 5 and state.flagaktiv = 1 
  
                                              and statedef.sysstatedef not in (select sysstatedef from cttstatedef,ctlang where cttstatedef.sysctlang = ctlang.sysctlang and ctlang.isocode=:isocode) 
                                              union 
                                              select cttstatedef.SYSSTATEDEF, cttstatedef.replacezustand, cttstatedef.replacebeschreibung, statedef.zustand code
                                              from cttstatedef, statedef,ctlang where cttstatedef.sysctlang = ctlang.sysctlang and cttstatedef.sysstatedef = statedef.sysstatedef and statedef.sysstatedef in (select state.sysstatedef from state where state.syswftable = 5 and state.flagaktiv = 1) and cttstatedef.sysctlang = ctlang.sysctlang and ctlang.isocode=:isocode 
                                            ) 
                                            order by upper(bezeichnung)";
                        return ctx.ExecuteStoreQuery<DropListDto>(query, par.ToArray()).ToArray();
                    }
                }
            });

            dictionary.Remove(XproEntityType.FZART);
            
           dictionary.Add(XproEntityType.FZART, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction2 = (input) =>
                {
                    

                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        DropListDto[] dl = new Cic.OpenOne.GateBANKNOW.Common.BO.RoleContextListsBo(new Cic.OpenOne.GateBANKNOW.Common.DAO.RoleContextListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao()).listAllAvailableObjekttypen(input.isoCode);
                        return (from d in dl
                                where d.sysID == input.EntityId
                                select d).FirstOrDefault();
                    }
                },

                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended olCtx = new Cic.OpenOne.Common.Model.DdOl.DdOlExtended())
                    {
                        return new Cic.OpenOne.GateBANKNOW.Common.BO.RoleContextListsBo(new Cic.OpenOne.GateBANKNOW.Common.DAO.RoleContextListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao()).listAllAvailableObjekttypen(input.isoCode);
                    }
                }
            });


            dictionary.Remove(XproEntityType.VERTRIEBSWEG);
            /*dictionary.Add(XproEntityType.VERTRIEBSWEG, new XproInfoDao()
            {
                QueryItemFunction2 = (input) =>
                {
                    var bo = CreateDictionaryListsBo(input.isoCode);
                    return bo.listByCode("GESCHAEFTSART", input.domainId).FirstOrDefault((a) => a.sysID == input.EntityId);
                },
                QueryItemsFunction = (filter) =>
                {
                    var bo = CreateDictionaryListsBo(filter.isoCode);
                    return bo.listByCode("GESCHAEFTSART", filter.domainId);
                },
            });*/

            dictionary.Add(XproEntityType.VERTRIEBSWEG, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction2 = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = input.EntityId });
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = input.isoCode });
                        return ctx.ExecuteStoreQuery<DropListDto>("SELECT VC_DDLKPPOS.sysddlkppos SYSID,VC_DDLKPPOS.ACTUALTERM bezeichnung,VC_DDLKPPOS.ACTUALTERM beschreibung,VC_DDLKPPOS.ORIGTERM code FROM CIC.VC_DDLKPPOS VC_DDLKPPOS, ctlang WHERE ctlang.isocode=:isocode and ctlang.sysctlang=VC_DDLKPPOS.sysctlang and  UPPER(code) = 'GESCHAEFTSART' and  VC_DDLKPPOS.sysddlkppos=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = input.isoCode });
                        return ctx.ExecuteStoreQuery<DropListDto>("SELECT VC_DDLKPPOS.sysddlkppos SYSID,VC_DDLKPPOS.ACTUALTERM bezeichnung,VC_DDLKPPOS.ACTUALTERM beschreibung,VC_DDLKPPOS.ORIGTERM code FROM CIC.VC_DDLKPPOS VC_DDLKPPOS, ctlang WHERE ctlang.isocode=:isocode and ctlang.sysctlang=VC_DDLKPPOS.sysctlang and  UPPER(code) = 'GESCHAEFTSART' AND VC_DDLKPPOS.ACTIVEFLAG=1 ORDER BY VC_DDLKPPOS.ACTUALTERM", par.ToArray()).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "VERTRIEBSWEG", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });

            //Prisma Produkt for banknow
            
             dictionaryCode.Add("PRPRODUCTGUI", new XproInfoDao<DropListDto>()
             {
                 QueryItemFunction2 = (input) =>
                 {
                     IPrismaDao pDao = PrismaDaoFactory.getInstance().getPrismaDao();
                     IObTypDao obDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getObTypDao();
                     Cic.OpenOne.GateBANKNOW.Common.DAO.IPruefungDao pruefDao = Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getPruefungDao();

                     ITranslateDao transDao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao();
                     PrismaProductBo bo = new PrismaProductBo(pDao, obDao, transDao, PrismaProductBo.CONDITIONS_BANKNOW, input.isoCode);
                    
                     Cic.OpenOne.Common.Model.Prisma.PRPRODUCT p = bo.getProduct(input.EntityId);
                     if (p == null) return null;
                     DropListDto dp = new DropListDto();
                     dp.sysID = p.SYSPRPRODUCT;
                     dp.beschreibung = p.DESCRIPTION;
                     dp.bezeichnung = p.NAME;
                     dp.code = p.NAME; //fallback value if no real code can be found
                     String vttypcode = null;
                     if (p.SYSVTTYP.HasValue && pDao.getVttypById(p.SYSVTTYP.Value) != null) 
                         vttypcode = pDao.getVttypById(p.SYSVTTYP.Value).CODE;
                     if (!String.IsNullOrEmpty(vttypcode))
                     {
                         if (vttypcode.Equals("CREDIT-NOW CASA"))
                             dp.code = "CASA";
                         else if (vttypcode.Equals("CREDIT-NOW DIPLOMA"))
                             dp.code = "DIPLOMA";
                         else if (vttypcode.Equals("CREDIT-NOW FLEX"))
							 dp.code = "DISPO";
						 else if (vttypcode.Equals("CREDIT-NOW CASA FLEX"))
							 dp.code = "FLEX";      // due to new vttyp.UNTERTYP (rh 20171027)
                         else
                             dp.code = vttypcode;	//original without mapping
                     }
                     else 
                     {
                         Cic.OpenOne.Common.Model.Prisma.VART vart = pDao.getVertragsart(p.SYSPRPRODUCT);
                         if (vart != null && vart.CODE != null)
                         {
                             string vartCode = vart.CODE.ToUpper();
                             if (vartCode.Equals("KREDIT_CLASSIC"))
                                 dp.code = "CLASSIC";
                             else if (vartCode.Equals("KREDIT_DISPOPLUS"))
                                 dp.code = "CARD";
                             else if (vartCode.Equals("KREDIT_DISPO"))
                                 dp.code = "DISPO";
                             else if (vartCode.Equals("LEASING"))
                                 dp.code = "LEASE";
                             else if (vartCode.Equals("KREDIT_FIX"))
                                 dp.code = "CREDIT";
                             else if (vartCode.Equals("KREDIT_EXPRESS"))
                                 dp.code = "EXPRESS";
                             else if (vartCode.Equals("TZK_PLUS"))
                                 dp.code = "CAR";
                             else
                                 dp.code = vart.CODE; //original without mapping
                         }
                     }
                     return dp;

                 },
                 QueryItemsFunction = (input) =>
                 {
                     return null;
                 }

             });



            dictionary.Remove(XproEntityType.NATIONALITAETEN);
            dictionary.Add(XproEntityType.NATIONALITAETEN, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction2 = (input) =>
                {
                    DropListDto[] nationalities 
                        = new DictionaryListsBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getDictionaryListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao(), input.isoCode).listNationalitaeten();
                        //= CommonDaoFactory.getInstance().getDictionaryListsDao().deliverNATIONALITIES();
                    DropListDto rval = null;

                    rval = (from r in nationalities
                            where r.sysID == input.EntityId
                            select r).FirstOrDefault();

                    if (rval == null)
                        rval = new DropListDto();
                    return rval;
                },
                QueryItemsFunction = (input) =>
                {
                    DropListDto[] items = new DictionaryListsBo(Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getDictionaryListsDao(), Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao(), input.isoCode).listNationalitaeten();
                    if (input.Filter == null)
                        return items.ToArray();

                    return (from r in items
                            where r.bezeichnung.Contains(input.Filter)
                            select r).ToArray();
                }

            });

            //Objektart
            dictionary.Add(XproEntityType.OBART, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        return ctx.ExecuteStoreQuery<DropListDto>("select rank sysid, name bezeichnung, description beschreibung from obart where rank=" + id, null).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        return ctx.ExecuteStoreQuery<DropListDto>("select rank sysid, name bezeichnung, description beschreibung from obart where activeflag=1", null).ToArray();
                    }


                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "OBART", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });
            dictionary.Remove(XproEntityType.VTSTATUS);
            dictionary.Add(XproEntityType.VTSTATUS, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<DropListDto> states = ctx.ExecuteStoreQuery<DropListDto>(@"select zustand beschreibung, zustand bezeichnung, rownum sysid, zustand code from (
                                select distinct extstate.zustand  from attribut, attributdef, state, statedef extstate, statedef intstate, wftable  where   attribut.sysstate = state.sysstate  and attribut.sysattributdef= attributdef.sysattributdef  and attribut.sysstatedef= extstate.sysstatedef  and state.sysstatedef=intstate.sysstatedef  and state.syswftable = wftable.syswftable  and wftable.syscode in ('ANGEBOT','ANTRAG')  
                                    order by extstate.zustand)", null).ToList();
                        return (from s in states
                                where s.sysID == id
                                select s).FirstOrDefault();
                       
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        return ctx.ExecuteStoreQuery<DropListDto>(@"select trim(zustand) beschreibung, trim(zustand) bezeichnung, rownum sysid, trim(zustand) code from (
                                select distinct extstate.zustand  from attribut, attributdef, state, statedef extstate, statedef intstate, wftable  where   attribut.sysstate = state.sysstate  and attribut.sysattributdef= attributdef.sysattributdef  and attribut.sysstatedef= extstate.sysstatedef  and state.sysstatedef=intstate.sysstatedef  and state.syswftable = wftable.syswftable  and wftable.syscode in ('ANGEBOT','ANTRAG')  
                                    order by extstate.zustand)",null).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "VTSTATUS", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });

            dictionary.Remove(XproEntityType.VART);
            dictionary.Add(XproEntityType.VART, new XproInfoDao<XproEntityDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<XproEntityDto>(@"SELECT sysvart sysid, code,bezeichnung title, bezeichnung desc1
                                            FROM (SELECT vart.sysvart, vart.bezeichnung AS Bezeichnung,
                                            vart.aktivkz, vart.lgd, vart.code
                                            FROM vart
                                            WHERE code <> 'KREDIT_FIX'
												AND aktivkz = 1
                                            UNION
                                            SELECT vttyp.sysvttyp, vttyp.bezeichnung AS Bezeichnung, 1, 0,
                                            vttyp.code
                                            FROM vttyp
                                            WHERE vttyp.code IN('CREDIT-NOW CASA', 'CREDIT-NOW CASA FLEX', 'CREDIT-NOW DIPLOMA', 'CREDIT-NOW FLEX')) vart
                                            where sysvart=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        return ctx.ExecuteStoreQuery<XproEntityDto>(@"SELECT sysvart sysid, code,bezeichnung title, bezeichnung desc1
                                            FROM (SELECT vart.sysvart, vart.bezeichnung AS Bezeichnung,
                                            vart.aktivkz, vart.lgd, vart.code
                                            FROM vart
                                            WHERE code <> 'KREDIT_FIX'
												AND aktivkz = 1 
                                            UNION
                                            SELECT vttyp.sysvttyp, vttyp.bezeichnung AS Bezeichnung, 1, 0,
                                            vttyp.code
                                            FROM vttyp
                                            WHERE vttyp.code IN('CREDIT-NOW CASA', 'CREDIT-NOW CASA FLEX', 'CREDIT-NOW DIPLOMA', 'CREDIT-NOW FLEX')) vart
                                            ORDER BY 2", null).ToArray();
                    }
                }
            });



          
            //Kampagne BNOW, mit channel-Filterung (FF/KF)
            dictionary.Remove(XproEntityType.CAMP);
            dictionary.Add(XproEntityType.CAMP, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syscamp", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select camp.syscamp sysid, camp.name bezeichnung, camp.description beschreibung from camp where camp.syscamp=:syscamp", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    String syscamptp = "-1";
                    String sysprchannel = "-1";
                    bool hasFilter = false;
                    if (input.filters.Length > 0 && input.filters[0].fieldname.ToLower().IndexOf("syscamptp") > -1)
                    {
                        String filters = input.filters[0].value;
                        syscamptp = filters.Split(',')[0];
                        sysprchannel = filters.Split(',')[1];
                        hasFilter = true;
                    }
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        if(!hasFilter)
                            return ctx.ExecuteStoreQuery<DropListDto>(@"select camp.syscamp sysid,camp.name bezeichnung, camp.description beschreibung from camptp, camp where 
                                        camptp.SYSCAMPTP = camp.SYSCAMPTP 
                                        and (camp.validfrom is null or camp.validfrom <= sysdate  or camp.validfrom<=to_date('01.01.0111' , 'dd.MM.yyyy') ) 
                                        and (camp.validuntil is null or camp.VALIDUNTIL >= sysdate  or camp.VALIDUNTIL<=to_date('01.01.0111' , 'dd.MM.yyyy') ) 
                                        and camp.activeflag=1
                        
                                        order by camptp.name,camp.RANG asc",null).ToArray();

                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syscamptp", Value = syscamptp });
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprchannel", Value = sysprchannel });
                        return ctx.ExecuteStoreQuery<DropListDto>(@"select camp.syscamp sysid,camp.name bezeichnung, camp.description beschreibung from camptp, camp where 
                                        camptp.SYSCAMPTP = camp.SYSCAMPTP 
                                        and (camp.validfrom is null or camp.validfrom <= sysdate  or camp.validfrom<=to_date('01.01.0111' , 'dd.MM.yyyy') ) 
                                        and (camp.validuntil is null or camp.VALIDUNTIL >= sysdate  or camp.VALIDUNTIL<=to_date('01.01.0111' , 'dd.MM.yyyy') ) 
                                        and camp.activeflag=1
                                        and camptp.SYSCAMPTP = :syscamptp
                                        and camp.SYSPRCHANNEL = :sysprchannel 
                                        order by camptp.name,camp.RANG asc", par.ToArray()).ToArray();
                            //"select camp.syscamp sysid, camp.name bezeichnung, camp.description beschreibung from camptp, camp where camptp.SYSCAMPTP = camp.SYSCAMPTP and camptp.SYSCAMPTP = :syscamptp and (camp.validfrom is null or camp.validfrom <= sysdate) and (camp.validuntil is null or camp.VALIDUNTIL >= sysdate) and camp.SYSPRCHANNEL = :sysprchannel order by camp.RANG asc", par.ToArray()).ToArray();
                    }

                }
            });


            dictionary.Remove(XproEntityType.VERTRIEBSEBENEVK);
            dictionary.Add(XproEntityType.VERTRIEBSEBENEVK, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select PERSON.SYSPERSON sysid,PERSON.NAME code,PERSON.NAME||' '||PERSON.VORNAME bezeichnung, PERSON.NAME||' '||PERSON.VORNAME beschreibung from PERSON  where sysperson=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();


                        long sysperson = 0;
                       
                        if (input.filters != null && input.filters.Length > 0)
                        {
                            if (input.filters[0].value != null)
                            {
                                sysperson = long.Parse(input.filters[0].value);
                              
                            }
                           
                        }
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperson", Value = sysperson });
                        //nur Recht B2B_VERTRAGSABSCHLUSS siehe R13 CR 180 On-Off-Boarding
                        //EPOS anstatt B2B, rücksprache mit Stas
                        return ctx.ExecuteStoreQuery<DropListDto>(@"SELECT PERSON.SYSPERSON sysid,PERSON.NAME code,PERSON.NAME||' '||PERSON.VORNAME bezeichnung, PERSON.NAME||' '||PERSON.VORNAME beschreibung FROM CIC.PERSON PERSON WHERE  person.sysperson in (select perole.sysperson from perole, roletype, perole perole2 where perole.sysroletype = roletype.sysroletype and roletype.typ = 7 and perole.sysparent = perole2.sysperole and perole2.sysperson = :sysperson) AND EXISTS (select 1 from perole where perole.sysperson = person.sysperson and perole.inactiveflag = 0) 
                                and exists(select sysrgm from RGR, RGM,wfuser where rgr.sysrgr = rgm.sysrgr and rgr.name in ('EPOS_VERTRAGSABSCHLUSS') and rgm.syswfuser = wfuser.syswfuser and wfuser.sysperson =person.sysperson and not exists (select 1 from wftzvar v, wftzust z where z.syswftzust=v.syswftzust and z.syswftable=2 and z.syslease = person.sysperson and z.state = 'RECHTE_FREIGABE' and v.code = to_char(rgm.sysrgr) and v.value = '0' ))
                                ORDER BY PERSON.NAME", par.ToArray()).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "VERTRIEBSEBENEVK", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });

            //Währungen
            dictionary.Remove(XproEntityType.WAEHRUNG);
            dictionary.Add(XproEntityType.WAEHRUNG, new XproInfoDao<WaehrungDto>()
            {
                QueryItemFunction = (id) =>
                {
                    var bo = CreateEntityBo();
                    return bo.getDetails<WaehrungDto>(id);
                },
                QueryItemsFunction = (input) =>
                {
                    List<Filter> filters = null;

                    var bo = CreateSearchBo<WaehrungDto>();
                    WaehrungDto[] rval1 = bo.search(CreateISearchDto("CODE", input.Filter, "WAEHRUNG.CODE", SortOrder.Asc, filters)).results;


                    List<WaehrungDto> results = new List<WaehrungDto>();
                    results.AddRange(rval1);
                    // results.AddRange(rval2);
                    return results.ToArray();
                },
                CreateBezeichnung = (item) => item.code,
                CreateBeschreibung = (item, rval) => createPanel(rval, "WAEHRUNG", item.code, "", "", "", "", "" + item.entityId),


            });
            dictionary.Remove(XproEntityType.KONTO_BLZ);
            dictionary.Add(XproEntityType.KONTO_BLZ, new XproInfoDao<XproEntityDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select blz.sysblz sysid, blz code, name title, strasse desc1,strassezusatz desc2, plz||' '||ort desc3 from blz where sysblz=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value = "%" + input.Filter + "%" });
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select blz.sysblz sysid, blz code, name title, strasse desc1,strassezusatz desc2, plz||' '||ort desc3 from blz where (upper(name) like upper(:filter) or upper(blz) like upper(:filter))", par.ToArray()).ToArray();
                    }
                }
            });

            dictionary.Remove(XproEntityType.BANK_BLZ);
            dictionary.Add(XproEntityType.BANK_BLZ, new XproInfoDao<XproEntityDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select blz.sysblz sysid, blz code,  name title, bic desc1 from blz where sysblz=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {

                        Cic.OpenOne.GateBANKNOW.Common.DAO.KontoDao dao = new Cic.OpenOne.GateBANKNOW.Common.DAO.KontoDao();
                        List<Cic.OpenOne.GateBANKNOW.Common.DTO.BlzDto> banken = new List<Cic.OpenOne.GateBANKNOW.Common.DTO.BlzDto>();

                        if (input.filters.Length > 0 && input.filters[0].fieldname.ToLower().IndexOf("blz") > -1 && input.filters[0].value != "-1" && input.filters[0].value.Length>1)
                        {

                            String filters = input.filters[0].value;
                            try{
                            String blz = filters.Split(',')[0];
                            String bic = filters.Split(',')[1];
                            if(blz!=null && blz.Length>0)
                                banken = dao.findBlz(blz, Cic.OpenOne.GateBANKNOW.Common.DAO.BlzType.BLZENDSWITH);

                            if (banken == null || banken.Count == 0)
                            {
                                if(bic!=null && bic.Length>0)
                                    banken = dao.findBlz(bic, Cic.OpenOne.GateBANKNOW.Common.DAO.BlzType.BICENDSWITH);
                            }
                            }catch(Exception ex)
                            {

                            }

                        }
                        if (banken == null || banken.Count == 0) return null;

                        var t = (from b in banken
                                 select new XproEntityDto()
                                 {
                                     sysID=b.sysblz,
                                     code = b.blz,
                                     title = b.name+" "+" "+b.strasse+" "+b.plz+" "+b.ort,
                                     desc1 = b.bic,
                                     desc3 = b.ort+" "+b.plz+" "+b.name+" "+b.strasse+" "+b.strasseZusatz
                                 }).OrderBy(a=>a.desc3).ToArray();
                        return t;
                        /*
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value = "%" + input.Filter + "%" });
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select blz.sysblz sysid, blz code, name title, bic desc1 from blz where (upper(name) like upper(:filter) or upper(blz) like upper(:filter))", par.ToArray()).ToArray();
                         * */
                    }
                }
            });

            dictionary.Remove(XproEntityType.BLZ_BIC);
            dictionary.Add(XproEntityType.BLZ_BIC, new XproInfoDao<XproEntityDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select blz.sysblz sysid, blz code,  bic title, name desc1, where blz=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value = "%" + input.Filter + "%" });
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select blz.sysblz sysid, blz code, bic title, bic desc1 from blz where (upper(bic) like upper(:filter) or upper(blz) like upper(:filter))", par.ToArray()).ToArray();
                    }
                }
            });


            dictionary.Remove(XproEntityType.WFMMKAT);
            dictionary.Add(XproEntityType.WFMMKAT, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction2 = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syskat", Value = input.EntityId });
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = input.isoCode });
                        return ctx.ExecuteStoreQuery<DropListDto>(@"select wfmmkat.syswfmmkat sysid, vc_ctlut.actualterm1 bezeichnung, typ indicator
                                        from wfmmkat, cic.vc_ctlut, ctlang
                                        where vc_ctlut.sysid = wfmmkat.syswfmmkat and wfmmkat.syswfmmkat=:syskat
                                        and vc_ctlut.area = 'WFMMKAT' and vc_ctlut.sysctlang = ctlang.sysctlang and ctlang.isocode=:isocode", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = input.isoCode });
                        return ctx.ExecuteStoreQuery<DropListDto>(@"select wfmmkat.syswfmmkat sysid
                                                                        , vc_ctlut.actualterm1 bezeichnung, typ indicator
                                                                        from wfmmkat
                                                                        , cic.vc_ctlut, ctlang
                                                                        where vc_ctlut.sysid = wfmmkat.syswfmmkat
                                                                        and vc_ctlut.area = 'WFMMKAT'
                                                                        and vc_ctlut.sysctlang = ctlang.sysctlang and ctlang.isocode=:isocode
																		and wfmmkat.syswfmmkat in ( select cfgvar.wert
                                                                        from cfg
                                                                        , cfgsec
                                                                        , cfgvar
                                                                        where cfg.syscfg = cfgsec.syscfg
                                                                        and cfgsec.syscfgsec = cfgvar.syscfgsec
                                                                        and upper(trim(cfgsec.code)) = upper('memo_kategorien')
																		and upper(trim(cfg.code)) in ('ANTRAGSASSISTENT','ANGEBOTSSASSISTENT')
																		)
                                                                        order by cic.vc_ctlut.actualterm1",par.ToArray()).ToArray();
                    }
                }
            });

            dictionary.Remove(XproEntityType.WFMMKATANGEBOT);
            dictionary.Add(XproEntityType.WFMMKATANGEBOT, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction2 = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syskat", Value = input.EntityId });
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = input.isoCode });
                        return ctx.ExecuteStoreQuery<DropListDto>(@"select wfmmkat.syswfmmkat sysid, vc_ctlut.actualterm1 bezeichnung, typ indicator
                                        from wfmmkat, cic.vc_ctlut, ctlang
                                        where vc_ctlut.sysid = wfmmkat.syswfmmkat and wfmmkat.syswfmmkat=:syskat
                                        and vc_ctlut.area = 'WFMMKAT' and vc_ctlut.sysctlang = ctlang.sysctlang and ctlang.isocode=:isocode", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = input.isoCode });
                        return ctx.ExecuteStoreQuery<DropListDto>(@"select wfmmkat.syswfmmkat sysid
                                                                        , vc_ctlut.actualterm1 bezeichnung, typ indicator
                                                                        from wfmmkat
                                                                        , cic.vc_ctlut, ctlang
                                                                        where vc_ctlut.sysid = wfmmkat.syswfmmkat
                                                                        and vc_ctlut.area = 'WFMMKAT'
                                                                        and vc_ctlut.sysctlang = ctlang.sysctlang and ctlang.isocode=:isocode
                                                                        and wfmmkat.syswfmmkat in ( select cfgvar.wert
                                                                        from cfg
                                                                        , cfgsec
                                                                        , cfgvar
                                                                        where cfg.syscfg = cfgsec.syscfg
                                                                        and cfgsec.syscfgsec = cfgvar.syscfgsec
                                                                        and upper(trim(cfgsec.code)) = upper('memo_kategorien')
                                                                        and upper(trim(cfg.code)) in ('ANGEBOTSSASSISTENT')
                                                                        )
                                                                        order by cic.vc_ctlut.actualterm1", par.ToArray()).ToArray();
                    }
                }
            });

            dictionary.Remove(XproEntityType.WFMMKATANTRAG);
            dictionary.Add(XproEntityType.WFMMKATANTRAG, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction2 = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syskat", Value = input.EntityId });
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = input.isoCode });
                        return ctx.ExecuteStoreQuery<DropListDto>(@"select wfmmkat.syswfmmkat sysid, vc_ctlut.actualterm1 bezeichnung, typ indicator
                                        from wfmmkat, cic.vc_ctlut, ctlang
                                        where vc_ctlut.sysid = wfmmkat.syswfmmkat and wfmmkat.syswfmmkat=:syskat
                                        and vc_ctlut.area = 'WFMMKAT' and vc_ctlut.sysctlang = ctlang.sysctlang and ctlang.isocode=:isocode", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = input.isoCode });
                        return ctx.ExecuteStoreQuery<DropListDto>(@"select wfmmkat.syswfmmkat sysid
                                                                        , vc_ctlut.actualterm1 bezeichnung, typ indicator
                                                                        from wfmmkat
                                                                        , cic.vc_ctlut, ctlang
                                                                        where vc_ctlut.sysid = wfmmkat.syswfmmkat
                                                                        and vc_ctlut.area = 'WFMMKAT'
                                                                        and vc_ctlut.sysctlang = ctlang.sysctlang and ctlang.isocode=:isocode
                                                                        and wfmmkat.syswfmmkat in ( select cfgvar.wert
                                                                        from cfg
                                                                        , cfgsec
                                                                        , cfgvar
                                                                        where cfg.syscfg = cfgsec.syscfg
                                                                        and cfgsec.syscfgsec = cfgvar.syscfgsec
                                                                        and upper(trim(cfgsec.code)) = upper('memo_kategorien')
                                                                        and upper(trim(cfg.code)) in ('ANTRAGSASSISTENT','ANGEBOTSSASSISTENT')
                                                                        )
                                                                        order by cic.vc_ctlut.actualterm1", par.ToArray()).ToArray();
                    }
                }
            });


            dictionary.Remove(XproEntityType.WFMMKATVT);
            dictionary.Add(XproEntityType.WFMMKATVT, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction2 = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syskat", Value = input.EntityId });
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = input.isoCode });
                        return ctx.ExecuteStoreQuery<DropListDto>(@"select wfmmkat.syswfmmkat sysid, vc_ctlut.actualterm1 bezeichnung, typ indicator
                                        from wfmmkat, cic.vc_ctlut, ctlang
                                        where vc_ctlut.sysid = wfmmkat.syswfmmkat and wfmmkat.syswfmmkat=:syskat
                                        and vc_ctlut.area = 'WFMMKAT' and vc_ctlut.sysctlang = ctlang.sysctlang and ctlang.isocode=:isocode", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = input.isoCode });
                        return ctx.ExecuteStoreQuery<DropListDto>(@"select wfmmkat.syswfmmkat sysid
                                                                        , vc_ctlut.actualterm1 bezeichnung, typ indicator
                                                                        from wfmmkat
                                                                        , cic.vc_ctlut, ctlang
                                                                        where vc_ctlut.sysid = wfmmkat.syswfmmkat
                                                                        and vc_ctlut.area = 'WFMMKAT'
                                                                        and vc_ctlut.sysctlang = ctlang.sysctlang and ctlang.isocode=:isocode
                                                                        order by cic.vc_ctlut.actualterm1", par.ToArray()).ToArray();
                    }
                }
            });

			// rh: not here: dictionary.Remove ("CAMPAIGNS");
			dictionaryCode.Add ("CAMPAIGNS", new XproInfoDao<DropListDto> ()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syscamp", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select camp.syscamp sysid, camp.name bezeichnung, camp.description beschreibung from camp where camp.syscamp=:syscamp", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                  
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        
                        return ctx.ExecuteStoreQuery<DropListDto>(@"select camp.syscamp sysid, '('||camptp.name||') '||camp.name bezeichnung, camp.description beschreibung from camptp, camp where 
                                        camptp.SYSCAMPTP = camp.SYSCAMPTP 
                                        
                                        order by camptp.name,camp.RANG asc", null).ToArray();
                    }

                }
            });

            dictionary.Remove(XproEntityType.ZEKKARTENTYCODE);
            dictionary.Add(XproEntityType.ZEKKARTENTYCODE, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction2 = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = input.EntityId });
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = input.isoCode });
                        return ctx.ExecuteStoreQuery<DropListDto>("SELECT VC_DDLKPPOS.sysddlkppos SYSID,VC_DDLKPPOS.ACTUALTERM bezeichnung,VC_DDLKPPOS.ACTUALTERM beschreibung,VC_DDLKPPOS.ORIGTERM code FROM CIC.VC_DDLKPPOS VC_DDLKPPOS, ctlang WHERE ctlang.isocode=:isocode and ctlang.sysctlang=VC_DDLKPPOS.sysctlang and  UPPER(code) = 'ZEKKARTENTYPCODE' and  id = :id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = input.isoCode });
                        return ctx.ExecuteStoreQuery<DropListDto>("SELECT VC_DDLKPPOS.sysddlkppos SYSID,VC_DDLKPPOS.ACTUALTERM bezeichnung,VC_DDLKPPOS.ACTUALTERM beschreibung,VC_DDLKPPOS.ORIGTERM code FROM CIC.VC_DDLKPPOS VC_DDLKPPOS, ctlang WHERE ctlang.isocode=:isocode and ctlang.sysctlang=VC_DDLKPPOS.sysctlang and  UPPER(code) = 'ZEKKARTENTYPCODE' AND VC_DDLKPPOS.ACTIVEFLAG=1 ORDER BY VC_DDLKPPOS.ACTUALTERM", par.ToArray()).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "ZEKKARTENTYCODE", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });

            dictionary.Remove(XproEntityType.ZEKEREIGNISCODE);
            dictionary.Add(XproEntityType.ZEKEREIGNISCODE, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction2 = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = input.EntityId });
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = input.isoCode });
                        return ctx.ExecuteStoreQuery<DropListDto>("SELECT VC_DDLKPPOS.sysddlkppos SYSID,VC_DDLKPPOS.ACTUALTERM bezeichnung,VC_DDLKPPOS.ACTUALTERM beschreibung,VC_DDLKPPOS.ORIGTERM code FROM CIC.VC_DDLKPPOS VC_DDLKPPOS, ctlang WHERE ctlang.isocode=:isocode and ctlang.sysctlang=VC_DDLKPPOS.sysctlang and  UPPER(code) = 'ZEKEREIGNISCODE' and  id = :id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = input.isoCode });
                        return ctx.ExecuteStoreQuery<DropListDto>("SELECT VC_DDLKPPOS.sysddlkppos SYSID,VC_DDLKPPOS.ACTUALTERM bezeichnung,VC_DDLKPPOS.ACTUALTERM beschreibung,VC_DDLKPPOS.ORIGTERM code FROM CIC.VC_DDLKPPOS VC_DDLKPPOS, ctlang WHERE ctlang.isocode=:isocode and ctlang.sysctlang=VC_DDLKPPOS.sysctlang and  UPPER(code) = 'ZEKEREIGNISCODE' AND VC_DDLKPPOS.ACTIVEFLAG=1 ORDER BY VC_DDLKPPOS.ACTUALTERM", par.ToArray()).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "ZEKEREIGNISCODE", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });


            
        
          

			dictionary.Remove (XproEntityType.SLAPAUSECODE);
			dictionary.Add (XproEntityType.SLAPAUSECODE, new XproInfoDao<DropListDto> ()
			{
				//QueryItemFunction = (id) =>
				//{
				//	using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
				//	{
				//		return CommonDaoFactory.getInstance ().getDictionaryListsDao ().deliverSlaPause (id.isoCode);
				//	}
				//},
                QueryItemsFunction = (input) =>
                {
                  
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        
                       return CommonDaoFactory.getInstance ().getDictionaryListsDao ().deliverSlaPause (input.isoCode).ToArray();
                    }

                }
			});
        }
    }
}
