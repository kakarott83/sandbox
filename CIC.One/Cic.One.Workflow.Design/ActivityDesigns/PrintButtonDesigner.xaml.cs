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
using Cic.One.Web.BO;
using Cic.One.DTO;
using Cic.One.Workflow.Design.DataSource;
using Cic.One.Workflow.Design.Converter;
using System.Activities.Presentation.Services;

namespace Cic.One.Workflow.Design.ActivityDesigns
{
    // Interaction logic for PrintButtonDesigner.xaml
    public partial class PrintButtonDesigner
    {
        delegate void EventHandler<TEventArgs>(object sender, TEventArgs e) ;
        public PrintButtonDesigner()
        {
            InitializeComponent();
            //loads the icon into the WF4-Editor Activity
            this.Icon = ResourceLoading.loadIcon("CICOne.png");
            this.viewBox1.ItemsSource = new ViewItemSource().PrintAreaItems();
           
           
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
           if (e.PropertyName.Equals("docarea", StringComparison.OrdinalIgnoreCase))
            {
                IPrintBo bo = BOFactoryFactory.getInstance().getPrintBo();
                List<PrintDocumentDto> docs = bo.getDocumentList(ComboBoxItemConverter.getString(ModelItem.Properties["docarea"].Value));


                ModelItem.Properties["templates"].SetValue(docs);//new ViewItemSource().PrintItems(ComboBoxItemConverter.getString(ModelItem.Properties["docarea"].Value)));
               
                return;
            }
            //manages to write a activity property to another property of the activity
             if (!e.PropertyName.Equals("DisplayName", StringComparison.OrdinalIgnoreCase))
                 return;
             ModelItem.Properties["DisplayName"].SetValue(ModelItem.Properties["DisplayName"].Value );
        }

      
    }
}
