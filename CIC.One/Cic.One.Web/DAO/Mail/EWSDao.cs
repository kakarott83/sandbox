using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;

using Microsoft.Exchange.WebServices.Data;
using Cic.OpenOne.Common.Properties;
using Cic.One.DTO;

namespace Cic.One.Web.DAO.Mail
{
    public class EWSDao : IMailDao
    {
        private ITraceListenerDao traceDao;
        private IMailDBDao mailDBDao;

        public ExchangeService Service { get; set; }

        public EWSUserDto UserDataDto { get; set; }

        private bool triedToFindFolder = false;
        private FolderId cRMFolder;

        private const string sentButNoIdString = "Sent but no Id received";
        public static string SentButNoIdString
        {
            get { return sentButNoIdString; }
        } 


        public FolderId CRMFolder
        {
            get
            {
                if (cRMFolder == null && !triedToFindFolder)
                {
                    cRMFolder = FindFolderId(CRMFolderName);
                    triedToFindFolder = true;
                }
                return cRMFolder;
            }
            set { cRMFolder = value; }
        }

        public  string CRMFolderName = "CRM";
        public ExchangeVersion Version = ExchangeVersion.Exchange2007_SP1;

        public EWSDao(SubscriptionWrapper subscription, IMailDBDao mailDBDao, ITraceListenerDao traceDao = null)
        {
            this.UserDataDto = subscription.UserDataDto;
            this.traceDao = traceDao;
            this.mailDBDao = mailDBDao;

            LoadConfig();

            Service = subscription.Service;
            if (Service == null)
                Service = Connect();
            CRMFolder = subscription.CRMFolder;
        }

        protected string GetCustomSetting(string Section, string Setting)
        {
            var config = System.Configuration.ConfigurationManager.GetSection(Section);

            if (config != null)
                return ((System.Configuration.ClientSettingsSection)config).Settings.Get(Setting).Value.ValueXml.InnerText;

            return string.Empty;
        }

        /// <summary>       
        /// Lädt die Einstellungen aus der Web.config
        /// </summary>
        private void LoadConfig()
        {
            string version = GetCustomSetting(@"applicationSettings/Cic.OpenOne.Common.Properties.Config", "ExchangeVersion").Trim();
            CRMFolderName = GetCustomSetting(@"applicationSettings/Cic.OpenOne.Common.Properties.Config", "ExchangeSyncedFolderName");

            ExchangeVersion temp;
            if(Enum.TryParse(version, out temp))
            {
                Version = temp;
            }
        }

        /// <summary>
        /// Erstellt die Verbindung,
        /// sucht den CRM-Ordner
        /// lädt den Synchronisationsstatus,
        /// synchronisiert sich
        /// und subscribt sich zum Notification Stream
        /// </summary>
        /// <param name="userDataDto"></param>
        /// <param name="mailDBDao"></param>
        /// <param name="traceDao"></param>
        public EWSDao(EWSUserDto userDataDto, IMailDBDao mailDBDao, ITraceListenerDao traceDao = null)
        {
            LoadConfig();
            this.UserDataDto = userDataDto;
            this.traceDao = traceDao;
            this.mailDBDao = mailDBDao;

            if(userDataDto == null)
            {
                //Kein User -> Exchange ausgeschaltet
                return;
            }

            //CertificateCallback.Initialize();
            Service = Connect();
        }

        /// <summary>
        /// Sucht nach einem Ordnernamen und liefert den 1. gefundenen zurück
        /// </summary>
        /// <param name="foldername">Displayname, nach dem gesucht werden soll</param>
        /// <returns>1. gefundener Ordner</returns>
        public FolderId FindFolderId(string foldername)
        {
            SearchFilter filter = new SearchFilter.IsEqualTo(FolderSchema.DisplayName, foldername);
            var results = Service.FindFolders(WellKnownFolderName.Root, filter,
                new FolderView(1) { PropertySet = BasePropertySet.IdOnly, Traversal = FolderTraversal.Deep });

            if (results.Count() > 0)
            {
                return results.FirstOrDefault().Id;
            }
            return null;
        }

