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
    public partial class PRPARAM
    {
    
        #region Private variables
        private static Cic.Basic.Data.Objects.Property.InfoPackageDictionary _InfoPackageDictionary;
        #endregion

        #region Methods
        public static Cic.Basic.Data.Objects.Property.InfoPackageDictionary DeliverInfoPackageDictionary()
        {
            if (_InfoPackageDictionary == null)
            {
                _InfoPackageDictionary = new Cic.Basic.Data.Objects.Property.InfoPackageDictionary(typeof(PRPARAM));
            }

            return _InfoPackageDictionary;
        }
        #endregion
        
        public partial struct FieldNames
        {
            #region Properties
            public static string SYSPRPARAM
            {
                get { return "SYSPRPARAM"; }
            }

            public static string NAME
            {
                get { return "NAME"; }
            }

            public static string DESCRIPTION
            {
                get { return "DESCRIPTION"; }
            }

            public static string TYP
            {
                get { return "TYP"; }
            }

            public static string STEPSIZE
            {
                get { return "STEPSIZE"; }
            }

            public static string MINVALN
            {
                get { return "MINVALN"; }
            }

            public static string MAXVALN
            {
                get { return "MAXVALN"; }
            }

            public static string DEFVALN
            {
                get { return "DEFVALN"; }
            }

            public static string MINVALP
            {
                get { return "MINVALP"; }
            }

            public static string MAXVALP
            {
                get { return "MAXVALP"; }
            }

            public static string DEFVALP
            {
                get { return "DEFVALP"; }
            }

            public static string MINVALD
            {
                get { return "MINVALD"; }
            }

            public static string MAXVALD
            {
                get { return "MAXVALD"; }
            }

            public static string DEFVALD
            {
                get { return "DEFVALD"; }
            }

            public static string VISIBILITYFLAG
            {
                get { return "VISIBILITYFLAG"; }
            }

            public static string DISABLEDFLAG
            {
                get { return "DISABLEDFLAG"; }
            }


			#endregion
        }

    }
}