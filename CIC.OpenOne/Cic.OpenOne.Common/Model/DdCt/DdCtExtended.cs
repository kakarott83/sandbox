using System;
using System.Data;
using Cic.OpenOne.Common.Util.Config;

namespace Cic.OpenOne.Common.Model.DdCt
{
    /// <summary>
    /// Extension of generated EDMX to use the configured database connection
    /// </summary>
    public class DdCtExtended : CtEntities
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public DdCtExtended()
            : base(ConnectionStringBuilder.DeliverConnectionString(typeof(CtEntities), "DdCt"))
        {
        }

        /// <summary>
        /// getKey
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long getKey(System.Data.EntityKey key)
        {
            if (key == null) return 0;
            EntityKeyMember[] member = key.EntityKeyValues;
            if (member == null || member.Length == 0) return 0;
            return (long)member[0].Value;
        }

        /// <summary>
        /// getKeyName
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public String getKeyName(System.Data.EntityKey key)
        {
            if (key == null) return null;
            EntityKeyMember[] member = key.EntityKeyValues;
            if (member == null || member.Length == 0) return null;
            return member[0].Key;
        }

        /// <summary>
        /// getEntityKey
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public System.Data.EntityKey getEntityKey(System.Type type, long value)
        {
            System.Data.Metadata.Edm.EntityType etype = MyGetEntityType(this, type);

            return new System.Data.EntityKey(this.DefaultContainerName + "." + etype.Name, etype.KeyMembers[0].Name, value);
        }

        private static System.Data.Metadata.Edm.EntityType MyGetEntityType(System.Data.Objects.ObjectContext context, System.Type type)
        {
            return context.MetadataWorkspace.GetType(type.Name, type.Namespace, System.Data.Metadata.Edm.DataSpace.CSpace) as System.Data.Metadata.Edm.EntityType;
        }
    }
}