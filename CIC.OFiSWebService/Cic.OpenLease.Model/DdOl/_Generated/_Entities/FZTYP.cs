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
    public partial class FZTYP
    {
    
        #region Private variables
        private static Cic.Basic.Data.Objects.Property.InfoPackageDictionary _InfoPackageDictionary;
        #endregion

        #region Methods
        public static Cic.Basic.Data.Objects.Property.InfoPackageDictionary DeliverInfoPackageDictionary()
        {
            if (_InfoPackageDictionary == null)
            {
                _InfoPackageDictionary = new Cic.Basic.Data.Objects.Property.InfoPackageDictionary(typeof(FZTYP));
            }

            return _InfoPackageDictionary;
        }
        #endregion
        
        public partial struct FieldNames
        {
            #region Properties
            public static string SYSFZTYP
            {
                get { return "SYSFZTYP"; }
            }

            public static string HUBRAUM
            {
                get { return "HUBRAUM"; }
            }

            public static string LEISTUNG
            {
                get { return "LEISTUNG"; }
            }

            public static string MART
            {
                get { return "MART"; }
            }

            public static string GART
            {
                get { return "GART"; }
            }

            public static string NOVA
            {
                get { return "NOVA"; }
            }

            public static string GRUND
            {
                get { return "GRUND"; }
            }

            public static string VERBRAUCH
            {
                get { return "VERBRAUCH"; }
            }

            public static string CO2EMI
            {
                get { return "CO2EMI"; }
            }

            public static string NOX
            {
                get { return "NOX"; }
            }

            public static string PARTIKEL
            {
                get { return "PARTIKEL"; }
            }


			#endregion
        }

    }
}