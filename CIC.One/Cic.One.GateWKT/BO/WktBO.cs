using System;
using System.Collections.Generic;
using System.Linq;
using Cic.One.DTO;
using Cic.OpenLease.Service.Services.DdOl;
using Cic.OpenLease.ServiceAccess.DdOl;
using Cic.One.GateWKT.DAO;
using Cic.OpenLease.Service;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.One.GateWKT.DTO;
using Cic.OpenLease.Model.DdOl;
using Cic.OpenOne.Common.DAO;
using Cic.OpenLease.ServiceAccess;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.OpenLease.Service.Versicherung;
using Cic.One.Web.BO;
using System.Data.Common;
using System.Data.EntityClient;
using Cic.OpenOne.Common.Util.Security;
using Cic.One.Web.BO.Search;
using Devart.Data.Oracle;
using System.IO;
using System.Text;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.BO.Search;

namespace Cic.One.GateWKT.BO
{
    /// <summary>
    /// WKT BOS Business Object for special logic of AlphaOne
    /// Entrypoint for all WKT business logic calls
    /// </summary>
    public class WktBO : Cic.One.GateWKT.BO.IWktBO
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static String QUERY_WRREP = "select sysvgwr from obtyp where sysobtyp=:sysobtyp";
        private static String QUERY_MWST="select mwst.prozent from lsadd,mwst where mwst.sysmwst=lsadd.sysmwst and lsadd.syslsadd=:syslsadd";
        private static String QUERY_MOTORVS="select syskorrtyp from korrtyp where name='MOTOR_VS'";
        private static String QUERY_FUELPRICE="select fspreistab.preis from fstyp,fspreistab, fspreis where fstyp.sysfstyp=fspreis.sysfstyp and fspreistab.sysfspreis=fspreis.sysfspreis and fspreis.mappingint=:fuelcode and fspreistab.gueltigab<=sysdate order by fspreistab.gueltigab desc";
        private static String QUERY_CONSUMPTION = @"select case when txtfueltypecd2!='00100004' then kw else seckw end as kw, 
                  case when txtfueltypecd2!='00100004' then kw*1.36 else seckw*1.36 end as ps,
                  case when txtfueltypecd2='00100004' then kw else seckw end as kwe,
                  case when txtfueltypecd2='00100004' then kw*1.36 else seckw*1.36 end as pse,
                    kw+seckw kwgesamt, kw*1.36+seckw*1.36 psgesamt, 0 eek, 0 reichweite from etgtype where natcode=:natcode";
        private static String  QUERY_WARTUNG_REPERATUR =@"select adj.value 
                               from vgadj adj,
                                    vgadjtrg brd,
                                    vgavg wg,
                                    vgadjvalid val,
                                    obtyp obt
                               where adj.sysvgadjtrg = brd.sysvgadjtrg AND brd.sysbrand = obt.sysbrand AND
                                     adj.sysvgavg = wg.sysvgavg AND wg.sysvg = obt.sysvgwr AND
                                     wg.sysvgadjvalid = val.sysvgadjvalid AND brd.sysvgadjvalid = val.sysvgadjvalid AND
                                     val.name = 'Wartungskorrektur'  AND
                                     val.validfrom <= sysdate AND (val.validuntil >= sysdate OR val.validuntil is null) AND obt.sysobtyp=:sysobtyp";

        private static String AKLASSE_QUERY = "select aklasse from obtyp where schwacke=:schwacke";
        private static String QUERY_OBDETAILETGTYPE = "SELECT SEAT anzahlsitze,CYLINDER zylinder,DOOR anzahltueren from etgtype where natcode=:natcode";
        private static String QUERY_OBDETAILFABRIKAT="select vc_obtyp3.bezeichnung from vc_obtyp3,vc_obtyp4,vc_obtyp5 where vc_obtyp3.id3=vc_obtyp4.id3 and vc_obtyp4.id4=vc_obtyp5.id4 and vc_obtyp5.id5=:sysobtyp";

