using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Model.DdEurotax;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.EntityClient;
using Cic.P000001.Common;
using System.IO;
using System.Reflection;

using Cic.P000001.Common.DataProvider;
using System.Text.RegularExpressions;

using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;

using Cic.OpenOne.CarConfigurator.BO.ExpressionParser;
using LinqKit;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.CarConfigurator.Util.Expressions;
using CIC.Database.OL.EF4.Model;

namespace Cic.OpenOne.CarConfigurator.BO.EurotaxDataProvider
{
    internal static class EurotaxData
    {

        #region Private enumerators
        enum LiteralType { Number, Text, Parenthesis, None };
        #endregion

        #region Private constants
        private const string CnstMotorradText = "MOT";
        private const string CnstWohnmobilText = "WW";
        private const string CnstPKWText = "PW";
        private const string CnstNutzfahrzeugText = "LN";

        private const string CnstCarsText = "Cars";
        private const string CnstCommercialVehiclesText = "Commercial vehicles";
        private const string CnstMotorcyclesText = "Motorcycles";
        private const string CnstSelectVehicleTypeText = "Select vehicle type";
        private const string CnstSelectBrandText = "Select brand";
        private const string CnstSelectModelGroupText = "Select model group";
        private const string CnstSelectModelText = "Select model";
        private const string CnstSelectTypeText = "Select type";
        private static Dictionary<string, Dictionary<string, string>> translation = new Dictionary<string, Dictionary<string, string>>();
        #endregion

        private const string General = "General";
        private const string Segmentation = "Segmentation";
        private const string Vehiclecategory = "Vehiclecategory";
        private const string Totalweight = "Total weight";
        private const string Length = "Length";
        private const string Width = "Width";
        private const string Height = "Height";
        private const string Wheelbase = "Wheelbase";
        private const string Fueltype = "Fuel type";
        private const string Cylinders = "Cylinders";
        private const string Cylinderarrangement = "Cylinder arrangement";
        private const string Engine = "Engine";
        private const string Capacity = "Capacity";
        private const string Exhausttreatment = "Exhaust treatment";
        private const string Valvespercylinder = "Valves per cylinder";
        private const string Chargertype = "Charger type";
        private const string Horsepower = "Horse power";
        private const string Torque = "Torque";
        private const string Consumptionindex = "Consumption index";
        private const string PollutionNorm = "PollutionNorm";
        private const string Body = "Body";
        private const string Bodytype = "Body type";
        private const string Doors = "Doors";
        private const string Roofloading = "Roof loading";
        private const string Transmission = "Transmission";
        private const string Transmissiontype = "Transmission type";
        private const string Drivetype = "Drive type";
        private const string Numberofgears = "Number of gears";
        private const string Interior = "Interior";
        private const string Seats = "Seats";
        private const string Bootcapacity = "Boot capacity";


        static EurotaxData()
        {
            Dictionary<string, string> t = new Dictionary<string, string>();
            t[Vehiclecategory] = "Fahrzeugkategorie";
            t[General] = "Allgemein";
            t[Segmentation] = "Aufteilung";
            t[General] = "Fahrzeugkategorie";
            t[Totalweight] = "Gesamtgewicht";
            t[Length] = "Länge";
            t[Width] = "Breite";
            t[Height] = "Höhe";
            t[Wheelbase] = "Achsabstand";
            t[Fueltype] = "Kraftstoffart";
            t[Cylinders] = "Zylinder";
            t[Cylinderarrangement] = "Bauart";
            t[Engine] = "Motor";
            t[Capacity] = "Hubraum";
            t[Exhausttreatment] = "Abgasbehandlung";
            t[Valvespercylinder] = "Ventile pro Zylinder";
            t[Chargertype] = "Ladertyp";
            t[Horsepower] = "Leistung";
            t[Torque] = "Drehmoment";
            t[Consumptionindex] = "Verbrauchsindex";
            t[PollutionNorm] = "Abgasnorm";
            t[Body] = "Karosserie";
            t[Bodytype] = "Karosserietyp";
            t[Doors] = "Türen";
            t[Roofloading] = "Dachbeladung";
            t[Transmission] = "Getriebe";
            t[Transmissiontype] = "Getriebetyp";
            t[Drivetype] = "Antriebstyp";
            t[Numberofgears] = "Anzahl Gänge";
            t[Interior] = "Innenausstattung";
            t[Seats] = "Sitzplätze";
            t[Bootcapacity] = "Kofferraumvolumen";

            t[CnstCarsText] = "PKW";
            t[CnstCommercialVehiclesText] = "Leicht-LKW / Transporter";
            t[CnstMotorcyclesText] = "Motorräder";
            t[CnstSelectVehicleTypeText] = "Fahrzeugart wählen";
            t[CnstSelectBrandText] = "Marke wählen";
            t[CnstSelectModelGroupText] = "Modellgruppe wählen";
            t[CnstSelectModelText] = "Modell wählen";
            t[CnstSelectTypeText] = "Typ wählen";

            t[CnstMotorradText] = "Motorräder";
            t[CnstWohnmobilText] = "Wohnmobile";
            t[CnstPKWText] = "PKW";
            t[CnstNutzfahrzeugText] = "LKW";
           

            translation["de-at"] = t;
            translation["de-ch"] = t;
            translation["de"] = t;
            translation["eude"] = t;
            translation["atde"] = t;
            translation["dede"] = t;
            translation["dech"] = t;
            translation["chde"] = t;

            t = new Dictionary<string, string>();
            t[Vehiclecategory] = Vehiclecategory;
            t[General] = General;
            t[Segmentation] = Segmentation;
            t[General] = General;
            t[Totalweight] = Totalweight;
            t[Length] = Length;
            t[Width] = Width;
            t[Height] = Height;
            t[Wheelbase] = Wheelbase;
            t[Fueltype] = Fueltype;
            t[Cylinders] = Cylinders;
            t[Cylinderarrangement] = Cylinderarrangement;
            t[Engine] = Engine;
            t[Capacity] = Capacity;
            t[Exhausttreatment] = Exhausttreatment;
            t[Valvespercylinder] = Valvespercylinder;
            t[Chargertype] = Chargertype;
            t[Horsepower] = Horsepower;
            t[Torque] = Torque;
            t[Consumptionindex] = Consumptionindex;
            t[PollutionNorm] = PollutionNorm;
            t[Body] = Body;
            t[Bodytype] = Bodytype;
            t[Doors] = Doors;
            t[Roofloading] = Roofloading;
            t[Transmission] = Transmission;
            t[Transmissiontype] = Transmissiontype;
            t[Drivetype] = Drivetype;
            t[Numberofgears] = Numberofgears;
            t[Interior] = Interior;
            t[Seats] = Seats;
            t[Bootcapacity] = Bootcapacity;

            t[CnstCarsText] = CnstCarsText;
            t[CnstCommercialVehiclesText] = CnstCommercialVehiclesText;
            t[CnstMotorcyclesText] = CnstMotorcyclesText;
            t[CnstSelectVehicleTypeText] = CnstSelectVehicleTypeText;
            t[CnstSelectBrandText] = CnstSelectBrandText;
            t[CnstSelectModelGroupText] = CnstSelectModelGroupText;
            t[CnstSelectModelText] = CnstSelectModelText;
            t[CnstSelectTypeText] = CnstSelectTypeText;


            translation["en"] = t;
        }

        public static string getTranslation(string language, string key)
        {
            Dictionary<string, string> t;
            String lng = language.ToLower();
            if (!translation.ContainsKey(lng))
                t = translation["en"];
            t = translation[lng];
            if (!t.ContainsKey(key))
                return "Unmapped Id " + key;
            return t[key];

        }

        public static string getTranslation(Setting setting, string key)
        {
            return getTranslation(setting.SelectedLanguage, key);

        }
        #region Methods

        /// <summary>
        /// Returns the filterparams of the given ones suitable for the level of the given node
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="treeNode"></param>
        /// <returns></returns>
        public static FilterParam[] GetFilterParamsForNode(FilterParam[] filters, TreeNode treeNode)
        {
            // Create the result list
            List<FilterParam> ResultFilters = new List<FilterParam>();

            // Add the appropriate filters
            if (filters != null)
                foreach (FilterParam LoopFilter in filters)
                    if (LoopFilter.FilterAtLevel.Number == treeNode.Level.Number)
                        ResultFilters.Add(LoopFilter);

            // Return the filters
            return ResultFilters.ToArray();
        }

        public static SearchParam[] GetSearchParamsForNode(SearchParam[] searches, TreeNode treeNode)
        {
            // Create the result list
            List<SearchParam> ResultSearches = new List<SearchParam>();

            // Add the appropriate searches
            if (searches != null)
                foreach (SearchParam LoopSearch in searches)
                    if (LoopSearch.SearchAtLevel.Number == treeNode.Level.Number)
                        ResultSearches.Add(LoopSearch);

            // Return the searches
            return ResultSearches.ToArray();
        }

        public static int GetModelGroupCode(DdEurotaxExtended context, Setting setting, TreeNode treeNode)
        {
            EurotaxKey Key = new EurotaxKey(setting, treeNode);

            var CurrentGroup = (from Group in context.ETGMODLEVONE
                                join Model in context.ETGMODEL
                                on Group.NAME equals Model.NAMEGRP1
                                where Model.NATCODE == Key.ModelCode
                                select Group).FirstOrDefault();

            if (CurrentGroup == null)
                return 0;

            return CurrentGroup.CODE;
        }

        public static bool CheckNode(Setting setting, TreeNode treeNode, FilterParam[] filterParams, SearchParam[] searchParams)
        {
            // Assume the node passed filtering
            bool FilteringPassed = true;

            // Get the filters apropriate for the node
            FilterParam[] NodeFilters = GetFilterParamsForNode(filterParams, treeNode);

            // When there are some filters to apply
            if (NodeFilters.Length > 0)
            {
                // Assume the node didn't passed
                FilteringPassed = false;

                // Iterate through the filter params, only one filter must be passed
                foreach (FilterParam LoopParam in NodeFilters)
                {
                    // Check if the node matches the filter
                    if (Regex.IsMatch(treeNode.DisplayName+treeNode.FILTERDATA, LoopParam.Filter, RegexOptions.IgnoreCase | RegexOptions.Compiled))
                    {
                        // Yes, the filtering is passed
                        FilteringPassed = true;
                        break;
                    }
                }
            }

            // Assume the node passed the searching
            bool SearchingPassed = true;

            // Get the search params appropriate for the node
            SearchParam[] NodeSearches = GetSearchParamsForNode(searchParams, treeNode);

            // Loop through the search params, all Search params must be matched!
            foreach (SearchParam LoopSearch in NodeSearches)
            {
                // Check if the node matches the search
                if (!Regex.IsMatch(treeNode.DisplayName+treeNode.FILTERDATA, LoopSearch.Pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled))
                {
                    // The node didn't pass this search, so didn't pass whole searching
                    SearchingPassed = false;
                    break;
                }
            }

            // If all tests are passed
            return FilteringPassed && SearchingPassed;
        }

