using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.BO.Prisma;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.DTO.Prisma;

namespace Cic.OpenOne.Common.DAO.Prisma
{

    /// <summary>
    /// Prisma Data Access Object
    /// </summary>
    public class PrismaDao : IPrismaDao
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// IMPORTANT - when updating prproduct or other tables which are casted to EDMX-Entity, include ALL Fields in the Query!!!!!
        /// </summary>
        private const string QUERYPRODUCTS = "select SYSPRPRODUCT ,SYSVART ,SYSVARTTAB  ,SYSVTTYP  ,ACTUALTERM1 NAME   ,DESCRIPTION   ,prod.ACTIVEFLAG  ,VALIDFROM  ,VALIDUNTIL  ,SOURCEBASIS  ,SYSIBOR ,SYSINTSTRCT,SYSVG ,SYSPRRAP  ,SYSPRPRODTYPE ,SYSKALKTYP   ,SYSINTTYPE   ,NAMEINTERN ,TARIFCODE  ,SYSAKTION, SYSPRINTSETVF, SYSPRINTSETAF, SYSPRTLGSET, CODE,HASFF,HASAF,SECTOR  from prproduct prod, vc_ctlut trans , ctlang where ctlang.sysctlang=trans.sysctlang and trans.area='PRPRODUCT' and trans.sysid=prod.sysprproduct and isocode=:isocode";
        private const string QUERYNEWS = "select SYSPRNEWS ,CATCHWORDS,DATUM,UHRZEIT,VALIDFROM,VALIDUNTIL from prnews";

        private const string QUERYPRODUCTTYPES = "select * from prprodtype";
        private const string QUERYDIFF = "SELECT COUNT(1) FROM prclprsubvset WHERE sysprproduct=:prodId";

        private const string QUERYKUNDENZINS = "SELECT 0 visible, 0 sysprparset, -1 sysid,0 disabled,0 minvaln,0 maxvaln,0 defvaln,0 stepsize, '' steplistcsv, -1 sysprfld, intsrate.minrate minvalp, intsrate.maxrate maxvalp, intsrate.maxrate defvalp, 1 type, 'Kundenzins' name, 'KALK_BORDER_KUNDENZINS' meta " +
                                                " FROM intsrate " +
                                                " WHERE intsrate.sysintsdate = (SELECT intsdate.sysintsdate FROM intsdate WHERE intsdate.sysintstrct = " +
                                                " (SELECT intstrct.sysintstrct FROM intstrct, prproduct WHERE prproduct.sysprproduct=:prodId AND prproduct.sysintstrct   = intstrct.sysintstrct " +
                                                " and intstrct.method = 1) " +
                                                " AND intsdate.validfrom = (SELECT MAX(maxdate.validfrom) FROM intsdate maxdate WHERE maxdate.validfrom <= :currentdate and maxdate.sysintstrct = intsdate.sysintstrct  ) )";

