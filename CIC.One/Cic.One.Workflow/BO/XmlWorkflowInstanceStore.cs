
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DAO;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Serialization;
using System;
using System.Activities.DurableInstancing;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.DurableInstancing;
using System.Runtime.Serialization;
using System.ServiceModel.Persistence;
using System.Text;
using System.Xml;
using System.Xml.Linq;



namespace Cic.One.Workflow.BO
{
   

      
    /// <summary>
    /// File Storage for WF4 Workflow states
    /// Saves to  string fileName = IOHelper.GetFileName(this.ownerInstanceID);
    /// 
    /// </summary>
    public class XmlWorkflowInstanceStore : InstanceStore
    {
        private Guid ownerInstanceID;
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static bool useDB = true;
        public XmlWorkflowInstanceStore()
            : this(Guid.NewGuid())
        {

        }

        public XmlWorkflowInstanceStore(Guid id)
        {
            ownerInstanceID = id;
        }

        /// <summary>
        /// Deletes all workflow storage files
        /// </summary>
        public static void cleanStorage()
        {
            try
            {
                if (useDB)
                {
                    using (PrismaExtended ctx = new PrismaExtended())
                    {
                        ctx.ExecuteStoreCommand("delete from regvar where area='WF4' and chgdate=trunc(sysdate-1)",null);
                    }
                }
                else
                {


                    string fileName = IOHelper.GetFileName(Guid.NewGuid());
                    String path = Path.GetDirectoryName(fileName);
                    System.IO.DirectoryInfo di = new DirectoryInfo(path);
                    int i = 0;
                    foreach (FileInfo file in di.GetFiles())
                    {
                        bool isYesterday = DateTime.Today - file.CreationTime.Date >= TimeSpan.FromDays(1);
                        if (isYesterday)
                        {
                            file.Delete();
                            i++;
                        }
                    }
                    _log.Debug("Cleanup WF4 for " + i + " items succeeded");
                }
            }catch(Exception e)
            {
                _log.Error("Error cleaning WF4-Storage", e);
            }
        }

        //Synchronous version of the Begin/EndTryCommand functions
        protected override bool TryCommand(InstancePersistenceContext context, InstancePersistenceCommand command, TimeSpan timeout)
        {
            return EndTryCommand(BeginTryCommand(context, command, timeout, null, null));
        }

        /// <summary>
        /// Returns a regvar configured report by its unique id or null if not found
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected String getWorkflowById(String id)
        {
            String uid = id.Substring(0, 30);
            AppSettingsBo asb = new AppSettingsBo(new AppSettingsDao());
            RegVarDto result = asb.getAppSettingsItem(new igetAppSettingsItemsDto() { bezeichnung = "/USERS/USRCACHEWF/" +uid, syswfuser = -1 });
            if (result != null && result.blobWert != null)
                return result.blobWert;
            return null;
        }
        protected void setWorkflowById(String id, String wf)
        {
            String uid = id.Substring(0, 30);
            AppSettingsDao settingsDao = new AppSettingsDao();
            AppSettingsBo asb = new AppSettingsBo(new AppSettingsDao());

            RegVarDto rv = new RegVarDto()
                {
                    completePath = "/USERS/USRCACHEWF/" + uid,
                    code = uid,
                    wert = "0",
                    area = "WF4",
                    syswfuser = -1
                };
            rv.blobWert = wf;
            settingsDao.createOrUpdateAppSettingsItem(new icreateOrUpdateAppSettingsItemDto()
            {
                regVar = rv,
                sysWfuser = -1
            });
        }
        public static Stream GenerateStreamFromString(string s)
        {
            // New encoding
            System.Text.UTF8Encoding UTF8Encoding = new System.Text.UTF8Encoding();
            return new System.IO.MemoryStream(UTF8Encoding.GetBytes(s));
            // New memory stream
/*            using (System.IO.MemoryStream MemoryStream = new System.IO.MemoryStream(UTF8Encoding.GetBytes(s)))
            {
                // New serializer
                XmlSerializer = XMLSerializer.getSerializer(type);
                // Deserialize
                Result = XmlSerializer.Deserialize(MemoryStream);
                // Close stream
                MemoryStream.Close();
            }

            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;*/
        }