        public static TreeNode[] SimpleSearch(DdEurotaxExtended context, Setting setting, FilterParam[] filter, SimpleSearchParam simpleSearchParam, TreeNode treeNode)
        {
            // Check if search pattern is not valid
            if (string.IsNullOrEmpty(simpleSearchParam.SearchPattern))
            {
                // Throw an exception
                throw new ApplicationException("Search pattern is not specified.");
            }

            switch (simpleSearchParam.SearchBy)
            {
                case Cic.P000001.Common.SearchBy.Level3Name:
                    return MySimpleSearchModel(simpleSearchParam.SearchPattern, setting);

                case Cic.P000001.Common.SearchBy.Level2Name:
                    return MySimpleSearchModelGroup(simpleSearchParam.SearchPattern, setting);

                case Cic.P000001.Common.SearchBy.Level1Name:
                    return MySimpleSearchBrand(simpleSearchParam.SearchPattern, setting);

                case Cic.P000001.Common.SearchBy.Level0Name:
                    return MySimpleSearchVehicleTypes(simpleSearchParam.SearchPattern, setting);
            }

            // Find the highest level
            int HighestLevel = 0;

            if (filter != null)
            {
                foreach (FilterParam LoopFilterParam in filter)
                {
                    // Check if filter pattern is not valid
                    if (string.IsNullOrEmpty(LoopFilterParam.Filter))
                    {
                        // Throw an exception 
                        throw new ApplicationException("Filter pattern is not specified.");
                    }

                    // Check if level is not valid
                    if (LoopFilterParam.FilterAtLevel == null)
                    {
                        // Throw an exception
                        throw new ApplicationException("Filter level is not specified");
                    }

                    if (LoopFilterParam.FilterAtLevel.Number > HighestLevel)
                    {
                        HighestLevel = LoopFilterParam.FilterAtLevel.Number;
                    }
                }
            }

            // Create a result list
            List<TreeNode> ResultNodes = new List<TreeNode>();

            TreeNode[] Nodes = null;

            switch (simpleSearchParam.SearchBy)
            {
                // When we need to search by the Eurotax number
                case SearchBy.Id:
                    Nodes = SimpleSearchTypes(context, setting, simpleSearchParam.SearchPattern);
                    break;
            }

            if (setting.hasFilter())
            {
                Nodes = filterNodes(setting, Nodes);
            }

            TreeNode[] FilteredNodes = GetFilteredNodes(context, setting, null, filter, null, HighestLevel);

            foreach (var LoopFilteredNode in FilteredNodes)
            {
                foreach (var LoopNode in Nodes)
                {
                    if (LoopNode.Key.StartsWith(LoopFilteredNode.Key))
                    {
                        ResultNodes.Add(LoopNode);
                    }
                }
            }



            // Return the result
            return ResultNodes.ToArray();
        }

        /// <summary>
        /// Used for searching and filtering
        /// </summary>
        /// <param name="context"></param>
        /// <param name="setting"></param>
        /// <param name="treeNode"></param>
        /// <param name="filterParams"></param>
        /// <param name="searchParams"></param>
        /// <param name="returnLevel"></param>
        /// <returns></returns>
        public static TreeNode[] GetFilteredNodes(DdEurotaxExtended context, Setting setting, TreeNode treeNode, FilterParam[] filterParams, SearchParam[] searchParams, int returnLevel)
        {
            // Create a result list
            List<TreeNode> ResultNodes = new List<TreeNode>();

            // Get the nodes
            TreeNode[] Nodes = GetNextLevel(context, setting, treeNode, filterParams, searchParams);

            // Loop trough all nodes
            foreach (TreeNode LoopNode in Nodes)
            {

                // If all tests are passed - apply filter and search param
                if (CheckNode(setting, LoopNode, filterParams, searchParams))
                {

                    // Check if the node is at the needed level
                    if (LoopNode.Level.Number == returnLevel)
                    {
                        // Add the node to the result list
                        ResultNodes.Add(LoopNode);
                    }
                    // If the level is less than needed
                    else if (LoopNode.Level.Number < returnLevel)
                    {
                        // Call GetFilteredNodes for the child node
                        ResultNodes.AddRange(GetFilteredNodes(context, setting, LoopNode, filterParams, searchParams, returnLevel));
                    }
                }

            }



            // Return the result
            return ResultNodes.ToArray();
        }

        public static TreeNode GetPreviousLevel(EurotaxEntities context, Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode)
        {
            // Create the result node
            TreeNode ResultNode = null;

            // Parse the key
            EurotaxKey Key = new EurotaxKey(setting, treeNode);
            string NewKey;
            string NewParentKey;

            // Check the treenode's level
            switch (treeNode.Level.Number)
            {
                // Level 4: get model node
                case 4:
                    // Query ETGMODEL
                    var CurrentModel = (from Model in context.ETGMODEL
                                        where Model.NATCODE == Key.ModelCode
                                        select Model).FirstOrDefault();

                    // Construct the key strings
                    NewKey = Key.VehicleType + ">" + Key.BrandCode + ">" + Key.GroupCode + ">" + Key.ModelCode;
                    NewParentKey = Key.VehicleType + ">" + Key.BrandCode + ">" + Key.GroupCode;

                    // Create the node
                    ResultNode = new TreeNode(NewKey, NewParentKey, new Level(3, getTranslation(setting, CnstSelectModelText)), CurrentModel.NAME, null, false, false, false, 0, 0);
                    break;

                // Level 3: get model group node
                case 3:
                    // Query ETGMODLEVONE
                    var CurrentModelGroup = (from Group in context.ETGMODLEVONE
                                             where Group.CODE == Key.GroupCode
                                             select Group).FirstOrDefault();

                    // Construct the key strings
                    NewKey = Key.VehicleType + ">" + Key.BrandCode + ">" + Key.GroupCode;
                    NewParentKey = Key.VehicleType + ">" + Key.BrandCode;

                    // Create the tree node
                    ResultNode = new TreeNode(NewKey, NewParentKey, new Level(2, getTranslation(setting, CnstSelectModelGroupText)), CurrentModelGroup.NAME, null, false, false, false, 0, 0);
                    break;

                // Level 2: get brand node
                case 2:
                    // Query ETGMAKE
                    var CurrentBrand = (from Brand in context.ETGMAKE
                                        where Brand.NATCODE == Key.BrandCode
                                        select Brand).FirstOrDefault();

                    // Construct the key strings
                    NewKey = Key.VehicleType + ">" + Key.BrandCode;
                    NewParentKey = Key.VehicleType.ToString();

                    // Create the node
                    ResultNode = new TreeNode(NewKey, NewParentKey, new Level(1, getTranslation(setting, CnstSelectBrandText)), CurrentBrand.NAME, null, false, false, false, 0, 0);
                    break;

                // Level 1: get vehicle type node
                case 1:
                    //Set display name to empty
                    string DisplayName = string.Empty;

                    // Get the appropriate display name
                    switch (Key.VehicleType)
                    {
                        case 10: DisplayName = getTranslation(setting, CnstCarsText); break;
                        case 20: DisplayName = getTranslation(setting, CnstCommercialVehiclesText); break;
                        case 40: DisplayName = getTranslation(setting, CnstMotorcyclesText); break;
                    }

                    // Construct the node's key string
                    NewKey = Key.VehicleType.ToString();

                    // Create the node
                    ResultNode = new TreeNode(NewKey, null, new Level(0, getTranslation(setting, CnstSelectVehicleTypeText)), DisplayName, null, false, false, false, 0, 0);
                    break;
            }

            // Return the node
            return ResultNode;
        }

        public static TreeNode[] GetPreviousLevels(EurotaxEntities context, Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode)
        {
            // Check if the node is valid
            if (treeNode == null)
                throw new ArgumentNullException("treeNode is null");

            // Create a nodes list
            List<TreeNode> ResultNodes = new List<TreeNode>();

            // If this is not the first level
            while (treeNode.Level.Number > 0)
            {
                // Get the previous level
                treeNode = GetPreviousLevel(context, setting, treeNode);

                // Add the node to the list if it's not null
                if (treeNode != null)
                    ResultNodes.Add(treeNode);
            }

            // Return the list
            return ResultNodes.ToArray();
        }

        // Gets supported languages (in .NET format) from config table
        public static String[] GetSupportedLanguages(Cic.OpenOne.Common.Model.DdEurotax.DdEurotaxExtended context)
        {
            var ConfigEntries = from ConfigEntry in context.ETGCONFIG
                                where ConfigEntry.NAME == "SupportedLanguage"
                                select ConfigEntry;

            String[] Languages = new String[ConfigEntries.Count()];
            int I = 0;
            foreach (ETGCONFIG LoopConfigEntry in ConfigEntries)
                Languages[I] = EurotaxGlobalization.GetNativeLanguageCode(LoopConfigEntry.VALUE);
            return Languages;
        }

        // Get supported currencies (in .NET format) from config table
        public static String[] GetSupportedCurrencies(Cic.OpenOne.Common.Model.DdEurotax.DdEurotaxExtended context)
        {
            var ConfigEntries = from ConfigEntry in context.ETGCONFIG
                                where ConfigEntry.NAME == "SupportedCurrency"
                                select ConfigEntry;

            String[] Languages = new String[ConfigEntries.Count()];
            int I = 0;
            foreach (ETGCONFIG LoopConfigEntry in ConfigEntries)
                Languages[I] = EurotaxGlobalization.GetNativeCurrencyCode(LoopConfigEntry.VALUE);
            return Languages;
        }

        public static Cic.P000001.Common.TreeNode[] GetNextLevel(Cic.OpenOne.Common.Model.DdEurotax.DdEurotaxExtended context, Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode)
        {
            return GetNextLevel(context, setting, treeNode, null, null);
        }

