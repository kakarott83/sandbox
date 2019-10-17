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


using System.Reflection;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Service.DTO;
using System.ServiceModel;
using Cic.OpenOne.Common.Util.SOAP;


namespace Cic.OpenOne.TestUI
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            SampleServiceReference.SampleServiceClient cl = new SampleServiceReference.SampleServiceClient();
            using (OperationContextScope Scope = new OperationContextScope(cl.InnerChannel))
            {
                System.ServiceModel.Channels.MessageHeaders headers = System.ServiceModel.OperationContext.Current.OutgoingMessageHeaders;


                DefaultMessageHeader dmh = new DefaultMessageHeader();
                dmh.UserName = "Default";
                dmh.Password = "XAKLOP901ASDDDA";
                dmh.SysPEROLE = 47;
               
                dmh.ISOLanguageCode = "en";
                //dmh.UserType = 0;
                System.ServiceModel.MessageHeader<DefaultMessageHeader> mh = new System.ServiceModel.MessageHeader<DefaultMessageHeader>(dmh);
                //System.ServiceModel.Channels.MessageHeader myHeader = System.ServiceModel.Channels.MessageHeader.CreateHeader(dmh.getID(), dmh.getNamespace(), dmh, false ,"",true);

                headers.Add(mh.GetUntypedHeader(dmh.getID(), dmh.getNamespace()));

                iSampleMethodDto iParam = new iSampleMethodDto();
                iParam.inputData = "Hallo Welt";
                oSampleMethodDto result = cl.sampleMethod(iParam);
                if (result.message.type == MessageType.None)
                    resultTxt2.Text = "Duration: " + result.message.duration + "\nResult: " + result.sampleResult;
                else
                    resultTxt2.Text = "Duration: " + result.message.duration +
                        "\nError-Code: " + result.message.code +
                        "\nMessage: " + result.message.message +
                        "\nDetail: " + result.message.detail +
                        "\nStack: " + result.message.stacktrace;
            }


          
        }

        private void logBtn_Click(object sender, RoutedEventArgs e)
        {
            ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            _log.Debug("Logging...");
        }
    }
}
