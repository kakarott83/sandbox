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
using Cic.One.Workflow.Design.Utils;

namespace Cic.One.Workflow.Design.ActivityDesigns
{
    // Interaction logic for AddMessageDesigner.xaml
    public partial class AddMessageDesigner
    {
        public AddMessageDesigner()
        {
            InitializeComponent();
            //loads the icon into the WF4-Editor Activity
            this.Icon = ResourceLoading.loadIcon("CICOne.png");
        }
    }
}
