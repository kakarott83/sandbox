Ribbon: from http://www.microsoft.com/en-us/download/details.aspx?id=11877

http://wf4host.codeplex.com/SourceControl/latest#WF4Host/WF4Host/MainWindow.xaml
http://www.felicepollano.com/2011/03/14/WF4ReHostingBindAllTogether.aspx
http://blogs.msdn.com/b/asgisv/archive/2010/02/10/displaying-net-framework-4-built-in-workflow-activity-icons-in-a-rehosted-workflow-designer.aspx
http://blogs.msdn.com/b/tilovell/archive/2012/10/04/wf4-vs-workflowdesigner-extensions-in-visual-studio-2012.aspx

http://social.msdn.microsoft.com/Forums/vstudio/en-US/87064412-406a-4646-a61a-1f316280bda8/implmentation-of-iexpressioneditorservice?forum=wfprerelease
http://social.msdn.microsoft.com/Forums/vstudio/en-US/07f89fc0-81ff-4e85-a974-013b15b62f50/rehosting-question-intellisense-support?forum=wfprerelease

if you want the application to run in an enviroment where no Visual Studio is installed, you'll need these dlls too:
Microsoft.VisualBasic.dll
Microsoft.VisualBasic.Editor.dll
Microsoft.VisualBasic.LanguageService.dll
Microsoft.VisualStudio.Activities.Addin.dll
Microsoft.VisualStudio.Activities.AddinView.dll
Microsoft.VisualStudio.Activities.dll
Microsoft.VisualStudio.ComponentModelHost.dll
Microsoft.VisualStudio.CoreUtility.dll
Microsoft.VisualStudio.ExtensibilityHosting.dll
Microsoft.VisualStudio.Language.Intellisense.dll
Microsoft.VisualStudio.Language.StandardClassification.dll
Microsoft.VisualStudio.OLE.Interop.dll
Microsoft.VisualStudio.Platform.VSEditor.dll
Microsoft.VisualStudio.Platform.VSEditor.Interop.dll
Microsoft.VisualStudio.Shell.10.0.dll
Microsoft.VisualStudio.Shell.Interop.10.0.dll
Microsoft.VisualStudio.Shell.Interop.8.0.dll
Microsoft.VisualStudio.Shell.Interop.9.0.dll
Microsoft.VisualStudio.Shell.Interop.dll
Microsoft.VisualStudio.Text.Data.dll
Microsoft.VisualStudio.Text.Internal.dll
Microsoft.VisualStudio.Text.Logic.dll
Microsoft.VisualStudio.Text.UI.dll
Microsoft.VisualStudio.Text.UI.Wpf.dll
Microsoft.VisualStudio.TextManager.Interop.dll
msvbide.dll
msvbideui.dll

Also the latest Visual Studio C++ 10 runtime needs tobe installed.

And a Registry Key needs tobe setup

[HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\VisualStudio\10.0\Setup\VS]
"EnvironmentDirectory"="C:\\Program Files\\somerealpath"






