// AUTOGENERATED, 18.11.2010 15:27:37
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace Cic.OpenLease.Model.DdOl
{
    [System.CodeDom.Compiler.GeneratedCode("Cic.OpenLease.CodeGenerator", "1.0.0.0")]
    [System.CLSCompliant(true)]
    public partial class KONTO
    {
    
        #region Private variables
        private static Cic.Basic.Data.Objects.Property.InfoPackageDictionary _InfoPackageDictionary;
        #endregion

        #region Methods
        public static Cic.Basic.Data.Objects.Property.InfoPackageDictionary DeliverInfoPackageDictionary()
        {
            if (_InfoPackageDictionary == null)
            {
                _InfoPackageDictionary = new Cic.Basic.Data.Objects.Property.InfoPackageDictionary(typeof(KONTO));
            }

            return _InfoPackageDictionary;
        }
        #endregion
        
        public partial struct FieldNames
        {
            #region Properties
            public static string SYSKONTO
            {
                get { return "SYSKONTO"; }
            }

            public static string RANG
            {
                get { return "RANG"; }
            }

            public static string SYSBLZ
            {
                get { return "SYSBLZ"; }
            }

            public static string KONTONR
            {
                get { return "KONTONR"; }
            }

            public static string BEZEICHNUNG
            {
                get { return "BEZEICHNUNG"; }
            }

            public static string IBAN
            {
                get { return "IBAN"; }
            }

            public static string AKTIVKZ
            {
                get { return "AKTIVKZ"; }
            }


			#endregion
        }

    }
}