using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
using Cic.OpenOne.CarConfigurator.BO;
using Cic.OpenOne.Common.Util.Security;
using Cic.OpenOne.Common.DTO;




namespace Cic.OpenLease.Service.Services.Merge.OpenLease
{
    /// <summary>
    /// Car Configurator Service
    /// </summary>
    [System.ServiceModel.ServiceBehavior(Namespace = "http://cic-software.de/CarConfigurator/CarConfiguratorService")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public class CarConfigurator : ICarConfigurator
    {
        private static int CUSTOMER_CODE = 300;//HEK

        private static IDataProviderUtilities getProvider()
        {
            return  new SmallDataProviderUtilities(); 
        }

        /// <summary>
        /// Deliver Service Version
        /// </summary>
        /// <returns>Version</returns>
        public string DeliverServiceVersion()
        {
            IDataProviderUtilities DataProviderUtilities = getProvider();
            return DataProviderUtilities.DeliverServiceVersion();
        }

        /// <summary>
        /// Deliver Data Provider Service State
        /// </summary>
        /// <returns>State</returns>
        public Cic.P000001.Common.ServiceState DeliverDataProviderServiceState()
        {
            IDataProviderUtilities DataProviderUtilities = getProvider();
            return DataProviderUtilities.DeliverServiceState();
        }

        /// <summary>
        /// Deliver Data Source Information
        /// </summary>
        /// <returns>Information Dataset</returns>
        public Cic.P000001.Common.DataSourceInformation DeliverDataSourceInformation()
        {
            IDataProviderUtilities DataProviderUtilities = getProvider();
            return DataProviderUtilities.DeliverDataSourceInformation();
        }

        /// <summary>
        /// Get Tree Info
        /// </summary>
        /// <param name="setting">Setting Structure</param>
        /// <param name="treeNode">Node Data</param>
        /// <returns>TreeInfo</returns>
        public TreeInfo GetTreeInfo(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode)
        {
            CredentialContext cctx = new CredentialContext();
            setting.SelectedLanguage = cctx.getUserLanguange();
            setting.customerCode = CUSTOMER_CODE;
            IDataProviderUtilities DataProviderUtilities = getProvider();
            return DataProviderUtilities.GetTreeInfo(setting, treeNode);
        }

        /// <summary>
        /// Get Tree Nodes
        /// </summary>
        /// <param name="setting">Setting Structure</param>
        /// <param name="treeNode">Node Data</param>
        /// <param name="getTreeNodeSearchModeConstant">Mode Constants</param>
        /// <returns>Treenodes List</returns>
        public Cic.P000001.Common.TreeNode[] GetTreeNodes(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.GetTreeNodeSearchModeConstants getTreeNodeSearchModeConstant)
        {

            ServiceValidator serviceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            setFilter(serviceValidator.SysBRAND, setting, Cic.OpenLease.Common.MembershipProvider.listBrands(serviceValidator.MembershipUserValidationInfo.PUSERDto.SYSPUSER.Value));
            setting.customerCode = CUSTOMER_CODE;
            setting.sysperole = serviceValidator.SysPEROLE;
            setting.SelectedLanguage = "de-DE";
            IDataProviderUtilities DataProviderUtilities = getProvider();
            Cic.P000001.Common.TreeNode[] rval =  DataProviderUtilities.GetTreeNodes(setting, treeNode, getTreeNodeSearchModeConstant);

            decimal Ust = LsAddHelper.getGlobalUst(serviceValidator.SysPEROLE);
              
            foreach(Cic.P000001.Common.TreeNode node in rval)
            {
                node.data.neupreisbrutto = (double)Cic.OpenLease.Service.RoundingFacade.getInstance().RoundPrice(Cic.OpenLease.Service.MwStFacade.getInstance().CalculateGrossValue((decimal)node.data.neupreisnetto, Ust));
            }
            return rval;
        }

        private void setFilter(long sysBrand, Cic.P000001.Common.Setting setting, Cic.OpenLease.ServiceAccess.DdOl.BRANDDto[] userBrands)
        {
           /* using (Cic.OpenLease.Model.DdOl.OlExtendedEntities Context = new Cic.OpenLease.Model.DdOl.OlExtendedEntities())
            {
                Cic.OpenLease.Model.DdOl.BRAND brand = Cic.OpenLease.Model.DdOl.BRANDHelper.DeliverBRAND(Context, (long)sysBrand);
                if (brand == null) return;
                string cbrand = brand.MANDANT.ToLower();
                //check if current brand is bmw or alphera

                if (cbrand.IndexOf("bmw") > -1 || cbrand.IndexOf("alphera") > -1)
                {
                    int bcount = 0;
                    foreach (Cic.OpenLease.ServiceAccess.DdOl.BRANDDto btest in userBrands)
                    {
                        if (btest.NAME.ToLower().IndexOf("bmw") < 0 && btest.NAME.ToLower().IndexOf("alphera") < 0) continue;
                        bcount++;
                        if (bcount > 1) break;
                    }

                    if (bcount > 1)
                    {
                        if (cbrand.IndexOf("bmw") > -1)
                        {
                            setting.addShowOnly("bmw");
                            setting.addShowOnly("mini");

                        }
                        else if (cbrand.IndexOf("alphera") > -1)
                        {
                            setting.addHide("bmw");
                            setting.addHide("mini");

                        }
                    }
                }
            }*/
        }
        /// <summary>
        /// Search Tree Nodes
        /// </summary>
        /// <param name="setting">Settings Structure</param>
        /// <param name="filter">Filter Settings</param>
        /// <param name="search">Search Criteria</param>
        /// <returns>List of found Treenodes</returns>
        public Cic.P000001.Common.TreeNode[] SearchTreeNodes(Cic.P000001.Common.Setting setting, Cic.P000001.Common.FilterParam[] filter, Cic.P000001.Common.Search search)
        {
            CredentialContext cctx = new CredentialContext();
            setting.SelectedLanguage = cctx.getUserLanguange();
            setting.customerCode = CUSTOMER_CODE;
            IDataProviderUtilities DataProviderUtilities = getProvider();
            return DataProviderUtilities.SearchTreeNodes(setting, filter, search);
        }

        /// <summary>
        /// Get Treenode Details
        /// </summary>
        /// <param name="setting">Settings Structure</param>
        /// <param name="treeNode">Node Data</param>
        /// <param name="treeNodeDetailTypeConstant">Detail Constants</param>
        /// <returns>Node Detail List</returns>
        public Cic.P000001.Common.TreeNodeDetail[] GetTreeNodeDetails(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.TreeNodeDetailTypeConstants treeNodeDetailTypeConstant)
        {
            CredentialContext cctx = new CredentialContext();
            setting.SelectedLanguage = cctx.getUserLanguange();
            setting.customerCode = CUSTOMER_CODE;
            IDataProviderUtilities DataProviderUtilities = getProvider();
            return DataProviderUtilities.GetTreeNodeDetails(setting, treeNode, treeNodeDetailTypeConstant);
        }

        /// <summary>
        /// Get Treenode Pictures
        /// </summary>
        /// <param name="setting">Settings Structure</param>
        /// <param name="treeNode">Node Data</param>
        /// <param name="pictureTypeConstant">Picture type Constants</param>
        /// <param name="top">top</param>
        /// <param name="withoutContent">Without Content flag</param>
        /// <returns>Treenode Pictures List</returns>
        public Cic.P000001.Common.Picture[] GetTreeNodePictures(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.PictureTypeConstants pictureTypeConstant, int top, bool withoutContent)
        {
            CredentialContext cctx = new CredentialContext();
            setting.SelectedLanguage = cctx.getUserLanguange();
            setting.customerCode = CUSTOMER_CODE;
            IDataProviderUtilities DataProviderUtilities = getProvider();
            return DataProviderUtilities.GetTreeNodePictures(setting, treeNode, pictureTypeConstant, top, withoutContent);
        }

        /// <summary>
        /// Get Components
        /// </summary>
        /// <param name="setting">Settings Structure</param>
        /// <param name="treeNode">Node Data</param>
        /// <param name="componentTypeConstant">Content Type Constants</param>
        /// <returns>Component List</returns>
        public Cic.P000001.Common.Component[] GetComponents(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.ComponentTypeConstants componentTypeConstant)
        {
            CredentialContext cctx = new CredentialContext();
            setting.SelectedLanguage = cctx.getUserLanguange();
            setting.customerCode = CUSTOMER_CODE;
            IDataProviderUtilities DataProviderUtilities = getProvider();
            return DataProviderUtilities.GetComponents(setting, treeNode, componentTypeConstant);
        }

        /// <summary>
        /// Get Component Details
        /// </summary>
        /// <param name="setting">Settings Structure</param>
        /// <param name="treeNode">Node Data</param>
        /// <param name="component">Component Data</param>
        /// <param name="componentDetailTypeConstant">Component Detail Constants</param>
        /// <returns>Component Details List</returns>
        public Cic.P000001.Common.ComponentDetail[] GetComponentDetails(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.Component component, Cic.P000001.Common.ComponentDetailTypeConstants componentDetailTypeConstant)
        {
            CredentialContext cctx = new CredentialContext();
            setting.SelectedLanguage = cctx.getUserLanguange();
            setting.customerCode = CUSTOMER_CODE;
            IDataProviderUtilities DataProviderUtilities = getProvider();
            return DataProviderUtilities.GetComponentDetails(setting, treeNode, component, componentDetailTypeConstant);
        }

        /// <summary>
        /// Get Component Pictures
        /// </summary>
        /// <param name="setting">Settings Structure</param>
        /// <param name="component">Component Data</param>
        /// <param name="pictureTypeConstant">Picture type COnstants</param>
        /// <param name="top">top</param>
        /// <param name="withoutContent">Without Content Flag</param>
        /// <returns>Component Pictures List</returns>
        public Cic.P000001.Common.Picture[] GetComponentPictures(Cic.P000001.Common.Setting setting, Cic.P000001.Common.Component component, Cic.P000001.Common.PictureTypeConstants pictureTypeConstant, int top, bool withoutContent)
        {
            CredentialContext cctx = new CredentialContext();
            setting.SelectedLanguage = cctx.getUserLanguange();
            setting.customerCode = CUSTOMER_CODE;
            IDataProviderUtilities DataProviderUtilities = getProvider();
            return DataProviderUtilities.GetComponentPictures(setting, component, pictureTypeConstant, top, withoutContent);
        }

        /// <summary>
        /// Check Component
        /// </summary>
        /// <param name="setting">Settings Structure</param>
        /// <param name="treeNode">Node Data</param>
        /// <param name="component">Component Data</param>
        /// <param name="components">Components Data</param>
        /// <returns>Check Result</returns>
        public CheckComponentResult CheckComponent(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, Cic.P000001.Common.Component component, Cic.P000001.Common.Component[] components)
        {
            CredentialContext cctx = new CredentialContext();
            setting.SelectedLanguage = cctx.getUserLanguange();
            setting.customerCode = CUSTOMER_CODE;
            IDataProviderUtilities DataProviderUtilities = getProvider();
            return DataProviderUtilities.CheckComponent(setting, treeNode, component, components);
        }

        /// <summary>
        /// Deliver Configuration Manager Service State
        /// </summary>
        /// <returns>Service State Data</returns>
        public Cic.P000001.Common.ServiceState DeliverConfigurationManagerServiceState()
        {
            ConfigurationManagerUtilities ConfigurationManagerUtilities = new ConfigurationManagerUtilities();
            return ConfigurationManagerUtilities.DeliverServiceState();
        }

        /// <summary>
        /// Reserve Configuration Identifier
        /// </summary>
        /// <param name="userCode">User Code</param>
        /// <returns>GUID</returns>
        public System.Guid? ReserveConfigurationIdentifier(string userCode)
        {
            ConfigurationManagerUtilities ConfigurationManagerUtilities = new ConfigurationManagerUtilities();
            return ConfigurationManagerUtilities.ReserveConfigurationIdentifier(userCode);
        }

        /// <summary>
        /// Cancel Configuration Identifier
        /// </summary>
        /// <param name="userCode">User Code</param>
        /// <param name="configurationIdentifier">Configuration ID</param>
        /// <returns>Success</returns>
        public bool CancelConfigurationIdentifier(string userCode, System.Guid configurationIdentifier)
        {
            ConfigurationManagerUtilities ConfigurationManagerUtilities = new ConfigurationManagerUtilities();
            return ConfigurationManagerUtilities.CancelConfigurationIdentifier(userCode, configurationIdentifier);
        }

        /// <summary>
        /// Save Configuration
        /// </summary>
        /// <param name="userCode">User Code</param>
        /// <param name="configurationPackage">Configuration Package Data</param>
        /// <returns>Success</returns>
        public bool SaveConfiguration(string userCode, Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationPackage configurationPackage)
        {
            ConfigurationManagerUtilities ConfigurationManagerUtilities = new ConfigurationManagerUtilities();
            return ConfigurationManagerUtilities.SaveConfiguration(userCode, configurationPackage);
        }

        /// <summary>
        /// Rename Configuration
        /// </summary>
        /// <param name="userCode">User Code</param>
        /// <param name="configurationIdentifier">Configuration Identifier Data</param>
        /// <param name="designation">New Name</param>
        /// <returns>Success</returns>
        public bool RenameConfiguration(string userCode, System.Guid configurationIdentifier, string designation)
        {
            ConfigurationManagerUtilities ConfigurationManagerUtilities = new ConfigurationManagerUtilities();
            return ConfigurationManagerUtilities.RenameConfiguration(userCode, configurationIdentifier, designation);
        }

        /// <summary>
        /// Publish Configuration
        /// </summary>
        /// <param name="userCode">User Code</param>
        /// <param name="configurationIdentifier">Configuration ID</param>
        /// <param name="isPublic">Is Public Flag</param>
        /// <returns>Success</returns>
        public bool PublishConfiguration(string userCode, System.Guid configurationIdentifier, bool isPublic)
        {
            ConfigurationManagerUtilities ConfigurationManagerUtilities = new ConfigurationManagerUtilities();
            return ConfigurationManagerUtilities.PublishConfiguration(userCode, configurationIdentifier, isPublic);
        }


        /// <summary>
        /// Lock Configuration
        /// </summary>
        /// <param name="userCode">User Code</param>
        /// <param name="configurationIdentifier">Identifier</param>
        /// <param name="isLocked">Is Locked Flag</param>
        /// <returns>Success</returns>
        public bool LockConfiguration(string userCode, System.Guid configurationIdentifier, bool isLocked)
        {
            ConfigurationManagerUtilities ConfigurationManagerUtilities = new ConfigurationManagerUtilities();
            return ConfigurationManagerUtilities.LockConfiguration(userCode, configurationIdentifier, isLocked);
        }

        /// <summary>
        /// Move Configuration
        /// </summary>
        /// <param name="userCode">User Code</param>
        /// <param name="configurationIdentifier">Identifier</param>
        /// <param name="targetGroupName">Target Group Name</param>
        /// <param name="targetGroupDescription">Target Group Description</param>
        /// <returns>Success</returns>
        public bool MoveConfiguration(string userCode, System.Guid configurationIdentifier, string targetGroupName, string targetGroupDescription)
        {
            ConfigurationManagerUtilities ConfigurationManagerUtilities = new ConfigurationManagerUtilities();
            return ConfigurationManagerUtilities.MoveConfiguration(userCode, configurationIdentifier, targetGroupName, targetGroupDescription);
        }

        /// <summary>
        /// Copy Configuration
        /// </summary>
        /// <param name="userCode">User Code</param>
        /// <param name="createdAt">Created At</param>
        /// <param name="groupName">Group Name</param>
        /// <param name="groupDescription">Group Description</param>
        /// <param name="sourceConfigurationIdentifier">Source Config ID</param>
        /// <param name="targetConfigurationIdentifier">Target Config ID</param>
        /// <param name="includeCatalogItems">Include Catalog Item</param>
        /// <returns>Success</returns>
        public bool CopyConfiguration(string userCode, System.DateTime createdAt, string groupName, string groupDescription, System.Guid sourceConfigurationIdentifier, System.Guid targetConfigurationIdentifier, bool includeCatalogItems)
        {
            ConfigurationManagerUtilities ConfigurationManagerUtilities = new ConfigurationManagerUtilities();
            return ConfigurationManagerUtilities.CopyConfiguration(userCode, createdAt, groupName, groupDescription, sourceConfigurationIdentifier, targetConfigurationIdentifier, includeCatalogItems);
        }

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
        public bool CopyConfigurationWithinTheSameGroup(string userCode, System.DateTime createdAt, string targetConfigurationDesignation, System.Guid sourceConfigurationIdentifier, System.Guid targetConfigurationIdentifier, bool includeCatalogItems)
        {
            ConfigurationManagerUtilities ConfigurationManagerUtilities = new ConfigurationManagerUtilities();
            return ConfigurationManagerUtilities.CopyConfigurationWithinTheSameGroup(userCode, createdAt, targetConfigurationDesignation, sourceConfigurationIdentifier, targetConfigurationIdentifier, includeCatalogItems);
        }

        /// <summary>
        /// Delete Configuration
        /// </summary>
        /// <param name="userCode">User Code</param>
        /// <param name="configurationIdentifier">Config ID</param>
        /// <returns>Success</returns>
        public bool DeleteConfiguration(string userCode, System.Guid configurationIdentifier)
        {
            ConfigurationManagerUtilities ConfigurationManagerUtilities = new ConfigurationManagerUtilities();
            return ConfigurationManagerUtilities.DeleteConfiguration(userCode, configurationIdentifier);
        }

        /// <summary>
        /// Load Config 
        /// </summary>
        /// <param name="userCode">User Code</param>
        /// <param name="configurationIdentifier">Config ID</param>
        /// <returns>Success</returns>
        public Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationPackage LoadConfiguration(string userCode, System.Guid configurationIdentifier)
        {
            ConfigurationManagerUtilities ConfigurationManagerUtilities = new ConfigurationManagerUtilities();
            return ConfigurationManagerUtilities.LoadConfiguration(userCode, configurationIdentifier);
        }

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
        public Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationPackage[] LoadConfigurations(string userCode, System.Guid? dataSourceIdentifier, string dataSourceVersion, string groupName, System.Guid? configurationIdentifier, string configurationDesignation, bool whereUserIsOwner)
        {
            ConfigurationManagerUtilities ConfigurationManagerUtilities = new ConfigurationManagerUtilities();
            return ConfigurationManagerUtilities.LoadConfigurations(userCode, dataSourceIdentifier, dataSourceVersion, groupName, configurationIdentifier, configurationDesignation, whereUserIsOwner);
        }

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
        public int CountConfigurations(string userCode, System.Guid? dataSourceIdentifier, string dataSourceVersion, string groupName, System.Guid? configurationIdentifier, string configurationDesignation, bool whereUserIsOwner)
        {

            ConfigurationManagerUtilities ConfigurationManagerUtilities = new ConfigurationManagerUtilities();
            return ConfigurationManagerUtilities.CountConfigurations(userCode, dataSourceIdentifier, dataSourceVersion, groupName, configurationIdentifier, configurationDesignation, whereUserIsOwner);
        }



        /// <summary>
        /// Lists a reduced list of Aufbau-types (Coupe, Sedan)
        /// </summary>
        /// <returns>olistDto</returns>
        public olistDto listAufbau()
        {
            throw new Exception("not implemented");
            /*
            olistDto rval = new olistDto();
            CredentialContext cctx = new CredentialContext();
            try
            {

                cctx.validateService();
                rval.entries = new DictionaryListsBo(CommonDaoFactory.getInstance().getDictionaryListsDao(), CommonDaoFactory.getInstance().getTranslateDao(), cctx.getUserLanguange()).listByCode(Common.DTO.DDLKPPOSType.AUFBAUCODE);
                rval.success();
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }*/
        }

        /// <summary>
        /// Lists all Getriebearten
        /// </summary>
        /// <returns>olistDto</returns>
        public olistDto listGetriebearten()
        {
            throw new Exception("not implemented");
            /*
            olistDto rval = new olistDto();
            CredentialContext cctx = new CredentialContext();
            try
            {

                cctx.validateService();
                rval.entries = new DictionaryListsBo(CommonDaoFactory.getInstance().getDictionaryListsDao(), CommonDaoFactory.getInstance().getTranslateDao(), cctx.getUserLanguange()).listByCode(Common.DTO.DDLKPPOSType.GETRIEBEARTCODE);
                rval.success();
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
             * */
        }

         /// <summary>
        /// Lists all SlaPause reasons
        /// </summary>
        /// <returns>olistDto</returns>
		public olistDto listSlaPause ()
		{
			throw new Exception ("not implemented");
		}

		/// <summary>
        /// Lists all Getriebearten
        /// </summary>
        /// <returns>olistDto</returns>
        public olistDto listTreibstoffe()
        {
            throw new Exception("not implemented");
            /*
            olistDto rval = new olistDto();
            CredentialContext cctx = new CredentialContext();
            try
            {

                cctx.validateService();
                rval.entries = new DictionaryListsBo(CommonDaoFactory.getInstance().getDictionaryListsDao(), CommonDaoFactory.getInstance().getTranslateDao(), cctx.getUserLanguange()).listByCode(Common.DTO.DDLKPPOSType.TREIBSTOFFCODE);
                rval.success();
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
             * */
        }

        /// <summary>
        /// Lists all Aufbau-types (Coupe, Sedan) as resulted in the carconfig nodes
        /// </summary>
        /// <returns>olistDto</returns>
        public olistDto listAufbauCodes()
        {
            throw new Exception("not implemented");
            /*
            olistDto rval = new olistDto();
            CredentialContext cctx = new CredentialContext();
            try
            {

                cctx.validateService();
                rval.entries = new DictionaryListsBo(CommonDaoFactory.getInstance().getDictionaryListsDao(), CommonDaoFactory.getInstance().getTranslateDao(), cctx.getUserLanguange()).listAufbau();
                rval.success();
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
             */
        }

        /// <summary>
        /// Lists all Getriebeart-Codes as resulted in the carconfig nodes
        /// </summary>
        /// <returns>olistDto</returns>
        public olistDto listGetriebeartenCodes()
        {
            throw new Exception("not implemented");
            /*
            olistDto rval = new olistDto();
            CredentialContext cctx = new CredentialContext();
            try
            {

                cctx.validateService();
                rval.entries = new DictionaryListsBo(CommonDaoFactory.getInstance().getDictionaryListsDao(), CommonDaoFactory.getInstance().getTranslateDao(), cctx.getUserLanguange()).listGetriebeart();
                rval.success();
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }*/
        }

        /// <summary>
        /// Lists all SlaPause reasons
        /// </summary>
        /// <returns>olistDto</returns>
		public olistDto listSlaPauseCodes ()
		{
			throw new Exception ("not implemented");
		}

        /// <summary>
        /// Lists all Treibstoffart-Codes as resulted in the carconfig nodes
        /// </summary>
        /// <returns>olistDto</returns>
        public olistDto listTreibstoffCodes()
        {
            throw new Exception("not implemented");
            /*
            olistDto rval = new olistDto();
            CredentialContext cctx = new CredentialContext();
            try
            {

                cctx.validateService();
                rval.entries = new DictionaryListsBo(CommonDaoFactory.getInstance().getDictionaryListsDao(), CommonDaoFactory.getInstance().getTranslateDao(), cctx.getUserLanguange()).listTreibstoffart();
                rval.success();
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }*/
        }


        /// <summary>
        /// Lists all Brands
        /// </summary>
        /// <returns>olistDto</returns>
        public olistDto listMarken(long obtypid)
        {
            throw new Exception("not implemented");
            /*
            olistDto rval = new olistDto();
            CredentialContext cctx = new CredentialContext();
            try
            {

                cctx.validateService();
                rval.entries = new DictionaryListsBo(CommonDaoFactory.getInstance().getDictionaryListsDao(), CommonDaoFactory.getInstance().getTranslateDao(), cctx.getUserLanguange()).listMarken((int)obtypid);
                rval.success();
                return rval;
            }
            catch (ServiceBaseException e)//expected service exceptions
            {
                cctx.fillBaseDto(rval, e, "F_00002_ServiceBaseException");
                return rval;
            }
            catch (Exception e)//unhandled exception - shoLdnt happen!
            {
                cctx.fillBaseDto(rval, e, "F_00001_GeneralError");
                return rval;
            }
            */

        }




    }
}
