namespace Cic.OpenLease.Service
{
    #region Using
    using Cic.OpenLease.ServiceAccess;
    using Cic.OpenLease.ServiceAccess.DdOl;
    using Cic.OpenOne.Common.DAO.Prisma;
    using Cic.OpenOne.Common.Model.DdOl;
    using Cic.OpenOne.Common.Util.Collection;
    using Cic.OpenOne.Common.Util.Logging;
    using CIC.Database.OL.EF6.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    #endregion

    public class FsPreisHelper
    {
        #region Private constants
        //public const string[] bmwservices = { "Krafststoff", "Reifen", "Wartung", "AnAbMeldung", "AnAbMeldungMotorrad", "Ersatzfahrzeug","Sonstige Dienstleistungen" };
        private const string CnstFuelFsArt = "Kraftstoff";
        private const string CnstFuelLieferantFsArt = "Kraftstofflieferant";
        private const string CnstReifenFsArt = "Reifen";
        public const string CnstWartungFsArt = "Wartung";
        private const string CnstUnregistrationFsArt = "AnAbMeldung";
        private const string CnstUnregistrationFsArtMotorrad = "AnAbMeldungMotorrad";
        private const string CnstUnregistrationMotorrad = "Motorrad";
        private const string CnstReplacementCarFsArt = "Ersatzfahrzeug";
        private const string CnstManagmentFeeFsArt = "Sonstige Dienstleistungen";
        private const string CnstNebenKostenPriceFsArt = "Zusatzkosten";
        private const string CnstBsiMiniFsArt = "BSI Mini";
        private const string CnstBsiBmwFsArt = "BSI BMW";
        private const string CnstMotorSchwacke = "40";
        #endregion

        #region Private variables
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Methods
        public static decimal GetFuelPrice(FuelTypeConstants fuelType, long SYSFSTYP)
        {
            // Create the entitiess
            using (DdOlExtended Entities = new DdOlExtended())
            {
                try
                {

                    // Get the appropriate fuel type
                    string FuelTypeStr = ((int)fuelType).ToString();


                    // Create a query
                    var Query = from FsPrice in Entities.FSPREIS
                                where FsPrice.SYSFSTYP == SYSFSTYP
                                && FsPrice.MAPPINGEXT == FuelTypeStr
                                select FsPrice;

                    // Query FSPREIS
                    var CurrentFsPrice = MygetFsPreis(Query);

                    // Query FSPREISTAB
                    var FuelPrice = MyGetFsPriceTab(CurrentFsPrice.SYSFSPREIS, Entities);

                    // Return the price
                    return FuelPrice;
                }
                catch (Exception e)
                {
                    // Throw an exception
                    throw new ApplicationException("Could not get the fuel price.", e);
                }
            }
        }
        public static decimal GetFuelPrice(FuelTypeConstants fuelType)
        {
            // Create the entitiess
            using (DdOlExtended Entities = new DdOlExtended())
            {
                try
                {
                    // Get the appropriate fuel type
                    string FuelTypeStr = ((int)fuelType).ToString();

                    // Qery FSART
                    var CurrentFsArt = MyGetFsArt(CnstFuelFsArt, Entities);

                    // Query FSTYP
                    var CurrentFsTyp = MyGetFsTyp(CurrentFsArt.SYSFSART, Entities);

                    // Create a query
                    var Query = from FsPrice in Entities.FSPREIS
                                where FsPrice.SYSFSTYP == CurrentFsTyp.SYSFSTYP
                                && FsPrice.MAPPINGEXT == FuelTypeStr
                                select FsPrice;

                    // Query FSPREIS
                    var CurrentFsPrice = MygetFsPreis(Query);

                    // Query FSPREISTAB
                    var FuelPrice = MyGetFsPriceTab(CurrentFsPrice.SYSFSPREIS, Entities);

                    // Return the price
                    return FuelPrice;
                }
                catch (Exception e)
                {
                    // Throw an exception
                    throw new ApplicationException("Could not get the fuel price.", e);
                }
            }
        }


        public static PetrolLieferantDto[] GetFuelLieferanten(FuelTypeConstants fuelType)
        {
            // Create the entitiess
            using (DdOlExtended Entities = new DdOlExtended())
            {
                try
                {
                    // Get the appropriate fuel type
                    string FuelTypeStr = ((int)fuelType).ToString();

                    // Qery FSART
                    var CurrentFsArt = MyGetFsArt(CnstFuelLieferantFsArt, Entities);


                    List<PetrolLieferantDto> result = new List<PetrolLieferantDto>();


                    var FsTypes = (from FsTyp in Entities.FSTYP
                                   where FsTyp.SYSFSART == CurrentFsArt.SYSFSART
                                   orderby FsTyp.BEZEICHNUNG ascending
                                   select FsTyp);

                    foreach (FSTYP FsTypesLoop in FsTypes)
                    {
                        /*var Person = (from p in Entities.PERSON
                                      where p.SYSPERSON == FsTypesLoop.SYSFS
                                      select p.MATCHCODE);*/

                        PetrolLieferantDto Item = new PetrolLieferantDto();
                        Item.SYSFSTYP = FsTypesLoop.SYSFSTYP;
                        Item.BEZEICHNUNG = FsTypesLoop.BEZEICHNUNG;// Person.FirstOrDefault();
                        result.Add(Item);

                    }



                    // Return the price
                    return result.ToArray();
                }
                catch (Exception e)
                {
                    // Throw an exception
                    throw new ApplicationException("Could not get the fuels suppliers.", e);
                }
            }
        }

