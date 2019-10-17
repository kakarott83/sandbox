using WfvXmlConfigurator.DTO;

namespace WfvXmlConfigurator.DAO
{
    /// <summary>
    /// Creates access providers for different data sources
    /// </summary>
    public class DataManagerFactory
    {
        private static DataSource LastCreatedDataSource = DataSource.NO_SOURCE;

        /// <summary>
        /// Create a new instance of a data manager communicating with the given data source
        /// </summary>
        /// <param name="datasource">Where the data comes from and goes to</param>
        /// <returns>manager for communication with the given data source</returns>
        public static IDataManager CreateDataManager(DataSource datasource)
        {
            switch (datasource)
            {
                case DataSource.XML_FILE:
                    return new FileManager("..\\..\\..\\Cic.One.Web.Service\\wfvconfig.dll");
                case DataSource.DATABASE:
                    return new DatabaseManager();
                default:
                    return new FileManager("..\\..\\..\\Cic.One.Web.Service\\wfvconfig.dll");
            }
        }

        /// <summary>
        /// Get the data source of the most recent data manager
        /// </summary>
        /// <returns>saved data source of last created data manager</returns>
        public static DataSource GetLastDataManagerSource()
        {
            return LastCreatedDataSource;
        }
    }
}
