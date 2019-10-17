using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Dynamic;
using System.Xml.Linq;
using System.IO;
using Cic.One.DTO;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.One.Web.DAO.Mail;

namespace Cic.One.Web.DAO
{
    /// <summary>
    /// Implementierung der DocumentSearch anhand des ITA Servers
    /// </summary>
    public class ITADocumentSearchDao : IDocumentSearchDao
    {

     
        bool settingsLoaded = false;

        string address;

        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Host und Port des Dokumentenservers
        /// </summary>
        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        WebClient webClient;
        /// <summary>
        /// Webclient werlcher verwendet werden soll
        /// Die Cookies sollen dabei bestehen bleiben.
        /// </summary>
        public WebClient WebClient
        {
            get
            {
                if (webClient == null)
                    webClient = new WebClient();
                return webClient;
            }
            set { webClient = value; }
        }

        string userProfile, user, password, role, unit, system;

        /// <summary>
        /// System, an der sich der ITA-Benuter anmeldet.
        /// </summary>
        public string System
        {
            get { return system; }
            set { system = value; }
        }

        /// <summary>
        /// Einheit des ITA-Benutzers
        /// </summary>
        public string Unit
        {
            get { return unit; }
            set { unit = value; }
        }

        /// <summary>
        /// Rolle des ITA-Benutzers
        /// </summary>
        public string Role
        {
            get { return role; }
            set { role = value; }
        }

        /// <summary>
        /// Passwort des ITA-Benutzers
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        /// <summary>
        /// ITA-Benutzer
        /// </summary>
        public string User
        {
            get { return user; }
            set { user = value; }
        }

        /// <summary>
        /// Name des Benutzerprofils
        /// </summary>
        public string UserProfile
        {
            get { return userProfile; }
            set { userProfile = value; }
        }


        /// <summary>
        /// Meldet sich am Server an
        /// </summary>
        /// <returns>true falls erfolgreich</returns>
        public bool Login()
        {
            if (!settingsLoaded)
            {
                LoadSettings();
            }
            return Login(userProfile, user, password, role, unit, system);
        }

        /// <summary>
        /// Meldet sich am Server an
        /// </summary>
        /// <param name="ProfileName">Profilname, welcher verwendet werden soll</param>
        /// <returns>true falls erfolgreich</returns>
        public bool Login(string ProfileName)
        {
            if (string.IsNullOrEmpty(ProfileName))
                return Login();

            if (!settingsLoaded)
            {
                LoadSettings();
            }

            return Login(ProfileName, user, password, role, unit, system);
        }

        bool useTestData = false;
        private void LoadSettings()
        {
            if (settingsLoaded)
                return;

            settingsLoaded = true;

            UserProfile = EWSDBDao.GetFromWebconfig("ITADocumentSearchUserProfile");
            User = EWSDBDao.GetFromWebconfig("ITADocumentSearchUser");
            Password = EWSDBDao.GetFromWebconfig("ITADocumentSearchPassword");
            Address = EWSDBDao.GetFromWebconfig("ITADocumentSearchAddress");
            Role = EWSDBDao.GetFromWebconfig("ITADocumentSearchRole");
            Unit = EWSDBDao.GetFromWebconfig("ITADocumentSearchUnit");
            System = EWSDBDao.GetFromWebconfig("ITADocumentSearchSystem");
            Dbas = EWSDBDao.GetFromWebconfig("ITADocumentSearchDbas");

            useTestData = Boolean.Parse(EWSDBDao.GetFromWebconfig("ITADocumentSearchUseTestdata"));
        }

        /// <summary>
        /// Meldet sich am Server ab
        /// </summary>
        /// <returns>true falls erfolgreich</returns>
        public bool Logout()
        {
            //TODO
            return true;

            /*if (useTestData)
                return true;

            string requestUri = "http://" + Address + "/itawebsearch/Logout.jsp";
            _log.Debug("SER-Request-URL (Logout): " + requestUri);
            string result = WebClient.DownloadString(requestUri);
            //TODO result auswerten
            return true;*/
        }


