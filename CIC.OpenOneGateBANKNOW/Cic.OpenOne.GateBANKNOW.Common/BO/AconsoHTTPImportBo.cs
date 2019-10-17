using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Serialization;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO.AconsoBatchReturn;
using Cic.OpenOne.GateBANKNOW.Extern;
using CIC.Database.OW.EF6.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    /// <summary>
    /// Aconso DMS Use-Case Strings for IMPORT_CONFIG
    /// </summary>
    public enum AconsoImportConfig
    {
        // new Use-Cases
        [StringValue("CreateStammsatzOhneVGNR")]
        CreateStammsatzOhneVGNR,
        [StringValue("UpdateStammsatzMitAkteID")]
        UpdateStammsatzMitAkteID,
        [StringValue("UpdateStammsatzMitKNR")]
        UpdateStammsatzMitKNR,
        [StringValue("UpdateStammsatzMitITID")]
        UpdateStammsatzMitITID,

        [StringValue("UpdateStammsatzMappeZuAkte")]
        UpdateStammsatzMappeZuAkte,
        [StringValue("UploadDokumentMitAKTEID")]
        UploadDokumentMitAKTEID,
        [StringValue("UploadDocumentMitAKTEID")]
        UploadDocumentMitAKTEID,
		[StringValue("UploadDocumentMitREFERENZ")]		// rh 20181018
		UploadDocumentMitREFERENZ,

		// Old Configs:
		// set attribute for a document
		[StringValue ("documentAttribution")]
        documentAttribution,
        // Import Attributes
        [StringValue("masterdataImport")]
        masterdataImport,
        // Import a PDF
        [StringValue("documentImport")]
        documentImport,


        [StringValue("BatchUpdateMitAkteID")]
        BatchUpdateMitAkteID,
        [StringValue("BatchUpdateMitPartnerNr")]
        BatchUpdateMitPartnerNr
    }

    public class AconsoException : System.Exception
    {
        public AconsoException(String msg) : base(msg) { }
    }
    /// <summary>
    /// Manages the ACONSO HTTP IMPORT INTERFACE
    /// </summary>
    public class AconsoHTTPImportBo
    {
        private Document attrs = new Document();
        private List<Cic.OpenOne.GateBANKNOW.Common.DTO.AconsoBatch.Record> records = new List<DTO.AconsoBatch.Record>();
        private ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);



        // Create request and receive response


        private byte[] attrdata = null;
        private byte[] filedata = null;
        private List<Field> masterDataFields;
        private List<Field> attribFields;
        private List<Field> componentFields;
        private Dictionary<string, object> postParameters = new Dictionary<string, object>();
        private String filetype, filename;
        private AconsoImportConfig importConfig;
        private DMSConfig config;

        public AconsoHTTPImportBo(DMSConfig config)
        {

            this.importConfig = AconsoImportConfig.masterdataImport;
            this.config = config;
        }

        /*public void test()
        {
            String pars = "alsdjfasldf";
            String test = createSignature(findCertificate("c:\\temp\\testcert.p12", "test"), pars);
            _log.Debug(test);

            test = createSignature(findCertificate("c:\\temp\\cert\\dms.p12", "IayLdj7o"), pars);

            test = createSignature(findCertificate(System.Security.Cryptography.X509Certificates.StoreLocation.CurrentUser, "CicOneDMSCertificate"), pars);
            
            
            _log.Debug(test);
        }*/


        /// <summary>
        /// Sets the aconso interface configuration to a use-case
        /// </summary>
        /// <param name="cfg"></param>
        public void setUseCase(AconsoImportConfig cfg)
        {
            this.importConfig = cfg;

        }

        /// <summary>
        /// Returns the current aconso interface configuration
        /// </summary>
        /// <returns></returns>
        public AconsoImportConfig getUseCase()
        {
            return this.importConfig;
        }

        /// <summary>
        /// set the file to upload
        /// </summary>
        /// <param name="data"></param>
        /// <param name="contentType"></param>
        /// <param name="name"></param>
        public void addFileData(byte[] data, String contentType, String name)
        {
            filedata = data;
            filetype = contentType;
            filename = name;

            addComponentField("id", "DATA");
            addComponentField("mimetype", contentType);
            addComponentField("size", "" + data.Length);
            addComponentField("checksum", getDataChecksum());
            addComponentField("checksumAlgorithm", "SHA-256");
            addComponentField("checksumEncoding", "HEX");
        }
        private static XmlSerializerNamespaces xmlnamespace;

        /// <summary>
        /// return the interface attribute xml as bytes
        /// </summary>
        /// <returns></returns>
        private byte[] getAttr()
        {
            if (this.importConfig == AconsoImportConfig.BatchUpdateMitAkteID || this.importConfig == AconsoImportConfig.BatchUpdateMitPartnerNr)
            {
                
                if (xmlnamespace == null)
                {
                    xmlnamespace = new XmlSerializerNamespaces();
                    xmlnamespace.Add("aconso", "http://www.aconso.com/xml/ns/Records");
                }
                DTO.AconsoBatch.Records recs = new DTO.AconsoBatch.Records();
                recs.Record = records.ToArray();

                XmlSerializer xser = XMLSerializer.getSerializer(typeof(DTO.AconsoBatch.Records));//<-RECORDS here
                System.IO.StringWriter stWrite = new StringWriterEncoded(Encoding.GetEncoding("UTF-8"));
                xser.Serialize(stWrite, recs, xmlnamespace);
                stWrite.Close();
                String attrString = stWrite.ToString();
                _log.Debug("ATTR.xml = " + attrString);
                attrdata = Encoding.GetEncoding("UTF-8").GetBytes(attrString);
                return attrdata;
            }
            else
            {
                if (masterDataFields != null)
                {
                    this.attrs.MasterData = this.masterDataFields.ToArray();
                }
                if (attribFields != null)
                {
                    this.attrs.Attributes = this.attribFields.ToArray();
                }
                if (componentFields != null)
                {
                    attrs.Components = new Component[1];
                    attrs.Components[0] = new Component();
                    attrs.Components[0].Field = componentFields.ToArray();
                    attrs.Components[0].id = "DATA";
                }
                if (xmlnamespace == null)
                {
                    xmlnamespace = new XmlSerializerNamespaces();
                    xmlnamespace.Add("aconso", "http://www.aconso.com/xml/ns/Document");
                }

                XmlSerializer xser = XMLSerializer.getSerializer(typeof(Document));
                System.IO.StringWriter stWrite = new StringWriterEncoded(Encoding.GetEncoding("UTF-8"));
                xser.Serialize(stWrite, attrs, xmlnamespace);
                stWrite.Close();
                String attrString = stWrite.ToString();
                _log.Debug("ATTR.xml = " + attrString);
                attrdata = Encoding.GetEncoding("UTF-8").GetBytes(attrString);
                return attrdata;
            }
        }
        /// <summary>
        /// calculate the attribute file checksum
        /// </summary>
        /// <returns></returns>
        public String getAttrChecksum()
        {
            if (attrdata == null)
            {
                getAttr();
            }

            var sha = new SHA256Managed();
            byte[] checksum = sha.ComputeHash(attrdata);
            return BitConverter.ToString(checksum).Replace("-", String.Empty);
        }
        /// <summary>
        /// calculate the document file checksum
        /// </summary>
        /// <returns></returns>
        public String getDataChecksum()
        {

            var sha = new SHA256Managed();
            byte[] checksum = sha.ComputeHash(filedata);
            return BitConverter.ToString(checksum).Replace("-", String.Empty);
        }
        /// <summary>
        /// get the interface current timestamp
        /// </summary>
        /// <returns></returns>       
        public String getTimestamp()
        {
            return DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        }
        /// <summary>
        /// builds the complete url-suffix including checksums and signature
        /// also prepares the formular post-parameter contents (needed for checksum+signature calculation)
        /// </summary>
        /// <returns></returns>
        private String getUrlSuffix()
        {
            postParameters.Add("ATTR", new FormUpload.FileParameter(getAttr(), "attr.xml", "text/xml"));
            StringBuilder sb = new StringBuilder();

            if (importConfig == AconsoImportConfig.BatchUpdateMitAkteID || importConfig==AconsoImportConfig.BatchUpdateMitPartnerNr )
            {
                //batch-parameters
            }
            else
            {
                //non-batch parameters
                sb.Append(addParam("action", "importDocument"));
                sb.Append(addParam("IMPORT_CONFIG", importConfig.ToString()));
            }

            sb.Append(addParam("ATTR_CHECKSUM", getAttrChecksum()));

            if (filedata != null)
            {
                postParameters.Add("DATA", new FormUpload.FileParameter(filedata, filename, filetype));
                sb.Append(addParam("DATA_CHECKSUM", getDataChecksum()));
            }

            sb.Append(addParam("timestamp", getTimestamp(), false));
            String pars = sb.ToString();

            X509Certificate2 cert = null;
            if (config.certpassword != null && config.certpassword.Length > 0)
            {
                cert = findCertificate (this.config.certpath, this.config.certpassword);
            }
            else
            {
                cert = findCertificate (System.Security.Cryptography.X509Certificates.StoreLocation.LocalMachine, this.config.certsubject);
            }
            if(cert==null)
            {
                throw new Exception ("ACONSO Certificate not found for Subject " + config.certsubject + " or password <" + config.certpassword + "> at " + config.certpath);
            }
            sb.Append (addParam ("&signature", createSignature(cert, pars), false));

            return sb.ToString();
        }

        /// <summary>
        /// calls the aconso dms, performing an update or import of document
        /// Returns the Dokumenten-ID
        /// </summary>
        public String call (DMSAKTE akte)
        {	try
            {
                //reset errors:
                akte.ERRCODE = null;
                akte.ERRMESSAGE = null;
                akte.ERRTYPE = null;
                akte.RETCODE = null;

                _log.Debug("Calling DMS to " + config.interfaceUrl + " userAgent: " + config.userAgent);
                String suffix = getUrlSuffix();
                _log.Debug("URL: "+config.interfaceUrl+suffix);

                akte.REQDATE = DateTime.Now;
                akte.REQTIME = DateTimeHelper.DateTimeToClarionTime(DateTime.Now);

                HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(config.interfaceUrl + suffix, config.userAgent, postParameters, config.timeout);
                if (webResponse == null)
                { 
                    akte.RETCODE = "503";
                    throw new Exception("Connection to " + config.interfaceUrl + " could not be established");
                }

                Stream rs = webResponse.GetResponseStream();
                if (rs == null)
                {
                    akte.RETCODE = "503";
                    throw new Exception("Connection to " + config.interfaceUrl + " could not be established, no data received");
                }
                // Process response
                StreamReader responseReader = new StreamReader(rs);
                string fullResponse = responseReader.ReadToEnd();
                webResponse.Close();

                akte.RETCODE = "" + (int)webResponse.StatusCode;
                result aconsoResult = XMLDeserializer.objectFromXml<result>(System.Text.Encoding.UTF8.GetBytes(fullResponse), "UTF-8");
                _log.Debug("DMS-Call Result: " + aconsoResult.status);

                akte.RETRYONERR = 0;

                if (aconsoResult.details != null)
                {
                    foreach (item i in aconsoResult.details)
                    {
                        //process result
                        _log.Debug("GOT ITEM :" + i.id + "=" + i.Value);
                        //BNRDR-2400 Dokumenten-ID von Fremdapplikation in DMSAKTE speichern
                        if("RETURN_FIELD".Equals(i.id))
                        {
                            //akte.EXTID = i.Value;
                            return i.Value;
                        }
                    }
                }
            }
            catch (WebException ex)//for HTTP != 200 or 201 we jump into this section (so for 400/500)
            {
                _log.Error("DMS communication failed: " + ex.Message, ex);
                if(ex.InnerException!=null)
                    _log.Error("detailled error: " + ex.InnerException.Message, ex.InnerException);

                akte.ERRMESSAGE = ex.Message;
                akte.RETCODE = "";
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    var response = ex.Response as HttpWebResponse;
                    if (response != null)
                    {
                        akte.RETCODE = "" + (int) response.StatusCode;
                    }
                    else
                    {
                        akte.RETCODE = "418"; // no http status code available
                    }
                }
                else
                {
                    akte.RETCODE = "418"; // no http status code available	// ANM. rh: diese Zeile wird, wenn ex.Response == null, durch nächste ELSE-Abzweigung überschrieben  (akte.RETCODE = "503") 
				}

                if (ex.Response != null)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            StringBuilder seb = new StringBuilder();
                            seb.Append(reader.ReadToEnd());
                            String errXml = seb.ToString();
                            String errMsg = Substring(errXml, "<aconso:errorMessage>", "</aconso:errorMessage>");
                            String errCode = Substring(errXml, "<aconso:errorCode>", "</aconso:errorCode>");
                            String errType = Substring(errXml, "<aconso:errorType>", "</aconso:errorType>");
                            String emsg = "DMS-Update delivered error for SYSDMSAKTE=" + akte.SYSDMSAKTE + " " + akte.AREA + "/" + akte.SYSID + " Message: " + errMsg + " Code: " + errCode + " Type: " + errType;
                            // _log.Error(emsg);

                            akte.ERRMESSAGE = errMsg;
                            akte.ERRCODE = errCode;
                            akte.ERRTYPE = errType;
                            throw new AconsoException(emsg);
                        }
                    }
                }
                else if("".Equals(akte.RETCODE))
                {
                    akte.RETCODE = "503";
                }

            }
            catch (Exception e)
            {
                akte.ERRMESSAGE = e.Message;
                akte.ERRCODE = "0";
                akte.ERRTYPE = "permanent";

                _log.Error("DMS communication failed", e);
                //dont throw, error is logged in dmsakte
            }
            return null;
        }

        public ocreateOrUpdateDMSAkteBatchDto callBatch()
        {
            ocreateOrUpdateDMSAkteBatchDto rval = new ocreateOrUpdateDMSAkteBatchDto();
            rval.errcode = "0";
            try
            {

                _log.Debug("Calling DMS to " + config.interfaceUrl + " userAgent: " + config.userAgent);
                String suffix = getUrlSuffix();
                _log.Debug("URL: " + config.interfaceUrl + suffix);

                HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(config.interfaceUrl + suffix, config.userAgent, postParameters, config.timeoutBatch);
                if (webResponse == null)
                {
                    throw new Exception("Connection to " + config.interfaceUrl + " could not be established");
                }

                Stream rs = webResponse.GetResponseStream();
                if (rs == null)
                {
                    throw new Exception("Connection to " + config.interfaceUrl + " could not be established, no data received");
                }
                // Process response
                StreamReader responseReader = new StreamReader(rs);
                string fullResponse = responseReader.ReadToEnd();
                webResponse.Close();

                rval.retcode = "" + (int)webResponse.StatusCode;
                
                Records aconsoResult = XMLDeserializer.objectFromXml<Records>(System.Text.Encoding.UTF8.GetBytes(fullResponse), "UTF-8");

                if (aconsoResult != null && aconsoResult.Record != null)
                {
                    using (PrismaExtended ctx = new PrismaExtended())
                    {
                        foreach (Record i in aconsoResult.Record)
                         {
                                //process results
                                String errmsg = i.statustext, errcode = ""+i.status;
                                long sysdmsakte = i.id;

                                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysdmsakte", Value = sysdmsakte });
                                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "errcode", Value = errcode });
                                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "errmessage", Value = errmsg });
                                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "retcode", Value = rval.retcode });
                                ctx.ExecuteStoreCommand("UPDATE DMSAKTE set errcode=:errcode, errmessage=:errmessage, retcode=:retcode where sysdmsakte=:sysdmsakte", parameters.ToArray());
                                if ("2".Equals(errcode))
                                    rval.errcode = "2";
                            
                        }
                    }
                }
                 
            }
            catch (WebException ex)//for HTTP != 200 or 201 we jump into this section (so for 400/500)
            {
                rval.errmessage = ex.Message;
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    var response = ex.Response as HttpWebResponse;
                    if (response != null)
                    {
                        rval.retcode = "" + (int)response.StatusCode;
                    }
                    else
                    {
                        rval.retcode = "418"; // no http status code available
                    }
                }
                else
                {
                    rval.retcode = "418"; // no http status code available	// ANM. rh: diese Zeile wird, wenn ex.Response == null, durch nächste ELSE-Abzweigung überschrieben  (akte.RETCODE = "503") 
                }

                if (ex.Response != null)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            StringBuilder seb = new StringBuilder();
                            seb.Append(reader.ReadToEnd());
                            String errXml = seb.ToString();
                            String errMsg = Substring(errXml, "<aconso:errorMessage>", "</aconso:errorMessage>");
                            String errCode = Substring(errXml, "<aconso:errorCode>", "</aconso:errorCode>");
                            String errType = Substring(errXml, "<aconso:errorType>", "</aconso:errorType>");
                            throw new AconsoException("Batch-Processing failed with "+errMsg);
                        }
                    }
                }
                else
                {
                    rval.retcode = "503";
                }
                throw new AconsoException("Batch-Processing failed with Code " + rval.retcode);
            }
            catch (Exception e)
            {
                _log.Error("DMS communication failed", e);
                throw new AconsoException("Batch-Processing failed with Code " + e.Message);
            }
            return rval;
        }

        /// <summary>
        /// takes a substring between two anchor strings (or the end of the string if that anchor is null)
        /// </summary>
        /// <param name="this">a string</param>
        /// <param name="from">an optional string to search after</param>
        /// <param name="until">an optional string to search before</param>
        /// <param name="comparison">an optional comparison for the search</param>
        /// <returns>a substring based on the search</returns>
        private static string Substring(String str, string from = null, string until = null, StringComparison comparison = StringComparison.InvariantCulture)
        {
            var fromLength = (from ?? string.Empty).Length;
            var startIndex = !string.IsNullOrEmpty(from)
                ? str.IndexOf(from, comparison) + fromLength
                : 0;

            if (startIndex < fromLength) { throw new ArgumentException("from: Failed to find an instance of the first anchor"); }

            var endIndex = !string.IsNullOrEmpty(until)
            ? str.IndexOf(until, startIndex, comparison)
            : str.Length;

            if (endIndex < 0) { throw new ArgumentException("until: Failed to find an instance of the last anchor"); }

            var subString = str.Substring(startIndex, endIndex - startIndex);
            return subString;
        }

        /// <summary>
        /// find a certificate by filename and password
        /// </summary>
        /// <param name="certFileName"></param>
        /// <param name="certPass"></param>
        /// <returns></returns>
        private X509Certificate2 findCertificate(String certFileName, String certPass)
        {
            X509Certificate2 cert = new X509Certificate2(certFileName, certPass);
            return cert;
        }

        /// <summary>
        /// Find a Certificate by machine store location and subject
        /// </summary>
        /// <param name="loc"></param>
        /// <param name="subject"></param>
        /// <returns></returns>
        private X509Certificate2 findCertificate(System.Security.Cryptography.X509Certificates.StoreLocation loc, String subject)
        {

            X509Store store = new X509Store(loc);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2 rval = null;
            X509Certificate2Collection cers = store.Certificates.Find(X509FindType.FindBySubjectName, subject, false);
            if (cers.Count > 0)
            {
                rval = cers[0];
            };
            store.Close();
            return rval;
        }

        /// <summary>
        /// this method creates an MD5/RSA signature for the given String
        /// </summary>
        /// <param name="certFileName"> the file to load the certificate from</param>
        /// <param name="certPass">the password of the keystore</param>
        /// <param name="stringToSign">the String to sign</param>
        /// <returns>the signature in base64 format</returns>
        protected static String createSignature(X509Certificate2 cert, String stringToSign)
        {

            AsymmetricAlgorithm pkey = cert.PrivateKey;
            byte[] data = Encoding.UTF8.GetBytes(stringToSign);
            //----
            // Instantiate a RSA Algorithm object with Private Key
            RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)cert.PrivateKey;
            //----
            // Sign it
            // New MD5CryptoServiceProvider -> Instantiate the hash Algorithm to create the hash value.
            byte[] signature = rsa.SignData(data, new MD5CryptoServiceProvider());
            String Base64EncodededSignatureString = Convert.ToBase64String(signature, Base64FormattingOptions.None);
            return HttpUtility.UrlEncode(Base64EncodededSignatureString);
        }
        /// <summary>
        /// adds a aconso masterdata field
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void addMDField(String name, String value)
        {
            if (masterDataFields == null)
            {
                this.masterDataFields = new List<Field>();
            }
            Field f = new Field();
            f.id = name;
            f.Value = value;
            masterDataFields.Add(f);
        }

        /// <summary>
        /// Creates a Master-Data-Entry as new record under records
        /// <![CDATA[
        /// <aconso:Records xmlns:aconso="http://www.aconso.com/xml/ns/Records">
        ///     <aconso:Record id = "12345678" >
        ///     < aconso:MasterData>
        ///       <!-- ... -->
        ///]]>
        /// </summary>
        /// <param name="sysdmsakte"></param>
        public void addRecord(long sysdmsakte)
        {
            //add new record with id=sysdmsakte under record
            Cic.OpenOne.GateBANKNOW.Common.DTO.AconsoBatch.Record rec = new Cic.OpenOne.GateBANKNOW.Common.DTO.AconsoBatch.Record();
            rec.id = (int)sysdmsakte;

            List<Cic.OpenOne.GateBANKNOW.Common.DTO.AconsoBatch.Field> recordFields = new List<Cic.OpenOne.GateBANKNOW.Common.DTO.AconsoBatch.Field>();
            foreach(Field fsrc in this.masterDataFields)
            {
                Cic.OpenOne.GateBANKNOW.Common.DTO.AconsoBatch.Field f = new Cic.OpenOne.GateBANKNOW.Common.DTO.AconsoBatch.Field();
                f.id = fsrc.id;
                f.Value = fsrc.Value;
                recordFields.Add(f);
            }

            rec.MasterData = recordFields.ToArray();
            //record contains a masterdata with all fields of  masterDataFields;
            records.Add(rec);
            this.masterDataFields = null;//clear after adding
        }

        /// <summary>
        /// adds a aconso attribute field
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void addAttrField(String name, String value)
        {
            if (attribFields == null)
            {
                this.attribFields = new List<Field>();
            }
            Field f = new Field();
            f.id = name;
            f.Value = value;
            attribFields.Add(f);
        }
        /// <summary>
        /// adds a aconso component field
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void addComponentField(String name, String value)
        {
            if (componentFields == null)
            {
                this.componentFields = new List<Field>();
            }
            Field f = new Field();
            f.id = name;
            f.Value = value;
            componentFields.Add(f);
        }
        /// <summary>
        /// add a url parameter with ampersand suffix
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static String addParam(String name, String value)
        {
            return addParam(name, value, true);
        }
        /// <summary>
        /// add a url parameter, optionally with ampersand suffix
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        private static String addParam(String name, String value, bool suffix)
        {
            return name + "=" + value + (suffix ? "&" : "");
        }
    }

    #region HTTP FORM Management
    public static class FormUpload
    {
        private static readonly Encoding encoding = Encoding.UTF8;
        public static HttpWebResponse MultipartFormDataPost(string postUrl, string userAgent, Dictionary<string, object> postParameters, int timeout)
        {
            string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;

            byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);

            return PostForm(postUrl, userAgent, contentType, formData, timeout);
        }
        private static HttpWebResponse PostForm(string postUrl, string userAgent, string contentType, byte[] formData, int timeout)
        {
            HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;

            if (request == null)
            {
                throw new NullReferenceException("request is not a http request");
            }
            request.Timeout = timeout;
            // Set up the request properties.
            request.Method = "POST";
            request.ContentType = contentType;
            request.UserAgent = userAgent;
            request.CookieContainer = new CookieContainer();
            request.ContentLength = formData.Length;

            // You could add authentication here as well if needed:
            // request.PreAuthenticate = true;
            // request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;
            // request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes("username" + ":" + "password")));

            // Send the form data to the request.
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(formData, 0, formData.Length);
                requestStream.Close();
            }

            return request.GetResponse() as HttpWebResponse;
        }

        private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        {
            using (Stream formDataStream = new System.IO.MemoryStream())
            {
                bool needsCLRF = false;

                foreach (var param in postParameters)
                {
                    // Thanks to feedback from commenters, add a CRLF to allow multiple parameters to be added.
                    // Skip it on the first parameter, add it to subsequent parameters.
                    if (needsCLRF)
                    {
                        formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));
                    }

                    needsCLRF = true;

                    if (param.Value is FileParameter)
                    {
                        FileParameter fileToUpload = (FileParameter)param.Value;

                        // Add just the first part of this param, since we will write the file data directly to the Stream
                        string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
                            boundary,
                            param.Key,
                            fileToUpload.FileName ?? param.Key,
                            fileToUpload.ContentType ?? "application/octet-stream");

                        formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));

                        // Write the file data directly to the Stream, rather than serializing it to a string.
                        formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
                    }
                    else
                    {
                        string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                            boundary,
                            param.Key,
                            param.Value);
                        formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
                    }
                }

                // Add the end of the request.  Start with a newline
                string footer = "\r\n--" + boundary + "--\r\n";
                formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

                // Dump the Stream into a byte[]
                formDataStream.Position = 0;
                byte[] formData = new byte[formDataStream.Length];
                formDataStream.Read(formData, 0, formData.Length);
                formDataStream.Close();
                return formData;
            }
            
        }

        public class FileParameter
        {
            public byte[] File { get; set; }
            public string FileName { get; set; }
            public string ContentType { get; set; }
            public FileParameter(byte[] file) : this(file, null) { }
            public FileParameter(byte[] file, string filename) : this(file, filename, null) { }
            public FileParameter(byte[] file, string filename, string contenttype)
            {
                File = file;
                FileName = filename;
                ContentType = contenttype;
            }
        }
    }
    #endregion
}
