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
    public partial class SUBVTYP
    {
    
        #region Private variables
        private static Cic.Basic.Data.Objects.Property.InfoPackageDictionary _InfoPackageDictionary;
        #endregion

        #region Methods
        public static Cic.Basic.Data.Objects.Property.InfoPackageDictionary DeliverInfoPackageDictionary()
        {
            if (_InfoPackageDictionary == null)
            {
                _InfoPackageDictionary = new Cic.Basic.Data.Objects.Property.InfoPackageDictionary(typeof(SUBVTYP));
            }

            return _InfoPackageDictionary;
        }
        #endregion
        
        public partial struct FieldNames
        {
            #region Properties
            public static string SYSSUBVTYP
            {
                get { return "SYSSUBVTYP"; }
            }

            public static string BEZEICHNUNG
            {
                get { return "BEZEICHNUNG"; }
            }

            public static string EVALGUELTIGAB
            {
                get { return "EVALGUELTIGAB"; }
            }

            public static string EVALGUELTIGVON
            {
                get { return "EVALGUELTIGVON"; }
            }

            public static string EVALFAKTORREL
            {
                get { return "EVALFAKTORREL"; }
            }

            public static string EVALFAKTORABS
            {
                get { return "EVALFAKTORABS"; }
            }

            public static string EVALBEGINN
            {
                get { return "EVALBEGINN"; }
            }

            public static string EVALLZ
            {
                get { return "EVALLZ"; }
            }

            public static string SYSSUBVG
            {
                get { return "SYSSUBVG"; }
            }

            public static string EVALSYSSUBVG
            {
                get { return "EVALSYSSUBVG"; }
            }

            public static string SYSFT
            {
                get { return "SYSFT"; }
            }

            public static string EVALSYSFT
            {
                get { return "EVALSYSFT"; }
            }


			#endregion
        }

    }
}