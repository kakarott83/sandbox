//-------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved
//-------------------------------------------------------------------


using Cic.One.Workflow.Design.Utils;
namespace Cic.One.Workflow.ActivityDesigns
{
    // Interaction logic for MultiAssignDesigner.xaml
    public partial class CASCallDesigner
    {
        public CASCallDesigner()
        {
            InitializeComponent();
            //loads the icon into the WF4-Editor Activity
            this.Icon = ResourceLoading.loadIcon("QuestionMark.png");
            
        }
      
    }
}
