// AUTOGENERATED, 18.11.2010 15:27:38
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
    public partial class PRCLPRFSSET
    {
    
        #region Private variables
        private static Cic.Basic.Data.Objects.Property.InfoPackageDictionary _InfoPackageDictionary;
        #endregion

        #region Methods
        public static Cic.Basic.Data.Objects.Property.InfoPackageDictionary DeliverInfoPackageDictionary()
        {
            if (_InfoPackageDictionary == null)
            {
                _InfoPackageDictionary = new Cic.Basic.Data.Objects.Property.InfoPackageDictionary(typeof(PRCLPRFSSET));
            }

            return _InfoPackageDictionary;
        }
        #endregion
        
        public partial struct FieldNames
        {
            #region Properties
            public static string SYSPRCLPRFSSET
            {
                get { return "SYSPRCLPRFSSET"; }
            }

            public static string NEEDEDFLAG
            {
                get { return "NEEDEDFLAG"; }
            }

            public static string DISABLEDFLAG
            {
                get { return "DISABLEDFLAG"; }
            }

            public static string MITFINFLAG
            {
                get { return "MITFINFLAG"; }
            }


			#endregion
        }

    }
}