        // Get next tree level
        public static Cic.P000001.Common.TreeNode[] GetNextLevel(Cic.OpenOne.Common.Model.DdEurotax.DdEurotaxExtended context, Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, FilterParam[] filterParams, SearchParam[] searchParams)
        {


            /*
select etgmake.natcode,etgmodlevone.code,etgmodel.natcode,etgtype.natcode, etgtype.name,etgtype.kw,etgtype.impbegin,etgtype.impend, etgtype.natcode,etgtype.txtfueltypecd2,etgtype.secfueltypcd2,etgtype.txttranstypecd2,etgtype.txtbodyco1cd2, etgprice.np1 from etgtype,etgmodel,etgmake,etgmodlevone,etgprice  where  etgprice.natcode = etgtype.natcode and etgmodlevone.makcd=etgmake.natcode and etgmodlevone.code=etgtype.mlocd and etgmake.vehtype=10 and etgmake.natcode=etgmodel.makcd and etgtype.modcd=etgmodel.natcode and etgmake.natcode=38 ;

--level 1:
and etgmake.natcode=38
--level 2:
and etgmodlevone.code=46 
--level 3:
and etgmodel.natcode=22
--level 4:
and  etgtype.natcode='102171693';
             */

            // Declare a node list
            TreeNode[] Nodes = null;

            // Create a key
            EurotaxKey Key = new EurotaxKey(setting, treeNode);

            // If tree node is null, start from the root - get vehicle types available
            if (treeNode == null)
                Nodes = EurotaxData.GetVehicleTypes(context, Key);
            else
            {

                switch (treeNode.Level.Number)
                {
                    // Get car manufacturers names
                    case 0:
                        Nodes = GetBrands(context, Key, getFilter(treeNode.Level.Number, filterParams), getSearchParam(treeNode.Level.Number, searchParams));
                        if (setting.hasFilter())
                        {
                            Nodes = filterNodes(setting, Nodes);
                        }
                        break;
                    // Get model group names
                    case 1:
                        Nodes = GetModelGroups(context, Key, getFilter(treeNode.Level.Number, filterParams), getSearchParam(treeNode.Level.Number, searchParams));
                        break;
                    // Get model names
                    case 2:
                        Nodes = GetModels(context, Key, getFilter(treeNode.Level.Number, filterParams), getSearchParam(treeNode.Level.Number, searchParams));
                        break;
                    case 3:
                    // Get type names, level 3
                    default:
                        Nodes = GetTypes(context, Key, getFilter(treeNode.Level.Number, filterParams), getSearchParam(treeNode.Level.Number, searchParams));
                        break;
                }
            }


            // Return the nodes
            return Nodes;
        }

        /// <summary>
        /// returns the filters of the given filters where the level equals the given level
        /// </summary>
        /// <param name="level"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        private static FilterParam[] getFilter(int level, FilterParam[] filters)
        {
            if (filters == null) return null;
            List<FilterParam> rval = new List<FilterParam>();
            foreach (FilterParam f in filters)
            {
                if (f.FilterAtLevel.Number == (level + 1))
                    rval.Add(f);
            }
            return rval.ToArray();
        }

        private static SearchParam[] getSearchParam(int level, SearchParam[] filters)
        {
            if (filters == null) return null;
            List<SearchParam> rval = new List<SearchParam>();
            foreach (SearchParam f in filters)
            {
                if (f.SearchAtLevel.Number == (level + 1))
                    rval.Add(f);
            }
            return rval.ToArray();
        }

        private static List<TreeNode> filterNodes(Setting setting, List<TreeNode> Nodes)
        {
            if (!setting.hasFilter()) return Nodes;
            List<TreeNode> rval = new List<TreeNode>();
            foreach (TreeNode t in Nodes)
            {
                if (!setting.filter(t.DisplayName))
                    rval.Add(t);
            }
            return rval;
        }

        private static TreeNode[] filterNodes(Setting setting, TreeNode[] Nodes)
        {
            if (!setting.hasFilter()) return Nodes;
            List<TreeNode> rval = new List<TreeNode>();
            foreach (TreeNode t in Nodes)
            {
                if (!setting.filter(t.DisplayName))
                    rval.Add(t);
            }
            return rval.ToArray();
        }


        public static Cic.P000001.Common.TreeNode[] SimpleSearchTypes(Cic.OpenOne.Common.Model.DdEurotax.DdEurotaxExtended context, Setting setting, String pattern)
        {
            string Language = EurotaxGlobalization.GetEurotaxLanguageCode(setting.SelectedLanguage);
            string Currency = EurotaxGlobalization.GetEurotaxCurrencyCode(setting.SelectedCurrency);
            string Market = EurotaxGlobalization.GetEurotaxMarketCode(setting.SelectedLanguage);

            // Query ETGTYPE
            var Types = from Type in context.ETGTYPE
                        join Price in context.ETGPRICE
                        on Type.NATCODE equals Price.NATCODE
                        join Make in context.ETGMAKE
                        on Type.MAKCD equals Make.NATCODE
                        where Type.MARKET == Market
                        && Type.NATCODE.StartsWith(pattern)
                         && Type.RECSTATUS != 9
                         && Price.CURRENCY == Currency && Make.VEHTYPE == Type.VEHTYPE
                        orderby Type.NAME, Type.IMPBEGIN descending
                        select new { Type, Price, Make };

            // Create a node list
            List<Cic.P000001.Common.TreeNode> Nodes = new List<TreeNode>();
            int mcount = 0;
            // Loop through all rows
            foreach (var LoopType in Types)
            {
                string Years = LoopType.Type.IMPBEGIN.Substring(0, 4);
                if (LoopType.Type.IMPEND != null && LoopType.Type.IMPEND.Length >= 4)
                    Years += " - " + LoopType.Type.IMPEND.Substring(0, 4);
                else
                    Years += "+";

                // Create a new node
                Cic.P000001.Common.TreeNode Node = new TreeNode();
                Node.DisplayName = LoopType.Make.NAME + " " + LoopType.Type.NAME + ", " + LoopType.Type.KW + " kW (" + Years + ")";
                Node.HasDetails = true;
                Node.HasPictures = false;
                Node.Level = new Level(4, CnstSelectTypeText);
                Node.Price = (double)LoopType.Price.NP1;
                Node.NewPrice = (double)LoopType.Price.NP1;
                Node.Parameters = null;
                Node.IsType = true;

                Node.Key = LoopType.Type.VEHTYPE + ">" + LoopType.Type.MAKCD + ">0>" + LoopType.Type.MODCD + ">" + LoopType.Type.NATCODE;
                Node.ParentKey = LoopType.Type.VEHTYPE + ">" + LoopType.Type.MAKCD + ">0>" + LoopType.Type.MODCD;


                EurotaxKey Key = new EurotaxKey(setting, Node);
                Key.GroupCode = GetModelGroupCode(context, setting, Node);

                Key.ParentKey = Key.ParentKeyText;
                Node.Key = Key.KeyText;
                Node.ParentKey = Key.ParentKey;




                // Add node to the list
                Nodes.Add(Node);
                mcount++;
                if (mcount > 40) break;
            }


            /*

            // Query TYPE
            var Types = from Type in context.ETGTYPE
                        where Type.NATCODE.StartsWith(pattern)
                        select Type;

            // Create a node list
            List<Cic.P000001.Common.TreeNode> Nodes = new List<TreeNode>();
            List<EurotaxKey> Keys = new List<EurotaxKey>();
            int mcount = 0;
            foreach (ETGTYPE LoopType in Types)
            {
                Cic.P000001.Common.TreeNode Node = new Cic.P000001.Common.TreeNode();
                Node.DisplayName = LoopType.NAME;
                Node.HasDetails = false;
                Node.HasPictures = false;
                Node.Level = new Level(4, "Select type");
                Node.NewPrice = 0;
                Node.Parameters = null;
                Node.Key = LoopType.VEHTYPE + ">" + LoopType.MAKCD + ">0>" + LoopType.MODCD + ">" + LoopType.NATCODE;
                Node.ParentKey = LoopType.VEHTYPE + ">" + LoopType.MAKCD + ">0>" + LoopType.MODCD;
                Node.IsType = true;

                EurotaxKey Key = new EurotaxKey(setting, Node);
                //Key.GroupCode = GetModelGroupCode(context, setting, Node);
                Key.ParentKey = Key.ParentKeyText; 
                Keys.Add(Key);
                mcount++;
                if (mcount > 100) break;
            }
            
            foreach (EurotaxKey Key in Keys)
            {
                TreeNode[] nodes = GetTypesByNatCode(context, Key);
                Nodes.AddRange(nodes.ToArray());
            }*/

            return Nodes.ToArray();

        }

        // Gets all available vehicle types
        public static Cic.P000001.Common.TreeNode[] GetVehicleTypes(Cic.OpenOne.Common.Model.DdEurotax.DdEurotaxExtended context, EurotaxKey key)
        {

            // Define all vehicle types
            Dictionary<int, String> Types = MyGetVehicleTypes(key.Language);

            // Create a node list
            List<Cic.P000001.Common.TreeNode> Nodes = new List<TreeNode>();

            // Iterate through all vehicle types
            foreach (KeyValuePair<int, String> Type in Types)
            {
                // Create a node and add it to the list
                Cic.P000001.Common.TreeNode Node = new Cic.P000001.Common.TreeNode();
                Node.DisplayName = Type.Value;
                Node.HasDetails = false;
                Node.HasPictures = false;
                Node.Level = new Level(0, CnstSelectVehicleTypeText);
                Node.NewPrice = 0;
                Node.Parameters = null;
                Node.Key = Type.Key.ToString();
                Node.ParentKey = key.ParentKey;
                Node.IsType = false;
                Nodes.Add(Node);
            }

            // Return the list as an array
            return Nodes.ToArray();
        }

