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
using Cic.One.Workflow.Activities;
using System.Activities.Presentation.Metadata;
using System.Drawing;

namespace Cic.One.Workflow.Design
{
    // Interaction logic for ActivityDesigner1.xaml
    public partial class ActivityDesigner1
    {
        public ActivityDesigner1()
        {
            InitializeComponent();
        }
        public static void RegisterMetadata(AttributeTableBuilder builder)
        {
            builder.AddCustomAttributes(
                typeof(UserInteraction),
                new DesignerAttribute(typeof(ActivityDesigner1)),
                new DescriptionAttribute("My sample activity"),
                new ToolboxBitmapAttribute(typeof(UserInteraction), "CICOne.png"));
        } 
    }
}
