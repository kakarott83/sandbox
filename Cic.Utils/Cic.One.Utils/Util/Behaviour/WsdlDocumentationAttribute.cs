using Cic.OpenOne.Common.Util.SOAP;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Xml;
using System.Xml.Schema;

namespace Cic.One.Utils.Util.Behaviour
{
    /// <summary>
    /// Allows to add wsdl description by adding as OperationContract and/or ServiceContract eg. [WsdlDocumentationAttribute("BNOW Gateway Service")]
    /// </summary>
    public class WsdlDocumentationAttribute : Attribute, IContractBehavior, IWsdlExportExtension
    {
        string text;
        XmlElement customWsdlDocElement = null;
        public WsdlDocumentationAttribute(string text)
        { this.text = text; }

        public WsdlDocumentationAttribute(XmlElement wsdlDocElement)
        { this.customWsdlDocElement = wsdlDocElement; }

        public XmlElement WsdlDocElement
        {
            get { return this.customWsdlDocElement; }
            set { this.customWsdlDocElement = value; }
        }
        public string Text
        {
            get { return this.text; }
            set { this.text = value; }
        }
        private List<RequiredMessagePart> _requiredParameter = new List<RequiredMessagePart>();
        public void ExportContract(WsdlExporter exporter, WsdlContractConversionContext context)
        {
            Console.WriteLine("Inside ExportContract");

            //Surrogate Type replacement:
            /*
            object dataContractExporter;
            XsdDataContractExporter xsdInventoryExporter;
            if (!exporter.State.TryGetValue(typeof(XsdDataContractExporter),
                out dataContractExporter))
            {
                xsdInventoryExporter = new XsdDataContractExporter(exporter.GeneratedXmlSchemas);
            }
            else
                xsdInventoryExporter = (XsdDataContractExporter)dataContractExporter;
            exporter.State.Add(typeof(XsdDataContractExporter), xsdInventoryExporter);    

            if (xsdInventoryExporter.Options == null)
                xsdInventoryExporter.Options = new ExportOptions();
            xsdInventoryExporter.Options.DataContractSurrogate = new StringSurrogated();
            */

            if (context.Contract != null)
            {
                //Documentation annotation for Contract
                // Set the document element; if this is not done first, there is no XmlElement in the   
                // DocumentElement property.  
                context.WsdlPortType.Documentation = string.Empty;
                // Contract comments.  
                XmlDocument owner = context.WsdlPortType.DocumentationElement.OwnerDocument;
                XmlElement summaryElement = Formatter.CreateSummaryElement(owner, this.Text);
                context.WsdlPortType.DocumentationElement.AppendChild(summaryElement);

                foreach (OperationDescription op in context.Contract.Operations)
                {

                    Operation operation = context.GetOperation(op);
                    
                    //Message Header self-description
                   /* MessageHeaderDescription header = new MessageHeaderDescription("DefaultMessageHeader", "http://cic-software.de/MessageHeader");
                    header.Type = typeof(DefaultMessageHeader);

                    foreach (MessageDescription msgDescription in operation.Messages)
                    {
                            msgDescription.Headers.Add(header);
                    }
                    */

                    //Documentation annotation for Contract Methods
                    object[] opAttrs = op.SyncMethod.GetCustomAttributes(typeof(WsdlDocumentationAttribute), false);
                    if (opAttrs.Length == 1)
                    {
                        string opComment = ((WsdlDocumentationAttribute)opAttrs[0]).Text;

                        // This.Text returns the string for the operation-level attributes.  
                        // Set the document element; if this is not done first, there is no XmlElement in the   
                        // DocumentElement property.  
                        operation.Documentation = String.Empty;

                        XmlDocument opOwner = operation.DocumentationElement.OwnerDocument;
                        XmlElement newSummaryElement = Formatter.CreateSummaryElement(opOwner, opComment);
                        operation.DocumentationElement.AppendChild(newSummaryElement);
                    }
                    //Documentation annotation end


                    //Possible hooks to input parameters/namespaces for later mapping during xml processing
                    /*
                    var inputMessage = op.Messages.Where(m => m.Direction == MessageDirection.Input).First();
                    var parameters = op.SyncMethod.GetParameters();
                    for (int i = 0; i < parameters.Length; i++)
                    {
                            //object[] attributes = parameters[i].GetCustomAttributes(typeof(OptionalAttribute), false);

                            //if (attributes.Length == 0)

                            // The parameter has no [Optional] attribute, add it
                            // to the list of parameters that we need to adjust
                            // the XML schema for later on.
                            _requiredParameter.Add(new RequiredMessagePart()
                            {
                                Namespace = inputMessage.Body.Parts[i].Namespace,
                                Message = operation.Name,
                                Name = inputMessage.Body.Parts[i].Name,
                                Member = parameters[i].Member
                            });
                    }
                    */

                }
            }
        }
        private class RequiredMessagePart
        {
            public String Namespace, Message, Name;
            public MemberInfo Member;
            public XmlQualifiedName XmlQualifiedName
            {
                get
                {
                    return new XmlQualifiedName(Message, Namespace);
                }
            }
        }

