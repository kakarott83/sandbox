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
using Cic.One.Workflow.Icons;
using System.IO;
using Cic.One.Workflow.Design.Utils;
using Cic.One.Workflow.Design.DataSource;
using System.Activities.Presentation.Model;
using System.ComponentModel;
using Cic.One.Workflow.Design.Converter;
using System.Activities.Presentation;

namespace Cic.One.Workflow.ActivityDesigns
{
    // Interaction logic for UserInteractionDesigner.xaml
    public partial class UserInteractionDesigner
    {
        public UserInteractionDesigner()
        {
           
            InitializeComponent();
            //loads the icon into the WF4-Editor Activity
            this.Icon = ResourceLoading.loadIcon("CICOne.png");
            this.viewBox.ItemsSource = new ViewItemSource().VorgangItems();
            
        }
        protected override void OnModelItemChanged(object newItem)
        {
            ModelItem modelItem = newItem as ModelItem;
            if (modelItem != null)
                modelItem.PropertyChanged += this.ModelItemPropertyChangedHandler;
            base.OnModelItemChanged(newItem);
        }
        //private bool changing = false;

        private void ModelItemPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            /*if (changing) return;
            
            //manages to write a activity property to another property of the activity
            if (!e.PropertyName.Equals("workflowViewId", StringComparison.OrdinalIgnoreCase))
                return;

            changing = true;
            try
            {
                MessageBox.Show("model: " + ModelItem.Properties["workflowViewId"].Value + " selected: " + this.viewBox.SelectedValue);
                ModelItem cval = ModelItem.Properties["workflowViewId"].Value;
                ComboBoxItemConverter conv = new ComboBoxItemConverter();
                this.viewBox.SelectedValue = conv.Convert(cval, null, null, null).ToString().Replace("\"","");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler "+ex.Message);
            }
            //this.viewBox.SelectedValue = ModelItem.Properties["workflowViewId"].Value;
            //ModelItem.Properties["workflowViewId"].SetValue(ModelItem.Properties["workflowViewId"].Value);
            changing = false;*/
        }
    }
}
