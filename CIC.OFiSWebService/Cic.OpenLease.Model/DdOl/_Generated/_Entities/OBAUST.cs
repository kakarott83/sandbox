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
    public partial class OBAUST
    {
    
        #region Private variables
        private static Cic.Basic.Data.Objects.Property.InfoPackageDictionary _InfoPackageDictionary;
        #endregion

        #region Methods
        public static Cic.Basic.Data.Objects.Property.InfoPackageDictionary DeliverInfoPackageDictionary()
        {
            if (_InfoPackageDictionary == null)
            {
                _InfoPackageDictionary = new Cic.Basic.Data.Objects.Property.InfoPackageDictionary(typeof(OBAUST));
            }

            return _InfoPackageDictionary;
        }
        #endregion
        
        public partial struct FieldNames
        {
            #region Properties
            public static string SYSOBAUST
            {
                get { return "SYSOBAUST"; }
            }

            public static string SNR
            {
                get { return "SNR"; }
            }

            public static string BESCHREIBUNG
            {
                get { return "BESCHREIBUNG"; }
            }

            public static string BETRAG
            {
                get { return "BETRAG"; }
            }

            public static string FLAGPACKET
            {
                get { return "FLAGPACKET"; }
            }

            public static string BETRAG2
            {
                get { return "BETRAG2"; }
            }


			#endregion
        }

    }
}