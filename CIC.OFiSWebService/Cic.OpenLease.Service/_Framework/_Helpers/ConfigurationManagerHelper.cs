namespace Cic.OpenLease.Service
{
    using Cic.OpenOne.Common.DTO;
    using Cic.OpenOne.Common.Model.DdEurotax;
    using Cic.OpenOne.Common.Model.DdOl;
    #region Using
    using Cic.OpenOne.Common.Util.Config;
    using CIC.Database.OL.EF6.Model;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    // using Cic.OpenLease.Model.DdCcCm;
    using System.Xml;
    using System.Xml.Linq;

    #endregion

    public static class ConfigurationManagerHelper
    {
        private const string CnstEtgTypeImportTableName = "ETGTYPE";
        #region Methods
       /* public static OfferConfiguration GetCmOfferConfiguration(Guid configurationIdentifier)
        {
            // Check if the XML data is empty
            if (configurationIdentifier == null || configurationIdentifier == Guid.Empty)
            {
                // Throw the exception
                throw new ArgumentException("configuationIdentifier");
            }

            try
            {
                // Create the result
                OfferConfiguration OfferConfiguraton = new OfferConfiguration(OfferTypeConstants.CarConfigurator);

                OfferConfiguraton.OfferId = configurationIdentifier.ToString();

                // Create the vehicle data
                OfferConfiguraton.Vehicle = new VehicleData();

                // Create the entities
                using (Cic.OpenLease.Model.DdCcCm.CcCmExtendedEntities Entities = new Cic.OpenLease.Model.DdCcCm.CcCmExtendedEntities())
                {
                    // Convert guid to string
                    string IdentifierString = configurationIdentifier.ToString();

                    // Get the current configuration
                    var CurrentConfiguration = (from Configuration in Entities.CMCPKGS
                                                where Configuration.CMCIDENTS.IDENTIFIER == IdentifierString
                                                select Configuration).FirstOrDefault();

                    // Check if the current configuration was found
                    if (CurrentConfiguration == null)
                    {
                        // Throw an exception
                        throw new ApplicationException("Could not find identifier \"" + IdentifierString + "\" in the database.");
                    }

                    // Check if node XML is empty
                    if (Cic.OpenOne.Common.Util.StringUtil.IsTrimedNullOrEmpty(CurrentConfiguration.XMLTREENODE))
                    {
                        // Throw an exception
                        throw new ArgumentException("Treenode XML is empty");
                    }

                    // Set the currency
                    OfferConfiguraton.Currency = CurrentConfiguration.SELECTEDCURRENCY;

                    try
                    {
                        // Load the XML
                        XDocument NodeXmlDocument = XDocument.Load(new XmlTextReader(new MemoryStream(Encoding.UTF8.GetBytes(CurrentConfiguration.XMLTREENODE))));

 
                        try
                        {
                            OfferConfiguraton.Vehicle.Code = NodeXmlDocument.Root.Element("TreeNode").Element("Key").Value;
                        }
                        catch
                        {
                            // Throw an exception
                            throw new ApplicationException("The treenode key is invalid.");
                        }

                        MyGetEurotaxData(OfferConfiguraton, CurrentConfiguration.XMLCOMPONENTS);

                        // Get the vehicle name
                        OfferConfiguraton.Vehicle.Name = NodeXmlDocument.Root.Element("TreeNode").Element("DisplayName").Value;

                        // Get the vehicle price
                        OfferConfiguraton.Vehicle.Price = MyGetPrice(NodeXmlDocument.Root.Element("TreeNode").Element("Price").Value);

                    }
                    catch (Exception e)
                    {
                        // Throw an exception
                        throw new ApplicationException("Treenode XML is invalid", e);
                    }

                }
              
                // Return the configuration
                return OfferConfiguraton;
            }
            catch(Exception e)
            {
                // Throw an exception
                throw new ApplicationException("Could not get the configuration from Configuration Manager.", e);
            }
        }
        */

        private static string getVinServiceId()
        {
            EurotaxLoginDataDto accessData = new OpenOne.Common.DAO.Auskunft.AuskunftCfgDao().GetEurotaxAccessData(OpenOne.Common.DAO.Auskunft.AuskunftCfgDao.EUROTAXVINDECODE);
            return accessData.serviceId;
        }
        private static Cic.OpenOne.CarConfigurator.VinSearch.ETGHeaderType getVinHeader()
        {
            Cic.OpenOne.CarConfigurator.VinSearch.ETGHeaderType header = new Cic.OpenOne.CarConfigurator.VinSearch.ETGHeaderType();

            EurotaxLoginDataDto accessData = new OpenOne.Common.DAO.Auskunft.AuskunftCfgDao().GetEurotaxAccessData(OpenOne.Common.DAO.Auskunft.AuskunftCfgDao.EUROTAXVINDECODE);
            Cic.OpenOne.CarConfigurator.VinSearch.LoginDataType LoginData = new Cic.OpenOne.CarConfigurator.VinSearch.LoginDataType();
            LoginData.Name = accessData.name;
            LoginData.Password = accessData.password;
            header.Originator = new Cic.OpenOne.CarConfigurator.VinSearch.OriginatorType();
            header.Originator.LoginData = LoginData;
            header.Originator.Signature = accessData.signature;
            header.VersionRequest = Cic.OpenOne.CarConfigurator.VinSearch.VersionType.Item110;
            return header;
        }

            /// <summary>
        /// Delivers the configuration for a carconfigurator selected Object
        /// Basically, only the Price is used, everything else is fetched later by deliverObjectContext/technicalData with the sysobtyp/schwacke 
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <returns></returns>
        public static OfferConfiguration GetObjectConfigurationSysob(long sysob,long sysperole)
        {

            try
            {
                long sysobtyp = 0;
                //select baujahr,erinklmwst,farbea,schwacke,serie,sysobart,sysobtyp,erstzul,kmstand from ob where sysob=575;
                OfferConfiguration OfferConfiguraton = null;
                using (DdOlExtended Context = new DdOlExtended())
                {

                    OfferConfiguraton = Context.ExecuteStoreQuery<OfferConfiguration>("select baujahr,erinklmwst,farbea,trim(schwacke) schwacke,serie,sysobart,sysobtyp,erstzul erstzulassung,kmstand kilometer from ob where sysob="+sysob, null).FirstOrDefault();
                
                    OfferConfiguraton.Type = OfferTypeConstants.HEK;
                    OfferConfiguraton.IsFromObTyp = false;
                    OfferConfiguraton.UseFzData = false;

                    //Maybe wrong:
                    OfferConfiguraton.OfferId = "ANGEBOT_"+sysob;
                    //Maybe dependent on user in the future?
                    OfferConfiguraton.Currency = "EUR";
                    if(OfferConfiguraton.sysobart==0)
                    {
                        OBART obart = OBARTHelper.SearchName(Context, "Gebrauchtwagen");
                        if (obart != null)
                            OfferConfiguraton.sysobart = obart.SYSOBART;
                    }
                    // Create the vehicle data
                    OfferConfiguraton.Vehicle = new VehicleData();
                    sysobtyp= OfferConfiguraton.sysobtyp;

                    if(OfferConfiguraton.schwacke==null||OfferConfiguraton.schwacke.Length<4)
                    {
                        //vin-search
                        if(OfferConfiguraton.serie!=null&&OfferConfiguraton.serie.Length==17)
                        {
                            Cic.OpenOne.CarConfigurator.VinSearch.vinsearchSoapPortClient vs = new Cic.OpenOne.CarConfigurator.VinSearch.vinsearchSoapPortClient();
                            Cic.OpenOne.CarConfigurator.VinSearch.ETGHeaderType header = getVinHeader();
                            Cic.OpenOne.CarConfigurator.VinSearch.VinDecodeInputType vinInput = new Cic.OpenOne.CarConfigurator.VinSearch.VinDecodeInputType();

                            vinInput.VinCode = OfferConfiguraton.serie;
                            vinInput.ServiceId = getVinServiceId();
                            vinInput.ExtendedOutput = true;
                            vinInput.Settings = new Cic.OpenOne.CarConfigurator.VinSearch.ETGsettingType();
                            vinInput.Settings.ISOcountryCode = Cic.OpenOne.CarConfigurator.VinSearch.ISOcountryType.DE;
                            vinInput.Settings.ISOlanguageCode = Cic.OpenOne.CarConfigurator.VinSearch.ISOlanguageType.DE;
                             Cic.OpenOne.CarConfigurator.VinSearch.VinDecodeOutputType outData = vs.VinDecode(ref header, vinInput);
                             if (outData != null && outData.StatusCode == 0)
                             {
                                 Cic.OpenOne.CarConfigurator.VinSearch.VehicleType v = outData.Vehicle[0];
                                 //assign schwacke
                                 OfferConfiguraton.schwacke=v.TypeETGCode;
                             }
                        }
                        
                        
                       
                    }
                    if (sysobtyp == 0)
                    {
                        List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "schwacke", Value = OfferConfiguraton.schwacke });
                        sysobtyp = Context.ExecuteStoreQuery<long>("select sysobtyp from obtyp where schwacke=:schwacke").FirstOrDefault();
                    }
                }
                
                OfferConfiguraton.Vehicle.Code = OfferConfiguraton.schwacke;
                
                OfferConfiguraton.Vehicle.Options = new VehicleOptionData[0];
                
                // Create a context
               using (DdOlExtended Context = new DdOlExtended())
                {

                    //var tmpObj = Context.ExecuteStoreQuery<
                    // Get ObTyp
                    var CurrentObTyp = (from ObTyp in Context.OBTYP
                                        where ObTyp.SYSOBTYP == sysobtyp
                                        select ObTyp).FirstOrDefault();

                    // Check if anything was found
                    if (CurrentObTyp == null)
                    {
                        // Throw an exception
                        throw new Exception("Could not get the configuration from Configuration Manager for sysobtyp: " + sysobtyp);
                    }

                    if (CurrentObTyp.SCHWACKE != null && CurrentObTyp.SCHWACKE.Length > 0 && CurrentObTyp.IMPORTTABLE == CnstEtgTypeImportTableName)
                    {
                        OfferConfiguraton.IsFromObTyp = false;
                        OfferConfiguraton.UseFzData = true;
                        OfferConfiguraton.Vehicle.Code = CurrentObTyp.SCHWACKE;
                        using (DdEurotaxExtended Entities = new DdEurotaxExtended())
                        {
                            // Query ETGPRICE
                            var CurrentPrice = (from Price in Entities.ETGPRICE
                                                where Price.NATCODE == CurrentObTyp.SCHWACKE
                                                select Price).FirstOrDefault();
                            if(CurrentPrice!=null)
                            {
                                OfferConfiguraton.Vehicle.Price = CurrentPrice.NP2.Value;

                                decimal Ust = LsAddHelper.getGlobalUst(sysperole);

                                 OfferConfiguraton.Vehicle.Price = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue((decimal)OfferConfiguraton.Vehicle.Price, Ust));
           
                            }
                            // Query ETGCONSUMER
                            var CurrentConsumer = (from Consumer in Entities.ETGCONSUMER
                                                   where Consumer.NATCODE == CurrentObTyp.SCHWACKE
                                                   select Consumer).FirstOrDefault();


                            if (CurrentConsumer != null)
                            {


                                OfferConfiguraton.Vehicle.co2overal = (decimal)CurrentConsumer.CO2EMI.GetValueOrDefault();
                                OfferConfiguraton.Vehicle.nox = (decimal)CurrentConsumer.NOX.GetValueOrDefault();
                                OfferConfiguraton.Vehicle.consumptionoveral = (decimal)CurrentConsumer.CONSTOT.GetValueOrDefault();
                                OfferConfiguraton.Vehicle.particlemass = (decimal)CurrentConsumer.PART.GetValueOrDefault();
                            }


                            //Change Nox grom grams to miligrams
                            OfferConfiguraton.Vehicle.nox = (OfferConfiguraton.Vehicle.nox * 1000);

                          

                        }
                    }
                    else
                    {
                        decimal Ust = LsAddHelper.getGlobalUst(sysperole);
                        OfferConfiguraton.Vehicle.Price = MyGetPrice(CurrentObTyp, Ust,Context);

                        OfferConfiguraton.Vehicle.co2overal = CurrentObTyp.FZTYP.CO2EMI.GetValueOrDefault();
                        OfferConfiguraton.Vehicle.particlemass = CurrentObTyp.FZTYP.PARTIKEL.GetValueOrDefault();
                        OfferConfiguraton.Vehicle.nox = CurrentObTyp.FZTYP.NOX.GetValueOrDefault();
                        OfferConfiguraton.Vehicle.consumptionoveral = CurrentObTyp.FZTYP.VERBRAUCH.GetValueOrDefault();


                        OfferConfiguraton.Nova = (decimal)CurrentObTyp.FZTYP.NOVA;

                    }
                }
                

                // Return the configuration
                return OfferConfiguraton;
            }
            catch (Exception e)
            {
                // Throw an exception
                throw new ApplicationException("Could not get the configuration from Configuration Manager.", e);
            }
        }
        /// <summary>
        /// Delivers the configuration for a carconfigurator selected Object
        /// Basically, only the Price is used, everything else is fetched later by deliverObjectContext/technicalData with the sysobtyp/schwacke 
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <returns></returns>
        public static OfferConfiguration GetObjectConfiguration(long sysobtyp,long sysperole)
        {

            try
            {
                // Create the result
                OfferConfiguration OfferConfiguraton = new OfferConfiguration(OfferTypeConstants.CarConfigurator);

                //Maybe wrong:
                OfferConfiguraton.OfferId = "ANGEBOT_"+sysobtyp;
                //Maybe dependent on user in the future?
                OfferConfiguraton.Currency = "EUR";

                // Create the vehicle data
                OfferConfiguraton.Vehicle = new VehicleData();


                OfferConfiguraton.IsFromObTyp=true;
                OfferConfiguraton.UseFzData = false;
                OfferConfiguraton.Erstzulassung = DateTime.Now;
                OfferConfiguraton.Vehicle.Code = sysobtyp.ToString();
                OfferConfiguraton.Vehicle.Options = new VehicleOptionData[0];

                // Create a context
                using (DdOlExtended Context = new DdOlExtended())
                {
                    // Get ObTyp
                    var CurrentObTyp = (from ObTyp in Context.OBTYP
                                        where ObTyp.SYSOBTYP == sysobtyp
                                        select ObTyp).FirstOrDefault();

                    // Check if anything was found
                    if (CurrentObTyp == null)
                    {
                        // Throw an exception
                        throw new Exception("Could not get the configuration from Configuration Manager for sysobtyp: " + sysobtyp);
                    }

                    if (CurrentObTyp.SCHWACKE != null && CurrentObTyp.SCHWACKE.Length > 0 && CurrentObTyp.IMPORTTABLE == CnstEtgTypeImportTableName)
                    {
                        OfferConfiguraton.IsFromObTyp = false;
                        OfferConfiguraton.UseFzData = true;
                        OfferConfiguraton.Vehicle.Code = CurrentObTyp.SCHWACKE;
                        using (DdEurotaxExtended Entities = new DdEurotaxExtended())
                        {
                            // Query ETGPRICE
                            var CurrentPrice = (from Price in Entities.ETGPRICE
                                                where Price.NATCODE == CurrentObTyp.SCHWACKE
                                                select Price).FirstOrDefault();
                            if(CurrentPrice!=null)
                            {
                                OfferConfiguraton.Vehicle.Price = CurrentPrice.NP2.Value;

                                decimal Ust = LsAddHelper.getGlobalUst(sysperole);

                                 OfferConfiguraton.Vehicle.Price = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue((decimal)OfferConfiguraton.Vehicle.Price, Ust));
           
                            }
                            // Query ETGCONSUMER
                            var CurrentConsumer = (from Consumer in Entities.ETGCONSUMER
                                                   where Consumer.NATCODE == CurrentObTyp.SCHWACKE
                                                   select Consumer).FirstOrDefault();


                            if (CurrentConsumer != null)
                            {


                                OfferConfiguraton.Vehicle.co2overal = (decimal)CurrentConsumer.CO2EMI.GetValueOrDefault();
                                OfferConfiguraton.Vehicle.nox = (decimal)CurrentConsumer.NOX.GetValueOrDefault();
                                OfferConfiguraton.Vehicle.consumptionoveral = (decimal)CurrentConsumer.CONSTOT.GetValueOrDefault();
                                OfferConfiguraton.Vehicle.particlemass = (decimal)CurrentConsumer.PART.GetValueOrDefault();
                            }


                            //Change Nox grom grams to miligrams
                            OfferConfiguraton.Vehicle.nox = (OfferConfiguraton.Vehicle.nox * 1000);

                          

                        }
                    }
                    else
                    {
                        decimal Ust = LsAddHelper.getGlobalUst(sysperole);
                        OfferConfiguraton.Vehicle.Price = MyGetPrice(CurrentObTyp, Ust,Context);

                        OfferConfiguraton.Vehicle.co2overal = CurrentObTyp.FZTYP.CO2EMI.GetValueOrDefault();
                        OfferConfiguraton.Vehicle.particlemass = CurrentObTyp.FZTYP.PARTIKEL.GetValueOrDefault();
                        OfferConfiguraton.Vehicle.nox = CurrentObTyp.FZTYP.NOX.GetValueOrDefault();
                        OfferConfiguraton.Vehicle.consumptionoveral = CurrentObTyp.FZTYP.VERBRAUCH.GetValueOrDefault();


                        OfferConfiguraton.Nova = (decimal)CurrentObTyp.FZTYP.NOVA;

                    }
                }
                
               
                // OfferConfiguraton.Vehicle.Name; //to ANGOBFABRIKAT, fetched from deliverObjectContext....
                // OfferConfiguraton.Vehicle.BrandName = BrandObTyp.BEZEICHNUNG; //to ANGOBHERSTELLER, fetched from deliverObjectContext....
                //OfferConfiguraton.Vehicle.ManufacturerName = BrandObTyp.BEZEICHNUNG;//not used at all!
               

                

                // Return the configuration
                return OfferConfiguraton;
            }
            catch (Exception e)
            {
                // Throw an exception
                throw new ApplicationException("Could not get the configuration from Configuration Manager.", e);
            }
        }

        private static decimal MyGetPrice(OBTYP obTyp, decimal Ust, DdOlExtended context)
        {
            // Check if ObTyp is valid
            if (obTyp == null)
            {
                // Throw an exception
                throw new ArgumentNullException("obTyp", "ObTyp is null.");
            }

            
            if (obTyp.FZTYP == null)
                context.Entry(obTyp).Reference(f => f.FZTYP).Load();
            // Check if FZTYP exists
            if (obTyp.FZTYP == null)
            {
                return 0;
            }

            decimal grundnetto = (decimal)obTyp.FZTYP.GRUND.GetValueOrDefault();
            decimal nova = obTyp.FZTYP.NOVA.GetValueOrDefault();
            decimal rval = grundnetto;


            rval *= (1 + Ust / 100);
            rval += rval * nova / 100;
            rval = Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(rval);

            //needs brutto incl. nova!
            return rval;
        }
      
        #endregion

        #region My methods
        private static void MyGetObTypData(OfferConfiguration offerConfiguraton)
        {
            // Parse the key
            ObTypKey Key = new ObTypKey(offerConfiguraton.Vehicle.Code);

            // Get the vehicle code
            offerConfiguraton.Vehicle.Code = Key.TypeId.ToString();

            // Set the ObTyp flag
            offerConfiguraton.IsFromObTyp = true;

            // Create a context
            using (DdOlExtended Context = new DdOlExtended())
            {
                // Get brand
                var BrandObTyp = (from ObTyp in Context.OBTYP
                                  where ObTyp.SYSOBTYP == Key.BrandId
                                  select ObTyp).FirstOrDefault();

                // Check if nothing was found
                if (BrandObTyp == null)
                {
                    // Throw an exception
                    throw new Exception("Brand not found: ObTyp " + Key.BrandId + ".");
                }

                // Assign brand name
                offerConfiguraton.Vehicle.BrandName = BrandObTyp.BEZEICHNUNG;
                offerConfiguraton.Vehicle.ManufacturerName = BrandObTyp.BEZEICHNUNG;

                // Create an empty components array
                offerConfiguraton.Vehicle.Options = new VehicleOptionData[0];
            }
        }

        private static void MyGetEurotaxData(OfferConfiguration offerConfiguraton, string componentsXml)
        {
            // Parse the key
            string[] KeyElements = offerConfiguraton.Vehicle.Code.Split(new string[] { ">" }, StringSplitOptions.None);
            offerConfiguraton.Vehicle.Code = KeyElements[KeyElements.Length - 1];

            // Get the brand code
            int BrandCode = int.Parse(KeyElements[1]);

            // Create the Eurotax entities
            using (DdEurotaxExtended EurotaxEntities = new DdEurotaxExtended())
            {

                // Query ETGMAKE
                var CurrentBrand = (from Brand in EurotaxEntities.ETGMAKE
                                    where Brand.NATCODE == BrandCode
                                    select Brand).FirstOrDefault();

                // Check if the brand was found
                if (CurrentBrand == null)
                    throw new ApplicationException("Cannot find brand code " + BrandCode);

                // Get the brand name
                offerConfiguraton.Vehicle.BrandName = CurrentBrand.NAME;
                offerConfiguraton.Vehicle.ManufacturerName = CurrentBrand.NAME;

                


            }

            // Create an options list
            List<VehicleOptionData> OptionsList = new List<VehicleOptionData>();

            if (!string.IsNullOrEmpty(componentsXml))
            {
                try
                {
                    // Load an options XML document
                    XDocument OptionsXmlDocument = XDocument.Load(new XmlTextReader(new MemoryStream(Encoding.UTF8.GetBytes(componentsXml))));

                    // Loop through all options
                    foreach (var LoopOptionNode in OptionsXmlDocument.Root.Elements("ConfigurationComponent"))
                    {
                        // Get the option
                        VehicleOptionData OptionData = MyGetOption(LoopOptionNode);

                        // Add the option to the list
                        OptionsList.Add(OptionData);
                    }
                }
                catch (Exception e)
                {
                    // Throw an exception
                    throw new ApplicationException("Options XML is invalid", e);
                }
            }

            // Assign the options
            offerConfiguraton.Vehicle.Options = OptionsList.ToArray();
        }

        private static VehicleOptionData MyGetOption(XElement OptionNode)
        {
            try
            {
                // Create the result
                VehicleOptionData OptionData = new VehicleOptionData();

                // Get the key
                OptionData.Code = OptionNode.Element("Component").Element("Key").Value;

                // Get the name
                OptionData.Name = OptionNode.Element("Component").Element("DisplayName").Value;

                // Get the price
                OptionData.Price = MyGetPrice(OptionNode.Element("Component").Element("Price").Value);


                // Get the appropriate option type
                switch (OptionNode.Element("Component").Element("ComponentTypeConstant").Value)
                {
                    // Package
                    case "Package":
                        OptionData.Type = OptionTypeConstants.Package;
                        break;

                    // Accessory
                    case "Accessory":
                        OptionData.Type = OptionTypeConstants.OriginalAccessory;
                        break;

                    // Equipment
                    case "Equipment":
                        OptionData.Type = OptionTypeConstants.Option;
                        break;

                    // Exterior design
                    case "ExteriorDesign":
                        OptionData.Type = OptionTypeConstants.Option;
                        break;

                    // Interior design
                    case "InteriorDesign":
                        OptionData.Type = OptionTypeConstants.Option;
                        break;

                    // All other values
                    default:
                        OptionData.Type = OptionTypeConstants.Miscellaneous;
                        break;
                }

                // Return the result
                return OptionData;
            }
            catch (Exception e)
            {
                // Throw an exception
                throw new ApplicationException("Could not parse the option node.", e);
            }
        }

        private static decimal MyGetPrice(string price)
        {
            // Create the culture info
            CultureInfo CultureInfo = new CultureInfo("en-US");

            try
            {
                
                // Set the point as the decimal separator
                CultureInfo.NumberFormat.NumberDecimalSeparator = ".";

                // Try to get the price
                return decimal.Parse(price, CultureInfo);
            }
            catch
            {
                try
                {
                    // Set the comma as the decimal separator
                    CultureInfo.NumberFormat.NumberDecimalSeparator = ",";

                    // Try to get the price
                    return decimal.Parse(price, CultureInfo);
                }
                catch (Exception e)
                {
                    // Throw an exception
                    throw new ApplicationException("Could not parse the price.", e);
                }
            }
        }
        #endregion
    }
}