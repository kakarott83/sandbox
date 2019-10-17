using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Security;
using AutoMapper;

using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Util.Security;
using Cic.One.DTO;
using Cic.One.Web.BO;
using Cic.One.Web.BO.Search;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.BO.Search;

namespace Cic.One.Web.DAO.Mail
{
    public class EWSDBDao : IMailDBDao
    {
        private const string CnstBlowfishKey = "C.I.C.-S0ftwareGmbH1987Muenchen0";
        private static string QUERYACCOUNTEMAIL = "select sysperson from person where email=:email";

        IAppSettingsBo appSettingsBo;
        IEntityDao crmEntityDao;

        long sysWfuser;

        public long SysWfuser
        {
            get { return sysWfuser; }
            set { sysWfuser = value; }
        }

        public EWSDBDao(IEntityDao crmEntityDao, IAppSettingsBo appSettingsBo)
        {
            this.appSettingsBo = appSettingsBo;
            this.crmEntityDao = crmEntityDao;
        }

        /// <summary>
        /// Findet den SyncState eines bestimmten Benutzers raus
        /// </summary>
        /// <param name="userDataDto">Benutzer, von welchem der Status geladen werden soll</param>
        /// <returns>Gefundene Status</returns>
        public string GetSyncState(EWSUserDto userDataDto)
        {
            long wfuserid = userDataDto.SysWfuser;
            RegVarDto result = appSettingsBo.getAppSettingsItem(new igetAppSettingsItemsDto() { bezeichnung = RegVarPaths.getInstance().EXCHANGE+"SyncState", syswfuser = wfuserid });

            if (result != null)
            {
                return result.blobWert;
            }

            return "";
        }

        /// <summary>
        /// Setzt den Syncstate eines Benutzers
        /// </summary>
        /// <param name="userDataDto">Benutzer</param>
        /// <param name="syncState">neuer Status</param>
        public void SetSyncState(EWSUserDto userDataDto, string syncState)
        {
            long wfuserid = userDataDto.SysWfuser;
            appSettingsBo.createOrUpdateAppSettingsItemAsync(new icreateOrUpdateAppSettingsItemDto()
            {
                regVar = new RegVarDto()  
            {
                completePath = RegVarPaths.getInstance().EXCHANGE + "SyncState", 
                code = "SyncState", 
                blobWert = syncState,
                syswfuser = wfuserid,
                
            
            }, sysWfuser = wfuserid });

        }

        /// <summary>
        /// Speichert eine Liste von ItemChanges in der Datenbank ab
        /// </summary>
        /// <param name="list">Liste der veränderten Items</param>
        public void SaveChangedItems(List<MItemChangeDto> list)
        {
            //TODO
            //return;
            foreach (MItemChangeDto change in list)
            {
                switch (change.ChangeType)
                {
                    case MChangeTypeEnum.Create:
                        createOrUpdateItem(change.Item);
                        break;

                    case MChangeTypeEnum.Delete:
                        DeleteItem(change.Item);
                        break;

                    case MChangeTypeEnum.ReadFlagChange:
                        SetReadFlag(change.ItemId, change.IsRead);
                        break;

                    case MChangeTypeEnum.Update:
                        createOrUpdateItem(change.Item);
                        break;
                }
            }
        }

        /// <summary>
        /// Read Flag setzen
        /// </summary>
        /// <param name="id">Item-Id</param>
        /// <param name="value">Wert</param>
        public void SetReadFlag(string id, bool value)
        {
            using (DdOwExtended ctx = new DdOwExtended())
            {
                if (!string.IsNullOrEmpty(id))
                {
                    MAILMSG found = (from p in ctx.MAILMSG
                                     where p.ITEMID == id
                                     select p).FirstOrDefault();
                    if (found != null)
                    {
                        found.READFLAG = value ? 1 : 0;
                        ctx.SaveChanges();
                    }
                }
            }
        }

