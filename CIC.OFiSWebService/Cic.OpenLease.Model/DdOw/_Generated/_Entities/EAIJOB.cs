// AUTOGENERATED, 16.10.2009 12:05:17
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace Cic.OpenLease.Model.DdOw
{
    [System.CodeDom.Compiler.GeneratedCode("Cic.OpenLease.CodeGenerator", "1.0.0.0")]
    [System.CLSCompliant(true)]
    public partial class EAIJOB
    {
    
        #region Private variables
        private static Cic.Basic.Data.Objects.Property.InfoPackageDictionary _InfoPackageDictionary;
        #endregion

        #region Methods
        public static Cic.Basic.Data.Objects.Property.InfoPackageDictionary DeliverInfoPackageDictionary()
        {
            if (_InfoPackageDictionary == null)
            {
                _InfoPackageDictionary = new Cic.Basic.Data.Objects.Property.InfoPackageDictionary(typeof(EAIJOB));
            }

            return _InfoPackageDictionary;
        }
        #endregion
        
        public partial struct FieldNames
        {
            #region Properties
            public static string SYSEAIJOB
            {
                get { return "SYSEAIJOB"; }
            }

            public static string CODE
            {
                get { return "CODE"; }
            }

            public static string PROZESSSTATUS
            {
                get { return "PROZESSSTATUS"; }
            }

            public static string SYSWFUSER
            {
                get { return "SYSWFUSER"; }
            }

            public static string FILEFLAG
            {
                get { return "FILEFLAG"; }
            }

            public static string EXECPRIORITY
            {
                get { return "EXECPRIORITY"; }
            }

            public static string INPUTPARAMETER1
            {
                get { return "INPUTPARAMETER1"; }
            }

            public static string INPUTPARAMETER2
            {
                get { return "INPUTPARAMETER2"; }
            }

            public static string INPUTPARAMETER3
            {
                get { return "INPUTPARAMETER3"; }
            }

            public static string INPUTPARAMETER4
            {
                get { return "INPUTPARAMETER4"; }
            }

            public static string INPUTPARAMETER5
            {
                get { return "INPUTPARAMETER5"; }
            }

            public static string OUTPUTPARAMETER1
            {
                get { return "OUTPUTPARAMETER1"; }
            }

            public static string OUTPUTPARAMETER2
            {
                get { return "OUTPUTPARAMETER2"; }
            }

            public static string OUTPUTPARAMETER3
            {
                get { return "OUTPUTPARAMETER3"; }
            }

            public static string OUTPUTPARAMETER4
            {
                get { return "OUTPUTPARAMETER4"; }
            }

            public static string OUTPUTPARAMETER5
            {
                get { return "OUTPUTPARAMETER5"; }
            }

            public static string SUBMITDATE
            {
                get { return "SUBMITDATE"; }
            }

            public static string SUBMITTIME
            {
                get { return "SUBMITTIME"; }
            }

            public static string FINISHDATE
            {
                get { return "FINISHDATE"; }
            }

            public static string FINISHTIME
            {
                get { return "FINISHTIME"; }
            }

            public static string SYSWFEXEC
            {
                get { return "SYSWFEXEC"; }
            }

            public static string STARTDATE
            {
                get { return "STARTDATE"; }
            }

            public static string STARTTIME
            {
                get { return "STARTTIME"; }
            }

            public static string TRANSACTIONFLAG
            {
                get { return "TRANSACTIONFLAG"; }
            }


			#endregion
        }

    }
}