using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Cic.OpenOne.GateBANKNOW.TestUI.DTOS
{
    /// <summary>
    /// Notify Property Changed 
    /// </summary>
    public class DTO : INotifyPropertyChanged
    {
        private string _typeAttribute;
        private string _nameAttribute;
        private string _valueAttribute;
        private object _objectAttribute;

        /// <summary>
        /// Declaration Property Changed Event Handler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Type of Attribute
        /// </summary>
        public string typeAttribute
        {
            get { return _typeAttribute; }
            set
            {
                _typeAttribute = value;
                Changed("typeAttribute");
            }
        }

        /// <summary>
        /// Name of Attribute
        /// </summary>
        public string nameAttribute
        {
            get { return _nameAttribute; }
            set
            {
                _nameAttribute = value;
                Changed("nameAttribute");
            }
        }

        /// <summary>
        /// Value Attribute
        /// </summary>
        public string valueAttribute
        {
            get { return _valueAttribute; }
            set
            {
                _valueAttribute = value;
                Changed("valueAttribute");
            }
        }

        /// <summary>
        /// Object of Attribute
        /// </summary>
        public object objectAttribute
        {
            get { return _objectAttribute; }
            set
            { 
                _objectAttribute = value;
                Changed("objectAttribute");
            }
        }

        /// <summary>
        /// Changed Call
        /// </summary>
        /// <param name="propertyName"></param>
        private void Changed(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

      
        
    }
}

