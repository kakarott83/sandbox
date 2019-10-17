using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities.Presentation;
using System.Activities.Presentation.Hosting;

namespace Workflows.BO
{
    public static class WorklfowDesignerExtension
    {
        public static void SetReferencedAssemblies(
           this WorkflowDesigner workflowDesigner)
        {
            if (workflowDesigner == null)
            {
                return;
            }

            var assemblyContextControlItem = new AssemblyContextControlItem();
            var assemblyNameList = assemblyContextControlItem.GetEnvironmentAssemblyNames().Where(a=>!a.Name.Equals("System")).ToList();

            
            assemblyContextControlItem.ReferencedAssemblyNames = assemblyNameList;

            
            
            workflowDesigner.Context.Items.SetValue(assemblyContextControlItem);
        }
    }
}
