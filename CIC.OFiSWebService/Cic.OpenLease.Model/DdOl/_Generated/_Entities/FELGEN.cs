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
    public partial class FELGEN
    {
    
        #region Private variables
        private static Cic.Basic.Data.Objects.Property.InfoPackageDictionary _InfoPackageDictionary;
        #endregion

        #region Methods
        public static Cic.Basic.Data.Objects.Property.InfoPackageDictionary DeliverInfoPackageDictionary()
        {
            if (_InfoPackageDictionary == null)
            {
                _InfoPackageDictionary = new Cic.Basic.Data.Objects.Property.InfoPackageDictionary(typeof(FELGEN));
            }

            return _InfoPackageDictionary;
        }
        #endregion
        
        public partial struct FieldNames
        {
            #region Properties
            public static string SYSFELGEN
            {
                get { return "SYSFELGEN"; }
            }

            public static string BEZEICHNUNG
            {
                get { return "BEZEICHNUNG"; }
            }

            public static string BESCHREIBUNG
            {
                get { return "BESCHREIBUNG"; }
            }

            public static string HERSTELLER
            {
                get { return "HERSTELLER"; }
            }

            public static string FLAGLM
            {
                get { return "FLAGLM"; }
            }

            public static string ARTIKELNR
            {
                get { return "ARTIKELNR"; }
            }

            public static string EAN
            {
                get { return "EAN"; }
            }

            public static string TEILE
            {
                get { return "TEILE"; }
            }

            public static string PREIS
            {
                get { return "PREIS"; }
            }

            public static string PREISKOMPLETT
            {
                get { return "PREISKOMPLETT"; }
            }

            public static string PREISKOMPLETTP
            {
                get { return "PREISKOMPLETTP"; }
            }

            public static string SYSFELGTYP
            {
                get { return "SYSFELGTYP"; }
            }


			#endregion
        }

    }
}