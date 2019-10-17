using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.Common.DAO
{
    using System.Data.EntityClient;

    using Util.Config;

    public interface IContextFactory
    {
        T Create<T>();
    }

    public class ContextFactory : IContextFactory
    {
        public T Create<T>()
        {
            var connectionString = GetConnectionString(typeof(T), Configuration.DeliverOpenLeaseConnectionString());
            return (T)Activator.CreateInstance(typeof(T), connectionString);
        }

        private string GetConnectionString(Type contextType, string devartConnectionString)
        {
            var assemblyName = contextType.Assembly.GetName().Name;
            if (contextType.Namespace == null)
            {
                throw new ArgumentException("The namespace of the contextType can't be null.");
            }

            var folder = contextType.Namespace.Substring(assemblyName.Length + 1);
            var edmx = contextType.Name.EndsWith("Context") ? contextType.Name.Substring(0, contextType.Name.Length - 7) : contextType.Name;
            var entityBuilder = new EntityConnectionStringBuilder
            {
                Provider = Cic.OpenOne.Common.Properties.Config.Default.DatasourceProviderName,
                ProviderConnectionString = devartConnectionString,
                Metadata = string.Format("res://*/{0}.{1}.csdl|res://*/{0}.{1}.ssdl|res://*/{0}.{1}.msl", folder, edmx),
            };
            var connectionString = entityBuilder.ToString();
            return connectionString;
        }
    }
}
