using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Activities.Presentation.Model;
using Cic.One.Workflow.Design.Utils;

namespace Cic.One.Workflow.Design.ActivityDesigns
{
    // Interaction logic for WorkflowButtonDesigner.xaml
    public partial class WorkflowButtonDesigner
    {
        public WorkflowButtonDesigner()
        {
            InitializeComponent();
            //loads the icon into the WF4-Editor Activity
            this.Icon = ResourceLoading.loadIcon("CICOne.png");
        }

       
        protected override void OnModelItemChanged(object newItem)
        {
            ModelItem modelItem = newItem as ModelItem;
            if (modelItem != null)
                modelItem.PropertyChanged += this.ModelItemPropertyChangedHandler;
            base.OnModelItemChanged(newItem);
        }

        private void ModelItemPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            //manages to write a activity property to another property of the activity
             if (!e.PropertyName.Equals("DisplayName", StringComparison.OrdinalIgnoreCase))
                 return;
             ModelItem.Properties["DisplayName"].SetValue(ModelItem.Properties["DisplayName"].Value );
        }
    }
}
