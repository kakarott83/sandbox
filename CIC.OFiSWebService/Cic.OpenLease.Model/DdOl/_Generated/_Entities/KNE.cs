// AUTOGENERATED, 18.11.2010 15:27:36
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
    public partial class KNE
    {
    
        #region Private variables
        private static Cic.Basic.Data.Objects.Property.InfoPackageDictionary _InfoPackageDictionary;
        #endregion

        #region Methods
        public static Cic.Basic.Data.Objects.Property.InfoPackageDictionary DeliverInfoPackageDictionary()
        {
            if (_InfoPackageDictionary == null)
            {
                _InfoPackageDictionary = new Cic.Basic.Data.Objects.Property.InfoPackageDictionary(typeof(KNE));
            }

            return _InfoPackageDictionary;
        }
        #endregion
        
        public partial struct FieldNames
        {
            #region Properties
            public static string SYSKNE
            {
                get { return "SYSKNE"; }
            }

            public static string SYSOBER
            {
                get { return "SYSOBER"; }
            }

            public static string SYSUNTER
            {
                get { return "SYSUNTER"; }
            }

            public static string QUOTE
            {
                get { return "QUOTE"; }
            }

            public static string BEZEICHNUNG
            {
                get { return "BEZEICHNUNG"; }
            }

            public static string GUELTIGVON
            {
                get { return "GUELTIGVON"; }
            }

            public static string GUELTIGBIS
            {
                get { return "GUELTIGBIS"; }
            }

            public static string GEPRUEFTAM
            {
                get { return "GEPRUEFTAM"; }
            }

            public static string AKTEVOM
            {
                get { return "AKTEVOM"; }
            }

            public static string PRUEFKOMMENTAR
            {
                get { return "PRUEFKOMMENTAR"; }
            }


			#endregion
        }

    }
}