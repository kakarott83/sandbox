using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.ComponentModel;
using System.Reflection;

namespace Cic.OpenOne.GateBANKNOW.TestUI.DTOS
{
    /// <summary>
    /// Notify Property Changed Data Transfer Object
    /// </summary>
    public class NotifyPropertyChangedProxy : DynamicObject, INotifyPropertyChanged
    {
        /// <summary>
        /// Wrapped Object
        /// </summary>
        public object WrappedObject { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="wrappedObject">Wrapped Object</param>
        public NotifyPropertyChangedProxy(object wrappedObject)
        {
            if (wrappedObject == null)
                throw new ArgumentNullException("wrappedObject");

            WrappedObject = wrappedObject;
        }


        #region INotifyPropertyChanged support
        /// <summary>
        /// Property Changed Event
        /// </summary>
        /// <param name="propertyName">Property Name</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Declaration of Property Changed Event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        /// <summary>
        /// Get Dynamic Member Names
        /// </summary>
        /// <returns>Members</returns>
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return from f in WrappedObject.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                   select f.Name;
        }

        /// <summary>
        /// TryGet Member
        /// </summary>
        /// <param name="binder">binder</param>
        /// <param name="result">result</param>
        /// <returns>Success</returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            // Locate property by name
            var propertyInfo = WrappedObject.GetType().GetProperty(binder.Name, BindingFlags.Instance | BindingFlags.Public | (binder.IgnoreCase ? BindingFlags.IgnoreCase : 0));
            if (propertyInfo == null || !propertyInfo.CanRead)
            {
                result = null;
                return false;
            }

            result = propertyInfo.GetValue(WrappedObject, null);
            return true;
        }

        /// <summary>
        /// Try Set Member
        /// </summary>
        /// <param name="binder">binder</param>
        /// <param name="value">value</param>
        /// <returns>Success</returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            // Locate property by name
            var propertyInfo = WrappedObject.GetType().GetProperty(binder.Name, BindingFlags.Instance | BindingFlags.Public | (binder.IgnoreCase ? BindingFlags.IgnoreCase : 0));
            if (propertyInfo == null || !propertyInfo.CanWrite)
                return false;
            
            object newValue = value;
            // Check the types are compatible
            Type propertyType = propertyInfo.PropertyType;
            Type underlyingType = null;
            if (!propertyType.IsAssignableFrom(value.GetType()))
            {
               

                if (propertyType.IsGenericType && (propertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {

                    underlyingType = Nullable.GetUnderlyingType(propertyType);

                }

                else
                {

                    underlyingType = propertyType;

                }
                newValue = Convert.ChangeType(value, underlyingType);





            }

            propertyInfo.SetValue(WrappedObject, newValue, null);
            OnPropertyChanged(binder.Name);
            return true;
        }
    }
}