        private void processItems(XmlSchemaObjectCollection coll)
        {

            foreach (XmlSchemaObject el in coll)
            {
                if (processObject(el))
                {
                    if (el is XmlSchemaElement)
                    {
                        XmlSchemaElement se = (XmlSchemaElement)el;
                        XmlSchemaSimpleType newType = new XmlSchemaSimpleType();
                        XmlSchemaSimpleTypeRestriction tempRestriction = new XmlSchemaSimpleTypeRestriction();
                        XmlSchemaMaxLengthFacet mlen = new XmlSchemaMaxLengthFacet();
                        mlen.Value = "777";
                        tempRestriction.Facets.Add(mlen);
                        XmlSchemaLengthFacet lengthFacet = new XmlSchemaLengthFacet();
                        tempRestriction.BaseTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");
                        tempRestriction.Facets.Add(lengthFacet);

                        lengthFacet.Value = "888";
                        newType.Content = tempRestriction;
                        ((XmlSchemaSimpleType)se.ElementSchemaType).Content = newType.Content;


                        se.SchemaType = newType;
                    }

                }
            }
        }
        private Dictionary<String, bool> processed = new Dictionary<string, bool>();
        private bool processObject(object el)
        {
            XmlSchemaObject obj = (XmlSchemaObject)el;
            
            if (el is XmlSchemaComplexType)
            {
                XmlSchemaComplexType cplx = ((XmlSchemaComplexType)el);
                XmlSchemaParticle part = cplx.Particle;
                if(part==null)
                {
                    part = cplx.ContentTypeParticle;
                }
                if (part == null) return false;
                if (processed.ContainsKey(cplx.QualifiedName.Name)) return false;
                processed[cplx.QualifiedName.Name] = true;
                
                if (part is XmlSchemaSequence)
                {

                    XmlSchemaObjectCollection coll2 = ((XmlSchemaSequence)part).Items;//recurse line 123
                    processItems(coll2);
                }

            }
            else if (el is XmlSchemaSimpleType)
            {
                XmlSchemaSimpleType st = ((XmlSchemaSimpleType)el);
                
                if (st.Datatype != null && st.Datatype.TypeCode == XmlTypeCode.String)
                {

                    return true;
                   
                }
            }
            else if (el is XmlSchemaElement)
            {
                XmlSchemaElement se = ((XmlSchemaElement)el);
                
                if (se.ElementSchemaType != null)
                {
                    
                    String fieldName = se.Name;
                    if("matchcode".Equals(fieldName))
                    {
                        int t = 0;
                        t++;
                    }
                    //currently this only works for SimpleTypes (like Enums). plain types like string wont have an explicit xs:simpleType and therefore no restrictions
                    //the only thing this succeeds in is that the enum will become a string!
                    if(processObject(se.ElementSchemaType))
                    {
                        XmlSchemaSimpleType newType = new XmlSchemaSimpleType();
                        XmlSchemaSimpleTypeRestriction tempRestriction = new XmlSchemaSimpleTypeRestriction();
                        XmlSchemaMaxLengthFacet mlen = new XmlSchemaMaxLengthFacet();
                        mlen.Value = "777";
                        tempRestriction.Facets.Add(mlen);
                        XmlSchemaLengthFacet lengthFacet = new XmlSchemaLengthFacet();
                        tempRestriction.BaseTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");
                        tempRestriction.Facets.Add(lengthFacet);                        
                        lengthFacet.Value = "888";
                        newType.Content = tempRestriction;                        
                        ((XmlSchemaSimpleType)se.ElementSchemaType).Content = newType.Content;
                        
                    }
                }
            }
            return false;
        }
        public void ExportEndpoint(WsdlExporter exporter, WsdlEndpointConversionContext context)
        {

           /* var schemas1 = exporter.GeneratedXmlSchemas.Schemas("http://schemas.datacontract.org/2004/07/Cic.One.DTO");
            foreach (XmlSchema schema in schemas1)
            {
                processItems(schema.Items);
            }*/
            /*
            //Sample for minOccurs Annotation Processing
                foreach (var p in _requiredParameter)
            {
                var schemas = exporter.GeneratedXmlSchemas.Schemas(p.Namespace);

                foreach (XmlSchema schema in schemas)
                {
                   // for(XmlSchemaObject el in schema.Items)

                    var message = (XmlSchemaElement)schema.Elements[p.XmlQualifiedName];

                    /*var complexType = (XmlSchemaComplexType)message.ElementSchemaType;
                    var sequence = (XmlSchemaSequence)complexType.Particle;

                    foreach (XmlSchemaElement item in sequence.Items)
                    {
                        if (item.Name == p.Name)
                        {
                            item.MinOccurs = 1;
                            item.MinOccursString = "1";
                        }
                    }
                }
            }*/
        }