        private const string QUERYKUNDENZINSEXT = "SELECT intsrate.intrate, intsrate.addrate, intsrate.redrate, intsrate.minrate, intsrate.maxrate " +
                                                "FROM intsrate  " +
                                                "WHERE intsrate.sysintsdate = (SELECT intsdate.sysintsdate FROM intsdate WHERE intsdate.sysintstrct = " +
                                                "(SELECT intstrct.sysintstrct FROM intstrct, prproduct WHERE prproduct.sysprproduct =:SysPROD AND prproduct.sysintstrct = intstrct.sysintstrct  " +
                                                "AND intstrct.method = 1)  " +
                                                "AND intsdate.validfrom = (SELECT MAX(maxdate.validfrom) FROM intsdate maxdate WHERE maxdate.validfrom <= :currentdate AND maxdate.sysintstrct = intsdate.sysintstrct))  " +
                                                "UNION  " +
                                                "SELECT intsband.intrate, intsband.addrate, intsband.redrate, intsband.minrate, intsband.maxrate  " +
                                                "FROM intsband  " +
                                                "WHERE intsband.sysintsdate = (SELECT intsdate.sysintsdate FROM intsdate WHERE intsdate.sysintstrct = " +
                                                "(SELECT intstrct.sysintstrct FROM intstrct, prproduct WHERE prproduct.sysprproduct=:SysPROD AND prproduct.sysintstrct = intstrct.sysintstrct  " +
                                                "AND intstrct.method = 3)  " +
                                                "AND intsdate.validfrom = (SELECT MAX(maxdate.validfrom) FROM intsdate maxdate WHERE maxdate.validfrom <= :currentdate and maxdate.sysintstrct = intsdate.sysintstrct))  " +
                                                "AND (intsband.lowerb <= :Saldo AND (intsband.upperb > :Saldo OR intsband.upperb =0)) " +
                                                "UNION  " +
                                                "SELECT intsmatu.intrate, intsmatu.addrate, intsmatu.redrate, intsmatu.minrate, intsmatu.maxrate  " +
                                                "FROM intsmatu  " +
                                                "WHERE intsmatu.sysintsdate = (SELECT intsdate.sysintsdate FROM intsdate WHERE intsdate.sysintstrct = " +
                                                "(SELECT intstrct.sysintstrct FROM intstrct, prproduct WHERE prproduct.sysprproduct= :SysPROD AND prproduct.sysintstrct = intstrct.sysintstrct " +
                                                "AND intstrct.method = 2) " +
                                                "AND intsdate.validfrom = (SELECT MAX(maxdate.validfrom) FROM intsdate maxdate WHERE maxdate.validfrom <= :currentdate AND maxdate.sysintstrct = intsdate.sysintstrct)) " +
                                                "AND intsmatu.maturity = (SELECT MAX(maxmatu.maturity) FROM intsmatu maxmatu WHERE maxmatu.maturity <= :pLZ AND maxmatu.sysintsdate = intsmatu.sysintsdate) ";

        private const string QUERYPRODUCTLINKS = "select * from {0} where ACTIVEFLAG=1";
        private const string QUERYPARAMS = "select prparam.sysprparset, prparam.sysprparam sysid, prfld.objectmeta meta, prparam.name, visibilityflag visible, disabledflag disabled, typ type, minvaln, maxvaln, defvaln, maxvalp, minvalp, defvalp, stepsize, steplist steplistcsv,prparam.sysprfld, prparam.exprnet,prparam.sysvg from prparam, prfld where prparam.sysprfld=prfld.sysprfld";

        private const string QUERYNEWSLINKS = "select * from {0} where ACTIVEFLAG=1";

        //all productparameter-sets from the parameterset-hierarchy without the parametersets with productlinks
        private const string QUERYPRODUCTPARSET = "select level,prparset.* from prparset left outer join prclparset on prclparset.sysprparset=prparset.sysprparset where sysprclparset is null and activeflag=1 connect by prior prparset.sysprparset=sysparent start with sysparent is null order by level";
        private const string QUERYPRODUCTPARSETCHILDREN = "select prparset.sysprparset from prparset where activeflag=1 connect by prior prparset.sysprparset=sysparent start with sysprparset=:sysprparset";

        //all productparameter links
        private const string QUERYPARAMLINKS = "SELECT prclparset.*,prparset.validfrom, prparset.validuntil,prparset.area areapset, prparset.sysid sysidpset FROM prclparset, prparset WHERE prclparset.sysprparset = prparset.sysprparset AND prparset.ActiveFlag = 1  order by rank desc, case prclparset.area when 99 then 1 when 0 then 1 when 1 then 2 when 3 then 3 when 2 then 4 else 5 end";

        private const string QUERYNEWSDATA = "select news.* from news,ctlang where ctlang.sysctlang=news.sysctlang and sysprnews=:sysprnews and isocode=:isocode";
        private const string QUERYNEWSDATAATT = "select newsatt.* from newsatt where sysnews=:sysnews";

        private const string QUERYPRFLDS = "select * from prfld";
        private const string QUERYVART = "select vart.* from vart,prproduct where prproduct.sysvart=vart.sysvart and prproduct.sysprproduct=:sysprproduct";
        private const string QUERYVTTYP = "select vttyp.* from vttyp,prproduct where prproduct.sysvttyp=vttyp.sysvttyp and prproduct.sysprproduct=:psysprproduct";
        private const string QUERYVTTYPBYID = "select vttyp.* from vttyp where sysvttyp=:psysvttyp";

