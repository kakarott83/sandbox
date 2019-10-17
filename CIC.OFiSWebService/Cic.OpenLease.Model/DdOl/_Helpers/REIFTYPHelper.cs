namespace Cic.OpenLease.Model.DdOl
{
    #region Using
    using System.Linq;
    using System.Collections.Generic;
    using System;
    #endregion

    [System.CLSCompliant(true)]
    public static class REIFTYPHelper
    {
        public static string DURCHSCHNITT = "Durchschnitt";

        public static REIFTYP GetReiftypWithAveragePrice(System.Collections.Generic.List<REIFTYP> reiftypList)
        {
            REIFTYP REIFTYP = new REIFTYP();
            decimal Sum, Count;

            //Check if list is not null
            if ((reiftypList != null) && (reiftypList.Count > 0))
            {
                Sum = 0;
                Count = reiftypList.Count;

                //Count sum of prices
                foreach (REIFTYP REIFTYPLoop in reiftypList)
                {
                    Sum += REIFTYPLoop.NETTO.GetValueOrDefault();
                }

                //Calculate average price
                REIFTYP.NETTO = RoundingFacade.getInstance().RoundPrice(Sum / Count);

            }

            return REIFTYP;


        }

        public static System.Collections.Generic.List<string> GetTires(OlExtendedEntities context, string bauart)
        {
            //Get reiftyp
            var Query = from reiftyp in context.REIFTYP
                        where reiftyp.BAUART.ToUpper() == bauart.ToUpper()
                        select reiftyp.CODE;


            return Query.Distinct().ToList();
        }

        public static System.Collections.Generic.List<REIFTYP> GetTiresFromCode(OlExtendedEntities context, string code)
        {
            //Get reiftyp
            var Query = from reiftyp in context.REIFTYP
                        where reiftyp.CODE.ToUpper() == code.ToUpper()
                        select reiftyp;


            return GetTiresFromCodeWithAveragePrices(context, Query.ToList(), code);
        }
        //TODO WB 0 WB, Implement
        public static System.Collections.Generic.List<REIFTYP> GetTiresFromCodeWithAveragePrices(OlExtendedEntities context, System.Collections.Generic.List<REIFTYP> reiftypList, string code)
        {
            System.Collections.Generic.List<REIFTYP> ReiftypCalculatedList = new System.Collections.Generic.List<REIFTYP>();
            System.Collections.Generic.List<string> HerstellerList = null;
            int Count;
            decimal AveragePrice;

            REIFTYP REIFTYPAverage = null;

            var Query = from reiftyp in reiftypList
                        select reiftyp.HERSTELLER;

            HerstellerList = Query.Distinct().ToList<string>();
            foreach (string REIFTYPHerstellerLoop in HerstellerList)
            {
                AveragePrice = 0;
                Count = 0;

                var QueryGetAverage = from reiftyp in reiftypList
                                      where reiftyp.HERSTELLER == REIFTYPHerstellerLoop
                                      select reiftyp;
                REIFTYPAverage = new REIFTYP();
                foreach (REIFTYP REIFTYPPricesLoop in QueryGetAverage)
                {
                    Count++;
                    AveragePrice += REIFTYPPricesLoop.NETTO.GetValueOrDefault();
                }

                if (Count > 0)
                {
                    REIFTYPAverage.NETTO = RoundingFacade.getInstance().RoundPrice(AveragePrice / Count);
                    REIFTYPAverage.CODE = code;
                    REIFTYPAverage.HERSTELLER = REIFTYPHerstellerLoop;
                    ReiftypCalculatedList.Add(REIFTYPAverage);
                }

            }
            return ReiftypCalculatedList;
        }

        public static REIFTYP GetTire(OlExtendedEntities context, string code)
        {
            var Query = from reiftyp in context.REIFTYP
                        where reiftyp.CODE.ToUpper() == code.ToUpper()
                        select reiftyp;

            return Query.FirstOrDefault<REIFTYP>();
        }

        public static System.Collections.Generic.List<REIFTYP> GetTiresFromCodeAndBauart(OlExtendedEntities context, string code, string bauart)
        {
            System.Collections.Generic.List<REIFTYP> ReiftypCalculatedList = new System.Collections.Generic.List<REIFTYP>();
            var Query = from reiftyp in context.REIFTYP
                        where reiftyp.CODE.ToUpper() == code.ToUpper() && reiftyp.BAUART.ToUpper() == bauart.ToUpper()
                        select reiftyp;



            return Query.ToList();
        }


        public static Dictionary<string, decimal> GetTiresFromCodeAndBauartAVG(OlExtendedEntities context, string code, string bauart)
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


    }
    class AvgDto
    {
        public String Name { get; set; }
        public decimal Average { get; set; }
    }
}