        // Gets the brand names
        public static Cic.P000001.Common.TreeNode[] GetBrands(Cic.OpenOne.Common.Model.DdEurotax.DdEurotaxExtended context, EurotaxKey key)
        {
            return GetBrands(context, key, null, null);
        }
        public static Cic.P000001.Common.TreeNode[] GetBrands(Cic.OpenOne.Common.Model.DdEurotax.DdEurotaxExtended context, EurotaxKey key, FilterParam[] filter, SearchParam[] search)
        {
           /* System.Text.StringBuilder queryBuilder = new System.Text.StringBuilder();
            String query = "select distinct etgmake.NATCODE, etgmake.NAME from etgmake,etgmodlevone,etgmodel,etgtype  where etgmake.natcode=etgmodlevone.makcd and etgmodlevone.code=etgmodel.makcd and etgmodel.natcode=etgtype.modcd ";//,,etgmake,,etgprice,etgconsumer  where  etgconsumer.natcode=etgtype.natcode and etgprice.natcode = etgtype.natcode  and etgmodlevone.code=etgtype.mlocd and etgmake.natcode=etgmodel.makcd and etgtype.modcd=etgmodel.natcode ";
            queryBuilder.Append(query);
            queryBuilder.Append(" and etgmake.market='");
            queryBuilder.Append(key.Market);
            queryBuilder.Append("' ");
            queryBuilder.Append(" and etgmake.langcode='");
            queryBuilder.Append(key.Language);
            queryBuilder.Append("' ");
            queryBuilder.Append(" and etgmake.VEHTYPE=");
            queryBuilder.Append(key.VehicleType);
            queryBuilder.Append(" and etgmake.RECSTATUS!=9 ");
            
            // Create a node list
            List<Cic.P000001.Common.TreeNode> Nodes = new List<TreeNode>();
         
                List<Cic.OpenOne.CarConfigurator.BO.DataProviderService.QueryData> brands = context.ExecuteStoreQuery<Cic.OpenOne.CarConfigurator.BO.DataProviderService.QueryData>(queryBuilder.ToString(), null).ToList();
                // Iterate through all rows
                foreach (Cic.OpenOne.CarConfigurator.BO.DataProviderService.QueryData element in brands)
                {
                    // Create a new node and add it to the list
                    Cic.P000001.Common.TreeNode Node = new Cic.P000001.Common.TreeNode();
                    Node.DisplayName = element.name;
                    Node.HasDetails = false;
                    Node.HasPictures = false;
                    Node.Level = new Level(1, CnstSelectBrandText);
                    Node.NewPrice = 0;
                    Node.Parameters = null;
                    Node.Key = key + ">" + element.natcode;
                    Node.ParentKey = key.ParentKey;
                    Node.IsType = false;
                    Nodes.Add(Node);
                }
            
            // Return the list as an array
            return Nodes.ToArray();
            */
            
            var predicateMain = PredicateBuilder.True<ETGMAKE>();

            predicateMain = predicateMain.And(e => e.MARKET == key.Market);
            predicateMain = predicateMain.And(e => e.LANGCODE == key.Language);
            predicateMain = predicateMain.And(e => e.VEHTYPE == key.VehicleType);
            predicateMain = predicateMain.And(e => e.RECSTATUS != 9);

            var predicate1 = PredicateBuilder.True<ETGMAKE>();
            if (filter != null)
            {
                foreach (FilterParam fp in filter)
                {
                    String filterPattern = fp.Filter.ToLower();
                    predicate1 = predicate1.And(e => e.NAME.ToLower().Contains(filterPattern));
                }
            }
            /* if (filter != null && search != null && filter.Filter.Equals(search.Pattern));
             else */
            if (search != null)
            {
                foreach (SearchParam spar in search)
                {
                    String sp = spar.Pattern.ToLower();
                    predicate1 = predicate1.And(e => e.NAME.ToLower().Contains(sp));
                }
            }

            if (search != null || filter != null)
                predicateMain = predicateMain.And(predicate1.Expand());

            var Brands = context.ETGMAKE.AsExpandable().Where(predicateMain.Expand()).OrderBy(e => e.NAME);
            
            // Query ETGMAKE
            /* var Brands = from Brand in context.ETGMAKE
                          where Brand.MARKET == key.Market
                          && Brand.LANGCODE == key.Language
                          && Brand.VEHTYPE == key.VehicleType
                          && Brand.RECSTATUS != 9
                          orderby Brand.NAME
                          select Brand;*/

            // Create a node list
            List<Cic.P000001.Common.TreeNode> Nodes = new List<TreeNode>();

            // Iterate through all rows
            foreach (ETGMAKE LoopBrand in Brands)
            {
                // Create a new node and add it to the list
                Cic.P000001.Common.TreeNode Node = new Cic.P000001.Common.TreeNode();
                Node.DisplayName = LoopBrand.NAME;
                Node.HasDetails = false;
                Node.HasPictures = false;
                Node.Level = new Level(1, CnstSelectBrandText);
                Node.NewPrice = 0;
                Node.Parameters = null;
                Node.Key = key + ">" + LoopBrand.NATCODE;
                Node.ParentKey = key.ParentKey;
                Node.IsType = false;
                Nodes.Add(Node);
            }

            // Return the list as an array
            return Nodes.ToArray();
        }

        public static Cic.P000001.Common.TreeNode[] GetModelGroups(Cic.OpenOne.Common.Model.DdEurotax.DdEurotaxExtended context, EurotaxKey key)
        {
            return GetModelGroups(context, key, null, null);
        }
        // Gets the model groups for specified brand name
        public static Cic.P000001.Common.TreeNode[] GetModelGroups(Cic.OpenOne.Common.Model.DdEurotax.DdEurotaxExtended context, EurotaxKey key, FilterParam[] filter, SearchParam[] search)
        {

            // Query ETGMODLEVONE
            var Groups = from Group in context.ETGMODLEVONE
                         where Group.MARKET == key.Market
                         && Group.LANGCODE == key.Language
                         && Group.VEHTYPE == key.VehicleType
                         && Group.MAKCD == key.BrandCode
                         && Group.RECSTATUS != 9
                         orderby Group.NAME
                         select Group;

            // Create a node list
            List<Cic.P000001.Common.TreeNode> Nodes = new List<TreeNode>();

            // Itereate through all rows
            foreach (ETGMODLEVONE LoopGroup in Groups)
            {
                // Create a new node and add it to the list
                Cic.P000001.Common.TreeNode Node = new Cic.P000001.Common.TreeNode();
                Node.DisplayName = LoopGroup.NAME;
                Node.HasDetails = false;
                Node.HasPictures = false;
                Node.Level = new Level(2, CnstSelectModelGroupText);
                Node.NewPrice = 0;
                Node.Parameters = null;
                Node.Key = key + ">" + LoopGroup.CODE;
                Node.ParentKey = key.ParentKey;
                Node.IsType = false;
                Nodes.Add(Node);
            }

            // Return the list as an array
            return Nodes.ToArray();
        }

        public static Cic.P000001.Common.TreeNode[] GetModels(Cic.OpenOne.Common.Model.DdEurotax.DdEurotaxExtended context, EurotaxKey key)
        {
            return GetModels(context, key, null, null);
        }
        // Gets models for specified model group
        public static Cic.P000001.Common.TreeNode[] GetModels(Cic.OpenOne.Common.Model.DdEurotax.DdEurotaxExtended context, EurotaxKey key, FilterParam[] filter, SearchParam[] search)
        {

            // Query ETGMODEL
            var Models = from Model in context.ETGMODEL
                         where Model.MARKET == key.Market
                         && Model.LANGCODE == key.Language
                         && Model.VEHTYPE == key.VehicleType
                         && Model.MAKCD == key.BrandCode
                         && Model.RECSTATUS != 9
                         && Model.NAMEGRP1 == (from Group
                                               in context.ETGMODLEVONE
                                               where Group.CODE == key.GroupCode
                                               select Group).FirstOrDefault().NAME
                         orderby Model.NAME ascending, Model.IMPBEGIN descending
                         select Model;

            // Create a node list
            List<Cic.P000001.Common.TreeNode> Nodes = new List<TreeNode>();

            // Loop through all rows
            foreach (ETGMODEL LoopModel in Models)
            {
                // Get production years
                String ProductionYears = "";
                if (!String.IsNullOrEmpty(LoopModel.IMPBEGIN) && LoopModel.IMPBEGIN.Length == 6)
                {
                    ProductionYears = LoopModel.IMPBEGIN.Substring(0, 4);
                    if (!String.IsNullOrEmpty(LoopModel.IMPEND) && LoopModel.IMPEND.Length == 6)
                        ProductionYears += " - " + LoopModel.IMPEND.Substring(0, 4);
                    else
                        ProductionYears += "+";
                }



                // Create a new node
                Cic.P000001.Common.TreeNode Node = new TreeNode();
                Node.DisplayName = LoopModel.NAME + " (" + ProductionYears + ")";
                Node.HasDetails = false;
                Node.HasPictures = false;
                Node.Level = new Level(3, CnstSelectModelText);
                Node.NewPrice = 0;
                Node.Parameters = null;
                Node.Key = key + ">" + LoopModel.NATCODE;
                Node.ParentKey = key.ParentKey;
                Node.IsType = false;

                // Add node to the list
                Nodes.Add(Node);
            }

            // Return the list
            return Nodes.ToArray();
        }

        public static Cic.P000001.Common.TreeNode[] GetTypes(Cic.OpenOne.Common.Model.DdEurotax.DdEurotaxExtended context, EurotaxKey key)
        {
            return GetTypes(context, key, null, null);
        }

        // Get types for specified model
        public static Cic.P000001.Common.TreeNode[] GetTypes(Cic.OpenOne.Common.Model.DdEurotax.DdEurotaxExtended context, EurotaxKey key, FilterParam[] filter, SearchParam[] search)
        {

            //String query = "select etgtype.name,kw,impbegin,impend, etgtype.natcode,txtfueltypecd2,secfueltypcd2,txttranstypecd2,txtbodyco1cd2 from etgprice,etgtype where etgprice.natcode = etgtype.natcode and etgtype.natcode like '102%' and  etgtype.vehtype=10 and  etgtype.market='CH' and modcd=1643 and  etgtype.recstatus!=9 and currency='CHF'";


            var predicateMain = PredicateBuilder.True<ETGTYPE>();

            predicateMain = predicateMain.And(e => e.MARKET == key.Market);
            predicateMain = predicateMain.And(e => e.VEHTYPE == key.VehicleType);
            predicateMain = predicateMain.And(e => e.MODCD == key.ModelCode);
            predicateMain = predicateMain.And(e => e.RECSTATUS != 9);
            if (key.NatCode != null && key.NatCode.Length > 0)
                predicateMain = predicateMain.And(e => e.NATCODE == key.NatCode);

            var predicate1 = PredicateBuilder.True<ETGTYPE>();
            if (filter != null)
            {
                foreach (FilterParam fp in filter)
                {
                    String filterPattern = fp.Filter.ToLower();
                    predicate1 = predicate1.And(e => e.NAME.ToLower().Contains(filterPattern));
                }
            }
            /* if (filter != null && search != null && filter.Filter.Equals(search.Pattern));
             else */
            if (search != null)
            {
                foreach (SearchParam spar in search)
                {
                    String sp = spar.Pattern.ToLower();
                    predicate1 = predicate1.And(e => e.NAME.ToLower().Contains(sp));
                }
            }


            if (search != null || filter != null)
                predicateMain = predicateMain.And(predicate1.Expand());


            var Types = context.ETGTYPE.AsExpandable().Where(predicateMain.Expand()).OrderBy(e => e.NAME).OrderBy(e => e.IMPBEGIN);

            // Query ETGTYPE
            /*  var Types = from Type in context.ETGTYPE
                          join Price in context.ETGPRICE
                          on Type.NATCODE equals Price.NATCODE
                          where Type.MARKET == key.Market
                           && Type.VEHTYPE == key.VehicleType
                           && Type.MODCD == key.ModelCode
                           && Type.RECSTATUS != 9
                           && Price.CURRENCY == key.Currency
                          orderby Type.NAME, Type.IMPBEGIN descending
                          select new { Type, Price };*/

            // Create a node list
            List<Cic.P000001.Common.TreeNode> Nodes = new List<TreeNode>();

            // Loop through all rows
            foreach (var LoopType2 in Types)
            {
                var infos = from Type in context.ETGTYPE
                            join Price in context.ETGPRICE
                            on Type.NATCODE equals Price.NATCODE
                            where Type.NATCODE == LoopType2.NATCODE
                            && Price.CURRENCY == key.Currency
                            select new { Type, Price };
                var LoopType = infos.FirstOrDefault();

                string Years = LoopType.Type.IMPBEGIN.Substring(0, 4);
                if (LoopType.Type.IMPEND != null && LoopType.Type.IMPEND.Length >= 4)
                    Years += " - " + LoopType.Type.IMPEND.Substring(0, 4);
                else
                    Years += "+";

                // Create a new node
                Cic.P000001.Common.TreeNode Node = new TreeNode();
                Node.DisplayName = LoopType.Type.NAME + ", " + LoopType.Type.KW + " kW (" + Years + ")";
                Node.HasDetails = true;
                Node.HasPictures = false;
                Node.Level = new Level(4, CnstSelectTypeText);
                Node.Price = (double)LoopType.Price.NP1;
                Node.NewPrice = (double)LoopType.Price.NP1;
                Node.Parameters = null;
                Node.Key = key + ">" + LoopType.Type.NATCODE;
                Node.ParentKey = key.ParentKey;
                Node.IsType = true;

                // Add node to the list
                Nodes.Add(Node);
            }

            // Return the list
            return Nodes.ToArray();
        }
        // Get types for specified model
        public static Cic.P000001.Common.TreeNode[] GetTypesByNatCode(Cic.OpenOne.Common.Model.DdEurotax.DdEurotaxExtended context, EurotaxKey key)
        {

            // Query ETGTYPE
            var Types = from Type in context.ETGTYPE
                        join Price in context.ETGPRICE
                        on Type.NATCODE equals Price.NATCODE
                        where Type.MARKET == key.Market
                         && Type.VEHTYPE == key.VehicleType
                         && Type.MODCD == key.ModelCode
                         && Type.NATCODE == key.NatCode
                         && Type.RECSTATUS != 9
                         && Price.CURRENCY == key.Currency
                        orderby Type.NAME, Type.IMPBEGIN descending
                        select new { Type, Price };

            // Create a node list
            List<Cic.P000001.Common.TreeNode> Nodes = new List<TreeNode>();

            // Loop through all rows
            foreach (var LoopType in Types)
            {
                string Years = LoopType.Type.IMPBEGIN.Substring(0, 4);
                if (LoopType.Type.IMPEND != null && LoopType.Type.IMPEND.Length >= 4)
                    Years += " - " + LoopType.Type.IMPEND.Substring(0, 4);
                else
                    Years += "+";

                // Create a new node
                Cic.P000001.Common.TreeNode Node = new TreeNode();
                Node.DisplayName = LoopType.Type.NAME + ", " + LoopType.Type.KW + " kW (" + Years + ")";
                Node.HasDetails = true;
                Node.HasPictures = false;
                Node.Level = new Level(4, CnstSelectTypeText);
                Node.Price = (double)LoopType.Price.NP1;
                Node.NewPrice = (double)LoopType.Price.NP1;
                Node.Parameters = null;
                Node.Key = key + ">" + LoopType.Type.NATCODE;
                Node.ParentKey = key.ParentKey;
                Node.IsType = true;

                // Add node to the list
                Nodes.Add(Node);
            }

            // Return the list
            return Nodes.ToArray();
        }

