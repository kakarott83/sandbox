using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;
using System.Xml.Serialization;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SHS.W002
{
    /// <summary>
    /// This Helper Class must be in the same namespace as the generated reference.cs
    /// it manages to set the "xySpecified" Fields automatically when the value gets assigned 
    /// 
    /// How it works:
    /// Every Class in the Contract is a subclass of ValidatableDataContract - replace object, INotifyPropertyChanged with ValidatableDataContract
    /// 
    /// remove:  public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    /// remove method protected void RaisePropertyChanged(string propertyName)
    /// 
    /// 
    /// NOTE: replace [][] for ubo with [], thats generated wrongly by the tool
    /// </summary>
    public partial class ValidatableDataContract : object, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ValidatableDataContract()
        {
            this.PropertyChanged += myPropertyChanged;
        }

        public void myPropertyChanged(object obj, PropertyChangedEventArgs target)
        {
            PropertyInfo prop = obj.GetType().GetProperty(
                string.Format("{0}{1}", target.PropertyName, "Specified"),
                BindingFlags.Public | BindingFlags.Instance);

            if (null != prop && prop.CanWrite)
            {
                prop.SetValue(obj, true, null);
            }
        }
        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

}


namespace Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference
{
    /// <summary>
    /// This Helper Class must be in the same namespace as the generated reference.cs
    /// it manages to set the "xySpecified" Fields automatically when the value gets assigned 
    /// 
    /// How it works:
    /// Every Class in the Contract is a subclass of ValidatableDataContract - replace object, INotifyPropertyChanged with ValidatableDataContract
    /// 
    /// remove:  public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    /// remove method protected void RaisePropertyChanged(string propertyName)
    /// 
    /// 
    /// NOTE: replace [][] for ubo with [], thats generated wrongly by the tool
    /// </summary>
    public partial class ValidatableDataContract : object, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ValidatableDataContract()
        {
            this.PropertyChanged += myPropertyChanged;
        }

        public void myPropertyChanged(object obj, PropertyChangedEventArgs target)
        {
            PropertyInfo prop = obj.GetType().GetProperty(
                string.Format("{0}{1}", target.PropertyName, "Specified"),
                BindingFlags.Public | BindingFlags.Instance);

            if (null != prop && prop.CanWrite)
            {
                prop.SetValue(obj, true, null);
            }
        }
        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                bool executePropertyChanged = true;
                var prop = this.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                if (prop.PropertyType == typeof(DateTime))
                {
                    var value = (DateTime)prop.GetValue(this);
                    if (value == DateTime.MinValue || value == new DateTime(0111, 1, 1))
                    {
                        executePropertyChanged = false;
                    }
                }

                if (executePropertyChanged)
                {
                    propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                }
            }
        }
    }
}

namespace Cic.OpenOne.GateBANKNOW.Common.GuardeanServiceReference
{
    /// <summary>
    /// This Helper Class must be in the same namespace as the generated reference.cs
    /// it manages to set the "xySpecified" Fields automatically when the value gets assigned 
    /// 
    /// How it works:
    /// Every Class in the Contract is a subclass of ValidatableDataContract - replace object, INotifyPropertyChanged with ValidatableDataContract
    /// 
    /// remove:  public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    /// remove method protected void RaisePropertyChanged(string propertyName)
    /// 
    /// 
    /// NOTE: replace [][] for ubo with [], thats generated wrongly by the tool
    /// </summary>
    public partial class ValidatableDataContract : object, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ValidatableDataContract()
        {
            this.PropertyChanged += myPropertyChanged;
        }

        public void myPropertyChanged(object obj, PropertyChangedEventArgs target)
        {
            PropertyInfo prop = obj.GetType().GetProperty(
                string.Format("{0}{1}", target.PropertyName, "Specified"),
                BindingFlags.Public | BindingFlags.Instance);

            if (null != prop && prop.CanWrite)
            {
                prop.SetValue(obj, true, null);
            }
        }
        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                bool executePropertyChanged = true;
                var prop = this.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                if (prop.PropertyType == typeof(DateTime))
                {
                    var value = (DateTime) prop.GetValue(this);
                    if (value == DateTime.MinValue || value == new DateTime(0111, 1, 1))
                    {
                        executePropertyChanged = false;
                    }
                }

                if (executePropertyChanged)
                {
                    propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                }
            }
        }
    }
}

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SHS.W003
{
    public partial class ValidatableDataContract : object, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ValidatableDataContract()
        {
            this.PropertyChanged += myPropertyChanged;
        }

        public void myPropertyChanged(object obj, PropertyChangedEventArgs target)
        {
            PropertyInfo prop = obj.GetType().GetProperty(
                string.Format("{0}{1}", target.PropertyName, "Specified"),
                BindingFlags.Public | BindingFlags.Instance);

            if (null != prop && prop.CanWrite)
            {
                prop.SetValue(obj, true, null);
            }
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                bool executePropertyChanged = true;
                var prop = this.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                if (prop.PropertyType == typeof(DateTime))
                {
                    var value = (DateTime)prop.GetValue(this);
                    if (value == DateTime.MinValue || value == new DateTime(0111, 1, 1))
                    {
                        executePropertyChanged = false;
                    }
                }

                if (executePropertyChanged)
                {
                    propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                }
            }
        }
    }

}
namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SHS.W006
{
    public partial class ValidatableDataContract : object, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ValidatableDataContract()
        {
            this.PropertyChanged += myPropertyChanged;
        }

        public void myPropertyChanged(object obj, PropertyChangedEventArgs target)
        {
            PropertyInfo prop = obj.GetType().GetProperty(
                string.Format("{0}{1}", target.PropertyName, "Specified"),
                BindingFlags.Public | BindingFlags.Instance);

            if (null != prop && prop.CanWrite)
            {
                prop.SetValue(obj, true, null);
            }
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                bool executePropertyChanged = true;
                var prop = this.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                if (prop.PropertyType == typeof(DateTime))
                {
                    var value = (DateTime)prop.GetValue(this);
                    if (value == DateTime.MinValue || value == new DateTime(0111, 1, 1))
                    {
                        executePropertyChanged = false;
                    }
                }

                if (executePropertyChanged)
                {
                    propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                }
            }
        }
    }
}

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SHS.W007
{
    public partial class ValidatableDataContract : object, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ValidatableDataContract()
        {
            this.PropertyChanged += myPropertyChanged;
        }

        public void myPropertyChanged(object obj, PropertyChangedEventArgs target)
        {
            PropertyInfo prop = obj.GetType().GetProperty(
                string.Format("{0}{1}", target.PropertyName, "Specified"),
                BindingFlags.Public | BindingFlags.Instance);

            if (null != prop && prop.CanWrite)
            {
                prop.SetValue(obj, true, null);
            }
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                bool executePropertyChanged = true;
                var prop = this.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                if (prop.PropertyType == typeof(DateTime))
                {
                    var value = (DateTime)prop.GetValue(this);
                    if (value == DateTime.MinValue || value == new DateTime(0111, 1, 1))
                    {
                        executePropertyChanged = false;
                    }
                }

                if (executePropertyChanged)
                {
                    propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                }
            }
        }
    }
}