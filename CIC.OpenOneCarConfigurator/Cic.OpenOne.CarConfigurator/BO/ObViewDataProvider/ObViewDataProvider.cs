using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.P000001.Common.DataProvider;
using Cic.P000001.Common;
using System.Reflection;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.EntityClient;
using System.IO;

using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;

using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Model.DdOl;
namespace Cic.OpenOne.CarConfigurator.BO.ObViewDataProvider
{

    [System.CLSCompliant(true)]
    public class ObViewDataProvider : Cic.P000001.Common.DataProvider.IAdapter
    {
        #region Private constants
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //bool CnstUseRelations = false;
        private IObViewDao dao;
        #endregion

        #region IAdapter methods

        public ObViewDataProvider()
        {
            this.dao = new ObViewDao();
        }
        public Cic.P000001.Common.AdapterState DeliverAdapterState()
        {
            // Assume the database works
            bool ServiceAvailable = true;
            String Message = null;
            try
            {
                // Try to establish a connection
                using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended ctx = new DdOlExtended())
                {
                    var Query = from type in ctx.OBART
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
            return new AdapterState("ObViewDataProvider", ServiceAvailable, Message);
        }

        public Cic.P000001.Common.DataSourceInformation DeliverDataSourceInformation()
        {
            DataSourceInformation Result = null;

            try
            {
                // Make a connection
                using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended ctx = new DdOlExtended())
                {

                    // Fill DataSourceInformation
                    Result = new DataSourceInformation();
                    Result.Identifier = new Guid("{8ce9f870-101a-4ffb-9acf-e95974302dbb}");
                    Result.Version = "1.0";
                    Result.VersionDescription = "ObViewDataProvider";
                    Result.Description = null;
                    Result.Designation = "OB-View";
                    Result.Copyright = "Copyright © C.I.C. Software GmbH 2011";
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
                    //TODO:
                    Result.AvailableCurrencies = new string[] { "CHF","EUR","EUR" };
                    Result.AvailableLanguages = new string[] { "de-CH","de-AT","de-DE" };

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
                    // Get nodes if specified search mode is available
                    switch (getTreeNodeSearchModeConstant)
                    {
                        // Get the next level
                        default:
                        case GetTreeNodeSearchModeConstants.NextLevel:
                            return dao.getNextLevel(setting, treeNode,false);

                       
                        case GetTreeNodeSearchModeConstants.NextLevels:
                            return dao.getNextLevel(setting, treeNode, true);

                        // Get the previous level
                        case GetTreeNodeSearchModeConstants.PreviousLevel:
                            return dao.getPreviousLevel(setting, treeNode, false);

                        // Get all previous levels
                        case GetTreeNodeSearchModeConstants.PreviousLevels:
                            return dao.getPreviousLevel(setting, treeNode, true);
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
            throw new NotImplementedException("SearchTreeNodes() is not implemented.");
        }

        public Cic.P000001.Common.DataProvider.TreeInfo GetTreeInfo(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode)
        {
            // If treeNode is null
            if (treeNode == null)
            {
                // Create a level list
                List<Level> Levels = new List<Level>();
                Levels.Add(new Level(0));
                Levels.Add(new Level(1));
                Levels.Add(new Level(2));
                Levels.Add(new Level(3));
                Levels.Add(new Level(4));

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


                // Check what type of details is needed
                switch (treeNodeDetailTypeConstant)
                {
                    // Technical details
                    case TreeNodeDetailTypeConstants.Technic:

                    // Standard equipment
                    case TreeNodeDetailTypeConstants.StandardEquipment:

                    // Other type of details
                    default:
                        Details = new TreeNodeDetail[0];
                        break;
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
            throw new NotImplementedException("GetComponents() is not implemented.");
        }


        public Cic.P000001.Common.DataProvider.CheckComponentResult CheckComponent(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.Component component, Cic.P000001.Common.Component[] components)
        {
            throw new NotImplementedException("CheckComponent() is not implemented.");

        }


        public Cic.P000001.Common.ComponentDetail[] GetComponentDetails(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.Component component, Cic.P000001.Common.ComponentDetailTypeConstants componentDetailTypeConstant)
        {
            throw new NotImplementedException("GetComponentDetails() is not implemented.");


        }


        public Cic.P000001.Common.Picture[] GetComponentPictures(Cic.P000001.Common.Setting setting, Cic.P000001.Common.Component component, Cic.P000001.Common.PictureTypeConstants pictureTypeConstant, int top, bool withoutContent)
        {
            throw new NotImplementedException("GetComponentPictures() is not implemented.");
        }

        #endregion
    }
}