        /// <summary>
        /// Wählt den Ordner aus nach folgenden Kriterien:
        /// Wenn FolderId nicht leer ist, wird sie verwendet.
        /// Als nächstes wird versucht der WellKnownFolder zu verwenden.
        /// Falls returnAKFIfNone auf true ist, wird der AKF-Ordner nun zurückgeliefert
        /// ansonsten null
        /// </summary>
        /// <param name="folderid">1. Priorität</param>
        /// <param name="wellknown">2. Priorität</param>
        /// <param name="returnAKFIfNone">3. Priorität</param>
        /// <returns></returns>
        private FolderId GetFolderId(string folderid, MWellKnownFolderNameEnum? wellknown, bool returnAKFIfNone)
        {
            FolderId folder = null;
            if (!string.IsNullOrEmpty(folderid))
                folder = new FolderId(folderid);
            else if (wellknown.HasValue)
                folder = new FolderId((WellKnownFolderName)wellknown.Value);
            else if (returnAKFIfNone)
                folder = CRMFolder;
            return folder;
        }

        /// <summary>
        /// Connectet sich zu dem ExchangeService
        /// </summary>
        /// <returns></returns>
        public ExchangeService Connect()
        {
            DateTime start = DateTime.Now;

            ExchangeService service = new ExchangeService(Version);

            if (traceDao != null)
            {
                service.TraceListener = traceDao;
                service.TraceFlags = TraceFlags.All;
                service.TraceEnabled = true;
            }

            string username = UserDataDto.EmailAddress;

            if (!string.IsNullOrEmpty(UserDataDto.Username))
                username = UserDataDto.Username;

            if (!string.IsNullOrEmpty(UserDataDto.ImpersonatedUser) && UserDataDto.ImpersonatedUser != UserDataDto.EmailAddress)
                service.ImpersonatedUserId = new ImpersonatedUserId(ConnectingIdType.SmtpAddress, UserDataDto.ImpersonatedUser);

            service.UseDefaultCredentials = true;

            service.Credentials = new NetworkCredential(username, UserDataDto.Password);

            if (UserDataDto.AutodiscoverUrl == null)
            {
                service.AutodiscoverUrl(UserDataDto.EmailAddress, RedirectionUrlValidationCallback);
                UserDataDto.AutodiscoverUrl = service.Url;
            }
            else
            {
                service.Url = UserDataDto.AutodiscoverUrl;
            }

            double elapsedTimeMs = (DateTime.Now - start).TotalMilliseconds;

            return service;
        }

        // The following is a basic redirection validation callback method. It
        // inspects the redirection URL and only allows the Service object to
        // follow the redirection link if the URL is using HTTPS.

        // This redirection URL validation callback provides sufficient security
        // for development and testing of your application. However, it may not
        // provide sufficient security for your deployed application. You should
        // always make sure that the URL validation callback method that you use
        // meets the security requirements of your organization.
        private static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            // The default for the validation callback is to reject the URL.
            bool result = false;

            Uri redirectionUri = new Uri(redirectionUrl);

            // Validate the contents of the redirection URL. In this simple validation
            // callback, the redirection URL is considered valid if it is using HTTPS
            // to encrypt the authentication credentials.
            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }

