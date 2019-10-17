using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.P000001.Common.DataProvider;
using Cic.P000001.Common;
using System.Reflection;
using Cic.OpenOne.Common.Model.DdEurotax;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.EntityClient;
using System.IO;

using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;

using Cic.OpenOne.Common.Util.Logging;
namespace Cic.OpenOne.CarConfigurator.BO.EurotaxDataProvider
{

 	[System.CLSCompliant(true)]
    public class EurotaxDataProvider : Cic.P000001.Common.DataProvider.IAdapter
    {
        #region Private constants
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        bool CnstUseRelations = true;

        #endregion

        #region IAdapter methods

        public Cic.P000001.Common.AdapterState DeliverAdapterState()
		{
            // Assume the database works
            bool ServiceAvailable = true;
            String Message = null;


            try
            {
                // Try to establish a connection
                using (Cic.OpenOne.Common.Model.DdEurotax.DdEurotaxExtended context = new DdEurotaxExtended())
                {
                    var Query = from type in context.ETGTYPE
                                select type;
                    Query.Count();
                }
            }
            catch (Exception e)
            {
                // Database is not available
                ServiceAvailable = false;
                Message = e.Message;
                if (e.InnerException != null)
                    Message += Environment.NewLine + e.InnerException.Message;
            }

            // Return state information
            return new AdapterState("Cic.P000001.EurotaxDataProvider", ServiceAvailable, Message);
		}

		public Cic.P000001.Common.DataSourceInformation DeliverDataSourceInformation()
		{
            DataSourceInformation Result = null;

            try
            {
                // Make a connection
                using (Cic.OpenOne.Common.Model.DdEurotax.DdEurotaxExtended context = new DdEurotaxExtended())
                {

                    // Fill DataSourceInformation
                    Result = new DataSourceInformation();
                    Result.Identifier = new Guid("{8ce9f870-101a-4ffb-9acf-e95974302dbb}");
                    Result.Version = "1.0";
                    Result.VersionDescription = "Cic.P000001.EurotaxDataProvider";
                    Result.Description = null;
                    Result.Designation = "Eurotax";
                    Result.Copyright = "Copyright © C.I.C. Software GmbH 2010";
                    Result.PriceInclusionConstant = PriceInclusionConstants.None;
                    Result.ExteriorPicturesSupported = true;
                    Result.InteriorPicturesSupported = true;
                    Result.StandardEquipmentDetailsSupported = true;
                    Result.TechnicalDataDetailsSupported = true;
                    Result.ValueAddedTaxInfo = null;
                    Result.SpecialCarTaxInfo = null;
                    Result.ShippingCostInfo = null;
                    Result.ImportDutyInfo = null;
                    Result.PriceInfo = null;
                    Result.AvailableCurrencies = EurotaxData.GetSupportedCurrencies(context);
                    Result.AvailableLanguages = EurotaxData.GetSupportedLanguages(context);
                }
            }
            catch (System.Exception Exception)
            {
                _log.Error("DeliverDataSourceInformationHelper Error", Exception);

             
            }
        

            // Return adapter information
            return Result;
		}

		public Cic.P000001.Common.TreeNode[] GetTreeNodes(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.GetTreeNodeSearchModeConstants getTreeNodeSearchModeConstant)
		{
            try
            {
                // Make a connection
                using (Cic.OpenOne.Common.Model.DdEurotax.DdEurotaxExtended context = new DdEurotaxExtended())
                {

                    // Get nodes if specified search mode is available
                    switch (getTreeNodeSearchModeConstant)
                    {
                        // Get the next level
                        default:
                        case GetTreeNodeSearchModeConstants.NextLevel:
                            return EurotaxData.GetNextLevel(context, setting, treeNode);

                        // Get next levels - not implemented
                        case GetTreeNodeSearchModeConstants.NextLevels:
                            throw new NotImplementedException("'NextLevels' search mode is not available.");

                        // Get the previous level
                        case GetTreeNodeSearchModeConstants.PreviousLevel:
                            return new TreeNode[] { EurotaxData.GetPreviousLevel(context, setting, treeNode) };

                        // Get all previous levels
                        case GetTreeNodeSearchModeConstants.PreviousLevels:
                            return EurotaxData.GetPreviousLevels(context, setting, treeNode);
                    }
                }
            }
            catch (System.Exception Exception)
            {
                _log.Error("GetTreeNodes Error", Exception);

             
            }

            return null;
		}

