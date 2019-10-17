using System;
using System.Collections.Generic;
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
using Cic.OpenOne.GateBANKNOW.TESTAUSKUNFTUI.UserControls;


namespace Cic.OpenOne.GateBANKNOW.TESTAUSKUNFTUI
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.EurotaxGetForecast EurotaxGetForecast = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF.EurotaxGetForecast();
            Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.EurotaxInDto _eiorig = new Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.EurotaxInDto();
            NewAuskunft Eurotax = new Cic.OpenOne.GateBANKNOW.TESTAUSKUNFTUI.UserControls.NewAuskunft("TestAuskunft", _eiorig, EurotaxGetForecast);

            var tabItemEurotax = new System.Windows.Controls.TabItem();
            tabItemEurotax.Header = "Eurotax";
            tabItemEurotax.TabIndex = 1;
            tabItemEurotax.Content = Eurotax;
            tabcontrol.Items.Add(tabItemEurotax);
        }
    }
}
