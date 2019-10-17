using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.PST {
    using System.Xml.Serialization;


    /// <remarks/>
    /// manually added for the webservices substructure
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class OutputDataWebServices : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string anwendungField;

        private string programmPfadField;

        private string wbm_webmethodnameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Anwendung
        {
            get
            {
                return this.anwendungField;
            }
            set
            {
                this.anwendungField = value;
                this.RaisePropertyChanged("Anwendung");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ProgrammPfad
        {
            get
            {
                return this.programmPfadField;
            }
            set
            {
                this.programmPfadField = value;
                this.RaisePropertyChanged("ProgrammPfad");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string WBM_WebMethodName
        {
            get
            {
                return this.wbm_webmethodnameField;
            }
            set
            {
                this.wbm_webmethodnameField = value;
                this.RaisePropertyChanged("WBM_WebMethodName");
            }
        }



        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

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