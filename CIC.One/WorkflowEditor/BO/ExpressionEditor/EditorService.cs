using Microsoft.VisualBasic;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Activities.Presentation.View;
using System.Activities.Presentation.Model;
using System.Activities.Presentation.Hosting;
using System.Reflection;
using Microsoft.VisualBasic.Activities;
using System.Activities.Presentation.Converters;
using System.Activities;
using System.Windows.Controls;
using System.Activities.Presentation;
using System.Collections.ObjectModel;

namespace Workflows.BO.ExpressionEditor
{



    public class EditorService : IExpressionEditorService
    {

      

        public EditorService(IntelliScan iscan)
        {
            EditorKeyWord = iscan.keyWords;
            IntellisenseData = iscan._inttelisenseList;
        }
        

        public void CreateEditorService(WorkflowDesigner wd)
        {

            //_service = new EditorService();
            wd.Context.Services.Publish<IExpressionEditorService>(this);

        }


        internal TreeNodes IntellisenseData { get; set; }
        internal string EditorKeyWord { get; set; }


        private Dictionary<string, EditorInstance> editorInstances = new Dictionary<string, EditorInstance>();

        public void CloseExpressionEditors()
        {
            foreach (EditorInstance childEditor in editorInstances.Values)
            {

                // childEditor.LostAggregateFocus -= LostFocus;
                //childEditor = null;
            }
            editorInstances.Clear();
        }

        public IExpressionEditorInstance CreateExpressionEditor(AssemblyContextControlItem assemblies, ImportedNamespaceContextItem importedNamespaces, System.Collections.Generic.List<ModelItem> variables, string text)
        {
            return CreateExpressionEditorPrivate(assemblies, importedNamespaces, variables, text, null, System.Windows.Size.Empty);
        }

        public IExpressionEditorInstance CreateExpressionEditor(AssemblyContextControlItem assemblies, ImportedNamespaceContextItem importedNamespaces, System.Collections.Generic.List<ModelItem> variables, string text, System.Type expressionType)
        {
            return CreateExpressionEditorPrivate(assemblies, importedNamespaces, variables, text, expressionType, System.Windows.Size.Empty);
        }

        public IExpressionEditorInstance CreateExpressionEditor(AssemblyContextControlItem assemblies, ImportedNamespaceContextItem importedNamespaces, System.Collections.Generic.List<ModelItem> variables, string text, System.Type expressionType, System.Windows.Size initialSize)
        {
            return CreateExpressionEditorPrivate(assemblies, importedNamespaces, variables, text, expressionType, initialSize);
        }

        public IExpressionEditorInstance CreateExpressionEditor(AssemblyContextControlItem assemblies, ImportedNamespaceContextItem importedNamespaces, System.Collections.Generic.List<ModelItem> variables, string text, System.Windows.Size initialSize)
        {
            return CreateExpressionEditorPrivate(assemblies, importedNamespaces, variables, text, null, initialSize);
        }


        public void UpdateContext(AssemblyContextControlItem assemblies, ImportedNamespaceContextItem importedNamespaces)
        {
        }




