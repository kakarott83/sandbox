// AUTOGENERATED, 10/02/2010 17:11:43
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace Cic.OpenLease.Model.DdCt
{
    [System.CodeDom.Compiler.GeneratedCode("Cic.OpenLease.CodeGenerator", "1.0.0.0")]
    [System.CLSCompliant(true)]
    public partial class CTDDLKPPOS
    {
    
        #region Private variables
        private static Cic.Basic.Data.Objects.Property.InfoPackageDictionary _InfoPackageDictionary;
        #endregion

        #region Methods
        public static Cic.Basic.Data.Objects.Property.InfoPackageDictionary DeliverInfoPackageDictionary()
        {
            if (_InfoPackageDictionary == null)
            {
                _InfoPackageDictionary = new Cic.Basic.Data.Objects.Property.InfoPackageDictionary(typeof(CTDDLKPPOS));
            }

            return _InfoPackageDictionary;
        }
        #endregion
        
        public partial struct FieldNames
        {
            #region Properties
            public static string SYSDDLKPPOS
            {
                get { return "SYSDDLKPPOS"; }
            }

            public static string ID
            {
                get { return "ID"; }
            }

            public static string CODE
            {
                get { return "CODE"; }
            }

            public static string DOMAINID
            {
                get { return "DOMAINID"; }
            }

            public static string VALUE
            {
                get { return "VALUE"; }
            }

            public static string TOOLTIP
            {
                get { return "TOOLTIP"; }
            }

            public static string RANK
            {
                get { return "RANK"; }
            }


			#endregion
        }

    }
}