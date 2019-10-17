using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.OpenLease.Model.DdOl;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Collection;

namespace Cic.One.GateWKT.BO
{
    /// <summary>
    /// BO for Managing Tires (Eurotax) and Rims
    /// Used for Dropdowns for Sizes/Dimensions and AVG and Manufacturer infos
    /// 
    /// </summary>
    public class TireBO
    {
        private const string CnstNebenKostenPriceFsArt = "Zusatzkosten";
        private const string CnstSummerTire = "S";
        private const string CnstWinterTire = "W";
        public static string DURCHSCHNITT = "Durchschnitt";
        

        private static long CACHE_TIMEOUT = 1000 * 60 * 60 * 8;
        private static CacheDictionary<String, decimal[]> nebenkostenCache = CacheFactory<String, decimal[]>.getInstance().createCache(CACHE_TIMEOUT, CacheCategory.Data);
        private static CacheDictionary<String, List<TireRimSize>> etTireCache = CacheFactory<String, List<TireRimSize>>.getInstance().createCache(CACHE_TIMEOUT, CacheCategory.Data);
        private static CacheDictionary<String, TireDto[]> priceCache = CacheFactory<String, TireDto[]>.getInstance().createCache(CACHE_TIMEOUT, CacheCategory.Data);
        private static CacheDictionary<String, RimDto[]> rimCache = CacheFactory<String, RimDto[]>.getInstance().createCache(CACHE_TIMEOUT, CacheCategory.Data);