        private static CacheDictionary<long, Dictionary<int, List<long>>> obtypSearchFilter = CacheFactory<long, Dictionary<int, List<long>>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Prisma), CacheCategory.Prisma);

        public WktBO()
        {
        }

        /// <summary>
        /// Searches for Obtypes for the carselector
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public oSearchDto<ObtypDto> searchObtyp(iSearchObtypDto input, String language)
        {
            oSearchDto<ObtypDto> result = new oSearchDto<ObtypDto>();
            Dictionary<int, List<long>> obfilter = new Dictionary<int, List<long>>();
            
            if(input.sysrvt>0)
            {
                if (!obtypSearchFilter.ContainsKey(input.sysrvt))
                {
                    Dictionary<int, List<long>> oblevelfilters = new Dictionary<int, List<long>>();
                    using (PrismaExtended ctx = new PrismaExtended())
                    {
                        List<long> obtypfilters = ctx.ExecuteStoreQuery<long>("select sysabrregel from rvtpos,fstyp where rvtpos.sysfstyp=fstyp.sysfstyp and fstyp.bezeichnung='OBTYP' and sysrvt=" + input.sysrvt, null).ToList();
                        if (obtypfilters != null && obtypfilters.Count > 0)
                        {
                            String obtypfilter = String.Join(",", obtypfilters.ToArray());
                            oblevelfilters[0] = obtypfilters;//remember all object ids
                            oblevelfilters[1] = ctx.ExecuteStoreQuery<long>("select id1 from vc_obtyp1 where id1 in (" + obtypfilter + ")", null).ToList();
                            oblevelfilters[2] = ctx.ExecuteStoreQuery<long>("select id2 from vc_obtyp2 where id2 in (" + obtypfilter + ")", null).ToList();
                            oblevelfilters[3] = ctx.ExecuteStoreQuery<long>("select id3 from vc_obtyp3 where id3 in (" + obtypfilter + ")", null).ToList();
                            oblevelfilters[4] = ctx.ExecuteStoreQuery<long>("select id4 from vc_obtyp4 where id4 in (" + obtypfilter + ")", null).ToList();
                            oblevelfilters[5] = ctx.ExecuteStoreQuery<long>("select id5 from vc_obtyp5 where id5 in (" + obtypfilter + ")", null).ToList();
                        }
                    }
                    obtypSearchFilter[input.sysrvt] = oblevelfilters;
                }
                obfilter = obtypSearchFilter[input.sysrvt];
            }
            

            Cic.P000001.Common.Setting setting = new P000001.Common.Setting();
            setting.SelectedCurrency = "EUR";
            setting.SelectedLanguage = language;
            setting.sysperole = input.sysperole;
            setting.customerCode = 300;//HCE default (germany)
            using (PrismaExtended ctx = new PrismaExtended())
            {
                int isBN = ctx.ExecuteStoreQuery<int>("select count(*) from lsadd where mandant='BANK-now'",null).FirstOrDefault();
                if(isBN==1)
                {
                    setting.customerCode = 200;//BANKNOW
                    setting.SelectedCurrency = "CHF";
                }
            }


            long sysobtyp = 0;
            List<P000001.Common.TypedSearchParam> searchParams = new List<P000001.Common.TypedSearchParam>();
            foreach (Filter f in input.filters)
            {
                if (Enum.IsDefined(typeof(Cic.P000001.Common.SearchType), f.fieldname))
                {

                    P000001.Common.TypedSearchParam p = new P000001.Common.TypedSearchParam();
                    p.searchType = (Cic.P000001.Common.SearchType)Enum.Parse(typeof(Cic.P000001.Common.SearchType), f.fieldname, true);
                    if (p.searchType == P000001.Common.SearchType.ID)
                    {
                        sysobtyp = long.Parse(f.value);
                        continue;
                    }
                    p.Pattern = f.value;
                    searchParams.Add(p);
                }
            }

            setting.SearchParams = searchParams.ToArray();


            Cic.P000001.Common.TreeNode node = new Cic.P000001.Common.TreeNode();
            Filter tlevel = (from v in input.filters
                             where v.fieldname.Equals("LEVEL")
                             select v).FirstOrDefault();
            int treelevel = int.Parse(tlevel.value) - 1;
            node.Level = new Cic.P000001.Common.Level(treelevel);

            List<long> filtersysob = new List<long>();
            if (obfilter.ContainsKey(int.Parse(tlevel.value)))
                filtersysob = obfilter[int.Parse(tlevel.value)];

            bool noSearch = false;//true when filtering via rvtpos does not allow the current obtyp to be searched
            if (filtersysob.Count > 0 && sysobtyp>0)
            {
                if(!filtersysob.Contains(sysobtyp))
                {
                    noSearch = true;
                }
            }
            Filter tpath = (from v in input.filters
                            where v.fieldname.Equals("PATH")
                            select v).FirstOrDefault();
            if (tpath != null)
            {
                node.Key = tpath.value.Replace(";", ">");
            }

            Cic.OpenOne.CarConfigurator.BO.DataProviderUtilities DataProviderUtilities = new Cic.OpenOne.CarConfigurator.BO.DataProviderUtilities();
            //rootnode-handling
           /* if ("0".Equals(tlevel.value))
            {
                node = null;
                setting.SearchParams = new P000001.Common.TypedSearchParam[0];
            }*/
            //setting.customerCode = 300;//HCE

            Cic.P000001.Common.TreeNode[] nodes = null;
            
            if(!noSearch)
                nodes = DataProviderUtilities.GetTreeNodes(setting, node, Cic.P000001.Common.GetTreeNodeSearchModeConstants.NextLevel);

            List<ObtypDto> rval = new List<ObtypDto>();
            if (nodes != null)
            {
                foreach (Cic.P000001.Common.TreeNode tn in nodes)
                {
                    if ( setting.customerCode == 300 && tn.data.art == 130) continue;//NO MOTORBIKE! for hce
                    ObtypDto obt = new ObtypDto();
                    try
                    {
                        obt.baujahr = int.Parse(tn.data.baujahr);
                    }
                    catch (Exception) { }
                    try
                    {
                        obt.baubisjahr = int.Parse(tn.data.baubisjahr);
                    }
                    catch (Exception) { }
                    try
                    {
                        obt.sysobtyp = long.Parse(tn.data.id);
                    }
                    catch (Exception) { }
                    int aktlvl = int.Parse(tlevel.value);
                    obt.bezeichnung = tn.data.bezeichnung;
                    obt.bezeichnung2 = tn.data.bezeichnung2;
                    obt.bezeichnung3 = tn.data.bezeichnung3;
                    //if (aktlvl >= 3 && tn.data.bezeichnung2 != null && !tn.data.bezeichnung.Equals(tn.data.bezeichnung2))
                    //    obt.bezeichnung += " " + tn.data.bezeichnung2;
                    

                    obt.marke = tn.data.marke;
                    obt.modell = tn.data.modell;
                    obt.fzart = "" + tn.data.art;
                    obt.schwacke = tn.data.schwacke;
                    obt.typengenehmigung = tn.data.typengenehmigung;
                    if(obt.typengenehmigung!=null&& obt.typengenehmigung.Length<5)
                        obt.typengenehmigung = null;
                    obt.neupreisbrutto = tn.data.neupreisbrutto;
                    obt.neupreisnetto = tn.data.neupreisnetto;
                    obt.leistung = tn.data.leistung;
                    int level = aktlvl + 1;
                    if(tn.Key!=null)
                    { 
                        String[] path = tn.Key.Split('>');
                        List<long> pathids = Array.ConvertAll(path, long.Parse).ToList();

                        if (obfilter.ContainsKey(0) && obfilter[0].Intersect(pathids).Count() == 0 && obt.sysobtyp > 0 && obfilter.ContainsKey(level))
                        {
                            List<long> filtersysob2 = obfilter[level];
                            if (filtersysob2.Count > 0)
                            {
                                if (!filtersysob2.Contains(obt.sysobtyp))
                                {
                                    continue;
                                }
                            }
                        }
                     }
                    if ("4".Equals(tlevel.value)//last level has no children
                        ||
                        setting.customerCode == 300 && (obt.schwacke!=null &&obt.schwacke.Length>4) //sometimes obtyp_4 has entries with prices, so no children //(obt.schwacke!=null &&obt.schwacke.Length>4)
                        )
                        obt.children = 0;
                    else obt.children = -1;
                    //obt.children = obt.neupreisbrutto>0?0:-1;//setze childcount auf 0 damit keine Lazynode mehr im baum gezeichnet wird
                    rval.Add(obt);
                }
            }
            result.results = rval.ToArray();
            result.searchCountFiltered = result.results.Count();
            result.searchCountMax = result.searchCountFiltered;
            result.searchNumPages = 1;
            return result;
        }

        /// <summary>
        /// returns all tire and rims information for the vehicle
        /// </summary>
        /// <param name="igetTires"></param>
        /// <returns></returns>
        public TireInfoDto getTires(igetTiresDto igetTires)
        {

            return new TireBO().getTireData(igetTires.eurotaxNr, igetTires.codeWinterVorne, igetTires.codeWinterHinten, igetTires.codeSommerVorne, igetTires.codeSommerHinten, igetTires.felgenCodeVorne, igetTires.felgenCodeHinten);

        }

        /// <summary>
        /// Validates the equipment for the vehicle
        /// </summary>
        /// <param name="ivalidate"></param>
        public EquipmentValidationDto validateEquipment(ivalidateEquipmentDto ivalidate)
        {
            EquipmentValidationDto rval = new EquipmentValidationDto();

            Cic.OpenOne.CarConfigurator.BO.EurotaxDataProvider.EurotaxDataProvider ep = new OpenOne.CarConfigurator.BO.EurotaxDataProvider.EurotaxDataProvider();
            Cic.P000001.Common.Setting setting = new P000001.Common.Setting();
            setting.SelectedCurrency = "EUR";
            setting.SelectedLanguage = "de-AT";
            setting.customerCode = 100;//FOR BMW 
            Cic.P000001.Common.TreeNode node = new Cic.P000001.Common.TreeNode();
            //key needed for carconfig interface (vehtype and schwacke)
            using (DdOlExtended ctx = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "schwacke", Value = ivalidate.eurotaxNr });

                String aklasse = ctx.ExecuteStoreQuery<String>(AKLASSE_QUERY, parameters.ToArray()).FirstOrDefault();
                node.Key = aklasse + ">0>0>0>" + ivalidate.eurotaxNr;
            }
            rval.valid = true;
            //iterate every item and check if it would have errors if added to all other items
            foreach (long id in ivalidate.equipmentIds)
            {
                Cic.P000001.Common.Component c = new Cic.P000001.Common.Component();
                c.Key = id.ToString();
                if (ivalidate.checkAddEquipmentId > 0)
                {
                    c.Key = ivalidate.checkAddEquipmentId.ToString();
                }
                else if (ivalidate.checkRemoveEquipmentId > 0)
                {
                    c.Key = ivalidate.checkRemoveEquipmentId.ToString();
                }



                var t = (from a in ivalidate.equipmentIds
                         where a != id
                         select a).ToList();
                List<Cic.P000001.Common.Component> comps = new List<P000001.Common.Component>();
                foreach (long other in t)
                {
                    Cic.P000001.Common.Component ct = new Cic.P000001.Common.Component();
                    ct.Key = other.ToString();
                    comps.Add(ct);
                }
                Cic.P000001.Common.DataProvider.CheckComponentResult checkRes = ep.CheckComponent(setting, node, c, comps.ToArray());
                //currently break on first failure
                if (checkRes.CheckComponentResultConstant != P000001.Common.DataProvider.CheckComponentResultConstants.Valid)
                {
                    rval.valid = false;
                    break;
                }
                if (ivalidate.checkAddEquipmentId > 0 || ivalidate.checkRemoveEquipmentId > 0)
                {
                    break;
                }

            }

            return rval;
        }

        /// <summary>
        /// Fetches all available object infos for the obtyp id
        ///  including additional eurotaxdata if available
        ///  including manual vehicle (fztyp) configured data if available
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <returns></returns>
        public AngobDto getAngobDetailFromObtyp(long sysobtyp)
        {
            AngobDto rval = null;
            VehicleDao vd = new VehicleDao();

            TechnicalDataDto techDto = new TechnicalDataDto();
            //XXX HCE has different tech data
            techDto = vd.deliverTechnicalDataExtendedFromObTyp(sysobtyp, 1, techDto,0);

            ObjectContextDto obctx = vd.deliverObjectContextFromObTyp(sysobtyp, false, false, 0, null, 0);

            Cic.One.Web.DAO.IEntityDao crmdao = Cic.One.Web.DAO.DAOFactoryFactory.getInstance().getEntityDao();
            ObtypDto obtyp = crmdao.getObtypDetails(sysobtyp);
            using (DdOlExtended ctx = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "natcode", Value = obtyp.schwacke });

                rval = ctx.ExecuteStoreQuery<AngobDto>(QUERY_OBDETAILETGTYPE, parameters.ToArray()).FirstOrDefault();
                if (rval == null)
                    rval = new AngobDto();

                //Fabrikat
                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysobtyp", Value = sysobtyp });
                obctx.Fabrikat = ctx.ExecuteStoreQuery<String>(QUERY_OBDETAILFABRIKAT , parameters.ToArray()).FirstOrDefault();

                //read power ratings
                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "natcode", Value = obtyp.schwacke });
                TechDataDto eti = ctx.ExecuteStoreQuery<TechDataDto>(QUERY_CONSUMPTION, parameters.ToArray()).FirstOrDefault();
                if (eti != null)
                {
                    techDto.eek = "";// eti.eek;
                    techDto.pse = eti.pse;
                    techDto.kwh = eti.kwh;
                    techDto.kwgesamt = eti.kwgesamt;
                    techDto.psgesamt = eti.psgesamt;
                    techDto.kwe = eti.kwe;
                    techDto.reichweite = eti.reichweite;
                    techDto.Kw = eti.kw;
                    techDto.Ps = eti.ps;
                }

            }
            rval.zusatzdaten = new AngobIniDto();
            if (obtyp.baujahr > 1900)
            {
                rval.baujahr = new DateTime((int)obtyp.baujahr, 1, 1);
            }
            try
            {
                rval.kwe = techDto.kwe;
                rval.kwh = techDto.kwh;
                rval.reichweite = techDto.reichweite;
                rval.eek = techDto.eek;
                rval.pse = techDto.pse;
                rval.kwgesamt = techDto.kwgesamt;
                rval.psgesamt = techDto.psgesamt;

                rval.ahkBrutto = (double)techDto.ListenpreisBrutto;
                rval.ahk = obtyp.neupreisnetto;
                rval.grund = obtyp.neupreisnetto;
                rval.ahkUst = rval.ahkBrutto - rval.ahk;
                rval.grundUst = rval.ahkUst;
                rval.fznr = obtyp.schwacke;
                rval.schwacke = obtyp.schwacke;
                rval.hersteller = obctx.Hersteller;
                rval.typ = obctx.ObjectType.NAME;
                rval.grundBrutto = (double)techDto.ListenpreisBrutto;
                rval.sysObTyp = sysobtyp;

                rval.fabrikat = obctx.Fabrikat;
                rval.zusatzdaten.kw = (int)techDto.Kw;
                rval.kw = (int)techDto.Kw;
                rval.ps = (int)techDto.Ps;
                rval.zusatzdaten.co2 = (long)techDto.CO2Emission;
                rval.zusatzdaten.nox = (double)techDto.NOXEmission;
                rval.zusatzdaten.verbrauch_d = (double)techDto.Verbrauch;
                rval.zusatzdaten.particles = (double)techDto.Particles;
                rval.zusatzdaten.ps = (int)techDto.Ps;
                rval.zusatzdaten.ccm = (int)techDto.Ccm;
                rval.ccm = (int)techDto.Ccm;
                rval.automatik = techDto.Automatic;

                rval.erstzul = DateTime.Now;
                rval.wagentyp = "Neufahrzeug";
                rval.zusatzdaten.actuation = 0;
                rval.zusatzdaten.motorfuel = "0";
                rval.zusatzdaten.motortyp = "0";
                String motortype = NoVA.getMotortyp(techDto.Antriebsart);
                switch (motortype)
                {

                    case "UNDEFINED": rval.zusatzdaten.motorfuel = "0"; rval.zusatzdaten.motortyp = "0"; break;
                    case "UNLEADED_PETROL_AND_ETHANOL": rval.zusatzdaten.actuation = 1; rval.zusatzdaten.motorfuel = "14"; rval.zusatzdaten.motortyp = "0"; break;
                    case "DIESEL": rval.zusatzdaten.actuation = 1; rval.zusatzdaten.motorfuel = "3"; rval.zusatzdaten.motortyp = "1"; break;
                    case "UNLEADED_PETROL": rval.zusatzdaten.actuation = 1; rval.zusatzdaten.motorfuel = "14"; rval.zusatzdaten.motortyp = "0"; break;
                    case "PETROL": rval.zusatzdaten.actuation = 1; rval.zusatzdaten.motorfuel = "8"; rval.zusatzdaten.motortyp = "0"; break;
                    case "GAS": rval.zusatzdaten.actuation = 1; rval.zusatzdaten.motorfuel = "10"; rval.zusatzdaten.motortyp = "0"; break;
                    case "ELECTRICITY": rval.zusatzdaten.actuation = 2; rval.zusatzdaten.motorfuel = "4"; rval.zusatzdaten.motortyp = "0"; break;

                }
                if (techDto.Hybrid)
                    rval.zusatzdaten.actuation = 4;

                rval.zusatzdaten.kmstand = 0;
                rval.zusatzdaten.kraftstoff = (int)techDto.Antriebsart;
                rval.kraftstoff = (int)techDto.Antriebsart;
                rval.nova = 0;
                rval.novap = (double)techDto.NovaSatz;
                rval.farbea = "";
                rval.serie = "";
                rval.gehnid = "";
                if (rval.fabrikat != null && rval.fabrikat.Length > 60)
                    rval.fabrikat = rval.fabrikat.Substring(0, 60);
                if (rval.typ != null && rval.typ.Length > 60)
                    rval.typ = rval.typ.Substring(0, 60);
            }
            catch (Exception e)
            {
                _log.Error("Reading obtyp data failed for " + sysobtyp, e);
            }
            return rval;
        }

        /// <summary>
        /// calculates all service rates:
        ///  - fuel
        ///  - maintenance
        ///  
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <param name="icalcServices"></param>
        /// <param name="rval"></param>
        public void calculateServices(long syswfuser, icalcServicesDto icalcServices, ocalcServicesDto rval)
        {
            try
            {
                rval.fuelPrice = calcFuelPrice(icalcServices.sysobtyp, icalcServices.lz, icalcServices.ll, icalcServices.avgConsumption, icalcServices.fuelCode);
                double maintDef = 0;
                rval.maintenancePrice = calcWartungReparatur(syswfuser, icalcServices.sysobtyp, icalcServices.lz, icalcServices.ll, icalcServices.wrAufschlag, icalcServices.wrfix, icalcServices.sysangob, ref maintDef);
                rval.maintenancePriceDef = maintDef;
            }
            catch (Exception e)
            {
                _log.Error("Service calculation failed", e);
            }
        }





        /// <summary>
        /// Calculates Rate, NovaBM, RW for WKT
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <param name="icalcRate"></param>
        /// <returns></returns>
        public void calculateRate(long syswfuser, icalcRateDto input, ocalcRateDto rval)
        {
            new CalculationBO().calculateRate(syswfuser, input, rval);
        }



       

        /// <summary>
        /// calculates maintenance-costs
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <param name="sysobtyp"></param>
        /// <param name="lz"></param>
        /// <param name="ll"></param>
        /// <param name="wrAufschlag"></param>
        /// <param name="wrfix"></param>
        /// <param name="sysangob"></param>
        /// <returns></returns>
        private double calcWartungReparatur(long syswfuser, long sysobtyp, int lz, long ll, double wrAufschlag, bool wrfix, long sysangob, ref double maintdefault)
        {
            //no data found in db wrtab, wrkm
            //See ServicePresenter in OperationFormatStyle.Presenter line 338

            String query = QUERY_WARTUNG_REPERATUR;
            //sysvgwr über obtypmap zu obtyp where code=jatocode
            //interpolate über lz,ll, sysvgwr ->wr
            long sysVg = 0;
            using (OlExtendedEntities dbctx = new OlExtendedEntities())
            {

                List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysobtyp", Value = sysobtyp });
                sysVg = dbctx.ExecuteStoreQuery<long>(QUERY_WRREP, pars.ToArray()).FirstOrDefault();


                CachedVGDao vg = new CachedVGDao();
                VGBoundaries bounds = vg.getVGBoundaries(sysVg, DateTime.Now);

                double wr = 0;
                int vglz = lz;
                long vgll = ll;


                if (vglz > bounds.xmax || vgll > bounds.ymax)
                {
                    return Math.Round(0d, 2);
                }
                else if (vglz < bounds.xmin || vgll < bounds.ymin)
                {
                    return Math.Round(0d, 2);
                }
                else
                {
                    wr = vg.getVGValue(sysVg, DateTime.Now, vglz.ToString(), vgll.ToString(), VGInterpolationMode.LINEAR);
                }


                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysobtyp", Value = sysobtyp });
                long korr = dbctx.ExecuteStoreQuery<long>(query, parameters.ToArray()).FirstOrDefault();
                //korrektur
                wr = wr * (1 + korr / 100.0d);

                maintdefault = wr;
                wr = wr + (wrAufschlag / 100.0 * wr);

                if (!wrfix)
                {
                    maintdefault -= (maintdefault / 5.0);
                    wr -= (wr / 5.0);
                }

                return Math.Round(wr, 2);

            }

        }

        /// <summary>
        /// calculates the fuelprice
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <param name="lz"></param>
        /// <param name="ll"></param>
        /// <param name="avgConsumption"></param>
        /// <param name="fuelcode"></param>
        /// <returns></returns>
        private double calcFuelPrice(long sysobtyp, int lz, long ll, double avgConsumption, int fuelcode)
        {
            using (OlExtendedEntities ctx = new OlExtendedEntities())
            {
                List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "fuelcode", Value = fuelcode });
                double fuelPrice = ctx.ExecuteStoreQuery<double>(QUERY_FUELPRICE, par.ToArray()).FirstOrDefault();
                return Math.Round(((lz / 12) * ll / 100 * avgConsumption * fuelPrice) / lz, 2);
            }
        }

        /// <summary>
        /// fetches the commonly used calculationdata
        /// </summary>
        /// <param name="input"></param>
        /// <param name="rval"></param>
        public void getCalculationData(igetCalcDataDto input, Cic.One.DTO.ogetCalcDataDto rval)
        {

            CachedQuoteDao qd = new CachedQuoteDao();
            using (OlExtendedEntities ctx = new OlExtendedEntities())
            {
                List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syslsadd", Value = input.sysls });
                rval.mwst = ctx.ExecuteStoreQuery<double>(QUERY_MWST, par.ToArray()).FirstOrDefault();
            }
            rval.minderkmfactor = qd.getQuote(QUOTEDao.QUOTE_MINDERKMSATZALPHABET);
            rval.mehrkmfactor = qd.getQuote(QUOTEDao.QUOTE_MEHRKMSATZALPHABET);
            rval.wechselrhythmus_def = qd.getQuote(QUOTEDao.QUOTE_WECHSELRHYTHMUSDEF_ALPHABET);
            if (rval.wechselrhythmus_def <= 0)
                rval.wechselrhythmus_def = 25000;

            rval.tirerisk = qd.getQuote(QUOTEDao.QUOTE_REIFENKORREKTUR_RISIKO_ALPHABET);
            rval.tirefix = qd.getQuote(QUOTEDao.QUOTE_REIFENKORREKTUR_FIXLIMITIERT_ALPHABET);
            rval.tirefixunlimited = qd.getQuote(QUOTEDao.QUOTE_REIFENKORREKTUR_FIXUNLIMITIERT_ALPHABET);
            rval.tirevariabel = qd.getQuote(QUOTEDao.QUOTE_REIFENKORREKTUR_VARIABEL_ALPHABET);
            rval.angebotvaliduntil = qd.getQuote(QUOTEDao.QUOTE_ANGEBOT_GUELTIG_BIS_ALPHABET); 
        }

        /// <summary>
        /// calculates BSI price
        /// </summary>
        /// <param name="input"></param>
        /// <param name="rval"></param>
        public void calculateBSI(icalcBSIDto input, ocalcBSIDto rval)
        {
            CachedVGDao vg = new CachedVGDao();


            using (OlExtendedEntities ctx = new OlExtendedEntities())
            {
                long sysVg = ctx.ExecuteStoreQuery<long>("select sysvgsi from obtyp where sysobtyp=" + input.sysobtyp, null).FirstOrDefault();
                String paket = ctx.ExecuteStoreQuery<String>("select bezeichnung from fstyp where sysfstyp=" + input.sysfstyp, null).FirstOrDefault();
                double ust = 0;//XXX pending - hce has new ust handling (double)LsAddHelper.getGlobalUst();
                rval.price = vg.getVGValue(sysVg, DateTime.Now, "Betrag", paket, VGInterpolationMode.NONE);
                rval.price = (rval.price * 100.0) / (100.0 + ust);
                rval.price = Math.Round(rval.price, 2);
            }
        }

        /// <summary>
        /// calculates insurance price and tax
        /// </summary>
        /// <param name="input"></param>
        /// <param name="rval"></param>
        public void calculateVS(icalcVSDto input, ocalcVSDto rval)
        {
            CachedQuoteDao qd = new CachedQuoteDao();

            DateTime perDatum = DateTime.Now;
            if (input.lieferdatum != null && input.lieferdatum.HasValue)
                perDatum = input.lieferdatum.Value;
            if (perDatum.CompareTo(new DateTime(2014, 3, 1)) < 0)
                perDatum = new DateTime(2014, 3, 1);

            using (OlExtendedEntities ctx = new OlExtendedEntities())
            {
                bool electric = HaftpflichtCalculator.isElectric(input.sysobtyp, ctx);
                bool pkw = !HaftpflichtCalculator.isMotorrad(input.sysobtyp, ctx);
                string fahrzeugart = "PKW";
                if (!pkw) fahrzeugart = "BIKE";

                string query = "select aklasse from OBTYP where sysobtyp=" + input.sysobtyp;
                String aKlasse = ctx.ExecuteStoreQuery<String>(query).FirstOrDefault();
                bool lkw = (aKlasse.Equals("20"));

                double kwbase = input.kw - 24;
                KORREKTURDao korr = new KORREKTURDao(ctx);
                string op = "+";



                double min = qd.getQuote(QUOTEDao.QUOTE_MOTORVS_MIN_ALPHABET, DateTime.Now);
                double max = qd.getQuote(QUOTEDao.QUOTE_MOTORVS_MAX_ALPHABET, DateTime.Now);

                if (input.actuation.HasValue)
                {
                    electric = input.actuation.Value == 2;
                }

                if (electric)//Elektrofahrzeuge sind von der Versicherungssteuer befreit – bei dieser Antriebsart darf keine Berechnung stattfinden
                {
                    rval.steuer = 0;
                    return;
                }

                long syskorrtyp = ctx.ExecuteStoreQuery<long>(QUERY_MOTORVS, null).FirstOrDefault();

                if (pkw && !electric)
                {
                    rval.steuer = (double)korr.Correct(syskorrtyp, 0, op, perDatum, input.kw.ToString(), fahrzeugart);
                }
                else if (!pkw)
                {
                    rval.steuer = (double)korr.Correct(syskorrtyp, 0, op, perDatum, input.ccm.ToString(), fahrzeugart);
                }
                if (rval.steuer < min)
                    rval.steuer = min;

                if (lkw && rval.steuer > max)//Bei LKW soll die Deckelung des Maximalbetrages bestehen bleiben.
                    rval.steuer = max;

            }
        }

        /// <summary>
        /// assigns all search-result contracts to the campaign
        /// </summary>
        /// <param name="syscamp"></param>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public String assignCampaignContracts(long syscamp, iSearchDto iSearch)
        {
            CredentialContext cctx = new CredentialContext();

            List<Filter> filters = new List<Filter>(iSearch.filters);
            Filter cfilter = new Filter();
            cfilter.fieldname = "camp.syscamp";
            cfilter.value = syscamp.ToString();
            cfilter.filterType = FilterType.Equal;
            filters.Add(cfilter);
            iSearch.filters = filters.ToArray();
            iSearch.skip = 0;

            int assigncounter = 0;
            //now assigning all results to syscamp...
            using (OlExtendedEntities ctx = new OlExtendedEntities())
            {
                long wfuser = cctx.getMembershipInfo().sysWFUSER;
                int hoflag = ctx.ExecuteStoreQuery<int>("select hoflag from camp where syscamp=" + syscamp, null).FirstOrDefault();//wenn 1, dann result durch händler, wenn 0 dann Agentur vorqualifiziert
                
                int sysiam = ctx.ExecuteStoreQuery<int>("select sysiam from iam, iamtype where iam.sysiamtype=iamtype.sysiamtype and iamtype.code='CAMP' and iam.phase=0", null).FirstOrDefault();
                IEnumerable<VertragToCampDto> sr = new SearchBo<VertragToCampDto>(SearchQueryFactoryFactory.getInstance(), cctx.getMembershipInfo().sysPEROLE).searchQueryable(ctx, iSearch);
                DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;
                con.Open();
                try
                {

                    DbCommand cmd = con.CreateCommand();
                    DbCommand cmdot = con.CreateCommand();

                    // query values with a stored procedure with two out parameters
                    //TODO kdname, kdstrasse, kdort, kdplz
                    cmd.CommandText = "insert into oppo (sysid,area,validfrom,validuntil,status,ptelefon,telefon,handy,handy2,email,email2,name,description,syscamp,syspersonkd,syswfuser,crtdate,sysiam,crttime,syschguser,extqualflag,kdname, kdstrasse, kdort, kdplz,kdvorname) values(:sysid,:area,:validfrom,:validuntil,:status,:ptelefon,:telefon,:handy,:handy2,:email,:email2,:name,:description,:syscamp,:syspersonkd,:syswfuser,:crtdate,:sysiam,:crttime,:syschguser,:extqualflag,:kdname, :kdstrasse, :kdort, :kdplz,:kdvorname) returning sysoppo  into :myOutputParameter";
                    //TODO kunde name, vorname, plz,strasse, ort
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add(new OracleParameter("sysid", OracleDbType.Long));
                    cmd.Parameters.Add(new OracleParameter("sysiam", OracleDbType.Long));
                    cmd.Parameters.Add(new OracleParameter("area", OracleDbType.VarChar));
                    cmd.Parameters.Add(new OracleParameter("validfrom", OracleDbType.Date));
                    cmd.Parameters.Add(new OracleParameter("validuntil", OracleDbType.Date));
                    cmd.Parameters.Add(new OracleParameter("status", OracleDbType.Long));
                    cmd.Parameters.Add(new OracleParameter("ptelefon", OracleDbType.VarChar));
                    cmd.Parameters.Add(new OracleParameter("telefon", OracleDbType.VarChar));
                    cmd.Parameters.Add(new OracleParameter("handy", OracleDbType.VarChar));
                    cmd.Parameters.Add(new OracleParameter("handy2", OracleDbType.VarChar));
                    cmd.Parameters.Add(new OracleParameter("email", OracleDbType.VarChar));
                    cmd.Parameters.Add(new OracleParameter("email2", OracleDbType.VarChar));
                    cmd.Parameters.Add(new OracleParameter("name", OracleDbType.VarChar));
                   
                    cmd.Parameters.Add(new OracleParameter("kdname", OracleDbType.VarChar));
                    cmd.Parameters.Add(new OracleParameter("kdvorname", OracleDbType.VarChar));
                    cmd.Parameters.Add(new OracleParameter("kdstrasse", OracleDbType.VarChar));
                    cmd.Parameters.Add(new OracleParameter("kdplz", OracleDbType.VarChar));
                    cmd.Parameters.Add(new OracleParameter("kdort", OracleDbType.VarChar));
                    
                    cmd.Parameters.Add(new OracleParameter("description", OracleDbType.VarChar));
                    cmd.Parameters.Add(new OracleParameter("syscamp", OracleDbType.Long));
                    cmd.Parameters.Add(new OracleParameter("syspersonkd", OracleDbType.Long));
                    cmd.Parameters.Add(new OracleParameter("syswfuser", OracleDbType.Long));
                    cmd.Parameters.Add(new OracleParameter("syschguser", OracleDbType.Long));
                    cmd.Parameters.Add(new OracleParameter("extqualflag", OracleDbType.Integer));
                    cmd.Parameters.Add(new OracleParameter("crtdate", OracleDbType.Date));
                    cmd.Parameters.Add(new OracleParameter("crttime", OracleDbType.Long));
                    cmd.Parameters.Add(new OracleParameter("myOutputParameter", OracleDbType.Long,System.Data.ParameterDirection.ReturnValue));

                    cmd.Parameters["area"].Value = "VT";
                    cmd.Parameters["status"].Value = 0;
                    //Agentur vorqualifizierung erst wenn ein Ergebnis vorliegt
                    cmd.Parameters["extqualflag"].Value = 0;
                    cmd.Parameters["syscamp"].Value = syscamp;
                    cmd.Parameters["sysiam"].Value = sysiam;
                    cmd.Parameters["crtdate"].Value = new OracleDate(DateTime.Now);
                    cmd.Parameters["crttime"].Value = (long)Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(DateTime.Now);

                    //Bei Generierung eines Kampagnenapplets ohne Agenturvorqualifizierung wird automatisch eine Aktivität „Kontaktaufnahme Kunde“ generiert.
                    //wir machen das immer weil das Applet sowieso erst später sichtbar wird
                    cmdot.CommandText = "insert into oppotask(syswfuser,syscrtuser,sysoppo,sysoppotasktype,art,phase,crtdate,status,changedflag,area,sysid,crttime) values(:syswfuser,:syscrtuser,:sysoppo,:sysoppotasktype,1,0,:crtdate,0,1,'VT',:sysid,:crttime)";
                    
                    cmdot.CommandType = System.Data.CommandType.Text;
                    cmdot.Parameters.Add(new OracleParameter("sysid", OracleDbType.Long));
                    cmdot.Parameters.Add(new OracleParameter("syswfuser", OracleDbType.Long));
                    cmdot.Parameters.Add(new OracleParameter("syscrtuser", OracleDbType.Long));
                    cmdot.Parameters.Add(new OracleParameter("sysoppo", OracleDbType.Long));
                    cmdot.Parameters.Add(new OracleParameter("sysoppotasktype", OracleDbType.Long));
                    cmdot.Parameters.Add(new OracleParameter("crtdate", OracleDbType.Date));
                    cmdot.Parameters.Add(new OracleParameter("crttime", OracleDbType.Long));


                    cmdot.Parameters["sysoppotasktype"].Value = 1;
                    cmdot.Parameters["syscrtuser"].Value = wfuser;
                    cmdot.Parameters["crtdate"].Value = new OracleDate(DateTime.Now);
                    cmdot.Parameters["crttime"].Value = (long)Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(DateTime.Now);
                    
                    foreach (VertragToCampDto vt in sr)
                    {
                        if (vt.assigned > 0) continue;

                        cmd.Parameters["sysid"].Value = vt.sysid;
                        cmd.Parameters["validfrom"].Value = vt.validFrom;
                        cmd.Parameters["validuntil"].Value = vt.validUntil;
                        cmd.Parameters["ptelefon"].Value = vt.ptelefon;
                        cmd.Parameters["telefon"].Value = vt.telefon;
                        cmd.Parameters["handy"].Value = vt.handy;
                        cmd.Parameters["handy2"].Value = vt.handy2;
                        cmd.Parameters["email"].Value = vt.email;
                        cmd.Parameters["email2"].Value = vt.email2;
                        cmd.Parameters["name"].Value = vt.name;
                        cmd.Parameters["description"].Value = vt.description;
                        cmd.Parameters["syspersonkd"].Value = vt.sysPersonKd;
                        cmd.Parameters["syswfuser"].Value = vt.syswfuser;
                        cmd.Parameters["syschguser"].Value = vt.syswfuser;
                        
                        cmd.Parameters["kdname"].Value = vt.kdName;
                        cmd.Parameters["kdvorname"].Value = vt.kdvorname;
                        cmd.Parameters["kdstrasse"].Value = vt.kdstrasse;
                        cmd.Parameters["kdplz"].Value = vt.kdplz;
                        cmd.Parameters["kdort"].Value = vt.kdort;
                        
                        //Execute Stored Procedure
                        cmd.ExecuteNonQuery();
                        int sysoppo = Convert.ToInt32(cmd.Parameters["myOutputParameter"].Value);
                        
                        //immer anlegen, wird eh erst sichtbar wenn campagne aktiv ist
                        {
                            cmdot.Parameters["syswfuser"].Value = vt.syswfuser;
                            cmdot.Parameters["sysid"].Value = vt.sysid;
                            cmdot.Parameters["sysoppo"].Value = sysoppo;
                            cmdot.ExecuteNonQuery();
                        }

                        assigncounter++;
                    }


                    cmd.Dispose();
                }
                catch (Exception ex)
                {
                    _log.Error("Error assigning contract to campaign", ex);
                    throw new Exception("Zuweisung der Verträge fehlgeschlagen: " + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            SearchCache.entityChanged("OPPO");
            return assigncounter + " Verträge zugeordnet";

        }
        /// <summary>
        /// removes all search-result contracts from the campaign
        /// </summary>
        /// <param name="syscamp"></param>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public String removeCampaignContracts(long syscamp, iSearchDto iSearch)
        {
            CredentialContext cctx = new CredentialContext();

            List<Filter> filters = new List<Filter>(iSearch.filters);
            Filter cfilter = new Filter();
            cfilter.fieldname = "camp.syscamp";
            cfilter.value = syscamp.ToString();
            cfilter.filterType = FilterType.Equal;
            filters.Add(cfilter);
            iSearch.filters = filters.ToArray();
            iSearch.skip = 0;
            int assigncounter = 0;
            //now assigning all results to syscamp...
            using (OlExtendedEntities ctx = new OlExtendedEntities())
            {
                IEnumerable<VertragToCampDto> sr = new SearchBo<VertragToCampDto>(SearchQueryFactoryFactory.getInstance(), cctx.getMembershipInfo().sysPEROLE).searchQueryable(ctx, iSearch);
                DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;
                try
                {

                    DbCommand cmd = con.CreateCommand();

                    // query values with a stored procedure with two out parameters
                    cmd.CommandText = "delete from oppo where sysoppo=:sysoppo";
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add(new OracleParameter("sysoppo", OracleDbType.Long));

                    foreach (VertragToCampDto vt in sr)
                    {
                        if (vt.assigned == 0) continue;

                        cmd.Parameters["sysoppo"].Value = vt.assigned;

                        //Execute Stored Procedure
                        cmd.ExecuteNonQuery();
                        assigncounter++;
                    }


                    cmd.Dispose();
                }
                catch (Exception ex)
                {
                    _log.Error("Error removing contract from campaign", ex);
                    throw new Exception("Entfernen der Verträge fehlgeschlagen: " + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            SearchCache.entityChanged("OPPO");
            return assigncounter + " Verträge entfernt";
        }

        /// <summary>
        /// sets all campaign opportunities to checked
        /// </summary>
        /// <param name="syscamp"></param>
        /// <param name="iSearch"></param>
        /// <returns></returns>
        public String setAllCampaignOpportunitiesChecked(long syscamp, iSearchDto iSearch)
        {
            CredentialContext cctx = new CredentialContext();

            iSearch.skip = 0;

            //now assigning all results to syscamp...
            using (OlExtendedEntities ctx = new OlExtendedEntities())
            {
                IEnumerable<OpportunityDto> sr = new SearchBo<OpportunityDto>(SearchQueryFactoryFactory.getInstance(), cctx.getMembershipInfo().sysPEROLE).searchQueryable(ctx, iSearch);
                DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;
                con.Open();
                try
                {

                    DbCommand cmd = con.CreateCommand();

                    // query values with a stored procedure with two out parameters
                    cmd.CommandText = "update oppo set activeFlag=:flag where sysoppo=:sysoppo";
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add(new OracleParameter("sysoppo", OracleDbType.Long));
                    cmd.Parameters.Add(new OracleParameter("flag", OracleDbType.Integer));
                    cmd.Parameters["flag"].Value = 1;

                    foreach (OpportunityDto oppo in sr)
                    {
                        cmd.Parameters["sysoppo"].Value = oppo.sysOppo;
                        //Execute Stored Procedure
                        cmd.ExecuteNonQuery();
                    }


                    cmd.Dispose();
                }
                catch (Exception ex)
                {
                    _log.Error("Error setting oppo aktivflag", ex);
                    throw new Exception("Funktion fehlgeschlagen: " + ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            SearchCache.entityChanged("OPPO");

            return "Alle Opportunities auf geprüft gesetzt";
        }

        /// <summary>
        /// uploads all csv campaign results to the opportunities of the campaign
        /// </summary>
        /// <param name="syscamp"></param>
        /// <param name="fileData"></param>
        /// <returns></returns>
        public String uploadCampaignResults(long syscamp, FileattDto fileData)
        {

            StringBuilder uploadResult = new StringBuilder();
            StringBuilder rowResult = new StringBuilder();
            try{
            using (OlExtendedEntities ctx = new OlExtendedEntities())
            {
                DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;
                try
                {
                    CredentialContext cctx = new CredentialContext();
                    long wfuser = cctx.getMembershipInfo().sysWFUSER;
                    int hoflag = ctx.ExecuteStoreQuery<int>("select hoflag from camp where syscamp=" + syscamp, null).FirstOrDefault();//wenn 1, dann result durch händler, wenn 0 dann Agentur vorqualifiziert
                    con.Open();
                    DbCommand cmd = con.CreateCommand();
                    cmd.CommandText = "update oppo set extqualflag=:extqualflag,kdname=:kdname, kdstrasse=:kdstrasse, kdplz=:kdplz, kdort=:kdort, ptelefon=:ptelefon, telefon=:telefon, handy=:handy, handy2=:handy2, email=:email, email2=:email2, letterflag=:letterflag, telefonflag=:telefonflag, activeFlag=:activeflag where sysoppo=:sysoppo";
                    if (cctx.getMembershipInfo().IsInternalMitarbeiter)
                    {
                        cmd.CommandText = "update oppo set  extqualflag=:extqualflag, extresultat=:erg, notiz=:notiz, status=:status, kdname=:kdname, kdstrasse=:kdstrasse, kdplz=:kdplz, kdort=:kdort, ptelefon=:ptelefon, telefon=:telefon, handy=:handy, handy2=:handy2, email=:email, email2=:email2, letterflag=:letterflag, telefonflag=:telefonflag, activeFlag=:activeflag where sysoppo=:sysoppo";
                        cmd.Parameters.Add(new OracleParameter("erg", OracleDbType.Integer));
                        cmd.Parameters.Add(new OracleParameter("notiz", OracleDbType.VarChar));
                        cmd.Parameters.Add(new OracleParameter("status", OracleDbType.Integer));

                    }
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add(new OracleParameter("sysoppo", OracleDbType.Long));
                    
                    cmd.Parameters.Add(new OracleParameter("kdname", OracleDbType.VarChar));
                    cmd.Parameters.Add(new OracleParameter("kdstrasse", OracleDbType.VarChar));
                    cmd.Parameters.Add(new OracleParameter("kdplz", OracleDbType.VarChar));
                    cmd.Parameters.Add(new OracleParameter("kdort", OracleDbType.VarChar));
                    cmd.Parameters.Add(new OracleParameter("ptelefon", OracleDbType.VarChar));
                    cmd.Parameters.Add(new OracleParameter("telefon", OracleDbType.VarChar));
                    cmd.Parameters.Add(new OracleParameter("handy", OracleDbType.VarChar));
                    cmd.Parameters.Add(new OracleParameter("handy2", OracleDbType.VarChar));
                    cmd.Parameters.Add(new OracleParameter("email", OracleDbType.VarChar));
                    cmd.Parameters.Add(new OracleParameter("email2", OracleDbType.VarChar));
                    cmd.Parameters.Add(new OracleParameter("extqualflag", OracleDbType.Integer));
                    cmd.Parameters.Add(new OracleParameter("letterflag", OracleDbType.Integer));
                    cmd.Parameters.Add(new OracleParameter("telefonflag", OracleDbType.Integer));
                    cmd.Parameters.Add(new OracleParameter("activeflag", OracleDbType.Integer));

                    DbCommand cmdot = con.CreateCommand();
                    //Bei Generierung eines Kampagnenapplets ohne Agenturvorqualifizierungwird automatisch eine Aktivität „Kontaktaufnahme Kunde“ generiert.
                    //Bei Agenturvorqualifizierung wird diese Aktivität erst im Zuge des Campaign Result Load generiert 
                    //wir machen das aber gleich bei der Vertragszuweisung weil einfacher und aktivität eh erst später sichtbar wird
                    cmdot.CommandText = "insert into oppotask(syswfuser,syscrtuser,sysoppo,sysoppotasktype,art,phase,crtdate,status,changedflag,area,sysid,crttime) values(:syswfuser,:syscrtuser,:sysoppo,:sysoppotasktype,1,0,:crtdate,0,1,'VT',:sysid,:crttime)";
                    
                    cmdot.CommandType = System.Data.CommandType.Text;
                    cmdot.Parameters.Add(new OracleParameter("sysid", OracleDbType.Long));
                    cmdot.Parameters.Add(new OracleParameter("syswfuser", OracleDbType.Long));
                    cmdot.Parameters.Add(new OracleParameter("syscrtuser", OracleDbType.Long));
                    cmdot.Parameters.Add(new OracleParameter("sysoppo", OracleDbType.Long));
                    cmdot.Parameters.Add(new OracleParameter("sysoppotasktype", OracleDbType.Long));
                    cmdot.Parameters.Add(new OracleParameter("crtdate", OracleDbType.Date));
                    cmdot.Parameters.Add(new OracleParameter("crttime", OracleDbType.Long));


                    cmdot.Parameters["sysoppotasktype"].Value = 1;
                    cmdot.Parameters["syscrtuser"].Value = wfuser;
                    cmdot.Parameters["crtdate"].Value = new OracleDate(DateTime.Now);
                    cmdot.Parameters["crttime"].Value = (long)Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(DateTime.Now);

                    Dictionary<int,int> mapstatus = new Dictionary<int,int>();
                    mapstatus[0] = 0;
                    mapstatus[1] = 1;
                    mapstatus[2] = 1;
                    mapstatus[3] = 2;
                    mapstatus[4] = 3;
                    mapstatus[5] = 5;
                    mapstatus[6] = 4;
                    mapstatus[7] = 4;
                    /* ERG:
                        Noch kein Ergebnis = 0 ->0
                        Interesse / Kontaktaufnahme durch Verkäufer=1 ->1
                        Interesse / Kontaktaufnahme durch Kunden=2 ->1
                        Bereits in Händlerkontakt=3->2
                        Bereits gekauft/finanziert=4->3
                        Kein Interesse=5->5
                        Ungültige Telefonnr./E-Mail Adresse =6->4
                        Kein Kontakt möglich =7->4
                      
                     * OPPOSTATUS
                        Noch kein Ergebnis=0
                        Hot Lead=1
                        Bereits in Händlerkontakt=2
                        Bereits gekauft/finanziert=3
                        Nachbearbeitung HO=4
                        Cold Lead=5
                    */
                    MemoryStream ms = new MemoryStream(fileData.content);

                    String line;
                    System.IO.StreamReader sr = new StreamReader(ms);
                    int row = 1;
                    sr.ReadLine();//read header line
                    //process all csv lines
                    /*
                     * kdname 17
                        kdstrasse 19
                        kdplz 20 
                        kdort 21
                        ptelefon 22 
                        telefon 23
                        handy 25
                        handy2 24 
                        email 27
                        email2 26
                        letterflag 2
                        telefonflag 3
                        activeFlag 1
                     * */
                    Encoding iso = Encoding.GetEncoding("Windows-1252");
                    Encoding utf8 = Encoding.UTF8;

                    int errrow = 0;

                    while ((line = sr.ReadLine()) != null)
                    {
                        /*byte[] isoBytes = iso.GetBytes(line);
                        byte[] utf8Bytes = Encoding.Convert(iso, utf8, isoBytes);
                        line = utf8.GetString(utf8Bytes);*/
                        if (line.EndsWith(";")) line = line.Substring(0, line.Length - 1);
                        String[] items = line.Split(';');
                        try
                        {
                            long sysoppo = long.Parse(items[0]);
                            int lastidx = items.Length - 1;
                            int erg = 0;
                            if(lastidx<37)
								throw new Exception("Spalte Agenturergebnis nicht enthalten");
                            int.TryParse(items[37],out erg);
                            if (erg > 7) throw new Exception("Ungültiger Ergebnis-Code");
                            cmd.Parameters["sysoppo"].Value = sysoppo;

                            //Agenturergebnisse:
                            if (cctx.getMembershipInfo().IsInternalMitarbeiter)
                            {
                                cmd.Parameters["erg"].Value = erg;
								if(lastidx>=38)
									cmd.Parameters["notiz"].Value = items[38];
                                cmd.Parameters["status"].Value = mapstatus[erg];//map campaign status from agentur result
                            }
                           
                            cmd.Parameters["extqualflag"].Value = erg > 0 ? 0 : 1;//nur wenn agenturergebnis vorhanden, dann flag setzen
                           
                            cmd.Parameters["kdname"].Value = items[17];
                            cmd.Parameters["kdstrasse"].Value = items[19];
                            cmd.Parameters["kdplz"].Value = items[20];
                            cmd.Parameters["kdort"].Value = items[21];
                            cmd.Parameters["ptelefon"].Value = getStrippedNumber(items[22]);
                            cmd.Parameters["telefon"].Value = getStrippedNumber(items[23]);
                            cmd.Parameters["handy"].Value = getStrippedNumber(items[25]);
                            cmd.Parameters["handy2"].Value = getStrippedNumber(items[24]);
                            cmd.Parameters["email"].Value = items[27];
                            cmd.Parameters["email2"].Value = items[26];

                            cmd.Parameters["letterflag"].Value = items[2];
                            cmd.Parameters["telefonflag"].Value = items[3];
                            cmd.Parameters["activeflag"].Value = items[1];
                            //Execute Stored Procedure
                            cmd.ExecuteNonQuery();

                            //wenn vorqualifizierung und interner mitarbeiter den result load durchführt und ein ergebnis vorhanden ist, dann die Aktivität Kontaktaufnahme Kunde anlegen
                            //wir machen das aber immer gleich bei der vertragszuordnung
                            /*
                            if (hoflag == 0 && cctx.getMembershipInfo().IsInternalMitarbeiter && erg>0)
                            {
                                cmdot.Parameters["sysoppo"].Value = sysoppo;
                                cmdot.ExecuteNonQuery();
                            }*/
                        }
                        catch (Exception ex)
                        {
                            _log.Error("Error updating csv row " + row + ": " + ex.Message);
                            rowResult.Append(row + " ("+ex.Message+")\n");
                            errrow++;
                        }
                        row++;

                    }
                    _log.Debug("Processed " + row + " rows of csv-file");
                    uploadResult.Append(row + " Zeilen verarbeitet\n");
                    uploadResult.Append(errrow + " Zeilen fehlerhaft\n");
                    uploadResult.Append(rowResult);
                    //close command
                    cmd.Dispose();
                    //update wfuser and sysid references
                   // ctx.ExecuteStoreCommand("update oppotask t1 set (sysid,syswfuser) = (select t2.sysid, t2.syswfuser from oppo t2 where t1.sysoppo=t2.sysoppo) where exists(select 1 from oppo t2 where t1.sysoppo=t2.sysoppo)",null);
                }
                catch (Exception ex)
                {
                    _log.Error("Error importing CSV", ex);
                    uploadResult.Append("Unbekannter Fehler: "+ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            }catch(Exception me)
            {
                uploadResult.Append("Unbekannter Fehler: "+me.Message);
                _log.Error("Error importing CSV", me);
            }
            SearchCache.entityChanged("OPPO");
            return "Upload-Ergebnis: \n"+uploadResult;
        }
        private static String getStrippedNumber(String value)
        {
            if (value == null) return null;
            String rval = value.Replace('\'',' ');
            rval = rval.Replace(" ", String.Empty);
            rval = rval.Trim();
            return rval;
        }

    }
}