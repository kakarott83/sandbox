namespace Cic.OpenOne.CarConfigurator.BO.ObTypDataProvider
{
    #region Using
    using Cic.One.Utils.Util;
    using CIC.Database.OL.EF4.Model;
    using Cic.OpenOne.Common.Util.Collection;
    using Cic.OpenOne.Common.Util.Config;
    using Cic.P000001.Common;
    using Cic.P000001.Common.DataProvider;
    using LinqKit;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Cic.OpenOne.Common.Model.DdOl;

    #endregion

    internal static class TreeNodeHelper
    {
        #region Private constants
        private const int CnstMaxLevel = 4;
        private const string CnstCarsText = "Cars";
        private const string CnstCommercialVehiclesText = "Commercial vehicles";
        private const string CnstMotorcyclesText = "Motorcycles";
        private const string CnstSelectVehicleTypeText = "Select vehicle type";
        private const string CnstSelectBrandText = "Select brand";
        private const string CnstSelectModelGroupText = "Select model group";
        private const string CnstSelectModelText = "Select model";
        private const string CnstSelectTypeText = "Select type";

        private const string Ecology = "Ecology";
        private const string Particles = "Particles";
        private const string CO2emission = "CO2 emission";
        private const string NOX = "NOX";
        private const string Power = "Power";
        private const string Consumption = "Consumption";
        private const string General = "General";

        private const int CnstImportSource1 = 2;
        private const int CnstImportSource2 = 0;
        private const int CnstNoExtId = 1;
        private static Dictionary<string, Dictionary<string, string>> translation = new Dictionary<string, Dictionary<string, string>>();
        #endregion

        static TreeNodeHelper()
        {
            Dictionary<string, string> t = new Dictionary<string, string>();

            t[CnstSelectVehicleTypeText] = "Fahrzeugart wählen";
            t[CnstSelectBrandText] = "Marke wählen";
            t[CnstSelectModelGroupText] = "Modellgruppe wählen";
            t[CnstSelectModelText] = "Modell wählen";
            t[CnstSelectTypeText] = "Typ wählen";
            t[Ecology] = "Ökologie";
            t[Particles] = "Partikel";
            t[CO2emission] = "CO2 Ausstoß";
            t[NOX] = "Stickoxid";
            t[Power] = "Leistung";
            t[Consumption] = "Verbrauch";
            t[General] = "Allgemein";
            t[CnstCarsText] = "PKW";
            t[CnstCommercialVehiclesText] = "Leicht-LKW / Transporter";
            t[CnstMotorcyclesText] = "Motorräder";

            translation["de-at"] = t;
            translation["de-ch"] = t;
            translation["de"] = t;
            translation["eude"] = t;
            translation["atde"] = t;
            translation["dede"] = t;
            translation["dech"] = t;


            t = new Dictionary<string, string>();

            t[CnstCarsText] = CnstCarsText;
            t[CnstCommercialVehiclesText] = CnstCommercialVehiclesText;
            t[CnstMotorcyclesText] = CnstMotorcyclesText;
            t[CnstSelectVehicleTypeText] = CnstSelectVehicleTypeText;
            t[CnstSelectBrandText] = CnstSelectBrandText;
            t[CnstSelectModelGroupText] = CnstSelectModelGroupText;
            t[CnstSelectModelText] = CnstSelectModelText;
            t[CnstSelectTypeText] = CnstSelectTypeText;
            t[Ecology] = Ecology;
            t[Particles] = Particles;
            t[CO2emission] = CO2emission;
            t[NOX] = NOX;
            t[Power] = Power;
            t[Consumption] = Consumption;
            t[General] = General;

            translation["en"] = t;
        }
        public static string getTranslation(string language, string key)
        {
            Dictionary<string, string> t;
            if (!translation.ContainsKey(language))
                t = translation["en"];
            t = translation[language];
            if (!t.ContainsKey(key))
                return "Unmapped Id " + key;
            return t[key];

        }

        public static string getTranslation(Setting setting, string key)
        {
            return getTranslation(setting.SelectedLanguage, key);

        }
        #region Methods
        public static TreeInfo GetTreeInfo(TreeNode treeNode, string language)
        {
            // Check if treeNode is null
            if (treeNode == null)
            {
                // Create a level list
                List<Level> Levels = new List<Level>();
                Levels.Add(new Level(0, getTranslation(language,CnstSelectVehicleTypeText)));
                Levels.Add(new Level(1, getTranslation(language,CnstSelectBrandText)));
                Levels.Add(new Level(2, getTranslation(language,CnstSelectModelGroupText)));
                Levels.Add(new Level(3, getTranslation(language,CnstSelectModelText)));
                Levels.Add(new Level(4, getTranslation(language,CnstSelectTypeText)));

                // Return tree info
                return new TreeInfo(true, CnstMaxLevel + 1, 0, null, null, null, Levels.ToArray());
            }

            // Throw an exception
            throw new System.Exception("Tree node has to be null.");
        }

        private static Dictionary<int, string> MyGetVehicleTypes(string language)
        {
            Dictionary<int, string> Result = new Dictionary<int, string>();
            Result.Add(10, getTranslation(language, CnstCarsText));
            Result.Add(20, getTranslation(language, CnstCommercialVehiclesText));
            Result.Add(40, getTranslation(language, CnstMotorcyclesText));
            return Result;
        }

        // Gets all available vehicle types
        public static Cic.P000001.Common.TreeNode[] GetVehicleTypes(Setting setting)
        {

            // Define all vehicle types
            Dictionary<int, String> Types = MyGetVehicleTypes(setting.SelectedLanguage);

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
                Node.ParentKey = null;
                Node.IsType = false;
                Nodes.Add(Node);
            }

            // Return the list as an array
            return Nodes.ToArray();
        }

        public static TreeNode[] GetNextLevel(TreeNode treeNode, DdOlExtended context, Setting setting)
        {
            // Create a nodes list
            List<TreeNode> TreeNodes = new List<TreeNode>();

            //select * from obtyp o where noextid=WantedLevel and 5=(select count(*) from obtyp start with sysobtyp=o.sysobtyp connect by prior sysobtypp=sysobtyp);
            // Query OBTYP
            var ObTyps = from ObTyp in context.OBTYP
                         where ObTyp.NOEXTID == CnstNoExtId
                         && (ObTyp.IMPORTSOURCE == CnstImportSource1 || ObTyp.IMPORTSOURCE == CnstImportSource2 || ObTyp.IMPORTSOURCE == null) && ObTyp.SYSOBTYPP !=null
                         select ObTyp;
            if(treeNode!=null && treeNode.Key!=null)
            {
                 ObTypKey key = new ObTypKey(treeNode.Key);
                 if (key.TypeId > 0)
                 {
                     ObTyps = from ObTyp in context.OBTYP
                              where ObTyp.SYSOBTYP == key.TypeId

                              select ObTyp;
                 }
            }
            // Get wanted tree level number
            int WantedLevel = treeNode == null || treeNode.Level == null ? 0 : treeNode.Level.Number + 1;
            decimal Ust = getGlobalUst(setting.customerCode,setting.sysperole);

           
            

            // Iterate through all last level ObTyps
            foreach (var LoopObTyp in ObTyps)
            {
                var CurrentObTyp = LoopObTyp;
                int CurrentLevel = CnstMaxLevel;

                // Iterate while the wanted level is not reached
                while (CurrentLevel > WantedLevel)
                {
                    // Check if ObTyp has parent
                    if (CurrentObTyp.SYSOBTYPP == null)
                    {
                        // Throw an exception
                       // throw new System.Exception("ObTyp at level " + CurrentLevel + " has no parent.");
                        CurrentLevel = -99;
                        break;
                    }

                    // Query OBTYP
                    CurrentObTyp = (from ObTyp in context.OBTYP
                                    where ObTyp.SYSOBTYP == CurrentObTyp.SYSOBTYPP
                                    select ObTyp).FirstOrDefault();

                    // Check if ObTyp is found
                    if (CurrentObTyp == null)
                    {
                        // Throw an exception
                        throw new System.Exception("ObTyp " + CurrentObTyp.SYSOBTYPP + " not found.");
                    }

                    // Decrease the current level number
                    CurrentLevel--;
                }
                if (CurrentLevel == -99) continue;

                // Get a parent key string
                string ParentKey = treeNode == null ? null : treeNode.Key;

                // Check if parent key string is not empty
                if (!string.IsNullOrEmpty(ParentKey))
                {
                    // Create ObTypKey for parent
                    ObTypKey ParentObTypKey = new ObTypKey(ParentKey);

                    // If the node is not a child of treeNode node
                    if (ParentObTypKey.LastValidId != CurrentObTyp.SYSOBTYPP)
                    {
                        // Ignore the node
                        continue;
                    }
                }

                // Create an ObTypKey for the node
                ObTypKey ObTypKey = new ObTypKey(ParentKey, CurrentObTyp.SYSOBTYP);

                // Create a node
                List<TreeNode> nodes = new List<TreeNode>();
                TreeNode Node = new TreeNode();
                Node.DisplayName = CurrentObTyp.BEZEICHNUNG;
                if (CurrentObTyp.SYSOBTYPP == null)
                {
                    if (Node.DisplayName.ToLower().IndexOf("car") > -1)
                        Node.DisplayName = getTranslation(setting.SelectedLanguage, CnstCarsText);
                    else if (Node.DisplayName.ToLower().IndexOf("cv") > -1)
                        Node.DisplayName = getTranslation(setting.SelectedLanguage, CnstCommercialVehiclesText);
                    else if (Node.DisplayName.ToLower().IndexOf("mot") > -1)
                        Node.DisplayName = getTranslation(setting.SelectedLanguage, CnstMotorcyclesText);
                }
                
                Node.Level = new Level(WantedLevel, MyGetLevelText(WantedLevel,setting.SelectedLanguage));
                Node.Key = ObTypKey.ToString();
                Node.ParentKey = ParentKey;
                Node.IsType = WantedLevel == CnstMaxLevel;
                Node.Price = !Node.IsType ? 0 : MyGetPrice(CurrentObTyp,Ust);
                Node.NewPrice = Node.Price;
                nodes.Add(Node);
                if (setting.hasFilter() && WantedLevel == 1)
                {
                    nodes = filterNodes(setting, nodes);
                }
                if (nodes.Count > 0)
                {
                    Node = nodes[0];
                    // Check if the node is not already on the list
                    if (!TreeNodes.Exists(N => N.Key == Node.Key))
                    {
                        TreeNodes.Add(Node);
                    }
                }
            }
           
            // Return the nodes
            return TreeNodes.ToArray();
        }

        public static TreeNode GetPreviousLevel(TreeNode treeNode, DdOlExtended context, String language)
        {
            // Check if tree node is valid
            if (treeNode == null)
            {
                throw new ArgumentNullException("treeNode", "Tree node is null.");
            }

            if (treeNode.Level == null || treeNode.Level.Number < 1)
            {
                throw new System.Exception("Tree node level is invalid.");
            }

            // Create ObTypKey
            ObTypKey ObTypKey = new ObTypKey(treeNode.Key);
            ObTypKey ParentObTypKey = ObTypKey.Parent;

            // Query OBTYP
            var CurrentObTyp = (from ObTyp in context.OBTYP
                                where ObTyp.SYSOBTYP == ParentObTypKey.LastValidId
                                select ObTyp).FirstOrDefault();

            // Create a node
            TreeNode Node = new TreeNode();
            if(treeNode.Level.Number==1)
                Node.DisplayName =getTranslation(language,CnstCarsText);
            else
                 Node.DisplayName = CurrentObTyp.BEZEICHNUNG;
            Node.Level = new Level(treeNode.Level.Number - 1, MyGetLevelText(treeNode.Level.Number - 1, language));
            Node.Key = ParentObTypKey.ToString();
            Node.ParentKey = ParentObTypKey.Parent == null ? null : ParentObTypKey.Parent.ToString();
            Node.IsType = false;
            Node.Price = 0;
            Node.NewPrice = 0;

            // Return the node
            return Node;
        }

        public static TreeNodeDetail[] GetTechnicalDetails(TreeNode treeNode, DdOlExtended context, String language)
        {
            // Check if node is valid
            if (treeNode == null)
            {
                // Throw an exception
                throw new ArgumentNullException("treeNode", "Tree node is null.");
            }

            // Create a key
            ObTypKey Key = new ObTypKey(treeNode.Key);

            // Query OBTYP
            var CurrentObTyp = (from ObTyp in context.OBTYP
                                where ObTyp.SYSOBTYP == Key.TypeId
                                select ObTyp).FirstOrDefault();

            // Check if anything was found
            if (CurrentObTyp == null)
            {
                // Throw an exception
                throw new System.Exception("ObTyp " + Key.TypeId + " was not found.");
            }

            // Check if FZTYP reference is loaded
            if (!CurrentObTyp.FZTYPReference.IsLoaded)
            {
                CurrentObTyp.FZTYPReference.Load();
            }

            if (CurrentObTyp.FZTYP == null)
            {
                return new TreeNodeDetail[0];
            }

            // Create detail list
            List<TreeNodeDetail> Details = new List<TreeNodeDetail>();

            TreeNodeDetail Detail;
            TreeNodeDetailTypeConstants Category = TreeNodeDetailTypeConstants.Technic;
            TreeNodeDetailValueTypeConstants DetailType;


            DetailType = TreeNodeDetailValueTypeConstants.TecParticlesEmission;
            Detail = new TreeNodeDetail(Category, DetailType, getTranslation(language,Ecology),getTranslation(language,Particles), CurrentObTyp.FZTYP.PARTIKEL.GetValueOrDefault().ToString());
            Details.Add(Detail);

            DetailType = TreeNodeDetailValueTypeConstants.TecCO2EmissionCombined;
            Detail = new TreeNodeDetail(Category, DetailType, getTranslation(language,Ecology),getTranslation(language,CO2emission), CurrentObTyp.FZTYP.CO2EMI.GetValueOrDefault().ToString());
            Details.Add(Detail);

            DetailType = TreeNodeDetailValueTypeConstants.TecNoxEmission;
            Detail = new TreeNodeDetail(Category, DetailType, getTranslation(language,Ecology),getTranslation(language,NOX), CurrentObTyp.FZTYP.NOX.GetValueOrDefault().ToString());
            Details.Add(Detail);

            DetailType = TreeNodeDetailValueTypeConstants.TecEnginePerformancekW;
            
            long kw =  CurrentObTyp.FZTYP.LEISTUNG.GetValueOrDefault();
            long ps = (long)Math.Round(kw * 1.36);
            Detail = new TreeNodeDetail(Category, DetailType, getTranslation(language,General),getTranslation(language,Power),kw+" kW / "+ ps+" PS");
            Details.Add(Detail);

            DetailType = TreeNodeDetailValueTypeConstants.TecConsumptionCombined;
            Detail = new TreeNodeDetail(Category, DetailType, getTranslation(language,General),getTranslation(language,Consumption), CurrentObTyp.FZTYP.VERBRAUCH.GetValueOrDefault().ToString());
            Details.Add(Detail);

            // Return the details
            return Details.ToArray();
        }

        public static TreeNode[] GetPreviousLevels(TreeNode treeNode, DdOlExtended context, String language)
        {
            // Check if the node is valid
            if (treeNode == null)
            {
                throw new ArgumentNullException("treeNode", "Tree node is null.");
            }

            // Create a nodes list
            List<TreeNode> Nodes = new List<TreeNode>();

            // If this is not the first level
            while (treeNode.Level.Number > 0)
            {
                // Get the previous level
                treeNode = GetPreviousLevel(treeNode, context, language);

                // Add the node to the list if it's not null
                if (treeNode != null)
                {
                    Nodes.Add(treeNode);
                }
            }

            // Return the list
            return Nodes.ToArray();
        }

        public static TreeNode[] SearchTreeNodes(FilterParam[] filter, Search search, DdOlExtended context, Setting setting)
        {
            if (search != null && search.SimpleSearch)
            {
                return SimpleSearchTreeNodes(filter, search.SimpleSearchParam, context, setting);
            }

            int HighestLevel = MyGetHighestLevel(filter, search.SearchParams);

            // Filter the nodes
            TreeNode[] FilteredNodes = GetFilteredNodes(filter, HighestLevel, context, setting);

            List<TreeNode> ResultNodes = new List<TreeNode>();

            // Check if search parameters are valid
            if (search != null && search.SearchParams != null && search.SearchParams.Length > 0)
            {
                // Iterate through all filtered nodes
                foreach (var LoopNode in FilteredNodes)
                {
                    // Create a key
                    ObTypKey ObTypKey = new ObTypKey(LoopNode.Key);

                    // Assume the node does not match
                    bool Matches = true;

                    // Iterate through all search params
                    foreach (var LoopSearchParam in search.SearchParams)
                    {
                        long Id = 0;

                        // Get appropriate OBTYP id
                        switch (LoopSearchParam.SearchAtLevel.Number)
                        {
                            case 0:
                                Id = ObTypKey.VehicleTypeId;
                                break;

                            case 1:
                                Id = ObTypKey.BrandId;
                                break;

                            case 2:
                                Id = ObTypKey.ModelGroupId;
                                break;

                            case 3:
                                Id = ObTypKey.ModelId;
                                break;

                            case 4:
                                Id = ObTypKey.TypeId;
                                break;
                        }

                        var CurrentObTyp = (from ObTyp in context.OBTYP
                                            where ObTyp.SYSOBTYP == Id
                                            select ObTyp).FirstOrDefault();

                        if (CurrentObTyp == null)
                        {
                            throw new System.Exception("ObTyp " + Id + " was not found.");
                        }

                        if (!Regex.IsMatch(CurrentObTyp.BEZEICHNUNG, LoopSearchParam.Pattern, RegexOptions.IgnoreCase|RegexOptions.Compiled))
                        {
                            Matches = false;
                            break;
                        }
                    }

                    if (Matches)
                    {
                        if (setting.hasFilter())
                        {
                             var CurrentObTyp = (from ObTyp in context.OBTYP
                                                 where ObTyp.SYSOBTYP == ObTypKey.BrandId
                                            select ObTyp).FirstOrDefault();
                             if (!setting.filter(CurrentObTyp.BEZEICHNUNG))
                                 ResultNodes.Add(LoopNode);
                        }
                        else
                            ResultNodes.Add(LoopNode);
                    }
                }


               /* if (setting.hasFilter())
                {
                    ResultNodes = filterNodes(setting, ResultNodes);
                }*/
                return ResultNodes.ToArray();
            }
            else
            {
               /* if (setting.hasFilter())
                {
                    FilteredNodes = filterNodes(setting, FilteredNodes);
                }*/
                return FilteredNodes;
            }
        }

        public static TreeNode[] GetFilteredNodes(FilterParam[] filter, int filterAtLevel, DdOlExtended context, Setting setting)
        {
            var predicateMain = PredicateBuilder.True<OBTYP>();
            predicateMain = predicateMain.And(e => e.NOEXTID == CnstNoExtId);

            var predicate = PredicateBuilder.True<OBTYP>();
            bool hasPredicate = false;
            if(filter!=null)
            {
                foreach(FilterParam f in filter)
                {
                     if (filterAtLevel != f.FilterAtLevel.Number) continue;
                     String fname = f.Filter.ToLower();
                     predicate = predicate.And(e => e.BEZEICHNUNG.ToLower().Contains(fname));
                     hasPredicate = true;

                }
            }


            var predicate2 = PredicateBuilder.False<OBTYP>();
            predicate2 = predicate2.Or(e => e.IMPORTSOURCE == CnstImportSource1);
            predicate2 = predicate2.Or(e => e.IMPORTSOURCE == CnstImportSource2);
            predicate2 = predicate2.Or(e => e.IMPORTSOURCE == null);

            if (filter != null && hasPredicate)
                predicateMain = predicateMain.And(predicate.Expand());
            predicateMain = predicateMain.And(predicate2.Expand());

            var ObTyps = context.OBTYP.AsExpandable().Where(predicateMain.Expand());

            List<TreeNode> Nodes = new List<TreeNode>();

            // Query OBTYP
           /* var ObTyps = from ObTyp in context.OBTYP
                         where ObTyp.NOEXTID == CnstNoExtId
                         && (ObTyp.IMPORTSOURCE == CnstImportSource1 || ObTyp.IMPORTSOURCE == CnstImportSource2 || ObTyp.IMPORTSOURCE==null)
                         
                         select ObTyp;*/

            decimal Ust = getGlobalUst(setting.customerCode,setting.sysperole);
            bool hasError = false;
            // Iterate through all last level ObTyps
            foreach (var LoopObTyp in ObTyps)
            {
                var CurrentObTyp = LoopObTyp;
                int CurrentLevel = CnstMaxLevel - 1;
                hasError = false;

                KeyValuePair<long, string>[] NodePath = new KeyValuePair<long, string>[CnstMaxLevel + 1];
                NodePath[CnstMaxLevel] = new KeyValuePair<long, string>(LoopObTyp.SYSOBTYP, LoopObTyp.BEZEICHNUNG);
                String schwacke = "";
                String brand = "";
                // Iterate while the level is greater than 0
                while (CurrentLevel >= 0)
                {
                    // Check if ObTyp has parent
                    if (CurrentObTyp.SYSOBTYPP == null)
                    {
                        // Throw an exception
                       // throw new System.Exception("ObTyp +"+CurrentObTyp.SYSOBTYP+"/"+CurrentObTyp.BEZEICHNUNG+" at level " + CurrentLevel + " has no parent!");
                        hasError = true;
                        break;
                    }

                    // Query OBTYP
                    CurrentObTyp = (from ObTyp in context.OBTYP
                                    where ObTyp.SYSOBTYP == CurrentObTyp.SYSOBTYPP
                                    select ObTyp).FirstOrDefault();

                    // Check if ObTyp is found
                    if (CurrentObTyp == null)
                    {
                        // Throw an exception
                        throw new System.Exception("ObTyp " + CurrentObTyp.SYSOBTYPP + " not found.");
                    }

                    NodePath[CurrentLevel] = new KeyValuePair<long, string>(CurrentObTyp.SYSOBTYP, CurrentObTyp.BEZEICHNUNG);
                    if (CurrentLevel == 4)
                        schwacke = CurrentObTyp.SCHWACKE;
                    if (CurrentLevel == 1)
                        brand = CurrentObTyp.BEZEICHNUNG;
                    // Decrease the current level number
                    CurrentLevel--;
                }
                if (hasError) continue;

                bool PathFilteringPassed = true;

                // Check if the filter is valid
                if (filter != null && filter.Length != 0)
                {

                    // Iterate through all levels
                    for (int LevelNumber = 0; LevelNumber <= filterAtLevel; LevelNumber++)
                    {
                        // Get filters for current level
                        var FiltersForCurrentLevel = from Filter in filter
                                                     where Filter.FilterAtLevel.Number == LevelNumber
                                                     select Filter;

                        bool LevelFilteringPassed = false;

                        // Iterate through all filters
                        foreach (var LoopFilter in FiltersForCurrentLevel)
                        {
                            if (Regex.IsMatch(NodePath[LevelNumber].Value, LoopFilter.Filter, RegexOptions.IgnoreCase | RegexOptions.Compiled))
                            {
                                LevelFilteringPassed = true;
                                break;
                            }
                        }

                        if (!LevelFilteringPassed && FiltersForCurrentLevel.Count() != 0)
                        {
                            PathFilteringPassed = false;
                            break;
                        }
                    }
                }

                if (PathFilteringPassed)
                {
                    long[] Ids = new long[CnstMaxLevel + 1];

                    for (int NodePathIndex = 0; NodePathIndex < NodePath.Length; NodePathIndex++)
                    {
                        if (NodePathIndex <= filterAtLevel)
                        {
                            Ids[NodePathIndex] = NodePath[NodePathIndex].Key;
                        }
                        else
                        {
                            Ids[NodePathIndex] = 0;
                        }
                    }

                    ObTypKey ObTypKey = new ObTypKey(Ids[0], Ids[1], Ids[2], Ids[3], Ids[4]);

                    TreeNode Node = new TreeNode();
                    Node.DisplayName = NodePath[filterAtLevel].Value;
                    Node.Key = ObTypKey.ToString();
                    Node.ParentKey = ObTypKey.Parent == null ? null : ObTypKey.Parent.ToString();
                    Node.Level = new Level(filterAtLevel, MyGetLevelText(filterAtLevel, setting.SelectedLanguage));
                    Node.IsType = filterAtLevel == CnstMaxLevel;
                    Node.Price = !Node.IsType ? 0 : MyGetPrice(LoopObTyp, Ust);
                    Node.NewPrice = Node.Price;
                    //Node.SCHWACKE = schwacke;
                    Node.CODE = LoopObTyp.SCHWACKE;
                    if (Node.CODE == null)
                        Node.CODE = "";
                    
                    if (!Nodes.Exists(N => N.Key == Node.Key))
                    {
                        if (setting.hasFilter())
                        {
                            if (!setting.filter(schwacke + brand+Node.DisplayName))
                                Nodes.Add(Node);
                        }
                        else
                            Nodes.Add(Node);
                    }
                }

            }

          /*  if (setting.hasFilter())
            {
                Nodes = filterNodes(setting, Nodes);
            }*/

            return Nodes.ToArray();
        }

        public static TreeNode[] SimpleSearchTreeNodes(FilterParam[] filter, SimpleSearchParam search, DdOlExtended context, Setting setting)
        {
            if (search == null)
            {
                throw new ArgumentNullException("search", "Search parameter is null.");
            }

            switch (search.SearchBy)
            {
                case SearchBy.Id:
                    TreeNode[] Nodes = GetFilteredNodes(filter, CnstMaxLevel, context, setting);

                   
                    //TreeNode[] rval = Nodes.Where(N => Regex.IsMatch(new ObTypKey(N.Key).TypeId.ToString(), search.SearchPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled)).ToArray();
                    TreeNode[] rval = Nodes.Where(N => Regex.IsMatch(N.CODE, search.SearchPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled)).ToArray();
                    
                    return rval;

                case SearchBy.Level0Name:
                    return MyGetSimpleSearchName(search.SearchPattern, 0, context, setting);

                case SearchBy.Level1Name:
                    return MyGetSimpleSearchName(search.SearchPattern, 1, context, setting);

                case SearchBy.Level2Name:
                    return MyGetSimpleSearchName(search.SearchPattern, 2, context, setting);

                case SearchBy.Level3Name:
                    return MyGetSimpleSearchName(search.SearchPattern, 3, context, setting);
            }

            return new TreeNode[0];
        }
        #endregion

        #region My methods
        private static int MyGetHighestLevel(FilterParam[] filter)
        {
            if (filter == null)
            {
                return 0;
            }

            int HighestLevel = 0;

            foreach (var LoopFilter in filter)
            {
                if (LoopFilter == null || LoopFilter.FilterAtLevel == null)
                {
                    throw new System.Exception("Filtering level not specified.");
                }

                if (LoopFilter.FilterAtLevel.Number > HighestLevel)
                {
                    HighestLevel = LoopFilter.FilterAtLevel.Number;
                }
            }

            return HighestLevel;
        }

        private static int MyGetHighestLevel(SearchParam[] search)
        {
            if (search == null)
            {
                return 0;
            }

            int HighestLevel = 0;

            foreach (var LoopSearch in search)
            {
                if (LoopSearch == null || LoopSearch.SearchAtLevel == null)
                {
                    throw new System.Exception("Searching level not specified.");
                }

                if (LoopSearch.SearchAtLevel.Number > HighestLevel)
                {
                    HighestLevel = LoopSearch.SearchAtLevel.Number;
                }
            }

            return HighestLevel;
        }

        private static int MyGetHighestLevel(FilterParam[] filter, SearchParam[] search)
        {
            int HighestFilterLevel = MyGetHighestLevel(filter);
            int HighestSearchLevel = MyGetHighestLevel(search);

            return HighestFilterLevel > HighestSearchLevel ? HighestFilterLevel : HighestSearchLevel;
        }

        private static string MyGetLevelText(int LevelNumber, string language)
        {
            switch (LevelNumber)
            {
                case 0:
                    return getTranslation(language,CnstSelectVehicleTypeText);

                case 1:
                    return getTranslation(language,CnstSelectBrandText);

                case 2:
                    return getTranslation(language,CnstSelectModelGroupText);

                case 3:
                    return getTranslation(language,CnstSelectModelText);

                case 4:
                    return getTranslation(language,CnstSelectTypeText);

                default:
                    return string.Empty;
            }
        }

        private static double MyGetPrice(OBTYP obTyp, decimal Ust)
        {
            // Check if ObTyp is valid
            if (obTyp == null)
            {
                // Throw an exception
                throw new ArgumentNullException("obTyp", "ObTyp is null.");
            }

            // Check if FZTYP reference is loaded
            if (!obTyp.FZTYPReference.IsLoaded)
            {
                // Load the FZTYP reference
                obTyp.FZTYPReference.Load();
            }

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
            rval = System.Math.Round(rval, 2);

            //needs brutto incl. nova!
            return (double)rval;
        }
        private static CacheDictionary<String, decimal> ustCache = CacheFactory<String, decimal>.getInstance().createCache(1000*60*1, CacheCategory.Data);

        /// <summary>
        /// Returns the global Ust Value of MWST-Table, defined by the code in the Configsection AIDA/GENERAL/USTCODE
        /// </summary>
        /// <returns></returns>
        private static decimal getGlobalUst(int customerCode, long sysperole)
        {
            String cc = customerCode + "_" + sysperole;
            if (!ustCache.ContainsKey(cc))
            {
                decimal rval = 0;
                using (DdOlExtended context = new DdOlExtended())
                {

                    if (customerCode == 100)//BMW
                    {

                        string code = AppConfig.Instance.GetEntry("GENERAL", "USTCODE", "USt 20%", "AIDA");
                        var q = from m in context.MWST
                                where m.CODE == code
                                select m.PROZENT;
                        decimal? mwst = q.FirstOrDefault();

                        rval= mwst.HasValue ? mwst.Value : 0;

                    }
                    else if (customerCode == 200)
                    {
                        //rval = 7.6M;//BNOW
                        rval = context.ExecuteStoreQuery<decimal>("select prozent from mwst,lsadd where mwst.sysmwst=lsadd.sysmwst and lsadd.mandant='BANK-now'", null).FirstOrDefault();
                    }
                    else //Default , also 300 HCE
                    {
                        String mandant = context.ExecuteStoreQuery<String>("select lsadd.mandant from perole, person,lsadd where  person.sysperson=perole.sysperson and lsadd.sysperson=person.sysperson and sysparent is null and "
                                + SQLDateUtil.CheckCurrentSysDate ("perole")
                                + " and perole.sysperole in (select sysperole from perole connect by PRIOR sysparent = sysperole start with sysperole="+sysperole+")", null).FirstOrDefault();
                        rval = context.ExecuteStoreQuery<decimal>("select prozent from mwst,lsadd where mwst.sysmwst=lsadd.sysmwst and lsadd.mandant='" + mandant + "'", null).FirstOrDefault();
                    }


                }
                ustCache[cc] = rval;
            }
            return ustCache[cc];
        }

        private static TreeNode[] MyGetSimpleSearchName(string name, int levelNumber, DdOlExtended context, Setting setting)
        {
            List<TreeNode> Nodes = new List<TreeNode>();
            MySearchForTreeNodes(name, Nodes, null, levelNumber, context, setting);
            return Nodes.ToArray();

            // Another algorithm:
            /*List<TreeNode> Nodes = new List<TreeNode>();

            var ObTyps = from ObTyp in context.OBTYP
                         where ObTyp.BESCHREIBUNG == name
                         select ObTyp;

            foreach(var LoopObTyp in ObTyps)
            {
                int CurrentLevel = 0;

                List<long> Ids = new List<long>();
                Ids.Add(LoopObTyp.SYSOBTYP);

                var ParentObTyp = LoopObTyp;
                while (ParentObTyp != null && ParentObTyp.SYSOBTYPP != null)
                {
                    ParentObTyp = (from ObTyp in context.OBTYP
                                   where ObTyp.SYSOBTYP == ParentObTyp.SYSOBTYPP
                                   select ObTyp).FirstOrDefault();

                    CurrentLevel++;
                    Ids.Insert(0, ParentObTyp.SYSOBTYP);
                }

                if (CurrentLevel != levelNumber)
                {
                    continue;
                }

                if (MyCheckObTyp(LoopObTyp.SYSOBTYP, context))
                {
                    while (Ids.Count < 5)
                    {
                        Ids.Add(0);
                    }

                    ObTypKey Key = new ObTypKey(Ids[0], Ids[1], Ids[2], Ids[3], Ids[4]);

                    TreeNode Node = new TreeNode();
                    Node.DisplayName = LoopObTyp.BESCHREIBUNG;
                    Node.HasDetails = false;
                    Node.HasPictures = false;
                    Node.IsType = levelNumber == CnstMaxLevel;
                    Node.Key = Key.ToString();
                    Node.ParentKey = Key.Parent.ToString();
                    Node.Level = new Level(levelNumber, MyGetLevelText(levelNumber));
                    Node.NewPrice = MyGetPrice(LoopObTyp);
                    Node.Price = MyGetPrice(LoopObTyp);

                    Nodes.Add(Node);
                }
            }

            return Nodes.ToArray();*/
        }

       /* private static bool MyCheckObTyp(long SysObTyp, DdOlExtended context)
        {
            var ObTyps = from ObTyp in context.OBTYP
                         where ObTyp.SYSOBTYPP == SysObTyp
                         select ObTyp;

            foreach (var LoopObTyp in ObTyps)
            {
                if (LoopObTyp.NOEXTID == CnstNoExtId && (LoopObTyp.IMPORTSOURCE == CnstImportSource1 || LoopObTyp.IMPORTSOURCE == CnstImportSource2))
                {
                    return true;
                }
                else
                {
                    bool Found = MyCheckObTyp(LoopObTyp.SYSOBTYP, context);

                    if (Found)
                    {
                        return true;
                    }
                }
            }

            return false;
        }*/

        private static void MySearchForTreeNodes(string name, List<TreeNode> nodes, TreeNode treeNode, int levelNumber, DdOlExtended context, Setting setting)
        {
            TreeNode[] Nodes = GetNextLevel(treeNode, context, setting);

            foreach (var LoopNode in Nodes)
            {
                if (LoopNode.Level.Number == levelNumber && LoopNode.DisplayName == name)
                {
                    nodes.Add(LoopNode);
                }
                else if (LoopNode.Level.Number < levelNumber)
                {
                    MySearchForTreeNodes(name, nodes, LoopNode, levelNumber, context, setting);
                }
            }
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
        #endregion
    }
   
 
}