        /// <summary>
        /// Löscht ein Item aus der Datenbank, welches von Exchange gelöscht wurde
        /// (eventuell nicht löschen)
        /// </summary>
        /// <param name="item">Jeglicher Typ von Item</param>
        public void DeleteItem(MItemDto item)
        {
            //TODO, vielleicht Flag setzen, dass keine Updates für das Item mehr gemacht werden
            if (item is MContactDto)
            {
                DeleteItem(item as MContactDto);
            }
            else if (item is MAppointmentDto)
            {
                DeleteItem(item as MAppointmentDto);
            }
            else if (item is MTaskDto)
            {
                DeleteItem(item as MTaskDto);
            }
            else if (item is MEmailMessageDto)
            {
                DeleteItem(item as MEmailMessageDto);
            }
        }

        /// <summary>
        /// Löscht ein Item aus der Datenbank, welches von Exchange gelöscht wurde
        /// (eventuell nicht löschen)
        /// </summary>
        /// <param name="item">Contact</param>
        private void DeleteItem(MContactDto item)
        {
        }

        /// <summary>
        /// Löscht ein Item aus der Datenbank, welches von Exchange gelöscht wurde
        /// (eventuell nicht löschen)
        /// </summary>
        /// <param name="item">Appointment</param>
        private void DeleteItem(MAppointmentDto item)
        {
        }

        /// <summary>
        /// Löscht ein Item aus der Datenbank, welches von Exchange gelöscht wurde
        /// (eventuell nicht löschen)
        /// </summary>
        /// <param name="item">Task</param>
        private void DeleteItem(MTaskDto item)
        {
        }

        /// <summary>
        /// Löscht ein Item aus der Datenbank, welches von Exchange gelöscht wurde
        /// (eventuell nicht löschen)
        /// </summary>
        /// <param name="item">Mail</param>
        private void DeleteItem(MEmailMessageDto item)
        {
        }

        /// <summary>
        /// Erzeugt ein neues Item
        /// Oder Updatet ein Item, welches schon vorhanden ist
        /// </summary>
        /// <param name="item">Jeglicher Typ von Item</param>
        public void createOrUpdateItem(MItemDto item)
        {
            if (item is MContactDto)
            {
                createOrUpdateItem(item as MContactDto);
            }
            else if (item is MAppointmentDto)
            {
                createOrUpdateItem(item as MAppointmentDto);
            }
            else if (item is MTaskDto)
            {
                createOrUpdateItem(item as MTaskDto);
            }
            else if (item is MEmailMessageDto)
            {
                createOrUpdateItem(item as MEmailMessageDto);
            }
        }

        /// <summary>
        /// Erzeugt ein neues Item
        /// Oder Updatet ein Item, welches schon vorhanden ist
        /// </summary>
        /// <param name="item">Contact</param>
        public void createOrUpdateItem(MContactDto item)
        {
            //TODO
        }

        /// <summary>
        /// Erzeugt ein neues Item
        /// Oder Updatet ein Item, welches schon vorhanden ist
        /// </summary>
        /// <param name="item">Appointment</param>
        public void createOrUpdateItem(MAppointmentDto item)
        {
            using (DdOwExtended ctx = new DdOwExtended())
            {
                if (!string.IsNullOrEmpty(item.Id))
                {
                    APPTMT found = (from p in ctx.APPTMT
                                    where p.ITEMID == item.Id
                                    select p).FirstOrDefault();

                    bool addToTable = false;
                    if (found == null)
                    {
                        found = new APPTMT();
                        addToTable = true;
                    }

                    found = Mapper.Map(item, found);

                    createOrUpdateAttachments(addToTable, item.Attachments, found.FILEATTList);

                    if (addToTable)
                    {
                        ctx.AddToAPPTMT(found);
                       
                    }
                    ctx.SaveChanges();
                    SearchCache.entityChanged("APPTMT");
                    long perole = crmEntityDao.getSysPerole(sysWfuser);
                    if (perole != 0)
                        crmEntityDao.createOrUpdatePeuni("APPTMT", perole, found.SYSAPPTMT);
                }
            }
        }



