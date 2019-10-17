using System.Collections;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Xml.Schema;
using ServiceDescription = System.Web.Services.Description.ServiceDescription;

namespace Cic.OpenOne.Common.Util.Behaviour
{
    /// <summary>
    /// FlatWsdl-Klasse
    /// </summary>
    public class FlatWsdl : BehaviorExtensionElement, IWsdlExportExtension, IEndpointBehavior
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exporter"></param>
        /// <param name="context"></param>
        public void ExportEndpoint(WsdlExporter exporter, WsdlEndpointConversionContext context)
        {
            XmlSchemaSet generatedXmlSchemas = exporter.GeneratedXmlSchemas;

            foreach (ServiceDescription generatedWsdl in exporter.GeneratedWsdlDocuments)
            {
                var referencedXmlSchemas = FindAllReferencedXmlSchemasRecursively(generatedWsdl, generatedXmlSchemas);
                ClearWsdlOfExistingSchemas(generatedWsdl);
                AddAllReferencedSchemas(generatedWsdl, referencedXmlSchemas);
            }
            RemoveSchemaLocationFromXmlSchemaImports(exporter, generatedXmlSchemas);
        }

        private static IEnumerable<XmlSchema> FindAllReferencedXmlSchemasRecursively(ServiceDescription wsdl, XmlSchemaSet generatedXmlSchemas)
        {
            var referencedXmlSchemas = new List<XmlSchema>();
            foreach (XmlSchema schema in wsdl.Types.Schemas)
            {
                AddReferencedXmlSchemasRecursively(schema, generatedXmlSchemas, referencedXmlSchemas);
            }
            return referencedXmlSchemas;
        }

        /// <summary>
        /// Recursively extract all the list of imported
        /// schemas
        /// </summary>
        /// <param name="schema">Schema to examine</param>
        /// <param name="generatedXmlSchemas">SchemaSet with all referenced schemas</param>
        /// <param name="referencedXmlSchemas">List of all referenced schemas</param>
        private static void AddReferencedXmlSchemasRecursively(
           XmlSchema schema,
           XmlSchemaSet generatedXmlSchemas,
           List<XmlSchema> referencedXmlSchemas
           )
        {
            foreach (XmlSchemaImport import in schema.Includes)
            {
                ICollection realSchemas = generatedXmlSchemas.Schemas(import.Namespace);
                foreach (XmlSchema ixsd in realSchemas)
                {
                    if (!referencedXmlSchemas.Contains(ixsd))
                    {
                        referencedXmlSchemas.Add(ixsd);
                        AddReferencedXmlSchemasRecursively(ixsd, generatedXmlSchemas, referencedXmlSchemas);
                    }
                }
            }
        }

        private static void ClearWsdlOfExistingSchemas(ServiceDescription wsdl)
        {
            wsdl.Types.Schemas.Clear();
        }

        private static void AddAllReferencedSchemas(ServiceDescription wsdl, IEnumerable<XmlSchema> referencedXmlSchemas)
        {
            foreach (XmlSchema schema in referencedXmlSchemas)
            {
                wsdl.Types.Schemas.Add(schema);
            }
        }

        private static void RemoveSchemaLocationFromXmlSchemaImports(WsdlExporter exporter, XmlSchemaSet schemaSet)
        {
            var mySchemaSet = new XmlSchemaSet();
            mySchemaSet.Add(schemaSet);
            foreach (XmlSchema schema in mySchemaSet.Schemas())
            {
                exporter.GeneratedXmlSchemas.Remove(schema);
            }
        }

        /// <summary>
        /// ExportContract
        /// </summary>
        /// <param name="exporter"></param>
        /// <param name="context"></param>
        public void ExportContract(WsdlExporter exporter, WsdlContractConversionContext context)
        {
        }

        /// <summary>
        /// ExportEndpoint3
        /// </summary>
        /// <param name="exporter"></param>
        /// <param name="context"></param>
        public void ExportEndpoint3(WsdlExporter exporter, WsdlEndpointConversionContext context)
        {
            XmlSchemaSet schemaSet = exporter.GeneratedXmlSchemas;

            foreach (ServiceDescription wsdl in exporter.GeneratedWsdlDocuments)
            {
                List<XmlSchema> importsList = new List<XmlSchema>();

                foreach (XmlSchema schema in wsdl.Types.Schemas)
                {
                    AddImportedSchemas(schema, schemaSet, importsList);
                }

                if (importsList.Count == 0)
                {
                    continue;
                }
                wsdl.Types.Schemas.Clear();

                foreach (XmlSchema schema in importsList)
                {
                    RemoveXsdImports(schema);
                    wsdl.Types.Schemas.Add(schema);
                }
            }
        }

        /* public void ExportEndpoint2(WsdlExporter exporter, WsdlEndpointConversionContext context)
         {
             XmlSchemaSet schemaSet = exporter.GeneratedXmlSchemas;

             foreach (ServiceDescription wsdl in exporter.GeneratedWsdlDocuments)
             {
                 List<XmlSchema> importsList = new List<XmlSchema>();

                 foreach (XmlSchema schema in wsdl.Types.Schemas)
                 {
                     AddImportedSchemas(schema, schemaSet, importsList);
                 }

                 wsdl.Types.Schemas.Clear();

                 foreach (XmlSchema schema in importsList)
                 {
                     RemoveXsdImports(schema);
                     wsdl.Types.Schemas.Add(schema);
                 }
             }
         }*/

        private void AddImportedSchemas(XmlSchema schema, XmlSchemaSet schemaSet, List<XmlSchema> importsList)
        {
            foreach (XmlSchemaImport import in schema.Includes)
            {
                ICollection realSchemas = schemaSet.Schemas(import.Namespace);

                foreach (XmlSchema ixsd in realSchemas)
                {
                    if (!importsList.Contains(ixsd))
                    {
                        importsList.Add(ixsd);
                        AddImportedSchemas(ixsd, schemaSet, importsList);
                    }
                }
            }
        }

        private void RemoveXsdImports(XmlSchema schema)
        {
            for (int i = 0; i < schema.Includes.Count; i++)
            {
                if (schema.Includes[i] is XmlSchemaImport)
                    schema.Includes.RemoveAt(i--);
            }
        }

        /// <summary>
        /// AddBindingParameters
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="bindingParameters"></param>
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// ApplyClientBehavior
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="clientRuntime"></param>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        /// <summary>
        /// ApplyDispatchBehavior
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="endpointDispatcher"></param>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        /// <summary>
        /// Validate
        /// </summary>
        /// <param name="endpoint"></param>
        public void Validate(ServiceEndpoint endpoint)
        {
        }

        /// <summary>
        /// BehaviorType
        /// </summary>
        public override System.Type BehaviorType
        {
            get { return typeof(FlatWsdl); }
        }

        /// <summary>
        /// CreateBehavior
        /// </summary>
        /// <returns></returns>
        protected override object CreateBehavior()
        {
            return new FlatWsdl();
        }
    }
}