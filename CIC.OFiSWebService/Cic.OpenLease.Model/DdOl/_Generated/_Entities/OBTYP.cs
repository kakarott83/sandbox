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
    public partial class OBTYP
    {
    
        #region Private variables
        private static Cic.Basic.Data.Objects.Property.InfoPackageDictionary _InfoPackageDictionary;
        #endregion

        #region Methods
        public static Cic.Basic.Data.Objects.Property.InfoPackageDictionary DeliverInfoPackageDictionary()
        {
            if (_InfoPackageDictionary == null)
            {
                _InfoPackageDictionary = new Cic.Basic.Data.Objects.Property.InfoPackageDictionary(typeof(OBTYP));
            }

            return _InfoPackageDictionary;
        }
        #endregion
        
        public partial struct FieldNames
        {
            #region Properties
            public static string SYSOBTYP
            {
                get { return "SYSOBTYP"; }
            }

            public static string BEZEICHNUNG
            {
                get { return "BEZEICHNUNG"; }
            }

            public static string BESCHREIBUNG
            {
                get { return "BESCHREIBUNG"; }
            }

            public static string AKLASSE
            {
                get { return "AKLASSE"; }
            }

            public static string SCHWACKE
            {
                get { return "SCHWACKE"; }
            }

            public static string SYSOBTYPP
            {
                get { return "SYSOBTYPP"; }
            }

            public static string SYSVGRW
            {
                get { return "SYSVGRW"; }
            }

            public static string SYSVGWR
            {
                get { return "SYSVGWR"; }
            }

            public static string SYSVGRF
            {
                get { return "SYSVGRF"; }
            }

            public static string NOOBJECTS
            {
                get { return "NOOBJECTS"; }
            }

            public static string IMPORTSOURCE
            {
                get { return "IMPORTSOURCE"; }
            }

            public static string IMPORTTABLE
            {
                get { return "IMPORTTABLE"; }
            }

            public static string IMPORTDATE
            {
                get { return "IMPORTDATE"; }
            }

            public static string FLAGLOCKEDVG
            {
                get { return "FLAGLOCKEDVG"; }
            }

            public static string NOEXTID
            {
                get { return "NOEXTID"; }
            }

            public static string SYSVGSI
            {
                get { return "SYSVGSI"; }
            }

            public static string ART
            {
                get { return "ART"; }
            }


			#endregion
        }

    }
}