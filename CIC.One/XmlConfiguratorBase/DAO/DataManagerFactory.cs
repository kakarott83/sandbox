using XmlConfiguratorBase.DTO;

namespace XmlConfiguratorBase.DAO
{
    /// <summary>
    /// Creates access providers for different data sources
    /// </summary>
    public class DataManagerFactory
    {
        public static IDataManager Custom { get; set; }
        private static IDataManager NewFileManager { get { return new FileManager("..\\..\\..\\Cic.One.Web.Service\\wfvconfig.dll"); } }
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
                    return NewFileManager;
                case DataSource.DATABASE:
                    return new DatabaseManager();
                case DataSource.CUSTOM:
                    if (Custom == null)
                        Custom = NewFileManager;
                    return Custom;
                default:
                    return NewFileManager;
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