            return result;
        }




        /// <summary>
        /// Sendet eine Mail anhand des Inputs
        /// </summary>
        /// <param name="input">Eingabeparameter</param>
        /// <returns>Output</returns>
        public osendMailDto SendMail(isendMailDto input)
        {
            if (input.Mail.SysOwner != 0 && input.Mail.SysOwner != UserDataDto.SysWfuser)
            {
                return MailDaoFactory.getInstance().getMailDao(input.Mail.SysOwner).SendMail(input);
            }

            EmailMessage message = new EmailMessage(Service);
            //message.Load(PropertySet.IdOnly);
            message = Mapper.Map(input.Mail, message);

            AddAttachments(input.Mail, message);

            FillEmailCollection(message.ToRecipients, input.Mail.ToRecipients);
            FillEmailCollection(message.CcRecipients, input.Mail.CcRecipients);
            FillEmailCollection(message.BccRecipients, input.Mail.BccRecipients);
            string id = null;

            if (input.UseStandardFolder)
            {
                message.Save();
                id = message.Id.ToString();
                message.SendAndSaveCopy();
            }
            else
            {
                FolderId folder = GetFolderId(input.FolderId, input.WellKnownFolderName, true);
                message.SendAndSaveCopy(folder);
            }

            if (message.Id == null)
            {
                if (id == null)
                    return new osendMailDto() { Id = sentButNoIdString };
                else
                    return new osendMailDto() { Id = id };
            }
            else
                return new osendMailDto() { Id = message.Id.ToString() };
        }

        /// <summary>
        /// Füllt die EmailAddresscollection mit einer Liste an Emails
        /// </summary>
        /// <param name="emailAddressCollection">Collection, welche gefüllt werden soll</param>
        /// <param name="list">Liste der Items, die angefügt werden</param>
        private void FillEmailCollection(EmailAddressCollection emailAddressCollection, List<MEmailAddressDto> list)
        {
            if (list != null)
                foreach (MEmailAddressDto ea in list)
                {
                    emailAddressCollection.Add(Mapper.Map<MEmailAddressDto, EmailAddress>(ea));
                }
        }

        /// <summary>
        /// Synchronisiert Mails ab einem speziellen Punkt.
        /// </summary>
        /// <param name="input">Eingabeparameter</param>
        /// <returns>Output</returns>
        public osyncItemsDto SyncItems(isyncItemsDto input)
        {
            FolderId folder = GetFolderId(input.FolderId, input.WellKnownFolderName, true);

            List<ItemId> ids = new List<ItemId>();

            if (input.IdsToIgnore != null)
                ids.AddRange(from item in input.IdsToIgnore
                             select new ItemId(item));

            var res = Service.SyncFolderItems(folder, PropertySet.IdOnly, ids, 512, SyncFolderItemsScope.NormalItems, input.SyncState);
            var fullres = res.ToList();
            while (res.MoreChangesAvailable)
            {
                res = Service.SyncFolderItems(folder, PropertySet.IdOnly, ids, 512, SyncFolderItemsScope.NormalItems, res.SyncState);
                fullres.AddRange(res);
            }
            var items = fullres.Select((a) => a.Item);
            EWSLoadDao.LoadProperties(items, Service);

            foreach (var item in items)
            {
                if (item != null)
                {
                    foreach (var attachment in item.Attachments)
                    {
                        attachment.Load();
                    }
                }
            }
            var mappedItems = Mapper.Map(fullres, new List<MItemChangeDto>());
            foreach (var item in mappedItems)
            {
                if(item.Item != null)
                    item.Item.SysOwner = UserDataDto.SysWfuser;
            }
            return new osyncItemsDto() { Changed = mappedItems, SyncState = res.SyncState };
        }

        /// <summary>
        /// Verschiebt ein Item
        /// </summary>
        /// <param name="input">Eingabeparameter</param>
        /// <returns>Output</returns>
        public omoveItem MoveItem(imoveItem input)
        {
            Item item = Item.Bind(Service, new ItemId(input.Id), new PropertySet(BasePropertySet.IdOnly));
            if (!string.IsNullOrEmpty(input.FolderId))
                item.Move(new FolderId(input.FolderId));
            else if (input.WellKnownFolderName.HasValue)
            {
                item.Move((WellKnownFolderName)input.WellKnownFolderName.Value);
            }
            else
                item.Move(CRMFolder);

            return new omoveItem();
        }

        /// <summary>
        /// Sucht nach Items in einem bestimmten Ordner mit optionalen Suchparametern
        /// </summary>
        /// <param name="input">Eingabeparameter</param>
        /// <returns>Output</returns>
        public ofindItemsDto FindItems(ifindItemsDto input)
        {
            FolderId folder = GetFolderId(input.FolderId, input.WellKnownFolderName, true);

            var l = new List<MItemDto>();

            ItemView view = new ItemView(input.PageSize, input.Offset);
            view.PropertySet = new PropertySet(BasePropertySet.IdOnly);

            FindItemsResults<Item> results = null;
            if (!string.IsNullOrEmpty(input.QueryString) && Version > 0)
            {
                results = Service.FindItems(folder, input.QueryString, input.HighlightTerms, view);
            }
            else if (input.Search != null)
            {
                results = Service.FindItems(folder, input.Search, view);
            }
            else
                results = Service.FindItems(folder, view);

            EWSLoadDao.LoadProperties(results, Service);

            l = Mapper.Map(results, l);

            foreach (MItemDto item in l)
            {
                if (item is MContactGroupDto)
                {
                    var group = item as MContactGroupDto;
                    ExpandGroupResults groupResults = Service.ExpandGroup(new ItemId(group.Id));
                    Mapper.Map(groupResults, group);
                }
            }
            return new ofindItemsDto() { Items = l };
        }

        /// <summary>
        /// Sucht nach einem string in der GAL und auf dem Mailserver
        /// </summary>
        /// <param name="input">Eingabeparameter</param>
        /// <returns>Output</returns>s
        public ofindContact FindContacts(ifindContact input)
        {
            var resolved = Service.ResolveName(input.NameToResolve, (ResolveNameSearchLocation)input.ResolveNameSearchLocation, input.ReturnContactDetails);
            var res = Mapper.Map(resolved, new List<MNameResolutionDto>());

            return new ofindContact() { Items = res };
        }

        /// <summary>
        /// Löscht ein Item auf dem MailServer
        /// </summary>
        /// <param name="input">Eingabeparameter</param>
        /// <returns>Output</returns>
        public odeleteItem DeleteItem(ideleteItem input)
        {
            if (input.SysOwner.HasValue && input.SysOwner != 0 && input.SysOwner != UserDataDto.SysWfuser)
            {
                return MailDaoFactory.getInstance().getMailDao(input.SysOwner.Value).DeleteItem(input);
            }

            Item item = Item.Bind(Service, new ItemId(input.Id), new PropertySet(BasePropertySet.IdOnly));

            if (item is Appointment)
            {
                var v = item as Appointment;
                v.Delete((DeleteMode)input.DeleteMode, (SendCancellationsMode)input.SendInvitationsOrCancellationsMode);
            }
            else
                item.Delete((DeleteMode)input.DeleteMode);

            return new odeleteItem();
        }

        /// <summary>
        /// Verändert ein Item auf dem MailServer
        /// </summary>
        /// <param name="input">Eingabeparameter</param>
        /// <returns>Output</returns>
        public ochangeItem ChangeItem(ichangeItem input)
        {

            if (input.Item.SysOwner != 0 && input.Item.SysOwner != UserDataDto.SysWfuser)
            {
                return MailDaoFactory.getInstance().getMailDao(input.Item.SysOwner).ChangeItem(input);
            }

            Item item = Item.Bind(Service, new ItemId(input.Item.Id), PropertySet.IdOnly);

            //EWSLoadDao.LoadProperties(new List<Item>() { item }, Service);

            if (item is Contact && input.Item is MContactDto)
            {
                Contact v = item as Contact;
                AddAttachments(input.Item, v, true);
                Mapper.Map(input.Item as MContactDto, v);
                v.Update((ConflictResolutionMode)input.ConflictResolution);
            }
            else if (item is Appointment && input.Item is MAppointmentDto)
            {
                Appointment v = item as Appointment;
                AddAttachments(input.Item, v, true);
                Mapper.Map(input.Item as MAppointmentDto, v);
                v.StartTimeZone = TimeZoneInfo.Local;
                v.Update((ConflictResolutionMode)input.ConflictResolution, (SendInvitationsOrCancellationsMode)input.SendInvitationsOrCancellationsMode);
            }
            else if (item is Task && input.Item is MTaskDto)
            {
                Task v = item as Task;
                AddAttachments(input.Item, v, true);
                Mapper.Map(input.Item as MTaskDto, v);
                v.Update((ConflictResolutionMode)input.ConflictResolution);
            }
            else if (item is EmailMessage && input.Item is MEmailMessageDto)
            {
                EmailMessage v = item as EmailMessage;
                Mapper.Map(input.Item as MEmailMessageDto, v);

                //AddAttachments(input.Item, v);

                v.Update((ConflictResolutionMode)input.ConflictResolution);
                if (input.SendMail && v.IsDraft)
                {
                    if (input.UseStandardFolder)
                        v.SendAndSaveCopy();
                    else
                    {
                        FolderId folder = GetFolderId(input.FolderId, input.WellKnownFolderName, true);
                        v.SendAndSaveCopy(folder);
                    }
                }
            }
            else
            {
                throw new ArgumentException("Type not supported or a missmatch: " + item.GetType() + " <-> " + input.Item.GetType());
            }

            return new ochangeItem();
        }

        /// <summary>
        /// Fügt alle Attachments von dem itemDto dem Exchange Item hinzu
        /// ACHTUNG: Attachments können nicht verändert werden.
        /// </summary>
        /// <param name="myItem">Item, von welchem die Attachments genommen werden sollen</param>
        /// <param name="item">Item, welches die Attachments bekommt</param>
        private void AddAttachments(MItemDto myItem, Item item, bool removeOldAttachments = false)
        {
            if (removeOldAttachments)
            {
                item.Load(new PropertySet(ItemSchema.Attachments));
                item.Attachments.Clear();
            }
            if (myItem.Attachments != null)
            {
                //item.Attachments.Clear();
                foreach (var attachment in myItem.Attachments)
                {
                    item.Attachments.AddFileAttachment(attachment.Name, attachment.Content);
                }
            }
        }

        /// <summary>
        /// Erzeugt ein Item auf dem MailServer
        /// </summary>
        /// <param name="input">Eingabeparameter</param>
        /// <returns>Output</returns>
        public ocreateItem CreateItem(icreateItem input)
        {

            if (input.Item.SysOwner != 0 && input.Item.SysOwner != UserDataDto.SysWfuser)
            {
                return MailDaoFactory.getInstance().getMailDao(input.Item.SysOwner).CreateItem(input);
            }
            FolderId folder = GetFolderId(input.FolderId, input.WellKnownFolderName, false);

            var result = new ocreateItem();

            //Je nachdem, was für ein Typ das Item ist,
            //Muss es anders gemapt werden und gespeichert.
            if (input.Item is MContactDto)
            {
                Contact item = new Contact(Service);
                item = Mapper.Map(input.Item as MContactDto, item);
                AddAttachments(input.Item, item);

                if (input.UseStandardFolder || folder == null)
                    item.Save();
                else
                    item.Save(folder);

                result.Id = item.Id.ToString();
            }
            else if (input.Item is MAppointmentDto)
            {
                Appointment item = new Appointment(Service);
                item = Mapper.Map(input.Item as MAppointmentDto, item);
                AddAttachments(input.Item, item);

                if (input.UseStandardFolder || folder == null)
                    item.Save((SendInvitationsMode)input.SendInvitationsMode);
                else
                    item.Save(folder, (SendInvitationsMode)input.SendInvitationsMode);

                result.Id = item.Id.ToString();
            }
            else if (input.Item is MTaskDto)
            {
                Task item = new Task(Service);

                item = Mapper.Map(input.Item as MTaskDto, item);
                AddAttachments(input.Item, item);

                if (input.UseStandardFolder || folder == null)
                    item.Save();
                else
                    item.Save(folder);

                result.Id = item.Id.ToString();
            }
            else if (input.Item is MEmailMessageDto)
            {
                var secInp = new isendMailDto() { Mail = input.Item as MEmailMessageDto, UseStandardFolder = input.UseStandardFolder, FolderId = input.FolderId, WellKnownFolderName = input.WellKnownFolderName };
                var res = SendMail(secInp);
                result.Id = res.Id;
            }

            return result;
        }


        public EWSUserDto getUser()
        {
            return UserDataDto;
        }
    }
}