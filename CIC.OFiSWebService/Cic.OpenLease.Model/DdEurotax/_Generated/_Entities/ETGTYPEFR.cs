// AUTOGENERATED, 26.01.2010 17:01:12
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
    public partial class ETGTYPEFR
    {
    
        #region Private variables
        private static Cic.Basic.Data.Objects.Property.InfoPackageDictionary _InfoPackageDictionary;
        #endregion

        #region Methods
        public static Cic.Basic.Data.Objects.Property.InfoPackageDictionary DeliverInfoPackageDictionary()
        {
            if (_InfoPackageDictionary == null)
            {
                _InfoPackageDictionary = new Cic.Basic.Data.Objects.Property.InfoPackageDictionary(typeof(ETGTYPEFR));
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

            public static string RECSTATUS
            {
                get { return "RECSTATUS"; }
            }

            public static string RECDATE
            {
                get { return "RECDATE"; }
            }

            public static string ASSUCODEAUTO
            {
                get { return "ASSUCODEAUTO"; }
            }

            public static string ASSUGROUP
            {
                get { return "ASSUGROUP"; }
            }

            public static string ASSUCLASS
            {
                get { return "ASSUCLASS"; }
            }

            public static string ASSUCLE
            {
                get { return "ASSUCLE"; }
            }

            public static string TYPNAMEFR
            {
                get { return "TYPNAMEFR"; }
            }

            public static string VIRTUALFLAG
            {
                get { return "VIRTUALFLAG"; }
            }


			#endregion
        }

    }
}