        /// <summary>
        /// Erzeugt ein neues Item
        /// Oder Updatet ein Item, welches schon vorhanden ist
        /// </summary>
        /// <param name="item">Task</param>
        public void createOrUpdateItem(MTaskDto item)
        {
            using (DdOwExtended ctx = new DdOwExtended())
            {
                if (!string.IsNullOrEmpty(item.Id))
                {
                    PTASK found = (from p in ctx.PTASK
                                   where p.ITEMID == item.Id
                                   select p).FirstOrDefault();

                    bool addToTable = false;
                    if (found == null)
                    {
                        found = new PTASK();
                        addToTable = true;
                    }

                    found = Mapper.Map(item, found);

                    createOrUpdateAttachments(addToTable, item.Attachments, found.FILEATTList);

                    if (addToTable)
                    {
                        ctx.AddToPTASK(found);
                        
                    }
                    ctx.SaveChanges();
                    SearchCache.entityChanged("PTASK");
                    long perole = crmEntityDao.getSysPerole(sysWfuser);
                    if (perole != 0)
                        crmEntityDao.createOrUpdatePeuni("PTASK", perole, found.SYSPTASK);
                }
            }
        }

        /// <summary>
        /// Erzeugt ein neues Item
        /// Wird aufgerufen wenn eine neue Mail im Exchange angekommen ist und aktualisiert CRM
        /// Oder Updatet ein Item, welches schon vorhanden ist
        /// </summary>
        /// <param name="item">Mail</param>
        /// <returns></returns>
        public MailmsgDto createOrUpdateItem(MEmailMessageDto item)
        {
            MailmsgDto rval = null;
            bool addToTable = false;
            using (DdOwExtended ctx = new DdOwExtended())
            {
                if (!string.IsNullOrEmpty(item.Id))
                {

                    MAILMSG found = (from p in ctx.MAILMSG
                                     where p.ITEMID == item.Id
                                     select p).FirstOrDefault();

                    if (found == null)//neue eingehende Mail
                    {
                        found = new MAILMSG();
                        addToTable = true;
                    }
                    found = Mapper.Map(item, found);
                    found.PRIORITY = 0;//set default priority for incoming mails
                    if(item.Importance.HasValue)
                    {
                        if (MImportanceEnum.Normal == item.Importance.Value)
                            found.PRIORITY = 1;
                        if (MImportanceEnum.High == item.Importance.Value)
                            found.PRIORITY = 2;
                    }
                    
                        
                    if (item.From != null)
                    {
                        List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "email", Value = item.From.Address });
                        List<long> accounts = ctx.ExecuteStoreQuery<long>(QUERYACCOUNTEMAIL, pars.ToArray()).ToList();

                        if (accounts != null && accounts.Count == 1)//auto-link to account by from-email
                        {
                            found.SYSPERSON = accounts[0];
                        }
                    }
                    createOrUpdateAttachments(addToTable, item.Attachments, found.FILEATTList);

                    if (addToTable)
                    {
                        //create new contact for new incoming mail
                        ContactDto contact = new ContactDto();
                        if (found.SYSPERSON.HasValue)
                            contact.sysPerson = found.SYSPERSON.Value;
                        if (found.SYSOWNER.HasValue)
                            contact.sysOwner = found.SYSOWNER.Value;

                        contact.way = 3;//EMail
                        contact.sysContactTp = 1;
                        contact.reason = "Mailing";
                        contact.reasonCode = "3";
                        contact.comDate = DateTime.Now;
                        contact.direction = 1;//Eingehend
                        contact = crmEntityDao.createOrUpdateContact(contact);
                        found.SYSCONTACT = contact.sysContact;
                        ctx.AddToMAILMSG(found);
                        
                    }
                    ctx.SaveChanges();

                    long perole = crmEntityDao.getSysPerole(sysWfuser);
                    if (perole != 0)
                        crmEntityDao.createOrUpdatePeuni("MAILMSG", perole, found.SYSMAILMSG);
                    SearchCache.entityChanged("MAILMSG");
                    rval = Mapper.Map<MAILMSG, MailmsgDto>(found);
                    rval.isNew = addToTable;//remember for the bo if this is a completely new email
                }
            }