        public static decimal GetNebenKostenPriceParameter()
        {
            using (DdOlExtended Entities = new DdOlExtended())
            {
                try
                {
                    // Qery FSART
                    var CurrentFsArt = MyGetFsArt(CnstNebenKostenPriceFsArt, Entities);

                    // Query FSTYP
                    var CurrentFsTyp = MyGetFsTyp(CurrentFsArt.SYSFSART, Entities);

                    // Create a query
                    var Query = from FsPrice in Entities.FSPREIS
                                where FsPrice.SYSFSTYP == CurrentFsTyp.SYSFSTYP
                                select FsPrice;
                    // Query FSPREIS
                    var CurrentFsPrice = MygetFsPreis(Query);

                    // Query FSPREISTAB
                    var NebenKostenPriceParameter = MyGetFsPriceTab(CurrentFsPrice.SYSFSPREIS, Entities);

                    // Return the price
                    return NebenKostenPriceParameter;
                }
                catch (Exception e)
                {
                    // Throw an exception
                    throw new ApplicationException("Could not get price.", e);
                }
            }
        }

        public static FSTYPDto getFsTyp(String Name)
        {
            // Create the entities
            using (DdOlExtended Entities = new DdOlExtended())
            {

                // Qery FSART
                var CurrentFsArt = MyGetFsArt(Name, Entities);

                FSTYP fstyp = MyGetFsTyp(CurrentFsArt.SYSFSART, Name, Entities, false);
                if (fstyp != null)
                {
                    FSTYPDto fs = new FSTYPDto();
                    fs.BESCHREIBUNG = fstyp.BESCHREIBUNG;
                    fs.BEZEICHNUNG = fstyp.BEZEICHNUNG;
                    fs.DISABLED = 0;
                    fs.FIXVARDEFAULT = fstyp.FIXVARDEFAULT;
                    fs.FIXVAROPTION = fstyp.FIXVAROPTION;
                    fs.NEEDED = 0;
                    fs.SYSFSTYP = fstyp.SYSFSTYP;
                    fs.SYSVG = (long)fstyp.SYSVG;
                    fs.SYSFSART = (long)fstyp.SYSFSART;
                    fs.SYSFS = (long)fstyp.SYSFS;
                    fs.ZINS = (decimal)fstyp.ZINS;
                    fs.ZINSFLAG = (int)fstyp.ZINSFLAG;
                    return fs;
                }
                return null;
            }
        }

        public static decimal GetUnregistrationPrice(NumberPlateConstants numberPlate, string schwacke)
        {
            // Create the entities
            using (DdOlExtended Entities = new DdOlExtended())
            {
                try
                {
                    // Qery FSART
                    var CurrentFsArt = MyGetFsArt(CnstUnregistrationFsArt, Entities);

                    FSTYP CurrentFsTyp = null;

                    // Query FSTYP
                    if (schwacke != CnstMotorSchwacke)
                    {
                        CurrentFsTyp = MyGetFsTyp(CurrentFsArt.SYSFSART, CnstUnregistrationMotorrad, Entities, true);
                    }
                    else
                    {
                        CurrentFsTyp = MyGetFsTyp(CurrentFsArt.SYSFSART, CnstUnregistrationMotorrad, Entities, false);
                    }

                    // Get the FSPREIS mapping
                    string NumberPlateMapping = ((int)numberPlate).ToString();

                    // Create a query
                    var Query = from FsPrice in Entities.FSPREIS
                                where FsPrice.SYSFSTYP == CurrentFsTyp.SYSFSTYP
                                && FsPrice.MAPPINGEXT == NumberPlateMapping
                                select FsPrice;

                    // Query FSPREIS
                    var CurrentFsPrice = MygetFsPreis(Query);

                    // Query FSPREISTAB
                    var UnregistrationPrice = MyGetFsPriceTab(CurrentFsPrice.SYSFSPREIS, Entities);

                    // Return the price
                    return UnregistrationPrice;

                }
                catch (Exception e)
                {
                    // Throw an exception
                    throw new ApplicationException("Could not get the unregistration price.", e);
                }
            }
        }

        public static decimal[] GetNebenKostenPriceParameters()
        {
            //            List<decimal> NebenKostenPrices = new List<decimal>();
            using (DdOlExtended Entities = new DdOlExtended())
            {

                DateTime dt = Entities.ExecuteStoreQuery<DateTime>("select max(fspreistab.gueltigab) from fspreistab,fspreis,fstyp, fsart where fspreistab.sysfspreis=fspreis.sysfspreis and fspreis.sysfstyp=fstyp.sysfstyp and fstyp.sysfsart=fsart.sysfsart and fsart. beschreibung='" + CnstNebenKostenPriceFsArt + "' and (fspreistab.gueltigab<=sysdate) order by fspreistab.gueltigab desc", null).FirstOrDefault();
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "perDate", Value = dt });
                return Entities.ExecuteStoreQuery<decimal>("select fspreistab.preis from fspreistab,fspreis,fstyp, fsart where fspreistab.sysfspreis=fspreis.sysfspreis and fspreis.sysfstyp=fstyp.sysfstyp and fstyp.sysfsart=fsart.sysfsart and fsart. beschreibung='Zusatzkosten' and (fspreistab.gueltigab<=:perDate) order by fspreistab.preis", parameters.ToArray()).ToArray();



