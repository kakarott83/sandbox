using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;

namespace Cic.OpenOne.Common.Util.Config
{
  
    /// <summary>
    /// Methods for creating an edmx db connection
    /// </summary>
    public static class ConnectionStringBuilder
    {
        #region Methods

        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        

        /// <summary>
        ///  Delivers the Connection for a EDMX-Context
        /// </summary>
        /// <param name="objectContext">The EDMX ObjectContext Type</param>
        /// <param name="edmxName">The Name of the EDMX e.g. DdOl</param>
        /// <returns></returns>
        public static string DeliverConnectionString(Type objectContext, string edmxName)
        {
            return DeliverConnectionString(System.Reflection.Assembly.GetAssembly(objectContext).FullName, edmxName,edmxName);
            
        }
        /// <summary>
        /// Gets the Connection for the DB-ISLE EDMX-Context
        /// </summary>
        /// <param name="objectContext"></param>
        /// <returns></returns>
        public static string GetConnectionString(Type contextType)
        {

            String devartConnectionString = Configuration.DeliverOpenLeaseConnectionString();
            var assemblyName = contextType.Assembly.GetName().Name;
            if (contextType.Namespace == null)
            {
                throw new ArgumentException("The namespace of the contextType can't be null.");
            }

            var folder = contextType.Namespace.Substring(assemblyName.Length + 1);
            var edmx = contextType.Name.EndsWith("Context") ? contextType.Name.Substring(0, contextType.Name.Length - 7) : contextType.Name;
            var entityBuilder = new System.Data.EntityClient.EntityConnectionStringBuilder
            {
                Provider = Cic.OpenOne.Common.Properties.Config.Default.DatasourceProviderName,
                ProviderConnectionString = devartConnectionString,
                Metadata = string.Format("res://*/{0}.{1}.csdl|res://*/{0}.{1}.ssdl|res://*/{0}.{1}.msl", folder, edmx),
            };
            var connectionString = entityBuilder.ToString();
            return connectionString;
        
        }
        /// <summary>
        /// Delivers the Connection for a EDMX-Context
        /// Reading this property does take almost no time, so caching is unnecessary
        /// </summary>
        /// <param name="assemblyFullName"></param>
        /// <param name="folderName"></param>
        /// <param name="edmxName"></param>
        /// <returns></returns>
        public static string DeliverConnectionString(string assemblyFullName, string folderName, string edmxName)
        {
            
            string rval = DeliverConnectionString(assemblyFullName, folderName, edmxName, Cic.OpenOne.Common.Properties.Config.Default.DatasourceProviderName, Configuration.DeliverOpenLeaseConnectionString());
            
            return rval;
            
        }

       /// <summary>
       /// Creates a Metadata Workspace and Connection String for EF
       /// 
       /// </summary>
       /// <param name="assemblyFullName">the Assembly (binary) containing the Model Descriptors</param>
       /// <param name="folderName">The Folder (after the Assembly-Namespace)</param>
       /// <param name="edmxName">the Name of the EDMX without suffix</param>
       /// <param name="providerName">The database provider name (eg. Devart.Data.Oracle)</param>
       /// <param name="providerConnectionString">The sql connection String</param>
       /// <returns></returns>
        public static string DeliverConnectionString(string assemblyFullName, string folderName, string edmxName, string providerName, string providerConnectionString)
        {
            System.Text.StringBuilder StringBuilder;

            // New string builder
            StringBuilder = new System.Text.StringBuilder();
            // Add metadata
            StringBuilder.Append(@"metadata=");
            // Add resource files
            DeliverMetaDataPath(StringBuilder, assemblyFullName, folderName, edmxName);
            // Check provider name
            if (providerName != null)
            {
                // Add provider key
                StringBuilder.Append(@";provider=");
                // Add provider name
                StringBuilder.Append(providerName);
            }
            // Check provider connection string
            if (providerConnectionString != null)
            {
                // Add provider connection string key
                StringBuilder.Append(@";provider connection string=");
                // Add quotation mark
                StringBuilder.Append(@"""");
                // Add provider name
                StringBuilder.Append(providerConnectionString);
                // Check concluding semicolon
                if (!providerConnectionString.EndsWith(";"))
                {
                    // Add provider name
                    StringBuilder.Append(@";");
                }
                // Add quotation mark
                StringBuilder.Append(@"""");
            }

            // Return
            return StringBuilder.ToString();
        }
      
        #endregion

        #region My methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyFullName"></param>
        /// <returns></returns>
        private static string MyDeliverResourceMark(string assemblyFullName)
        {
            // Check assembly full name
            if (assemblyFullName == null)
            {
                // Set wildcard
                assemblyFullName = @"*";
            }
            // Set complete
            return (@"res://" + assemblyFullName + @"/");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="assemblyFullName"></param>
        /// <param name="folderName"></param>
        /// <param name="edmxName"></param>
        public static void DeliverMetaDataPath(System.Text.StringBuilder stringBuilder, string assemblyFullName, string folderName, string edmxName)
        {
            string ResourceMark;
            string FolderNameAndEdmxName = null;

            // Get resource mark
            ResourceMark = MyDeliverResourceMark(assemblyFullName);

            // Append quotation mark
            stringBuilder.Append(@"""");
            // Check edmx name
            if (edmxName != null)
            {
                // Check folder name
                if (folderName != null)
                {
                    // Combine
                    FolderNameAndEdmxName = (folderName + @"." + edmxName);
                }
                else
                {
                    // Single
                    FolderNameAndEdmxName = edmxName;
                }
                // Append csdl
                MyAppendSingleResourceFileToStringBuilder(stringBuilder, ResourceMark, FolderNameAndEdmxName, "csdl");
                // Append slash
                stringBuilder.Append(@"|");
                // Append csdl
                MyAppendSingleResourceFileToStringBuilder(stringBuilder, ResourceMark, FolderNameAndEdmxName, "ssdl");
                // Append slash
                stringBuilder.Append(@"|");
                // Append csdl
                MyAppendSingleResourceFileToStringBuilder(stringBuilder, ResourceMark, FolderNameAndEdmxName, "msl");
            }
            else
            {
                // Append one for all
                stringBuilder.Append(ResourceMark);
            }
            // Append quotation mark
            stringBuilder.Append(@"""");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="resourceMark"></param>
        /// <param name="folderNameAndEdmxName"></param>
        /// <param name="suffix"></param>
        private static void MyAppendSingleResourceFileToStringBuilder(System.Text.StringBuilder stringBuilder, string resourceMark, string folderNameAndEdmxName, string suffix)
        {
            // Append resource mark
            stringBuilder.Append(resourceMark);
            // Append folder name and edmx name
            stringBuilder.Append(folderNameAndEdmxName);
            // Append dot
            stringBuilder.Append(".");
            // Append suffix
            stringBuilder.Append(suffix);
        }
        #endregion
    }
}