            return rval;
        }

        private void createOrUpdateAttachments(bool newEntity, List<MFileAttachement> attachments, System.Data.Objects.DataClasses.EntityCollection<FILEATT> collection)
        {
            if (!newEntity && !collection.IsLoaded)
                collection.Load();

            foreach (var attachment in attachments)
            {
                FILEATT old = collection.FirstOrDefault((a) => a.ATTID == attachment.Id);
                if (old == null)
                {
                    if (attachment is MFileAttachement)
                        collection.Add(Mapper.Map(attachment as MFileAttachement, new FILEATT()));
                    else
                        collection.Add(Mapper.Map(attachment, new FILEATT()));
                }
                else
                {
                    if (attachment is MFileAttachement)
                        Mapper.Map(attachment as MFileAttachement, old);
                    else
                        Mapper.Map(attachment, old);
                }
            }
        }

        /// <summary>
        /// Liefert das Appointment Objekt zurück, welches an Exchange geliefert wird.
        /// </summary>
        /// <param name="sysApptmt"></param>
        /// <returns></returns>
        public MAppointmentDto getMAppointmentDetails(long sysApptmt)
        {
            using (DdOwExtended ctx = new DdOwExtended())
            {
                if (sysApptmt != 0)
                {
                    APPTMT found = (from p in ctx.APPTMT
                                    where p.SYSAPPTMT == sysApptmt
                                    select p).FirstOrDefault();

                    if (found == null)
                        throw new Exception("Entity Id not found");

                    MAppointmentDto result = Mapper.Map(found, new MAppointmentDto());
                    result.Attachments = getAttachments(found.FILEATTList);
                    result.Recurrence = getRecurrence(found.RECURRList);
                    result.Categories = getCategories(ctx, "APPTMT", sysApptmt);

                    //TODO
                    return result;
                }
            }
            return null;
        }

        /// <summary>
        /// Liefert das Task Objekt zurück, welches an Exchange geliefert wird.
        /// </summary>
        /// <param name="sysPtask"></param>
        /// <returns></returns>
        public MTaskDto getMTaskDetails(long sysPtask)
        {
            using (DdOwExtended ctx = new DdOwExtended())
            {
                if (sysPtask != 0)
                {
                    PTASK found = (from p in ctx.PTASK
                                   where p.SYSPTASK == sysPtask
                                   select p).FirstOrDefault();

                    if (found == null)
                        throw new Exception("Entity Id not found");

                    MTaskDto result = Mapper.Map(found, new MTaskDto());
                    result.Attachments = getAttachments(found.FILEATTList);
                    result.Recurrence = getRecurrence(found.RECURRList);
                    result.Categories = getCategories(ctx, "PTASK", sysPtask);
                    if (found.COMPLETEFLAG == 1)
                        result.Status = MTaskStatusEnum.Completed;
                    //TODO
                    return result;
                }
            }
            return null;
        }

        /// <summary>
        /// Liefert das EmailMessage Objekt zurück, welches an Exchange geliefert wird.
        /// </summary>
        /// <param name="sysMailmsg"></param>
        /// <returns></returns>
        public MEmailMessageDto getMEmailMessage(long sysMailmsg)
        {
            using (DdOwExtended ctx = new DdOwExtended())
            {
                if (sysMailmsg != 0)
                {
                    MAILMSG found = (from p in ctx.MAILMSG
                                     where p.SYSMAILMSG == sysMailmsg
                                     select p).FirstOrDefault();

                    if (found == null)
                        throw new Exception("Entity Id not found");

                    MEmailMessageDto result = Mapper.Map(found, new MEmailMessageDto());

                    result.Attachments = getAttachments(found.FILEATTList);
                    result.Categories = getCategories(ctx, "MAILMSG", sysMailmsg);

                    //TODO
                    return result;
                }
            }
            return null;
        }

        private List<MFileAttachement> getAttachments(EntityCollection<FILEATT> entityCollection)
        {
            if (!entityCollection.IsLoaded)
            {
                entityCollection.Load();
            }
            return Mapper.Map(entityCollection.Where((a)=>a.ACTIVEFLAG.HasValue && a.ACTIVEFLAG.Value == 1), new List<MFileAttachement>());
        }

        private MRecurrence getRecurrence(EntityCollection<RECURR> entityCollection)
        {
            if (!entityCollection.IsLoaded)
                entityCollection.Load();

            if (entityCollection.Count > 0)
            {
                return Mapper.Map(entityCollection.FirstOrDefault(), new MRecurrence());
            }
            return null;
        }

        private List<string> getCategories(DdOwExtended ctx, string area, long sysid)
        {
            var found = (from p in ctx.ITEMCATM
                         where p.SYSID == sysid && p.AREA == area
                         select p);

            if (found == null)
                throw new Exception("Entity not found");

            List<string> categories = new List<string>();
            if (found != null)
            {
                foreach (ITEMCATM item in found)
                {
                    if (!item.ITEMCATReference.IsLoaded)
                        item.ITEMCATReference.Load();

                    if (item.ITEMCAT != null)
                        categories.Add(item.ITEMCAT.DESIGNCODE);
                }
            }
            return categories;
        }

        /// <summary>
        /// Updatet die ExchangeId von einem Datenbank Objekt (Mailmsg)
        /// </summary>
        /// <param name="sysMailmsg">Id</param>
        /// <param name="exchangeId">Neue ExchangeId, welche gespeichert werden soll</param>
        public void UpdateMailmsgExchangeId(long sysMailmsg, string exchangeId)
        {
            using (DdOwExtended ctx = new DdOwExtended())
            {
                if (sysMailmsg != 0)
                {
                    MAILMSG found = (from p in ctx.MAILMSG
                                     where p.SYSMAILMSG == sysMailmsg
                                     select p).FirstOrDefault();
                    if (found != null)
                    {
                        found.ITEMID = exchangeId;
                        ctx.SaveChanges();
                    }
                    else throw new Exception("Mailmsg with id " + sysMailmsg + " not found!");
                }
            }
        }

        /// <summary>
        /// Updatet die ExchangeId von einem Datenbank Objekt (Apptmt)
        /// </summary>
        /// <param name="sysApptmt">Id</param>
        /// <param name="exchangeId">Neue ExchangeId, welche gespeichert werden soll</param>
        public void UpdateApptmtExchangeId(long sysApptmt, string exchangeId)
        {
            using (DdOwExtended ctx = new DdOwExtended())
            {
                if (sysApptmt != 0)
                {
                    APPTMT found = (from p in ctx.APPTMT
                                    where p.SYSAPPTMT == sysApptmt
                                    select p).FirstOrDefault();

                    if (found == null)
                        throw new Exception("Entity Id not found");

                    if (found != null)
                    {
                        found.ITEMID = exchangeId;
                        ctx.SaveChanges();
                    }
                    else throw new Exception("Apptmt with id " + sysApptmt + " not found!");
                }
            }
        }

        /// <summary>
        /// Updatet die ExchangeId von einem Datenbank Objekt (PTask)
        /// </summary>
        /// <param name="sysPtask">Id</param>
        /// <param name="exchangeId">Neue ExchangeId, welche gespeichert werden soll</param>
        public void UpdatePtaskExchangeId(long sysPtask, string exchangeId)
        {
            using (DdOwExtended ctx = new DdOwExtended())
            {
                if (sysPtask != 0)
                {
                    PTASK found = (from p in ctx.PTASK
                                   where p.SYSPTASK == sysPtask
                                   select p).FirstOrDefault();
                    if (found != null)
                    {
                        found.ITEMID = exchangeId;
                        ctx.SaveChanges();
                    }
                    else throw new Exception("Ptask with id " + sysPtask + " not found!");
                }
            }
        }

        /// <summary>
        /// Liefert den Setting-String aus der Webconfig zurück
        /// DEPRECATED. Add proper config-settings in Common project and use 
        /// Cic.OpenOne.Common.Properties.Config.Default.Name-of-the-Variable
        /// 
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static string GetFromWebconfig(string Name)
        {

            var config = System.Configuration.ConfigurationManager.GetSection("applicationSettings/Cic.OpenOne.Common.Properties.Config");

            if (config != null)
            {
                if(((System.Configuration.ClientSettingsSection)config).Settings.Get(Name)!=null)
                    return ((System.Configuration.ClientSettingsSection)config).Settings.Get(Name).Value.ValueXml.InnerText;
            }

            return string.Empty;
        }

        /// <summary>
        /// Gibt den Benutzer zurück, welcher verwendet werden soll
        /// </summary>
        /// <param name="syswfuser">Id von dem User</param>
        /// <returns></returns>
        public EWSUserDto GetUser(long syswfuser)
        {
            String cfg = GetFromWebconfig("ExchangeEnabled");
            bool exchangeEnabled = (cfg != string.Empty);
            if(cfg!=string.Empty)
                exchangeEnabled = Convert.ToBoolean(cfg);

            if (!exchangeEnabled)
                return null;

            //id = 391; //SCHÖDLBAUER
            if (syswfuser == 0)
                //syswfuser = 391; //SCHÖDLBAUER
                syswfuser = 780; //PMAGER

            using (DdOwExtended ctx = new DdOwExtended())
            {
                WFUSER found = (from p in ctx.WFUSER
                                where p.SYSWFUSER == syswfuser
                                select p).FirstOrDefault();

                
                if (found == null)
                    throw new Exception("No valid user found");

                bool useServiceAccount = Convert.ToBoolean(GetFromWebconfig("ExchangeUseServiceAccount"));
                string AutodiscoverURL = GetFromWebconfig("ExchangeAutodiscoverURL");

                if (useServiceAccount)
                {
                    string Account = GetFromWebconfig("ExchangeServiceAccountName");
                    string Password = GetFromWebconfig("ExchangeServiceAccountPassword");
                    bool plainPassword = Convert.ToBoolean(GetFromWebconfig("ExchangeServiceAccountPlainPassword"));

                    Blowfish Blowfish = new Blowfish(CnstBlowfishKey);
                    return new EWSUserDto()
                    {
                        AutodiscoverUrl = string.IsNullOrEmpty(AutodiscoverURL) ? null : new Uri(AutodiscoverURL),

                        EmailAddress = Account,
                        ImpersonatedUser = found.EXTMAILADDRESS,
                        Password = ToSecureString(plainPassword ? Password : Blowfish.Decode(Password)),
                        Username = Account,
                        SysWfuser = found.SYSWFUSER
                    };
                }
                else
                {

                    Blowfish Blowfish = new Blowfish(CnstBlowfishKey);

                    string password = found.EXTMAILPASSWORD;

                    if (string.IsNullOrEmpty(found.EXTMAILACCOUNT))
                    {
                        //Kein User -> Exchange ausgeschaltet
                        return null;
                    }
                    if (string.IsNullOrEmpty(password))
                    {
                        //Kein User -> Exchange ausgeschaltet
                        return null;
                    }

                    return new EWSUserDto()
                    {
                        AutodiscoverUrl = string.IsNullOrEmpty(AutodiscoverURL) ? null : new Uri(AutodiscoverURL),
                        EmailAddress = found.EXTMAILADDRESS,
                        ImpersonatedUser = found.EXTMAILADDRESS,
                        Password = ToSecureString(Blowfish.Decode(password)),
                        Username = found.EXTMAILACCOUNT,
                        SysWfuser = found.SYSWFUSER
                    };
                }
            }

        }

        /// <summary>
        /// Erzeugt aus einem String einen SecureString (zur Sicherheit)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static System.Security.SecureString ToSecureString(string str)
        {
            SecureString password = new SecureString();
            foreach (char c in str.ToCharArray())
            {
                password.AppendChar(c);
            }
            return password;
        }
    }
}