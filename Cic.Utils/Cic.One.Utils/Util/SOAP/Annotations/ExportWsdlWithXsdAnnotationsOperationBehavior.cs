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
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Cic.OpenOne.Common.Util.SOAP.Annotations
{
    public class ExportWsdlWithXsdAnnotationsOperationBehavior : Attribute, IOperationBehavior
    {

        #region IOperationBehavior Members

        public void AddBindingParameters(OperationDescription description, System.ServiceModel.Channels.BindingParameterCollection parameters)
        {
        }

        public void ApplyClientBehavior(OperationDescription description, System.ServiceModel.Dispatcher.ClientOperation proxy)
        {
        }

        public void ApplyDispatchBehavior(OperationDescription description, System.ServiceModel.Dispatcher.DispatchOperation dispatch)
        {
            //TODO: Check if the behavior is enabled (using for example configuration)

            
            DataContractSerializerOperationBehavior dataContractSerializerOperationBehavior = description.Behaviors.Find<DataContractSerializerOperationBehavior>();
            //Remove the default operation behavior and add this new one
            if (dataContractSerializerOperationBehavior != null)
            {
                description.Behaviors.Remove(dataContractSerializerOperationBehavior);
                description.Behaviors.Add(
                    new DataContractWithXsdAnnotationsSerializerOperationBehavior(
                    dataContractSerializerOperationBehavior, description, dataContractSerializerOperationBehavior.DataContractFormatAttribute));

            }

        }

        public void Validate(OperationDescription description)
        {

        }

        #endregion

        #region IWsdlExportExtension Members


        #endregion
    }
}