        // Gets technical details
        internal static TreeNodeDetail[] GetTypeTechnicalDetails(DdEurotaxExtended context, EurotaxKey key)
        {
            // Query ETGTYPE
            var Types = from Type in context.ETGTYPE
                        where Type.MARKET == key.Market
                         && Type.VEHTYPE == key.VehicleType
                         && Type.NATCODE == key.NatCode
                         && Type.RECSTATUS != 9
                        select Type;

            // Create detail list
            List<TreeNodeDetail> Details = new List<TreeNodeDetail>();


            // Get the first and the only one result
            var SpecifiedType = Types.First();

            // Declare variables
            TreeNodeDetail Detail;
            TreeNodeDetailTypeConstants Category = TreeNodeDetailTypeConstants.Technic;
            TreeNodeDetailValueTypeConstants DetailType;


            // Get data:

            // Segmentation
            var Segmentation = (from Text in context.ETGTXTTABEL
                                where Text.CODE == SpecifiedType.TXTSEG1CD2
                                && Text.MARKET == key.Market
                                && Text.LANGCODE == key.Language
                                select Text).First();
            DetailType = TreeNodeDetailValueTypeConstants.Undefined;
            Detail = new TreeNodeDetail(Category, DetailType, getTranslation(key.Language, General), getTranslation(key.Language, EurotaxData.Segmentation), Segmentation.TEXTLONG);
            Details.Add(Detail);

            // Vehicle category
            var Segmentation2 = (from Text in context.ETGTXTTABEL
                                 where Text.CODE == SpecifiedType.TXTSEG1CD2
                                 && Text.MARKET == key.Market
                                 && Text.LANGCODE == key.Language
                                 select Text).First();
            DetailType = TreeNodeDetailValueTypeConstants.Undefined;
            Detail = new TreeNodeDetail(Category, DetailType, getTranslation(key.Language, General), getTranslation(key.Language, Vehiclecategory), Segmentation2.TEXTLONG);
            Details.Add(Detail);

            // Weight
            DetailType = TreeNodeDetailValueTypeConstants.Undefined;
            Detail = new TreeNodeDetail(Category, DetailType, getTranslation(key.Language, General), getTranslation(key.Language, Totalweight), SpecifiedType.TOTWGT + " kg");
            Details.Add(Detail);

            // Length
            DetailType = TreeNodeDetailValueTypeConstants.Undefined;
            Detail = new TreeNodeDetail(Category, DetailType, getTranslation(key.Language, General), getTranslation(key.Language, Length), SpecifiedType.LENGTH + " mm");
            Details.Add(Detail);

            // Width
            DetailType = TreeNodeDetailValueTypeConstants.Undefined;
            Detail = new TreeNodeDetail(Category, DetailType, getTranslation(key.Language, General), getTranslation(key.Language, Width), SpecifiedType.WIDTH + " mm");
            Details.Add(Detail);

            // Height
            DetailType = TreeNodeDetailValueTypeConstants.Undefined;
            Detail = new TreeNodeDetail(Category, DetailType, getTranslation(key.Language, General), getTranslation(key.Language, Height), SpecifiedType.HEIGHT + " mm");
            Details.Add(Detail);

            // Wheelbase
            DetailType = TreeNodeDetailValueTypeConstants.Undefined;
            Detail = new TreeNodeDetail(Category, DetailType, getTranslation(key.Language, General), getTranslation(key.Language, Wheelbase), SpecifiedType.WHEELB1 + " mm");
            Details.Add(Detail);

            // Fuel type
            var FuelType = (from Text in context.ETGTXTTABEL
                            where Text.CODE == SpecifiedType.TXTFUELTYPECD2
                            && Text.MARKET == key.Market
                            && Text.LANGCODE == key.Language
                            select Text).First();
            DetailType = TreeNodeDetailValueTypeConstants.HieFuelType;
            Detail = new TreeNodeDetail(Category, DetailType, getTranslation(key.Language, Engine), getTranslation(key.Language, Fueltype), FuelType.TEXTLONG);
            Details.Add(Detail);

            // Number of cylinders
            DetailType = TreeNodeDetailValueTypeConstants.TecCountOfCylinder;
            Detail = new TreeNodeDetail(Category, DetailType, getTranslation(key.Language, Engine), getTranslation(key.Language, Cylinders), SpecifiedType.CYLINDER.ToString());
            Details.Add(Detail);

            // Cylinder arrangement
            var CylinderArrangement = (from Text in context.ETGTXTTABEL
                                       where Text.CODE == SpecifiedType.TXTCYLARRCD2
                                       && Text.MARKET == key.Market
                                       && Text.LANGCODE == key.Language
                                       select Text).First();
            DetailType = TreeNodeDetailValueTypeConstants.HieFuelType;
            Detail = new TreeNodeDetail(Category, DetailType, getTranslation(key.Language, Engine), getTranslation(key.Language, Cylinderarrangement), CylinderArrangement.TEXTLONG);
            Details.Add(Detail);

            // Cylinders capacity
            DetailType = TreeNodeDetailValueTypeConstants.TecCylinderCapacity;
            Detail = new TreeNodeDetail(Category, DetailType, getTranslation(key.Language, Engine), getTranslation(key.Language, Capacity), SpecifiedType.CAPTECH + " ccm");
            Details.Add(Detail);

            // Exhaust Treatmend
            var ExhaustTreatment = (from Text in context.ETGTXTTABEL
                                    where Text.CODE == SpecifiedType.TXTEXHTREATCD2
                                    && Text.MARKET == key.Market
                                    && Text.LANGCODE == key.Language
                                    select Text).First();
            DetailType = TreeNodeDetailValueTypeConstants.HieFuelType;
            Detail = new TreeNodeDetail(Category, DetailType, getTranslation(key.Language, Engine), getTranslation(key.Language, Exhausttreatment), ExhaustTreatment.TEXTLONG);
            Details.Add(Detail);

            // Valves per cylinder
            DetailType = TreeNodeDetailValueTypeConstants.Undefined;
            Detail = new TreeNodeDetail(Category, DetailType, getTranslation(key.Language, Engine), getTranslation(key.Language, Valvespercylinder), SpecifiedType.VALVPCYL.ToString());
            Details.Add(Detail);

            // Charger type
            var ChargerType = (from Text in context.ETGTXTTABEL
                               where Text.CODE == SpecifiedType.TXTCHARGECD2
                               && Text.MARKET == key.Market
                               && Text.LANGCODE == key.Language
                               select Text).First();
            DetailType = TreeNodeDetailValueTypeConstants.HieFuelType;
            Detail = new TreeNodeDetail(Category, DetailType, getTranslation(key.Language, Engine), getTranslation(key.Language, Chargertype), ChargerType.TEXTLONG);
            Details.Add(Detail);

            // Horse power
            DetailType = TreeNodeDetailValueTypeConstants.TecEnginePerformancePS;
            Detail = new TreeNodeDetail(Category, DetailType, getTranslation(key.Language, Engine), getTranslation(key.Language, Horsepower), SpecifiedType.KW.ToString() + " kW / " + SpecifiedType.HP.ToString() + " PS");
            Details.Add(Detail);

            // Torque
            DetailType = TreeNodeDetailValueTypeConstants.Undefined;
            Detail = new TreeNodeDetail(Category, DetailType, getTranslation(key.Language, Engine), getTranslation(key.Language, Torque), SpecifiedType.TORQUE + " Nm");
            Details.Add(Detail);

            // Consumption index
            DetailType = TreeNodeDetailValueTypeConstants.Undefined;
            Detail = new TreeNodeDetail(Category, DetailType, getTranslation(key.Language, Engine), getTranslation(key.Language, Consumptionindex), SpecifiedType.CONSINDEX.ToString());
            Details.Add(Detail);

            // Pollution norm
            var PollutionNorm = (from Text in context.ETGTXTTABEL
                                 where Text.CODE == SpecifiedType.TXTPOLLNORMCD2
                                 && Text.MARKET == key.Market
                                 && Text.LANGCODE == key.Language
                                 select Text).First();
            DetailType = TreeNodeDetailValueTypeConstants.Undefined;
            Detail = new TreeNodeDetail(Category, DetailType, getTranslation(key.Language, Engine), getTranslation(key.Language, EurotaxData.PollutionNorm), PollutionNorm.TEXTLONG.ToString());
            Details.Add(Detail);


            // Body type
            var BodyType = (from Text in context.ETGTXTTABEL
                            where Text.CODE == SpecifiedType.TXTBODYCO1CD2
                            && Text.MARKET == key.Market
                            && Text.LANGCODE == key.Language
                            select Text).First();
            DetailType = TreeNodeDetailValueTypeConstants.HieBodyType;
            Detail = new TreeNodeDetail(Category, DetailType, getTranslation(key.Language, Body), getTranslation(key.Language, Bodytype), BodyType.TEXTLONG);
            Details.Add(Detail);

            // Number of doors
            DetailType = TreeNodeDetailValueTypeConstants.TecCountOfDoors;
            Detail = new TreeNodeDetail(Category, DetailType, getTranslation(key.Language, Body), getTranslation(key.Language, Doors), SpecifiedType.DOOR.ToString());
            Details.Add(Detail);

            // Roof loading
            DetailType = TreeNodeDetailValueTypeConstants.TecCountOfDoors;
            Detail = new TreeNodeDetail(Category, DetailType, getTranslation(key.Language, Body), getTranslation(key.Language, Roofloading), SpecifiedType.ROOFLOAD + " kg");
            Details.Add(Detail);

            // Transmission type
            DetailType = TreeNodeDetailValueTypeConstants.TecTransmissionType;
            var TransmissionType = (from Text in context.ETGTXTTABEL
                                    where Text.CODE == SpecifiedType.TXTTRANSTYPECD2
                                    && Text.MARKET == key.Market
                                    && Text.LANGCODE == key.Language
                                    select Text).First();
            Detail = new TreeNodeDetail(Category, DetailType, getTranslation(key.Language, Transmission), getTranslation(key.Language, Transmissiontype), TransmissionType.TEXTLONG);
            Details.Add(Detail);

            // Drive type
            DetailType = TreeNodeDetailValueTypeConstants.Undefined;
            var DriveType = (from Text in context.ETGTXTTABEL
                             where Text.CODE == SpecifiedType.TXTDRIVETYPECD2
                             && Text.MARKET == key.Market
                             && Text.LANGCODE == key.Language
                             select Text).First();
            Detail = new TreeNodeDetail(Category, DetailType, getTranslation(key.Language, Transmission), getTranslation(key.Language, Drivetype), DriveType.TEXTLONG);
            Details.Add(Detail);

            // Number of forward gears
            DetailType = TreeNodeDetailValueTypeConstants.TecCountOfGears;
            Detail = new TreeNodeDetail(Category, DetailType, getTranslation(key.Language, Transmission), getTranslation(key.Language, Numberofgears), SpecifiedType.NUMGEARF.ToString());
            Details.Add(Detail);

            // Number of seats
            DetailType = TreeNodeDetailValueTypeConstants.TecCountOfSeats;
            Detail = new TreeNodeDetail(Category, DetailType, getTranslation(key.Language, Interior), getTranslation(key.Language, Seats), SpecifiedType.SEAT.ToString());
            Details.Add(Detail);

            // Boot capacity
            DetailType = TreeNodeDetailValueTypeConstants.TecLuggageSpace;
            Detail = new TreeNodeDetail(Category, DetailType, getTranslation(key.Language, Interior), getTranslation(key.Language, Bootcapacity), SpecifiedType.TRUNKCAPMED + " l");
            Details.Add(Detail);


            // Return details
            return Details.ToArray();
        }


