using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WfvXmlConfigurator.BO.ContentLogics
{
    internal class PropertyAttributeAdder<T> : CustomTypeDescriptor where T : class
    {
        /// <summary>
        /// For a property name, the property descriptor containing the additional attributes is defined
        /// </summary>
        private Dictionary<string, PropertyDescriptor> OverwrittenProperties = new Dictionary<string,PropertyDescriptor>();
        private T SpecificInstance = null;

        /// <summary>
        /// Helper for dynamically adding property attributes to properties
        /// </summary>
        /// <param name="instance">only change the property attributes of this specific object, not the whole type</param>
        internal PropertyAttributeAdder (T instance = null)
            : base(instance == null ? TypeDescriptor.GetProvider(typeof(T)).GetTypeDescriptor(typeof(T)) : TypeDescriptor.GetProvider(instance).GetTypeDescriptor(instance))
        {
            SpecificInstance = instance;
        }

        public override PropertyDescriptorCollection GetProperties()
        {
            return GetOverwrittenProperties(base.GetProperties());
        }
        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return GetOverwrittenProperties(base.GetProperties(attributes));
        }

        /// <summary>
        /// Add property attributes to the class
        /// </summary>
        /// <param name="attributes">additional property attributes</param>
        internal void AddAttributes(params Attribute[] attributes)
        {
            if (SpecificInstance == null)
                TypeDescriptor.AddAttributes(typeof(T), attributes);
            else
                TypeDescriptor.AddAttributes(SpecificInstance, attributes);
        }

        /// <summary>
        /// Add property attributes to the property with the given name
        /// </summary>
        /// <param name="propertyName">name of property which shall get the additional attributes</param>
        /// <param name="attributes">additional attributes for the property</param>
        internal void AddAttributes(string propertyName, params Attribute[] attributes)
        {
            PropertyDescriptor propDesc = null;
            if (OverwrittenProperties.ContainsKey(propertyName))
                propDesc = OverwrittenProperties[propertyName];
            else
                propDesc = SpecificInstance == null ? TypeDescriptor.GetProperties(typeof(T))[propertyName] : TypeDescriptor.GetProperties(SpecificInstance)[propertyName];

            if (propDesc == null)
                return; //If there is no such property, there is nowhere to add the attributes

            PropertyDescriptor propDescNew = TypeDescriptor.CreateProperty(typeof(T), propDesc, attributes);
            OverwrittenProperties[propertyName] = propDescNew;
        }

        /// <summary>
        /// Tell the system that the given type has additional attributes
        /// </summary>
        internal void ProvideAttributes()
        {
            if (SpecificInstance == null)
                TypeDescriptor.AddProvider(new CustomTypeDescriptionProvider(this), typeof(T));
            else
                TypeDescriptor.AddProvider(new CustomTypeDescriptionProvider(this), SpecificInstance);
        }


        /// <summary>
        /// Based on default properties, get the additional attributes along with the default properties
        /// </summary>
        /// <param name="baseCollection">default properties</param>
        /// <returns></returns>
        private PropertyDescriptorCollection GetOverwrittenProperties(PropertyDescriptorCollection baseCollection)
        {
            List<PropertyDescriptor> properties = new List<PropertyDescriptor>();
            foreach (PropertyDescriptor propDesc in baseCollection)
            {
                if (OverwrittenProperties.ContainsKey(propDesc.Name))
                    properties.Add(OverwrittenProperties[propDesc.Name]);
                else
                    properties.Add(propDesc);
            }
            return new PropertyDescriptorCollection(properties.ToArray());
        }

    }
}
