// AUTOGENERATED, 26.01.2010 17:01:13
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace Cic.OpenLease.Model.DdEurotax
{
    [System.CodeDom.Compiler.GeneratedCode("Cic.OpenLease.CodeGenerator", "1.0.0.0")]
    [System.CLSCompliant(true)]
    public partial class ETGEXCLUDE
    {
    
        #region Private variables
        private static Cic.Basic.Data.Objects.Property.InfoPackageDictionary _InfoPackageDictionary;
        #endregion

        #region Methods
        public static Cic.Basic.Data.Objects.Property.InfoPackageDictionary DeliverInfoPackageDictionary()
        {
            if (_InfoPackageDictionary == null)
            {
                _InfoPackageDictionary = new Cic.Basic.Data.Objects.Property.InfoPackageDictionary(typeof(ETGEXCLUDE));
            }

            return _InfoPackageDictionary;
        }
        #endregion
        
        public partial struct FieldNames
        {
            #region Properties
            public static string MARKET
            {
                get { return "MARKET"; }
            }

            public static string VEHTYPE
            {
                get { return "VEHTYPE"; }
            }

            public static string NATCODE
            {
                get { return "NATCODE"; }
            }

            public static string ADDCD
            {
                get { return "ADDCD"; }
            }

            public static string EQTCODECD
            {
                get { return "EQTCODECD"; }
            }

            public static string VAL
            {
                get { return "VAL"; }
            }

            public static string VALUNTIL
            {
                get { return "VALUNTIL"; }
            }

            public static string CURRENCY
            {
                get { return "CURRENCY"; }
            }

            public static string PRICE1
            {
                get { return "PRICE1"; }
            }

            public static string PRICE2
            {
                get { return "PRICE2"; }
            }

            public static string TAXRT
            {
                get { return "TAXRT"; }
            }

            public static string TAX1
            {
                get { return "TAX1"; }
            }

            public static string TAX2
            {
                get { return "TAX2"; }
            }

            public static string NET
            {
                get { return "NET"; }
            }

            public static string FLAG
            {
                get { return "FLAG"; }
            }

            public static string TARGETGRP
            {
                get { return "TARGETGRP"; }
            }

            public static string RECSTATUS
            {
                get { return "RECSTATUS"; }
            }

            public static string RECDATE
            {
                get { return "RECDATE"; }
            }


			#endregion
        }

    }
}