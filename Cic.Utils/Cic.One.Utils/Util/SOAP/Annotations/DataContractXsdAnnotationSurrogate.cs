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
using System.Runtime.Serialization;
using System.Reflection;
using System.Diagnostics;
using System.Xml;

namespace Cic.OpenOne.Common.Util.SOAP.Annotations
{
    public class DataContractXsdAnnotationSurrogate : IDataContractSurrogate
    {
        #region IDataContractSurrogate Members

        public object GetCustomDataToExport(Type clrType, Type dataContractType)
        {
            Debug.WriteLine("In GetCustomDataToExport");
            return null;
        }

      
        public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
        {
            Debug.Assert(memberInfo != null, "memberInfo argument is null");

            object[] dataMemberAnnotations = memberInfo.GetCustomAttributes(typeof(DataMemberAnnotationAttribute), false);

            if (dataMemberAnnotations != null && dataMemberAnnotations.Length > 0)
            {
                DataMemberAnnotationAttribute annotation = dataMemberAnnotations[0] as DataMemberAnnotationAttribute;
                Debug.WriteLine("Annotation to export:" + annotation.Annotation);

                //XmlDocument doc = new XmlDocument();
                //doc.LoadXml("<MyAnnotation xmlns=''>" + annotation.Annotation + "</MyAnnotation>");

                //return doc.DocumentElement;
                return annotation.Annotation;
            }
            return null;

        }

        public Type GetDataContractType(Type type)
        {
            return type;
            
        }

        public object GetDeserializedObject(object obj, Type targetType)
        {
            return obj;
        }

       

        public object GetObjectToSerialize(object obj, Type targetType)
        {
            return obj;
        }

        public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
        {
            return null;
        }

        public System.CodeDom.CodeTypeDeclaration ProcessImportedType(System.CodeDom.CodeTypeDeclaration typeDeclaration, System.CodeDom.CodeCompileUnit compileUnit)
        {
            return typeDeclaration;
        }

        #endregion


        public void GetKnownCustomDataTypes(System.Collections.ObjectModel.Collection<Type> customDataTypes)
        {
            
        }
    }
}
