using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities.Presentation.PropertyEditing;
using System.Activities.Presentation;
using System.Activities.Presentation.Model;
using System.Windows;
using System.Windows.Controls;
using System.Activities.Presentation.Converters;

namespace Cic.One.Workflow.Design
{
    public class OneEditor : DialogPropertyValueEditor
    {
        private static DataTemplate loadXAML(String template)
        {
            Uri resourceLocator = new Uri("/System.Activities.Core.Presentation;component/system/activities/core/presentation/themes/editorcategorytemplatedictionary.xaml", UriKind.Relative);
            ResourceDictionary d = new ResourceDictionary();
            Application.LoadComponent(d, resourceLocator);
            return (DataTemplate)d[template];
        }
        private class EditorWindow : WorkflowElementDialog
        {
          
            public EditorWindow(ModelItem activity, EditingContext context)
            {
                base.ModelItem = activity;
                base.Context = context;
                base.Owner = activity.View;
                base.EnableMaximizeButton = false;
                base.EnableMinimizeButton = false;
                base.MinHeight = 250.0;
                base.MinWidth = 450.0;
                base.WindowResizeMode = ResizeMode.CanResize;
                base.WindowSizeToContent = SizeToContent.Manual;
                DataTemplate categoryTemplate = loadXAML("CorrelatesOnDesigner_DialogTemplate");
                ContentPresenter content = new ContentPresenter
                {
                    Content = activity,
                    ContentTemplate = categoryTemplate
                };
                base.Title = (string)categoryTemplate.Resources["controlTitle"];
                base.Content = content;
                base.HelpKeyword = "CorrelatesOnDefinition";
            }
            protected override void OnWorkflowElementDialogClosed(bool? dialogResult)
            {
                if (dialogResult.HasValue && dialogResult.Value)
                {
                    ModelProperty modelProperty = base.ModelItem.Properties["CorrelatesOn"];
                    if (modelProperty.IsSet && modelProperty.Dictionary.Count == 0)
                    {
                        modelProperty.ClearValue();
                    }
                }
            }
        }
        public OneEditor()
        {
            base.InlineEditorTemplate = loadXAML("CorrelatesOnDesigner_InlineTemplate");
        }
        public override void ShowDialog(PropertyValue propertyValue, IInputElement commandSource)
        {
            ModelPropertyEntryToOwnerActivityConverter modelPropertyEntryToOwnerActivityConverter = new ModelPropertyEntryToOwnerActivityConverter();
            ModelItem modelItem = (ModelItem)modelPropertyEntryToOwnerActivityConverter.Convert(propertyValue.ParentProperty, typeof(ModelItem), false, null);
            EditingContext editingContext = modelItem.GetEditingContext();
            this.ShowDialog(modelItem, editingContext);
        }
        public void ShowDialog(ModelItem activity, EditingContext context)
        {
            string description = (string)base.InlineEditorTemplate.Resources["bookmarkTitle"];
            context.Services.GetService<UndoEngine>();
            /*using (EditingScope editingScope = context.Services.GetRequiredService<ModelTreeManager>().CreateEditingScope(description, true))
            {
                if (new OneEditor.EditorWindow(activity, context).ShowOkCancel())
                {
                    editingScope.Complete();
                }
            }*/
        }
    }
}
