using System;
using System.Reflection;
using System.Runtime.InteropServices;

// Allgemeine Informationen über eine Assembly werden über die folgenden 
// Attribute gesteuert. Ändern Sie diese Attributwerte, um die Informationen zu ändern,
// die einer Assembly zugeordnet sind.
[assembly: System.Reflection.AssemblyTitle("Cic.OpenOne.GateBANKNOW.Service")]
[assembly: System.Reflection.AssemblyDescription("CIC BOS Services")]
[assembly: System.Reflection.AssemblyConfiguration("")]
[assembly: System.Reflection.AssemblyCompany("C.I.C. Software GmbH")]
[assembly: System.Reflection.AssemblyProduct("Cic.OpenOne.GateBANKNOW.Service")]
[assembly: System.Reflection.AssemblyCopyright("Copyright © C.I.C. Software GmbH 2015")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]


// Durch Festlegen von ComVisible auf "false" werden die Typen in dieser Assembly unsichtbar 
// für COM-Komponenten. Wenn Sie auf einen Typ in dieser Assembly von 
// COM zugreifen müssen, legen Sie das ComVisible-Attribut für diesen Typ auf "true" fest.
[assembly: ComVisible(false)]

// Die folgende GUID bestimmt die ID der Typbibliothek, wenn dieses Projekt für COM verfügbar gemacht wird
[assembly: Guid("e88da537-68d4-4d04-b1f4-3b6655313b42")]

// Versionsinformationen für eine Assembly bestehen aus den folgenden vier Werten:
//
//      Hauptversion
//      Nebenversion 
//      Buildnummer
//      Revision
//
// Sie können alle Werte angeben oder die standardmäßigen Revisions- und Buildnummern 
// übernehmen, indem Sie "*" eingeben:
// [assembly: AssemblyVersion("1.0.*")]

[assembly: AssemblyVersion("1.19.44570.0")]
[assembly: AssemblyFileVersion("1.19.0.0")]
[assembly:CLSCompliant(false)]
[assembly: System.Resources.NeutralResourcesLanguage("en")]
[assembly: AssemblyInformationalVersion("2.0.0.0")]


namespace Cic.OpenOne.GateBANKNOW.Service
{
    //[System.CLSCompliant(true)]
    internal sealed class AssemblyInfo
    {
        #region Constructor
      
        private AssemblyInfo()
        {
        }
        #endregion

        #region Methods
       
        internal static string GetVersion()
        {
            System.Reflection.Assembly Assembly;

            // Get executing assembly
            Assembly = System.Reflection.Assembly.GetExecutingAssembly();
            // Return
            return Assembly.GetName().Version.ToString();
        }

       
        internal static string GetFullName()
        {
            System.Reflection.Assembly Assembly;

            // Get executing assembly
            Assembly = System.Reflection.Assembly.GetExecutingAssembly();
            // Return
            return Assembly.FullName;
        }
        #endregion
    }
}