        /// <summary>
        /// Lädt ein Dokument anhand dem Input
        /// </summary>
        /// <param name="input"></param>
        /// <returns>das Dokument als byte[]</returns>
        public byte[] DocumentLoad(iDocumentLoadDto input)
        {
            if (useTestData)
                return null;

            string requestUri = "http://" + Address + "/itawebsearch/Show.jsp";

            bool first = true;
            requestUri += ParameterToString("docid", input.Docid, ref first);
            requestUri += ParameterToString("ext", input.Ext, ref first);
            if (string.IsNullOrEmpty(input.ProfileName))
                input.ProfileName = UserProfile;
            requestUri += ParameterToString("profile_name", input.ProfileName, ref first);

            _log.Debug("SER-Request-URL (DocumentLoad): " + requestUri);

            return WebClient.DownloadData(requestUri);
        }


        /// <summary>
        /// Liefert die Version der ITA WebSearch zurück
        /// </summary>
        /// <param name="info"></param>
        /// <returns>Information</returns>
        public ogetVersionInfo getVersionInfo(igetVersionInfo input)
        {
            string requestUri = "http://" + Address + "/itawebsearch/VersionInfo.jsp";

            string result = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<VERSION_INFO>
<VERSION>ITA WebSearch V01.01.0004</VERSION>
<ITA_FRAMEWARE>Blueline 5.0.0 / BL@HH 5.0.0</ITA_FRAMEWARE>
<WFL_FRAMEWARE>bpmline 5.0.0</WFL_FRAMEWARE>
<COPYRIGHT>(c) SER Solutions Deutschland GmbH 2007</COPYRIGHT>
</VERSION_INFO>";

            if (!useTestData)
            {
                _log.Debug("SER-Request-URL (getVersionInfo): " + requestUri);
                result = WebClient.DownloadString(requestUri);
            }


            return (from version in XDocument.Load(new StringReader(result)).Elements("VERSION_INFO")
                    select new ogetVersionInfo()
                    {
                        Version = ElementValueNull(version.Element("VERSION")),
                        ITA_Frameware = ElementValueNull(version.Element("ITA_FRAMEWARE")),
                        WFL_Frameware = ElementValueNull(version.Element("WFL_FRAMEWARE")),
                        Copyright = ElementValueNull(version.Element("COPYRIGHT")),
                    }).FirstOrDefault();
        }