                /*
            


                     try
                     {
                         // Qery FSART
                         var CurrentFsArt = MyGetFsArt(CnstNebenKostenPriceFsArt, Entities);

                         // Query FSTYP
                         var CurrentFsTyp = MyGetFsTypes(CurrentFsArt.SYSFSART, Entities);

                         foreach (FSTYP FSTYPLoop in CurrentFsTyp)
                         {
                             try
                             {
                                 // Create a query
                                 var Query = from FsPrice in Entities.FSPREIS
                                             where FsPrice.SYSFSTYP == FSTYPLoop.SYSFSTYP
                                             select FsPrice;
                                 // Query FSPREIS
                                 var CurrentFsPrice = MygetFsPrices(Query);

                                 foreach (FSPREIS FSPREISLoop in CurrentFsPrice)
                                 {
                                     try
                                     {
                                         // Query FSPREISTAB
                                         var NebenKostenPriceParameter = MyGetFsPriceTabs(FSPREISLoop.SYSFSPREIS, Entities);

                                         foreach (FSPREISTAB PriceLoop in NebenKostenPriceParameter)
                                         {
                                             NebenKostenPrices.Add(PriceLoop.PREIS.GetValueOrDefault());
                                         }
                                     }
                                     catch
                                     {
                                         //Do not add to list
                                     }

                                 }
                             }
                             catch
                             {
                                 //Do not add to list
                             }
                         }
                         // Return the price
                         return NebenKostenPrices.ToArray();
                     }
                     catch (Exception e)
                     {
                         // Throw an exception
                         throw new ApplicationException("Could not get price.", e);
                     }*/
            }
        }

        public static decimal[] GetReplacementCarPrices()
        {
            List<decimal> ReplacementCarPrices = new List<decimal>();

            // Create the entities
            using (DdOlExtended Entities = new DdOlExtended())
            {
                try
                {
                    // Qery FSART
                    var CurrentFsArt = MyGetFsArt(CnstReplacementCarFsArt, Entities);

                    // Query FSTYP
                    var CurrentFsTyp = MyGetFsTypes(CurrentFsArt.SYSFSART, Entities);

                    foreach (FSTYP FSTYPLoop in CurrentFsTyp)
                    {
                        try
                        {
                            // Create a query
                            var Query = from FsPrice in Entities.FSPREIS
                                        where FsPrice.SYSFSTYP == FSTYPLoop.SYSFSTYP
                                        select FsPrice;

                            // Query FSPREIS
                            var CurrentFsPrice = MygetFsPrices(Query);

                            foreach (FSPREIS FSPREISLoop in CurrentFsPrice)
                            {
                                try
                                {
                                    // Query FSPREISTAB
                                    var ReplacementCarPrice = MyGetFsPriceTabs(FSPREISLoop.SYSFSPREIS, Entities);

                                    foreach (FSPREISTAB PriceLoop in ReplacementCarPrice)
                                    {
                                        ReplacementCarPrices.Add(PriceLoop.PREIS.GetValueOrDefault());
                                    }
                                }
                                catch
                                {
                                    //Do not add to list
                                }
                            }
                        }
                        catch
                        {
                            //Do not add to list
                        }
                    }

                    ReplacementCarPrices.Sort();
                    // Return the prices
                    return ReplacementCarPrices.ToArray();

                }
                catch (Exception e)
                {
                    // Throw an exception
                    throw new ApplicationException("Could not get the replacement car price.", e);
                }
            }
        }

        public static decimal[] GetManagementFees()
        {
            List<decimal> ManagementFess = new List<decimal>();
            // Create the entities
            using (DdOlExtended Entities = new DdOlExtended())
            {
                try
                {
                    // Qery FSART
                    var CurrentFsArt = MyGetFsArt(CnstManagmentFeeFsArt, Entities);

                    // Query FSTYP
                    var CurrentFsTyp = MyGetFsTypes(CurrentFsArt.SYSFSART, Entities);

                    // Create a query
                    foreach (FSTYP FSTYPLoop in CurrentFsTyp)
                    {
                        try
                        {
                            var Query = from FsPrice in Entities.FSPREIS
                                        where FsPrice.SYSFSTYP == FSTYPLoop.SYSFSTYP
                                        select FsPrice;

                            // Query FSPREIS
                            var CurrentFsPrice = MygetFsPrices(Query);

                            foreach (FSPREIS FSPREISLoop in CurrentFsPrice)
                            {
                                // Query FSPREISTAB
                                try
                                {
                                    var ManagementFee = MyGetFsPriceTabs(FSPREISLoop.SYSFSPREIS, Entities);
                                    foreach (FSPREISTAB PriceLoop in ManagementFee)
                                    {
                                        ManagementFess.Add(PriceLoop.PREIS.GetValueOrDefault());
                                    }
                                }
                                catch
                                {
                                    //Do not add to list
                                }
                            }
                        }
                        catch
                        {
                            //Do not add to list
                        }
                    }

                    // Return the prices
                    return ManagementFess.ToArray();

                }
                catch (Exception e)
                {
                    // Throw an exception
                    throw new ApplicationException("Could not get the management fee.", e);
                }
            }
        }

        public static decimal GetReplacementCarPrice()
        {
            // Create the entities
            using (DdOlExtended Entities = new DdOlExtended())
            {
                try
                {
                    // Qery FSART
                    var CurrentFsArt = MyGetFsArt(CnstReplacementCarFsArt, Entities);

                    // Query FSTYP
                    var CurrentFsTyp = MyGetFsTyp(CurrentFsArt.SYSFSART, Entities);

                    // Create a query
                    var Query = from FsPrice in Entities.FSPREIS
                                where FsPrice.SYSFSTYP == CurrentFsTyp.SYSFSTYP
                                select FsPrice;

                    // Query FSPREIS
                    var CurrentFsPrice = MygetFsPreis(Query);

                    // Query FSPREISTAB
                    var ReplacementCarPrice = MyGetFsPriceTab(CurrentFsPrice.SYSFSPREIS, Entities);

                    // Return the price
                    return ReplacementCarPrice;

                }
                catch (Exception e)
                {
                    // Throw an exception
                    throw new ApplicationException("Could not get the replacement car price.", e);
                }
            }
        }

        public static decimal GetManagementFee()
        {
            // Create the entities
            using (DdOlExtended Entities = new DdOlExtended())
            {
                try
                {
                    // Qery FSART
                    var CurrentFsArt = MyGetFsArt(CnstManagmentFeeFsArt, Entities);

                    // Query FSTYP
                    var CurrentFsTyp = MyGetFsTyp(CurrentFsArt.SYSFSART, Entities);

                    // Create a query
                    var Query = from FsPrice in Entities.FSPREIS
                                where FsPrice.SYSFSTYP == CurrentFsTyp.SYSFSTYP
                                select FsPrice;

                    // Query FSPREIS
                    var CurrentFsPrice = MygetFsPreis(Query);

                    // Query FSPREISTAB
                    var ManagementFee = MyGetFsPriceTab(CurrentFsPrice.SYSFSPREIS, Entities);

                    // Return the price
                    return ManagementFee;

                }
                catch (Exception e)
                {
                    // Throw an exception
                    throw new ApplicationException("Could not get the management fee.", e);
                }
            }
        }

        public static MitfinanzierteBestandteileDto[] GetMitfinanzierteBestandteile()
        {
            // Create a context
            using (DdOlExtended Context = new DdOlExtended())
            {
                // Query FSART
                var FsArts = from FsArt in Context.FSART
                             where FsArt.BESCHREIBUNG == CnstBsiBmwFsArt
                             || FsArt.BESCHREIBUNG == CnstBsiMiniFsArt
                             select FsArt;

                // Create a result list
                List<MitfinanzierteBestandteileDto> Result = new List<MitfinanzierteBestandteileDto>();

                // Iterate through all FsArts
                foreach (var LoopFsArt in FsArts)
                {
                    // Get FsTyps
                    List<FSTYP> FsTypes = MyGetFsTypes(LoopFsArt.SYSFSART, Context);

                    // Iterate through all FsTyps
                    foreach (var LoopFsType in FsTypes)
                    {
                        // Create a query
                        var FsPrices = from FsPrice in Context.FSPREIS
                                       where FsPrice.SYSFSTYP == LoopFsType.SYSFSTYP
                                       && FsPrice.GUELTIGAB < DateTime.Today
                                       select FsPrice;

                        // Set the price to 0
                        decimal Price = 0;

                        // Iterate through all FsPrices
                        foreach (var LoopFsPrice in FsPrices)
                        {
                            // Get the price tab
                            var CurrentFsPriceTab = (from FsPriceTab in Context.FSPREISTAB
                                                     where FsPriceTab.FSPREIS.SYSFSPREIS == LoopFsPrice.SYSFSPREIS
                                                     && FsPriceTab.GUELTIGAB < DateTime.Today
                                                     orderby FsPriceTab.GUELTIGAB descending
                                                     select FsPriceTab).FirstOrDefault();

                            // Add the price
                            Price += CurrentFsPriceTab.PREIS.GetValueOrDefault();
                        }

                        // Create MitfinansierteBestandteileDto
                        MitfinanzierteBestandteileDto Item = new MitfinanzierteBestandteileDto();

                        // Set the properties
                        Item.FsArt = LoopFsArt.BESCHREIBUNG;
                        Item.FsTyp = LoopFsType.BEZEICHNUNG;
                        Item.FsPreis = Price;
                        Item.FIXVARDEFAULT = LoopFsType.FIXVARDEFAULT;
                        Item.FIXVAROPTION = LoopFsType.FIXVAROPTION;


                        // Add the item to the list
                        Result.Add(Item);
                    }
                }

                // Return the result
                return Result.ToArray();
            }
        }

        public static MitfinanzierteBestandteileDto[] GetMitfinanzierteBestandteileObjTyp(long sysobjtyp)
        {

            // Create a context
            using (DdOlExtended Context = new DdOlExtended())
            {

                var ObType = (from ObTyp in Context.OBTYP
                              where ObTyp.SYSOBTYP == sysobjtyp
                              select ObTyp).FirstOrDefault();

                var FsVgs = (from FsVg in Context.VG
                             where FsVg.SYSVG == ObType.SYSVGSI
                             select FsVg).FirstOrDefault();


                // Query FSTYP
                var FsTypes = from FsTyp in Context.FSTYP
                              where FsTyp.SYSVG == FsVgs.SYSVG
                              select FsTyp;

                // Create a result list
                List<MitfinanzierteBestandteileDto> Result = new List<MitfinanzierteBestandteileDto>();

                // Iterate through all FsTypes
                foreach (var LoopFsTyp in FsTypes)
                {


                    // Iterate through all FsTypes
                    foreach (var LoopFsType in FsTypes)
                    {
                        // Create a query
                        var FsArts = (from FsArt in Context.FSART
                                      where FsArt.SYSFSART == LoopFsType.SYSFSART
                                      select FsArt).FirstOrDefault();


                        // Create a query
                        var FsPrices = from FsPrice in Context.FSPREIS
                                       where FsPrice.SYSFSTYP == LoopFsType.SYSFSTYP
                                       && FsPrice.GUELTIGAB < DateTime.Today
                                       select FsPrice;

                        // Set the price to 0
                        decimal Price = 0;

                        // Iterate through all FsPrices
                        foreach (var LoopFsPrice in FsPrices)
                        {
                            // Get the price tab
                            var CurrentFsPriceTab = (from FsPriceTab in Context.FSPREISTAB
                                                     where FsPriceTab.FSPREIS.SYSFSPREIS == LoopFsPrice.SYSFSPREIS
                                                     && FsPriceTab.GUELTIGAB < DateTime.Today
                                                     orderby FsPriceTab.GUELTIGAB descending
                                                     select FsPriceTab).FirstOrDefault();

                            // Add the price
                            Price += CurrentFsPriceTab.PREIS.GetValueOrDefault();
                        }

                        // Create MitfinanzierteBestandteileDto
                        MitfinanzierteBestandteileDto Item = new MitfinanzierteBestandteileDto();

                        // Set the properties
                        Item.FsArt = FsArts.BESCHREIBUNG;
                        Item.FsTyp = LoopFsType.BEZEICHNUNG;
                        Item.FsPreis = Price;
                        Item.SysFsTyp = LoopFsType.SYSFSTYP;
                        Item.FIXVARDEFAULT = LoopFsType.FIXVARDEFAULT;
                        Item.FIXVAROPTION = LoopFsType.FIXVAROPTION;

                        // Add the item to the list
                        Result.Add(Item);
                    }
                }

                // Return the result
                return Result.ToArray();
            }
        }

        public static MitfinanzierteBestandteileDto GetMitfinanzierteBestandteile(DdOlExtended Context, Cic.OpenLease.ServiceAccess.DdOl.FSTYPDto fstyp, OBTypInfo obtyp, long lz, long ll, long sysvart)
        {
            // Create a context

            MitfinanzierteBestandteileDto Item = new MitfinanzierteBestandteileDto();
            string laufzeit = lz.ToString();
            string laufleistung = ll.ToString();
            int interpolationMode = 0;
            VGDao vgd = new VGDao(Context);

            var FsArts = (from FsArt in Context.FSART
                          where FsArt.SYSFSART == fstyp.SYSFSART
                          select FsArt).FirstOrDefault();
            decimal Price = 0;

            int digit1 = fstyp.METHOD < 3 ? fstyp.METHOD : 3;
            int digit2 = fstyp.METHOD - 30;

            // Create a query
            switch (digit1)
            {

                case 1:


                    // Create a query
                    var FsPrices = from FsPrice in Context.FSPREIS
                                   where FsPrice.SYSFSTYP == fstyp.SYSFSTYP
                                   && FsPrice.GUELTIGAB < DateTime.Today
                                   select FsPrice;

                    // Set the price to 0


                    // Iterate through all FsPrices
                    foreach (var LoopFsPrice in FsPrices)
                    {
                        // Get the price tab
                        var CurrentFsPriceTab = (from FsPriceTab in Context.FSPREISTAB
                                                 where FsPriceTab.FSPREIS.SYSFSPREIS == LoopFsPrice.SYSFSPREIS
                                                 && FsPriceTab.GUELTIGAB < DateTime.Today
                                                 orderby FsPriceTab.GUELTIGAB descending
                                                 select FsPriceTab).FirstOrDefault();
                        if (CurrentFsPriceTab != null)
                            // Add the price
                            Price += CurrentFsPriceTab.PREIS.GetValueOrDefault();
                    }
                    break;






                case 2:

                    Price = vgd.deliverVGValue((long)fstyp.SYSVG, DateTime.Now, laufzeit, laufleistung, interpolationMode);

                    break;

                case 3:
                    {

                        try
                        {
                            if (digit2 == 1 && obtyp.SYSVGWR.HasValue) Price = vgd.deliverVGValue((long)obtyp.SYSVGWR, DateTime.Now, laufzeit, laufleistung, interpolationMode);
                            if (digit2 == 2 && obtyp.SYSVGRF.HasValue) Price = vgd.deliverVGValue((long)obtyp.SYSVGRF, DateTime.Now, laufzeit, laufleistung, interpolationMode);
                            if (digit2 == 3 && obtyp.SYSVGRW.HasValue) Price = vgd.deliverVGValue((long)obtyp.SYSVGRW, DateTime.Now, laufzeit, laufleistung, interpolationMode);
                            if (digit2 == 4 && obtyp.SYSVGSI.HasValue) Price = vgd.deliverVGValue((long)obtyp.SYSVGSI, DateTime.Now, "Betrag", fstyp.BEZEICHNUNG, interpolationMode);
                        }
                        catch (Exception ex)
                        {
                            String errorInfo = " Laufzeit: " + laufzeit + " Laufleistung: " + laufleistung;
                            if (digit2 == 1) errorInfo += "SYSVGWR=" + obtyp.SYSVGWR;
                            if (digit2 == 2) errorInfo += "SYSVGRF=" + obtyp.SYSVGRF;
                            if (digit2 == 3) errorInfo += "SYSVGRW=" + obtyp.SYSVGRW;
                            if (digit2 == 4) errorInfo += "SYSVGSI=" + obtyp.SYSVGSI;
                            _Log.Error("Berechnung FSTYP aus Objekttyp fehlgeschlagen: " + errorInfo, ex);
                        }
                    }
                    break;

            }


            // Set the properties
            decimal TaxRate = LsAddHelper.GetTaxRate(Context, sysvart);
            Item.SysFsTyp = fstyp.SYSFSTYP;
            Item.FsArt = FsArts.BESCHREIBUNG;
            Item.FsTyp = fstyp.BEZEICHNUNG;
            Item.ARTCODE = fstyp.ARTCODE;
            /* if ("40".Equals(obtyp.AKLASSE) && FsArts.BESCHREIBUNG.Equals(CnstReifenFsArt))
             {
                 return null;
             }*/

            Item.NEEDED = fstyp.NEEDED;
            Item.DISABLED = fstyp.DISABLED;
            Item.FsPreisNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(Price, TaxRate);
            Item.FsPreisUst = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(Price, TaxRate);
            Item.FsPreisNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Item.FsPreisNetto);
            Item.FsPreisUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Item.FsPreisUst);
            Item.FsPreis = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Price);

            return Item;

        }
        private static CacheDictionary<long, List<PRFSTDto>> prfsCache = CacheFactory<long, List<PRFSTDto>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);

        /// <summary>
        /// Delivers a list of all configured availabilities
        /// </summary>
        /// <param name="sysPrProduct"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<PRFSTDto> DeliverAvailabilities(DdOlExtended context, long sysPrProduct)
        {

            if (!prfsCache.ContainsKey(sysPrProduct))
            {
                List<PRFSTDto> rList = new List<PRFSTDto>();

                try
                {


                    //Parameters for query
                    System.Data.Common.DbParameter[] Parameters = 
                        { 
                           new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysPrProduct", Value = sysPrProduct}
                        };

                    string Query = "SELECT *  FROM CIC.PRFST_V where SYSPRPRODUCT=:sysPrProduct";

                    rList.AddRange(context.ExecuteStoreQuery<PRFSTDto>(Query, Parameters).ToList());

                }
                catch
                {
                    throw;
                }


                prfsCache[sysPrProduct] = rList;
            }
            return prfsCache[sysPrProduct];
        }

        private static CacheDictionary<String, List<FSTYPDto>> fstypCache = CacheFactory<String, List<FSTYPDto>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        public static MitfinanzierteBestandteileDto[] GetMitfinanzierteBestandteileProduct(long sysprproduct, long sysobtyp, long sysprkgroup, long syskdtyp, long sysobart, long lz, long ll, bool mitfinanziert)
        {
            // Create a context

            // Create a result list
            List<MitfinanzierteBestandteileDto> Result = new List<MitfinanzierteBestandteileDto>();
            List<FSTYPDto> FsTypDtoList = null;
            using (DdOlExtended Context = new DdOlExtended())
            {

                PrismaDao pd = new CachedPrismaDao();
                if (sysprproduct == 0) return Result.ToArray();
                CIC.Database.PRISMA.EF6.Model.VART va = pd.getVertragsart(sysprproduct);
                if (va == null) return Result.ToArray();
                long sysvart = va.SYSVART;// PRPRODUCTHelper.DeliverSYSVART(Context, sysprproduct);

                OBTypInfo obtyp = Context.ExecuteStoreQuery<OBTypInfo>("select sysobtyp, sysvgrw, sysvgwr, sysvgrf, sysvgsi from obtyp where sysobtyp=" + sysobtyp, null).FirstOrDefault();

                try
                {
                    String key = sysprproduct + "_" + sysprkgroup + "_" + sysobtyp + "_" + sysobart + "_" + syskdtyp;

                    if (!fstypCache.ContainsKey(key))
                    {
                        //Parameters for query
                        System.Data.Common.DbParameter[] Parameters = 
                        {   
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysPrProduct", Value = sysprproduct},
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysPrkGroup", Value = sysprkgroup}, 
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysObTyp", Value = sysobtyp},
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysObArt", Value = sysobart},
                            new Devart.Data.Oracle.OracleParameter{ ParameterName = "sysKdTyp", Value = syskdtyp}
                        };

                        string Query = "SELECT distinct p.*,fa.beschreibung artcode  FROM fstyp p,fsart fa, TABLE(CIC.CIC_PRISMA_UTILS.DeliverAvailFsTyp(:sysPrProduct, :sysPrkGroup, :sysObTyp, :sysObArt, :sysKdTyp)) t where p.sysfstyp = t.sysid and fa.sysfsart=p.sysfsart";
                        //string Query = "SELECT p.* FROM fstyp p,TABLE(CIC.CIC_PRISMA_UTILS.DeliverAvailFsTypNew(:sysPrProduct :sysPrkGroup, :sysVsArt, :sysObTyp, :sysObArt, :sysKdTyp)) t where p.sysfstyp = t.sysid";
                        //#145216
                        fstypCache[key] = Context.ExecuteStoreQuery<FSTYPDto>(Query, Parameters).ToList();
                    }
                    FsTypDtoList = fstypCache[key];

                    List<PRFSTDto> availabilities = FsPreisHelper.DeliverAvailabilities(Context, sysprproduct);//maske serviceposition ist relevant für die aktiv/schaltbar properties
                    List<FSTYPDto> tempList = new List<FSTYPDto>();
                    //NEEDED = Aktiv / nicht aktiv
                    //DISABLED =  readonly /Änderbar/"Nicht schaltbar"
                    //DISABLED==1 && NEEDED=0 -> entfernen
                    int mitfin = mitfinanziert ? 1 : 0;



                    foreach (FSTYPDto fs in FsTypDtoList)
                    {
                        //Nicht schaltbar
                        fs.DISABLED = 1;//nicht änderbar, ausser korrekt konfiguriert, nur dann korrekt konfiguriert, wenn mitfinflag = 1
                        fs.NEEDED = 0;//Aktiv

                        //FSART
                        var disable1 = from a in availabilities
                                       where (a.MITFINFLAG == mitfin) && a.METHOD == 1 && a.SYSFSART == fs.SYSFSART
                                       select a;
                        if (disable1.Count() > 0)
                        {
                            PRFSTDto prfs = disable1.FirstOrDefault();
                            fs.DISABLED = (int)prfs.DISABLEDFLAGPOS;
                            fs.NEEDED = (int)prfs.NEEDEDPOS;
                        }

                        //Geber
                        var disable2 = from a in availabilities
                                       where (a.MITFINFLAG == mitfin) && a.METHOD == 2 && a.SYSFS == fs.SYSFS
                                       select a;
                        if (disable2.Count() > 0)
                        {
                            PRFSTDto prfs = disable2.FirstOrDefault();
                            fs.DISABLED = (int)prfs.DISABLEDFLAGPOS;
                            fs.NEEDED = (int)prfs.NEEDEDPOS;
                        }

                        //FStyp
                        //Unabhängig von der Zuweisungsmethode (Art, Geber, Typ) sind immer die Verfügbarkeiten am FSTYP zu berücksichtigen.
                        var disable3 = from a in availabilities
                                       where (a.MITFINFLAG == mitfin) && a.METHOD == 3 && a.SYSFSTYP == fs.SYSFSTYP
                                       select a;
                        if (disable3.Count() > 0)
                        {
                            PRFSTDto prfs = disable3.FirstOrDefault();
                            fs.DISABLED = (int)prfs.DISABLEDFLAGPOS;
                            fs.NEEDED = (int)prfs.NEEDEDPOS;
                        }

                        if (!(fs.DISABLED == 1 && fs.NEEDED == 0))//nicht änderbar und nicht ausgewählt -> entfernen, andere verwenden
                            tempList.Add(fs);
                    }
                    FsTypDtoList = tempList;

                }
                catch
                {
                    throw;
                }





                


               


                decimal Ust = LsAddHelper.GetTaxRate(Context, sysvart);
                foreach (var LoopFsTypDtoList in FsTypDtoList)
                {
                    try
                    {
                        MitfinanzierteBestandteileDto dto = GetMitfinanzierteBestandteile(Context, LoopFsTypDtoList, obtyp, lz, ll, sysvart);

                        if (dto != null)
                        {


                            //Subvention Gebühr--------------------
                            Subvention sub = new Subvention(Context);
                            decimal subvention = dto.FsPreis - sub.deliverSubvention(dto.FsPreis, sysprproduct, Subvention.CnstAREA_SERVICE, (long)dto.SysFsTyp, lz, Ust);

                            if (subvention != 0)
                            {
                                MitfinanzierteBestandteileDto dtoSub = new MitfinanzierteBestandteileDto();
                                dtoSub.ARTCODE = dto.ARTCODE;
                                dtoSub.DISABLED = dto.DISABLED;
                                dtoSub.FIXVARDEFAULT = dto.FIXVARDEFAULT;
                                dtoSub.FIXVAROPTION = dto.FIXVAROPTION;
                                dtoSub.FsArt = dto.FsArt;
                                dtoSub.FsPreis = -1 * subvention;
                                dtoSub.FsPreisNetto = dto.FsPreisNetto;
                                dtoSub.FsPreisUst = dto.FsPreisUst;
                                dtoSub.FsTyp = dto.FsTyp;
                                dtoSub.NEEDED = 1;
                                dtoSub.Subvention = subvention;
                                dtoSub.SysFsTyp = dto.SysFsTyp;
                                dtoSub.isSubvention = true;

                                dto.Subvention = subvention;

                                decimal TaxRate = LsAddHelper.GetTaxRate(Context, sysvart);
                                dtoSub.FsPreisNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(dtoSub.FsPreis, TaxRate);
                                dtoSub.FsPreisUst = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateTaxValueFromGrossValue(dtoSub.FsPreis, TaxRate);
                                dtoSub.FsPreisNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(dtoSub.FsPreisNetto);
                                dtoSub.FsPreisUst = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(dtoSub.FsPreisUst);
                                dtoSub.FsPreis = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(dtoSub.FsPreis);
                                dtoSub.FsTyp = "Subvention für " + dtoSub.FsTyp;
                                Result.Add(dtoSub);
                                _Log.Info("Subvention für FSTYP " + dto.SysFsTyp + ": " + subvention);
                            }
                            //Ende Subvention-------------------------------------
                            Result.Add(dto);

                        }
                    }
                    catch (Exception me)
                    {

                        // Log the exception
                        _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverMitfinanzierteBestandteileFailed + " for FSTYP: " + LoopFsTypDtoList.SYSFSTYP + "(" + LoopFsTypDtoList.BEZEICHNUNG + ") " + me.Message, me);

                        // Throw the exception
                        // throw TopLevelException;
                        //throw;
                    }
                }




                // Return the result
                return Result.OrderBy(t => t.FsTyp).ToArray();

            }
        }

        #endregion

        #region My methods
        private static List<FSTYP> MyGetFsTypes(long SysFsArt, DdOlExtended entities)
        {
            List<FSTYP> FSTYPList = null;

            // Query FSTYP
            var CurrentFsTyp = (from FsTyp in entities.FSTYP
                                where FsTyp.SYSFSART == SysFsArt
                                orderby FsTyp.SYSFSART descending
                                select FsTyp);

            FSTYPList = CurrentFsTyp.ToList<FSTYP>();

            // Check if FsTyp is found
            if ((FSTYPList == null) || (FSTYPList.Count <= 0))
            {
                // Throw an exception
                throw new ApplicationException("Could not find appropriate FSTYP in the database");
            }

            // Return FSTYPList
            return FSTYPList;
        }

        private static List<FSPREISTAB> MyGetFsPriceTabs(long SysFsPrice, DdOlExtended entities)
        {

            List<FSPREISTAB> FSPREISTABList = null;

            // Query FSPREISTAB
            var Prices1 = from Price in entities.FSPREISTAB
                          where Price.FSPREIS.SYSFSPREIS == SysFsPrice && Price.GUELTIGAB <= System.DateTime.Now
                          orderby Price.GUELTIGAB descending
                          select Price;

            FSPREISTAB fst = Prices1.FirstOrDefault();
            // Check if the price is found
            if ((fst == null))
            {
                // Throw an exception
                throw new ApplicationException("Price not found in the database.");
            }

            var Prices = from Price in entities.FSPREISTAB
                         where Price.FSPREIS.SYSFSPREIS == SysFsPrice && Price.GUELTIGAB == fst.GUELTIGAB
                         select Price;

            FSPREISTABList = Prices.ToList<FSPREISTAB>();

            // Check if the price is found
            if ((FSPREISTABList == null) || (FSPREISTABList.Count <= 0))
            {
                // Throw an exception
                throw new ApplicationException("Price not found in the database.");
            }

            // Return the price
            return FSPREISTABList;
        }

        private static List<FSPREIS> MygetFsPrices(IQueryable<FSPREIS> query)
        {
            // Get FsPreis
            List<FSPREIS> CurrentFsPrices = query.OrderByDescending(Price => Price.GUELTIGAB).ToList();

            // Check if FsPreis is found
            if ((CurrentFsPrices == null) || (CurrentFsPrices.Count <= 0))
            {
                // Throw an exception
                throw new ApplicationException("Could not find appropriate FSPREIS in the database");
            }

            // Return FsPreis
            return CurrentFsPrices;
        }

        private static FSTYP MyGetFsTyp(long SysFsArt, DdOlExtended entities)
        {
            // Query FSTYP
            var CurrentFsTyp = (from FsTyp in entities.FSTYP
                                where FsTyp.SYSFSART == SysFsArt
                                orderby FsTyp.SYSFSART descending
                                select FsTyp).FirstOrDefault();

            // Check if FsTyp is found
            if (CurrentFsTyp == null)
            {
                // Throw an exception
                throw new ApplicationException("Could not find appropriate FSTYP in the database");
            }

            // Return FSTYP
            return CurrentFsTyp;
        }

        private static FSTYP MyGetFsTyp(long SysFsArt, string bezeichnung, DdOlExtended entities, bool without)
        {
            // Query FSTYP
            var CurrentFsTypQuery = (from FsTyp in entities.FSTYP
                                     where FsTyp.SYSFSART == SysFsArt && FsTyp.BEZEICHNUNG.ToLower().IndexOf(bezeichnung.ToLower()) > -1
                                     orderby FsTyp.SYSFSART descending
                                     select FsTyp);
            if (without)
                CurrentFsTypQuery = (from FsTyp in entities.FSTYP
                                     where FsTyp.SYSFSART == SysFsArt && FsTyp.BEZEICHNUNG.ToLower().IndexOf(bezeichnung.ToLower()) < 0
                                     orderby FsTyp.SYSFSART descending
                                     select FsTyp);

            var CurrentFsTyp = CurrentFsTypQuery.FirstOrDefault();

            // Check if FsTyp is found
            if (CurrentFsTyp == null)
            {
                // Throw an exception
                throw new ApplicationException("Could not find appropriate FSTYP in the database");
            }

            // Return FSTYP
            return CurrentFsTyp;
        }

        public static FSTYP GetFsTypByKey(long SysFsTyp, DdOlExtended entities)
        {
            // Query FSTYP
            var CurrentFsTyp = (from FsTyp in entities.FSTYP
                                where FsTyp.SYSFSTYP == SysFsTyp

                                select FsTyp).FirstOrDefault();

            // Check if FsTyp is found
            if (CurrentFsTyp == null)
            {
                // Throw an exception
                throw new ApplicationException("Could not find appropriate FSTYP in the database");
            }

            // Return FSTYP
            return CurrentFsTyp;
        }

        private static decimal MyGetFsPriceTab(long SysFsPrice, DdOlExtended entities)
        {
            // Query FSPREISTAB
            var LatestPrice = (from Price in entities.FSPREISTAB
                               where Price.FSPREIS.SYSFSPREIS == SysFsPrice
                               orderby Price.GUELTIGAB descending
                               select Price).FirstOrDefault();

            // Check if the price is found
            if (LatestPrice == null)
            {
                // Throw an exception
                throw new ApplicationException("Price not found in the database.");
            }

            // Return the price
            return LatestPrice.PREIS.GetValueOrDefault();
        }

        private static FSPREIS MygetFsPreis(IQueryable<FSPREIS> query)
        {
            // Get FsPreis
            FSPREIS CurrentFsPrice = query.OrderByDescending(Price => Price.GUELTIGAB).FirstOrDefault();

            // Check if FsPreis is found
            if (CurrentFsPrice == null)
            {
                // Throw an exception
                throw new ApplicationException("Could not find appropriate FSPREIS in the database");
            }

            // Return FsPreis
            return CurrentFsPrice;
        }

        private static FSART MyGetFsArt(string name, DdOlExtended entities)
        {
            // Qery FSART
            var CurrentFsArt = (from FsArt in entities.FSART
                                where FsArt.BESCHREIBUNG == name
                                orderby FsArt.SYSFSART descending
                                select FsArt).FirstOrDefault();

            // Check if FsArt is found
            if (CurrentFsArt == null)
            {
                // Throw an exception
                throw new ApplicationException("Could not find FSART \"" + name + "\" in the database");
            }

            // Return FSART
            return CurrentFsArt;
        }
        #endregion
    }
    public class OBTypInfo
    {
        public long sysobtyp { get; set; }
        public long? SYSVGWR { get; set; }
        public long? SYSVGRF { get; set; }
        public long? SYSVGRW { get; set; }
        public long? SYSVGSI { get; set; }

    }
}