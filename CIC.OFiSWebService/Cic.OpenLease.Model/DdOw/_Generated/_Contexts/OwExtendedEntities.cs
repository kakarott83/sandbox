// AUTOGENERATED, 16.10.2009 12:05:18
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Cic.OpenOne.Common.Util.Config;
using System;
using System.Data;
namespace Cic.OpenLease.Model.DdOw
{
    [System.CodeDom.Compiler.GeneratedCode("Cic.OpenLease.CodeGenerator", "1.0.0.0")]
    [System.CLSCompliant(true)]
    public partial class OwExtendedEntities : OwEntities
    {
    	
       
        public OwExtendedEntities() :
            base(ConnectionStringBuilder.DeliverConnectionString(typeof (OwEntities), "DdOw"))
            
        {
        }
        public System.Data.EntityKey getEntityKey(System.Type type, long value)
        {
            System.Data.Metadata.Edm.EntityType etype = MyGetEntityType(this, type);

            return new System.Data.EntityKey(this.DefaultContainerName + "." + etype.Name, etype.KeyMembers[0].Name, value);

        }

        private static System.Data.Metadata.Edm.EntityType MyGetEntityType(System.Data.Objects.ObjectContext context, System.Type type)
        {
            return context.MetadataWorkspace.GetType(type.Name, type.Namespace, System.Data.Metadata.Edm.DataSpace.CSpace) as System.Data.Metadata.Edm.EntityType;
        }

        public static long getKey(System.Data.EntityKey key)
        {
            if (key == null) return 0;
            EntityKeyMember[] member = key.EntityKeyValues;
            if (member == null || member.Length == 0) return 0;
            return (long)member[0].Value;
        }

        public static String getKeyName(System.Data.EntityKey key)
        {
            if (key == null) return null;
            EntityKeyMember[] member = key.EntityKeyValues;
            if (member == null || member.Length == 0) return null;
            return member[0].Key;
        }
      
    }
}