        // Gets standard equipment for specified car
        public static TreeNodeDetail[] GetStandardEquipment(DdEurotaxExtended context, EurotaxKey key)
        {

            // Query ETGADDITION
            var Additions = from Addition in context.ETGADDITION
                            join Text in context.ETGEQTEXT
                            on Addition.EQCODE equals Text.EQCODE
                            where Addition.MARKET == key.Market
                         && Addition.VEHTYPE == key.VehicleType
                         && Addition.NATCODE == key.NatCode
                         && Addition.RECSTATUS != 9
                         && Addition.FLAG == 0
                         && Addition.FLAGPACK != 2
                            select new { Addition, Text };

            // Create a detail list
            List<TreeNodeDetail> Details = new List<TreeNodeDetail>();

            // Loop through all rows
            foreach (var LoopAddition in Additions)
            {
                // Create a detail
                TreeNodeDetailTypeConstants Category = TreeNodeDetailTypeConstants.Technic;
                TreeNodeDetailValueTypeConstants DetailType = TreeNodeDetailValueTypeConstants.Undefined;
                TreeNodeDetail Detail = new TreeNodeDetail(Category, DetailType, null, null, LoopAddition.Text.TEXT);
                Details.Add(Detail);
            }

            // Return details
            return Details.ToArray();
        }


        // Gets optional components for specified type
        public static Cic.P000001.Common.Component[] GetEquipment(DdEurotaxExtended context, EurotaxKey key)
        {

            // Query ETGADDITION
            var Additions = from Addition in context.ETGADDITION
                            join Text in context.ETGEQTEXT
                            on Addition.EQCODE equals Text.EQCODE
                            where Text.LANGCODE == key.Language
                            && Text.MARKET == key.Market
                            && Text.VEHTYPE == key.VehicleType
                            && Addition.MARKET == key.Market
                         && Addition.VEHTYPE == key.VehicleType
                         && Addition.NATCODE == key.NatCode
                         && Addition.RECSTATUS != 9
                         && Addition.FLAG != 0
                         && Addition.FLAGPACK != 1
                            orderby Addition.VAL descending
                            select new { Addition, Text };

            // Create component list
            List<Cic.P000001.Common.Component> Components = new List<Component>();

            // Create a list for used EqCodes
            List<long> EqCodes = new List<long>();

            // Iterate through all rows
            foreach (var LoopAddition in Additions)
                // If EqCode isn't used
                if (!EqCodes.Contains((long)LoopAddition.Addition.EQCODE))
                {
                    // Create a new component
                    Cic.P000001.Common.Component Component = new Component();
                    Component.HasPictures = false;
                    Component.ComponentTypeConstant = ComponentTypeConstants.Equipment;
                    Component.Category = "Equipment";
                    Component.HasDetails = false;
                    Component.Selectable = LoopAddition.Addition.FLAG != 4;
                    Component.Key = LoopAddition.Addition.ID.ToString();
                    Component.NewPrice = (double)LoopAddition.Addition.PRICE1;
                    Component.Price = (double)LoopAddition.Addition.PRICE1;
                    Component.DisplayName = LoopAddition.Text.TEXT;
                    Component.Components = null;
                    Component.Parameters = null;
                    Components.Add(Component);

                    // Add EqCode to the list
                    EqCodes.Add((long)LoopAddition.Addition.EQCODE);
                }

            // Return components
            return Components.ToArray();
        }

        // Gets all packages for specific type
        public static Cic.P000001.Common.Component[] GetPackages(DdEurotaxExtended context, EurotaxKey key)
        {
            // Query ETGADDITION
            var Additions = from Addition in context.ETGADDITION
                            join Text in context.ETGEQTEXT
                            on Addition.EQCODE equals Text.EQCODE
                            where Text.LANGCODE == key.Language
                            && Text.MARKET == key.Market
                            && Text.VEHTYPE == key.VehicleType
                            && Addition.MARKET == key.Market
                         && Addition.VEHTYPE == key.VehicleType
                         && Addition.NATCODE == key.NatCode
                         && Addition.RECSTATUS != 9
                         && Addition.FLAG != 0
                         && Addition.FLAGPACK == 1
                            orderby Addition.VAL descending
                            select new { Addition, Text };


            // Create component list
            List<Cic.P000001.Common.Component> Components = new List<Component>();

            // Create a list for used EqCodes
            List<long> EqCodes = new List<long>();

            // Iterate through all rows
            foreach (var LoopAddition in Additions)
                // If EqCode isn't used
                if (!EqCodes.Contains((long)LoopAddition.Addition.EQCODE))
                {
                    // Create a new component
                    Cic.P000001.Common.Component Component = new Component();
                    Component.HasPictures = false;
                    Component.ComponentTypeConstant = ComponentTypeConstants.Package;
                    Component.Category = "Packages";
                    Component.Components = GetPackageContents(context, key, (long)LoopAddition.Addition.EQCODE);
                    Component.HasDetails = Component.Components != null;
                    Component.Selectable = LoopAddition.Addition.FLAG != 4;
                    Component.Key = LoopAddition.Addition.ID.ToString();
                    Component.NewPrice = (double)LoopAddition.Addition.PRICE1;
                    Component.Price = (double)LoopAddition.Addition.PRICE1;
                    Component.DisplayName = LoopAddition.Text.TEXT;
                    Component.Parameters = null;
                    Components.Add(Component);

                    // Add EqCode to the list
                    EqCodes.Add((long)LoopAddition.Addition.EQCODE);
                }

            // Return components
            return Components.ToArray();
        }

        // Converts the formula from Eurotax format to Jato format
        private static String ConvertFormula(String formula)
        {
            LiteralType Literal = LiteralType.None;
            int I = 0;
            while (I < formula.Length)
            {
                LiteralType TempLiteral = LiteralType.None;

                if (Char.IsDigit(formula[I]))
                    TempLiteral = LiteralType.Number;
                else if (Char.IsLetter(formula[I]))
                    TempLiteral = LiteralType.Text;
                else if (formula[I] == '(' || formula[I] == ')')
                    TempLiteral = LiteralType.Parenthesis;

                if (Literal != LiteralType.Number && TempLiteral == LiteralType.Number)
                {
                    if (Literal == LiteralType.Text)
                    {
                        formula = formula.Insert(I, " ");
                        I++;
                    }
                    formula = formula.Insert(I, "{");
                    I++;
                }

                if (Literal == LiteralType.Number && TempLiteral != LiteralType.Number)
                {
                    formula = formula.Insert(I, "}");
                    I++;
                    if (TempLiteral == LiteralType.Text)
                    {
                        formula = formula.Insert(I, " ");
                        I++;
                    }
                }

                Literal = TempLiteral;
                I++;
            }

            if (Literal == LiteralType.Number)
                formula += "}";

            return formula;
        }


        // Gets Id from EqCode
        public static long GetComponentId(DdEurotaxExtended context, EurotaxKey key, long eqCode)
        {

            // Query ETGADDITION
            var Additions = from Addition in context.ETGADDITION
                            where Addition.MARKET == key.Market
                         && Addition.VEHTYPE == key.VehicleType
                         && Addition.NATCODE == key.NatCode
                         && Addition.RECSTATUS != 9
                         && Addition.FLAG != 0
                         && Addition.EQCODE == eqCode
                            orderby Addition.VAL descending
                            select Addition.ID;

            // If there is no such an addition, return 0
            if (Additions.Count() == 0)
                return 0;

            // Get first result
            var AdditionId = Additions.FirstOrDefault();

            // Return Id
            return AdditionId;
        }