        //The persistence engine will send a variety of commands to the configured InstanceStore,
        //such as CreateWorkflowOwnerCommand, SaveWorkflowCommand, and LoadWorkflowCommand.
        //This method is where we will handle those commands
        protected override IAsyncResult BeginTryCommand(InstancePersistenceContext context, InstancePersistenceCommand command, TimeSpan timeout, AsyncCallback callback, object state)
        {
            
            IDictionary<XName, InstanceValue> data = null;

            //The CreateWorkflowOwner command instructs the instance store to create a new instance owner bound to the instanace handle
            if (command is CreateWorkflowOwnerCommand)
            {
                context.BindInstanceOwner(ownerInstanceID, Guid.NewGuid());
            }
            //The SaveWorkflow command instructs the instance store to modify the instance bound to the instance handle or an instance key
            else if (command is SaveWorkflowCommand)
            {
                SaveWorkflowCommand saveCommand = (SaveWorkflowCommand)command;
                data = saveCommand.InstanceData;

                Save(data);
            }
            //The LoadWorkflow command instructs the instance store to lock and load the instance bound to the identifier in the instance handle
            else if (command is LoadWorkflowCommand)
            {
                if(useDB)
                { 
                    String wfdata = getWorkflowById(this.ownerInstanceID.ToString());
                    if (wfdata != null)
                    {
                        using (Stream inputStream = GenerateStreamFromString(wfdata))
                        {
                            data = LoadInstanceDataFromFile(inputStream);
                            //load the data into the persistence Context
                            context.LoadedInstance(InstanceState.Initialized, data, null, null, null);
                        }
                    }
                }
                else
                {
                    string fileName = IOHelper.GetFileName(this.ownerInstanceID);

                    try
                    {
                        using (FileStream inputStream = new FileStream(fileName, FileMode.Open))
                        {
                            data = LoadInstanceDataFromFile(inputStream);
                            //load the data into the persistence Context
                            context.LoadedInstance(InstanceState.Initialized, data, null, null, null);
                        }
                    }
                    catch (Exception exception)
                    {
                        throw new PersistenceException(exception.Message);
                    }
                }
            }

            return new CompletedAsyncResult<bool>(true, callback, state);
        }

        protected override bool EndTryCommand(IAsyncResult result)
        {
            return CompletedAsyncResult<bool>.End(result);
        }

        //Reads data from xml file and creates a dictionary based off of that.
        IDictionary<XName, InstanceValue> LoadInstanceDataFromFile(Stream inputStream)
        {
            IDictionary<XName, InstanceValue> data = new Dictionary<XName, InstanceValue>();

            NetDataContractSerializer s = new NetDataContractSerializer();

            XmlReader rdr = XmlReader.Create(inputStream);
            XmlDocument doc = new XmlDocument();
            doc.Load(rdr);

            XmlNodeList instances = doc.GetElementsByTagName("InstanceValue");
            foreach (XmlElement instanceElement in instances)
            {
                XmlElement keyElement = (XmlElement)instanceElement.SelectSingleNode("descendant::key");
                XName key = (XName)DeserializeObject(s, keyElement);

                XmlElement valueElement = (XmlElement)instanceElement.SelectSingleNode("descendant::value");
                object value = DeserializeObject(s, valueElement);
                InstanceValue instVal = new InstanceValue(value);

                data.Add(key, instVal);
            }

            return data;
        }

        object DeserializeObject(NetDataContractSerializer serializer, XmlElement element)
        {
            object deserializedObject = null;

            MemoryStream stm = new MemoryStream();
            XmlDictionaryWriter wtr = XmlDictionaryWriter.CreateTextWriter(stm);
            element.WriteContentTo(wtr);
            wtr.Flush();
            stm.Position = 0;

            deserializedObject = serializer.Deserialize(stm);

            return deserializedObject;
        }

        private XmlDocument ChangeXmlEncoding(XmlDocument xmlDoc, string newEncoding)
        {
            if (xmlDoc.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
            {
                XmlDeclaration xmlDeclaration = (XmlDeclaration)xmlDoc.FirstChild;
                xmlDeclaration.Encoding = newEncoding;
            }
            return xmlDoc;
        }

        //Saves the persistence data to an xml file.
        void Save(IDictionary<XName, InstanceValue> instanceData)
        {
            
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, root);
            XmlElement element1 = doc.CreateElement(string.Empty, "InstanceValues", string.Empty);
            doc.AppendChild(element1);

            //doc.LoadXml("<InstanceValues/>");

            foreach (KeyValuePair<XName, InstanceValue> valPair in instanceData)
            {
                XmlElement newInstance = doc.CreateElement("InstanceValue");

                XmlElement newKey = SerializeObject("key", valPair.Key, doc);
                newInstance.AppendChild(newKey);

                XmlElement newValue = SerializeObject("value", valPair.Value.Value, doc);
                newInstance.AppendChild(newValue);

                element1.AppendChild(newInstance);
                //doc.DocumentElement.AppendChild(newInstance);
            }
            if (useDB)
            {
               // doc = ChangeXmlEncoding(doc, "UTF-8");
                
                //using (var stringWriter = new StringWriter())
                using (var stringWriter = new StringWriterEncoded(Encoding.GetEncoding("UTF-8")))
                using (var xmlTextWriter = XmlWriter.Create(stringWriter))
                {
                    doc.WriteTo(xmlTextWriter);
                    xmlTextWriter.Flush();
                    String data = stringWriter.GetStringBuilder().ToString();
                    setWorkflowById(this.ownerInstanceID.ToString(), data);
                }
            }
            else
            {
                string fileName = IOHelper.GetFileName(this.ownerInstanceID);
                doc.Save(fileName);
            }
        }

        XmlElement SerializeObject(string elementName, object o, XmlDocument doc)
        {
            NetDataContractSerializer s = new NetDataContractSerializer();
            XmlElement newElement = doc.CreateElement(elementName);
            MemoryStream stm = new MemoryStream();

            s.Serialize(stm, o);
            stm.Position = 0;
            StreamReader rdr = new StreamReader(stm);
            newElement.InnerXml = rdr.ReadToEnd();

            return newElement;
        }
    }
}