        /// <summary>
        /// Sucht nach Dokumenten
        /// </summary>
        /// <param name="input">Parameter</param>
        /// <returns>Liste von Infos der gefundenen Elementen</returns>
        public HitlistDto DynamicDocumentSearch(iDynamicDocumentSearchDto input)
        {

            string requestUri = "http://" + Address + "/itawebsearch/DynamicSearch.jsp";

            bool first = true;

            if (string.IsNullOrEmpty(input.Dbas))
                input.Dbas = Dbas;
            if (string.IsNullOrEmpty(input.ProfileName))
                input.ProfileName = UserProfile;

            requestUri += ParameterToString("dbas", input.Dbas, ref first);
            requestUri += ParameterToString("from", FromDateTime(input.From), ref first);
            requestUri += ParameterToString("to", FromDateTime(input.To), ref first);

            if (input.Descriptors != null)
                foreach (var desc in input.Descriptors)
                {
                    requestUri += ParameterToString(desc.DescId, desc.Value, ref first);
                }


            requestUri += ParameterToString("limit", input.Limit, ref first);
            requestUri += ParameterToString("size", input.Size, ref first);
            requestUri += ParameterToString("ext", input.Ext, ref first);
            requestUri += ParameterToString("profile_name", input.ProfileName, ref first);
            requestUri += ParameterToString("orderby", input.OrderBy, ref first);

            string result = @"<?xml version=""1.0"" encoding=""UTF-8""?>
                                <HITLIST>
                                    <TOTAL_COUNT>2</TOTAL_COUNT>
                                    <SIZE>2</SIZE>
                                    <DOCUMENT>
                                        <DOCID>005EVITA031CKVVA7NMK5CKD2KB47LEK000EK00000008200701160081026:005</DOCID>
                                        <DOCDATE>16.01.2007</DOCDATE>
                                        <COUNT_DESCRIPTORS>2</COUNT_DESCRIPTORS>
                                        <DESCRIPTORS>
                                            <DESCRIPTOR ID=""1029"" NAME=""Kuerzel"">
                                                <COUNT_VALUES>1</COUNT_VALUES>
                                                <VALUE ID=""0"">pdf</VALUE>
                                            </DESCRIPTOR>
                                            <DESCRIPTOR ID=""1027"" NAME=""KundenID"">
                                                <COUNT_VALUES>1</COUNT_VALUES>
                                                <VALUE ID=""0"">0000000001</VALUE>
                                            </DESCRIPTOR>
                                        </DESCRIPTORS>
                                    </DOCUMENT>
                                    <DOCUMENT>
                                        <DOCID>005EVITA031CKVVA7NMK5CKD2KB47LEK000EK00000008200701160081026:005</DOCID>
                                        <DOCDATE>16.01.2008</DOCDATE>
                                        <COUNT_DESCRIPTORS>2</COUNT_DESCRIPTORS>
                                        <DESCRIPTORS>
                                            <DESCRIPTOR ID=""1029"" NAME=""Kuerzel"">
                                                <COUNT_VALUES>1</COUNT_VALUES>
                                                <VALUE ID=""0"">pdf</VALUE>
                                            </DESCRIPTOR>
                                            <DESCRIPTOR ID=""1027"" NAME=""KundenID"">
                                                <COUNT_VALUES>1</COUNT_VALUES>
                                                <VALUE ID=""0"">0000000001</VALUE>
                                            </DESCRIPTOR>
                                        </DESCRIPTORS>
                                    </DOCUMENT>
                                </HITLIST>";

//            string result = @"<?xml version=""1.0"" encoding=""UTF-8""?>
//<HITLIST>
//<SIZE>-1</SIZE>
//<ERROR>Fehlermeldung</ERROR>
//</HITLIST>";

            if (!useTestData)
            {
                _log.Debug("SER-Request-URL (DynamicDocumentSearch): " + requestUri);
                result = WebClient.DownloadString(requestUri);
            }

            HitlistDto hitlist = (from cust in XDocument.Load(new StringReader(result)).Elements("HITLIST")
                    select new HitlistDto()
                    {
                        Error = ElementValueNull(cust.Element("ERROR")),
                        TotalCount = ToInt32(ElementValueNull(cust.Element("TOTAL_COUNT"))),
                        Size = ToInt32(ElementValueNull(cust.Element("TOTAL_COUNT"))),
                        Documents = (from doc in cust.Descendants("DOCUMENT")
                                     select new DocumentDto()
                                     {
                                         DocId = ElementValueNull(doc.Element("DOCID")),
                                         DocDate = ToDateTime(ElementValueNull(doc.Element("DOCDATE"))),
                                         CountDescriptors = ToInt32(ElementValueNull(doc.Element("COUNT_DESCRIPTORS"))),
                                         Descriptors = (from desc in doc.Descendants("DESCRIPTOR")
                                                        select new DescriptorDto()
                                                        {
                                                            ID = AttributeValueNull(desc, "ID"),
                                                            Name = AttributeValueNull(desc, "NAME"),
                                                            CountValues = ToInt32(ElementValueNull(desc.Element("COUNT_VALUES"))),
                                                            Values = (from val in desc.Descendants("VALUE")
                                                                     select new DescriptorValueDto()
                                                                     {
                                                                         ID = AttributeValueNull(val, "ID"),
                                                                         Value = ElementValueNull(val)
                                                                     }).ToList()
                                                        }).ToList()
                                     }).ToList()
                    }).FirstOrDefault();

            if (!string.IsNullOrEmpty(hitlist.Error))
                throw new Exception(hitlist.Error);

            return hitlist;
        }


