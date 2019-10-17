using System;
using System.ComponentModel;

namespace XmlConfiguratorBase.BO.ContentLogics
{
    /// <summary>
    /// type description provider for custom types, needed for dynamically adding property attributes
    /// </summary>
    internal class CustomTypeDescriptionProvider : TypeDescriptionProvider
    {
        private readonly ICustomTypeDescriptor TypeDescriptor;

        internal CustomTypeDescriptionProvider (ICustomTypeDescriptor customDescriptor)
        {
            TypeDescriptor = customDescriptor;
        }

        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            return TypeDescriptor;
        }
    }
}
