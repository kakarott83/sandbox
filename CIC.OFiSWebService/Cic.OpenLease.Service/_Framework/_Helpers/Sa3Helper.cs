namespace Cic.OpenLease.Service
{
    using Cic.OpenLease.Service.Services.DdOl;

    #region Using
    using Cic.OpenLease.ServiceAccess.DdOl;
    using Cic.OpenOne.Common.Model.DdEurotax;
    using Cic.OpenOne.Common.Model.DdOl;
    using Cic.OpenOne.Common.Util.Logging;
    using CIC.Database.OL.EF6.Model;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml;
    using System.Xml.Linq;
    #endregion

    public static class Sa3Helper
    {

        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const string _CnstRW_SA_CODES = "RW_SA_CODES";

        #region Methods
        public static OfferConfiguration GetSa3OfferConfiguration(string sa3XmlData)
        {
            return GetSa3OfferConfiguration(sa3XmlData, null);
        }

        public static VehicleData getCO2Values(XElement VehicleNode, VehicleData Vehicle)
        {
            if (VehicleNode.Element("Emission") == null) return Vehicle;

            XElement em = VehicleNode.Element("Emission");
            if (em.Element("ConsumptionUrban") != null)
                Vehicle.consumptionurban = MyGetDecimal(em.Element("ConsumptionUrban"));

            if (em.Element("ConsumptionNonUrban") != null)
                Vehicle.consumptionnonurban = MyGetDecimal(em.Element("ConsumptionNonUrban"));

            if (em.Element("ConsumptionOverall") != null)
                Vehicle.consumptionoveral = MyGetDecimal(em.Element("ConsumptionOverall"));

            if (em.Element("CO2Urban") != null)
                Vehicle.co2urban = MyGetDecimal(em.Element("CO2Urban"));
            if (em.Element("CO2NonUrban") != null)
                Vehicle.co2nonurban = MyGetDecimal(em.Element("CO2NonUrban"));
            if (em.Element("CO2Overall") != null)
                Vehicle.co2overal = MyGetDecimal(em.Element("CO2Overall"));

            if (em.Element("ParticleCount") != null)
                Vehicle.particlecount = MyGetDecimal(em.Element("ParticleCount"));
            if (em.Element("ParticleMass") != null)
                Vehicle.particlemass = MyGetDecimal(em.Element("ParticleMass"));
            if (em.Element("NitrogenOxide") != null)
                Vehicle.nox = 1000 * MyGetDecimal(em.Element("NitrogenOxide"));

            return Vehicle;
        }
        public static OfferConfiguration GetSa3OfferConfiguration(string sa3XmlData, string etcode)
        {
            try
            {
                // Create XDocument
                XDocument XmlDocument = XDocument.Load(new XmlTextReader(new MemoryStream(Encoding.UTF8.GetBytes(sa3XmlData))));

                // Create a result
                OfferConfiguration ResultConfiguration = new OfferConfiguration(OfferTypeConstants.Sa3);

                // Get the dealer id
                if (XmlDocument.Root.Element("Dealer") != null && XmlDocument.Root.Element("Dealer").Attribute("Number") != null)
                    ResultConfiguration.DealerId = int.Parse(XmlDocument.Root.Element("Dealer").Attribute("Number").Value.Trim());

                // Get offer id
                if (XmlDocument.Root.Attribute("OfferNumber") != null)
                    ResultConfiguration.OfferId = XmlDocument.Root.Attribute("OfferNumber").Value.Trim();

                // Get the currency
                if (XmlDocument.Root.Element("TotalPrice") != null && XmlDocument.Root.Element("TotalPrice").Element("Price") != null && XmlDocument.Root.Element("TotalPrice").Element("Price").Attribute("Currency") != null)
                    ResultConfiguration.Currency = XmlDocument.Root.Element("TotalPrice").Element("Price").Attribute("Currency").Value.Trim();

                // Get the total discount
                if (XmlDocument.Root.Element("Discount") != null)
                    ResultConfiguration.TotalDiscount = MyGetDecimal(XmlDocument.Root.Element("Discount"));

                // Get the model discount
                if (XmlDocument.Root.Element("DiscountModel") != null)
                    ResultConfiguration.ModelDiscount = MyGetDecimal(XmlDocument.Root.Element("DiscountModel"));

                // Get the options discount
                if (XmlDocument.Root.Element("DiscountOptions") != null)
                    //ResultConfiguration.PackagesDiscount = MyGetDecimal(XmlDocument.Root.Element("DiscountOptions"));
                    ResultConfiguration.OptionsDiscount = MyGetDecimal(XmlDocument.Root.Element("DiscountOptions"));

                // Get the packages discount
                if (XmlDocument.Root.Element("DiscountOptionPackage") != null)
                    //ResultConfiguration.OptionsDiscount = MyGetDecimal(XmlDocument.Root.Element("DiscountOptionPackage"));
                    ResultConfiguration.PackagesDiscount = MyGetDecimal(XmlDocument.Root.Element("DiscountOptionPackage"));

                // Get the original accessories discount
                if (XmlDocument.Root.Element("DiscountAccessoryOriginal") != null)
                    ResultConfiguration.OriginalAccessoriesDiscount = MyGetDecimal(XmlDocument.Root.Element("DiscountAccessoryOriginal"));

                // Get the dealer accessories discount
                if (XmlDocument.Root.Element("DiscountAccessoryDealer") != null)
                    ResultConfiguration.DealerAccessoriesDiscount = MyGetDecimal(XmlDocument.Root.Element("DiscountAccessoryDealer"));

                // Get the miscellaneous options discount
                if (XmlDocument.Root.Element("DiscountMiscellaneous") != null)
                    ResultConfiguration.MiscellaneousOptionsDiscount = MyGetDecimal(XmlDocument.Root.Element("DiscountMiscellaneous"));

                // Down payment
                if (XmlDocument.Root.Element("DownPayment") != null)
                    ResultConfiguration.DownPayment = MyGetDecimal(XmlDocument.Root.Element("DownPayment"));

                
                decimal? nova = null;

                // Get the Nova
                if (XmlDocument.Root.Element("AddTax") != null)
                {
                    ResultConfiguration.Nova = MyGetDecimal(XmlDocument.Root.Element("AddTax"));
                    nova = ResultConfiguration.Nova;
                }
                //Get Used Vehicle Data
                ResultConfiguration.Kilometer = 0;

                if (XmlDocument.Root.Element("AddVehicleDetails") != null)
                {
                    XElement addVehNode = XmlDocument.Root.Element("AddVehicleDetails");
                    string IntegerPart = "0";

                    if (addVehNode.Element("Km") != null && addVehNode.Element("Km").Element("Int") != null)
                        IntegerPart = addVehNode.Element("Km").Element("Int").Value.Trim();

                    DateTime dt = DateTime.Now;
                    ResultConfiguration.Kilometer = (long)MyGetDecimalValue(IntegerPart, "0");

                    if (addVehNode.Element("RegistrationDate") != null && addVehNode.Element("RegistrationDate").Element("Date") != null)
                    {
                        ResultConfiguration.Erstzulassung = getDate(addVehNode.Element("RegistrationDate").Element("Date"));
                       
                    }
                }

                // Get the picture URL
                if (XmlDocument.Root.Element("ConfigurationImage") != null)
                    ResultConfiguration.ImageUrl = XmlDocument.Root.Element("ConfigurationImage").Value.Trim();

                // Set the vehicles array
                ResultConfiguration.Vehicle = MyGetVehicleData(XmlDocument.Root.Element("Vehicle"), ResultConfiguration, etcode, nova);
                decimal Ust = 19;// LsAddHelper.getGlobalUst();
                //Correct Brutto/Nova values, SA3 delivers netto without nova
                decimal additionalPackageDiscount = 0;
                if (ResultConfiguration.OptionsPrice == 0 && ResultConfiguration.OptionsDiscount > 0)
                {
                    additionalPackageDiscount = ResultConfiguration.OptionsDiscount;
                    
                }
                
                //NOVANEU
                //NachlassBetrag:  
                //#3805:
                ResultConfiguration.TotalDiscount -= (ResultConfiguration.PackagesDiscount + additionalPackageDiscount+ResultConfiguration.MiscellaneousOptionsDiscount + ResultConfiguration.DealerAccessoriesDiscount + ResultConfiguration.OriginalAccessoriesDiscount);

                //DiscountOptionPackage == Pakete
                ResultConfiguration.PackagesDiscount = addUstAndNova(ResultConfiguration.PackagesDiscount, Ust, ResultConfiguration.Nova,0);
                //DiscountOptions == Sonderausstattungen
                ResultConfiguration.OptionsDiscount = addUstAndNova(ResultConfiguration.OptionsDiscount, Ust, ResultConfiguration.Nova,0);
                //DiscountModel == Modell
                ResultConfiguration.ModelDiscount = addUstAndNova(ResultConfiguration.ModelDiscount, Ust, ResultConfiguration.Nova,0);
                //Discount == Gesamt

                
                if (ResultConfiguration.OptionsPrice == 0 && ResultConfiguration.OptionsDiscount > 0)
                {
                    ResultConfiguration.PackagesDiscount += ResultConfiguration.OptionsDiscount;
                    ResultConfiguration.OptionsDiscount = 0;

                }
                //update the netto-lp correctly it doesnt contain the sonderminderung
                NovaType nt = new NovaType(Ust, (ResultConfiguration.Nova * 100) - 100, NovaType.fetchNovaQuote(), ResultConfiguration.Vehicle.sonderMinderung);
                nt.setNetto(ResultConfiguration.Vehicle.Price);
                decimal newbrutto = nt.bruttoInklNova;// -ResultConfiguration.Vehicle.sonderMinderung;
                nt.setBruttoInklNova(newbrutto);
                ResultConfiguration.Vehicle.PriceNettoNetto = nt.netto;

                ResultConfiguration.Vehicle.Price = addUstAndNova(ResultConfiguration.Vehicle.Price, Ust, ResultConfiguration.Nova,0);//ResultConfiguration.Vehicle.sonderMinderung);
                ResultConfiguration.OptionsPrice = addUstAndNova(ResultConfiguration.OptionsPrice, Ust, ResultConfiguration.Nova,0);
                ResultConfiguration.PackagesPrice = addUstAndNova(ResultConfiguration.PackagesPrice, Ust, ResultConfiguration.Nova,0);

                //DownPayment
                ResultConfiguration.DownPayment = addUstAndNova(ResultConfiguration.DownPayment, Ust, 1,0);
                //DiscountMiscellaneous == Zusatzleistungen


                ResultConfiguration.MiscellaneousOptionsDiscount = addUstAndNova(ResultConfiguration.MiscellaneousOptionsDiscount, Ust, 1, 0);
                //DiscountAccessoryDealer == Händlerzubehör
                ResultConfiguration.DealerAccessoriesDiscount = addUstAndNova(ResultConfiguration.DealerAccessoriesDiscount, Ust, 1, 0);
                //DiscountAccessoryOriginal == Original Zubehör
                ResultConfiguration.OriginalAccessoriesDiscount = addUstAndNova(ResultConfiguration.OriginalAccessoriesDiscount, Ust, 1, 0);

                ResultConfiguration.TotalDiscount = addUstAndNova(ResultConfiguration.TotalDiscount, Ust, ResultConfiguration.Nova, 0);
                ResultConfiguration.OriginalAccessoriesPrice = addUstAndNova(ResultConfiguration.OriginalAccessoriesPrice, Ust, 1, 0);
                ResultConfiguration.DealerAccessoriesPrice = addUstAndNova(ResultConfiguration.DealerAccessoriesPrice, Ust, 1,0);
                ResultConfiguration.MiscellaneousOptionsPrice = addUstAndNova(ResultConfiguration.MiscellaneousOptionsPrice, Ust, 1,0);

                // Return the configuration
                return ResultConfiguration;
            }
            catch (Exception e)
            {
                throw new ApplicationException("Sa3 XML data is not valid", e);
            }
        }

        private static DateTime getDate(XElement dateNode)
        {

            string dayPart = "" + DateTime.Now.Day;
            if (dateNode.Element("Day") != null)
                dayPart = dateNode.Element("Day").Value.Trim();
            string monthPart = "" + DateTime.Now.Month;
            if (dateNode.Element("Month") != null)
                monthPart = dateNode.Element("Month").Value.Trim();
            string yearPart = "" + DateTime.Now.Year;
            if (dateNode.Element("Year") != null)
                yearPart = dateNode.Element("Year").Value.Trim();

            return new DateTime((int)decimal.Parse(yearPart), (int)decimal.Parse(monthPart), (int)decimal.Parse(dayPart));
        }
        public static decimal addUstAndNova(decimal value, decimal Ust, decimal Nova, decimal sonderMinderung)
        {
            if (Nova == 1)
            {
                value = value * (1 + Ust / 100);
                value *= Nova;
                value = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(value);
                return value;
            }
            NovaType nt = new NovaType(Ust, (Nova * 100) - 100, NovaType.fetchNovaQuote(), sonderMinderung);
            nt.setNetto(value);

            return Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(nt.bruttoInklNova);

            
            /*
             * decimal result = valueNetto * (1 + Ust / 100);
            result += valueNetto * Nova;
            result += valueNetto * NovaZuAb;
            result = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(result);
            return result;*/
        }

        public static List<FzConfiguration> GetSa3FzConfiguration(string sa3XmlData)
        {
            try
            {
                _Log.Debug("Content of SA3FZ: " + sa3XmlData + "EOF");
                // Create XDocument
                XDocument XmlDocument = XDocument.Load(new XmlTextReader(new MemoryStream(Encoding.UTF8.GetBytes(sa3XmlData))));
                List<FzConfiguration> fzConfigurationList = new List<FzConfiguration>();
                OfferConfiguration offerCfg = new OfferConfiguration(OfferTypeConstants.Sa3);
                offerCfg.Vehicle = new VehicleData();

                // Set the vehicles array
                if (XmlDocument.Root.Element("Vehicle") != null)
                {    // Get all options
                    // Create options list

                    decimal? nova = null;

                    // Get the Nova
                    if (XmlDocument.Root.Element("AddTax") != null)
                        nova = MyGetDecimal(XmlDocument.Root.Element("AddTax"));

                    XElement VehicleNode = XmlDocument.Root.Element("Vehicle");
                    // Create options list
                    List<VehicleOptionData> OptionsList = new List<VehicleOptionData>();

                    // Get all options
                    foreach (var LoopOption in VehicleNode.Elements("Option"))
                        OptionsList.Add(MyGetOptionData(LoopOption, null));

                    List<String> optionCodes = new List<String>();
                    String oka = VehicleNode.Attribute("InternalCode").Value.Trim();
                    // Get the Eurotax numbers
                    ObtypMapInfo[] etcodes = MyTranslateCodes(oka, OptionsList.ToArray(), optionCodes, nova);
                    String Sas = string.Join(",", optionCodes.ToArray());

                    getCO2Values(VehicleNode, offerCfg.Vehicle);
                    int novaWert = (int) ((nova - 1)*100);
                    foreach (var LoopEtcodes in etcodes)
                    {
                        //HIER MUSS SA3 Zusatzinfo zugewiesen werden!
                        FzConfiguration fzCfg = GetFzConfiguration(LoopEtcodes, Sas, oka);
                        //Dont display sa-imported values, Fix #4917
                        /*fzCfg.CO2 = offerCfg.Vehicle.co2overal;
                        fzCfg.Nova = novaWert;
                        fzCfg.NOx = offerCfg.Vehicle.nox;
                        fzCfg.Verbrauch = offerCfg.Vehicle.consumptionoveral;*/
                        fzConfigurationList.Add(fzCfg);

                    }


                }
                return fzConfigurationList;
            }
            catch (Exception e)
            {
                throw new ApplicationException("Sa3 XML data is not valid", e);
            }
        }


        private static ObtypMapInfo[] MyTranslateCodes(string internalCode, VehicleOptionData[] options, List<String> optionCodes, decimal? nova)
        {
            // Default values

            try
            {
                // Check if internalCode is empty
                if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(internalCode))
                {
                    // Throw an exception
                    throw new Exception("internalCode");
                }

                // Check if options are null
                if (options == null)
                {
                    // Throw an exception
                    throw new ArgumentException("options");
                }


                List<string> etcodes = EurotaxNumberHelper.GetOptionCodes(internalCode);
                _Log.Debug("SA3-Matching: " + internalCode + " -> " + etcodes);
                // Loop through all options
                foreach (var LoopOption in options)
                {

                    if (etcodes.Contains(LoopOption.Code))//use every possible option just once
                    {
                        etcodes.Remove(LoopOption.Code);
                        optionCodes.Add(LoopOption.Code);

                    }


                }

                // Execute the method from helper and return its result
                return EurotaxNumberHelper.GetEurotaxNumbersFromOka(internalCode, optionCodes, nova);
            }
            catch (Exception e)
            {
                // Log the exception
                _Log.Error("Could not translate OKA code to Eurotax number: Code: " + internalCode + " Reason: " + e.Message, e);
                // Throw an exception
                throw new ApplicationException("Could not translate OKA code to Eurotax number: Code: " + internalCode + " Reason: " + e.Message, e);
            }
        }






        public static ITConfiguration GetSa3ITConfiguration(string sa3XmlData)
        {
            try
            {
                _Log.Debug("Content of SA3IT: " + sa3XmlData + "EOF");

                // Create XDocument
                XDocument XmlDocument = XDocument.Load(new XmlTextReader(new MemoryStream(Encoding.UTF8.GetBytes(sa3XmlData))), LoadOptions.None);

                // Create a result
                ITConfiguration ResultConfiguration = new ITConfiguration(ITTypeConstants.Sa3);


                // Get the ITid
                if (XmlDocument.Root.Attribute("ID") != null)
                    ResultConfiguration.ITId = XmlDocument.Root.Attribute("ID").Value.Trim();

                try
                {
                    // Get the ITid
                    if (XmlDocument.Root.Attribute("Type") != null)
                        ResultConfiguration.PartnerTyp = int.Parse(XmlDocument.Root.Attribute("Type").Value.Trim());
                }
                catch (Exception) { }

                try
                {
                    // Get the ITid
                    if (XmlDocument.Root.Attribute("Flag") != null)
                        ResultConfiguration.Flag = int.Parse(XmlDocument.Root.Attribute("Flag").Value.Trim());
                }
                catch (Exception) { }


                // Get the Gender
                if (XmlDocument.Root.Element("Gender") != null)
                    ResultConfiguration.Gender = XmlDocument.Root.Element("Gender").Value.Trim();

                ResultConfiguration.Title = "0";
                // Get the Title
                if (XmlDocument.Root.Element("Title") != null)
                    ResultConfiguration.Title = XmlDocument.Root.Element("Title").Value.Trim();
                if (ResultConfiguration.Title != null)
                    ResultConfiguration.Title = ResultConfiguration.Title.Trim();
                if (ResultConfiguration.Title.Length == 0)
                    ResultConfiguration.Title = "0";

                // Get the Prename
                if (XmlDocument.Root.Element("Prename") != null)
                    ResultConfiguration.Prename = XmlDocument.Root.Element("Prename").Value.Trim();

                // Get the Surname
                if (XmlDocument.Root.Element("Surname") != null)
                    ResultConfiguration.Surname = XmlDocument.Root.Element("Surname").Value.Trim();

                // Get the Surname2
                if (XmlDocument.Root.Element("Surname2") != null)
                    ResultConfiguration.Surname2 = XmlDocument.Root.Element("Surname2").Value.Trim();

                // Get the VATRegNumber
                if (XmlDocument.Root.Element("VATRegNumber") != null)
                    ResultConfiguration.VATRegNumber = XmlDocument.Root.Element("VATRegNumber").Value.Trim();

                // Get the VATRegNumberInternational
                if (XmlDocument.Root.Element("VATRegNumberInternational") != null)
                    ResultConfiguration.VATRegNumberInternational = XmlDocument.Root.Element("VATRegNumberInternational").Value.Trim();

                // Get the VATGroupKey
                /* if (XmlDocument.Root.Element("VATGroupKey") != null)
                     ResultConfiguration.VATGroupKey = int.Parse( XmlDocument.Root.Element("VATGroupKey").Value.Trim());
                 */
                // Get the Street
                if (XmlDocument.Root.Element("Address") != null && XmlDocument.Root.Element("Address").Element("Street") != null)
                    ResultConfiguration.Street = XmlDocument.Root.Element("Address").Element("Street").Value.Trim();

                // Get the StreetNumber
                if (XmlDocument.Root.Element("Address") != null && XmlDocument.Root.Element("Address").Element("StreetNumber") != null)
                    ResultConfiguration.StreetNumber = XmlDocument.Root.Element("Address").Element("StreetNumber").Value.Trim();

                // Get the PoBox
                if (XmlDocument.Root.Element("Address") != null && XmlDocument.Root.Element("Address").Element("PoBox") != null)
                    ResultConfiguration.StreetNumber = XmlDocument.Root.Element("Address").Element("PoBox").Value.Trim();


                // Get the Zip
                if (XmlDocument.Root.Element("Address") != null && XmlDocument.Root.Element("Address").Element("Zip") != null)
                    ResultConfiguration.Zip = XmlDocument.Root.Element("Address").Element("Zip").Value.Trim();

                // Get the City
                if (XmlDocument.Root.Element("Address") != null && XmlDocument.Root.Element("Address").Element("City") != null)
                    ResultConfiguration.City = XmlDocument.Root.Element("Address").Element("City").Value.Trim();

                // Get the Country
                if (XmlDocument.Root.Element("Address") != null && XmlDocument.Root.Element("Address").Element("Country") != null)
                    ResultConfiguration.Country = XmlDocument.Root.Element("Address").Element("Country").Value.Trim();

                // Get the Phone
                if (XmlDocument.Root.Element("Address") != null && XmlDocument.Root.Element("Address").Element("Phone") != null)
                    ResultConfiguration.Phone = XmlDocument.Root.Element("Address").Element("Phone").Value.Trim();

                // Get the Fax
                if (XmlDocument.Root.Element("Address") != null && XmlDocument.Root.Element("Address").Element("Fax") != null)
                    ResultConfiguration.Fax = XmlDocument.Root.Element("Address").Element("Fax").Value.Trim();

                // Get the Email
                if (XmlDocument.Root.Element("Address") != null && XmlDocument.Root.Element("Address").Element("Email") != null)
                    ResultConfiguration.Email = XmlDocument.Root.Element("Address").Element("Email").Value.Trim();

                // Get the AddressField1
                if (XmlDocument.Root.Element("Address") != null && XmlDocument.Root.Element("Address").Element("AddressField1") != null)
                {
                    ResultConfiguration.AddressField1 = XmlDocument.Root.Element("Address").Element("AddressField1").Value.Trim();
                    ResultConfiguration.Street = ResultConfiguration.AddressField1;
                }

                // Get the AddressField2
                if (XmlDocument.Root.Element("Address") != null && XmlDocument.Root.Element("Address").Element("AddressField2") != null)
                    ResultConfiguration.AddressField2 = XmlDocument.Root.Element("Address").Element("AddressField2").Value.Trim();

                // Get the AddressField3
                if (XmlDocument.Root.Element("Address") != null && XmlDocument.Root.Element("Address").Element("AddressField3") != null)
                    ResultConfiguration.AddressField3 = XmlDocument.Root.Element("Address").Element("AddressField3").Value.Trim();

                // Get the AddressType
                if (XmlDocument.Root.Element("Address") != null && XmlDocument.Root.Element("Address").Element("AddressType") != null)
                    ResultConfiguration.AddressType = XmlDocument.Root.Element("Address").Element("AddressType").Value.Trim();

                // Get the Phone1
                if (XmlDocument.Root.Element("Address") != null && XmlDocument.Root.Element("Address").Element("Phone1") != null)
                    ResultConfiguration.Phone1 = XmlDocument.Root.Element("Address").Element("Phone1").Value.Trim();

                // Get the Phone2
                if (XmlDocument.Root.Element("Address") != null && XmlDocument.Root.Element("Address").Element("Phone2") != null)
                    ResultConfiguration.Phone2 = XmlDocument.Root.Element("Address").Element("Phone2").Value.Trim();

                // Get the Phone3
                if (XmlDocument.Root.Element("Address") != null && XmlDocument.Root.Element("Address").Element("Phone3") != null)
                    ResultConfiguration.Phone3 = XmlDocument.Root.Element("Address").Element("Phone3").Value.Trim();

                // Get the Salutation
                if (XmlDocument.Root.Element("Salutation") != null)
                    ResultConfiguration.Salutation = XmlDocument.Root.Element("Salutation").Value.Trim();

                // Get the Branch
                if (XmlDocument.Root.Element("Branch") != null)
                    ResultConfiguration.Branch = XmlDocument.Root.Element("Branch").Value.Trim();

                // Get the Birthday
                if (XmlDocument.Root.Element("Address")!=null && XmlDocument.Root.Element("Address").Element("Birthday") != null && XmlDocument.Root.Element("Address").Element("Birthday").Element("Date") != null)
                {
                    DateTime gebDatum = getDate(XmlDocument.Root.Element("Address").Element("Birthday").Element("Date"));
                    ResultConfiguration.BirthDay = gebDatum.Day;
                    ResultConfiguration.BirthMonth = gebDatum.Month;
                    ResultConfiguration.BirthYear = gebDatum.Year;

                }

              

                // Get the CustomerLanguage
                if (XmlDocument.Root.Element("CustomerLanguage") != null)
                    ResultConfiguration.CostumerLanguage = XmlDocument.Root.Element("CustomerLanguage").Value.Trim();


                // Get the Birthday
                if (XmlDocument.Root.Element("Date") != null )
                {
                    DateTime gebDatum = getDate(XmlDocument.Root.Element("Date"));
                    ResultConfiguration.Day = gebDatum.Day;
                    ResultConfiguration.Month = gebDatum.Month;
                    ResultConfiguration.Year = gebDatum.Year;

                }
               /* //KOMMKANAL
                * <select id="ERREICHBTREL" name="ERREICHBTREL" class=""><option selected="selected" value="0" style="color: menutext;">Bitte wählen</option><option value="2" style="color: menutext;">Mobiltelefon</option><option value="1" style="color: menutext;">Telefon geschäftlich</option><option value="3" style="color: menutext;">Telefon privat</option><option value="4" style="color: menutext;">E-Mail</option><option value="5" style="color: menutext;">Fax</option></select>
                */

                // Return the configuration
                return ResultConfiguration;
            }
            catch (Exception e)
            {
                throw new ApplicationException("Sa3 XML data is not valid", e);
            }
        }

        #endregion

        #region My methods
        private static decimal MyGetDecimalValue(string integerPart, string decimalPart)
        {
            string DecimalSeparator = System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            string TextValue = integerPart + DecimalSeparator + decimalPart;

            return decimal.Parse(TextValue);
        }

        private static VehicleData MyGetVehicleData(XElement VehicleNode, OfferConfiguration ResultConfiguration, String etcode, decimal? nova)
        {

            // Create vehicle data
            VehicleData ResultVehicle = new VehicleData();

            // Get the brand name
            switch (VehicleNode.Attribute("Brand").Value.Trim())
            {
                case "BM": ResultVehicle.BrandName = "BMW"; break;
                case "MI": ResultVehicle.BrandName = "Mini"; break;
                case "C1": ResultVehicle.BrandName = "C1"; break;
                case "MR": ResultVehicle.BrandName = "Motorcycles"; break;
                case "BI": ResultVehicle.BrandName = "BMW"; break;
            }

            // Set the manufacturer
            ResultVehicle.ManufacturerName = "BMW";

            // Get the additional type
            if (VehicleNode.Attribute("AddType") != null)
                ResultVehicle.AdditionalType = short.Parse(VehicleNode.Attribute("AddType").Value.Trim());

            // Get the type
            if (VehicleNode.Attribute("Type") != null)
                ResultVehicle.Type = (VehicleTypeConstants)int.Parse(VehicleNode.Attribute("Type").Value.Trim());

            // Get the model name
            ResultVehicle.Name = VehicleNode.Element("Name").Value.Trim();

            // Get the model price, thats currently nettonetto
            if (VehicleNode.Element("ModelPrice") != null)
                ResultVehicle.Price = MyGetDecimal(VehicleNode.Element("ModelPrice"));

            ResultVehicle.sonderMinderung = 0;
            if (VehicleNode.Element("NoVA") != null)//may contain -350 for diesel bonus
                ResultVehicle.sonderMinderung = MyGetDecimal(VehicleNode.Element("NoVA"));


            ResultVehicle.PriceNettoNetto = ResultVehicle.Price;//remember the nettonetto value

            // Get the VAT percentage
            if (VehicleNode.Element("VATPercentage") != null)
                ResultVehicle.VatPercentage = MyGetDecimal(VehicleNode.Element("VATPercentage"));

            // Get manufactured year
            if (VehicleNode.Element("ManufacturedYear") != null)
                ResultVehicle.ManufacturedYear = int.Parse(VehicleNode.Element("ManufacturedYear").Value.Trim());

            // Create options list
            List<VehicleOptionData> OptionsList = new List<VehicleOptionData>();

            ResultVehicle.SARVPriceNetNoNova = 0;

            //validate SA-Options for ResidualValue Correction....
            //hier werden die restwertrelevanten sonderausstattungen addiert für die BMW Restwertermittlung
            using (DdOlExtended Context = new DdOlExtended())
            {
                var ObTyp = (from ob in Context.OBTYP
                             where ob.SCHWACKE == etcode
                             select ob).FirstOrDefault();
                if (ObTyp != null && ObTyp.NOEXTID > 0)
                {
                    ResultConfiguration.UseFzData = true;

                }
                KORREKTURDao kh = new KORREKTURDao(Context);

                // Get all options
                foreach (var LoopOption in VehicleNode.Elements("Option"))
                {
                    VehicleOptionData option = MyGetOptionData(LoopOption, ResultConfiguration);
                    OptionsList.Add(option);

                    decimal test = kh.CorrectType(_CnstRW_SA_CODES, 0, "+", DateTime.Now, option.Code, "", KORREKTURDao.TYPE_STRING, KORREKTURDao.TYPE_STRING);
                    if (test > 0)
                        ResultVehicle.SARVPriceNetNoNova += option.Price;
                }

            }
            


            // Set the options
            ResultVehicle.Options = OptionsList.ToArray();

            // Get the Eurotax number
            if (etcode != null)
                ResultVehicle.Code = etcode;
            else
                ResultVehicle.Code = MyTranslateCode(VehicleNode.Attribute("InternalCode").Value, ResultVehicle.Options, nova);


            getCO2Values(VehicleNode, ResultVehicle);


            // Return the vehicle data
            return ResultVehicle;
        }

        private static VehicleOptionData MyGetOptionData(XElement OptionNode, OfferConfiguration ResultConfiguration)
        {
            try
            {
                // Create the option
                VehicleOptionData ResultOption = new VehicleOptionData();

                // Get the code
                ResultOption.Code = OptionNode.Attribute("InternalCode").Value.Trim();

                // Get the name
                ResultOption.Name = OptionNode.Element("Name").Value.Trim();

                // Get the option price
                ResultOption.Price = MyGetDecimal(OptionNode.Element("Price"));

                // Get the VAT percentage
                if (OptionNode.Element("VATPercentage") != null)
                    ResultOption.VatPercentage = MyGetDecimal(OptionNode.Element("VATPercentage"));

                // Assume this is not a package
                bool IsPackage = false;

                // Get the package flag
                if (OptionNode.Attribute("Package") != null)
                {
                    IsPackage = OptionNode.Attribute("Package").Value == "true";
                }

                // Assume the option type is empty
                string OptionTypeStr = string.Empty;

                // Get the option type
                if (OptionNode.Attribute("Type") != null)
                {
                    OptionTypeStr = OptionNode.Attribute("Type").Value.Trim();
                }

                // Determine the option type
                if (Regex.IsMatch(ResultOption.Code, "^S.{4}$") && OptionTypeStr == "OPTION" && IsPackage)
                {

                    ResultOption.Code = ResultOption.Code.Substring(2);
                    // This is a package
                    ResultOption.Type = OptionTypeConstants.Package;
                }
                else if (Regex.IsMatch(ResultOption.Code, "^S.{4}$") && OptionTypeStr == "OPTION" && !IsPackage)
                {
                    ResultOption.Code = ResultOption.Code.Substring(2);
                    // This is an option
                    ResultOption.Type = OptionTypeConstants.Option;
                }
                else if (Regex.IsMatch(ResultOption.Code, "^F.{4}$") && OptionTypeStr == "TRIM" && !IsPackage)
                {
                    ResultOption.Code = ResultOption.Code.Substring(2);
                    // This is an option
                    ResultOption.Type = OptionTypeConstants.Option;
                    if (ResultConfiguration != null)
                    {
                        ResultConfiguration.PolsterCode = ResultOption.Code;
                        ResultConfiguration.PolsterText = ResultOption.Name;

                    }
                }
                else if (Regex.IsMatch(ResultOption.Code, "^P.{4}$") && OptionTypeStr == "COLOR" && !IsPackage)
                {
                    ResultOption.Code = ResultOption.Code.Substring(2);
                    // This is an option
                    ResultOption.Type = OptionTypeConstants.Option;
                    if (ResultConfiguration != null)
                    {
                        ResultConfiguration.Farbea = ResultOption.Name;
                    }
                }
                else if (Regex.IsMatch(ResultOption.Code, "^Z.{4}$") && OptionTypeStr == "ACCESSORY" && !IsPackage)
                {
                    ResultOption.Code = ResultOption.Code.Substring(2);
                    // This is an original accessory
                    ResultOption.Type = OptionTypeConstants.OriginalAccessory;
                }
                else if (!Regex.IsMatch(ResultOption.Code, "^Z.{4}$") && OptionTypeStr == "ACCESSORY" && !IsPackage)
                {
                    ResultOption.Code = ResultOption.Code.Substring(2);
                    // This is a dealer accessory
                    ResultOption.Type = OptionTypeConstants.DealerAccessory;
                }
                else
                {
                    // This is a miscelaneous accessory
                    ResultOption.Type = OptionTypeConstants.Miscellaneous;
                }

                // Return the option
                return ResultOption;
            }
            catch (Exception exception)
            {
                throw new Exception("Could not get an option data.", exception);
            }
        }

        private static decimal MyGetDecimal(XElement decimalNode)
        {
            string IntegerPart = "0";
            if (decimalNode.Element("Price") != null)
                decimalNode = decimalNode.Element("Price");

            if (decimalNode.Element("DecimalValue") != null && decimalNode.Element("DecimalValue").Element("Int") != null)
                IntegerPart = decimalNode.Element("DecimalValue").Element("Int").Value.Trim();

            string DecimalPart = "0";
            if (decimalNode.Element("DecimalValue") != null && decimalNode.Element("DecimalValue").Element("Dec") != null)
                DecimalPart = Regex.Replace(decimalNode.Element("DecimalValue").Element("Dec").Value, "\\D", "");

            return MyGetDecimalValue(IntegerPart, DecimalPart);
        }

        private static string MyTranslateCode(string internalCode, VehicleOptionData[] options, decimal? nova)
        {

            try
            {
                // Check if internalCode is empty
                if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(internalCode))
                {
                    // Throw an exception
                    throw new Exception("internalCode");
                }



                List<string> etcodes = EurotaxNumberHelper.GetOptionCodes(internalCode.Trim());



                // Execute the method from helper and return its result
                List<String> optionCodes = new List<String>();
                // Loop through all options
                foreach (var LoopOption in options)
                {

                    if (etcodes.Contains(LoopOption.Code))//use every possible option just once
                    {
                        etcodes.Remove(LoopOption.Code);
                        optionCodes.Add(LoopOption.Code);

                    }


                }

                ObtypMapInfo[] infos = EurotaxNumberHelper.GetEurotaxNumbersFromOka(internalCode.Trim(), optionCodes, nova);
                if (infos == null || infos.Length == 0) return null;
                return infos[0].Eurotaxnummer;

            }
            catch (Exception e)
            {
                // Log the exception
                _Log.Error("Could not translate OKA code to Eurotax number: Code: " + internalCode, e);
                // Throw an exception
                throw new ApplicationException("Could not translate OKA code to Eurotax number: Code: " + internalCode + " Reason: " + e.Message, e);
            }
        }


        public static FzConfiguration GetFzConfiguration(ObtypMapInfo eurotaxInfo, string optionCodes, string oka)
        {
            try
            {
                _Log.Debug("GetFzConfiguration: Options: " + optionCodes + " OKA: " + oka+ "Schwacke: "+eurotaxInfo.Eurotaxnummer);
                FzConfiguration fzConfiguration = new FzConfiguration();
                fzConfiguration.OKA = oka;
                String eurotaxNr = eurotaxInfo.Eurotaxnummer;
                fzConfiguration.GUELTIGVON = eurotaxInfo.GUELTIGVON;
                fzConfiguration.Eurotaxnummer = eurotaxNr;
               
                using (DdOlExtended ctx = new DdOlExtended())
                {
                    var q = from ob in ctx.OBTYP
                            where ob.SCHWACKE == eurotaxNr
                            select ob.BEZEICHNUNG;

                    fzConfiguration.Bezeichnung = q.FirstOrDefault();


                    var ObTyp = (from ob in ctx.OBTYP
                                 where ob.SCHWACKE == eurotaxNr
                                 select ob).FirstOrDefault();



                    //manually configured obtyp
                    if (ObTyp != null && ObTyp.NOEXTID > 0)
                    {



                        fetchConfigFromFZTYP(optionCodes, fzConfiguration, ObTyp,ctx);

                        // Return the result
                        return fzConfiguration;
                    }
                    
                }


                using (DdEurotaxExtended Entities = new DdEurotaxExtended())
                {

                    // Query ETGTYPE
                    var CurrentType = (from Type in Entities.ETGTYPE
                                       where Type.NATCODE == eurotaxNr
                                       select Type).FirstOrDefault();

                    // Check if the type was found
                    if (CurrentType == null)
                    {
                        _Log.Error("The eurotax number " + eurotaxNr + " was not found in ETGTYPE, trying FZTYP");
                        using (DdOlExtended ctx = new DdOlExtended())
                        {


                            var ObTyp = (from ob in ctx.OBTYP
                                         where ob.SCHWACKE == eurotaxNr
                                         select ob).FirstOrDefault();
                            fetchConfigFromFZTYP(optionCodes, fzConfiguration, ObTyp,ctx);
                            return fzConfiguration;
                        }
                        //throw new System.ApplicationException("The eurotax number " + eurotaxNr + " was not found in ETGTYPE");
                    }

                    // Query ETGTYPEAT
                    var CurrentTypeAt = (from TypeAt in Entities.ETGTYPEAT
                                         where TypeAt.NATCODE == eurotaxNr
                                         select TypeAt).FirstOrDefault();

                    // Check if the austrian type was found
                    if (CurrentTypeAt == null)
                    {
                        _Log.Error("Could not retrieve the Austria specific data for " + eurotaxNr + " in ETGTYPEAT");
                        // Throw an exception
                        throw new ApplicationException("Could not retrieve the Austria specific data for " + eurotaxNr + " in ETGTYPEAT");
                    }

                    // Query ETGCONSUMER
                    var CurrentConsumer = (from Consumer in Entities.ETGCONSUMER
                                           where Consumer.NATCODE == eurotaxNr
                                           select Consumer).FirstOrDefault();

                    // Check if consumer data was found
                    if (CurrentConsumer == null)
                    {
                        if (CurrentType.VEHTYPE == 40)
                            _Log.Debug("Consumer Data for " + eurotaxNr + " was not found!");
                        else
                        {
                            _Log.Error("Could not retrieve Consumer Data for " + eurotaxNr + " in ETGCONSUMER");
                            throw new ApplicationException("Could not retrieve Consumer Data for " + eurotaxNr + " in ETGCONSUMER");
                        }
                        // Throw an exception

                    }

                    // Query ETGPRICE
                    var CurrentPrice = (from Price in Entities.ETGPRICE
                                        where Price.NATCODE == eurotaxNr
                                        select Price).FirstOrDefault();

                    // Check if the price was found
                    if (CurrentPrice == null)
                    {
                        // Throw an exception
                        _Log.Error("Price for " + eurotaxNr + " not found in ETGPRICE");
                        throw new ApplicationException("Price for " + eurotaxNr + " not found in ETGPRICE");
                    }

                    // Query ETGTXTTABEL
                    var CurrentFuel = (from Fuel in Entities.ETGTXTTABEL
                                       where Fuel.CODE == CurrentType.TXTFUELTYPECD2
                                       select Fuel).FirstOrDefault();

                    // Check if the fuel was found
                    if (CurrentFuel == null)
                    {
                        _Log.Error("Fuel for " + eurotaxNr + " could not be found in ETGTXTTABEL.");
                        // Throw an exception
                        throw new ApplicationException("Fuel for " + eurotaxNr + " could not be found in ETGTXTTABEL.");
                    }

                    // Query ETGTXTTABEL
                    var CurrentTransmission = (from Transmission in Entities.ETGTXTTABEL
                                               where Transmission.CODE == CurrentType.TXTTRANSTYPECD2
                                               select Transmission).FirstOrDefault();

                    // Check if the transmission data was found
                    if (CurrentTransmission == null)
                    {
                        _Log.Error("Transmission type for " + eurotaxNr + " could not be found in ETGTXTTABEL.");
                        // Throw an exception
                        throw new ApplicationException("Transmission type for " + eurotaxNr + " could not be found in ETGTXTTABEL.");
                    }




                    // Get the technical data
                    fzConfiguration.Ccm = (long)Math.Round(CurrentType.CAPTECH.GetValueOrDefault());
                    fzConfiguration.KW = (long)Math.Round(CurrentType.KW.GetValueOrDefault());
                    fzConfiguration.Nova = (decimal)CurrentTypeAt.NOVA2.GetValueOrDefault();

                    fzConfiguration.Verbrauch = 0;
                    fzConfiguration.CO2 = 0;
                    fzConfiguration.NOx = 0;
                    if (CurrentConsumer != null)
                    {
                        fzConfiguration.Verbrauch = (decimal)CurrentConsumer.CONSTOT.GetValueOrDefault();
                        fzConfiguration.CO2 = (decimal)CurrentConsumer.CO2EMI.GetValueOrDefault();
                        fzConfiguration.NOx = (decimal)CurrentConsumer.NOX.GetValueOrDefault();
                        //Change Nox grom grams to miligram
                        fzConfiguration.NOx = fzConfiguration.NOx * 1000;

                    }



                    decimal TaxRate = LsAddHelper.GetTaxRate(null);

                    // Get Price if not provided

                    fzConfiguration.LPBrutto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice((decimal)CurrentPrice.NP1);
                    fzConfiguration.LPNetto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateNetValue(fzConfiguration.LPBrutto, TaxRate);

                    fzConfiguration.Sas = optionCodes;

                    fzConfiguration.Treibstoff = NoVA.getAntriebsartEurotax(CurrentFuel.CODE);
                    



                    // Return the result
                    return fzConfiguration;
                }




            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverBmwTechnicalDataFailed, exception);


                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverBmwTechnicalDataFailed + " " + Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverBmwTechnicalDataFailed.ToString(), exception);

                // Throw the exception
                throw TopLevelException;
            }
        }

        private static void fetchConfigFromFZTYP(string optionCodes, FzConfiguration fzConfiguration, OBTYP ObTyp, DdOlExtended context)
        {
            if (ObTyp.FZTYP == null)
                context.Entry(ObTyp).Reference(f => f.FZTYP).Load();
            
            // Check if FzTyp exists
            if (ObTyp.FZTYP != null)
            {
                // Get the technical data
                fzConfiguration.Ccm = ObTyp.FZTYP.HUBRAUM.GetValueOrDefault();
                fzConfiguration.KW = ObTyp.FZTYP.LEISTUNG.GetValueOrDefault();
                fzConfiguration.Nova = ObTyp.FZTYP.NOVA.GetValueOrDefault();

                fzConfiguration.Verbrauch = ObTyp.FZTYP.VERBRAUCH.GetValueOrDefault();
                fzConfiguration.CO2 = ObTyp.FZTYP.CO2EMI.GetValueOrDefault();
                fzConfiguration.NOx = ObTyp.FZTYP.NOX.GetValueOrDefault();

                decimal TaxRate = LsAddHelper.GetTaxRate(null);

                // Get Price if not provided

                fzConfiguration.LPNetto = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice((decimal)ObTyp.FZTYP.GRUND);
                fzConfiguration.LPBrutto = Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue(fzConfiguration.LPNetto, TaxRate);

                fzConfiguration.Sas = optionCodes;
 

                long sysprmart = ObTyp.FZTYP.SYSPRMART.GetValueOrDefault();
                if (sysprmart > 0) 
                {
                     
                    
                        NoVA n = new NoVA(context);
                        fzConfiguration.Treibstoff = n.getAntriebsartMART(sysprmart);
                    

                }

                
            }
        }


        #endregion

    }
}