        /// <summary>
        /// Wandelt einen String zu Datetime um. falls er leer ist, wird null geliefert
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private DateTime? ToDateTime(string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;
            else
                return Convert.ToDateTime(str);
        }

        /// <summary>
        /// Wandelt einen String zu Int32 um. Falls er leer ist, wird -1 zurück geliefert
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private int ToInt32(string str)
        {
            if (string.IsNullOrEmpty(str))
                return -1;
            else
                return Convert.ToInt32(str);
        }

        /// <summary>
        /// Lädt ein Attribut. Falls es nicht existiert wird null zurück gelifert
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static string AttributeValueNull(XElement element, string attributeName)
        {
            if (element == null)
                return null;
            else
            {
                XAttribute attr = element.Attribute(attributeName);
                return attr == null ? null : attr.Value;
            }
        }
        /// <summary>
        /// Lädt den Wert eines Elements. Falls das Element nicht existiert wird null zurück geliefert
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string ElementValueNull( XElement element)
        {
            if (element != null)
                return element.Value;

            return null;
        }

        /// <summary>
        /// Konvertiert ein DateTime zu dem gewünschten Format der Abfrage
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private string FromDateTime(DateTime? date)
        {
            if (date == null)
                return null;

            return string.Format("{0:yyyyMMdd}", date);
        }

        /// <summary>
        /// Meldet sich am ITA Dokumentenserver an
        /// </summary>
        /// <param name="userProfile">(Optional) Name des Benutzerprofils. Wird versucht zuerst zu nehmen.</param>
        /// <param name="user">(Optional) ITA-Benutzer</param>
        /// <param name="password">(Optional) Passwort des ITA-Benutzers</param>
        /// <param name="role">(Optional) Rolle des ITA-Benutzers</param>
        /// <param name="unit">(Optional) Einheit des ITA-Benutzers</param>
        /// <param name="system">(Optional) System, an der sich der ITA-Benutzer anmeldet</param>
        /// <returns>True, falls erfolgreich</returns>
        public bool Login(string userProfile, string user, string password, string role, string unit, string system)
        {
            //TODO
            return true;

            /*string requestUri = "http://" + Address + "/itawebsearch/Login.jsp";
            if (!string.IsNullOrEmpty(userProfile))
            {
                bool first = true;
                requestUri += ParameterToString("user_profile", userProfile, ref first);
            }
            else
            {
                bool first = true;
                requestUri += ParameterToString("system", system, ref first);
                requestUri += ParameterToString("user", user, ref first);
                requestUri += ParameterToString("password", password, ref first);
                requestUri += ParameterToString("role", role, ref first);
                requestUri += ParameterToString("unit", unit, ref first);
            }

            string result = "";
            if (!useTestData)
            {
                _log.Debug("SER-Request-URL (Login): " + requestUri);
                result = WebClient.DownloadString(requestUri);
            }

            if(result.Contains("<ERROR>"))
            {
                string error = (from cust in XDocument.Load(new StringReader(result)).Elements("LOGIN")
                                select ElementValueNull(cust.Element("ERROR"))).FirstOrDefault();

                throw new Exception("Login-Exception. "+error);
            }

            return true;*/
        }


        /// <summary>
        /// Wandelt einen Parameter um, dass er an den RequestString angehängt werden kann.
        /// </summary>
        /// <param name="parameterName">Parameter Name</param>
        /// <param name="value">Wert des Parameters</param>
        /// <param name="first">Ob es der erste Parameter ist</param>
        /// <returns></returns>
        private string ParameterToString(string parameterName, string value, ref bool first)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(parameterName))
                return "";
            else
            {
                if (first)
                {
                    first = false;
                    return "?" + HttpUtility.HtmlEncode(parameterName) + "=" + HttpUtility.HtmlEncode(value);
                }
                else
                {
                    return "&" + HttpUtility.HtmlEncode(parameterName) + "=" + HttpUtility.HtmlEncode(value);
                }
            }
        }




        public string Dbas { get; set; }

    }
}