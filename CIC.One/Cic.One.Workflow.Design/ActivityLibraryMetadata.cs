using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities.Presentation.Metadata;
using Cic.One.Workflow.Activities;
using System.ComponentModel;
using System.Drawing;
using Cic.One.Workflow.ActivityDesigns;
using System.Activities.Presentation.PropertyEditing;
using Cic.One.Workflow.Design.ActivityDesigns;
using Cic.One.Workflow.Design.Editor;
using System.Configuration;

namespace Cic.One.Workflow.Design
{

    /// <summary>
    /// Class responsible for registering the ActivityDesigners in the WF4-Editor
    /// VS 2010 will automatically load this class when its inside <activity-library>.Design.dll
    /// </summary>
    public sealed class ActivityLibraryMetadata : IRegisterMetadata
    {
        public void Register()
        {
            RegisterAll();
        }

        public static void RegisterAll()
        {
            var builder = new AttributeTableBuilder();

            builder.AddCustomAttributes(typeof(Cic.One.Workflow.Activities.UserInteraction),new DesignerAttribute(typeof(UserInteractionDesigner)));
            builder.AddCustomAttributes(typeof(Cic.One.Workflow.Activities.WorkflowButton), new DesignerAttribute(typeof(WorkflowButtonDesigner)));
            builder.AddCustomAttributes(typeof(Cic.One.Workflow.Activities.PrintButton), new DesignerAttribute(typeof(PrintButtonDesigner)));
            builder.AddCustomAttributes(typeof(Cic.One.Workflow.Activities.MessageBox), new DesignerAttribute(typeof(MessageBoxDesigner)));
            builder.AddCustomAttributes(typeof(Cic.One.Workflow.Activities.AddMessage), new DesignerAttribute(typeof(AddMessageDesigner)));
            builder.AddCustomAttributes(typeof(Cic.One.Workflow.Activities.CASCall<>), new DesignerAttribute(typeof(CASCallDesigner)));

            //register bool checkbox in userInteraction Property
          //  builder.AddCustomAttributes(typeof(Cic.One.Workflow.Activities.UserInteraction), "endWorkflow", new EditorAttribute(typeof(InArgumentBoolPropertyEditor),typeof(PropertyValueEditor)));
            

            
            //Cic.OpenOne.Common.Util.Config.Configuration.setDBSettings();
            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
