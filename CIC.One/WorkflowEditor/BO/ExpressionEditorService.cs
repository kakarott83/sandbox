using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition.Hosting;
using System.Activities.Presentation.View;
using System.Reflection;
using System.IO;
using Microsoft.VisualBasic.Editor.ExpressionEditor;
using System.Activities.Presentation;
using System.Windows;
using Microsoft.VisualStudio.Activities.AddIn;
using System.Activities.Presentation.Hosting;
using Cic.OpenOne.Common.Util.Logging;

namespace Workflows.BO
{
    public class VSExpressionEditor 
    {
        ExpressionEditorService _service;
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void Dispose()
        {
            _service.Dispose();
        }

        public void CreateEditorService(WorkflowDesigner wd)
        {
            string[] _componentAssemblies = new string[] 
              { 
                "Microsoft.VisualStudio.CoreUtility", 
                "Microsoft.VisualStudio.Language.Intellisense", 
                "Microsoft.VisualStudio.Language.StandardClassification", 
                "Microsoft.VisualStudio.Platform.VSEditor", 
                "Microsoft.VisualStudio.Platform.VSEditor.Interop", 
                "Microsoft.VisualStudio.Text.Data", 
                "Microsoft.VisualStudio.Text.Internal", 
                "Microsoft.VisualStudio.Text.Logic", 
                "Microsoft.VisualStudio.Text.UI", 
                "Microsoft.VisualStudio.Text.UI.Wpf", 
                "Cic.One.DTO",
                "Cic.One.Utils",
                "Cic.One.Workflow",
                "Cic.One.Workflow.Design",
                "log4net",
                "Cic.OpenOne.Common",
                "Cic.OpenOne.Common.Model",
                "Cic.OpenLeaseAuskunftManagement",
                "AutoMapper"
                //"Cic.One.Workflow"/*, "Microsoft.VisualStudio.Editor.Implementation" */
              };
            var assemblyContextControlItem = new AssemblyContextControlItem();
            AssemblyName[] assis = assemblyContextControlItem.GetEnvironmentAssemblyNames().ToArray();

            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(HostableVBCompiler).Assembly));
            //foreach (AssemblyName assy in assis)
            foreach (String str in _componentAssemblies)
            {
                try
                {
                    //String str = assy.Name;
                    Assembly assembly = Assembly.Load(str);
                    catalog.Catalogs.Add(new AssemblyCatalog(assembly));
                }
                catch (Exception e)
                {
                    _log.Error("Error creating Intellisense-Editor", e);
                }
            }
           /* foreach (var ra in Assembly.GetExecutingAssembly().GetLoadedModules().ToList())//.Where(ra => ra.FullName.StartsWith("Microsoft.")))
            {
                catalog.Catalogs.Add(new AssemblyCatalog(Assembly.Load(ra.FullName)));
            }*/

            CompositionContainer container = new CompositionContainer(catalog, new ExportProvider[0]);
            Microsoft.VisualBasic.Editor.HostableEditor.GetHostableVBCompiler = new HostableVBCompiler(container);
            _service = new ExpressionEditorService();
            wd.Context.Services.Publish<IExpressionEditorService>(_service);
           
        }

       /* public void CloseExpressionEditors()
        {
           
        }

        public IExpressionEditorInstance CreateExpressionEditor(System.Activities.Presentation.Hosting.AssemblyContextControlItem assemblies, System.Activities.Presentation.Hosting.ImportedNamespaceContextItem importedNamespaces, List<System.Activities.Presentation.Model.ModelItem> variables, string text, System.Windows.Size initialSize)
        {
            return CreateExpressionEditor(assemblies, importedNamespaces, variables, text, null, initialSize);
        }

        public IExpressionEditorInstance CreateExpressionEditor(System.Activities.Presentation.Hosting.AssemblyContextControlItem assemblies, System.Activities.Presentation.Hosting.ImportedNamespaceContextItem importedNamespaces, List<System.Activities.Presentation.Model.ModelItem> variables, string text)
        {
            return CreateExpressionEditor(assemblies, importedNamespaces, variables, text, (Type)null,Size.Empty);
        }

        public IExpressionEditorInstance CreateExpressionEditor(System.Activities.Presentation.Hosting.AssemblyContextControlItem assemblies, System.Activities.Presentation.Hosting.ImportedNamespaceContextItem importedNamespaces, List<System.Activities.Presentation.Model.ModelItem> variables, string text, Type expressionType, System.Windows.Size initialSize)
        {
            return (IExpressionEditorService)_service;  
        }

        public IExpressionEditorInstance CreateExpressionEditor(System.Activities.Presentation.Hosting.AssemblyContextControlItem assemblies, System.Activities.Presentation.Hosting.ImportedNamespaceContextItem importedNamespaces, List<System.Activities.Presentation.Model.ModelItem> variables, string text, Type expressionType)
        {
            return CreateExpressionEditor(assemblies, importedNamespaces, variables, text, expressionType, Size.Empty);
        }

        public void UpdateContext(System.Activities.Presentation.Hosting.AssemblyContextControlItem assemblies, System.Activities.Presentation.Hosting.ImportedNamespaceContextItem importedNamespaces)
        {
            
        }*/

    }
}
