//===============================================================================
// Copyright 2006 Agilior, Sistemas de Informação Lda.  All rights reserved.
//
//
// Redistribution and use in source and binary forms, with or without modification, are permitted provided 
// that the following conditions are met:
//    * Redistributions of source code must retain the above copyright notice, this list 
//    of conditions and the following disclaimer.
//    * Neither the name of Agilior nor the names of its contributors may be used to endorse or 
//    promote products derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
// IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER 
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT 
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN 
// IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//===============================================================================
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Runtime.Serialization;

namespace Cic.OpenOne.Common.Util.SOAP.Annotations
{
    public class DataContractWithXsdAnnotationsSerializerOperationBehavior : DataContractSerializerOperationBehavior, IWsdlExportExtension
    {
        private DataContractSerializerOperationBehavior m_DataContractSerializerOperationBehavior;

        public DataContractWithXsdAnnotationsSerializerOperationBehavior(DataContractSerializerOperationBehavior originalSerializer, OperationDescription operation)
            : base(operation)
        {
            m_DataContractSerializerOperationBehavior = originalSerializer;

        }

        public DataContractWithXsdAnnotationsSerializerOperationBehavior(DataContractSerializerOperationBehavior originalSerializer, OperationDescription operation, DataContractFormatAttribute dataContractFormatAttribute)
            : base(operation, dataContractFormatAttribute)
        {
            m_DataContractSerializerOperationBehavior = originalSerializer;
        }

        public void ExportContract(WsdlExporter exporter, WsdlContractConversionContext context)
        {
            InitXsdDataContractExporter(exporter);
            typeof(IWsdlExportExtension).InvokeMember("ExportContract", BindingFlags.InvokeMethod, null, m_DataContractSerializerOperationBehavior, new object[] { exporter, context });
        }

        public void ExportEndpoint(WsdlExporter exporter, WsdlEndpointConversionContext context)
        {
            InitXsdDataContractExporter(exporter);
            typeof(IWsdlExportExtension).InvokeMember("ExportEndpoint", BindingFlags.InvokeMethod, null, m_DataContractSerializerOperationBehavior, new object[] { exporter, context });
        }

        void InitXsdDataContractExporter(WsdlExporter exporter)
        {
            object xsdDataContractExporterObj;
            if (!exporter.State.TryGetValue(typeof(XsdDataContractExporter), out xsdDataContractExporterObj))
            {
                xsdDataContractExporterObj = new XsdDataContractExporter(exporter.GeneratedXmlSchemas);
                exporter.State.Add(typeof(XsdDataContractExporter), xsdDataContractExporterObj);
            }

            XsdDataContractExporter xsdDataContractExporter = xsdDataContractExporterObj as XsdDataContractExporter;

            if (xsdDataContractExporter.Options == null)
            {
                xsdDataContractExporter.Options = new ExportOptions();
                xsdDataContractExporter.Options.DataContractSurrogate = new DataContractXsdAnnotationSurrogate();
            }
        }
    }

}
