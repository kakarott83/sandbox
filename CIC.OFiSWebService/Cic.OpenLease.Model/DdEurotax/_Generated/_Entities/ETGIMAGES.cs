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
    public partial class ETGIMAGES
    {
    
        #region Private variables
        private static Cic.Basic.Data.Objects.Property.InfoPackageDictionary _InfoPackageDictionary;
        #endregion

        #region Methods
        public static Cic.Basic.Data.Objects.Property.InfoPackageDictionary DeliverInfoPackageDictionary()
        {
            if (_InfoPackageDictionary == null)
            {
                _InfoPackageDictionary = new Cic.Basic.Data.Objects.Property.InfoPackageDictionary(typeof(ETGIMAGES));
            }

            return _InfoPackageDictionary;
        }
        #endregion
        
        public partial struct FieldNames
        {
            #region Properties
            public static string ID
            {
                get { return "ID"; }
            }

            public static string PHYSNAME
            {
                get { return "PHYSNAME"; }
            }

            public static string TXTVIEWCD2
            {
                get { return "TXTVIEWCD2"; }
            }

            public static string TXTCOLCD2
            {
                get { return "TXTCOLCD2"; }
            }

            public static string TXTBODYCD2
            {
                get { return "TXTBODYCD2"; }
            }

            public static string DOOR
            {
                get { return "DOOR"; }
            }

            public static string MODELY
            {
                get { return "MODELY"; }
            }

            public static string FORMAT
            {
                get { return "FORMAT"; }
            }

            public static string RECSTATUS
            {
                get { return "RECSTATUS"; }
            }

            public static string RECDATE
            {
                get { return "RECDATE"; }
            }


			#endregion
        }

    }
}