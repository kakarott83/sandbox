using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
namespace Cic.OpenOne.CarConfigurator.BO
{
    public class DataProviderUtilities
    {
        #region Methods


        public DataProviderUtilities()
        {

        }
        public Cic.P000001.Common.DataProvider.IAdapter getAdapter()
        {
            //TODO make configurable
            //return new Cic.OpenOne.CarConfigurator.BO.DataProviderService.EmbeddedDataProviderAdapterLoader<Cic.OpenOne.CarConfigurator.BO.MergeDataProvider.MergeDataProvider>().LoadAdapter();
            return new Cic.OpenOne.CarConfigurator.BO.DataProviderService.EmbeddedDataProviderAdapterLoader<Cic.OpenOne.CarConfigurator.BO.ObViewDataProvider.ObViewDataProvider>().LoadAdapter();
        }

       
        public string DeliverServiceVersion()
        {
            bool PassThrough;
            string Version = string.Empty;

            try
            {
                // Execute web method helper
                Version = Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.DeliverServiceVersionWebMethodHelper.Execute();
            }
            catch (System.Exception ex)
            {
                // Get state
                PassThrough =  ExeConfigurationHelper.GetPassThroughExceptionsValue();
                // Check state
                if (PassThrough)
                {
                    // Throw exception
                    throw ex;
                }
                else
                {
                    // Throw exception
                    throw ex;
                }
            }

            // Return
            return Version;
        }

        // TODO BK 0 BK, Not tested
        public Cic.P000001.Common.ServiceState DeliverServiceState()
        {
            bool PassThrough;
            Cic.P000001.Common.ServiceState ServiceState;

            try
            {
                // Execute web method helper
                ServiceState =  DeliverServiceStateWebMethodHelper.Execute();
            }
            catch (System.Exception ex)
            {
                // Get state
                PassThrough =  ExeConfigurationHelper.GetPassThroughExceptionsValue();
                // Check state
                if (PassThrough)
                {
                    // Throw exception
                    throw ex;
                }
                else
                {
                    // Throw exception
                    throw ex;
                }
            }

            // Return
            return ServiceState;
        }

        // TODO BK 0 BK, Not tested
        public Cic.P000001.Common.DataSourceInformation DeliverDataSourceInformation()
        {
            bool PassThrough;
            Cic.P000001.Common.DataSourceInformation DataSourceInformation;

            try
            {
                // Execute web method helper
                DataSourceInformation =  DeliverDataSourceInformationWebMethodHelper.Execute();
            }
            catch (System.Exception ex)
            {
                // Get state
                PassThrough =  ExeConfigurationHelper.GetPassThroughExceptionsValue();
                // Check state
                if (PassThrough)
                {
                    // Throw exception
                    throw ex;
                }
                else
                {
                    // Throw exception
                    throw ex;
                }
            }

            // Return
            return DataSourceInformation;
        }

        // TODO BK 0 BK, Not tested
        public  TreeInfo GetTreeInfo(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode)
        {
            bool PassThrough;
             TreeInfo TreeInfo;

            try
            {
                // Check properties
                if (setting != null)
                {
                    setting.CheckProperties();
                }
                if (treeNode != null)
                {
                    treeNode.CheckProperties();
                }
                // Execute web method helper
                TreeInfo = new Cic.OpenOne.CarConfigurator.BO.DataProviderService.TreeInfo(getAdapter().GetTreeInfo(setting, treeNode));
            }
            catch (System.Exception ex)
            {
                // Get state
                PassThrough =  ExeConfigurationHelper.GetPassThroughExceptionsValue();
                // Check state
                if (PassThrough)
                {
                    // Throw exception
                    throw ex;
                }
                else
                {
                    // Throw exception
                    throw ex;
                }
            }

            // Return
            return TreeInfo;
        }

        // TODO BK 0 BK, Not tested
        public Cic.P000001.Common.TreeNode[] GetTreeNodes(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.GetTreeNodeSearchModeConstants getTreeNodeSearchModeConstant)
        {
            bool PassThrough;
            Cic.P000001.Common.TreeNode[] TreeNodes;

            try
            {
                // Check properties
                if (setting != null)
                {
                    setting.CheckProperties();
                }

              
                // Execute adapter method
                TreeNodes = getAdapter().GetTreeNodes(setting, treeNode, getTreeNodeSearchModeConstant);
               
            }
            catch (System.Exception ex)
            {
                // Get state
                PassThrough =  ExeConfigurationHelper.GetPassThroughExceptionsValue();
                // Check state
                if (PassThrough)
                {
                    // Throw exception
                    throw ex;
                }
                else
                {
                    // Throw exception
                    throw ex;
                }
            }

            // Return
            return TreeNodes;
        }

