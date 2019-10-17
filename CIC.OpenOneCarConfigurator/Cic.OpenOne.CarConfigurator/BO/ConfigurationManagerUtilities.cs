
using Cic.OpenOne.CarConfigurator.BO.ConfigurationManager;
using Cic.OpenOne.CarConfigurator.BO.DataProviderService;
namespace Cic.OpenOne.CarConfigurator.BO
{
    public class ConfigurationManagerUtilities
    {

        public Cic.P000001.Common.ConfigurationManager.IAdapter getAdapter()
        {
            //XXX make configurable
            return new Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.EmbeddedConfigurationManagerAdapterLoader<Cic.OpenOne.CarConfigurator.BO.OracleConfigurationManager.OracleConfigurationManager>().LoadAdapter();
        }

        #region Methods
        public string DeliverServiceVersion()
        {
            bool PassThrough;
            string Version = string.Empty;

            try
            {
                // Execute web method helper
                Version =  DeliverServiceVersionWebMethodHelper.Execute();
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

        public System.Guid? ReserveConfigurationIdentifier(string userCode)
        {
            bool PassThrough;
            System.Guid? Identifier;

            try
            {
                // Execute web method helper
                Identifier = getAdapter().ReserveConfigurationIdentifier(userCode);
                //Identifier =  ReserveConfigurationIdentifierWebMethodHelper.Execute(userCode);
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
            return Identifier;
        }

        public bool CancelConfigurationIdentifier(string userCode, System.Guid configurationIdentifier)
        {
            bool PassThrough;
            bool Result;

            try
            {
                // Execute web method helper
                Result = getAdapter().CancelConfigurationIdentifierReservation(userCode, configurationIdentifier);
                //Result =  CancelConfigurationIdentifierReservationWebMethodHelper.Execute(userCode, configurationIdentifier);
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
            return Result;
        }

        public bool SaveConfiguration(string userCode, Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationPackage configurationPackage)
        {
            bool PassThrough;
            bool Result;

            try
            {
                // Check properties
                if (configurationPackage != null)
                {
                    configurationPackage.CheckProperties();
                }
                // Execute web method helper
                Result = getAdapter().SaveConfiguration(userCode, Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerWebToCommonConvertHelper.ConvertConfiguration(configurationPackage));

                
               
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
            return Result;
        }

        public bool RenameConfiguration(string userCode, System.Guid configurationIdentifier, string designation)
        {
            bool PassThrough;
            bool Result;

            try
            {
                // Execute web method helper
                Result = getAdapter().RenameConfiguration(userCode, configurationIdentifier, designation);
                //Result =  RenameConfigurationWebMethodHelper.Execute(userCode, configurationIdentifier, designation);
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
            return Result;
        }

        public bool PublishConfiguration(string userCode, System.Guid configurationIdentifier, bool isPublic)
        {
            bool PassThrough;
            bool Result;

            try
            {
                // Execute web method helper
                Result = getAdapter().PublishConfiguration(userCode, configurationIdentifier, isPublic);
                //Result =  PublishConfigurationWebMethodHelper.Execute(userCode, configurationIdentifier, isPublic);
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
            return Result;
        }

        public bool LockConfiguration(string userCode, System.Guid configurationIdentifier, bool isLocked)
        {
            bool PassThrough;
            bool Result;

            try
            {
                // Execute web method helper
                Result = getAdapter().LockConfiguration(userCode, configurationIdentifier, isLocked);
                //Result =  LockConfigurationWebMethodHelper.Execute(userCode, configurationIdentifier, isLocked);
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
            return Result;
        }

        public bool MoveConfiguration(string userCode, System.Guid configurationIdentifier, string targetGroupName, string targetGroupDescription)
        {
            bool PassThrough;
            bool Result;

            try
            {
                // Execute web method helper
                Result = getAdapter().MoveConfiguration(userCode, configurationIdentifier, targetGroupName, targetGroupDescription);
                //Result = MoveConfigurationWebMethodHelper.Execute(userCode, configurationIdentifier, targetGroupName, targetGroupDescription);
            }
            catch (System.Exception ex)
            {
                // Get state
                PassThrough = ExeConfigurationHelper.GetPassThroughExceptionsValue();
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
            return Result;
        }

        public bool CopyConfiguration(string userCode, System.DateTime createdAt, string groupName, string groupDescription, System.Guid sourceConfigurationIdentifier, System.Guid targetConfigurationIdentifier, bool includeCatalogItems)
        {
            bool PassThrough;
            bool Result;

            try
            {
                // Execute web method helper
                Result = getAdapter().CopyConfiguration(userCode, createdAt, groupName, groupDescription, sourceConfigurationIdentifier, targetConfigurationIdentifier, includeCatalogItems);
                //Result =  CopyConfigurationWebMethodHelper.Execute(userCode, createdAt, groupName, groupDescription, sourceConfigurationIdentifier, targetConfigurationIdentifier, includeCatalogItems);
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
            return Result;
        }

        public bool CopyConfigurationWithinTheSameGroup(string userCode, System.DateTime createdAt, string targetConfigurationDesignation, System.Guid sourceConfigurationIdentifier, System.Guid targetConfigurationIdentifier, bool includeCatalogItems)
        {
            bool PassThrough;
            bool Result;

            try
            {
                // Execute web method helper
                Result = getAdapter().CopyConfiguration(userCode, createdAt, targetConfigurationDesignation, sourceConfigurationIdentifier, targetConfigurationIdentifier, includeCatalogItems);
                
                //Result =  CopyConfigurationWithinTheSameGroupWebMethodHelper.Execute(userCode, createdAt, targetConfigurationDesignation, sourceConfigurationIdentifier, targetConfigurationIdentifier, includeCatalogItems);
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
            return Result;
        }

        public bool DeleteConfiguration(string userCode, System.Guid configurationIdentifier)
        {
            bool PassThrough;
            bool Result;

            try
            {
                // Execute web method helper
                Result = getAdapter().DeleteConfiguration(userCode, configurationIdentifier);

                //Result =  DeleteConfigurationWebMethodHelper.Execute(userCode, configurationIdentifier);
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
            return Result;
        }

        public ConfigurationPackage LoadConfiguration(string userCode, System.Guid configurationIdentifier)
        {
            bool PassThrough;
            ConfigurationPackage ConfigurationPackage;

            try
            {
                // Execute web method helper
                ConfigurationPackage = new Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationPackage(getAdapter().LoadConfiguration(userCode, configurationIdentifier));

                //ConfigurationPackage =  LoadConfigurationWebMethodHelper.Execute(userCode, configurationIdentifier);
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
            return ConfigurationPackage;
        }

        public ConfigurationPackage[] LoadConfigurations(string userCode, System.Guid? dataSourceIdentifier, string dataSourceVersion, string groupName, System.Guid? configurationIdentifier, string configurationDesignation, bool whereUserIsOwner)
        {
            bool PassThrough;
            ConfigurationPackage[] Configurations;

            try
            {
                // Execute web method helper
                Configurations = Cic.OpenOne.CarConfigurator.BO.ConfigurationManager.ConfigurationManagerCommonToWebConvertHelper.ConvertConfigurations(getAdapter().LoadConfigurations(userCode, dataSourceIdentifier, dataSourceVersion, groupName, configurationIdentifier, configurationDesignation, whereUserIsOwner));

                //Configurations =  LoadConfigurationsWebMethodHelper.Execute(userCode, dataSourceIdentifier, dataSourceVersion, groupName, configurationIdentifier, configurationDesignation, whereUserIsOwner);
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
            return Configurations;
        }

        public int CountConfigurations(string userCode, System.Guid? dataSourceIdentifier, string dataSourceVersion, string groupName, System.Guid? configurationIdentifier, string configurationDesignation, bool whereUserIsOwner)
        {
            bool PassThrough;
            int Count;

            try
            {
                // Execute web method helper
                Count = getAdapter().CountConfigurations(userCode, dataSourceIdentifier, dataSourceVersion, groupName, configurationIdentifier, configurationDesignation, whereUserIsOwner);
               // Count =  CountConfigurationsWebMethodHelper.Execute(userCode, dataSourceIdentifier, dataSourceVersion, groupName, configurationIdentifier, configurationDesignation, whereUserIsOwner);
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
            return Count;
        }
        #endregion
    }
}