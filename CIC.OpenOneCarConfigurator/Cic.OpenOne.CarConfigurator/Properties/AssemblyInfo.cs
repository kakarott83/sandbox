using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Allgemeine Informationen über eine Assembly werden über die folgenden 
// Attribute gesteuert. Ändern Sie diese Attributwerte, um die Informationen zu ändern,
// die mit einer Assembly verknüpft sind.
[assembly: AssemblyTitle("Cic.OpenOne.CarConfigurator")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("CIC Software GmbH")]
[assembly: AssemblyProduct("Cic.OpenOne.CarConfigurator")]
[assembly: AssemblyCopyright("Copyright © CIC Software GmbH 2011")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Durch Festlegen von ComVisible auf "false" werden die Typen in dieser Assembly unsichtbar 
// für COM-Komponenten. Wenn Sie auf einen Typ in dieser Assembly von 
// COM zugreifen müssen, legen Sie das ComVisible-Attribut für diesen Typ auf "true" fest.
[assembly: ComVisible(false)]

// Die folgende GUID bestimmt die ID der Typbibliothek, wenn dieses Projekt für COM verfügbar gemacht wird
[assembly: Guid("af6c6fb5-f1e9-4aa3-a90b-16564945373f")]

// Versionsinformationen für eine Assembly bestehen aus den folgenden vier Werten:
//
//      Hauptversion
//      Nebenversion 
//      Buildnummer
//      Revision
//
// Sie können alle Werte angeben oder die standardmäßigen Build- und Revisionsnummern 
// übernehmen, indem Sie "*" eingeben:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
namespace Cic.OpenOne.CarConfigurator
{
    //[System.CLSCompliant(true)]
    internal sealed class AssemblyInfo
    {
        #region Constructor
        // NOTE BK, Private Constructur, to avoid the creation of a standard constructor through the compiler
        // TODO BK 0 BK, Not tested
        private AssemblyInfo()
        {
        }
        #endregion

        #region Methods
        // TODO BK 0 BK, Not tested
        internal static string GetVersion()
        {
            System.Reflection.Assembly Assembly;

            // Get executing assembly
            Assembly = System.Reflection.Assembly.GetExecutingAssembly();
            // Return
            return Assembly.GetName().Version.ToString();
        }

        // TODO BK 0 BK, Not tested
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