        private const string QUERYBWVART = "select prbildweltv.*,vart.code from prbildweltv, vart where vart.sysvart=prbildweltv.sysvart";
        private const string QUERYBWVARTDEFAULT = "select vart.code code, bezeichnung, sysvart, prbildwelt.sysprbildwelt, 0 sysprbildweltv from vart,prbildwelt where prbildwelt.defaultflag=1";
        private static string QUERYBWVARTTRANS = "select ctlut.ACTUALTERM1 translation,isocode, sysprbildweltv frontid from prbildweltv,cic.vc_ctlut ctlut,ctlang lang where  ctlut.area='PRBILDWELTV' and ctlut.sysctlang = lang.sysctlang  and ctlut.sysid=prbildweltv.sysprbildweltv";
        private static string QUERYBWVARTDEFAULTTRANS = "select ctlut.ACTUALTERM1 translation,isocode, vart.sysvart frontid from vart,prbildwelt,cic.vc_ctlut ctlut,ctlang lang where prbildwelt.defaultflag=1 and ctlut.area='VART' and ctlut.sysctlang = lang.sysctlang  and ctlut.sysid=vart.sysvart";

        /// <summary>
        /// Standarad Constuctor
        /// Database access Object for Prisma Products and Parameters
        /// </summary>
        public PrismaDao()
        {
        }

