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
    public partial class ROLETYPE
    {
    
        #region Private variables
        private static Cic.Basic.Data.Objects.Property.InfoPackageDictionary _InfoPackageDictionary;
        #endregion

        #region Methods
        public static Cic.Basic.Data.Objects.Property.InfoPackageDictionary DeliverInfoPackageDictionary()
        {
            if (_InfoPackageDictionary == null)
            {
                _InfoPackageDictionary = new Cic.Basic.Data.Objects.Property.InfoPackageDictionary(typeof(ROLETYPE));
            }

            return _InfoPackageDictionary;
        }
        #endregion
        
        public partial struct FieldNames
        {
            #region Properties
            public static string SYSROLETYPE
            {
                get { return "SYSROLETYPE"; }
            }

            public static string NAME
            {
                get { return "NAME"; }
            }

            public static string DESCRIPTION
            {
                get { return "DESCRIPTION"; }
            }

            public static string DISPLAYABLE
            {
                get { return "DISPLAYABLE"; }
            }

            public static string ISROOT
            {
                get { return "ISROOT"; }
            }

            public static string TYP
            {
                get { return "TYP"; }
            }

            public static string SIGHTFIELDLEVEL
            {
                get { return "SIGHTFIELDLEVEL"; }
            }

            public static string ENABLEPRHGROUP
            {
                get { return "ENABLEPRHGROUP"; }
            }


			#endregion
        }

    }
}