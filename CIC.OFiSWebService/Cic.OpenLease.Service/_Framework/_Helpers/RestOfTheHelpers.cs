
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Util;
using CIC.Database.OL.EF6.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenLease.Service
{
    public class RestOfTheHelpers
    {
        public static string DURCHSCHNITT = "Durchschnitt";
        public static string GetSchwacke(DdOlExtended context, long sysObTyp)
        {
            string Schwacke = string.Empty;

            OBTYP OBTYP = MyGetObTyp(context, sysObTyp);

            if (OBTYP == null)
            {
                throw new Exception("OBTYP is null");
            }

            while (OBTYP.SYSOBTYPP != null)
            {
                Schwacke = MyGetSchwacke(context, OBTYP.SYSOBTYPP.GetValueOrDefault());
                if (Schwacke == "10" || Schwacke == "20" || Schwacke == "40")
                {
                    return Schwacke;
                }

                OBTYP = MyGetObTyp(context, OBTYP.SYSOBTYPP.GetValueOrDefault());
            }

            return Schwacke;
        }
        private static OBTYP MyGetObTyp(DdOlExtended context, long sysObTyp)
        {
            OBTYP OBTYP;
            var Query = from obtyp in context.OBTYP
                        where obtyp.SYSOBTYP == sysObTyp
                        select obtyp;

            OBTYP = Query.FirstOrDefault();

            return OBTYP;
        }
        private static string MyGetSchwacke(DdOlExtended context, long sysObTyp)
        {
            OBTYP OBTYP;
            var Query = from obtyp in context.OBTYP
                        where obtyp.SYSOBTYP == sysObTyp
                        select obtyp;

            OBTYP = Query.FirstOrDefault();

            return OBTYP.SCHWACKE;
        }

        public static Dictionary<string, decimal> GetTiresFromCodeAndBauartAVG(DdOlExtended context, string code, string bauart)
        {
            Dictionary<string, decimal> HerstelleNettoAvg = new Dictionary<string, decimal>();

            code = code.ToUpper();
            bauart = bauart.ToUpper();
            code = code.Replace("R ", "R");




            String queryavgcode = "select hersteller Name,avg(netto) Average from reiftyp where code='" + code + "' and bauart='" + bauart + "' group by hersteller";
            String queryavgbauart = "select hersteller Name,avg(netto) Average from reiftyp where bauart='" + bauart + "' group by hersteller";
            String queryallcode = "select avg(netto) from reiftyp where code='" + code + "' and bauart='" + bauart + "' ";
            String queryallbauart = "select avg(netto) from reiftyp where bauart='" + bauart + "' ";
            String query1 = queryavgcode;
            String query2 = queryallcode;
            if (code.Equals(""))
            {
                query1 = queryavgbauart;
                query2 = queryallbauart;
            }
            var Query = context.ExecuteStoreQuery<AvgDto>(query1, null).AsQueryable();

            /* System.Collections.Generic.List<REIFTYP> ReiftypCalculatedList = new System.Collections.Generic.List<REIFTYP>();
             var Query = from reiftyp in context.REIFTYP
                         where reiftyp.CODE.ToUpper() == code.ToUpper() && reiftyp.BAUART.ToUpper() == bauart.ToUpper()
                         group reiftyp by reiftyp.HERSTELLER into result
                         select new
                         {
                             Name = result.Key,
                             Average = result.Average(i => i.NETTO)
                         };

             if (code.Equals(""))
             {
                 Query = from reiftyp in context.REIFTYP
                         where reiftyp.BAUART.ToUpper() == bauart.ToUpper()
                         group reiftyp by reiftyp.HERSTELLER into result
                         select new
                         {
                             Name = result.Key,
                             Average = result.Average(i => i.NETTO)
                         };
             }
             var Query2 = from reiftyp in context.REIFTYP
                          where reiftyp.CODE.ToUpper() == code.ToUpper() && reiftyp.BAUART.ToUpper() == bauart.ToUpper()
                          select reiftyp.NETTO;
             if (code.Equals(""))
             {
                 Query2 = from reiftyp in context.REIFTYP
                          where reiftyp.BAUART.ToUpper() == bauart.ToUpper()
                          select reiftyp.NETTO;
             }*/
            HerstelleNettoAvg[DURCHSCHNITT] = 0;

            foreach (var q in Query)
            {
                HerstelleNettoAvg[q.Name] = q.Average;

            }
            try
            {
                if (HerstelleNettoAvg.Keys.Count > 0)
                    HerstelleNettoAvg[DURCHSCHNITT] = context.ExecuteStoreQuery<decimal>(query2, null).FirstOrDefault();
            }
            catch
            {
                //no average available
            }

            return HerstelleNettoAvg;
        }
        public static int GetDiameterFromCode(string code)
        {
            int Diameter = 0;
            string ParsedCode = code.ToUpper().Trim();
            int StartIndex = ParsedCode.IndexOf("R") + 2;
            int Length = ParsedCode.Length - (ParsedCode.IndexOf("R") + 2);

            if (Length < 0)
            {
                Length = Length * -1;
            }
            try
            {
                if (StartIndex + Length <= ParsedCode.Length)
                {
                    ParsedCode = ParsedCode.Substring(StartIndex, Length);
                    int.TryParse(ParsedCode, out Diameter);
                }
            }
            catch
            {
                throw;
            }

            return Diameter;
        }

        public static List<PLZ> SearchPlz(DdOlExtended context, string plz)
        {
            List<PLZ> PLZList = new List<PLZ>();

            var Query = from plzrow in context.PLZ
                        where plzrow.PLZ1.StartsWith(plz)
                        orderby plzrow.ORT
                        select plzrow;

            PLZList = Query.ToList();

            return PLZList;
        }
        public static decimal GetWidth(long? sysFelgtyp)
        {
            decimal Width;

            using (DdOlExtended Context = new DdOlExtended())
            {
                try
                {
                    var Query = from felgtyp in Context.FELGTYP
                                where felgtyp.SYSFELGTYP == sysFelgtyp
                                select felgtyp.BREITE;
                    Width = Query.FirstOrDefault().GetValueOrDefault();
                }
                catch
                {
                    throw;
                }

            }

            return Width;
        }
        public static decimal GetDiameter(long? sysFelgtyp)
        {
            decimal Width;

            using (DdOlExtended Context = new DdOlExtended())
            {
                try
                {
                    var Query = from felgtyp in Context.FELGTYP
                                where felgtyp.SYSFELGTYP == sysFelgtyp
                                select felgtyp.DURCHMESSER;
                    Width = Query.FirstOrDefault().GetValueOrDefault();
                }
                catch
                {
                    throw;
                }

            }

            return Width;
        }

        public static bool GetDisabled(DdOwExtended context)
        {
            int? disabled = context.ExecuteStoreQuery<int>("select disabled from wfsys").FirstOrDefault();
            

            if (disabled == null)
            {
                throw new System.Exception("Disabled is null");
            }
            return (disabled == 1);
        }
        public static OBKAT[] SearchKatDescription(DdOlExtended context, string description)
        {
            OBKAT[] OBKAT;

            // if description is null search all
            if (description == null || description == string.Empty)
            {
                var Query = from obKat in context.OBKAT
                            orderby obKat.SYSOBKAT descending
                            select obKat;
                OBKAT = Query.ToArray<OBKAT>();
            }
            else
            {
                var Query = from obKat in context.OBKAT
                            where obKat.DESCRIPTION == description
                            orderby obKat.SYSOBKAT descending
                            select obKat;
                OBKAT = Query.ToArray<OBKAT>();
            }

            return OBKAT;
        }

        public static OBTYP[] SearchTypDescription(DdOlExtended context, string description, int skip, int top)
        {
            try
            {
                //TODO WB, if Dictionary will change add IMPORTSOURCE field
                OBTYP[] OBTYP;

                // Check if description is empty
                if (StringUtil.IsTrimedNullOrEmpty(description))
                {
                    // Query OBTYP
                    var Query = from obTyp in context.OBTYP
                                where obTyp.IMPORTSOURCE == 2
                                select obTyp;

                    // Get the rows
                    OBTYP = Query.Distinct().OrderByDescending(Typ => Typ.SYSOBTYP).Skip(skip).Take(top).ToArray<OBTYP>();
                }
                else
                {
                    // Query OBTYP
                    var Query = from obTyp in context.OBTYP
                                where obTyp.BEZEICHNUNG == description && obTyp.IMPORTSOURCE == 2
                                select obTyp;

                    // Get the rows
                    OBTYP = Query.Distinct().OrderByDescending(Typ => Typ.SYSOBTYP).Skip(skip).Take(top).ToArray<OBTYP>();
                }

                // Return the OBTYP array
                return OBTYP;
            }
            catch (Exception e)
            {
                // Throw an exception
                throw new ApplicationException("SearchDescription() failed", e);
            }
        }
        public static List<BLZ> SearchBankname(DdOlExtended context, string blz, string bic)
        {
            List<BLZ> BLZList = new List<BLZ>();

            // Set null if empty
            if (blz == null || blz.Trim() == string.Empty)
            {
                blz = null;
            }

            // Set null if empty
            if (bic == null || bic.Trim() == string.Empty)
            {
                bic = null;
            }

            var Query = from blzrow in context.BLZ
                        where ((blz != null ? blzrow.BLZ1.StartsWith(blz) : true) && (bic != null ? blzrow.BIC.StartsWith(bic) : true))
                        orderby blzrow.NAME
                        select blzrow;

            BLZList = Query.ToList();

            return BLZList;
        }
        public static  INTTYPE[] SearchInttypes(DdOlExtended context, long sysPrProdukt)
        {

            var Query = from inttype in context.INTTYPE
                        orderby inttype.INTTYPE1
                        select inttype;

            List<INTTYPE> rval = Query.ToList<INTTYPE>(); ;
            List<INTTYPE> result = new List<INTTYPE>();

            //either some are configured or all are used
            var Query2 = from p in context.PRCLPRINTTYPE
                         where p.PRPRODUCT.SYSPRPRODUCT == sysPrProdukt
                         select p;
            bool all = true;
            if (Query2.Count() > 0)
            {
                all = false;
            }
            foreach (INTTYPE it in rval)
            {
                if (all)
                {
                    result.Add(it);
                }
                else
                {
                    var Query3 = from p in context.PRCLPRINTTYPE
                                 where it.SYSINTTYPE == p.INTTYPE.SYSINTTYPE
                                        && p.PRPRODUCT.SYSPRPRODUCT == sysPrProdukt
                                 select p.PRPRODUCT.INTTYPE.SYSINTTYPE;
                    if (Query3.Count() != 0)
                    {
                        result.Add(it);
                    }
                }


            }
            return result.ToArray<INTTYPE>();
        }
        public static string GetDisabledReason(DdOwExtended context)
        {
            string disabledreason = context.ExecuteStoreQuery<String>("select DISABLEDREASON from wfsys").FirstOrDefault();
            

            

            if (disabledreason == null)
            {
                throw new System.Exception("Disabled is null");
            }
            return disabledreason;
        }

        public static string GetTyreCodeWithR(string width, string crosssec, string diameter)
        {

            if (width.Contains("."))
            {
                width = width.Substring(0, width.LastIndexOf("."));
            }

            if (crosssec.Contains("."))
            {
                crosssec = crosssec.Substring(0, crosssec.LastIndexOf("."));
            }

            if (diameter.Contains("."))
            {
                diameter = diameter.Substring(0, diameter.LastIndexOf("."));
            }

            string Code = width + "/" + crosssec + " " + "R" + " " + diameter;
            return Code;
        }
        public static string GetTyreCode(CIC.Database.ET.EF6.Model.ETGTYRES etgtyres)
        {
            string Width = etgtyres.WIDTH;
            string Crosssec = etgtyres.CROSSSEC;
            string Design = etgtyres.DESIGN;
            string Diameter = etgtyres.DIAMETER;

            if (Width.Contains("."))
            {
                Width = Width.Substring(0, Width.LastIndexOf("."));
            }

            if (Crosssec.Contains("."))
            {
                Crosssec = Crosssec.Substring(0, Crosssec.LastIndexOf("."));
            }

            if (Diameter.Contains("."))
            {
                Diameter = Diameter.Substring(0, Diameter.LastIndexOf("."));
            }

            string Code = Width + "/" + Crosssec + " " + Design + " " + Diameter;
            return Code;
        }

        public static int DeliverpTypPUser(DdOlExtended context)
        {
            int pTypPUser = 0;

            var Query = from cicconf in context.CICCONF
                        select cicconf;

            try
            {
                CICCONF Cicconf = Query.FirstOrDefault<CICCONF>();

                if (Cicconf != null)
                {
                    pTypPUser = Cicconf.PTYPPUSER.GetValueOrDefault();
                }
            }
            catch
            {
                throw;
            }

            return pTypPUser;
        }



        public static bool ContainsCTLANG(DdOwExtended owExtendedEntities, long sysCTLANG)
        {
            // Check context
            if (owExtendedEntities == null)
            {
                throw new System.ArgumentException("owExtendedEntities");
            }

            return owExtendedEntities.CTLANG.Where(par => par.SYSCTLANG == sysCTLANG).Any<CIC.Database.OW.EF6.Model.CTLANG>();
        }

        public static bool ContainsIT(DdOlExtended olExtendedEntities, long sysIT)
        {
            // Check context
            if (olExtendedEntities == null)
            {
                throw new System.ArgumentException("owExtendedEntities");
            }

            return olExtendedEntities.IT.Where(par => par.SYSIT == sysIT).Any<IT>();
        }
        public static bool ContainsSTAAT(DdOlExtended olExtendedEntities, long sysSTAAT)
        {
            // Check context
            if (olExtendedEntities == null)
            {
                throw new System.ArgumentException("olExtendedEntities");
            }

            return olExtendedEntities.STAAT.Where(par => par.SYSSTAAT == sysSTAAT).Any<STAAT>();
        }
        public static bool ContainsLAND(DdOlExtended olExtendedEntities, long sysLAND)
        {
            // Check context
            if (olExtendedEntities == null)
            {
                throw new System.ArgumentException("olExtendedEntities");
            }

            return olExtendedEntities.LAND.Where(par => par.SYSLAND == sysLAND).Any<LAND>();
        }

    }
    class AvgDto
    {
        public String Name { get; set; }
        public decimal Average { get; set; }
    }
}