        public void AddBindingParameters(ContractDescription description, ServiceEndpoint endpoint, BindingParameterCollection parameters)
        { return; }

        public void ApplyClientBehavior(ContractDescription description, ServiceEndpoint endpoint, ClientRuntime client)
        { return; }

        public void ApplyDispatchBehavior(ContractDescription description, ServiceEndpoint endpoint, DispatchRuntime dispatch)
        { return; }

        public void Validate(ContractDescription description, ServiceEndpoint endpoint) { return; }
    }

    public class Formatter
    {

        #region Utility Functions  

        public static XmlElement CreateSummaryElement(XmlDocument owningDoc, string text)
        {
            XmlElement summaryElement = owningDoc.CreateElement("summary");
            summaryElement.InnerText = text;
            return summaryElement;
        }

        public static CodeCommentStatementCollection FormatComments(string text)
        {
            /*  
             * Note that in Visual C# the XML comment format absorbs a   
             * documentation element with a line break in the middle. This sample  
             * could take an XmlElement and create code comments in which   
             * the element never had a line break in it.  
            */

            CodeCommentStatementCollection collection = new CodeCommentStatementCollection();
            collection.Add(new CodeCommentStatement("From WsdlDocumentation:", true));
            collection.Add(new CodeCommentStatement(String.Empty, true));

            foreach (string line in WordWrap(text, 80))
            {
                collection.Add(new CodeCommentStatement(line, true));
            }

            collection.Add(new CodeCommentStatement(String.Empty, true));
            return collection;
        }

        public static Collection<string> WordWrap(string text, int columnWidth)
        {
            Collection<string> lines = new Collection<string>();
            System.Text.StringBuilder builder = new System.Text.StringBuilder();

            string[] words = text.Split(' ');
            foreach (string word in words)
            {
                if ((builder.Length > 0) && ((builder.Length + word.Length + 1) > columnWidth))
                {
                    lines.Add(builder.ToString());
                    builder = new System.Text.StringBuilder();
                }
                builder.Append(word);
                builder.Append(' ');
            }
            lines.Add(builder.ToString());

            return lines;
        }

        #endregion

        public static XmlElement CreateReturnsElement(XmlDocument owner, string p)
        {
            XmlElement returnsElement = owner.CreateElement("returns");
            returnsElement.InnerText = p;
            return returnsElement;
        }
    }
}