        /// <summary>
        /// fet
        /// </summary>
        /// <returns></returns>
        public static decimal[] GetNebenKostenPriceParameters()
        {
            if (!nebenkostenCache.ContainsKey("NK"))
            {
                using (DdOlExtended ctx = new DdOlExtended())
                {
                    DateTime dt = ctx.ExecuteStoreQuery<DateTime>("select max(fspreistab.gueltigab) from fspreistab,fspreis,fstyp, fsart where fspreistab.sysfspreis=fspreis.sysfspreis and fspreis.sysfstyp=fstyp.sysfstyp and fstyp.sysfsart=fsart.sysfsart and fsart. beschreibung='" + CnstNebenKostenPriceFsArt + "' and (fspreistab.gueltigab<=sysdate) order by fspreistab.gueltigab desc", null).FirstOrDefault();

                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "perDate", Value = dt });

                    nebenkostenCache["NK"] = ctx.ExecuteStoreQuery<decimal>("select fspreistab.preis from fspreistab,fspreis,fstyp, fsart where fspreistab.sysfspreis=fspreis.sysfspreis and fspreis.sysfstyp=fstyp.sysfstyp and fstyp.sysfsart=fsart.sysfsart and fsart. beschreibung='Zusatzkosten' and (fspreistab.gueltigab<=:perDate) order by fspreistab.preis", parameters.ToArray()).ToArray();

                }
            }
            return nebenkostenCache["NK"];
        }

        /// <summary>
        /// returns all tireinformation for the vehicle
        /// if no tirecodes given, will use the first available type for initialization
        /// </summary>
        /// <param name="eurotaxNr"></param>
        /// <param name="reifencodeVorne"></param>
        /// <param name="reifencodeHinten"></param>
        /// <param name="reifencodeVorneSommer"></param>
        /// <param name="reifencodeHintenSommer"></param>
        /// <param name="felgenCodeVorne"></param>
        /// <param name="felgenCodeHinten"></param>
        /// <returns></returns>
        public TireInfoDto getTireData(String eurotaxNr, String reifencodeVorne, String reifencodeHinten, String reifencodeVorneSommer, String reifencodeHintenSommer, String felgenCodeVorne, String felgenCodeHinten)
        {
            decimal ust = 0;

            TireInfoDto rval = new TireInfoDto();
            //rval.tiresFsPrices = GetNebenKostenPriceParameters();

            //Spalte Dimensionen für Reifen vorne/hinten, Felgen vorne/hinten
            rval.eurotaxTires = getEurotaxTires(eurotaxNr, ref reifencodeVorne, ref reifencodeHinten);

            //fix all tire dimension strings
            reifencodeVorne = reifencodeVorne.Replace("R1", "R 1");
            reifencodeVorne = reifencodeVorne.Replace("R2", "R 2");

            reifencodeHinten = reifencodeHinten.Replace("R1", "R 1");
            reifencodeHinten = reifencodeHinten.Replace("R2", "R 2");

            if (felgenCodeVorne == null)
                felgenCodeVorne = reifencodeVorne;
            if (felgenCodeHinten == null)
                felgenCodeHinten = reifencodeHinten;

            felgenCodeVorne = felgenCodeVorne.Replace("R1", "R 1");
            felgenCodeVorne = felgenCodeVorne.Replace("R2", "R 2");

            felgenCodeHinten = felgenCodeHinten.Replace("R1", "R 1");
            felgenCodeHinten = felgenCodeHinten.Replace("R2", "R 2");

            if (reifencodeVorneSommer != null)
            {
                reifencodeVorneSommer = reifencodeVorneSommer.Replace("R1", "R 1");
                reifencodeVorneSommer = reifencodeVorneSommer.Replace("R2", "R 2");
            }
            if (reifencodeHintenSommer != null)
            {
                reifencodeHintenSommer = reifencodeHintenSommer.Replace("R1", "R 1");
                reifencodeHintenSommer = reifencodeHintenSommer.Replace("R2", "R 2");
            }
            if (reifencodeVorneSommer == null || reifencodeVorneSommer.Length == 0)
                reifencodeVorneSommer = reifencodeVorne;
            if (reifencodeHintenSommer == null || reifencodeHintenSommer.Length == 0)
                reifencodeHintenSommer = reifencodeHinten;
            //-------------------------------------------------


            //Spalte Marke/Durchschnitt für aktuell gewählte Größe - Felgen
            rval.frontRims = getRims(felgenCodeVorne);
            rval.rearRims = getRims(felgenCodeHinten);

            rval.frontPricesSummer = getTiresPricesFromBauart(reifencodeVorneSommer, CnstSummerTire, ust);
            rval.rearPricesSummer = getTiresPricesFromBauart(reifencodeHintenSommer, CnstSummerTire, ust);

            rval.frontPricesWinter = getTiresPricesFromBauart(reifencodeVorne, CnstWinterTire, ust);
            rval.rearPricesWinter = getTiresPricesFromBauart(reifencodeHinten, CnstWinterTire, ust);

            rval.reifencodeHinten = reifencodeHinten;
            rval.reifencodeVorne = reifencodeVorne;
            rval.reifencodeVorneSommer = reifencodeVorneSommer;
            rval.reifencodeHintenSommer = reifencodeHintenSommer;

            return rval;
        }

        /// <summary>
        /// fetches all front tires from Eurotax
        /// </summary>
        /// <param name="natCode"></param>
        /// <returns></returns>
        private static List<TireRimSize> GetFrontTires(string natCode)
        {
            String key = "FT" + natCode;
            if (!etTireCache.ContainsKey(key))
            {
                using (DdOlExtended ctx = new DdOlExtended())
                {
                    string query = @"select width,crosssec,diameter from 
(
select * from (
select etgtyres.diameter,etgtyres.crosssec,etgtyres.width from etgtyres,etgjwheel where length(crosssec)>1 and etgjwheel.natcode='" + natCode + @"' and etgjwheel.tyrtyrefcd=etgtyres.id
group by etgtyres.diameter,etgtyres.crosssec,etgtyres.width order by etgtyres.diameter,etgtyres.crosssec,etgtyres.width
)
union all
select * from (
select TYRESFrontADD.diameter,TYRESFrontADD.crosssec,TYRESFrontADD.width
            from etgtype TYP
		              INNER JOIN ETGADDITION ADDITION ON ADDITION.VehType = Typ.VehType AND ADDITION.NatCode = Typ.NatCode
                  INNER JOIN ETGEQTEXT EQTEXT ON EQTEXT.VehType = ADDITION.VehType AND EQTEXT.EQCode = ADDITION.EQCode
			            INNER JOIN ETGEQJWheel WheelADD ON WheelADD.VehType = EQTEXT.VehType AND EQTEXT.EQCode = WheelADD.EQTCode
                  INNER JOIN ETGTYRES TYRESFrontADD ON TYRESFrontADD.ID = WheelADD.TYRTyreFCd AND TYRESFRONTADD.VehType = WheelADD.VehType
            where length(crosssec)>1 and typ.NATCODE ='" + natCode + @"'           
 group by TYRESFrontADD.diameter,TYRESFrontADD.crosssec,TYRESFrontADD.width order by TYRESFrontADD.diameter,TYRESFrontADD.CROSSSEC,TYRESFrontADD.width
 ))";

                    etTireCache[key] = ctx.ExecuteStoreQuery<TireRimSize>(query, null).ToList<TireRimSize>();
                }
            }
            return etTireCache[key];

        }

        /// <summary>
        /// fetches all rear tires from Eurotax
        /// </summary>
        /// <param name="natCode"></param>
        /// <returns></returns>
        private static List<TireRimSize> GetRearTires(String natCode)
        {
            String key = "RT" + natCode;
            if (!etTireCache.ContainsKey(key))
            {
                using (DdOlExtended ctx = new DdOlExtended())
                {
                    string query = @"select width,crosssec,diameter from 
(
select * from (
select etgtyres.diameter,etgtyres.crosssec,etgtyres.width from etgtyres,etgjwheel where length(crosssec)>1 and etgjwheel.natcode='" + natCode + @"'   and etgjwheel.tyrtyrercd=etgtyres.id
group by etgtyres.diameter,etgtyres.crosssec,etgtyres.width order by etgtyres.diameter,etgtyres.crosssec,etgtyres.width
)
union all
select * from (
select TYRESBackADD.diameter,TYRESBackADD.crosssec,TYRESBackADD.width
            from etgtype TYP
		              INNER JOIN ETGADDITION ADDITION ON ADDITION.VehType = Typ.VehType AND ADDITION.NatCode = Typ.NatCode
                  INNER JOIN ETGEQTEXT EQTEXT ON EQTEXT.VehType = ADDITION.VehType AND EQTEXT.EQCode = ADDITION.EQCode
			            INNER JOIN ETGEQJWheel WheelADD ON WheelADD.VehType = EQTEXT.VehType AND EQTEXT.EQCode = WheelADD.EQTCode
                  INNER JOIN ETGTYRES TYRESBackADD ON TYRESBackADD.ID = WheelADD.TYRTyreRCd AND TYRESBackADD.VehType = WheelADD.VehType
            where length(crosssec)>1 and typ.NATCODE ='" + natCode + @"'             
 group by TYRESBackADD.diameter,TYRESBackADD.crosssec,TYRESBackADD.width order by TYRESBackADD.diameter,TYRESBackADD.CROSSSEC,TYRESBackADD.width
 ))";

                    etTireCache[key] = ctx.ExecuteStoreQuery<TireRimSize>(query, null).ToList<TireRimSize>();
                }
            }
            return etTireCache[key];
        }

        /// <summary>
        /// fetches all front rims from Eurotax
        /// </summary>
        /// <param name="natCode"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private static List<TireRimSize> GetFrontRims(string natCode, string code)
        {
            String key = "FR" + natCode;
            if (!etTireCache.ContainsKey(key))
            {
                using (DdOlExtended ctx = new DdOlExtended())
                {
                    int Diameter = GetDiameterFromCode(code);

                    string DiameterToQuery = Diameter.ToString() + ".000";

                    string query = @"select diameter,width from (select RIMSFrontADD.*
from etgtype TYP
		  INNER JOIN ETGADDITION ADDITION ON ADDITION.VehType = Typ.VehType AND ADDITION.NatCode = Typ.NatCode
      INNER JOIN ETGEQTEXT EQTEXT ON EQTEXT.VehType = ADDITION.VehType AND EQTEXT.EQCode = ADDITION.EQCode
			INNER JOIN ETGEQJWheel WheelADD ON WheelADD.VehType = EQTEXT.VehType AND EQTEXT.EQCode = WheelADD.EQTCode
        INNER JOIN ETGRIMS RIMSFrontADD ON RIMSFrontADD.ID = WheelADD.RIMRimFCd AND RIMSFRONTADD.VehType = WheelADD.VehType
where typ.NATCODE ='" + natCode + @"'   
union all select etgrims.* from etgrims,etgjwheel where etgjwheel.natcode='" + natCode + @"' and etgjwheel.rimrimfcd=etgrims.id)
   where diameter='" + DiameterToQuery + @"'
group by diameter,width  order by diameter,width";


                    etTireCache[key] = ctx.ExecuteStoreQuery<TireRimSize>(query, null).ToList<TireRimSize>();
                }
            }
            return etTireCache[key];


        }

        /// <summary>
        /// fetches all rear rims from Eurotax
        /// </summary>
        /// <param name="natCode"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private static List<TireRimSize> GetRearRims(string natCode, string code)
        {
            String key = "RR" + natCode;
            if (!etTireCache.ContainsKey(key))
            {
                using (DdOlExtended ctx = new DdOlExtended())
                {
                    int Diameter = GetDiameterFromCode(code);
                    string DiameterToQuery = Diameter.ToString() + ".000";
                    string query = @"select diameter,width from (select RIMSBackADD.* from etgtype TYP INNER JOIN ETGADDITION ADDITION ON ADDITION.VehType = Typ.VehType AND ADDITION.NatCode = Typ.NatCode 
 INNER JOIN ETGEQTEXT EQTEXT ON EQTEXT.VehType = ADDITION.VehType AND EQTEXT.EQCode = ADDITION.EQCode
 INNER JOIN ETGEQJWheel WheelADD ON WheelADD.VehType = EQTEXT.VehType AND EQTEXT.EQCode = WheelADD.EQTCode
 INNER JOIN ETGRIMS RIMSBackADD ON RIMSBackADD.ID = WheelADD.RIMRimRCd AND RIMSBACKADD.VehType = WheelADD.VehType
where typ.NATCODE ='" + natCode + @"'  
union all select etgrims.* from etgrims,etgjwheel where etgjwheel.natcode='" + natCode + @"' and etgjwheel.rimrimrcd=etgrims.id)
   where diameter='" + DiameterToQuery + @"'
  group by diameter,width order by diameter,width";

                    etTireCache[key] = ctx.ExecuteStoreQuery<TireRimSize>(query, null).ToList<TireRimSize>();
                }
            }
            return etTireCache[key];
        }

        /// <summary>
        /// Delivers Dimension information for front and rear axis Tires, including the xxx/xx Rxx dimension displayed for the car
        /// </summary>
        /// <param name="eurotaxNr"></param>
        /// <param name="reifencodeVorne"></param>
        /// <param name="reifencodeHinten"></param>
        /// <returns></returns>
        private TiresEurotaxDto getEurotaxTires(String eurotaxNr, ref String reifencodeVorne, ref String reifencodeHinten)
        {
            TiresEurotaxDto rval = new TiresEurotaxDto();


            using (DdOlExtended ctx = new DdOlExtended())
            {
                TireDto separator = new TireDto();
                separator.Code = "---------";

                List<TireDto> allTires = ctx.ExecuteStoreQuery<TireDto>("select replace(code,'R','R ') code from (select distinct code from reiftyp where hbv is not null order by code)", null).ToList();

                //front tires
                List<TireRimSize> tires = GetFrontTires(eurotaxNr);

                List<TireDto> tiresFront = new List<TireDto>();
                foreach (TireRimSize tr in tires)
                {
                    tiresFront.Add(tr.getTire());
                }
                tiresFront.Add(separator);
                allTires.RemoveAll(delegate(TireDto tire)
                {
                    return tiresFront.Any(a => a.Code.Equals(tire.Code));
                });

                tiresFront.AddRange(allTires);
                rval.TiresFront = tiresFront.ToArray();

                //rear tires
                allTires = ctx.ExecuteStoreQuery<TireDto>("select replace(code,'R','R ') code from (select distinct code from reiftyp where hbv is not null order by code)", null).ToList();
                tires = GetRearTires(eurotaxNr);

                List<TireDto> tiresRear = new List<TireDto>();
                foreach (TireRimSize tr in tires)
                {
                    tiresRear.Add(tr.getTire());
                }
                tiresRear.Add(separator);
                allTires.RemoveAll(delegate(TireDto tire)
                {
                    return tiresRear.Any(a => a.Code.Equals(tire.Code));
                });
                tiresRear.AddRange(allTires);
                rval.TiresRear = tiresRear.ToArray();

                //default assignment of first tire-code if not yet selected
                if ((reifencodeVorne == null || reifencodeVorne.Length == 0) && rval != null && rval.TiresFront != null && rval.TiresFront.Length > 0)
                    reifencodeVorne = rval.TiresFront[0].Code;
                if ((reifencodeHinten == null || reifencodeHinten.Length == 0) && rval != null && rval.TiresRear != null && rval.TiresRear.Length > 0)
                    reifencodeHinten = rval.TiresRear[0].Code;

                if (reifencodeVorne != null && reifencodeVorne.Length > 0)
                {
                    List<TireRimSize> rims = GetFrontRims(eurotaxNr, reifencodeVorne);
                    rval.RimsFront = (from r in rims
                                      select r.getRim()).ToArray();

                }
                if (reifencodeHinten != null && reifencodeHinten.Length > 0)
                {
                    List<TireRimSize> rims = GetRearRims(eurotaxNr, reifencodeVorne);
                    rval.RimsRear = (from r in rims
                                     select r.getRim()).ToArray();
                }
            }

            return rval;


        }

        /// <summary>
        /// Liefert Marke/Durchschnitt für Reifen vorne/hinten
        /// 
        /// AIDA-Usage:
        /// 
        /// Reifen:
        /// Dimensionen: VTIRE_DIM_List / HTIRE_DIM_List - deliverEurotaxTires(eurotaxnr)
        /// gewählte Dimension ==== code für Felgen
        /// Marke/Durchschnitt: WVTIRE_MAKE_List / WHTIRE_MAKE_List / SVTIRE_MAKE_List / SHTIRE_MAKE_List - deliverTirePricesFromBauart(code, bauart)
        /// Felgen:
        /// Dimensionen: HRIM_DIM_List / VRIM_DIM_List - deliverEurotaxRims(eurotax, code);
        /// Marke/Durchschnitt: HRIM_MAKE_List / VRIM_MAKE_List - deliverRims(code);
        /// 
        /// </summary>
        /// <param name="code">Dimension z.B. 225/35 R17</param>
        /// <param name="bauart">S oder W für Sommer/Winter</param>
        /// <returns></returns>
        /// 
        private TireDto[] getTiresPricesFromBauart(string code, string bauart, decimal Ust)
        {
            String key = code + "_" + bauart + "_" + Ust;
            if (!priceCache.ContainsKey(key))
            {

                Dictionary<string, decimal> HerstellerPriceAvg = GetTiresFromCodeAndBauartAVG(code, bauart);

                List<TireDto> TireDtoList = new List<TireDto>();


                if (HerstellerPriceAvg.Count > 0)
                {
                    //put Average to front
                    decimal ds = HerstellerPriceAvg[REIFTYPHelper.DURCHSCHNITT];
                    HerstellerPriceAvg.Remove(REIFTYPHelper.DURCHSCHNITT);
                    TireDto TireDto = new TireDto();
                    TireDto.Code = code;
                    TireDto.Manufacturer = REIFTYPHelper.DURCHSCHNITT;
                    TireDto.Price = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(ds);
                    TireDto.Price = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue((decimal)TireDto.Price, Ust);
                    TireDto.Bauart = bauart;
                    TireDtoList.Add(TireDto);

                    foreach (KeyValuePair<string, decimal> hp in HerstellerPriceAvg)
                    {
                        //Create new assembler
                        TireDto = new TireDto();
                        TireDto.Code = code;
                        TireDto.Manufacturer = hp.Key;
                        TireDto.Price = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(hp.Value);
                        TireDto.Price = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue((decimal)TireDto.Price, Ust);
                        TireDto.Bauart = bauart;
                        TireDtoList.Add(TireDto);
                    }


                }

                if (TireDtoList != null)
                {
                    priceCache[key] = TireDtoList.ToArray();
                }
                else
                {
                    return null;
                }
            }
            return priceCache[key];

        }
        /// <summary>
        /// creates a average price list for all producers of tires
        /// </summary>
        /// <param name="code"></param>
        /// <param name="bauart"></param>
        /// <returns></returns>
        public static Dictionary<string, decimal> GetTiresFromCodeAndBauartAVG(string code, string bauart)
        {



            Dictionary<string, decimal> HerstelleNettoAvg = new Dictionary<string, decimal>();

            code = code.ToUpper();
            bauart = bauart.ToUpper();
            code = code.Replace("R ", "R");



            using (DdOlExtended ctx = new DdOlExtended())
            {
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
                var Query = ctx.ExecuteStoreQuery<AvgDto>(query1, null).AsQueryable();


                HerstelleNettoAvg[DURCHSCHNITT] = 0;//durchschnitt über alle hersteller

                foreach (var q in Query)
                {
                    HerstelleNettoAvg[q.Name] = q.Average;

                }
                try
                {
                    if (HerstelleNettoAvg.Keys.Count > 1)
                        HerstelleNettoAvg[DURCHSCHNITT] = ctx.ExecuteStoreQuery<decimal>(query2, null).FirstOrDefault();
                }
                catch
                {
                    //no average available
                }
            }
            return HerstelleNettoAvg;
        }




        /// <summary>
        /// fetches all Rims for the given tire code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private RimDto[] getRims(string code)
        {

            if (!rimCache.ContainsKey(code))
            {
                int diameter = GetDiameterFromCode(code);
                using (DdOlExtended ctx = new DdOlExtended())
                {
                    rimCache[code] = ctx.ExecuteStoreQuery<RimDto>("select felgen.bezeichnung code, felgen.hersteller manufacturer, felgen.preis price, felgtyp.durchmesser diameter, felgtyp.breite width from felgtyp,felgen where felgen.sysfelgtyp=felgtyp.sysfelgtyp and felgtyp.durchmesser=" + diameter, null).ToArray();
                }
            }
            return rimCache[code];
        }

        /// <summary>
        /// Utility to convert a string code into a number rim code for the query
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private static int GetDiameterFromCode(string code)
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
    }

    /// <summary>
    /// Holds info for average query
    /// </summary>
    class AvgDto
    {
        public String Name { get; set; }
        public decimal Average { get; set; }
    }
    /// <summary>
    /// Holds tires and rims sizes
    /// </summary>
    class TireRimSize
    {
        public string width { get; set; }
        public string crosssec { get; set; }
        public string diameter { get; set; }
        /// <summary>
        /// creates a RimDto from the sizes
        /// </summary>
        /// <returns></returns>
        public RimDto getRim()
        {
            RimDto rval = new RimDto();
            rval.Code = getRimCode();
            rval.Width = width;
            rval.Diameter = diameter;
            return rval;
        }
        /// <summary>
        /// creates a TireDto from the sizes
        /// </summary>
        /// <returns></returns>
        public TireDto getTire()
        {
            TireDto rval = new TireDto();
            rval.Code = getTireCode();
            return rval;
        }
        /// <summary>
        /// fetches a cleaned up tire code
        /// </summary>
        /// <returns></returns>
        public string getTireCode()
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
        /// <summary>
        /// fetches a cleaned up rim code
        /// </summary>
        /// <returns></returns>
        public string getRimCode()
        {

            if (width.Contains("."))
            {
                width = width.Substring(0, width.LastIndexOf("."));
            }
            if (diameter.Contains("."))
            {
                diameter = diameter.Substring(0, diameter.LastIndexOf("."));
            }

            string Code = width + "R" + diameter;
            return Code;
        }
    }
}