using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.CarConfigurator.BO.ConfigurationManager
{
    /// <summary>
    /// Car Configurator
    /// </summary>
    [System.CLSCompliant(true)]
    [System.ServiceModel.ServiceContract(Name = "CarConfigurator", Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
    public interface ICarConfigurator
    {
        /// <summary>
        /// Deliver Service Version
        /// </summary>
        /// <returns>Version</returns>
        [OperationContract]
        string DeliverServiceVersion();

        /// <summary>
        /// Deliver Data Provider Service State
        /// </summary>
        /// <returns>State</returns>
        [OperationContract]
        Cic.P000001.Common.ServiceState DeliverDataProviderServiceState();

        /// <summary>
        /// Deliver Data Source Information
        /// </summary>
        /// <returns>Information Dataset</returns>
        [OperationContract]
        Cic.P000001.Common.DataSourceInformation DeliverDataSourceInformation();

        /// <summary>
        /// Get Tree Info
        /// </summary>
        /// <param name="setting">Setting Structure</param>
        /// <param name="treeNode">Node Data</param>
        /// <returns>TreeInfo</returns>
        [OperationContract]
        Cic.OpenOne.CarConfigurator.BO.DataProviderService.TreeInfo GetTreeInfo(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode);

        /// <summary>
        /// Get Tree Nodes
        /// </summary>
        /// <param name="setting">Setting Structure</param>
        /// <param name="treeNode">Node Data</param>
        /// <param name="getTreeNodeSearchModeConstant">Mode Constants</param>
        /// <returns>Treenodes List</returns>
        [OperationContract]
        Cic.P000001.Common.TreeNode[] GetTreeNodes(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.GetTreeNodeSearchModeConstants getTreeNodeSearchModeConstant);

        /// <summary>
        /// Search Tree Nodes
        /// </summary>
        /// <param name="setting">Settings Structure</param>
        /// <param name="filter">Filter Settings</param>
        /// <param name="search">Search Criteria</param>
        /// <returns>List of found Treenodes</returns>
        [OperationContract]
        Cic.P000001.Common.TreeNode[] SearchTreeNodes(Cic.P000001.Common.Setting setting, Cic.P000001.Common.FilterParam[] filter, Cic.P000001.Common.Search search);

        /// <summary>
        /// Get Treenode Details
        /// </summary>
        /// <param name="setting">Settings Structure</param>
        /// <param name="treeNode">Node Data</param>
        /// <param name="treeNodeDetailTypeConstant">Detail Constants</param>
        /// <returns>Node Detail List</returns>
        [OperationContract]
        Cic.P000001.Common.TreeNodeDetail[] GetTreeNodeDetails(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.TreeNodeDetailTypeConstants treeNodeDetailTypeConstant);

        /// <summary>
        /// Get Treenode Pictures
        /// </summary>
        /// <param name="setting">Settings Structure</param>
        /// <param name="treeNode">Node Data</param>
        /// <param name="pictureTypeConstant">Picture type Constants</param>
        /// <param name="top">top</param>
        /// <param name="withoutContent">Without Content flag</param>
        /// <returns>Treenode Pictures List</returns>
        [OperationContract]
        Cic.P000001.Common.Picture[] GetTreeNodePictures(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.PictureTypeConstants pictureTypeConstant, int top, bool withoutContent);

        /// <summary>
        /// Get Components
        /// </summary>
        /// <param name="setting">Settings Structure</param>
        /// <param name="treeNode">Node Data</param>
        /// <param name="componentTypeConstant">Content Type Constants</param>
        /// <returns>Component List</returns>
        [OperationContract]
        Cic.P000001.Common.Component[] GetComponents(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.ComponentTypeConstants componentTypeConstant);

        /// <summary>
        /// Get Component Details
        /// </summary>
        /// <param name="setting">Settings Structure</param>
        /// <param name="treeNode">Node Data</param>
        /// <param name="component">Component Data</param>
        /// <param name="componentDetailTypeConstant">Component Detail Constants</param>
        /// <returns>Component Details List</returns>
        [OperationContract]
        Cic.P000001.Common.ComponentDetail[] GetComponentDetails(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.Component component, Cic.P000001.Common.ComponentDetailTypeConstants componentDetailTypeConstant);

        /// <summary>
        /// Get Component Pictures
        /// </summary>
        /// <param name="setting">Settings Structure</param>
        /// <param name="component">Component Data</param>
        /// <param name="pictureTypeConstant">Picture type COnstants</param>
        /// <param name="top">top</param>
        /// <param name="withoutContent">Without Content Flag</param>
        /// <returns>Component Pictures List</returns>
        [OperationContract]
        Cic.P000001.Common.Picture[] GetComponentPictures(Cic.P000001.Common.Setting setting, Cic.P000001.Common.Component component, Cic.P000001.Common.PictureTypeConstants pictureTypeConstant, int top, bool withoutContent);

        /// <summary>
        /// Check Component
        /// </summary>
        /// <param name="setting">Settings Structure</param>
        /// <param name="treeNode">Node Data</param>
        /// <param name="component">Component Data</param>
        /// <param name="components">Components Data</param>
        /// <returns>Check Result</returns>
        [OperationContract]
        Cic.OpenOne.CarConfigurator.BO.DataProviderService.CheckComponentResult CheckComponent(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.Component component, Cic.P000001.Common.Component[] components);

        /// <summary>
        /// Deliver Configuration Manager Service State
        /// </summary>
        /// <returns>Service State Data</returns>
        [OperationContract]
        Cic.P000001.Common.ServiceState DeliverConfigurationManagerServiceState();

        /// <summary>
        /// Reserve Configuration Identifier
        /// </summary>
        /// <param name="userCode">User Code</param>
        /// <returns>GUID</returns>
        [OperationContract]
        System.Guid? ReserveConfigurationIdentifier(string userCode);

        /// <summary>
        /// Cancel Configuration Identifier
        /// </summary>
        /// <param name="userCode">User Code</param>
        /// <param name="configurationIdentifier">Configuration ID</param>
        /// <returns>Success</returns>
        [OperationContract]
        bool CancelConfigurationIdentifier(string userCode, System.Guid configurationIdentifier);

        /// <summary>
        /// Save Configuration
        /// </summary>
        /// <param name="userCode">User Code</param>
        /// <param name="configurationPackage">Configuration Package Data</param>
        /// <returns>Success</returns>
        [OperationContract]
        bool SaveConfiguration(string userCode, Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationPackage configurationPackage);

        /// <summary>
        /// Rename Configuration
        /// </summary>
        /// <param name="userCode">User Code</param>
        /// <param name="configurationIdentifier">Configuration Identifier Data</param>
        /// <param name="designation">New Name</param>
        /// <returns>Success</returns>
        [OperationContract]
        bool RenameConfiguration(string userCode, System.Guid configurationIdentifier, string designation);

        /// <summary>
        /// Publish Configuration
        /// </summary>
        /// <param name="userCode">User Code</param>
        /// <param name="configurationIdentifier">Configuration ID</param>
        /// <param name="isPublic">Is Public Flag</param>
        /// <returns>Success</returns>
        [OperationContract]
        bool PublishConfiguration(string userCode, System.Guid configurationIdentifier, bool isPublic);

        /// <summary>
        /// Lock Configuration
        /// </summary>
        /// <param name="userCode">User Code</param>
        /// <param name="configurationIdentifier">Identifier</param>
        /// <param name="isLocked">Is Locked Flag</param>
        /// <returns>Success</returns>
        [OperationContract]
        bool LockConfiguration(string userCode, System.Guid configurationIdentifier, bool isLocked);

        /// <summary>
        /// Move Configuration
        /// </summary>
        /// <param name="userCode">User Code</param>
        /// <param name="configurationIdentifier">Identifier</param>
        /// <param name="targetGroupName">Target Group Name</param>
        /// <param name="targetGroupDescription">Target Group Description</param>
        /// <returns>Success</returns>
        [OperationContract]
        bool MoveConfiguration(string userCode, System.Guid configurationIdentifier, string targetGroupName, string targetGroupDescription);

        /// <summary>
        /// Copy Configuration
        /// </summary>
        /// <param name="userode">User Code</param>
        /// <param name="createdAt">Created At</param>
        /// <param name="groupName">Group Name</param>
        /// <param name="groupDescription">Group Description</param>
        /// <param name="sourceConfigurationIdentifier">Source Config ID</param>
        /// <param name="targetConfigurationIdentifier">Target Config ID</param>
        /// <param name="includeCatalogItems">Include Catalog Item</param>
        /// <returns>Success</returns>
        [OperationContract]
        bool CopyConfiguration(string userode, System.DateTime createdAt, string groupName, string groupDescription, System.Guid sourceConfigurationIdentifier, System.Guid targetConfigurationIdentifier, bool includeCatalogItems);

        /// <summary>
        /// Copy Configuration Within The Same Group
        /// </summary>
        /// <param name="userCode">User Code</param>
        /// <param name="createdAt">Created At</param>
        /// <param name="targetConfigurationDesignation">Target Config Designation</param>
        /// <param name="sourceConfigurationIdentifier">Source Config ID</param>
        /// <param name="targetConfigurationIdentifier">Target Config ID</param>
        /// <param name="includeCatalogItems">Included Items</param>
        /// <returns>Success</returns>
        [OperationContract]
        bool CopyConfigurationWithinTheSameGroup(string userCode, System.DateTime createdAt, string targetConfigurationDesignation, System.Guid sourceConfigurationIdentifier, System.Guid targetConfigurationIdentifier, bool includeCatalogItems);

        /// <summary>
        /// Delete Configuration
        /// </summary>
        /// <param name="userCode">User Code</param>
        /// <param name="configurationIdentifier">Config ID</param>
        /// <returns>Success</returns>
        [OperationContract]
        bool DeleteConfiguration(string userCode, System.Guid configurationIdentifier);

        /// <summary>
        /// Load Config 
        /// </summary>
        /// <param name="userCode">User Code</param>
        /// <param name="configurationIdentifier">Config ID</param>
        /// <returns>Success</returns>
        [OperationContract]
        Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationPackage LoadConfiguration(string userCode, System.Guid configurationIdentifier);

        /// <summary>
        /// Load Configurations
        /// </summary>
        /// <param name="userCode">User Code</param>
        /// <param name="dataSourceIdentifier">Source ID</param>
        /// <param name="dataSourceVersion">Source Version</param>
        /// <param name="groupName">Group Name</param>
        /// <param name="configurationIdentifier">Config ID</param>
        /// <param name="configurationDesignation">Config Designation</param>
        /// <param name="whereUserIsOwner">User Is Owner Flag</param>
        /// <returns>Success</returns>
        [OperationContract]
        Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationPackage[] LoadConfigurations(string userCode, System.Guid? dataSourceIdentifier, string dataSourceVersion, string groupName, System.Guid? configurationIdentifier, string configurationDesignation, bool whereUserIsOwner);

        /// <summary>
        /// Count Configurations
        /// </summary>
        /// <param name="userCode">User Code</param>
        /// <param name="dataSourceIdentifier">Source ID</param>
        /// <param name="dataSourceVersion">Source Version</param>
        /// <param name="groupName">Group Name</param>
        /// <param name="configurationIdentifier">Config ID</param>
        /// <param name="configurationDesignation">Config Designation</param>
        /// <param name="whereUserIsOwner">User is Owner Flag</param>
        /// <returns>Success</returns>
        [OperationContract]
        int CountConfigurations(string userCode, System.Guid? dataSourceIdentifier, string dataSourceVersion, string groupName, System.Guid? configurationIdentifier, string configurationDesignation, bool whereUserIsOwner);

        /// <summary>
        /// List all Brands
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        olistDto listMarken(long obtypid);

        /// <summary>
        /// Lists all Aufbau-types (Coupe, Sedan)
        /// </summary>
        /// <returns>olistDto</returns>
        [OperationContract]
        olistDto listAufbau();

        
        /// <summary>
        /// Lists all Aufbau-types (Coupe, Sedan)
        /// </summary>
        /// <returns>olistDto</returns>
        [OperationContract]
        olistDto listAufbauCodes();
        
        /// <summary>
        /// Lists all Getriebearten
        /// </summary>
        /// <returns>olistDto</returns>
        [OperationContract]
        olistDto listGetriebearten();

		/// <summary>
		/// Lists all SlaPause reasons
		/// </summary>
		/// <returns>olistDto</returns>
		[OperationContract]
		olistDto listSlaPause ();
		
		/// <summary>
        /// Lists all Treibstoffe
        /// </summary>
        /// <returns>olistDto</returns>
        [OperationContract]
        olistDto listTreibstoffe();

        /// <summary>
        /// Lists all Treibstoffe
        /// </summary>
        /// <returns>olistDto</returns>
        [OperationContract]
        olistDto listTreibstoffCodes();

		/// <summary>
		/// Lists all Getriebearten
		/// </summary>
		/// <returns>olistDto</returns>
		[OperationContract]
		olistDto listGetriebeartenCodes ();

		/// <summary>
		/// Lists all SlaPause reasons
		/// </summary>
		/// <returns>olistDto</returns>
		[OperationContract]
		olistDto listSlaPauseCodes ();

	}
}