        private object _intellisenseLock = new object();
        private TreeNodes CreateUpdatedIntellisense(List<ModelItem> vars)
        {
            TreeNodes result = IntellisenseData;
            lock (_intellisenseLock)
            {
                /*foreach (ModelItem vs in vars)
                {

                    dynamic vsProp = vs.Properties["Name"];
                    if (vsProp == null)
                        continue;
                    dynamic varName = vsProp.ComputedValue;
                    TreeNodes res = (from x in result.Nodes
                                  where x.Name == varName
                                  select x).FirstOrDefault();

                    if (res == null)
                    {
                        Type sysType = null;
                        dynamic sysTypeProp = vs.Properties["Type"];
                        if (sysTypeProp != null)
                        {
                            sysType = (Type)sysTypeProp.ComputedValue;
                        }
                        TreeNodes newVar = new TreeNodes
                        {
                            Name = varName,
                            ItemType = TreeNodes.NodeTypes.Primitive,
                            SystemType = sysType,
                            Description = ""
                        };
                        result.Nodes.Add(newVar);
                    }
                }*/
                foreach (var mitmp in vars)
                {
                    var mi = mitmp;
                    while (mi.Parent != null)
                    {
                        var displaynameproperty = mi.Parent.Properties["DisplayName"];
                        var displayname = displaynameproperty == null ? "Unknown" : displaynameproperty.ComputedValue;
                        Type parentType = mi.Parent.ItemType;
                        if (typeof(Activity).IsAssignableFrom(parentType))
                        {
                            // we have encountered an activity derived type
                            // look for variable collection
                            ModelProperty mp = mi.Parent.Properties["Variables"];
                            if (null != mp && mp.PropertyType == typeof(Collection<Variable>) && mp.Collection != null)
                            {
                                foreach (var item in mp.Collection.ToList())
                                {
                                    var modelProperty = item.Properties["Name"];
                                    if (modelProperty != null)
                                    {
                                        Type type = typeof(void);
                                        string name = modelProperty.ComputedValue.ToString();
                                        var property = item.Properties["Type"];
                                        if (property != null)
                                        {
                                            type = property.ComputedValue as Type;
                                        }
                                        var desc = string.Format("{0} variable defined in {1}", type.Name, displayname);


                                        TreeNodes res = (from x in result.Nodes
                                                         where x.Name == name
                                                         select x).FirstOrDefault();

                                        if (res == null)
                                        {
                                            TreeNodes newVar = new TreeNodes
                                            {
                                                Name = name,
                                                ItemType = TreeNodes.NodeTypes.Primitive,
                                                SystemType = type,
                                                Description = ""
                                            };
                                            result.Nodes.Add(newVar);
                                        }

                                        //variables.Add(new FdCompletionData() { Text = name, Type = type, CompletionType = CompletionType.Variable, Priority = 10, Description = desc });
                                    }
                                }
                            }
                        }

                        // now we need to look ataction handlers 
                        // this will ideally return a bunch of DelegateArguments
                        IEnumerable<ModelProperty> dels =
                        mi.Properties.Where(p => typeof(ActivityDelegate).IsAssignableFrom(p.PropertyType));
                        foreach (ModelProperty actdel in dels)
                        {
                            if (actdel.Value != null)
                            {
                                List<TreeNodes> testVars = (from innerProp in actdel.Value.Properties
                                                   let modelItem = innerProp.Value
                                                   where modelItem != null
                                                   where
                                                     typeof(DelegateArgument).IsAssignableFrom(innerProp.PropertyType) &&
                                                     null != modelItem
                                                   let modelProperty = modelItem.Properties["Name"]
                                                   where modelProperty != null
                                                   let name = modelProperty.ComputedValue.ToString()
                                                   let property = modelItem.Properties["Type"]
                                                   where property != null
                                                   let type = property.ComputedValue as Type
                                                   select new TreeNodes() { Name = name, SystemType = type, ItemType = TreeNodes.NodeTypes.Primitive, Description="" }).ToList();
                                foreach (TreeNodes tn in testVars)
                                {
                                    TreeNodes res = (from x in result.Nodes
                                                     where x.Name == tn.Name
                                                     select x).FirstOrDefault();
                                    if (res == null)
                                    {
                                        result.Nodes.Add(tn);
                                    }
                                }
                            }
                        }

                        mi = mi.Parent;
                    }
                }
            }
            return result;
        }

        private IExpressionEditorInstance CreateExpressionEditorPrivate(AssemblyContextControlItem assemblies, ImportedNamespaceContextItem importedNamespaces, System.Collections.Generic.List<ModelItem> variables, string text, System.Type expressionType, System.Windows.Size initialSize)
        {
            EditorInstance editor = new EditorInstance
            {
                IntellisenseList = this.CreateUpdatedIntellisense(variables),
                HighlightWords = this.EditorKeyWord,
                ExpressionType = expressionType,
                Guid = Guid.NewGuid(),
                Text = text
            };
            // editor.LostAggregateFocus += LostFocus;

            editorInstances.Add(editor.Guid.ToString(), editor);
            return editor;
        }

        private void LostFocus(object sender, EventArgs e)
        {
            dynamic edt = sender as TextBox;
            if (edt != null)
                DesignerView.CommitCommand.Execute(edt.Text);
        }



    }
}