        public Cic.P000001.Common.TreeNode[] SearchTreeNodes(Setting setting, FilterParam[] filter, Search search)
        {
            List<TreeNode> ResultNodes = null;

            try
            {
                // Make a connection
                using (Cic.OpenOne.Common.Model.DdEurotax.DdEurotaxExtended context = new DdEurotaxExtended())
                {

                    if (search != null && search.SimpleSearch)
                    {
                        return EurotaxData.SimpleSearch(context, setting, filter, search.SimpleSearchParam, null);
                    }
                    if (search != null && search.SearchParams != null && search.SearchParams.Length>0)
                    {
                        bool allLevel4 = true;
                        foreach (SearchParam sp in search.SearchParams)
                        {
                            if (sp.SearchAtLevel.Number != 4)
                                allLevel4 = false;
                        }
                      
                        if (allLevel4)
                        {
                            return EurotaxData.SearchModel(search.SearchParams, setting);
                           
                        }
                    }

                    // Find the highest level
                    int HighestLevel = 0;

                    if (search != null && search.SearchParams != null)
                    {
                        foreach (SearchParam LoopSearchParam in search.SearchParams)
                        {
                            // Check if search pattern is not valid
                            if (string.IsNullOrEmpty(LoopSearchParam.Pattern))
                            {
                                // Throw an exception 
                                throw new ApplicationException("Search pattern is not specified.");
                            }

                            // Check if level is not valid
                            if (LoopSearchParam.SearchAtLevel == null)
                            {
                                // Throw an exception
                                throw new ApplicationException("Search level is not specified.");
                            }

                            if (LoopSearchParam.SearchAtLevel.Number > HighestLevel)
                            {
                                HighestLevel = LoopSearchParam.SearchAtLevel.Number;
                            }
                        }
                    }

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

                    // Create the result list
                    ResultNodes = new List<TreeNode>();

                    // Get the nodes
                    TreeNode[] FilteredNodes = EurotaxData.GetFilteredNodes(context, setting, null, filter, search == null ? null : search.SearchParams, HighestLevel);
                    ResultNodes.AddRange(FilteredNodes);

                    // Return the results
                    return ResultNodes.ToArray();
                }
            }
            catch (System.Exception Exception)
            {
                _log.Error("SearchTreeNodes Error", Exception);

              
            }

            return null;
        }

		public Cic.P000001.Common.DataProvider.TreeInfo GetTreeInfo(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode)
		{
            // If treeNode is null
            if (treeNode == null)
            {
                // Create a level list
                List<Level> Levels = new List<Level>();
                Levels.Add(new Level(0, "Select vehicle type"));
                Levels.Add(new Level(1, "Select brand"));
                Levels.Add(new Level(2, "Select model group"));
                Levels.Add(new Level(3, "Select model"));
                Levels.Add(new Level(4, "Select type"));

                // Return tree info
                return new TreeInfo(true, 5, 0, null, null, null, Levels.ToArray());
            }

            // Throw an excption
            throw new ApplicationException("TreeNode in GetTreeInfo() has to be null.");
		}

        public Cic.P000001.Common.TreeNodeDetail[] GetTreeNodeDetails(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.TreeNodeDetailTypeConstants treeNodeDetailTypeConstant)
        {
            TreeNodeDetail[] Details = null;
            try
            {
                // Make a connection
                using (Cic.OpenOne.Common.Model.DdEurotax.DdEurotaxExtended context = new DdEurotaxExtended())
                {

                    // Create a key
                    EurotaxKey Key = new EurotaxKey(setting, treeNode);



                    // Check what type of details is needed
                    switch (treeNodeDetailTypeConstant)
                    {
                        // Technical details
                        case TreeNodeDetailTypeConstants.Technic:
                            Details = EurotaxData.GetTypeTechnicalDetails(context, Key);
                            break;
                        // Standard equipment
                        case TreeNodeDetailTypeConstants.StandardEquipment:
                            Details = EurotaxData.GetStandardEquipment(context, Key);
                            break;
                        // Other type of details
                        default:
                            Details = new TreeNodeDetail[0];
                            break;
                    }


                }
            }
            catch (System.Exception Exception)
            {
                _log.Error("GetTreeNodeDetails Error", Exception);

              
            }


            // Return the array
            return Details;

        }

		public Cic.P000001.Common.Picture[] GetTreeNodePictures(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.PictureTypeConstants pictureTypeConstant, int top, bool withoutContent)
		{
            throw new NotImplementedException("GetTreeNodePictures() is not implemented.");
		}