        // TODO BK 0 BK, Not tested
        public Cic.P000001.Common.TreeNode[] SearchTreeNodes(Cic.P000001.Common.Setting setting, Cic.P000001.Common.FilterParam[] filter, Cic.P000001.Common.Search search)
        {
            bool PassThrough;
            Cic.P000001.Common.TreeNode[] TreeNodes;

            try
            {
                // Check properties
                if (setting != null)
                {
                    setting.CheckProperties();
                }
                if (search != null)
                {
                    search.CheckProperties();
                }
                // Execute web method helper
                TreeNodes = getAdapter().SearchTreeNodes(setting, filter, search);
//                TreeNodes =  SearchTreeNodesWebMethodHelper.Execute(setting, filter, search);
            }
            catch (System.Exception ex)
            {
                // Get state
                PassThrough =  ExeConfigurationHelper.GetPassThroughExceptionsValue();
                // Check state
                if (PassThrough)
                {
                    // Throw exception
                    throw ex;
                }
                else
                {
                    // Throw exception
                    throw ex;
                }
            }

            // Return
            return TreeNodes;
        }

        // TODO BK 0 BK, Not tested
        public Cic.P000001.Common.TreeNodeDetail[] GetTreeNodeDetails(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.TreeNodeDetailTypeConstants treeNodeDetailTypeConstant)
        {
            bool PassThrough;
            Cic.P000001.Common.TreeNodeDetail[] TreeNodeDetails;

            try
            {
                // Check properties
                if (setting != null)
                {
                    setting.CheckProperties();
                }
                if (treeNode != null)
                {
                    treeNode.CheckProperties();
                }
                // Execute web method helper
                TreeNodeDetails = getAdapter().GetTreeNodeDetails(setting, treeNode, treeNodeDetailTypeConstant);
                //TreeNodeDetails =  GetTreeNodeDetailsWebMethodHelper.Execute(setting, treeNode, treeNodeDetailTypeConstant);
            }
            catch (System.Exception ex)
            {
                // Get state
                PassThrough =  ExeConfigurationHelper.GetPassThroughExceptionsValue();
                // Check state
                if (PassThrough)
                {
                    // Throw exception
                    throw ex;
                }
                else
                {
                    // Throw exception
                    throw ex;
                }
            }

            // Return
            return TreeNodeDetails;
        }

        // TODO BK 0 BK, Not tested
        public Cic.P000001.Common.Picture[] GetTreeNodePictures(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.PictureTypeConstants pictureTypeConstant, int top, bool withoutContent)
        {
            bool PassThrough;
            Cic.P000001.Common.Picture[] Pictures;

            try
            {
                // Check properties
                if (setting != null)
                {
                    setting.CheckProperties();
                }
                //if (treeNode != null)
                //{
                //    treeNode.CheckProperties();
                //}
                // Execute web method helper
                Pictures = getAdapter().GetTreeNodePictures(setting, treeNode, pictureTypeConstant, top, withoutContent);
                //Pictures =  GetTreeNodePicturesWebMethodHelper.Execute(setting, treeNode, pictureTypeConstant, top, withoutContent);
            }
            catch (System.Exception ex)
            {
                // Get state
                PassThrough =  ExeConfigurationHelper.GetPassThroughExceptionsValue();
                // Check state
                if (PassThrough)
                {
                    // Throw exception
                    throw ex;
                }
                else
                {
                    // Throw exception
                    throw ex;
                }
            }

            // Return
            return Pictures;
        }

        // TODO BK 0 BK, Not tested
        public Cic.P000001.Common.Component[] GetComponents(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.ComponentTypeConstants componentTypeConstant)
        {
            bool PassThrough;
            Cic.P000001.Common.Component[] Components;

            try
            {
                // Check properties
                if (setting != null)
                {
                    setting.CheckProperties();
                }
                //if (treeNode != null)
                //{
                //    treeNode.CheckProperties();
                //}
                // Execute web method helper
                Components = getAdapter().GetComponents(setting, treeNode, componentTypeConstant);
               // Components =  GetComponentsWebMethodHelper.Execute(setting, treeNode, componentTypeConstant);
            }
            catch (System.Exception ex)
            {
                // Get state
                PassThrough =  ExeConfigurationHelper.GetPassThroughExceptionsValue();
                // Check state
                if (PassThrough)
                {
                    // Throw exception
                    throw ex;
                }
                else
                {
                    // Throw exception
                    throw ex;
                }
            }

            // Return
            return Components;
        }

