using System;
using System.Reflection;
using Cic.OpenOne.Common.Util.Collection;
using System.ComponentModel;

namespace Cic.OpenOne.Common.Util
{
    /// <summary>
    /// StringValueAttribute-Klasse
    /// </summary>
    public class StringValueAttribute : System.Attribute
    {
        private string _value;

        /// <summary>
        /// StringValueAttribute
        /// </summary>
        /// <param name="value"></param>
        public StringValueAttribute(string value)
        {
            _value = value;
        }

        /// <summary>
        /// Value
        /// </summary>
        public string Value
        {
            get { return _value; }
        }
    }

    /// <summary>
    /// EnumUtil-Klasse
    /// </summary>
    public class EnumUtil
    {
        private static ThreadSafeDictionary<Enum, StringValueAttribute> _stringValues = new ThreadSafeDictionary<Enum, StringValueAttribute>();

  

        /// <summary>
        /// GetStringValue
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetStringValue(Enum value)
        {
            string output = null;
            Type type = value.GetType();

            //Check first in our cached results...

            if (_stringValues.ContainsKey(value))
                output = (_stringValues[value] as StringValueAttribute).Value;
            else
            {
                // Look for our 'StringValueAttribute' 
                // in the field's custom attributes
                FieldInfo fi = type.GetField(value.ToString());
                StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
                if (attrs.Length > 0)
                {
                    _stringValues.MergeSafe(value, attrs[0]);
                    output = attrs[0].Value;
                }
            }
            return output;
        }

        /// <summary>
        /// Returns an indication whether a constant with a specified value is not null and exists in a specified enumeration.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <param name="value">Value.</param>
        /// <returns>
        ///     <c>true</c> if the specified value is not null and exsists in a specified enumeration ; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNotNullAndIsDefined(System.Type enumType, object value)
        {
            // Check enum type
            if (enumType == null)
            {
                // Throw exception
                throw new Exception("enumType");
            }
            // Check type
            if (enumType.BaseType != typeof(System.Enum))
            {
                // Throw exception
                throw new Exception("enumType");
            }

            return (value != null && System.Enum.IsDefined(enumType, value));
        }

        /// <summary>
        /// Returns value if is not null and exists in a specified enumeration, otherwise, default value.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <param name="value">Value.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>
        ///     value if the specified value is not null and exsists in a specified enumeration ; otherwise, default value.
        /// </returns>
        public static object DeliverDefinedOrDefault(System.Type enumType, object value, object defaultValue)
        {
            if (IsNotNullAndIsDefined(enumType, value))
            {
                // Defined
                return value;
            }
            else
            {
                // Default
                return defaultValue;
            }
        }
    }
}