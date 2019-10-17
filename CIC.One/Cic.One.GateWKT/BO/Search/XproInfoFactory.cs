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


namespace Cic.One.GateWKT.BO.Search
{
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

        protected override void registerProviders()
        {
            base.registerProviders();
            //Kalkulationstyp
            dictionary.Add(XproEntityType.KALKTYP, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        return ctx.ExecuteStoreQuery<DropListDto>("select syskalktyp sysid, bezeichnung from kalktyp=" + id, null).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {


                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        return ctx.ExecuteStoreQuery<DropListDto>("select syskalktyp sysid, bezeichnung from kalktyp where syskalktyp in (44, 52, 42,48, 39, 50, 49) order by rangsl, syskalktyp", null).ToArray();
                        
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item,rval) => createPanel(rval,"KALKTYP", item.bezeichnung, "", "", "", "", "" + item.sysID),
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
                        return ctx.ExecuteStoreQuery<DropListDto>("select rank sysid, name bezeichnung, description beschreibung from obart", null).ToArray();
                    }
                  

                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item,rval) => createPanel(rval,"OBART", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });
            //Verkaufsförderung
            dictionary.Add(XproEntityType.KONSTELLATION, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select d2.value code,d2.rang sysid, d2.value bezeichnung, d2.bemerkung beschreibung FROM dd, ddkey d, ddkeys1 d1, ddkeys2 d2 WHERE dd.fieldkey = d.sysddkey AND d.sysddkey = d1.ddkeyid AND d1.sysddkeys1 = d2.ddkeyid AND UPPER(dd.field) = UPPER('ANTRAG:Konstellation') AND UPPER(d1.value) = UPPER(TRIM('Aktionen')) and d2.value=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        return ctx.ExecuteStoreQuery<DropListDto>("select d2.value code, d2.rang sysid, d2.value bezeichnung, d2.bemerkung beschreibung FROM dd, ddkey d, ddkeys1 d1, ddkeys2 d2 WHERE dd.fieldkey = d.sysddkey AND d.sysddkey = d1.ddkeyid AND d1.sysddkeys1 = d2.ddkeyid AND UPPER(dd.field) = UPPER('ANTRAG:Konstellation') AND UPPER(d1.value) = UPPER(TRIM('Aktionen')) order by d2.rang", null).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item,rval) => createPanel(rval,"KONSTELLATION", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });
            //Opt. Kundeninfo
            dictionary.Add(XproEntityType.OPTKUNDENINFO, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select d1.value code, d1.rang sysid, d1.value bezeichnung, '' beschreibung FROM ddkey d, ddkeys1 d1 WHERE d.sysddkey = d1.ddkeyid AND UPPER(d.description) like '%OPTIONALE KUNDENINFO%' and d1.value=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        return ctx.ExecuteStoreQuery<DropListDto>("select d1.value code, d1.rang sysid, d1.value bezeichnung, '' beschreibung FROM ddkey d, ddkeys1 d1 WHERE d.sysddkey = d1.ddkeyid AND UPPER(d.description) like '%OPTIONALE KUNDENINFO%'", null).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item,rval) => createPanel(rval,"OPTKUNDENINFO", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });
            //Fullservice Typ
            dictionary.Remove(XproEntityType.FSTYP);
            dictionary.Add(XproEntityType.FSTYP, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select sysfstyp code, sysfstyp sysid, bezeichnung,  beschreibung FROM fstyp where sysfstyp=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        return ctx.ExecuteStoreQuery<DropListDto>("select sysfstyp code, sysfstyp sysid, bezeichnung,  beschreibung FROM fstyp", null).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item,rval) => createPanel(rval,"FSTYP", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });
            //BSIPakete
            dictionary.Add(XproEntityType.BSIPAKET, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select sysfstyp code, sysfstyp sysid, bezeichnung, beschreibung from fstyp where sysfstyp=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        

                        String konstellation = "";
                        String sysobtyp = "0";
                        if (input.filters.Length > 0 && input.filters[0].fieldname.ToLower().IndexOf("konstellation") > -1)
                        {
                            String filters = input.filters[0].value;
                            konstellation = filters.Split(',')[0];
                            sysobtyp = filters.Split(',')[1];
                        }
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "konstellation", Value = konstellation });
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysobtyp", Value = sysobtyp });

                        
                        String sql = "SELECT  fstyp.sysfstyp code, fstyp.sysfstyp sysid, fstyp.bezeichnung, nvl(cic.cic_common_utils.cicvalue(sysdate,obtyp.sysvgsi,'Betrag',fstyp.bezeichnung,0,0,0),0) beschreibung FROM obtyp, ddkeys3,  ddkeys2,  ddkeys1,  ddkey,  fsart,  fstyp WHERE obtyp.sysobtyp=:sysobtyp and ddkeys3.ddkeyid=ddkeys2.sysddkeys2 AND ddkeys2.ddkeyid  =ddkeys1.sysddkeys1 AND ddkeys1.ddkeyid  =ddkey.sysddkey AND ddkey.description='Tarifkennzeichen' AND ddkeys1.value    ='Aktionen' AND ddkeys3.value = fsart.beschreibung and fstyp.sysfsart = fsart.sysfsart and  ddkeys2.value=:konstellation and cic.cic_common_utils.cicvalue(sysdate,obtyp.sysvgsi,'Betrag',fstyp.bezeichnung,0,0,0) IS NOT NULL order by  nvl(cic.cic_common_utils.cicvalue(sysdate,obtyp.sysvgsi,'Betrag',fstyp.bezeichnung,0,0,0),0)";


                        DropListDto[] rval =  ctx.ExecuteStoreQuery<DropListDto>(sql, par.ToArray()).ToArray();
                        double sum = (from r in rval
                                     select double.Parse(r.beschreibung)).Sum();

                        if (sum == 0)
                            return new DropListDto[0]; 

                        return rval;
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item,rval) => createPanel(rval,"OPTKUNDENINFO", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });
            //Fahrzeugzubehör
            dictionary.Remove(XproEntityType.ETGADDITION);
            dictionary.Add(XproEntityType.ETGADDITION, new XproInfoDao<XproEntityDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });

                        string q = "select obtyp.schwacke code,etgaddition.id sysid,  etgeqtext.text || ' (' || etgaddition.id || ')' title,etgeqtext.text || ' (' || etgaddition.id || ')' bezeichnung,etgaddition.id desc1, price2||' EUR' desc2, price2 beschreibung from obtyp,etgaddition,etgeqtext where ETGADDITION.natcode = schwacke AND etgeqtext.eqcode = etgaddition.eqcode and etgaddition.id=:id";
                        return ctx.ExecuteStoreQuery<XproEntityDto>(q, par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par =  new List<Devart.Data.Oracle.OracleParameter>();
                        if (input.filters.Length > 0 && input.filters[0].fieldname.ToLower().IndexOf("sysobtyp") > -1)
                        {
                            par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysobtyp", Value = input.filters[0].value });

                            string flagpack = "";
                            if (input.filters.Length > 0 && input.filters[1].fieldname.ToLower().IndexOf("flagpack") > -1)
                            {
                                try
                                {
                                    int f = Convert.ToInt32(input.filters[1].value);
                                    if (f == 1)
                                    {
                                        flagpack = " and flagpack = 1 ";
                                    }
                                    else
                                    {
                                        flagpack = " and flagpack != 1 ";
                                    }
                                }
                                catch
                                {
                                }                                
                            }

                            string besch = "";
                            if ((input.Filter != null) && (input.Filter.Length > 0))
                            {
                                long input_long = 0;
                                bool input_is_number = false;
                                try
                                {
                                    input_is_number = long.TryParse(input.Filter, out input_long);
                                }
                                catch
                                {
                                    input_is_number = false;
                                    input_long = 0;
                                }

                                if (input_is_number)
                                {
                                    besch = " and (upper(etgeqtext.text) LIKE '" + input.Filter.ToUpper() + "%' or etgaddition.id LIKE '" + input.Filter + "%') ";
                                }
                                else
                                {
                                    besch = " and upper(etgeqtext.text) LIKE '" + input.Filter.ToUpper() + "%' ";
                                }
                            }

                            //string q = "select obtyp.schwacke code,etgaddition.id sysid, etgeqtext.text beschreibung, etgaddition.price1 bezeichnung from obtyp,etgaddition,etgeqtext where ETGADDITION.natcode = schwacke AND etgeqtext.eqcode = etgaddition.eqcode " + add;
                            string q = "select price2 code,etgaddition.id sysid, etgeqtext.text || ' (' || etgaddition.id || ')' title,etgaddition.id desc1, price2||' EUR' desc2, etgeqtext.text || ' (' || etgaddition.id || ')' bezeichnung from obtyp,etgaddition,etgeqtext where etgaddition.flag!=4 and ETGADDITION.natcode = schwacke AND etgeqtext.eqcode = etgaddition.eqcode and obtyp.sysobtyp =:sysobtyp " + flagpack + besch;
                            //string q = "select obtyp.schwacke code,etgaddition.id sysid, etgeqtext.text bezeichnung, etgaddition.price1 beschreibung from obtyp,etgaddition,etgeqtext where ETGADDITION.natcode = obtyp.schwacke AND etgeqtext.eqcode = etgaddition.eqcode and obtyp.sysobtyp =:sysobtyp";
                            return ctx.ExecuteStoreQuery<XproEntityDto>(q, par.ToArray()).ToArray();
                        }
                        return new XproEntityDto[0];
                    }
                },
                CreateBezeichnung = (item) => item.beschreibung + "(" + item.sysID.ToString() + ")",
                CreateBeschreibung = (item,rval) => createPanel(rval,"ETGADDITION", item.beschreibung + "(" + item.sysID.ToString() + ")", "", "", "", "", "" + item.sysID),
            });

            //Rahmenverträge
            dictionary.Remove(XproEntityType.RAHMEN);
            dictionary.Add(XproEntityType.RAHMEN, new XproInfoDao<RahmenDto>()
            {
                QueryItemFunction = (id) =>
                {
                    var bo = CreateEntityBo();
                    return bo.getDetails<RahmenDto>(id);
                },
                QueryItemsFunction = (input) =>
                {
                    List<Filter> filters = null;
                    if (input.filters.Length > 0 && input.filters[0].fieldname.ToLower().IndexOf("syswktaccount") > -1)
                    {
                        //build new filters for wktaccount inputfilter
                        String wktacc = input.filters[0].value;
                        long syswktaccount = long.Parse(wktacc);
                        filters = new List<Filter>();
                        if (syswktaccount < 0)
                        {
                            filters.Add(new Filter()
                            {
                                fieldname = "sysit",
                                filterType = FilterType.Equal,
                                value = "" + syswktaccount * -1
                            });
                        }
                        else
                        {
                            filters.Add(new Filter()
                            {
                                fieldname = "sysperson",
                                filterType = FilterType.Equal,
                                value = "" + syswktaccount
                            });
                        }
                        filters.Add(new Filter()
                        {
                            fieldname = "beginn",
                            filterType = FilterType.DateLE,
                            value = string.Format("{0:yyyy-MM-dd}", DateTime.Now)
                        });
                        filters.Add(new Filter()
                        {
                            fieldname = "ende",
                            filterType = FilterType.DateGE,
                            value = string.Format("{0:yyyy-MM-dd}", DateTime.Now)
                        });
                       
                    }
                    if (input.filters.Length > 0 && input.filters[0].fieldname.ToLower().IndexOf("vorlage") > -1)
                    {

                        filters = new List<Filter>();
                        Filter f = new Filter();
                        f.filterType = FilterType.SQL;
                        f.fieldname = "(sysit";
                        f.value = "=0 or sysit is null)";
                        filters.Add(f);
                      

                        f = new Filter();
                        f.filterType = FilterType.SQL;
                        f.fieldname = "(sysperson";
                        f.value = "=0 or sysperson is null)";
                        filters.Add(f);
                      
                    }
                    if (input.Filter == null || input.filters == null || input.filters.Length == 0)
                    {
                        return new RahmenDto[0];
                    }

                    var bo = CreateSearchBo<RahmenDto>();
                    RahmenDto[] rval1 = bo.search(CreateISearchDto("BESCHREIBUNG", input.Filter, "RVT.BESCHREIBUNG", SortOrder.Asc, filters)).results;


                    //also present rvt templates in dropdown:
                    /*filters = new List<Filter>();
                    Filter f = new Filter();
                    f.filterType = FilterType.Equal;
                    f.fieldname = "sysit";
                    f.value = "0";
                    filters.Add(f);
                    f = new Filter();
                    f.filterType = FilterType.Equal;
                    f.fieldname = "sysperson";
                    f.value = "0";
                    filters.Add(f);

                    RahmenDto[] rval2 = bo.search(CreateISearchDto("BESCHREIBUNG", null, "RVT.BESCHREIBUNG", SortOrder.Asc, filters)).results;
                     * */
                    List<RahmenDto> results = new List<RahmenDto>();
                    results.AddRange(rval1);
                    // results.AddRange(rval2);
                    return results.ToArray();
                },
                CreateBezeichnung = (item) => item.entityBezeichnung,
                CreateBeschreibung = (item,rval) => createPanel(rval,"RVT", item.rahmen, "", "", "", "", "" + item.entityId),


            });

            //Versicherungsgesellschaften
            dictionary.Add(XproEntityType.VERSICHERUNG, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperson", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select code, sysperson sysid, name bezeichnung,  matchcode beschreibung from person where flaggst=1 and aktivkz=1 and sysperson=:sysperson", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                   
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        return ctx.ExecuteStoreQuery<DropListDto>("select code, sysperson sysid,trim(name)||' '||vorname bezeichnung, matchcode beschreibung from person where sysperson in (select to_number(cfgvar.code) from cfgvar, cfgsec, cfg where cfg.syscfg = cfgsec.syscfg and cfgsec.syscfgsec = cfgvar.syscfgsec and cfg.code = 'CICONE' and cfgsec.code ='DROPDOWN_VERSICHERUNG_RVT' ) order by name", null).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item,rval) => createPanel(rval,"VERSICHERUNG", item.bezeichnung,item.beschreibung, item.code, "", "", "" + item.sysID),
            });
            //Versicherungstyp
            dictionary.Remove(XproEntityType.VSTYP);
            dictionary.Add(XproEntityType.VSTYP, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvstyp", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select sysvstyp sysid, code, code bezeichnung,  beschreibung  from vstyp where sysvstyp =:sysvstyp", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    if (input.Filter == null || input.filters == null || input.filters.Length == 0)
                    {
                        return new DropListDto[0];
                    }
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        String method="";
                        if (input.filters.Length > 0 && input.filters[0].fieldname.ToLower().IndexOf("codemethod") > -1)
                        {
                            method = input.filters[0].value;
                        }
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "method", Value = method });
                        return ctx.ExecuteStoreQuery<DropListDto>("select sysvstyp sysid, vstyp.code, vstyp.code bezeichnung,  vstyp.beschreibung  from vstyp,vsart where vsart.sysvsart=vstyp.sysvsart and vsart.code=:method", par.ToArray()).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item,rval) => createPanel(rval,"VSTYP", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });
            //Nebenkonto
            dictionary.Remove(XproEntityType.NKONTO);
            dictionary.Add(XproEntityType.NKONTO, new XproInfoDao<NkontoDto>()
            {
                QueryItemFunction = (id) =>
                {
                    var bo = CreateEntityBo();
                    return bo.getDetails<NkontoDto>(id);
                },
                QueryItemsFunction = (input) =>
                {
                    List<Filter> filters = null;

                    var bo = CreateSearchBo<NkontoDto>();
                    NkontoDto[] rval1 = bo.search(CreateISearchDto("KONTO", input.Filter, "NKONTO.KONTO", SortOrder.Asc, filters)).results;


                    List<NkontoDto> results = new List<NkontoDto>();
                    results.AddRange(rval1);
                    // results.AddRange(rval2);
                    return results.ToArray();
                },
                CreateBezeichnung = (item) => item.konto,
                CreateBeschreibung = (item,rval) => createPanel(rval,"NKONTO", item.konto, "", "", "", "", "" + item.entityId),


            });
            //Prisma Produkt
            dictionary.Remove(XproEntityType.PRPRODUCT);
            dictionary.Add(XproEntityType.PRPRODUCT, new XproInfoDao<PrproductDto>()
            {
                QueryItemFunction = (id) =>
                {
                    var bo = CreateEntityBo();
                    return bo.getDetails<PrproductDto>(id);
                },
                QueryItemsFunction = (input) =>
                {
                    List<Filter> filters = null;

                    var bo = CreateSearchBo<PrproductDto>();
                    PrproductDto[] rval1 = bo.search(CreateISearchDto("NAME", input.Filter, "PRPRODUCT.NAME", SortOrder.Asc, filters)).results;


                    List<PrproductDto> results = new List<PrproductDto>();
                    results.AddRange(rval1);
                    // results.AddRange(rval2);
                    return results.ToArray();
                },
                CreateBezeichnung = (item) => item.name,
                CreateBeschreibung = (item,rval) => createPanel(rval,"PRPRODUCT", item.name, "", "", "", "", "" + item.entityId),


            });
            //Zinstypen
            dictionary.Remove(XproEntityType.PRINTSET);
            dictionary.Add(XproEntityType.PRINTSET, new XproInfoDao<PrintsetDto>()
            {
                QueryItemFunction = (id) =>
                {
                    var bo = CreateEntityBo();
                    return bo.getDetails<PrintsetDto>(id);
                },
                QueryItemsFunction = (input) =>
                {
                    List<Filter> filters = null;

                    var bo = CreateSearchBo<PrintsetDto>();
                    PrintsetDto[] rval1 = bo.search(CreateISearchDto("NAME", input.Filter, "PRINTSET.NAME", SortOrder.Asc, filters)).results;


                    List<PrintsetDto> results = new List<PrintsetDto>();
                    results.AddRange(rval1);
                    // results.AddRange(rval2);
                    return results.ToArray();
                },
                CreateBezeichnung = (item) => item.name,
                CreateBeschreibung = (item,rval) => createPanel(rval,"PRINTSET", item.name, "", "", "", "", "" + item.entityId),


            });
            //Tilgungsplan
            dictionary.Remove(XproEntityType.PRTLGSET);
            dictionary.Add(XproEntityType.PRTLGSET, new XproInfoDao<PrtlgsetDto>()
            {
                QueryItemFunction = (id) =>
                {
                    var bo = CreateEntityBo();
                    return bo.getDetails<PrtlgsetDto>(id);
                },
                QueryItemsFunction = (input) =>
                {
                    List<Filter> filters = null;

                    var bo = CreateSearchBo<PrtlgsetDto>();
                    PrtlgsetDto[] rval1 = bo.search(CreateISearchDto("NAME", input.Filter, "PRTLGSET.NAME", SortOrder.Asc, filters)).results;


                    List<PrtlgsetDto> results = new List<PrtlgsetDto>();
                    results.AddRange(rval1);
                    // results.AddRange(rval2);
                    return results.ToArray();
                },
                CreateBezeichnung = (item) => item.name,
                CreateBeschreibung = (item,rval) => createPanel(rval,"PRTLGSET", item.name, "", "", "", "", "" + item.entityId),


            });
            //Reifen Wechselrhythmus
            dictionary.Add(XproEntityType.WECHSELRHYTHMUS, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    DropListDto item = new DropListDto();
                    item.sysID = (long)id;
                    item.code = "" + (long)id;
                    item.beschreibung = item.code;
                    item.bezeichnung = item.code;
                    return item;
                },
                QueryItemsFunction = (input) =>
                {
                    CachedQuoteDao qd = new CachedQuoteDao();
                    double wechselrhythmus_max = qd.getQuote(Cic.OpenLease.Service.QUOTEDao.QUOTE_WECHSELRHYTHMUSMAX_ALPHABET);
                    double wechselrhythmus_min = qd.getQuote(Cic.OpenLease.Service.QUOTEDao.QUOTE_WECHSELRHYTHMUSMIN_ALPHABET);
                    double wechselrhythmus_step = qd.getQuote(Cic.OpenLease.Service.QUOTEDao.QUOTE_WECHSELRHYTHMUSSTEP_ALPHABET);
                    if (wechselrhythmus_max <= 0)
                        wechselrhythmus_max = 100000;
                    if (wechselrhythmus_min <= 0)
                        wechselrhythmus_min = 5000;
                    if (wechselrhythmus_step <= 0)
                        wechselrhythmus_step = 5000;

                    List<DropListDto> results = new List<DropListDto>();
                    for (double i = wechselrhythmus_min; i <= wechselrhythmus_max; i += wechselrhythmus_step)
                    {
                        DropListDto item = new DropListDto();
                        item.sysID = (long)i;
                        item.code = ""+(long)i;
                        item.beschreibung = item.code;
                        item.bezeichnung = item.code;
                        results.Add(item);
                    }
                    return results.ToArray();
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item,rval) => createPanel(rval,"WECHSELRHYTHMUS", item.bezeichnung, "", "", "", "", "" + item.sysID),


            });
            //Objektkategorien
            dictionary.Remove(XproEntityType.OBKAT);
            dictionary.Add(XproEntityType.OBKAT, new XproInfoDao<ObkatDto>()
            {
                QueryItemFunction = (id) =>
                {
                    var bo = CreateEntityBo();
                    return bo.getDetails<ObkatDto>(id);
                },
                QueryItemsFunction = (input) =>
                {
                    List<Filter> filters = null;

                    var bo = CreateSearchBo<ObkatDto>();
                    ObkatDto[] rval1 = bo.search(CreateISearchDto("NAME", input.Filter, "OBKAT.NAME", SortOrder.Asc, filters)).results;


                    List<ObkatDto> results = new List<ObkatDto>();
                    results.AddRange(rval1);
                    // results.AddRange(rval2);
                    return results.ToArray();
                },
                CreateBezeichnung = (item) => item.name,
                CreateBeschreibung = (item,rval) => createPanel(rval,"OBKAT", item.name, "", "", "", "", "" + item.entityId),


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
                CreateBeschreibung = (item,rval) => createPanel(rval,"WAEHRUNG", item.code, "", "", "", "", "" + item.entityId),


            });
            //Vertragsarten
            dictionary.Remove(XproEntityType.VART);
            dictionary.Add(XproEntityType.VART, new XproInfoDao<VartDto>()
            {
                QueryItemFunction = (id) =>
                {
                    var bo = CreateEntityBo();
                    return bo.getDetails<VartDto>(id);
                },
                QueryItemsFunction = (input) =>
                {
                    List<Filter> filters = null;

                    var bo = CreateSearchBo<VartDto>();
                    VartDto[] rval1 = bo.search(CreateISearchDto("SYSVART", input.Filter, "VART.SYSVART", SortOrder.Asc, filters)).results;
                  


                    List<VartDto> results = new List<VartDto>();
                    results.AddRange(rval1);
                    // results.AddRange(rval2);
                    return results.ToArray();
                },
                CreateBezeichnung = (item) => item.Bezeichnung,
                CreateBeschreibung = (item,rval) => createPanel(rval,"VART", item.Bezeichnung, "", "", "", "", "" + item.entityId),


            });

            #region obtyp
            dictionary.Remove(XproEntityType.OBTYP);
            //Fahrzeugsuche
            dictionary.Add(XproEntityType.OBTYP, new XproInfoDao<XproEntityDto>()//dieser typ definiert sein descriptionpanel selbst, es wird kein createBeschreibung aufgerufen!
            {
                QueryItemFunction = (id) =>
                {
                    if (id == 0)
                        return new XproEntityDto();

                    var bo = CreateEntityBo();
                    ObtypDto obtyp = bo.getObtypDetails(id);



                    return new XproEntityDto(obtyp.entityId,obtyp.marke+" "+obtyp.bezeichnung) ;

                },
                QueryItemsFunction = (input) =>
                {
                    if (input.Filter == null || input.Filter.Length < 2) return null;
                    Cic.P000001.Common.Setting setting = new P000001.Common.Setting();
                    setting.SelectedCurrency = "EUR";
                    setting.SelectedLanguage = "de-AT";//TODO configurable?
                    setting.SearchParams = new P000001.Common.TypedSearchParam[2];
                    setting.SearchParams[0] = new P000001.Common.TypedSearchParam();
                    setting.SearchParams[0].searchType = P000001.Common.SearchType.FUZZY;//filtert nur welche mit aufbaucode
                    setting.SearchParams[0].Pattern = input.Filter;
                    setting.SearchParams[1] = new P000001.Common.TypedSearchParam();
                    setting.SearchParams[1].searchType = P000001.Common.SearchType.OBJEKTTYP;


                    setting.SearchParams[1].Pattern = "100";//nur autos

                    long sysrvt = 0;
                    if (input.filters != null && input.filters.Length > 0)
                    {
                        foreach(Filter fi in  input.filters)
                        {
                            String[] fnames = fi.fieldname.Split(',');
                            int i = 0;
                            foreach(String fname in fnames)
                            {
                                String[] vals = fi.value.Split(',');
                                if (fname.ToLower().Equals("fzart"))
                                    setting.SearchParams[1].Pattern = vals[i];
                                if (fname.ToLower().Equals("sysrvt")&&vals[i]!=null&&vals[i].Length>0)
                                    sysrvt = long.Parse(vals[i]);
                                i++;
                            }
                        }
                    }

                    if (sysrvt > 0)
                    {
                        if (!obtypSearchFilter.ContainsKey(sysrvt))
                        {
                            Dictionary<int, List<long>> oblevelfilters = new Dictionary<int, List<long>>();
                            using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                            {
                                List<long> obtypfilters = ctx.ExecuteStoreQuery<long>("select sysabrregel from rvtpos,fstyp where rvtpos.sysfstyp=fstyp.sysfstyp and fstyp.bezeichnung='OBTYP' and sysrvt=" + sysrvt, null).ToList();
                                if (obtypfilters != null && obtypfilters.Count > 0)
                                {
                                    oblevelfilters[0] = obtypfilters;//remember all object ids
                                }
                            }
                            obtypSearchFilter[sysrvt] = oblevelfilters;
                        }
                    }

                    Cic.P000001.Common.TreeNode node = new Cic.P000001.Common.TreeNode();
                    node.Level = new Cic.P000001.Common.Level(3);

                    Cic.OpenOne.CarConfigurator.BO.DataProviderUtilities DataProviderUtilities = new Cic.OpenOne.CarConfigurator.BO.DataProviderUtilities();
                    Cic.P000001.Common.TreeNode[] nodes = DataProviderUtilities.GetTreeNodes(setting, node, Cic.P000001.Common.GetTreeNodeSearchModeConstants.NextLevel);
                    List<XproEntityDto> rval = new List<XproEntityDto>();
                    foreach (Cic.P000001.Common.TreeNode tn in nodes)
                    {
                        ObtypDto obt = new ObtypDto();
                        try
                        {
                            obt.baujahr = int.Parse(tn.data.baujahr);
                        }
                        catch (Exception) { }
                        try
                        {
                            obt.sysobtyp = long.Parse(tn.data.id);
                        }
                        catch (Exception) { }
                        
                        String[] path = tn.Key.Split('>');
                        List<long> pathids = Array.ConvertAll(path, long.Parse).ToList();
                        if (obtypSearchFilter.ContainsKey(sysrvt) && obtypSearchFilter[sysrvt][0].Intersect(pathids).Count() == 0 )
                        {
                           continue;
                        }


                        obt.bezeichnung = tn.data.bezeichnung;
                        obt.marke = tn.data.marke;
                        obt.modell = tn.data.modell;
                        obt.schwacke = tn.data.schwacke;
                        obt.neupreisbrutto = tn.data.neupreisbrutto;
                        obt.neupreisnetto = tn.data.neupreisnetto;
                        obt.leistung = tn.data.leistung;
                        XproEntityDto r = new XproEntityDto();
                        r.bezeichnung = obt.marke + " " + obt.bezeichnung;
                        r.beschreibung = r.bezeichnung;
                        r.code = ""+obt.sysobtyp;
                        r.sysID = obt.sysobtyp;
                        r.title = r.beschreibung;
                        r.desc1 = obt.modell;
                        r.desc2 = "Baujahr: " + obt.baujahr + " KW: " + obt.leistung;
                        r.desc3 = "Netto: " + obt.neupreisnetto + "EUR" + " ET:" + obt.schwacke;
                        rval.Add(r);
                    }
                    return rval.ToArray();

                }/*,
                CreateBezeichnung = (item) => item.marke + " " + item.bezeichnung,
                CreateBeschreibung = (item,rval) => createPanel(rval,"OBTYP", item.marke + " " + item.bezeichnung, item.modell, "Baujahr: " + item.baujahr + " KW: " + item.leistung, "Netto: " + item.neupreisnetto + "EUR" + " ET:" + item.schwacke, item.indicatorContent, "" + item.entityId),*/
            });
            #endregion

            String aidaFilterVT = AppConfig.Instance.getValueFromDb("AIDA", "FILTERS", "VT");
            if (aidaFilterVT != null && aidaFilterVT.Length > 0)
            {
                aidaFilterVT = " AND " + aidaFilterVT;
            }
            else aidaFilterVT = "";
            String aidaFilterANGEBOT = AppConfig.Instance.getValueFromDb("AIDA", "FILTERS", "ANGEBOT");
            if (aidaFilterANGEBOT != null && aidaFilterANGEBOT.Length > 0)
            {
                aidaFilterANGEBOT = " AND " + aidaFilterANGEBOT;
            }
            else aidaFilterANGEBOT = "";
            String aidaFilterANTRAG = AppConfig.Instance.getValueFromDb("AIDA", "FILTERS", "ANTRAG");
            if (aidaFilterANTRAG != null && aidaFilterANTRAG.Length > 0)
            {
                aidaFilterANTRAG = " AND " + aidaFilterANTRAG;
            }
            else aidaFilterANTRAG = "";
            dictionary.Remove(XproEntityType.CAMP);
            dictionary.Add(XproEntityType.CAMP, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select syscamp sysid, name code, name bezeichnung, description beschreibung from camp where syscamp=:id", par.ToArray()).FirstOrDefault();
                    }

                  
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        return ctx.ExecuteStoreQuery<DropListDto>("select syscamp sysid, name code, name bezeichnung, description beschreibung from camp where syscamp in (select distinct camp.syscamp from camp,oppo,vt where camp.status in (2,3) and camp.syscamp=oppo.syscamp and oppo.area='VT' and oppo.sysid=vt.sysid and vt.sysid in (SELECT sysid FROM peuni, perolecache WHERE area = 'VT' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = " + input.sysperole + ")) order by camp.name", null).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "CAMP", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });

            dictionary.Add(XproEntityType.ANGEBOTE, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select sysid, angebot code, angebot bezeichnung, angebot||' / '||it.name||' '||it.vorname||' / '||objektvt beschreibung  from angebot, it where angebot.sysit=it.sysit and angebot.sysid=:sysid", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    if (input.Filter == null || input.filters == null || input.filters.Length == 0)
                    {
                        return new DropListDto[0];
                    }
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        String sysit = "-1";
                        if (input.filters.Length > 0 && input.filters[0].fieldname.ToLower().IndexOf("sysit") > -1)
                        {
                            String filters = input.filters[0].value;
                            String type = filters.Split(',')[1];
                            sysit = filters.Split(',')[0];
                            if(!"ANGEBOT".Equals(type))
                                return new DropListDto[0];
                        }
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value = "%"+input.Filter+"%" });
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = sysit });
                        return ctx.ExecuteStoreQuery<DropListDto>("select sysid, angebot code, angebot bezeichnung, angebot||' / '||it.name||' '||it.vorname||' / '||objektvt beschreibung  from angebot, it where  angebot.sysit=it.sysit and (upper(objektvt) like upper(:filter) or upper(angebot) like upper(:filter) or upper(it.name||it.vorname) like (:filter)) and it.sysit=:sysit order by sysid desc", par.ToArray()).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "ANGEBOT", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });
            dictionary.Add(XproEntityType.ANTRAEGE, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select sysid, antrag code, antrag bezeichnung, antrag||' / '||trim(person.name)||' '||trim(person.vorname)||' / '||fabrikat beschreibung  from antrag, person where antrag.syskd=person.sysperson and antrag.sysid=:sysid", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    if (input.Filter == null || input.filters == null || input.filters.Length == 0)
                    {
                        return new DropListDto[0];
                    }
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        String sysperson = "-1";
                        if (input.filters.Length > 0 && input.filters[0].fieldname.ToLower().IndexOf("sysperson") > -1)
                        {
                            String filters = input.filters[0].value;
                            String type = filters.Split(',')[1];
                            sysperson = filters.Split(',')[0];
                            if (!"ANTRAG".Equals(type))
                                return new DropListDto[0];
                        }
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value = "%" + input.Filter + "%" });
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperson", Value = sysperson });
                        return ctx.ExecuteStoreQuery<DropListDto>("select sysid, antrag code, antrag bezeichnung, antrag||' / '||trim(person.name)||' '||trim(person.vorname)||' / '||fabrikat beschreibung  from antrag, person where antrag.syskd=person.sysperson and (upper(fabrikat) like upper(:filter) or upper(antrag) like upper(:filter) or upper(person.name||person.vorname) like (:filter)) and person.sysperson=:sysperson order by sysid desc", par.ToArray()).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "ANTRAG", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });
            dictionary.Remove(XproEntityType.CTLANG);
            dictionary.Add(XproEntityType.CTLANG, new XproInfoDao()
            {
                QueryItemFunction2 = (input) =>
                {
                    var bo = CreateDictionaryListsBo(input.isoCode);
                    return bo.listSprachen().FirstOrDefault((a) => a.sysID == input.EntityId);
                },
                QueryItemsFunction = (filter) =>
                {
                    var bo = CreateDictionaryListsBo(filter.isoCode);
                    return bo.listSprachen();
                },
            });


        }
    }
    class KalktypComparator<T> : IEqualityComparer<T> where T : Cic.OpenLease.ServiceAccess.DdOl.PRPRODUCTDto
    {
        public bool Equals(T p1, T p2)
        {
            return p1.SYSKALKTYP == p2.SYSKALKTYP;
        }

        public int GetHashCode(T p)
        {
            return (int)p.SYSKALKTYP;
        }
    }
}