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
    public partial class PEINFOCREL
    {
    
        #region Private variables
        private static Cic.Basic.Data.Objects.Property.InfoPackageDictionary _InfoPackageDictionary;
        #endregion

        #region Methods
        public static Cic.Basic.Data.Objects.Property.InfoPackageDictionary DeliverInfoPackageDictionary()
        {
            if (_InfoPackageDictionary == null)
            {
                _InfoPackageDictionary = new Cic.Basic.Data.Objects.Property.InfoPackageDictionary(typeof(PEINFOCREL));
            }

            return _InfoPackageDictionary;
        }
        #endregion
        
        public partial struct FieldNames
        {
            #region Properties
            public static string SYSPEINFOCREL
            {
                get { return "SYSPEINFOCREL"; }
            }

            public static string EXTERNEID
            {
                get { return "EXTERNEID"; }
            }

            public static string FUNCCODE
            {
                get { return "FUNCCODE"; }
            }

            public static string SIGCODE
            {
                get { return "SIGCODE"; }
            }

            public static string STARTDATE
            {
                get { return "STARTDATE"; }
            }

            public static string ENDDATE
            {
                get { return "ENDDATE"; }
            }

            public static string NEGATIVEFLAG
            {
                get { return "NEGATIVEFLAG"; }
            }


			#endregion
        }

    }
}