        public Cic.P000001.Common.Component[] GetComponents(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.ComponentTypeConstants componentTypeConstant)
        {
            List<Cic.P000001.Common.Component> Components = null;
            try
            {
                // Make a connection
                using (Cic.OpenOne.Common.Model.DdEurotax.DdEurotaxExtended context = new DdEurotaxExtended())
                {


                    // Make a connection
                    EurotaxKey Key = new EurotaxKey(setting, treeNode);

                    // Create a component list
                    Components = new List<Component>();

                    // Check component type requested
                    switch (componentTypeConstant)
                    {
                        // Packages
                        case ComponentTypeConstants.Package:
                            Components.AddRange(EurotaxData.GetPackages(context, Key));
                            break;
                        // Equipment
                        case ComponentTypeConstants.Equipment:
                            Components.AddRange(EurotaxData.GetEquipment(context, Key));
                            break;
                        // Undefined - get all
                        case ComponentTypeConstants.Undefined:
                            Components.AddRange(EurotaxData.GetPackages(context, Key));
                            Components.AddRange(EurotaxData.GetEquipment(context, Key));
                            break;
                    }

                }
            }
            catch (System.Exception Exception)
            {
                _log.Error("GetComponents Error", Exception);

             
            }
            // Return components
            return Components.ToArray();
        }


        public Cic.P000001.Common.DataProvider.CheckComponentResult CheckComponent(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.Component component, Cic.P000001.Common.Component[] components)
        {

            // Create an expression array
            CheckComponentExpression[] Expressions = null;
            CheckComponentResultConstants ResultType = CheckComponentResultConstants.Valid;
            // Create component list
            List<Cic.P000001.Common.Component> Components = null;

            try
            {
                // Make a connection
                using (Cic.OpenOne.Common.Model.DdEurotax.DdEurotaxExtended context = new DdEurotaxExtended())
                {

                    // Create key
                    EurotaxKey Key = new EurotaxKey(setting, treeNode);

                    if (components == null)
                        Components = new List<Component>();
                    else
                        Components = components.ToList();


                    // If the component is already added
                    if (EurotaxData.ContainsComponent(component, Components.ToArray()))
                    {
                        // Create an expression array
                        if (CnstUseRelations)
                            Expressions = EurotaxData.GetDeletionLimitations(context, Key, component, components);
                        else
                            Expressions = new CheckComponentExpression[0];

                        // If the component can be deleted
                        if (Expressions.Length == 0)
                        {
                            // Delete component
                            Components.Remove(component);
                        }

                        ResultType = CheckComponentResultConstants.ComponentIsStillSelected;

                    }
                    // If the component is not on the list
                    else
                    {
                        // Get relation expressions for the component
                        if (CnstUseRelations)
                            Expressions = EurotaxData.GetComponentRelations(context, Key, component, Components.ToArray());
                        else
                            Expressions = new CheckComponentExpression[0];

                        // If there are no expressions, add the component
                        if (Expressions.Length == 0)
                            Components.Add(component);
                        // If there are some, return them
                        else
                            ResultType = CheckComponentResultConstants.Relations;
                    }
                }
            }
            catch (System.Exception Exception)
            {
                _log.Error("CheckComponent Error", Exception);

               
            }

            // Return the result
            return new CheckComponentResult(ResultType, Expressions, "");
        }


        public Cic.P000001.Common.ComponentDetail[] GetComponentDetails(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.Component component, Cic.P000001.Common.ComponentDetailTypeConstants componentDetailTypeConstant)
        {
            ComponentDetail[] ComponentDetails = null;
            try
            {
                // Make a connection
                using (Cic.OpenOne.Common.Model.DdEurotax.DdEurotaxExtended context = new DdEurotaxExtended())
                {

                    // Create a key
                    EurotaxKey Key = new EurotaxKey(setting, treeNode);

                    // Get Addition ID
                    long AdditionId = int.Parse(component.Key);

                    // Query ETGADDITION
                    var Additions = from Addition in context.ETGADDITION
                                    where Addition.MARKET == Key.Market
                                    && Addition.VEHTYPE == Key.VehicleType
                                    && Addition.ID == AdditionId
                                    select Addition.EQCODE;

                    // If there is no such a component, return empty detail array
                    if (Additions.Count() == 0)
                        return new Cic.P000001.Common.ComponentDetail[0];

                    // Get Equipment Code
                    long EqCode = (long)Additions.FirstOrDefault();
                    ComponentDetails = EurotaxData.GetPackageDetails(context, Key, EqCode);
                }
            }
            catch (System.Exception Exception)
            {
                _log.Error("GetComponentDetails Error", Exception);

              
            }

            // Get package contents and return it
            return ComponentDetails;
        }


		public Cic.P000001.Common.Picture[] GetComponentPictures(Cic.P000001.Common.Setting setting, Cic.P000001.Common.Component component, Cic.P000001.Common.PictureTypeConstants pictureTypeConstant, int top, bool withoutContent)
		{
            throw new NotImplementedException("GetComponentPictures() is not implemented.");
        }

        #endregion
    }
}
