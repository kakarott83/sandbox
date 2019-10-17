using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.One.DTO
{

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class einrichtung
    {

        private einrichtungGrp grpField;

        private einrichtungQ1rec[] q1Field;

        private object q2Field;

        private einrichtungWpf wpfField;

        /// <remarks/>
        public einrichtungGrp grp
        {
            get
            {
                return this.grpField;
            }
            set
            {
                this.grpField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("q1rec", IsNullable = false)]
        public einrichtungQ1rec[] q1
        {
            get
            {
                return this.q1Field;
            }
            set
            {
                this.q1Field = value;
            }
        }

        /// <remarks/>
        public object q2
        {
            get
            {
                return this.q2Field;
            }
            set
            {
                this.q2Field = value;
            }
        }

        /// <remarks/>
        public einrichtungWpf Wpf
        {
            get
            {
                return this.wpfField;
            }
            set
            {
                this.wpfField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class einrichtungGrp
    {

        private string titelField;

        private string queryField;

        private string pb1textField;

        private object pb1iconField;

        private string pb1actionField;

        private object pb1tooltipField;

        private int pb1wfexecField;

        private int pb2wfexecField;

        private int pb3wfexecField;

        private string aktioneditField;

        private int sortierenField;

        private int absteigendField;

        private int editreloadField;

        private int insertreloadField;

        private int deletereloadField;

        private int pb1reloadField;

        private int pb2reloadField;

        private int pb3reloadField;

        private int deftabField;

        private int excelField;

        public string p1 { get; set; }
        public string p2 { get; set; }
        public string p3 { get; set; }
        public string p4 { get; set; }
        public string p5 { get; set; }
        public string p6 { get; set; }
        public string idausdruck { get; set; }
        public string aktioninsert { get; set; }
        public string aktiondelete { get; set; }
        /// <remarks/>
        public string titel
        {
            get
            {
                return this.titelField;
            }
            set
            {
                this.titelField = value;
            }
        }

        /// <remarks/>
        public string query
        {
            get
            {
                return this.queryField;
            }
            set
            {
                this.queryField = value;
            }
        }

        /// <remarks/>
        public string pb1text
        {
            get
            {
                return this.pb1textField;
            }
            set
            {
                this.pb1textField = value;
            }
        }

        /// <remarks/>
        public object pb1icon
        {
            get
            {
                return this.pb1iconField;
            }
            set
            {
                this.pb1iconField = value;
            }
        }

        /// <remarks/>
        public string pb1action
        {
            get
            {
                return this.pb1actionField;
            }
            set
            {
                this.pb1actionField = value;
            }
        }

        /// <remarks/>
        public object pb1tooltip
        {
            get
            {
                return this.pb1tooltipField;
            }
            set
            {
                this.pb1tooltipField = value;
            }
        }

        /// <remarks/>
        public int pb1wfexec
        {
            get
            {
                return this.pb1wfexecField;
            }
            set
            {
                this.pb1wfexecField = value;
            }
        }

        /// <remarks/>
        public int pb2wfexec
        {
            get
            {
                return this.pb2wfexecField;
            }
            set
            {
                this.pb2wfexecField = value;
            }
        }

        /// <remarks/>
        public int pb3wfexec
        {
            get
            {
                return this.pb3wfexecField;
            }
            set
            {
                this.pb3wfexecField = value;
            }
        }

        /// <remarks/>
        public string aktionedit
        {
            get
            {
                return this.aktioneditField;
            }
            set
            {
                this.aktioneditField = value;
            }
        }

        /// <remarks/>
        public int sortieren
        {
            get
            {
                return this.sortierenField;
            }
            set
            {
                this.sortierenField = value;
            }
        }

        /// <remarks/>
        public int absteigend
        {
            get
            {
                return this.absteigendField;
            }
            set
            {
                this.absteigendField = value;
            }
        }

        /// <remarks/>
        public int editreload
        {
            get
            {
                return this.editreloadField;
            }
            set
            {
                this.editreloadField = value;
            }
        }

        /// <remarks/>
        public int insertreload
        {
            get
            {
                return this.insertreloadField;
            }
            set
            {
                this.insertreloadField = value;
            }
        }

        /// <remarks/>
        public int deletereload
        {
            get
            {
                return this.deletereloadField;
            }
            set
            {
                this.deletereloadField = value;
            }
        }

        /// <remarks/>
        public int pb1reload
        {
            get
            {
                return this.pb1reloadField;
            }
            set
            {
                this.pb1reloadField = value;
            }
        }

        /// <remarks/>
        public int pb2reload
        {
            get
            {
                return this.pb2reloadField;
            }
            set
            {
                this.pb2reloadField = value;
            }
        }

        /// <remarks/>
        public int pb3reload
        {
            get
            {
                return this.pb3reloadField;
            }
            set
            {
                this.pb3reloadField = value;
            }
        }

        /// <remarks/>
        public int deftab
        {
            get
            {
                return this.deftabField;
            }
            set
            {
                this.deftabField = value;
            }
        }

        /// <remarks/>
        public int excel
        {
            get
            {
                return this.excelField;
            }
            set
            {
                this.excelField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class einrichtungQ1rec
    {

        private int rangField;

        private string bezeichnungField;

        private int breiteField;

        private string justificationField;

        private string ausdruckField;

        private int lascheField;

        private int summenField;

        private string summenTypField;

        private string columntypeField;

        /// <remarks/>
        public int rang
        {
            get
            {
                return this.rangField;
            }
            set
            {
                this.rangField = value;
            }
        }

        /// <remarks/>
        public string bezeichnung
        {
            get
            {
                return this.bezeichnungField;
            }
            set
            {
                this.bezeichnungField = value;
            }
        }

        /// <remarks/>
        public int breite
        {
            get
            {
                return this.breiteField;
            }
            set
            {
                this.breiteField = value;
            }
        }

        /// <remarks/>
        public string justification
        {
            get
            {
                return this.justificationField;
            }
            set
            {
                this.justificationField = value;
            }
        }

        /// <remarks/>
        public string ausdruck
        {
            get
            {
                return this.ausdruckField;
            }
            set
            {
                this.ausdruckField = value;
            }
        }

        /// <remarks/>
        public int lasche
        {
            get
            {
                return this.lascheField;
            }
            set
            {
                this.lascheField = value;
            }
        }

        /// <remarks/>
        public int summen
        {
            get
            {
                return this.summenField;
            }
            set
            {
                this.summenField = value;
            }
        }

        /// <remarks/>
        public string summenTyp
        {
            get
            {
                return this.summenTypField;
            }
            set
            {
                this.summenTypField = value;
            }
        }

        /// <remarks/>
        public string columntype
        {
            get
            {
                return this.columntypeField;
            }
            set
            {
                this.columntypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class einrichtungWpf
    {

        private bool loadInMemoryField;

        private int pageSizeField;

        private object additionalButtonsField;

        private int iconColumnWidthField;

        /// <remarks/>
        public bool LoadInMemory
        {
            get
            {
                return this.loadInMemoryField;
            }
            set
            {
                this.loadInMemoryField = value;
            }
        }

        /// <remarks/>
        public int PageSize
        {
            get
            {
                return this.pageSizeField;
            }
            set
            {
                this.pageSizeField = value;
            }
        }

        /// <remarks/>
        public object AdditionalButtons
        {
            get
            {
                return this.additionalButtonsField;
            }
            set
            {
                this.additionalButtonsField = value;
            }
        }

        /// <remarks/>
        public int IconColumnWidth
        {
            get
            {
                return this.iconColumnWidthField;
            }
            set
            {
                this.iconColumnWidthField = value;
            }
        }
    }


}