        // TODO BK 0 BK, Not tested
        public Cic.P000001.Common.ComponentDetail[] GetComponentDetails(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.Component component, Cic.P000001.Common.ComponentDetailTypeConstants componentDetailTypeConstant)
        {
            bool PassThrough;
            Cic.P000001.Common.ComponentDetail[] ComponentDetails;

            try
            {
                // Check properties
                if (setting != null)
                {
                    setting.CheckProperties();
                }
                if (treeNode != null)
                {
                    treeNode.CheckProperties();
                }
                if (component != null)
                {
                    component.CheckProperties();
                }

                // Execute web method helper
                ComponentDetails = getAdapter().GetComponentDetails(setting, treeNode, component, componentDetailTypeConstant);
                //ComponentDetails =  GetComponentDetailsWebMethodHelper.Execute(setting, treeNode, component, componentDetailTypeConstant);
            }
            catch (System.Exception ex)
            {
                // Get state
                PassThrough =  ExeConfigurationHelper.GetPassThroughExceptionsValue();
                // Check state
                if (PassThrough)
                {
                    // Throw exception
                    throw ex;
                }
                else
                {
                    // Throw exception
                    throw ex;
                }
            }

            // Return
            return ComponentDetails;
        }

        // TODO BK 0 BK, Not tested
        public Cic.P000001.Common.Picture[] GetComponentPictures(Cic.P000001.Common.Setting setting, Cic.P000001.Common.Component component, Cic.P000001.Common.PictureTypeConstants pictureTypeConstant, int top, bool withoutContent)
        {
            bool PassThrough;
            Cic.P000001.Common.Picture[] Pictures;

            try
            {
                // Check properties
                if (setting != null)
                {
                    setting.CheckProperties();
                }
                if (component != null)
                {
                    component.CheckProperties();
                }
                // Execute web method helper
                Pictures = getAdapter().GetComponentPictures(setting, component, pictureTypeConstant, top, withoutContent);
                //Pictures =  GetComponentPicturesWebMethodHelper.Execute(setting, component, pictureTypeConstant, top, withoutContent);
            }
            catch (System.Exception ex)
            {
                // Get state
                PassThrough =  ExeConfigurationHelper.GetPassThroughExceptionsValue();
                // Check state
                if (PassThrough)
                {
                    // Throw exception
                    throw ex;
                }
                else
                {
                    // Throw exception
                    throw ex;
                }
            }

            // Return
            return Pictures;
        }

        // TODO BK 0 BK, Not tested
        public  CheckComponentResult CheckComponent(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.Component component, Cic.P000001.Common.Component[] components)
        {
            bool PassThrough;
             CheckComponentResult CheckComponentResult;

            try
            {
                // Check properties
                if (setting != null)
                {
                    setting.CheckProperties();
                }
                if (treeNode != null)
                {
                    treeNode.CheckProperties();
                }
                if (component != null)
                {
                    component.CheckProperties();
                }
                if ((components != null) && (components.GetLength(0) > 0))
                {
                    foreach (Cic.P000001.Common.Component LoopComponent in components)
                    {
                        LoopComponent.CheckProperties();
                    }
                }
                // Execute web method helper
                CheckComponentResult = new Cic.OpenOne.CarConfigurator.BO.DataProviderService.CheckComponentResult(getAdapter().CheckComponent(setting, treeNode, component, components));
                //CheckComponentResult =  CheckComponentWebMethodHelper.Execute(setting, treeNode, component, components);
            }
            catch (System.Exception ex)
            {
                // Get state
                PassThrough =  ExeConfigurationHelper.GetPassThroughExceptionsValue();
                // Check state
                if (PassThrough)
                {
                    // Throw exception
                    throw ex;
                }
                else
                {
                    // Throw exception
                    throw ex;
                }
            }

            // Return
            return CheckComponentResult;
        }
        #endregion
    }
}