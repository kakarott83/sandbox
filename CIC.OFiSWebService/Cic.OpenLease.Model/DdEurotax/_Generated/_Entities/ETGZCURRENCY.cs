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
    public partial class ETGZCURRENCY
    {
    
        #region Private variables
        private static Cic.Basic.Data.Objects.Property.InfoPackageDictionary _InfoPackageDictionary;
        #endregion

        #region Methods
        public static Cic.Basic.Data.Objects.Property.InfoPackageDictionary DeliverInfoPackageDictionary()
        {
            if (_InfoPackageDictionary == null)
            {
                _InfoPackageDictionary = new Cic.Basic.Data.Objects.Property.InfoPackageDictionary(typeof(ETGZCURRENCY));
            }

            return _InfoPackageDictionary;
        }
        #endregion
        
        public partial struct FieldNames
        {
            #region Properties
            public static string CURRENCY
            {
                get { return "CURRENCY"; }
            }

            public static string LANGCODE
            {
                get { return "LANGCODE"; }
            }

            public static string CURDESCRSHORT
            {
                get { return "CURDESCRSHORT"; }
            }

            public static string CURDESCR
            {
                get { return "CURDESCR"; }
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