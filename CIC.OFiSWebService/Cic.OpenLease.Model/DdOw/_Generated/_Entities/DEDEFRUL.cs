// AUTOGENERATED, 16.10.2009 12:05:17
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace Cic.OpenLease.Model.DdOw
{
    [System.CodeDom.Compiler.GeneratedCode("Cic.OpenLease.CodeGenerator", "1.0.0.0")]
    [System.CLSCompliant(true)]
    public partial class DEDEFRUL
    {
    
        #region Private variables
        private static Cic.Basic.Data.Objects.Property.InfoPackageDictionary _InfoPackageDictionary;
        #endregion

        #region Methods
        public static Cic.Basic.Data.Objects.Property.InfoPackageDictionary DeliverInfoPackageDictionary()
        {
            if (_InfoPackageDictionary == null)
            {
                _InfoPackageDictionary = new Cic.Basic.Data.Objects.Property.InfoPackageDictionary(typeof(DEDEFRUL));
            }

            return _InfoPackageDictionary;
        }
        #endregion
        
        public partial struct FieldNames
        {
            #region Properties
            public static string SYSDEDEFRUL
            {
                get { return "SYSDEDEFRUL"; }
            }

            public static string EXTERNCODE
            {
                get { return "EXTERNCODE"; }
            }

            public static string NAME
            {
                get { return "NAME"; }
            }

            public static string DEFTEXT
            {
                get { return "DEFTEXT"; }
            }

            public static string DISPLAYTYPE
            {
                get { return "DISPLAYTYPE"; }
            }


			#endregion
        }

    }
}