        // Gets the relation type of two components
        public static int GetRelationType(DdEurotaxExtended context, EurotaxKey key, long additionId, long eqCode)
        {

            // Query ETGEXCLUDE
            var Relations = from Relation in context.ETGEXCLUDE
                            where Relation.MARKET == key.Market
                            && Relation.VEHTYPE == key.VehicleType
                            && Relation.NATCODE == key.NatCode
                            && Relation.RECSTATUS != 9
                            && Relation.ADDCD == additionId
                            && Relation.EQTCODECD == eqCode
                            select Relation;

            // If there is no such an addition, return 0
            if (Relations.Count() == 0)
                return 0;

            // Return exclude/include flag
            return (int)Relations.FirstOrDefault().FLAG;
        }


        // Checks if the specified component is on the list
        public static bool ContainsComponent(Cic.P000001.Common.Component component, Cic.P000001.Common.Component[] components)
        {
            if (components == null)
                return false;

            foreach (Cic.P000001.Common.Component LoopComponent in components)
                if (LoopComponent.Key == component.Key)
                    return true;

            return false;
        }


        // Gets the component names included in the package
        public static Cic.P000001.Common.ComponentDetail[] GetPackageDetails(DdEurotaxExtended context, EurotaxKey key, long eqCode)
        {
            // Query ETGCONTENT
            var Additions = from Addition in context.ETGCONTENT
                            join Text in context.ETGEQTEXT
                            on Addition.EQCODE equals Text.EQCODE
                            where Addition.MARKET == key.Market
                         && Addition.VEHTYPE == key.VehicleType
                         && Addition.NATCODE == key.NatCode
                         && Addition.RECSTATUS != 9
                         && Addition.EQCODEP == eqCode
                         && Text.LANGCODE == key.Language
                            select new { Addition, Text };

            // Create a list
            List<Cic.P000001.Common.ComponentDetail> Result = new List<ComponentDetail>();

            // Add details to the list
            foreach (var LoopAddition in Additions)
            {
                Cic.P000001.Common.ComponentDetail Detail = new ComponentDetail();
                Detail.Value = LoopAddition.Text.TEXT;

                Result.Add(Detail);
            }

            // Return details
            return Result.ToArray();
        }


        // Get exclusions or requirements when needed
        public static Cic.P000001.Common.DataProvider.CheckComponentExpression[] GetComponentRelations(DdEurotaxExtended context, EurotaxKey key, Cic.P000001.Common.Component component, Cic.P000001.Common.Component[] components)
        {
            // Create an expression list
            List<Cic.P000001.Common.DataProvider.CheckComponentExpression> Expressions = new List<Cic.P000001.Common.DataProvider.CheckComponentExpression>();

            // Get Addition ID
            long additionId = long.Parse(component.Key);

            // Query ETGFORMULA
            var Formulas = from Formula in context.ETGFORMULA
                           where Formula.MARKET == key.Market
                           && Formula.VEHTYPE == key.VehicleType
                           && Formula.ADDCD == additionId
                           && Formula.RECSTATUS != 9
                           select Formula;

            // Iterate through all formulas
            foreach (var LoopFormula in Formulas)
            {
                // Add braces to the formula
                string FormulaText = ConvertFormula(LoopFormula.FORMULA);

                // Create the EqCode list
                List<string> EqCodes = new List<string>();

                int LeftBraceIndex = 0, RightBraceIndex = 0;
                do
                {
                    // Search for left and right brace
                    LeftBraceIndex = FormulaText.IndexOf("{", LeftBraceIndex);
                    RightBraceIndex = FormulaText.IndexOf("}", RightBraceIndex);

                    // If there is something between them...
                    if (RightBraceIndex > LeftBraceIndex)
                    {
                        // Get EqCode from the formula and add it to the list
                        string EqCode = FormulaText.Substring(LeftBraceIndex + 1, RightBraceIndex - LeftBraceIndex - 1);
                        EqCodes.Add(EqCode);

                        LeftBraceIndex = RightBraceIndex;
                        RightBraceIndex++;
                    }

                }
                while (LeftBraceIndex != -1 && RightBraceIndex != -1); // Until there is nothing to find

                int RelationType = 0;

                // Loop through all EqCodes
                foreach (string EqCode in EqCodes)
                {
                    // Get Id from EqCode
                    long ComponentId = GetComponentId(context, key, long.Parse(EqCode));

                    // Get relation type
                    RelationType = GetRelationType(context, key, long.Parse(component.Key), long.Parse(EqCode));

                    // Replace EqCode with Id
                    FormulaText = FormulaText.Replace("{" + EqCode + "}", "{" + ComponentId + "}");
                }

                // Parse the formula
                Parser FormulaParser = new Parser(new StringReader(FormulaText));
                Expression<String> Expression = FormulaParser.ReadExpression();

                // Execute expression
                bool ExpressionResult = GetExpressionResult(Expression, components);

                // If this is exclusion and the result is true
                if (RelationType == 1 && ExpressionResult)
                    Expressions.Add(new ConflictCheckComponentExpression(Expression));

                // If this is requirement and the result is false
                if (RelationType == 2 && !ExpressionResult)
                    Expressions.Add(new RequirementCheckComponentExpression(Expression));
            }

            // Return expression list as array
            return Expressions.ToArray();
        }


        // Checks if specified component can be deleted
        public static CheckComponentExpression[] GetDeletionLimitations(DdEurotaxExtended context, EurotaxKey key, Cic.P000001.Common.Component component, Cic.P000001.Common.Component[] components)
        {
          
            // Create a temporary component list
            List<Cic.P000001.Common.Component> TempComponents = components.ToList();

            // Delete component from temporary list
            foreach (Cic.P000001.Common.Component LoopComponent in TempComponents)
                if (LoopComponent.Key == component.Key)
                {
                    TempComponents.Remove(LoopComponent);
                    break;
                }

            // Query ETGFORMULA
            var Formulas = from Formula in context.ETGFORMULA
                           where Formula.MARKET == key.Market
                           && Formula.VEHTYPE == key.VehicleType
                           && Formula.RECSTATUS != 9
                           && (from Addition in context.ETGADDITION
                               where Addition.MARKET == key.Market
                               && Addition.VEHTYPE == key.VehicleType
                               && Addition.NATCODE == key.NatCode
                               select Addition).Any(A => A.ID == Formula.ADDCD)
                           select Formula;


            // Iterate through all rows
            foreach (var LoopFormula in Formulas)
            {
                // Check current if formula is for a component existing on the list
                bool ComponentFound = false;
                foreach (Cic.P000001.Common.Component LoopComponent in TempComponents)
                    if (LoopComponent.Key == LoopFormula.ADDCD.ToString())
                    {
                        ComponentFound = true;
                        break;
                    }

                // If not, skip the formula
                if (!ComponentFound)
                    continue;

                // Add braces to the formula
                string FormulaText = ConvertFormula(LoopFormula.FORMULA);

                // Create the EqCode list
                List<string> EqCodes = new List<string>();

                int LeftBraceIndex = 0, RightBraceIndex = 0;
                do
                {
                    // Search for left and right brace
                    LeftBraceIndex = FormulaText.IndexOf("{", LeftBraceIndex);
                    RightBraceIndex = FormulaText.IndexOf("}", RightBraceIndex);

                    // If there is something between them...
                    if (RightBraceIndex > LeftBraceIndex)
                    {
                        // Get EqCode from the formula and add it to the list
                        string EqCode = FormulaText.Substring(LeftBraceIndex + 1, RightBraceIndex - LeftBraceIndex - 1);
                        EqCodes.Add(EqCode);

                        LeftBraceIndex = RightBraceIndex;
                        RightBraceIndex++;
                    }

                }
                while (LeftBraceIndex != -1 && RightBraceIndex != -1); // Until there is nothing to find

                int RelationType = 0;

                // Loop through all EqCodes
                foreach (string EqCode in EqCodes)
                {
                    // Get Id from EqCode
                    long ComponentId = GetComponentId(context, key, long.Parse(EqCode));

                    // Get relation type
                    RelationType = GetRelationType(context, key, LoopFormula.ADDCD, long.Parse(EqCode));

                    // Replace EqCode with Id
                    FormulaText = FormulaText.Replace("{" + EqCode + "}", "{" + ComponentId + "}");
                }

                // Parse the formula
                Parser FormulaParser = new Parser(new StringReader(FormulaText));
                Expression<String> Expression = FormulaParser.ReadExpression();

                // Execute expression
                bool ExpressionResult = GetExpressionResult(Expression, TempComponents.ToArray());

                // If this is requirement and the result is false, component can't be deleted
                if (RelationType == 2 && !ExpressionResult)
                    return new CheckComponentExpression[] { new RequirementCheckComponentExpression(Expression) };
            }

            // Component can be deleted
            return new CheckComponentExpression[0];
        }


        // Gets components included in the package
        public static Cic.P000001.Common.Component[] GetPackageContents(DdEurotaxExtended context, EurotaxKey key, long eqCode)
        {

            // Query ETGCONTENT
            var Additions = from Addition in context.ETGCONTENT
                            join Text in context.ETGEQTEXT
                            on Addition.EQCODE equals Text.EQCODE
                            where Addition.MARKET == key.Market
                         && Addition.VEHTYPE == key.VehicleType
                         && Addition.NATCODE == key.NatCode
                         && Addition.RECSTATUS != 9
                         && Addition.EQCODEP == eqCode
                         && Text.LANGCODE == key.Language
                            select new { Addition, Text };

            // If there are no rows, return null
            if (Additions.Count() == 0)
                return null;

            // Create a list of components
            List<Cic.P000001.Common.Component> Components = new List<Cic.P000001.Common.Component>();

            // Add components to the list
            foreach (var LoopAddition in Additions)
            {
                Cic.P000001.Common.Component Component = new Component();
                Component.Key = GetComponentId(context, key, LoopAddition.Addition.EQCODE).ToString();
                Component.Components = null;
                Component.ComponentTypeConstant = ComponentTypeConstants.Equipment;
                Component.HasDetails = false;
                Component.HasPictures = false;
                Component.NewPrice = 0;
                Component.Selectable = false;
                Component.Price = 0;
                Component.Category = "Package contents";
                Component.DisplayName = LoopAddition.Text.TEXT;
                Components.Add(Component);
            }

            // Return components
            return Components.ToArray();
        }