        /// <summary>
        /// Get News Data
        /// </summary>
        /// <param name="sysprnews">News ID</param>
        /// <param name="isoCode">ISO Sprachen Code</param>
        /// <returns>NewsData</returns>
        public virtual List<NEWS> getNewsData(long sysprnews, String isoCode)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                object[] pars = { new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = isoCode } ,
                                  new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprnews", Value = sysprnews } 
                                    };
                return ctx.ExecuteStoreQuery<NEWS>(QUERYNEWSDATA, pars).ToList();
            }
        }

        /// <summary>
        /// Get News Data Attributes
        /// </summary>
        /// <param name="sysnews">NewsData ID</param>
        /// <returns>NewsData</returns>
        public virtual List<NEWSATT> getNewsAttributes(long sysnews)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                object[] pars = { new Devart.Data.Oracle.OracleParameter { ParameterName = "sysnews", Value = sysnews } };
                List<NEWSATT> rval = ctx.ExecuteStoreQuery<NEWSATT>(QUERYNEWSDATAATT, pars).ToList();
                return rval;
            }
        }

        /// <summary>
        /// Get Product
        /// </summary>
        /// <param name="sysprproduct">Product ID</param>
        /// <param name="isoCode">ISO Sprachen Code</param>
        /// <returns>Product Data</returns>
        public virtual PRPRODUCT getProduct(long sysprproduct, String isoCode)
        {
            return getProducts(isoCode).Where(p => p.SYSPRPRODUCT == sysprproduct).FirstOrDefault();
        }

        /// <summary>
        /// Get Vertragsart
        /// </summary>
        /// <param name="sysprproduct">ID</param>
        /// <returns>Vertragsart</returns>
        public virtual VART getVertragsart(long sysprproduct)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                object[] pars = { new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprproduct", Value = sysprproduct } };

                return ctx.ExecuteStoreQuery<VART>(QUERYVART, pars).FirstOrDefault();
            }
        }

        /// <summary>
        /// Get all News
        /// </summary>
        /// <returns>News</returns>
        public virtual List<PRNEWS> getNews()
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                return ctx.ExecuteStoreQuery<PRNEWS>(QUERYNEWS, null).ToList();
            }
        }

        /// <summary>
        /// Get News
        /// </summary>
        /// <param name="sysprnews">News ID</param>
        /// <returns>Product Data</returns>
        public virtual PRNEWS getNews(long sysprnews)
        {
            return getNews().Where(p => p.SYSPRNEWS == sysprnews).FirstOrDefault();
        }

        /// <summary>
        ///  returns all Prisma Parameters 
        /// </summary>
        /// <returns>Parameter list</returns>
        public virtual List<ParamDto> getParams()
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                return ctx.ExecuteStoreQuery<ParamDto>(QUERYPARAMS, null).ToList();
            }
        }

        /// <summary>
        /// returns all Prisma Products
        /// </summary>
        /// <returns>Product list</returns>
        public virtual List<PRPRODUCT> getProducts(String isoCode)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                object[] pars = { new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = isoCode } };

                return ctx.ExecuteStoreQuery<PRPRODUCT>(QUERYPRODUCTS, pars).ToList();
            }
        }

        /// <summary>
        /// returns all Product types
        /// </summary>
        /// <returns>Product List</returns>
        public virtual List<PRPRODTYPE> getProductTypes()
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                return ctx.ExecuteStoreQuery<PRPRODTYPE>(QUERYPRODUCTTYPES, null).ToList();
            }
        }

        /// <summary>
        /// returns all Product Condition Links
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns>Product Condition List</returns>
        public virtual List<ProductConditionLink> getProductConditionLinks(String tableName)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                // Security check: Aufruf nur mit Konstanten.
                return ctx.ExecuteStoreQuery<ProductConditionLink>(String.Format(QUERYPRODUCTLINKS, tableName), null).ToList();
            }
        }

        /// <summary>
        /// returns all Product Parameter Sets (Hierarchical) for Sets not linked to Products
        /// </summary>
        /// <returns>Parameter Condition List</returns>
        public virtual List<ParameterSetConditionLink> getParamSets()
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                return ctx.ExecuteStoreQuery<ParameterSetConditionLink>(QUERYPRODUCTPARSET, null).ToList();
            }
        }

        /// <summary>
        /// returns all Product Parameter Sets (Hierarchical) Children
        /// </summary>
        /// <returns>Parameter Condition List</returns>
        public virtual List<ParameterSetConditionLink> getParamSetChildren(long sysprparset)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                object[] pars = { new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprparset", Value = sysprparset } };

                return ctx.ExecuteStoreQuery<ParameterSetConditionLink>(QUERYPRODUCTPARSETCHILDREN, pars).ToList();
            }
        }

        /// <summary>
        /// returns all Product Parameter Sets linked to Products
        /// </summary>
        /// <returns>Parameter Condition Link List</returns>
        public virtual List<ParameterConditionLink> getParamConditionLinks()
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                return ctx.ExecuteStoreQuery<ParameterConditionLink>(QUERYPARAMLINKS, null).ToList();
            }
        }

        /// <summary>
        /// Get Prisma Fields
        /// </summary>
        /// <returns></returns>
        public virtual List<PRFLD> getFields()
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                return ctx.ExecuteStoreQuery<PRFLD>(QUERYPRFLDS, null).ToList();
            }
        }

        /// <summary>
        /// Returns true if product is difference Leasing
        /// </summary>
        /// <param name="sysPrProduct"></param>
        /// <returns></returns>
        public virtual bool isDiffLeasing(long sysPrProduct)
        {
            object[] pars = { new Devart.Data.Oracle.OracleParameter { ParameterName = "prodId", Value = sysPrProduct } };
            using (PrismaExtended ctx = new PrismaExtended())
            {
                return ctx.ExecuteStoreQuery<long>(QUERYDIFF, pars).FirstOrDefault() > 0 ? true : false;
            }
        }

        /// <summary>
        /// Returns the 'virtual' Prisma Parameter for Kundenzins, generated from the zinsstructure
        /// </summary>
        /// <param name="sysPrProduct"></param>
        /// <returns></returns>
        public virtual ParamDto getKundenzinsParam(long sysPrProduct)
        {
            object[] pars = { new Devart.Data.Oracle.OracleParameter { ParameterName = "prodId", Value = sysPrProduct },
                              new Devart.Data.Oracle.OracleParameter { ParameterName = "currentdate", Value = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null) }
                            };
            using (PrismaExtended ctx = new PrismaExtended())
            {
                return ctx.ExecuteStoreQuery<ParamDto>(QUERYKUNDENZINS, pars).FirstOrDefault();
            }
        }

        /// <summary>
        /// Returns the 'virtual' extended Prisma Parameter for Kundenzins, generated from the zinsstructure
        /// </summary>
        /// <param name="sysPrProduct">ProduktID</param>
        /// <param name="laufzeit">laufzeit</param>
        /// <param name="saldo">Saldo</param>
        /// <returns></returns>
        public virtual KundenzinsDto getKundenzins(long sysPrProduct, long laufzeit, double saldo)
        {
            object[] pars = { new Devart.Data.Oracle.OracleParameter { ParameterName = "SysPROD", Value = sysPrProduct },
                              new Devart.Data.Oracle.OracleParameter { ParameterName = "pLZ", Value = laufzeit },
                              new Devart.Data.Oracle.OracleParameter { ParameterName = "Saldo", Value = saldo },
                              new Devart.Data.Oracle.OracleParameter { ParameterName = "currentdate", Value = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null) }
                            };
            using (PrismaExtended ctx = new PrismaExtended())
            {
                List<KundenzinsDto> Eintraege = ctx.ExecuteStoreQuery<KundenzinsDto>(QUERYKUNDENZINSEXT, pars).ToList();
                return Eintraege.FirstOrDefault();
            }
        }

        /// <summary>
        /// returns all News Condition Links
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns>Product Condition List</returns>
        public virtual List<NewsConditionLink> getNewsConditionLinks(String tableName)
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                // Security check: Aufruf nur mit Konstanten.
                return ctx.ExecuteStoreQuery<NewsConditionLink>(String.Format(QUERYNEWSLINKS, tableName), null).ToList();
            }
        }

        /// <summary>
        /// Delivers Vertragsarten for different bildwelten
        /// </summary>
        /// <returns></returns>
        public virtual List<PrBildweltVDto> getBildweltVertragsarten()
        {
            using (PrismaExtended ctx = new PrismaExtended())
            {
                List<PrBildweltVDto> rval = ctx.ExecuteStoreQuery<PrBildweltVDto>(QUERYBWVARTDEFAULT, null).ToList();
                List<TranslationValue> trans = ctx.ExecuteStoreQuery<TranslationValue>(QUERYBWVARTDEFAULTTRANS, null).ToList();
                foreach (PrBildweltVDto bw in rval)
                {
                    TranslationDto tDto = new TranslationDto();
                    tDto.frontId = bw.sysvart.ToString();
                    tDto.master = bw.bezeichnung;
                    tDto.translations = (from t in trans
                                         where t.frontId.Equals(tDto.frontId)
                                         select t).ToList();
                    bw.translation = tDto;
                }

                List<PrBildweltVDto> rval2 = ctx.ExecuteStoreQuery<PrBildweltVDto>(QUERYBWVART, null).ToList();
                trans = ctx.ExecuteStoreQuery<TranslationValue>(QUERYBWVARTTRANS, null).ToList();
                foreach (PrBildweltVDto bw in rval2)
                {
                    TranslationDto tDto = new TranslationDto();
                    tDto.frontId = bw.sysprbildweltv.ToString();
                    tDto.master = bw.bezeichnung;
                    tDto.translations = (from t in trans
                                         where t.frontId.Equals(tDto.frontId)
                                         select t).ToList();
                    bw.translation = tDto;
                }

                rval.AddRange(rval2);
                return rval;
            }

        }
        
        /// <summary>
        /// delivers the vttyp
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <returns></returns>
        public virtual VTTYP getVttyp(long sysprproduct)
        {
            
            using (PrismaExtended ctx = new PrismaExtended())
            {
                object[] pars = { new Devart.Data.Oracle.OracleParameter { ParameterName = "psysprproduct", Value = sysprproduct } };

                return ctx.ExecuteStoreQuery<VTTYP>(QUERYVTTYP, pars).FirstOrDefault();
            }

        }

        /// <summary>
        /// Delivers the vttyp by sysvttyp
        /// </summary>
        /// <param name="sysvttyp"></param>
        /// <returns></returns>
        public virtual VTTYP getVttypById(long sysvttyp)
        {

            using (PrismaExtended ctx = new PrismaExtended())
            {
                object[] pars = { new Devart.Data.Oracle.OracleParameter { ParameterName = "psysvttyp", Value = sysvttyp } };

                return ctx.ExecuteStoreQuery<VTTYP>(QUERYVTTYPBYID, pars).FirstOrDefault();
            }

        }

        public long getObjtyp1ID(string fzart) {
            long obtyp1ID = 0;
            using (PrismaExtended ctx = new PrismaExtended())
            {
                string query = "select id1 from vc_obtyp1 where art = :art";
                List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();

                pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "art", Value = fzart });

                obtyp1ID = ctx.ExecuteStoreQuery<long>(query, pars.ToArray()).FirstOrDefault();
            }
            return obtyp1ID;
        }
    }
}