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
    public partial class ETGGROUPVALIDNR
    {
    
        #region Private variables
        private static Cic.Basic.Data.Objects.Property.InfoPackageDictionary _InfoPackageDictionary;
        #endregion

        #region Methods
        public static Cic.Basic.Data.Objects.Property.InfoPackageDictionary DeliverInfoPackageDictionary()
        {
            if (_InfoPackageDictionary == null)
            {
                _InfoPackageDictionary = new Cic.Basic.Data.Objects.Property.InfoPackageDictionary(typeof(ETGGROUPVALIDNR));
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

            public static string TXTCODECD2
            {
                get { return "TXTCODECD2"; }
            }

            public static string NUMBER1
            {
                get { return "NUMBER1"; }
            }

            public static string RECSTATUS
            {
                get { return "RECSTATUS"; }
            }

            public static string RECDATE
            {
                get { return "RECDATE"; }
            }

            public static string REPLID
            {
                get { return "REPLID"; }
            }


			#endregion
        }

    }
}