        // Executes given expression and returns its result
        public static bool GetExpressionResult(Expression<String> Expression, Cic.P000001.Common.Component[] components)
        {

            // The expression is an AND expression
            if (Expression is AndExpression<String>)
            {
                Expression<String> Expression1 = ((AndExpression<String>)Expression).LeftExpression;
                Expression<String> Expression2 = ((AndExpression<String>)Expression).RightExpression;

                return GetExpressionResult(Expression1, components) && GetExpressionResult(Expression2, components);
            }

            // The expression is an OR expression
            if (Expression is OrExpression<String>)
            {
                Expression<String> Expression1 = ((OrExpression<String>)Expression).LeftExpression;
                Expression<String> Expression2 = ((OrExpression<String>)Expression).RightExpression;

                return GetExpressionResult(Expression1, components) || GetExpressionResult(Expression2, components);
            }

            // The expression is a XOR expression
            if (Expression is XorExpression<String>)
            {
                Expression<String> Expression1 = ((XorExpression<String>)Expression).LeftExpression;
                Expression<String> Expression2 = ((XorExpression<String>)Expression).RightExpression;

                return GetExpressionResult(Expression1, components) ^ GetExpressionResult(Expression2, components);
            }

            // The expression is a NOT expression
            if (Expression is NotExpression<String>)
            {
                Expression<String> InnerExpression = ((NotExpression<String>)Expression).InnerExpression;

                return !GetExpressionResult(InnerExpression, components);
            }

            // The expression is a value
            if (Expression is ValueExpression<String>)
            {
                // Read the value
                String Value = ((ValueExpression<String>)Expression).Value;

                // Search for the component in array
                bool ComponentFound = false;
                foreach (Cic.P000001.Common.Component LoopComponent in components)
                    if (LoopComponent.Key == Value)
                    {
                        ComponentFound = true;
                        break;
                    }

                // Return appropriate value
                return ComponentFound;
            }

            // If the expression is of some other type
            return false;
        }
        #endregion

        #region My methods
        private static Dictionary<int, string> MyGetVehicleTypes(string language)
        {
             Dictionary<int, string> Result = new Dictionary<int, string>();
           /* using (DdEurotaxExtended context = new DdEurotaxExtended())
            {
                var roots = from e in context.ETGZVEHTYPE
                            select e;
                foreach(ETGZVEHTYPE o in roots)
                {
                     Result.Add(o.VEHTYPE,getTranslation(language, o.DESCRSHORT));
                }
            }*/
            using (DdOlExtended context = new DdOlExtended())
            {
                var ObTyps = from ObTyp in context.OBTYP
                         where ObTyp.NOEXTID == 0
                         && (ObTyp.IMPORTSOURCE == 2) && ObTyp.SYSOBTYPP ==null
                         select ObTyp;
                foreach(OBTYP o in ObTyps)
                {

                    Result.Add(Convert.ToInt32(o.AKLASSE), getTranslation(language, o.BEZEICHNUNG));
                }
               /*
                Result.Add(10, getTranslation(language, CnstCarsText));
                Result.Add(20, getTranslation(language, CnstCommercialVehiclesText));
                Result.Add(40, getTranslation(language, CnstMotorcyclesText));
               */
            }
             return Result;
        }

        private static TreeNode[] MySimpleSearchVehicleTypes(string name, Setting setting)
        {
            // Get vehicle types
            Dictionary<int, string> VehicleTypes = MyGetVehicleTypes(setting.SelectedLanguage);

            // Get the specified vehicle type
            var CurrentVehicleType = (from VehicleType in VehicleTypes
                                      where VehicleType.Value == name
                                      select VehicleType).FirstOrDefault();

            // Check if nothing was found
            if (CurrentVehicleType.Equals(default(KeyValuePair<int, string>)))
            {
                // Return an empty array
                return new TreeNode[0];
            }

            // Create a node
            TreeNode Node = new TreeNode();
            Node.DisplayName = name;
            Node.HasDetails = false;
            Node.HasPictures = false;
            Node.IsType = false;
            Node.Key = CurrentVehicleType.Key.ToString();
            Node.Level = new Level(0, getTranslation(setting, CnstSelectVehicleTypeText));
            Node.NewPrice = 0;
            Node.ParentKey = null;
            Node.Price = 0;

            // Return a new array with the node
            return new TreeNode[] { Node };
        }

        private static TreeNode[] MySimpleSearchBrand(string name, Setting setting)
        {
            // Create a context
            using (DdEurotaxExtended Context = new DdEurotaxExtended())
            {
                // Get the model
                var CurrentBrand = (from Brand in Context.ETGMAKE
                                    where Brand.NAME == name
                                    select Brand).FirstOrDefault();

                // Check if model was found
                if (CurrentBrand == null)
                {
                    // Return an empty array
                    return new TreeNode[0];
                }

                // Create a node
                TreeNode Node = new TreeNode();
                Node.DisplayName = name;
                Node.HasDetails = false;
                Node.HasPictures = false;
                Node.IsType = false;
                Node.Key = CurrentBrand.VEHTYPE + ">" + CurrentBrand.NATCODE;
                Node.ParentKey = CurrentBrand.VEHTYPE.ToString();
                Node.Level = new Level(1, getTranslation(setting, CnstSelectBrandText));
                Node.NewPrice = 0;
                Node.Price = 0;

                return new TreeNode[] { Node };
            }
        }

        private static TreeNode[] MySimpleSearchModelGroup(string name, Setting setting)
        {
            // Create a context
            using (DdEurotaxExtended Context = new DdEurotaxExtended())
            {
                // Get the model group
                var CurrentGroup = (from Group in Context.ETGMODLEVONE
                                    where Group.NAME == name
                                    select Group).FirstOrDefault();

                // Check if group was found
                if (CurrentGroup == null)
                {
                    // Return an empty array
                    return new TreeNode[0];
                }

                // Create a node
                TreeNode Node = new TreeNode();
                Node.DisplayName = name;
                Node.HasDetails = false;
                Node.HasPictures = false;
                Node.IsType = false;
                Node.Key = CurrentGroup.VEHTYPE + ">" + CurrentGroup.MAKCD.GetValueOrDefault() + ">" + CurrentGroup.CODE;
                Node.ParentKey = CurrentGroup.VEHTYPE + ">" + CurrentGroup.MAKCD.GetValueOrDefault();
                Node.Level = new Level(2, getTranslation(setting, CnstSelectModelGroupText));
                Node.NewPrice = 0;
                Node.Price = 0;

                return new TreeNode[] { Node };
            }
        }

        public static TreeNode[] SearchModel(SearchParam[] searchParams, Setting setting)
        {
            // Create a context
            using (DdEurotaxExtended Context = new DdEurotaxExtended())
            {

                var predicateMain = PredicateBuilder.True<ETGTYPE>();

                predicateMain = predicateMain.And(e => e.RECSTATUS != 9);

                var predicate1 = PredicateBuilder.True<ETGTYPE>();
                foreach (SearchParam sp in searchParams)
                {
                    String pattern = sp.Pattern.ToLower();
                    predicate1 = predicate1.And(e => e.NAME.ToLower().Contains(pattern));
                }

                predicateMain = predicateMain.And(predicate1.Expand()).Expand();
                var Types = from t in Context.ETGTYPE.Where(predicateMain)
                            join Price in Context.ETGPRICE
                             on t.NATCODE equals Price.NATCODE
                            where Price.CURRENCY == setting.SelectedCurrency
                            orderby t.NAME, t.IMPBEGIN descending
                            select new
                            {
                                Type = t,
                                Price

                            };


                // Create a node list
                List<Cic.P000001.Common.TreeNode> Nodes = new List<TreeNode>();

                // Loop through all rows
                foreach (var LoopType in Types)
                {

                    // Get the model group

                    string Years = LoopType.Type.IMPBEGIN.Substring(0, 4);
                    if (LoopType.Type.IMPEND != null && LoopType.Type.IMPEND.Length >= 4)
                        Years += " - " + LoopType.Type.IMPEND.Substring(0, 4);
                    else
                        Years += "+";

                    var price = LoopType.Price;// Context.ETGPRICE.Where(p => p.CURRENCY == setting.SelectedCurrency && p.NATCODE == LoopType.Type.NATCODE).First();

                    // Create a new node
                    Cic.P000001.Common.TreeNode Node = new TreeNode();
                    Node.DisplayName = LoopType.Type.NAME + ", " + LoopType.Type.KW + " kW (" + Years + ")";
                    Node.HasDetails = true;
                    Node.HasPictures = false;
                    Node.Level = new Level(4, CnstSelectTypeText);
                    Node.Price = (double)price.NP1;
                    Node.NewPrice = (double)price.NP1;
                    Node.Parameters = null;
                    Node.Key = price.VEHTYPE + ">" + LoopType.Type.MAKCD + ">" + LoopType.Type.MLOCD + ">" + price.NATCODE;
                   
                    Node.ParentKey = price.VEHTYPE + ">" + LoopType.Type.MAKCD + ">" + LoopType.Type.MLOCD;
                    Node.IsType = true;


                    if (setting.hasFilter())
                    {
                        var CurrentBrand = (from Brand in Context.ETGMAKE
                                            where Brand.NATCODE == LoopType.Type.MAKCD
                                            select Brand).FirstOrDefault();
                        if (!setting.filter(CurrentBrand.NAME))
                              Nodes.Add(Node);
                    }
                    else 
                    // Add node to the list
                    Nodes.Add(Node);
                }

               

                // Return the list
                return Nodes.ToArray();






            }
        }

        private static TreeNode[] MySimpleSearchModel(string name, Setting setting)
        {
            // Create a context
            using (DdEurotaxExtended Context = new DdEurotaxExtended())
            {
                // Get the model
                var CurrentModel = (from Model in Context.ETGMODEL
                                    where Model.NAME == name
                                    select Model).FirstOrDefault();

                // Check if model was found
                if (CurrentModel == null)
                {
                    // Return an empty array
                    return new TreeNode[0];
                }

                // Get the model group
                var CurrentGroup = (from Group in Context.ETGMODLEVONE
                                    where Group.NAME == CurrentModel.NAMEGRP1
                                    select Group).FirstOrDefault();

                // Check if group was found
                if (CurrentGroup == null)
                {
                    // Return an empty array
                    return new TreeNode[0];
                }

                // Create a node
                TreeNode Node = new TreeNode();
                Node.DisplayName = name;
                Node.HasDetails = false;
                Node.HasPictures = false;
                Node.IsType = false;
                Node.Key = CurrentModel.VEHTYPE + ">" + CurrentModel.MAKCD.GetValueOrDefault() + ">" + CurrentGroup.CODE + ">" + CurrentModel.NATCODE;
                Node.ParentKey = CurrentModel.VEHTYPE + ">" + CurrentModel.MAKCD.GetValueOrDefault() + ">" + CurrentGroup.CODE;
                Node.Level = new Level(3, getTranslation(setting, CnstSelectModelText));
                Node.NewPrice = 0;
                Node.Price = 0;

                return new TreeNode[] { Node };
            }
        }
        #endregion
    }
}
